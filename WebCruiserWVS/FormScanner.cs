namespace WebCruiserWVS
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Net;
    using System.Runtime.CompilerServices;
    using System.Text.RegularExpressions;
    using System.Threading;
    using System.Timers;
    using System.Web.UI;
    using System.Windows.Forms;
    using System.Xml;

    public class FormScanner : Form
    {
        private ColumnHeader columnKeyWord;
        private ColumnHeader columnParameter;
        private ColumnHeader columnType;
        private ColumnHeader columnURL;
        private ColumnHeader columnVul;
        private IContainer components;
        private ImageList ImageListScanner;
        private bool IsMultiScanSubForm;
        private ToolStripStatusLabel lblProgress;
        private ToolStripStatusLabel lblThreadNum;
        private ListView listViewWVS;
        private FormMain mainfrm;
        private int ScanDepth;
        private System.Timers.Timer ScannerTimer;
        private WebSite SiteA;
        private SplitContainer splitScanner;
        private StatusStrip statusScanner;
        private ToolStripButton toolStripClearWVS;
        private ToolStripButton toolStripExpWVS;
        private ToolStripButton toolStripImpWVS;
        private ToolStripButton toolStripMultiScan;
        private ToolStrip toolStripScanner;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripSeparator toolStripSeparator4;
        private ToolStripSeparator toolStripSeparator5;
        private ToolStripSeparator toolStripSeparator6;
        private ToolStripSeparator toolStripSeparator7;
        private ToolStripStatusLabel toolStripStatusLabel1;
        private ToolStripButton toolStripURLScan;
        private ToolStripButton toolStripWVScan;
        private List<string> TreeNodeURL;
        private TreeView treeViewWVS;

        public FormScanner(FormMain fm)
        {
            this.TreeNodeURL = new List<string>();
            this.InitializeComponent();
            this.mainfrm = fm;
            this.SiteA = this.mainfrm.CurrentSite;
            this.InitSystemTimer();
        }

        public FormScanner(FormMain fm, WebSite wsite)
        {
            this.TreeNodeURL = new List<string>();
            this.InitializeComponent();
            this.mainfrm = fm;
            this.IsMultiScanSubForm = true;
            WebSite.MultiProcessNum++;
            this.toolStripScanner.Enabled = false;
            base.FormBorderStyle = FormBorderStyle.Sizable;
            this.SiteA = wsite;
            this.Text = this.SiteA.URL + " - Scanner";
            this.InitSystemTimer();
            if (!string.IsNullOrEmpty(this.SiteA.URL))
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(this.AutoSiteScan));
            }
        }

        public void AddItem2ListViewWVS(string Text)
        {
            if (!this.listViewWVS.InvokeRequired)
            {
                string[] separator = new string[] { "^^" };
                string[] strArray2 = Text.Split(separator, StringSplitOptions.None);
                ListViewItem item = this.listViewWVS.Items.Add(strArray2[0]);
                item.ImageKey = "vul.png";
                for (int i = 1; i < strArray2.Length; i++)
                {
                    item.SubItems.Add(strArray2[i]);
                }
                this.listViewWVS.Refresh();
            }
            else
            {
                dd method = new dd(this.AddItem2ListViewWVS);
                base.Invoke(method, new object[] { Text });
            }
        }

        private void AddItem2TreeViewWVS(string sURL)
        {
            if (!string.IsNullOrEmpty(sURL))
            {
                if (sURL.IndexOf('^') > 0)
                {
                    sURL = sURL.Split(new char[] { '^' })[0];
                }
                try
                {
                    sURL = sURL.Replace("/./", "/");
                    Uri uri = new Uri(sURL);
                    if (uri.Host.Equals(this.SiteA.DomainHost))
                    {
                        if (!this.treeViewWVS.InvokeRequired)
                        {
                            int startIndex = sURL.IndexOf('/', 9) + 1;
                            if (startIndex != 0)
                            {
                                string[] strArray2 = sURL.Substring(startIndex).Split(new char[] { '?' });
                                string[] strArray3 = strArray2[0].Split(new char[] { '/' });
                                string str = "";
                                if (strArray2.Length > 1)
                                {
                                    string[] strArray4;
                                    IntPtr ptr;
                                    for (int j = 1; j < strArray2.Length; j++)
                                    {
                                        if (!string.IsNullOrEmpty(str))
                                        {
                                            str = str + "?";
                                        }
                                        str = str + strArray2[j];
                                    }
                                    (strArray4 = strArray3)[(int) (ptr = (IntPtr) (strArray3.Length - 1))] = strArray4[(int) ptr] + "?" + str;
                                }
                                TreeNode node = new TreeNode();
                                TreeNode node2 = new TreeNode();
                                for (int i = 0; i < strArray3.Length; i++)
                                {
                                    if (string.IsNullOrEmpty(strArray3[i]))
                                    {
                                        return;
                                    }
                                    if (i == 0)
                                    {
                                        node = this.ContainsTreeNode(this.treeViewWVS.Nodes, strArray3[0]);
                                        if (node == null)
                                        {
                                            node = this.treeViewWVS.Nodes.Add(strArray3[0]);
                                        }
                                    }
                                    else
                                    {
                                        node2 = this.ContainsTreeNode(node.Nodes, strArray3[i]);
                                        if (node2 == null)
                                        {
                                            node2 = node.Nodes.Add(strArray3[i]);
                                        }
                                        node = node2;
                                    }
                                }
                                this.treeViewWVS.ExpandAll();
                                this.treeViewWVS.Refresh();
                            }
                        }
                        else
                        {
                            dd method = new dd(this.AddItem2TreeViewWVS);
                            base.Invoke(method, new object[] { sURL });
                        }
                    }
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message);
                }
            }
        }

        private void AutoSiteScan(object data)
        {
            try
            {
                this.BeginSiteScan();
                bool flag = false;
                while (!flag)
                {
                    Thread.Sleep(0x3e8);
                    if ((DateTime.Now.Subtract(this.SiteA.LastGetTime).Seconds > 20) && (this.SiteA.HTTPThreadNum == 0))
                    {
                        flag = true;
                    }
                }
                if (this.IsMultiScanSubForm)
                {
                    WebSite.MultiProcessNum--;
                }
                this.ExportWVS(this.SiteA.DomainHost + "_Vuls_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xml");
                XmlDocument xmlDocumentFromWVS = this.GetXmlDocumentFromWVS();
                this.GenReport(xmlDocumentFromWVS);
                this.mainfrm.DisposeSubForm(this);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void AutoSiteScanControl(object data)
        {
            string str = (string) data;
            string[] separator = new string[] { "^^" };
            string[] strArray2 = str.Split(separator, StringSplitOptions.None);
            for (int i = 0; i < strArray2.Length; i++)
            {
                string str2 = strArray2[i].ToString().Trim();
                if (!string.IsNullOrEmpty(str2) && (str2.IndexOf("http") >= 0))
                {
                    goto Label_005B;
                }
                continue;
            Label_0051:
                Thread.Sleep(0x3e8);
            Label_005B:
                if (WebSite.MultiProcessNum >= WCRSetting.MultiSitesNum)
                {
                    goto Label_0051;
                }
                WebSite wsite = new WebSite(str2);
                FormScanner scanner = new FormScanner(this.mainfrm, wsite);
                MethodInvoker method = new MethodInvoker(scanner.Show);
                base.Invoke(method, null);
            }
        }

        private void BeginSiteScan()
        {
            if (!this.IsMultiScanSubForm)
            {
                this.SiteA.URL = this.mainfrm.URL;
            }
            if (string.IsNullOrEmpty(this.SiteA.URL))
            {
                MessageBox.Show("Null URL!", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                this.AddItem2TreeViewWVS(this.SiteA.URL);
                this.DisplayProgress("Scanning...");
                ThreadPool.QueueUserWorkItem(new WaitCallback(this.CheckRobots));
                ThreadPool.QueueUserWorkItem(new WaitCallback(this.CheckSiteMapXML));
                ThreadPool.QueueUserWorkItem(new WaitCallback(this.CrawlPage), this.SiteA.URL);
                
                new Thread(new ParameterizedThreadStart(delegate {
                    for (int j = 0; j < WCRSetting.ScanDepth; j++)
                    {
                        while (this.SiteA.HTTPThreadNum > 0)
                        {
                            Thread.Sleep(200);
                        }
                        if (WebSite.CurrentStatus == TaskStatus.Stop)
                        {
                            return;
                        }
                        if (j == 0)
                        {
                            ThreadPool.QueueUserWorkItem(new WaitCallback(this.CheckVulnerability), this.SiteA.URL);
                        }
                        this.ScanDepth = j + 1;
                        Thread.Sleep(0x3e8);
                        this.CrawlTreeViewWVS();
                        if (WebSite.CurrentStatus == TaskStatus.Stop)
                        {
                            return;
                        }
                    }
                }));
            }
        }

        private void CheckRobots(object data)
        {
            try
            {
                string sURL = this.SiteA.HTTPRoot + "robots.txt";
                HttpWebResponse httpWebResponse = this.SiteA.GetHttpWebResponse(sURL, RequestType.GET);
                if (httpWebResponse.StatusCode == HttpStatusCode.OK)
                {
                    string sourceCodeFromHttpWebResponse = this.mainfrm.CurrentSite.GetSourceCodeFromHttpWebResponse(httpWebResponse);
                    Regex regex = new Regex(@"(?<=allow:\s+)[^\s?*$]+", RegexOptions.Singleline | RegexOptions.IgnoreCase);
                    foreach (Match match in regex.Matches(sourceCodeFromHttpWebResponse))
                    {
                        string str3 = match.Value;
                        if (str3.Length > 1)
                        {
                            str3 = this.SiteA.HTTPRoot + str3.Substring(1);
                            this.AddItem2TreeViewWVS(str3);
                        }
                    }
                }
            }
            catch
            {
            }
        }

        private void CheckSiteMapXML(object data)
        {
            try
            {
                string sURL = this.SiteA.HTTPRoot + "sitemap.xml";
                HttpWebResponse httpWebResponse = this.SiteA.GetHttpWebResponse(sURL, RequestType.GET);
                if (httpWebResponse.StatusCode == HttpStatusCode.OK)
                {
                    string sourceCodeFromHttpWebResponse = this.mainfrm.CurrentSite.GetSourceCodeFromHttpWebResponse(httpWebResponse);
                    Regex regex = new Regex(@"(?<=<loc>)[\S]+(?=<\/loc>)", RegexOptions.Singleline | RegexOptions.IgnoreCase);
                    foreach (Match match in regex.Matches(sourceCodeFromHttpWebResponse))
                    {
                        string str3 = match.Value;
                        if (str3.Length > 1)
                        {
                            str3 = str3.Replace("&amp;", "&");
                            this.AddItem2TreeViewWVS(str3);
                        }
                    }
                }
            }
            catch
            {
            }
        }

        private void CheckVulnerability(object data)
        {
            try
            {
                if (WebSite.CurrentStatus == TaskStatus.Stop)
                {
                    Thread.CurrentThread.Abort();
                }
                string sURL = (string) data;
                if (!this.SiteA.HasScannedURL(sURL))
                {
                    this.SiteA.AddScannedURL(WebSite.URL2DistinctURL(sURL));
                    if (!this.SiteA.GetFileExt(sURL).Equals(".js"))
                    {
                        if (sURL.IndexOf('=') > 0)
                        {
                            this.DisplayProgress("Checking SQL Injection: " + sURL);
                            foreach (string str2 in this.SiteA.GetInjectableURLDesc(sURL, RequestType.GET, ""))
                            {
                                this.AddItem2ListViewWVS(str2);
                            }
                        }
                        if (WebSite.CurrentStatus == TaskStatus.Stop)
                        {
                            Thread.CurrentThread.Abort();
                        }
                        if (WCRSetting.ScanXSS)
                        {
                            this.DisplayProgress("Checking URL XSS: " + sURL);
                            foreach (string str3 in this.GetXSSURLInfo(sURL))
                            {
                                if (!string.IsNullOrEmpty(str3))
                                {
                                    this.AddItem2ListViewWVS(str3);
                                }
                            }
                        }
                        if (WebSite.CurrentStatus == TaskStatus.Stop)
                        {
                            Thread.CurrentThread.Abort();
                        }
                        this.DisplayProgress("Checking Form Vul: " + sURL);
                        foreach (string str4 in this.SiteA.GetFormVuls(sURL))
                        {
                            if (!string.IsNullOrEmpty(str4))
                            {
                                this.AddItem2ListViewWVS(str4);
                            }
                        }
                    }
                }
            }
            catch
            {
            }
        }

        private void CloneTreeView(TreeView SourceTree, TreeView DestTree)
        {
            if (!this.treeViewWVS.InvokeRequired)
            {
                DestTree.Nodes.Clear();
                foreach (TreeNode node in SourceTree.Nodes)
                {
                    DestTree.Nodes.Add((TreeNode) node.Clone());
                }
                DestTree.ExpandAll();
            }
            else
            {
                ddc method = new ddc(this.CloneTreeView);
                base.Invoke(method, new object[] { SourceTree, DestTree });
            }
        }

        private TreeNode ContainsTreeNode(TreeNodeCollection tn, string Text)
        {
            string str = Text;
            string text = "";
            if (Text.IndexOf('?') >= 0)
            {
                string[] strArray = Text.Split(new char[] { '?' });
                str = strArray[0];
                if (strArray.Length >= 2)
                {
                    foreach (string str3 in strArray[1].Split(new char[] { '&' }))
                    {
                        string[] strArray3 = str3.Split(new char[] { '=' });
                        str = str + "?" + strArray3[0];
                    }
                }
            }
            for (int i = 0; i < tn.Count; i++)
            {
                text = tn[i].Text;
                if (text.IndexOf('?') >= 0)
                {
                    string[] strArray4 = text.Split(new char[] { '?' });
                    if (strArray4.Length < 2)
                    {
                        text = strArray4[0];
                    }
                    else
                    {
                        text = strArray4[0];
                        foreach (string str4 in strArray4[1].Split(new char[] { '&' }))
                        {
                            string[] strArray6 = str4.Split(new char[] { '=' });
                            text = text + "?" + strArray6[0];
                        }
                    }
                }
                if (text.Equals(str))
                {
                    return tn[i];
                }
            }
            return null;
        }

        private void CrawlPage(object Data)
        {
            if (WebSite.CurrentStatus != TaskStatus.Stop)
            {
                try
                {
                    string sURL = (string) Data;
                    if (!this.SiteA.HasCrawledURL(sURL))
                    {
                        this.SiteA.AddCrawledURL(WebSite.URL2DistinctURL(sURL));
                        if (this.IsWebPage(sURL))
                        {
                            string fileExt = this.SiteA.GetFileExt(sURL);
                            this.DisplayProgress("Depth: " + this.ScanDepth.ToString() + "  Scanning: " + sURL);
                            HttpWebResponse httpWebResponse = this.SiteA.GetHttpWebResponse(sURL, RequestType.GET);
                            if (httpWebResponse != null)
                            {
                                string sourceCodeFromHttpWebResponse = this.mainfrm.CurrentSite.GetSourceCodeFromHttpWebResponse(httpWebResponse);
                                string str4 = httpWebResponse.ResponseUri.ToString();
                                if (!str4.Equals(sURL))
                                {
                                    this.SiteA.AddCrawledURL(WebSite.URL2DistinctURL(str4));
                                }
                                string pathFromURL = WebSite.GetPathFromURL(str4);
                                string[] strArray = this.SiteA.GetLinksFromSource(sourceCodeFromHttpWebResponse, pathFromURL, fileExt);
                                string uriString = "";
                                for (int i = 0; i < strArray.Length; i++)
                                {
                                    if (WebSite.CurrentStatus == TaskStatus.Stop)
                                    {
                                        return;
                                    }
                                    uriString = strArray[i];
                                    Uri uri = new Uri(uriString);
                                    if (uri.Host.Equals(this.SiteA.DomainHost))
                                    {
                                        this.AddItem2TreeViewWVS(uriString);
                                        this.AddItem2TreeViewWVS(WebSite.URL2NoParaURL(uriString));
                                        if (!this.SiteA.HasScannedURL(uriString) && this.IsWebPage(uriString))
                                        {
                                            ThreadPool.QueueUserWorkItem(new WaitCallback(this.CheckVulnerability), uriString);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch
                {
                }
            }
        }

        private void CrawlTreeViewWVS()
        {
            try
            {
                this.TreeNodeURL.Clear();
                TreeView destTree = new TreeView();
                this.CloneTreeView(this.treeViewWVS, destTree);
                foreach (TreeNode node in destTree.Nodes)
                {
                    if (node.Nodes.Count == 0)
                    {
                        string sURL = this.SiteA.HTTPRoot + node.Text;
                        if (this.IsWebPage(sURL))
                        {
                            this.TreeNodeURL.Add(sURL);
                        }
                    }
                    else
                    {
                        this.TreeNode2URLS(this.SiteA.HTTPRoot, node);
                    }
                }
                for (int i = 0; i < this.TreeNodeURL.Count; i++)
                {
                    if (WebSite.CurrentStatus == TaskStatus.Stop)
                    {
                        Thread.CurrentThread.Abort();
                    }
                    if (this.IsWebPage(this.TreeNodeURL[i]))
                    {
                        this.CrawlPage(this.TreeNodeURL[i]);
                    }
                }
            }
            catch
            {
            }
        }

        public void DisplayProgress(string Text)
        {
            if (!this.statusScanner.InvokeRequired)
            {
                this.lblProgress.Text = Text;
                this.statusScanner.Refresh();
            }
            else
            {
                dd method = new dd(this.DisplayProgress);
                base.Invoke(method, new object[] { Text });
            }
        }

        public void DisplayThreadNum(string Text)
        {
            if (!this.statusScanner.InvokeRequired)
            {
                this.lblThreadNum.Text = Text;
                this.statusScanner.Refresh();
            }
            else
            {
                dd method = new dd(this.DisplayThreadNum);
                base.Invoke(method, new object[] { Text });
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        public void EnableFunc(bool RegOK)
        {
            this.toolStripWVScan.Enabled = RegOK;
        }

        private void ExportWVS(string WVSXmlFileName)
        {
            try
            {
                XmlDocument xmlDocumentFromWVS = this.GetXmlDocumentFromWVS();
                for (int i = 1; System.IO.File.Exists(Application.StartupPath + @"\" + WVSXmlFileName); i++)
                {
                    WVSXmlFileName = i.ToString() + "_" + WVSXmlFileName;
                }
                xmlDocumentFromWVS.Save(WVSXmlFileName);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void FormScanner_Resize(object sender, EventArgs e)
        {
            this.lblProgress.Width = this.toolStripScanner.Width - 150;
        }

        private string GenReport(XmlDocument WcrXml)
        {
            string path = this.SiteA.DomainHost + "_Scan_Report_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".html";
            StringWriter writer = new StringWriter();
            try
            {
                HtmlTextWriter writer2 = new HtmlTextWriter(writer);
                writer2.RenderBeginTag(HtmlTextWriterTag.Html);
                writer2.RenderBeginTag(HtmlTextWriterTag.Head);
                writer2.Write("<meta http-equiv=\"content-type\" content=\"text/html; charset=UTF-8\">");
                writer2.Write("<style>table{table-layout:fixed;overflow:hidden;}</style>");
                writer2.RenderBeginTag(HtmlTextWriterTag.Title);
                writer2.Write("Scan Report");
                writer2.RenderEndTag();
                writer2.RenderEndTag();
                writer2.RenderBeginTag(HtmlTextWriterTag.Body);
                writer2.RenderBeginTag(HtmlTextWriterTag.Center);
                writer2.Write("<br><br><br><br><br><br><br><br>");
                writer2.RenderBeginTag(HtmlTextWriterTag.H1);
                writer2.Write(this.SiteA.DomainHost + " Scan Report<br>");
                writer2.RenderEndTag();
                writer2.Write("<br><br><br><br><br><br><br><br>Created By WebCruiser - Web Vulnerability Scanner<br>" + DateTime.Now.ToString("yyyy-MM-dd"));
                writer2.Write("<div style=\"page-break-after:always\">&nbsp;</div>");
                XmlNodeList list = WcrXml.SelectNodes("//ROOT/SiteVulList/VulRow");
                if (list.Count > 0)
                {
                    writer2.RenderBeginTag(HtmlTextWriterTag.H2);
                    writer2.Write("Vulnerability Result");
                    writer2.RenderEndTag();
                    for (int i = 0; i < list.Count; i++)
                    {
                        writer2.AddAttribute("border", "1");
                        writer2.AddAttribute("width", "640");
                        writer2.AddAttribute("cellspacing", "0");
                        writer2.AddAttribute("bordercolordark", "009099");
                        writer2.RenderBeginTag(HtmlTextWriterTag.Table);
                        writer2.Write("<tr><td width=\"150\">No.</td><td>" + ((i + 1)).ToString() + "</td></tr>");
                        XmlNode node = list[i];
                        for (int j = 0; j < node.ChildNodes.Count; j++)
                        {
                            writer2.Write("<tr><td width=\"150\">");
                            writer2.Write(node.ChildNodes[j].Name);
                            writer2.Write("</td><td>");
                            writer2.Write(node.ChildNodes[j].InnerText.Replace("<>", "&lt;&gt;"));
                            writer2.Write("</td></tr>");
                        }
                        writer2.RenderEndTag();
                        writer2.Write("<br>");
                    }
                }
                writer2.RenderEndTag();
                writer2.RenderEndTag();
                writer2.RenderEndTag();
                System.IO.File.WriteAllText(path, writer.ToString());
                return path;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
                System.IO.File.WriteAllText(path, writer.ToString());
                return path;
            }
        }

        private ListViewItem GetListViewItem(int Index)
        {
            if (!this.listViewWVS.InvokeRequired)
            {
                return this.listViewWVS.Items[Index];
            }
            dl method = new dl(this.GetListViewItem);
            return (ListViewItem) base.Invoke(method, new object[] { Index });
        }

        public XmlDocument GetXmlDocumentFromDirTree()
        {
            XmlDocument wVSXml = new XmlDocument();
            XmlNode newChild = wVSXml.CreateXmlDeclaration("1.0", "utf-8", "");
            wVSXml.AppendChild(newChild);
            XmlElement element = wVSXml.CreateElement("ROOT");
            wVSXml.AppendChild(element);
            XmlElement element2 = wVSXml.CreateElement("SiteDirTree");
            element.AppendChild(element2);
            foreach (TreeNode node2 in this.treeViewWVS.Nodes)
            {
                wVSXml = this.TreeNode2XmlElement(wVSXml, element2, node2);
            }
            return wVSXml;
        }

        public XmlDocument GetXmlDocumentFromWVS()
        {
            XmlDocument document = new XmlDocument();
            XmlNode newChild = document.CreateXmlDeclaration("1.0", "utf-8", "");
            document.AppendChild(newChild);
            XmlElement element = document.CreateElement("ROOT");
            document.AppendChild(element);
            XmlElement element2 = document.CreateElement("SiteVulList");
            element.AppendChild(element2);
            for (int i = 0; i < this.listViewWVS.Items.Count; i++)
            {
                ListViewItem listViewItem = this.GetListViewItem(i);
                XmlElement element3 = document.CreateElement("VulRow");
                XmlElement element4 = document.CreateElement("ReferURL");
                element4.InnerText = listViewItem.SubItems[0].Text;
                element3.AppendChild(element4);
                element4 = document.CreateElement("Parameter");
                element4.InnerText = listViewItem.SubItems[1].Text;
                element3.AppendChild(element4);
                element4 = document.CreateElement("Type");
                element4.InnerText = listViewItem.SubItems[2].Text;
                element3.AppendChild(element4);
                element4 = document.CreateElement("KWordActionURL");
                element4.InnerText = listViewItem.SubItems[3].Text;
                element3.AppendChild(element4);
                element4 = document.CreateElement("Vulnerability");
                element4.InnerText = listViewItem.SubItems[4].Text;
                element3.AppendChild(element4);
                element2.AppendChild(element3);
            }
            return document;
        }

        private string[] GetXSSURLInfo(string sURL)
        {
            List<string> list = new List<string>();
            if (WebSite.CurrentStatus != TaskStatus.Stop)
            {
                string[] strArray = sURL.Split(new char[] { '?' });
                if (strArray.Length < 2)
                {
                    return list.ToArray();
                }
                string[] strArray2 = strArray[1].Split(new char[] { '&' });
                for (int i = 0; i < strArray2.Length; i++)
                {
                    string uRL = strArray[0];
                    string str2 = "";
                    for (int j = 0; j < i; j++)
                    {
                        if (!string.IsNullOrEmpty(str2))
                        {
                            str2 = str2 + "&";
                        }
                        str2 = str2 + strArray2[j];
                    }
                    string str3 = strArray2[i].Split(new char[] { '=' })[0];
                    string uRLPara = WebSite.URL2NoParaURL(sURL) + "^" + str3.ToLower() + "^XSS";
                    if (!this.SiteA.IsScannedParameter(uRLPara))
                    {
                        this.SiteA.AddScannedParameter(uRLPara);
                        if (!string.IsNullOrEmpty(str2))
                        {
                            str2 = str2 + "&";
                        }
                        str2 = str2 + str3 + "=" + WebSite.GenerateTestInput(i, "<>%3c%3e%253c%253e");
                        for (int k = i + 1; k < strArray2.Length; k++)
                        {
                            if (!string.IsNullOrEmpty(str2))
                            {
                                str2 = str2 + "&";
                            }
                            str2 = str2 + strArray2[k];
                        }
                        uRL = uRL + "?" + str2;
                        string keyTextFromSource = WebSite.GetKeyTextFromSource(this.SiteA.GetSourceCode(uRL, RequestType.GET), i);
                        if (!string.IsNullOrEmpty(keyTextFromSource) && (keyTextFromSource.IndexOf("<>") >= 0))
                        {
                            string str7 = WebSite.RemoveTestInput(uRL);
                            string item = sURL + "^^" + str3 + "^^GET^^" + str7 + "^^Cross Site Scripting(URL)";
                            list.Add(item);
                        }
                    }
                }
            }
            return list.ToArray();
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormScanner));
            this.splitScanner = new SplitContainer();
            this.treeViewWVS = new TreeView();
            this.ImageListScanner = new ImageList(this.components);
            this.toolStripScanner = new ToolStrip();
            this.toolStripSeparator7 = new ToolStripSeparator();
            this.toolStripWVScan = new ToolStripButton();
            this.toolStripSeparator1 = new ToolStripSeparator();
            this.toolStripURLScan = new ToolStripButton();
            this.toolStripSeparator2 = new ToolStripSeparator();
            this.toolStripMultiScan = new ToolStripButton();
            this.toolStripSeparator3 = new ToolStripSeparator();
            this.toolStripClearWVS = new ToolStripButton();
            this.toolStripSeparator4 = new ToolStripSeparator();
            this.toolStripImpWVS = new ToolStripButton();
            this.toolStripSeparator5 = new ToolStripSeparator();
            this.toolStripExpWVS = new ToolStripButton();
            this.toolStripSeparator6 = new ToolStripSeparator();
            this.statusScanner = new StatusStrip();
            this.lblProgress = new ToolStripStatusLabel();
            this.toolStripStatusLabel1 = new ToolStripStatusLabel();
            this.lblThreadNum = new ToolStripStatusLabel();
            this.columnURL = new ColumnHeader();
            this.columnParameter = new ColumnHeader();
            this.columnType = new ColumnHeader();
            this.columnKeyWord = new ColumnHeader();
            this.columnVul = new ColumnHeader();
            this.listViewWVS = new ListView();
            this.splitScanner.Panel1.SuspendLayout();
            this.splitScanner.Panel2.SuspendLayout();
            this.splitScanner.SuspendLayout();
            this.toolStripScanner.SuspendLayout();
            this.statusScanner.SuspendLayout();
            base.SuspendLayout();
            this.splitScanner.Dock = DockStyle.Fill;
            this.splitScanner.Location = new Point(0, 0x19);
            this.splitScanner.Name = "splitScanner";
            this.splitScanner.Orientation = Orientation.Horizontal;
            this.splitScanner.Panel1.Controls.Add(this.treeViewWVS);
            this.splitScanner.Panel2.Controls.Add(this.listViewWVS);
            this.splitScanner.Size = new Size(0x282, 0x169);
            this.splitScanner.SplitterDistance = 0xaf;
            this.splitScanner.TabIndex = 0;
            this.treeViewWVS.Dock = DockStyle.Fill;
            this.treeViewWVS.FullRowSelect = true;
            this.treeViewWVS.Location = new Point(0, 0);
            this.treeViewWVS.Name = "treeViewWVS";
            this.treeViewWVS.Size = new Size(0x282, 0xaf);
            this.treeViewWVS.TabIndex = 5;
            this.treeViewWVS.NodeMouseClick += new TreeNodeMouseClickEventHandler(this.treeViewWVS_NodeMouseClick);
            this.ImageListScanner.ImageStream = (ImageListStreamer)resources.GetObject("ImageListScanner.ImageStream");
            this.ImageListScanner.TransparentColor = Color.Transparent;
            this.ImageListScanner.Images.SetKeyName(0, "vul.png");
            this.toolStripScanner.BackColor = SystemColors.ButtonFace;
            this.toolStripScanner.GripStyle = ToolStripGripStyle.Hidden;
            this.toolStripScanner.Items.AddRange(new ToolStripItem[] { this.toolStripSeparator7, this.toolStripWVScan, this.toolStripSeparator1, this.toolStripURLScan, this.toolStripSeparator2, this.toolStripMultiScan, this.toolStripSeparator3, this.toolStripClearWVS, this.toolStripSeparator4, this.toolStripImpWVS, this.toolStripSeparator5, this.toolStripExpWVS, this.toolStripSeparator6 });
            this.toolStripScanner.LayoutStyle = ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.toolStripScanner.Location = new Point(0, 0);
            this.toolStripScanner.Name = "toolStripScanner";
            this.toolStripScanner.Size = new Size(0x282, 0x19);
            this.toolStripScanner.TabIndex = 1;
            this.toolStripScanner.Text = "toolStrip1";
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new Size(6, 0x19);
            this.toolStripWVScan.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.toolStripWVScan.Image = (Image)resources.GetObject("toolStripWVScan.Image");
            this.toolStripWVScan.ImageTransparentColor = Color.Magenta;
            this.toolStripWVScan.Name = "toolStripWVScan";
            this.toolStripWVScan.Size = new Size(0x6f, 0x16);
            this.toolStripWVScan.Text = "Scan Current Site";
            this.toolStripWVScan.Click += new EventHandler(this.toolStripWVScan_Click);
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new Size(6, 0x19);
            this.toolStripURLScan.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.toolStripURLScan.Image = (Image)resources.GetObject("toolStripURLScan.Image");
            this.toolStripURLScan.ImageTransparentColor = Color.Magenta;
            this.toolStripURLScan.Name = "toolStripURLScan";
            this.toolStripURLScan.Size = new Size(0x69, 0x16);
            this.toolStripURLScan.Text = "Scan Current URL";
            this.toolStripURLScan.Click += new EventHandler(this.toolStripURLScan_Click);
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new Size(6, 0x19);
            this.toolStripMultiScan.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.toolStripMultiScan.Image = (Image)resources.GetObject("toolStripMultiScan.Image");
            this.toolStripMultiScan.ImageTransparentColor = Color.Magenta;
            this.toolStripMultiScan.Name = "toolStripMultiScan";
            this.toolStripMultiScan.Size = new Size(0x63, 0x16);
            this.toolStripMultiScan.Text = "Scan Multi-Site";
            this.toolStripMultiScan.Click += new EventHandler(this.toolStripMultiScan_Click);
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new Size(6, 0x19);
            this.toolStripClearWVS.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.toolStripClearWVS.Image = (Image)resources.GetObject("toolStripClearWVS.Image");
            this.toolStripClearWVS.ImageTransparentColor = Color.Magenta;
            this.toolStripClearWVS.Name = "toolStripClearWVS";
            this.toolStripClearWVS.Size = new Size(0x7b, 0x16);
            this.toolStripClearWVS.Text = "Reset/Clear Scanner";
            this.toolStripClearWVS.Click += new EventHandler(this.toolStripClearWVS_Click);
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new Size(6, 0x19);
            this.toolStripImpWVS.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.toolStripImpWVS.Image = (Image)resources.GetObject("toolStripImpWVS.Image");
            this.toolStripImpWVS.ImageTransparentColor = Color.Magenta;
            this.toolStripImpWVS.Name = "toolStripImpWVS";
            this.toolStripImpWVS.Size = new Size(0x2d, 0x16);
            this.toolStripImpWVS.Text = "Import";
            this.toolStripImpWVS.Click += new EventHandler(this.toolStripImpWVS_Click);
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new Size(6, 0x19);
            this.toolStripExpWVS.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.toolStripExpWVS.Image = (Image)resources.GetObject("toolStripExpWVS.Image");
            this.toolStripExpWVS.ImageTransparentColor = Color.Magenta;
            this.toolStripExpWVS.Name = "toolStripExpWVS";
            this.toolStripExpWVS.Size = new Size(0x2d, 0x16);
            this.toolStripExpWVS.Text = "Export";
            this.toolStripExpWVS.Click += new EventHandler(this.toolStripExpWVS_Click);
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new Size(6, 0x19);
            this.statusScanner.Items.AddRange(new ToolStripItem[] { this.lblProgress, this.toolStripStatusLabel1, this.lblThreadNum });
            this.statusScanner.Location = new Point(0, 0x16c);
            this.statusScanner.Name = "statusScanner";
            this.statusScanner.Size = new Size(0x282, 0x16);
            this.statusScanner.TabIndex = 2;
            this.statusScanner.Text = "statusStrip1";
            this.lblProgress.AutoSize = false;
            this.lblProgress.Name = "lblProgress";
            this.lblProgress.Size = new Size(460, 0x11);
            this.lblProgress.Text = "Done";
            this.lblProgress.TextAlign = ContentAlignment.MiddleLeft;
            this.toolStripStatusLabel1.ForeColor = SystemColors.ButtonShadow;
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new Size(11, 0x11);
            this.toolStripStatusLabel1.Text = "|";
            this.lblThreadNum.AutoSize = false;
            this.lblThreadNum.Name = "lblThreadNum";
            this.lblThreadNum.Size = new Size(0x7d, 0x11);
            this.lblThreadNum.Text = "HTTP Thread:";
            this.lblThreadNum.TextAlign = ContentAlignment.MiddleLeft;
            this.columnURL.Text = "URL / Refer URL";
            this.columnURL.Width = 270;
            this.columnParameter.Text = "Parameter";
            this.columnParameter.Width = 0x4e;
            this.columnType.Text = "Type";
            this.columnType.Width = 0x3b;
            this.columnKeyWord.Text = "KeyWord/ActionURL";
            this.columnKeyWord.Width = 130;
            this.columnVul.Text = "Vulnerability";
            this.columnVul.Width = 170;
            this.listViewWVS.Columns.AddRange(new ColumnHeader[] { this.columnURL, this.columnParameter, this.columnType, this.columnKeyWord, this.columnVul });
            this.listViewWVS.Dock = DockStyle.Fill;
            this.listViewWVS.FullRowSelect = true;
            this.listViewWVS.Location = new Point(0, 0);
            this.listViewWVS.MultiSelect = false;
            this.listViewWVS.Name = "listViewWVS";
            this.listViewWVS.Size = new Size(0x282, 0xb6);
            this.listViewWVS.SmallImageList = this.ImageListScanner;
            this.listViewWVS.TabIndex = 3;
            this.listViewWVS.UseCompatibleStateImageBehavior = false;
            this.listViewWVS.View = View.Details;
            this.listViewWVS.MouseClick += new MouseEventHandler(this.listViewWVS_MouseClick);
            base.AutoScaleDimensions = new SizeF(6f, 12f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.ClientSize = new Size(0x282, 0x182);
            base.Controls.Add(this.splitScanner);
            base.Controls.Add(this.toolStripScanner);
            base.Controls.Add(this.statusScanner);
            base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            base.Icon = (Icon)resources.GetObject("$this.Icon");
            base.Name = "FormScanner";
            this.Text = "FormScanner";
            base.Resize += new EventHandler(this.FormScanner_Resize);
            this.splitScanner.Panel1.ResumeLayout(false);
            this.splitScanner.Panel2.ResumeLayout(false);
            this.splitScanner.ResumeLayout(false);
            this.toolStripScanner.ResumeLayout(false);
            this.toolStripScanner.PerformLayout();
            this.statusScanner.ResumeLayout(false);
            this.statusScanner.PerformLayout();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void InitSystemTimer()
        {
            this.ScannerTimer = new System.Timers.Timer();
            this.ScannerTimer.Interval = 2500.0;
            this.ScannerTimer.Elapsed += new ElapsedEventHandler(this.ScannerTimer_Elapsed);
            this.ScannerTimer.Enabled = true;
        }

        private bool IsWebPage(string sURL)
        {
            string fileExt = this.SiteA.GetFileExt(sURL);
            if (string.IsNullOrEmpty(fileExt))
            {
                return true;
            }
            fileExt = fileExt.Substring(1).ToLower();
            string[] strArray = WCRSetting.CrawlableExt.Split(new char[] { ':' });
            for (int i = 0; i < strArray.Length; i++)
            {
                if (fileExt.Equals(strArray[i]))
                {
                    return true;
                }
            }
            return false;
        }

        private void listViewWVS_MouseClick(object sender, MouseEventArgs e)
        {
            if (this.listViewWVS.SelectedItems.Count >= 1)
            {
                ContextMenuStrip strip = new ContextMenuStrip();
                strip.Items.Add("Copy URL To ClipBoard", null, new EventHandler(this.WVSItemClick));
                string text = this.listViewWVS.SelectedItems[0].SubItems[4].Text;
                if (text.IndexOf("SQL INJECTION") >= 0)
                {
                    strip.Items.Add("SQL INJECTION POC", null, new EventHandler(this.WVSItemClick));
                }
                else if (text.IndexOf("XPath INJECTION") >= 0)
                {
                    strip.Items.Add("XPath INJECTION POC", null, new EventHandler(this.WVSItemClick));
                }
                else if (text.IndexOf("Cross Site Scripting(URL)") >= 0)
                {
                    strip.Items.Add("Cross Site Scripting(URL) POC", null, new EventHandler(this.WVSItemClick));
                }
                else if (text.IndexOf("Cross Site Scripting(Form)") >= 0)
                {
                    strip.Items.Add("Cross Site Scripting(Form) POC", null, new EventHandler(this.WVSItemClick));
                }
                strip.Items.Add("Delete Vulnerability", null, new EventHandler(this.WVSItemClick));
                this.listViewWVS.ContextMenuStrip = strip;
            }
        }

        public void LoadFromXmlDocument(XmlDocument WVSXml)
        {
            foreach (XmlNode node in WVSXml.SelectNodes("//ROOT/SiteDirTree/DIR"))
            {
                TreeNode parentTn = this.treeViewWVS.Nodes.Add(node.Attributes["Value"].Value);
                if (node.ChildNodes.Count > 0)
                {
                    this.XmlNode2TreeNode(node, parentTn);
                }
            }
            this.treeViewWVS.ExpandAll();
            XmlNodeList list2 = WVSXml.SelectNodes("//ROOT/SiteVulList/VulRow");
            this.listViewWVS.Items.Clear();
            foreach (XmlNode node3 in list2)
            {
                ListViewItem item = new ListViewItem(node3.ChildNodes[0].InnerText);
                item.SubItems.Add(node3.ChildNodes[1].InnerText);
                item.SubItems.Add(node3.ChildNodes[2].InnerText);
                item.SubItems.Add(node3.ChildNodes[3].InnerText);
                item.SubItems.Add(node3.ChildNodes[4].InnerText);
                item.ImageKey = "vul.png";
                this.listViewWVS.Items.Add(item);
            }
        }

        public void ScanCurrentSite()
        {
            this.SiteA.URL = this.mainfrm.URL;
            if (string.IsNullOrEmpty(this.SiteA.URL))
            {
                MessageBox.Show("Please input the URL!", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                this.mainfrm.URLTextBoxFocus();
            }
            else if (MessageBox.Show("* Software Disclaimer: \r\n* Authorization must be obtained from the web application owner;\r\n* This program will try to get each link and post any data when scanning;\r\n* Backup the database before scanning so as to avoid disaster;\r\n* Using this software at your own risk.\r\n\r\n* Login as a legal user will help you find vulnerabilities to the most extent.\r\n* But not login is better if you intend to scan the login/authentication page.\r\n* Continue?\r\n", "Confirm", MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk) != DialogResult.Cancel)
            {
                this.BeginSiteScan();
            }
        }

        public void ScanCurrentURL()
        {
            if (string.IsNullOrEmpty(this.SiteA.URL))
            {
                MessageBox.Show("Please input the URL!", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                this.mainfrm.URLTextBoxFocus();
            }
            else
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(this.CheckVulnerability), this.SiteA.URL);
                this.DisplayProgress("Scanning...");
                MessageBox.Show("* Scanning Has Been Launched.\r\n* The Result Will List In The Vulnerabilities ListView.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }

        private void ScannerTimer_Elapsed(object sender, EventArgs e)
        {
            this.ScanningTimerTask();
        }

        private void ScanningTimerTask()
        {
            this.DisplayThreadNum("HTTP Thread: " + this.SiteA.HTTPThreadNum.ToString());
            DateTime now = DateTime.Now;
            TimeSpan span = now.Subtract(WebSite.StopTime);
            span = now.Subtract(this.SiteA.LastGetTime);
            if ((span.Seconds > 20) && (this.SiteA.HTTPThreadNum == 0))
            {
                this.DisplayProgress("Done");
            }
            if ((span.Seconds > 30) && (this.SiteA.HTTPThreadNum > 0))
            {
                this.SiteA.HTTPThreadNum = 0;
            }
        }

        private void SiteTreeItemClick(object sender, EventArgs e)
        {
            try
            {
                string str;
                if (((str = ((ToolStripMenuItem) sender).Text) != null) && (str == "Copy To ClipBoard"))
                {
                    Clipboard.SetText(this.treeViewWVS.SelectedNode.Text);
                }
            }
            catch
            {
            }
        }

        private void toolStripClearWVS_Click(object sender, EventArgs e)
        {
            this.treeViewWVS.Nodes.Clear();
            this.listViewWVS.Items.Clear();
            this.SiteA.ClearWVS();
        }

        private void toolStripExpWVS_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.listViewWVS.Items.Count >= 1)
                {
                    string wVSXmlFileName = this.SiteA.DomainHost + "_Vuls_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xml";
                    SaveFileDialog dialog = new SaveFileDialog {
                        Filter = "XML File(*.xml) | *.xml",
                        InitialDirectory = Application.StartupPath,
                        FileName = wVSXmlFileName
                    };
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        wVSXmlFileName = dialog.FileName;
                        dialog.Dispose();
                        this.ExportWVS(wVSXmlFileName);
                    }
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void toolStripImpWVS_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.listViewWVS.Items.Count > 0)
                {
                    if (MessageBox.Show("* Import Vuls Will Clear Current Vuls Information.\r\n* Continue?\r\n", "Confirm", MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk) == DialogResult.Cancel)
                    {
                        return;
                    }
                    this.listViewWVS.Items.Clear();
                }
                OpenFileDialog dialog = new OpenFileDialog {
                    Filter = "XML File(*.xml)|*.xml",
                    InitialDirectory = Application.StartupPath
                };
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    string fileName = dialog.FileName;
                    dialog.Dispose();
                    if (!string.IsNullOrEmpty(fileName))
                    {
                        XmlDocument wVSXml = new XmlDocument();
                        wVSXml.Load(fileName);
                        this.LoadFromXmlDocument(wVSXml);
                    }
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void toolStripMultiScan_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("* Software Disclaimer: \r\n* Authorization must be obtained from the web application owner;\r\n* This program will try to get each link and post any data when scanning;\r\n* Backup the database before scanning so as to avoid disaster;\r\n* Using this software at your own risk. \r\n\r\n* Multi-Site scanning will read the sites list from a text file.\r\n* Continue?\r\n", "Confirm", MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk) != DialogResult.Cancel)
                {
                    OpenFileDialog dialog = new OpenFileDialog {
                        Filter = "Text File(*.txt)|*.txt",
                        FileName = "SiteList.txt",
                        InitialDirectory = Application.StartupPath
                    };
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        string fileName = dialog.FileName;
                        dialog.Dispose();
                        StreamReader reader = new StreamReader(fileName);
                        string str2 = "";
                        string str3 = "";
                        while ((str2 = reader.ReadLine()) != null)
                        {
                            if (!string.IsNullOrEmpty(str2))
                            {
                                if (string.IsNullOrEmpty(str3))
                                {
                                    str3 = str3 + str2;
                                }
                                else
                                {
                                    str3 = str3 + "^^" + str2;
                                }
                            }
                        }
                        reader.Close();
                        ThreadPool.QueueUserWorkItem(new WaitCallback(this.AutoSiteScanControl), str3);
                    }
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void toolStripURLScan_Click(object sender, EventArgs e)
        {
            this.ScanCurrentURL();
        }

        private void toolStripWVScan_Click(object sender, EventArgs e)
        {
            this.ScanCurrentSite();
        }

        private void TreeNode2URLS(string Path, TreeNode tn)
        {
            if (WebSite.CurrentStatus != TaskStatus.Stop)
            {
                if (tn.Nodes.Count == 0)
                {
                    string sURL = Path + tn.Text;
                    if (this.IsWebPage(sURL))
                    {
                        this.TreeNodeURL.Add(Path + tn.Text);
                    }
                }
                else
                {
                    this.TreeNodeURL.Add(Path + tn.Text + "/");
                    foreach (TreeNode node in tn.Nodes)
                    {
                        this.TreeNode2URLS(Path + tn.Text + "/", node);
                    }
                }
            }
        }

        private XmlDocument TreeNode2XmlElement(XmlDocument WVSXml, XmlElement ParentElem, TreeNode tn)
        {
            XmlElement parentElem = WVSXml.CreateElement("DIR");
            parentElem.SetAttribute("Value", tn.Text);
            foreach (TreeNode node in tn.Nodes)
            {
                WVSXml = this.TreeNode2XmlElement(WVSXml, parentElem, node);
            }
            ParentElem.AppendChild(parentElem);
            return WVSXml;
        }

        private void treeViewWVS_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            Point pt = new Point(e.X, e.Y);
            TreeNode nodeAt = this.treeViewWVS.GetNodeAt(pt);
            this.treeViewWVS.SelectedNode = nodeAt;
            ContextMenuStrip strip = new ContextMenuStrip();
            strip.Items.Add("Copy To ClipBoard", null, new EventHandler(this.SiteTreeItemClick));
            this.treeViewWVS.ContextMenuStrip = strip;
        }

        private void WVSItemClick(object sender, EventArgs e)
        {
            try
            {
                string str5;
                string str7;
                string text = this.listViewWVS.SelectedItems[0].SubItems[1].Text;
                string str2 = this.listViewWVS.SelectedItems[0].SubItems[4].Text;
                string str9 = ((ToolStripMenuItem) sender).Text;
                if (str9 != null)
                {
                    if (!(str9 == "SQL INJECTION POC"))
                    {
                        if (str9 == "Copy URL To ClipBoard")
                        {
                            goto Label_023F;
                        }
                        if (str9 == "Delete Vulnerability")
                        {
                            goto Label_025F;
                        }
                        if (str9 == "XPath INJECTION POC")
                        {
                            goto Label_027A;
                        }
                        if ((str9 == "Cross Site Scripting(URL) POC") || (str9 == "Cross Site Scripting(Form) POC"))
                        {
                            goto Label_02D7;
                        }
                    }
                    else
                    {
                        string expression = this.listViewWVS.SelectedItems[0].Text;
                        if (expression.IndexOf('^') > 0)
                        {
                            string[] paraNameValue = WebSite.GetParaNameValue(expression, '^');
                            expression = paraNameValue[0];
                            this.mainfrm.UpdateSubmitData(paraNameValue[1]);
                            if (str2.IndexOf("POST") >= 0)
                            {
                                this.mainfrm.ReqType = RequestType.POST;
                            }
                            else if (str2.IndexOf("COOKIE") >= 0)
                            {
                                this.mainfrm.ReqType = RequestType.COOKIE;
                            }
                        }
                        else
                        {
                            this.mainfrm.ReqType = RequestType.GET;
                        }
                        this.mainfrm.URL = expression;
                        string str4 = this.listViewWVS.SelectedItems[0].SubItems[2].Text;
                        if (str4.Equals("Integer"))
                        {
                            this.SiteA.InjType = InjectionType.Integer;
                            this.mainfrm.InitByInjectionType(InjectionType.Integer, expression);
                        }
                        else if (str4.Equals("String"))
                        {
                            this.SiteA.InjType = InjectionType.String;
                            this.mainfrm.InitByInjectionType(InjectionType.String, expression);
                        }
                        else if (str4.Equals("Search"))
                        {
                            this.SiteA.InjType = InjectionType.Search;
                            this.mainfrm.InitByInjectionType(InjectionType.Search, expression);
                        }
                        this.SiteA.CurrentKeyWord = this.listViewWVS.SelectedItems[0].SubItems[3].Text;
                        this.mainfrm.UpdateKeyWordText(this.SiteA.CurrentKeyWord);
                        this.mainfrm.SelectTool("SQL");
                    }
                }
                return;
            Label_023F:
                Clipboard.SetText(this.listViewWVS.SelectedItems[0].Text);
                return;
            Label_025F:
                this.listViewWVS.SelectedItems[0].Remove();
                return;
            Label_027A:
                str5 = this.listViewWVS.SelectedItems[0].SubItems[3].Text;
                string refURL = this.listViewWVS.SelectedItems[0].Text;
                this.mainfrm.SelectTool("WebBrowser");
                this.mainfrm.XPathPOC(refURL, str5, text);
                return;
            Label_02D7:
                str7 = this.listViewWVS.SelectedItems[0].Text;
                string actionURL = this.listViewWVS.SelectedItems[0].SubItems[3].Text;
                this.mainfrm.XSSPOC(str7, actionURL);
                this.mainfrm.SelectTool("XSS");
            }
            catch
            {
            }
        }

        private void XmlNode2TreeNode(XmlNode dir, TreeNode ParentTn)
        {
            foreach (XmlNode node in dir.ChildNodes)
            {
                TreeNode parentTn = ParentTn.Nodes.Add(node.Attributes["Value"].Value);
                if (node.ChildNodes.Count > 0)
                {
                    this.XmlNode2TreeNode(node, parentTn);
                }
            }
        }

        private delegate void dd(string s);

        private delegate void ddc(TreeView s, TreeView d);

        private delegate ListViewItem dl(int i);
    }
}

