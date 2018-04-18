namespace WebCruiserWVS
{
    using Microsoft.JScript;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading;
    using System.Web;
    using System.Windows.Forms;
    using System.Xml;

    public class WebSite
    {
        private BlindType _BlindInjType = BlindType.UnKnown;
        private DBType _DatabaseType = DBType.UnKnown;
        private string _DomainHost = "";
        private string _HTTPRoot = "";
        private int _HTTPThreadNum;
        private InjectionType _InjType = InjectionType.UnKnown;
        private int _NeedEscapeSpace = -1;
        private string _URL;
        public CookieContainer cc = new CookieContainer();
        private List<string> CrawledURL = new List<string>();
        public int CurrentFieldEchoIndex;
        public int CurrentFieldNum;
        public static TaskStatus CurrentStatus = TaskStatus.Ready;
        public Encoding DBEncoding = Encoding.UTF8;
        public DateTime dtLastSearch = DateTime.Now;
        public static bool EscapeCookie = true;
        public string FileExt = "";
        private string KeyWord = "";
        public DateTime LastGetTime = DateTime.Now;
        public DateTime LastModifiedTime = DateTime.Now;
        public static bool LogScannedURL = false;
        public static int MultiProcessNum = 0;
        private List<string> ScannedParameter = new List<string>();
        private List<string> ScannedURL = new List<string>();
        public bool SingleThreadLocked;
        public static DateTime StopTime;
        private Uri strUri;
        public string WcrFileName = "";
        public XmlDocument WcrXml;
        public Encoding WebEncoding = Encoding.UTF8;
        public string WebRoot = "";

        public WebSite(string sURL)
        {
            this._URL = sURL;
            this.InitDomain();
            this.InitWCRXML();
        }

        public void AddCrawledURL(string sURL)
        {
            this.CrawledURL.Add(sURL);
        }

        public void AddScannedParameter(string sURL)
        {
            this.ScannedParameter.Add(sURL);
        }

        public void AddScannedURL(string sURL)
        {
            this.ScannedURL.Add(sURL);
        }

        public void ClearWVS()
        {
            this.ScannedURL.Clear();
            this.CrawledURL.Clear();
            this.ScannedParameter.Clear();
        }

        public string ConvertPostData(string PostData)
        {
            PostData = PostData.Replace("\r\n", "");
            string[] strArray = PostData.Split(new char[] { '&' });
            string str = "";
            for (int i = 0; i < strArray.Length; i++)
            {
                string[] strArray2 = strArray[i].Split(new char[] { '=' });
                string str2 = strArray2[0];
                string str3 = "";
                for (int j = 1; j < strArray2.Length; j++)
                {
                    if (!string.IsNullOrEmpty(str3))
                    {
                        str3 = str3 + "=";
                    }
                    str3 = str3 + strArray2[j];
                }
                if (!string.IsNullOrEmpty(str3))
                {
                    str3 = HttpUtility.UrlEncode(HttpUtility.UrlDecode(str3, this.WebEncoding), this.WebEncoding).Replace("'", "%27");
                }
                if (!string.IsNullOrEmpty(str))
                {
                    str = str + "&";
                }
                str = str + str2 + "=" + str3;
            }
            return str;
        }

        public static string GenerateTestInput(int Index, string Expression)
        {
            return ("!S!WCRTESTINPUT" + string.Format("{0:D6}", Index) + Expression + "!E!");
        }

        public static string GetBaseHref(string Source)
        {
            Regex regex = new Regex(@"(?<=<base\s+href=[\x27\x22]?)[^\x27\x22\s>]+(?=[\x27\x22\s>]?)", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            return regex.Match(Source).Value;
        }

        public static string GetCompleteURL(string sURL, string Path)
        {
            string str = "";
            if (Path.LastIndexOf('/') < 9)
            {
                Path = Path.Trim() + "/";
            }
            string str2 = Path.Substring(0, Path.IndexOf("/", 9) + 1);
            if (sURL.IndexOf("http") == 0)
            {
                if (sURL.Length <= 8)
                {
                    return "";
                }
                str = sURL;
            }
            else if (sURL.IndexOf('/') == 0)
            {
                str = str2 + sURL.Substring(1);
            }
            else
            {
                str = Path + sURL;
            }
            str = str.Replace("&amp;", "&").Replace("/./", "/");
            if (str.IndexOf('#') > 0)
            {
                str = str.Substring(0, str.IndexOf('#'));
            }
            return str;
        }

        public string GetCookieStrFromCC()
        {
            try
            {
                CookieCollection cookies = this.cc.GetCookies(this.strUri);
                string str = "";
                for (int i = 0; i < cookies.Count; i++)
                {
                    if (!string.IsNullOrEmpty(str))
                    {
                        str = str + "; ";
                    }
                    if (EscapeCookie)
                    {
                        str = str + cookies[i].ToString();
                    }
                    else
                    {
                        str = str + GlobalObject.unescape(cookies[i].ToString());
                    }
                }
                return str;
            }
            catch
            {
                return "";
            }
        }

        public static string GetElementFromInputLine(string Line, string Element)
        {
            Regex regex = new Regex("(?<=" + Element + @"\s*=\s*[\x22\x27])[^\x22\x27\x3e]+?(?=[\x22\x27\x3e])", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            string str = regex.Match(Line).Value;
            if (string.IsNullOrEmpty(str))
            {
                regex = new Regex("(?<=" + Element + @"=)[^\x22\x27\x3e\s]+?(?=[\s\x3e])", RegexOptions.Singleline | RegexOptions.IgnoreCase);
                str = regex.Match(Line).Value;
            }
            return str;
        }

        public string GetFileExt(string sURL)
        {
            try
            {
                string str = sURL.Split(new char[] { '?' })[0].Substring(sURL.LastIndexOf('/') + 1);
                this.FileExt = str.Substring(str.IndexOf('.'));
                return this.FileExt;
            }
            catch
            {
                return "";
            }
        }

        public string[] GetFormInfo(string Source, string ReferPage)
        {
            string pathFromURL = GetPathFromURL(ReferPage);
            Regex regex = new Regex(@"<form\s+[^>]+>", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            MatchCollection matchs = regex.Matches(Source);
            List<string> list = new List<string>();
            string baseHref = GetBaseHref(Source);
            if (string.IsNullOrEmpty(baseHref))
            {
                baseHref = pathFromURL;
            }
            string input = "";
            int[] numArray = new int[matchs.Count];
            string str4 = "";
            for (int i = 0; i < matchs.Count; i++)
            {
                str4 = matchs[i].Value;
                if (i == 0)
                {
                    numArray[i] = Source.IndexOf(str4);
                }
                else
                {
                    numArray[i] = Source.IndexOf(str4, (int) (numArray[i - 1] + 10));
                }
            }
            for (int j = 0; j < matchs.Count; j++)
            {
                int index = 0;
                str4 = matchs[j].Value;
                if (j == (matchs.Count - 1))
                {
                    input = Source.Substring(numArray[j]);
                }
                else
                {
                    input = Source.Substring(numArray[j], numArray[j + 1] - numArray[j]);
                }
                string elementFromInputLine = GetElementFromInputLine(str4, "action");
                if (string.IsNullOrEmpty(elementFromInputLine))
                {
                    elementFromInputLine = ReferPage;
                }
                string str6 = GetElementFromInputLine(str4, "method");
                if (string.IsNullOrEmpty(str6))
                {
                    str6 = "POST";
                }
                else
                {
                    str6 = str6.ToUpper();
                }
                elementFromInputLine = GetCompleteURL(elementFromInputLine, baseHref);
                Uri uri = new Uri(elementFromInputLine);
                if (uri.Host.Equals(this.DomainHost))
                {
                    regex = new Regex(@"<input\s+[^>]+>", RegexOptions.Singleline | RegexOptions.IgnoreCase);
                    MatchCollection matchs2 = regex.Matches(input);
                    string str7 = "";
                    string str8 = "";
                    string str9 = "";
                    foreach (Match match in matchs2)
                    {
                        str8 = match.Value;
                        Source.IndexOf(str8);
                        str9 = GetElementFromInputLine(str8, "type");
                        if (string.IsNullOrEmpty(str9))
                        {
                            str9 = "text";
                        }
                        str9 = str9.ToLower();
                        if (str9.Equals("hidden"))
                        {
                            string str10 = GetElementFromInputLine(str8, "name");
                            if (!string.IsNullOrEmpty(str10))
                            {
                                string str11 = GetElementFromInputLine(str8, "value");
                                if (!string.IsNullOrEmpty(str7))
                                {
                                    str7 = str7 + "&";
                                }
                                str7 = str7 + str10 + "=" + str11;
                            }
                        }
                        else if (str9.Equals("radio"))
                        {
                            if (GetElementFromInputLine(str8, "checked").Equals("checked"))
                            {
                                string str13 = GetElementFromInputLine(str8, "name");
                                if (!string.IsNullOrEmpty(str13))
                                {
                                    string str14 = GetElementFromInputLine(str8, "value");
                                    if (!string.IsNullOrEmpty(str7))
                                    {
                                        str7 = str7 + "&";
                                    }
                                    str7 = str7 + str13 + "=" + str14;
                                }
                            }
                        }
                        else if (str9.Equals("text") || str9.Equals("password"))
                        {
                            string str15 = GetElementFromInputLine(str8, "name");
                            if (!string.IsNullOrEmpty(str15))
                            {
                                string str16 = GetElementFromInputLine(str8, "value");
                                if (!string.IsNullOrEmpty(str16))
                                {
                                    if (!string.IsNullOrEmpty(str7))
                                    {
                                        str7 = str7 + "&";
                                    }
                                    str7 = str7 + str15 + "=" + str16;
                                }
                                else
                                {
                                    str16 = GenerateTestInput(index, "");
                                    index++;
                                    if (!string.IsNullOrEmpty(str7))
                                    {
                                        str7 = str7 + "&";
                                    }
                                    str7 = str7 + str15 + "=" + str16;
                                }
                            }
                        }
                        else if (str9.Equals("submit"))
                        {
                            string str17 = GetElementFromInputLine(str8, "name");
                            if (!string.IsNullOrEmpty(str17))
                            {
                                string str18 = GetElementFromInputLine(str8, "value");
                                if (!string.IsNullOrEmpty(str18))
                                {
                                    if (!string.IsNullOrEmpty(str7))
                                    {
                                        str7 = str7 + "&";
                                    }
                                    str7 = str7 + str17 + "=" + str18;
                                }
                            }
                        }
                    }
                    regex = new Regex(@"<textarea\s+[^>]+>", RegexOptions.Singleline | RegexOptions.IgnoreCase);
                    foreach (Match match2 in regex.Matches(input))
                    {
                        string str19 = GetElementFromInputLine(match2.Value, "name");
                        if (!string.IsNullOrEmpty(str19))
                        {
                            string str20 = "!S!WCRTESTTEXTAREA" + string.Format("{0:D6}", index) + "!E!";
                            index++;
                            if (!string.IsNullOrEmpty(str7))
                            {
                                str7 = str7 + "&";
                            }
                            str7 = str7 + str19 + "=" + str20;
                        }
                    }
                    string selectNameOptionValue = GetSelectNameOptionValue(input);
                    if (!string.IsNullOrEmpty(selectNameOptionValue))
                    {
                        if (!string.IsNullOrEmpty(str7))
                        {
                            str7 = str7 + "&";
                        }
                        str7 = str7 + selectNameOptionValue;
                    }
                    string item = "";
                    if (str6 == "GET")
                    {
                        item = elementFromInputLine + "?" + str7;
                    }
                    else
                    {
                        item = elementFromInputLine + "^" + str7;
                    }
                    list.Add(item);
                }
            }
            return list.ToArray();
        }

        public string[] GetFormVuls(string sURL)
        {
            List<string> list = new List<string>();
            if (sURL.IndexOf("edit", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return list.ToArray();
            }
            if (sURL.IndexOf("modify", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return list.ToArray();
            }
            if (CurrentStatus == TaskStatus.Stop)
            {
                return list.ToArray();
            }
            try
            {
                HttpWebResponse httpWebResponse = this.GetHttpWebResponse(sURL, RequestType.GET);
                string sourceCodeFromHttpWebResponse = this.GetSourceCodeFromHttpWebResponse(httpWebResponse);
                string referPage = httpWebResponse.ResponseUri.ToString();
                foreach (string str3 in this.GetFormInfo(sourceCodeFromHttpWebResponse, referPage))
                {
                    RequestType gET;
                    string[] strArray2;
                    if (str3.IndexOf('^') < 0)
                    {
                        gET = RequestType.GET;
                        strArray2 = str3.Split(new char[] { '?' });
                    }
                    else
                    {
                        gET = RequestType.POST;
                        strArray2 = str3.Split(new char[] { '^' });
                    }
                    if (strArray2.Length >= 2)
                    {
                        if (WCRSetting.ScanSQLInjection || WCRSetting.ScanXPathInjection)
                        {
                            foreach (string str4 in this.GetInjectableURLDesc(str3, gET, referPage))
                            {
                                list.Add(str4);
                            }
                        }
                        if (!WCRSetting.ScanXSS)
                        {
                            return list.ToArray();
                        }
                        string[] strArray4 = strArray2[1].Split(new char[] { '&' });
                        for (int i = 0; i < strArray4.Length; i++)
                        {
                            string str5 = strArray2[0];
                            string str6 = "";
                            for (int j = 0; j < i; j++)
                            {
                                if (!string.IsNullOrEmpty(str6))
                                {
                                    str6 = str6 + "&";
                                }
                                str6 = str6 + strArray4[j];
                            }
                            string str7 = strArray4[i].Split(new char[] { '=' })[0];
                            if (!str7.ToLower().Equals("submit"))
                            {
                                string uRLPara = URL2NoParaURL(str5) + "^" + str7.ToLower() + "^XSS";
                                if (!this.IsScannedParameter(uRLPara))
                                {
                                    this.AddScannedParameter(uRLPara);
                                    if (!string.IsNullOrEmpty(str6))
                                    {
                                        str6 = str6 + "&";
                                    }
                                    str6 = str6 + str7 + "=" + GenerateTestInput(i, "<>%3c%3e%253c%253e");
                                    for (int k = i + 1; k < strArray4.Length; k++)
                                    {
                                        if (!string.IsNullOrEmpty(str6))
                                        {
                                            str6 = str6 + "&";
                                        }
                                        str6 = str6 + strArray4[k];
                                    }
                                    if (gET == RequestType.GET)
                                    {
                                        str5 = str5 + "?" + str6;
                                    }
                                    else
                                    {
                                        str5 = str5 + "^" + str6;
                                    }
                                    bool flag = false;
                                    string keyTextFromSource = GetKeyTextFromSource(this.GetSourceCode(str5, gET), i);
                                    if (!string.IsNullOrEmpty(keyTextFromSource))
                                    {
                                        if (keyTextFromSource.IndexOf("<>") >= 0)
                                        {
                                            flag = true;
                                        }
                                    }
                                    else if (gET == RequestType.POST)
                                    {
                                        keyTextFromSource = GetKeyTextFromSource(this.GetSourceCode(sURL, RequestType.GET), i);
                                        if (!string.IsNullOrEmpty(keyTextFromSource) && (keyTextFromSource.IndexOf("<>") >= 0))
                                        {
                                            flag = true;
                                        }
                                    }
                                    if (flag)
                                    {
                                        string str11 = RemoveTestInput(str5);
                                        string item = sURL + "^^" + str7 + "^^" + gET.ToString() + "^^" + str11 + "^^Cross Site Scripting(Form)";
                                        list.Add(item);
                                    }
                                }
                            }
                        }
                    }
                }
                return list.ToArray();
            }
            catch
            {
                return list.ToArray();
            }
        }

        public HttpWebResponse GetHttpWebResponse(string sURL, RequestType ReqType)
        {
            HttpWebResponse response3;
            string requestUriString = "";
            sURL = sURL.Replace(" ", "%20");
            if (sURL.IndexOf("99999999") > 0)
            {
                sURL = sURL.Replace("%20and%20(", "%20or%20(");
            }
            if (WCRSetting.chkReplace1)
            {
                sURL = sURL.Replace(WCRSetting.FiltExpr1, WCRSetting.RepExpr1);
            }
            if (WCRSetting.chkReplace2)
            {
                sURL = sURL.Replace(WCRSetting.FiltExpr2, WCRSetting.RepExpr2);
            }
            if (WCRSetting.chkReplace3)
            {
                sURL = sURL.Replace(WCRSetting.FiltExpr3, WCRSetting.RepExpr3);
            }
            if (this._NeedEscapeSpace == 1)
            {
                sURL = sURL.Replace("%20", "/**/");
            }
            if ((this._DatabaseType == DBType.DB2) || (this._DatabaseType == DBType.Access))
            {
                sURL = sURL.Replace("/**/", "%20");
            }
            if ((this._InjType == InjectionType.Search) || (ReqType == RequestType.COOKIE))
            {
                while (this.SingleThreadLocked)
                {
                    Thread.Sleep(0x3e8);
                }
                this.SingleThreadLocked = true;
            }
            if (this._InjType == InjectionType.Search)
            {
                TimeSpan span = DateTime.Now.Subtract(this.dtLastSearch);
                int num = WCRSetting.SecondsDelay * 0x3e8;
                if (span.Milliseconds < num)
                {
                    Thread.Sleep((int) (num - span.Milliseconds));
                }
                this.dtLastSearch = DateTime.Now;
            }
            while (this._HTTPThreadNum >= WCRSetting.MaxHTTPThreadNum)
            {
                Thread.Sleep(300);
            }
            try
            {
                if (CurrentStatus == TaskStatus.Stop)
                {
                    Thread.CurrentThread.Abort();
                }
            }
            catch
            {
            }
            string s = "";
            requestUriString = sURL;
            if (ReqType == RequestType.POST)
            {
                string[] paraNameValue = GetParaNameValue(sURL, '^');
                requestUriString = paraNameValue[0];
                s = this.ConvertPostData(paraNameValue[1]);
            }
            else if ((ReqType == RequestType.COOKIE) && (sURL.IndexOf('^') > 0))
            {
                string[] strArray2 = GetParaNameValue(sURL, '^');
                requestUriString = strArray2[0];
                string cookieStr = strArray2[1];
                this.SetSingleCookie(cookieStr);
            }
            try
            {
                HttpWebRequest request = (HttpWebRequest) WebRequest.Create(requestUriString);
                request.UserAgent = WCRSetting.UserAgent;
                request.ContentType = "application/x-www-form-urlencoded";
                request.CookieContainer = this.cc;
                if (ReqType == RequestType.POST)
                {
                    request.Method = "POST";
                    byte[] bytes = Encoding.UTF8.GetBytes(s);
                    request.ContentLength = bytes.Length;
                    Stream requestStream = request.GetRequestStream();
                    requestStream.Write(bytes, 0, bytes.Length);
                    requestStream.Close();
                }
                else
                {
                    request.Method = "GET";
                }
                if (WCRSetting.UseProxy)
                {
                    WebProxy proxy = new WebProxy(WCRSetting.ProxyAddress, WCRSetting.ProxyPort);
                    if (!string.IsNullOrEmpty(WCRSetting.ProxyUsername))
                    {
                        proxy.Credentials = new NetworkCredential(WCRSetting.ProxyUsername, WCRSetting.ProxyPassword);
                    }
                    else
                    {
                        proxy.Credentials = CredentialCache.DefaultCredentials;
                    }
                    request.Proxy = proxy;
                    request.UnsafeAuthenticatedConnectionSharing = true;
                }
                request.Timeout = 0x7530;
                while (CurrentStatus == TaskStatus.Pause)
                {
                    Thread.Sleep(0x3e8);
                }
                try
                {
                    if (CurrentStatus == TaskStatus.Stop)
                    {
                        Thread.CurrentThread.Abort();
                    }
                }
                catch
                {
                }
                this._HTTPThreadNum++;
                HttpWebResponse response = (HttpWebResponse) request.GetResponse();
                string characterSet = response.CharacterSet;
                this.LastModifiedTime = response.LastModified;
                this.LastGetTime = DateTime.Now;
                this.SingleThreadLocked = false;
                if (LogScannedURL)
                {
                    LogScannedData(ReqType.ToString() + "  " + sURL + "  " + response.StatusCode.ToString());
                }
                response3 = response;
            }
            catch (WebException exception)
            {
                this.SingleThreadLocked = false;
                HttpWebResponse response2 = exception.Response as HttpWebResponse;
                if (LogScannedURL)
                {
                    string scanData = ReqType.ToString() + "  " + sURL;
                    if (response2 == null)
                    {
                        scanData = scanData + " NullResponse";
                    }
                    else
                    {
                        scanData = scanData + "  " + response2.StatusCode.ToString();
                    }
                    LogScannedData(scanData);
                }
                response3 = response2;
            }
            catch (Exception exception2)
            {
                if (LogScannedURL)
                {
                    LogScannedData(ReqType.ToString() + "  " + sURL + "  Exception:" + exception2.Message);
                }
                response3 = null;
            }
            finally
            {
                this.SingleThreadLocked = false;
                this._HTTPThreadNum--;
                if (this._HTTPThreadNum < 0)
                {
                    this._HTTPThreadNum = 0;
                }
            }
            return response3;
        }

        public string[] GetInjectableURLDesc(string sURL, RequestType ReqType, string RefPage)
        {
            string[] strArray;
            List<string> list = new List<string>();
            this.GetSourceCode(sURL, ReqType);
            if (ReqType == RequestType.GET)
            {
                strArray = sURL.Split(new char[] { '?' });
            }
            else
            {
                strArray = sURL.Split(new char[] { '^' });
            }
            if (strArray.Length >= 2)
            {
                string[] strArray2 = strArray[1].Split(new char[] { '&' });
                for (int i = 0; i < 2; i++)
                {
                    if (this.NeedEscapeSpace == 1)
                    {
                        i++;
                    }
                    else if ((this.NeedEscapeSpace == 0) && (i == 1))
                    {
                        break;
                    }
                    for (int j = 0; j < strArray2.Length; j++)
                    {
                        string str = strArray2[((j + strArray2.Length) - 1) % strArray2.Length];
                        string[] strArray3 = str.Split(new char[] { '=' });
                        if ((strArray3.Length >= 2) && !string.IsNullOrEmpty(strArray3[1]))
                        {
                            string injParameter = strArray3[0];
                            if (!injParameter.ToLower().Equals("submit"))
                            {
                                string uRLPara = URL2NoParaURL(sURL) + "^" + injParameter.ToLower() + "^Injection";
                                if (!this.IsScannedParameter(uRLPara))
                                {
                                    this.ScannedParameter.Add(uRLPara);
                                    string str4 = strArray3[1];
                                    for (int k = 0; k < 4; k++)
                                    {
                                        bool flag = false;
                                        string expression = str;
                                        char ch = '&';
                                        RequestType reqType = ReqType;
                                        switch (k)
                                        {
                                            case 1:
                                                if (ReqType == RequestType.GET)
                                                {
                                                    if (!WCRSetting.ScanCookieSQL)
                                                    {
                                                        continue;
                                                    }
                                                    ch = '^';
                                                    reqType = RequestType.COOKIE;
                                                }
                                                else
                                                {
                                                    if (((ReqType != RequestType.POST) || (str4.IndexOf("WCRTESTINPUT") < 0)) || !WCRSetting.ScanPostSQL)
                                                    {
                                                        continue;
                                                    }
                                                    expression = injParameter + "=1";
                                                }
                                                break;

                                            case 2:
                                                if (ReqType == RequestType.GET)
                                                {
                                                    if (flag)
                                                    {
                                                        continue;
                                                    }
                                                    expression = injParameter + "=99999999";
                                                }
                                                else
                                                {
                                                    if (((ReqType != RequestType.POST) || (str4.IndexOf("WCRTESTINPUT") < 0)) || flag)
                                                    {
                                                        continue;
                                                    }
                                                    expression = injParameter + "=99999999";
                                                }
                                                break;

                                            case 3:
                                                if (flag)
                                                {
                                                    continue;
                                                }
                                                expression = injParameter + "=";
                                                break;
                                        }
                                        string str6 = "";
                                        string str7 = "";
                                        if (strArray2.Length == 1)
                                        {
                                            str6 = expression;
                                            if ((ReqType == RequestType.GET) && (k != 1))
                                            {
                                                str7 = strArray[0] + "?" + str6;
                                            }
                                            else
                                            {
                                                str7 = strArray[0] + "^" + str6;
                                            }
                                        }
                                        else
                                        {
                                            if (strArray2.Length <= 1)
                                            {
                                                return list.ToArray();
                                            }
                                            str6 = strArray2[j];
                                            for (int m = 1; m < (strArray2.Length - 1); m++)
                                            {
                                                str6 = str6 + "&" + strArray2[(j + m) % strArray2.Length];
                                            }
                                            if (!string.IsNullOrEmpty(str6))
                                            {
                                                str6 = str6 + ch;
                                            }
                                            str6 = str6 + expression;
                                            if (ReqType == RequestType.GET)
                                            {
                                                str7 = strArray[0] + "?" + str6;
                                            }
                                            else
                                            {
                                                str7 = strArray[0] + "^" + str6;
                                            }
                                        }
                                        InjectionType lastParaInjectionType = GetLastParaInjectionType(str7);
                                        if (lastParaInjectionType != InjectionType.NotInjectable)
                                        {
                                            bool escapeSpace = false;
                                            if (i == 1)
                                            {
                                                escapeSpace = true;
                                            }
                                            string logicOperator = "aNd";
                                            switch (k)
                                            {
                                                case 2:
                                                    logicOperator = "oR";
                                                    break;

                                                case 3:
                                                    logicOperator = "oR";
                                                    lastParaInjectionType = InjectionType.Search;
                                                    break;
                                            }
                                            string str9 = this.GetKeyWordByURLLastPara(str7, reqType, lastParaInjectionType, logicOperator, escapeSpace, false, injParameter);
                                            if (CurrentStatus == TaskStatus.Stop)
                                            {
                                                return list.ToArray();
                                            }
                                            if (string.IsNullOrEmpty(str9))
                                            {
                                                if (lastParaInjectionType == InjectionType.Integer)
                                                {
                                                    str9 = this.GetKeyWordByURLLastPara(str7, reqType, InjectionType.String, logicOperator, escapeSpace, false, injParameter);
                                                    if (CurrentStatus == TaskStatus.Stop)
                                                    {
                                                        return list.ToArray();
                                                    }
                                                    if (!string.IsNullOrEmpty(str9))
                                                    {
                                                        flag = true;
                                                        lastParaInjectionType = InjectionType.String;
                                                        if (i == 0)
                                                        {
                                                            this.NeedEscapeSpace = 0;
                                                        }
                                                        else
                                                        {
                                                            this.NeedEscapeSpace = 1;
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                flag = true;
                                                if (i == 0)
                                                {
                                                    this.NeedEscapeSpace = 0;
                                                }
                                                else
                                                {
                                                    this.NeedEscapeSpace = 1;
                                                }
                                            }
                                            string str10 = "";
                                            string str11 = RemoveTestInput(str7);
                                            string str12 = str9;
                                            if (reqType == RequestType.GET)
                                            {
                                                str10 = "URL SQL INJECTION";
                                            }
                                            else if (reqType == RequestType.POST)
                                            {
                                                str10 = "POST SQL INJECTION";
                                            }
                                            else
                                            {
                                                str10 = "COOKIE SQL INJECTION";
                                            }
                                            if ((k == 2) && !string.IsNullOrEmpty(RefPage))
                                            {
                                                if (str7.IndexOf("WCRTESTTEXTAREA") > 0)
                                                {
                                                    continue;
                                                }
                                                if (!string.IsNullOrEmpty(this.GetKeyWordByURLLastPara(str7, reqType, InjectionType.String, logicOperator, escapeSpace, true, injParameter)))
                                                {
                                                    str10 = "XPath INJECTION";
                                                    str11 = RefPage;
                                                    expression = RemoveTestInput(expression);
                                                    lastParaInjectionType = InjectionType.String;
                                                    str12 = RemoveTestInput(str7);
                                                    flag = true;
                                                }
                                            }
                                            if (flag)
                                            {
                                                list.Add(str11 + "^^" + expression + "^^" + lastParaInjectionType.ToString() + "^^" + str12 + "^^" + str10);
                                                if ((ReqType == RequestType.GET) && (k == 1))
                                                {
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (this.NeedEscapeSpace == 0)
                    {
                        break;
                    }
                }
            }
            return list.ToArray();
        }

        public static string GetKeyTextFromSource(string SourceCode, int Index)
        {
            string str = "!S!WCRTESTINPUT" + string.Format("{0:D6}", Index);
            if (SourceCode.IndexOf(str) < 0)
            {
                str = "!S!WCRTESTTEXTAREA" + string.Format("{0:D6}", Index);
            }
            Regex regex = new Regex("(?<=(" + str + @"))[.\s\S]*?(?=(!E!))", RegexOptions.Singleline | RegexOptions.Multiline);
            return regex.Match(SourceCode).Value;
        }

        public static string GetKeyWordBySource(string Source1, string Source2, string ParaName)
        {
            if (IsErrorResponse(Source1, ParaName))
            {
                return "";
            }
            Regex regex = new Regex("<[^>]+>", RegexOptions.Singleline | RegexOptions.Multiline);
            Source1 = regex.Replace(Source1, " ");
            Source2 = regex.Replace(Source2, " ");
            string[] strArray = new Regex(@"[^a-zA-Z\u0080-\uFFFF]+", RegexOptions.Singleline | RegexOptions.Multiline).Split(Source1);
            for (int i = 0; i < strArray.Length; i++)
            {
                if (((strArray[i].Length >= 5) && !strArray[i].Equals("admin")) && (Source2.IndexOf(strArray[i]) < 0))
                {
                    return strArray[i];
                }
            }
            return "";
        }

        public string GetKeyWordByURLLastPara(string sURL, RequestType ReqType, InjectionType InjType, string LogicOperator, bool EscapeSpace, bool XPathTest, string InjParameter)
        {
            if (InjType == InjectionType.NotInjectable)
            {
                return "";
            }
            string cookieStr = "";
            if (ReqType == RequestType.COOKIE)
            {
                cookieStr = this.GetCookieStrFromCC();
            }
            try
            {
                string str2 = "";
                string uRL = "";
                if (!XPathTest)
                {
                    if (!WCRSetting.ScanSQLInjection)
                    {
                        return "";
                    }
                    if ((ReqType == RequestType.GET) && !WCRSetting.ScanURLSQL)
                    {
                        return "";
                    }
                    if ((ReqType == RequestType.POST) && !WCRSetting.ScanPostSQL)
                    {
                        return "";
                    }
                    if ((ReqType == RequestType.COOKIE) && !WCRSetting.ScanCookieSQL)
                    {
                        return "";
                    }
                    if (InjType == InjectionType.Integer)
                    {
                        str2 = sURL + "%20" + LogicOperator + "%207=7";
                        uRL = sURL + "%20" + LogicOperator + "%207=2";
                    }
                    else if (InjType == InjectionType.String)
                    {
                        str2 = sURL + "%27%20" + LogicOperator + "%20%277%27=%277";
                        uRL = sURL + "%27%20" + LogicOperator + "%20%277%27=%272";
                    }
                    else if (InjType == InjectionType.Search)
                    {
                        str2 = sURL + "%27%20" + LogicOperator + "%20%27%25%27%3D%27";
                        uRL = sURL + "%27%20" + LogicOperator + "%20%27%25%27%3D%272";
                    }
                    if (EscapeSpace)
                    {
                        str2 = str2.Replace("%20", "/**/");
                        uRL = uRL.Replace("%20", "/**/");
                    }
                }
                else
                {
                    if (!WCRSetting.ScanXPathInjection)
                    {
                        return "";
                    }
                    str2 = sURL + "%27] | * | user[@role=%27admin";
                    uRL = sURL;
                }
                HttpWebResponse httpWebResponse = this.GetHttpWebResponse(str2, ReqType);
                if (httpWebResponse == null)
                {
                    LogScannedData(ReqType.ToString() + "  " + sURL + "  " + InjType.ToString() + " URL1 Null");
                    return "";
                }
                if (httpWebResponse.StatusCode != HttpStatusCode.OK)
                {
                    LogScannedData(ReqType.ToString() + "  " + sURL + "  " + InjType.ToString() + " URL1 StatusCode!=OK");
                    return "";
                }
                httpWebResponse.ResponseUri.ToString();
                if (CurrentStatus == TaskStatus.Stop)
                {
                    Thread.CurrentThread.Abort();
                }
                string sourceCodeFromHttpWebResponse = this.GetSourceCodeFromHttpWebResponse(httpWebResponse);
                if (string.IsNullOrEmpty(sourceCodeFromHttpWebResponse))
                {
                    LogScannedData(ReqType.ToString() + "  " + sURL + "  " + InjType.ToString() + " KeyWord Null");
                    return "";
                }
                string sourceCode = this.GetSourceCode(uRL, ReqType);
                string str6 = GetKeyWordBySource(sourceCodeFromHttpWebResponse, sourceCode, InjParameter);
                if (ReqType == RequestType.COOKIE)
                {
                    this.SetCookie(cookieStr);
                }
                LogScannedData(ReqType.ToString() + "  " + sURL + "  " + InjType.ToString() + " KeyWord=" + str6);
                return str6;
            }
            catch
            {
                if (ReqType == RequestType.COOKIE)
                {
                    this.SetCookie(cookieStr);
                }
                return "";
            }
        }

        public static InjectionType GetLastParaInjectionType(string sURL)
        {
            string[] strArray;
            if (sURL.LastIndexOf('=') < 0)
            {
                return InjectionType.NotInjectable;
            }
            if (sURL.IndexOf('^') > 0)
            {
                strArray = sURL.Split(new char[] { '^' });
            }
            else
            {
                strArray = sURL.Split(new char[] { '?' });
            }
            if (strArray.Length < 2)
            {
                return InjectionType.NotInjectable;
            }
            string[] strArray2 = strArray[1].Split(new char[] { '&' });
            string[] strArray3 = strArray2[strArray2.Length - 1].Split(new char[] { '=' });
            if ((strArray3.Length < 2) || string.IsNullOrEmpty(strArray3[1]))
            {
                return InjectionType.Search;
            }
            string str2 = strArray3[0];
            string input = strArray3[1];
            if (str2.ToLower().IndexOf("search") >= 0)
            {
                return InjectionType.Search;
            }
            if (str2.ToLower().IndexOf("query") >= 0)
            {
                return InjectionType.Search;
            }
            if (Regex.IsMatch(input, @"^[\d\-]+$"))
            {
                return InjectionType.Integer;
            }
            return InjectionType.String;
        }

        public static string[] GetLinksByTagname(string Source, string TagName, string PropertyName)
        {
            List<string> list = new List<string>();
            Regex regex = new Regex("<" + TagName + @"\s+[^>]+>", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            foreach (Match match in regex.Matches(Source))
            {
                string elementFromInputLine = GetElementFromInputLine(match.Value, PropertyName);
                if ((((!string.IsNullOrEmpty(elementFromInputLine) && (elementFromInputLine.IndexOf("logout", StringComparison.OrdinalIgnoreCase) < 0)) && ((elementFromInputLine.IndexOf("signout", StringComparison.OrdinalIgnoreCase) < 0) && (elementFromInputLine.IndexOf("exit", StringComparison.OrdinalIgnoreCase) < 0))) && (((elementFromInputLine.IndexOf("quit", StringComparison.OrdinalIgnoreCase) < 0) && (elementFromInputLine.IndexOf("delete", StringComparison.OrdinalIgnoreCase) < 0)) && ((elementFromInputLine.IndexOf("#") != 0) && (elementFromInputLine.IndexOf(@"\") != 0)))) && (((elementFromInputLine.IndexOf("@") < 0) && (elementFromInputLine.IndexOf("javascript", StringComparison.OrdinalIgnoreCase) != 0)) && ((elementFromInputLine.IndexOf("http") != 0) || (elementFromInputLine.Length >= 10))))
                {
                    list.Add(elementFromInputLine);
                }
            }
            return list.ToArray();
        }

        public static string[] GetLinksFromScript(string Source)
        {
            List<string> list = new List<string>();
            Regex regex = new Regex(@"(?<=document\.location\s*=\s*[\x22\x27])[^\x22\x27]+?(?=[\x22\x27\x3b])", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            foreach (Match match in regex.Matches(Source))
            {
                string str = match.Value;
                if ((((!string.IsNullOrEmpty(str) && (str.IndexOf("logout", StringComparison.OrdinalIgnoreCase) < 0)) && ((str.IndexOf("signout", StringComparison.OrdinalIgnoreCase) < 0) && (str.IndexOf("delete", StringComparison.OrdinalIgnoreCase) < 0))) && (((str.IndexOf("#") != 0) && (str.IndexOf(@"\") != 0)) && ((str.IndexOf("@") < 0) && (str.IndexOf("javascript", StringComparison.OrdinalIgnoreCase) != 0)))) && ((str.IndexOf("http") != 0) || (str.Length >= 10)))
                {
                    list.Add(str);
                }
            }
            return list.ToArray();
        }

        public string[] GetLinksFromSource(string Source, string RefPath, string PageExt)
        {
            string baseHref = GetBaseHref(Source);
            if (string.IsNullOrEmpty(baseHref))
            {
                baseHref = RefPath;
            }
            List<string> list = new List<string>();
            if (PageExt.Equals(".js"))
            {
                foreach (string str2 in GetLinksFromScript(Source))
                {
                    list.Add(str2);
                }
            }
            else
            {
                foreach (string str3 in GetTextLinksBySource(Source))
                {
                    list.Add(str3);
                }
                foreach (string str4 in GetLinksByTagname(Source, "a", "href"))
                {
                    if (str4.IndexOf("mailto:") != 0)
                    {
                        list.Add(str4);
                    }
                }
                foreach (string str5 in GetLinksByTagname(Source, "form", "action"))
                {
                    list.Add(str5);
                }
                foreach (string str6 in GetLinksByTagname(Source, "iframe", "src"))
                {
                    list.Add(str6);
                }
                foreach (string str7 in GetLinksByTagname(Source, "script", "src"))
                {
                    list.Add(str7);
                }
                foreach (string str8 in GetLinksFromScript(Source))
                {
                    list.Add(str8);
                }
            }
            for (int i = 0; i < list.Count; i++)
            {
                list[i] = GetCompleteURL(list[i], baseHref);
            }
            return list.ToArray();
        }

        public static string[] GetParaNameValue(string Expression, char SplitChar)
        {
            string[] strArray = Expression.Split(new char[] { SplitChar });
            string[] strArray2 = new string[2];
            strArray2[0] = strArray[0];
            string str = "";
            for (int i = 1; i < strArray.Length; i++)
            {
                if (!string.IsNullOrEmpty(str))
                {
                    str = str + SplitChar;
                }
                str = str + strArray[i];
            }
            strArray2[1] = str;
            return strArray2;
        }

        public static string GetPathFromURL(string sURL)
        {
            if (sURL.IndexOf('?') < 0)
            {
                return sURL.Substring(0, sURL.LastIndexOf('/') + 1);
            }
            string[] strArray = sURL.Split(new char[] { '?' });
            return strArray[0].Substring(0, strArray[0].LastIndexOf('/') + 1);
        }

        public static string GetSelectNameOptionValue(string FormSource)
        {
            string str = "";
            Regex regex = new Regex(@"<select\s+[^>]+>", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            MatchCollection matchs = regex.Matches(FormSource);
            int[] numArray = new int[matchs.Count];
            for (int i = 0; i < matchs.Count; i++)
            {
                string str2 = matchs[i].Value;
                if (i == 0)
                {
                    numArray[i] = FormSource.IndexOf(str2);
                }
                else
                {
                    numArray[i] = FormSource.IndexOf(str2, (int) (numArray[i - 1] + 10));
                }
            }
            for (int j = 0; j < matchs.Count; j++)
            {
                string elementFromInputLine = GetElementFromInputLine(matchs[j].Value, "name");
                if (!string.IsNullOrEmpty(elementFromInputLine))
                {
                    string input = "";
                    if (j == (matchs.Count - 1))
                    {
                        input = FormSource.Substring(numArray[j]);
                    }
                    else
                    {
                        input = FormSource.Substring(numArray[j], numArray[j + 1] - numArray[j]);
                    }
                    MatchCollection matchs2 = new Regex(@"<option\s+[^>]+>", RegexOptions.Singleline | RegexOptions.IgnoreCase).Matches(input);
                    for (int k = 0; k < matchs2.Count; k++)
                    {
                        string str5 = GetElementFromInputLine(matchs2[k].Value, "value");
                        if (!string.IsNullOrEmpty(str5))
                        {
                            if (!string.IsNullOrEmpty(str))
                            {
                                str = str + "&";
                            }
                            str = str + elementFromInputLine + "=" + str5;
                            break;
                        }
                    }
                }
            }
            return str;
        }

        public string GetSourceCode(string URL, RequestType ReqType)
        {
            try
            {
                if (CurrentStatus == TaskStatus.Stop)
                {
                    Thread.CurrentThread.Abort();
                }
                HttpWebResponse httpWebResponse = this.GetHttpWebResponse(URL, ReqType);
                if (httpWebResponse == null)
                {
                    return "";
                }
                Stream responseStream = httpWebResponse.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream, this.WebEncoding);
                string str = reader.ReadToEnd();
                reader.Close();
                responseStream.Close();
                httpWebResponse.Close();
                return str;
            }
            catch
            {
                return "";
            }
        }

        public string GetSourceCode(string URL, RequestType ReqType, Encoding CurrentEncoding)
        {
            try
            {
                if (CurrentStatus == TaskStatus.Stop)
                {
                    Thread.CurrentThread.Abort();
                }
                HttpWebResponse httpWebResponse = this.GetHttpWebResponse(URL, ReqType);
                Stream responseStream = httpWebResponse.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream, CurrentEncoding);
                string str = reader.ReadToEnd();
                reader.Close();
                responseStream.Close();
                httpWebResponse.Close();
                return str;
            }
            catch
            {
                return "";
            }
        }

        public string GetSourceCodeFromHttpWebResponse(HttpWebResponse HttpResp)
        {
            if (CurrentStatus == TaskStatus.Stop)
            {
                return "";
            }
            try
            {
                Stream responseStream = HttpResp.GetResponseStream();
                string characterSet = HttpResp.CharacterSet;
                StreamReader reader = new StreamReader(responseStream, this.WebEncoding);
                string str = reader.ReadToEnd();
                reader.Close();
                responseStream.Close();
                HttpResp.Close();
                return str;
            }
            catch
            {
                return "";
            }
        }

        public static string[] GetTextLinksBySource(string Source)
        {
            List<string> list = new List<string>();
            Regex regex = new Regex(@"(?<=<b>)[^\s<]+(?=</b>)|(?<=<u>)[^\s<]+(?=</u>)", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            foreach (Match match in regex.Matches(Source))
            {
                string str = match.Value;
                if ((((!string.IsNullOrEmpty(str) && (str.IndexOf(@"\") < 0)) && ((str.IndexOf("logout", StringComparison.OrdinalIgnoreCase) < 0) && (str.IndexOf("signout", StringComparison.OrdinalIgnoreCase) < 0))) && (((str.IndexOf("exit", StringComparison.OrdinalIgnoreCase) < 0) && (str.IndexOf("quit", StringComparison.OrdinalIgnoreCase) < 0)) && ((str.IndexOf("delete", StringComparison.OrdinalIgnoreCase) < 0) && (str.IndexOf("#") != 0)))) && ((((str.IndexOf(@"\") != 0) && (str.IndexOf("@") < 0)) && (str.IndexOf("javascript", StringComparison.OrdinalIgnoreCase) != 0)) && ((str.IndexOf("http") != 0) || (str.Length >= 10))))
                {
                    list.Add(str);
                }
            }
            return list.ToArray();
        }

        public bool HasCrawledURL(string sURL)
        {
            string str = URL2DistinctURL(sURL);
            for (int i = 0; i < this.CrawledURL.Count; i++)
            {
                if (this.CrawledURL[i].Equals(str))
                {
                    return true;
                }
            }
            return false;
        }

        public bool HasScannedURL(string sURL)
        {
            string str = URL2DistinctURL(sURL);
            for (int i = 0; i < this.ScannedURL.Count; i++)
            {
                if (this.ScannedURL[i].Equals(str))
                {
                    return true;
                }
            }
            return false;
        }

        private void InitDomain()
        {
            if (!string.IsNullOrEmpty(this._URL) && (this._URL.IndexOf("about:blank") != 0))
            {
                if (this._URL.IndexOf("http") < 0)
                {
                    this._URL = "http://" + this._URL.Trim();
                }
                if (this._URL.LastIndexOf('/') < 9)
                {
                    this._URL = this._URL.Trim() + "/";
                }
                this._HTTPRoot = this._URL.Substring(0, this._URL.IndexOf("/", 9) + 1);
                try
                {
                    this.strUri = new Uri(this._HTTPRoot);
                    this._DomainHost = this.strUri.Host;
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message);
                }
            }
        }

        private void InitWCRXML()
        {
            this.WcrXml = new XmlDocument();
            XmlNode newChild = this.WcrXml.CreateXmlDeclaration("1.0", "utf-8", "");
            this.WcrXml.AppendChild(newChild);
            XmlComment comment = this.WcrXml.CreateComment("Created By WebCruiser - Web Vulnerability Scanner http://sec4app.com");
            this.WcrXml.AppendChild(comment);
            XmlElement element = this.WcrXml.CreateElement("ROOT");
            this.WcrXml.AppendChild(element);
            XmlElement element2 = this.WcrXml.CreateElement("CurrentSite");
            element.AppendChild(element2);
            XmlElement element3 = this.WcrXml.CreateElement("SiteVulList");
            element.AppendChild(element3);
            XmlElement element4 = this.WcrXml.CreateElement("SiteDirTree");
            element.AppendChild(element4);
            XmlElement element5 = this.WcrXml.CreateElement("SiteSQLEnv");
            element.AppendChild(element5);
            XmlElement element6 = this.WcrXml.CreateElement("SiteDBStructure");
            element.AppendChild(element6);
        }

        [DllImport("wininet.dll", CharSet=CharSet.Auto, SetLastError=true)]
        public static extern bool InternetSetCookie(string lpszUrlName, string lbszCookieName, string lpszCookieData);
        public static bool IsErrorResponse(string Source1, string ParaName)
        {
            try
            {
                Source1 = Source1.ToLower();
                if (Source1.IndexOf("<") < 0)
                {
                    return true;
                }
                if (Source1.Length < 200)
                {
                    if (Source1.IndexOf("error") > 0)
                    {
                        return true;
                    }
                    if (Source1.IndexOf("UnKnown") > 0)
                    {
                        return true;
                    }
                    if (Source1.IndexOf("invalid") > 0)
                    {
                        return true;
                    }
                    if (Source1.IndexOf("wrong") > 0)
                    {
                        return true;
                    }
                    if (Source1.IndexOf("错误") >= 0)
                    {
                        return true;
                    }
                    if (Source1.IndexOf("出错") >= 0)
                    {
                        return true;
                    }
                }
                else if (Source1.Length < 0x7d0)
                {
                    if ((Source1.IndexOf("sql ") > 0) && (Source1.IndexOf(" error") > 0))
                    {
                        return true;
                    }
                }
                else if (Source1.Length < 0x2800)
                {
                    if ((Source1.IndexOf("sorry ") > 0) && (Source1.IndexOf(" log") > 0))
                    {
                        return true;
                    }
                    if ((Source1.IndexOf("wrong") > 0) && (Source1.IndexOf("password") > 0))
                    {
                        return true;
                    }
                    if ((Source1.IndexOf("对不起") > 0) && (Source1.IndexOf("未登录") > 0))
                    {
                        return true;
                    }
                    if ((Source1.IndexOf("密码") > 0) && (Source1.IndexOf("错误") > 0))
                    {
                        return true;
                    }
                }
                Regex regex = new Regex("(?<=<title>).+(?=</title>)", RegexOptions.Singleline | RegexOptions.IgnoreCase);
                string str = regex.Match(Source1).Value;
                if (!string.IsNullOrEmpty(str))
                {
                    if (str.IndexOf("error") > 0)
                    {
                        return true;
                    }
                    if (str.IndexOf("UnKnown") > 0)
                    {
                        return true;
                    }
                    if (str.IndexOf("invalid") > 0)
                    {
                        return true;
                    }
                    if (str.IndexOf("wrong") > 0)
                    {
                        return true;
                    }
                    if (str.IndexOf("错误") >= 0)
                    {
                        return true;
                    }
                    if (str.IndexOf("出错") >= 0)
                    {
                        return true;
                    }
                }
                if ((Source1.IndexOf("99999999") >= 0) || (Source1.IndexOf("ParaName") >= 0))
                {
                    if (Source1.IndexOf("error") > 0)
                    {
                        return true;
                    }
                    if (Source1.IndexOf("UnKnown") > 0)
                    {
                        return true;
                    }
                    if (Source1.IndexOf("invalid") > 0)
                    {
                        return true;
                    }
                    if (Source1.IndexOf("wrong") > 0)
                    {
                        return true;
                    }
                    if (Source1.IndexOf("错误") >= 0)
                    {
                        return true;
                    }
                    if (Source1.IndexOf("出错") >= 0)
                    {
                        return true;
                    }
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public bool IsScannedParameter(string URLPara)
        {
            for (int i = 0; i < this.ScannedParameter.Count; i++)
            {
                if (this.ScannedParameter[i].Equals(URLPara))
                {
                    return true;
                }
            }
            return false;
        }

        public static void LogScannedData(string ScanData)
        {
            DateTime now = DateTime.Now;
            string path = Application.StartupPath + @"\WebCruiser" + now.ToString("yyyyMMdd") + ".log";
            string s = string.Concat(new object[] { now, "  ", ScanData, "\r\n" });
            byte[] bytes = Encoding.Default.GetBytes(s);
            FileStream stream = new FileStream(path, FileMode.Append, FileAccess.Write, FileShare.ReadWrite, 8, FileOptions.Asynchronous);
            stream.Write(bytes, 0, bytes.Length);
            stream.Close();
            stream.Dispose();
        }

        public static string RemoveTestInput(string Expression)
        {
            return Expression.Replace("!S!", "").Replace("!E!", "");
        }

        public string Save(bool IsSaveAs)
        {
            if (string.IsNullOrEmpty(this.WcrFileName) || IsSaveAs)
            {
                string str = "WebCruiserWVS_" + this.DomainHost + ".xml";
                SaveFileDialog dialog = new SaveFileDialog {
                    Filter = "XML File(*.xml) | *.xml",
                    InitialDirectory = Application.StartupPath,
                    FileName = str
                };
                if (dialog.ShowDialog() != DialogResult.OK)
                {
                    return "";
                }
                this.WcrFileName = dialog.FileName;
                dialog.Dispose();
            }
            this.WcrXml.Save(this.WcrFileName);
            return this.WcrFileName;
        }

        public void SetCookie(string CookieStr)
        {
            try
            {
                if (!string.IsNullOrEmpty(CookieStr) && !string.IsNullOrEmpty(this._HTTPRoot))
                {
                    Uri uri = new Uri(this._HTTPRoot);
                    string str = "";
                    foreach (string str2 in CookieStr.Split(new char[] { ';' }))
                    {
                        string[] strArray2 = str2.Split(new char[] { '=' });
                        string lbszCookieName = strArray2[0];
                        string str4 = "";
                        for (int i = 1; i < strArray2.Length; i++)
                        {
                            if (string.IsNullOrEmpty(str4))
                            {
                                str4 = str4 + strArray2[i];
                            }
                            else
                            {
                                str4 = str4 + "=" + strArray2[i];
                            }
                        }
                        lbszCookieName = lbszCookieName.Trim();
                        if (!string.IsNullOrEmpty(str4))
                        {
                            str4 = GlobalObject.unescape(str4.Trim());
                            if (EscapeCookie)
                            {
                                str4 = GlobalObject.escape(str4);
                            }
                            else if ((str4.IndexOf("\"") != 0) && ((str4.IndexOf('\'') > 0) || (str4.IndexOf(' ') > 0)))
                            {
                                str4 = "\"" + str4 + "\"";
                            }
                        }
                        InternetSetCookie(this.HTTPRoot, lbszCookieName, str4);
                        if (!string.IsNullOrEmpty(str))
                        {
                            str = str + ",";
                        }
                        str = str + lbszCookieName + "=" + str4;
                    }
                    this.cc.SetCookies(uri, str);
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        public void SetSingleCookie(string CookieStr)
        {
            try
            {
                if (!string.IsNullOrEmpty(CookieStr) && !string.IsNullOrEmpty(this._HTTPRoot))
                {
                    Uri uri = new Uri(this._HTTPRoot);
                    string str = "";
                    CookieStr.Split(new char[] { ';' });
                    string[] strArray = CookieStr.Split(new char[] { '=' });
                    string lbszCookieName = strArray[0];
                    string str3 = "";
                    for (int i = 1; i < strArray.Length; i++)
                    {
                        if (string.IsNullOrEmpty(str3))
                        {
                            str3 = str3 + strArray[i];
                        }
                        else
                        {
                            str3 = str3 + "=" + strArray[i];
                        }
                    }
                    lbszCookieName = lbszCookieName.Trim();
                    if (!string.IsNullOrEmpty(str3))
                    {
                        str3 = GlobalObject.unescape(str3.Trim());
                        if (EscapeCookie)
                        {
                            str3 = GlobalObject.escape(str3);
                        }
                        else if ((str3.IndexOf("\"") != 0) && ((str3.IndexOf('\'') > 0) || (str3.IndexOf(' ') > 0)))
                        {
                            str3 = "\"" + str3 + "\"";
                        }
                    }
                    InternetSetCookie(this.HTTPRoot, lbszCookieName, str3);
                    if (!string.IsNullOrEmpty(str))
                    {
                        str = str + ",";
                    }
                    str = str + lbszCookieName + "=" + str3;
                    this.cc.SetCookies(uri, str);
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        public static string URL2DistinctURL(string sURL)
        {
            if (sURL.IndexOf('?') > 0)
            {
                string[] strArray = sURL.Split(new char[] { '?' });
                string input = strArray[0];
                if (strArray.Length >= 2)
                {
                    foreach (string str2 in strArray[1].Split(new char[] { '&' }))
                    {
                        string[] strArray3 = str2.Split(new char[] { '=' });
                        if ((strArray3.Length > 1) && !string.IsNullOrEmpty(strArray3[1]))
                        {
                            if (strArray3[1].IndexOf('#') > 0)
                            {
                                strArray3[1] = strArray3[1].Substring(0, strArray3[1].IndexOf('#'));
                            }
                            if (Regex.IsMatch(strArray3[1], @"^\d+$"))
                            {
                                input = input + "?" + strArray3[0];
                            }
                            else
                            {
                                input = input + "?" + str2;
                            }
                        }
                        else
                        {
                            input = input + "?" + strArray3[0];
                        }
                    }
                }
                Regex regex = new Regex(@",\d+$", RegexOptions.Singleline);
                string str3 = regex.Match(input).Value;
                if (!string.IsNullOrEmpty(str3))
                {
                    input = input.Replace(str3, ",");
                }
                return input;
            }
            return sURL.Split(new char[] { '^' })[0];
        }

        public static string URL2NoParaURL(string sURL)
        {
            if (sURL.IndexOf('^') > 0)
            {
                sURL = sURL.Split(new char[] { '^' })[0];
            }
            if (sURL.IndexOf('?') > 0)
            {
                sURL = sURL.Split(new char[] { '?' })[0];
            }
            return sURL;
        }

        public BlindType BlindInjType
        {
            get
            {
                return this._BlindInjType;
            }
            set
            {
                this._BlindInjType = value;
            }
        }

        public string CurrentKeyWord
        {
            get
            {
                return this.KeyWord;
            }
            set
            {
                this.KeyWord = value;
            }
        }

        public DBType DatabaseType
        {
            get
            {
                return this._DatabaseType;
            }
            set
            {
                this._DatabaseType = value;
            }
        }

        public string DomainHost
        {
            get
            {
                return this._DomainHost;
            }
        }

        public string HTTPRoot
        {
            get
            {
                return this._HTTPRoot;
            }
        }

        public int HTTPThreadNum
        {
            get
            {
                return this._HTTPThreadNum;
            }
            set
            {
                this._HTTPThreadNum = value;
            }
        }

        public InjectionType InjType
        {
            get
            {
                return this._InjType;
            }
            set
            {
                this._InjType = value;
            }
        }

        public int NeedEscapeSpace
        {
            get
            {
                return this._NeedEscapeSpace;
            }
            set
            {
                this._NeedEscapeSpace = value;
            }
        }

        public Uri URI
        {
            get
            {
                return this.strUri;
            }
        }

        public string URL
        {
            get
            {
                return this._URL;
            }
            set
            {
                this._URL = value;
                this.InitDomain();
            }
        }
    }
}

