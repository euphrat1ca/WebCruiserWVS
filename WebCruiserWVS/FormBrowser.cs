namespace WebCruiserWVS
{
    using Microsoft.JScript;
    using SHDocVw;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Net;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Web;
    using System.Windows.Forms;

    public class FormBrowser : Form
    {
        private System.Windows.Forms.Timer BrowserTimer;
        private ToolStripButton ButtonLoadInBrowser;
        private ToolStripButton ButtonResend;
        private IContainer components;
        private ImageList ImageListBrowser;
        private ToolStripLabel LabelResendURL;
        private ToolStripLabel LabelResponseCode;
        private string LastDomainHost = "";
        private string LastURL = "";
        private FormMain mainfrm;
        private SplitContainer splitContainerResend;
        private TabPage tabBrowser;
        private TabControl tabBrowserForm;
        private TabPage tabResend;
        private ToolStripTextBox TextBoxResendURL;
        private ToolStrip toolStripResend;
        private ToolStrip toolStripResendResponse;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripSeparator toolStripSeparator4;
        private ToolStripSeparator toolStripSeparator5;
        private ToolStripSeparator toolStripSeparator6;
        private ToolStripSeparator toolStripSeparator7;
        private TextBox txtResendPostData;
        private TextBox txtResendResponseCode;
        private SHDocVw.WebBrowser wb;
        private System.Windows.Forms.WebBrowser WCRBrowser;

        public FormBrowser(FormMain fm)
        {
            this.InitializeComponent();
            this.mainfrm = fm;
            this.LoadDefaultPage();
            this.toolStripResendResponse.Visible = true;
            try
            {
                this.wb = (SHDocVw.WebBrowser) this.WCRBrowser.ActiveXInstance;
                //this.wb.add_BeforeNavigate2(new DWebBrowserEvents2_BeforeNavigate2EventHandler(this.wb_BeforeNavigate2));
                this.wb.BeforeNavigate2+=new DWebBrowserEvents2_BeforeNavigate2EventHandler(this.wb_BeforeNavigate2);
                //this.wb.add_NewWindow3(new DWebBrowserEvents2_NewWindow3EventHandler(this.wb_NewWindow3));
                this.wb.NewWindow3 +=new DWebBrowserEvents2_NewWindow3EventHandler(this.wb_NewWindow3);
            }
            catch
            {
            }
        }

        private void BrowserTimer_Tick(object sender, EventArgs e)
        {
            if (this.WCRBrowser.ReadyState != WebBrowserReadyState.Complete)
            {
                if (this.LastURL != this.WCRBrowser.Url.ToString())
                {
                    this.LastURL = this.WCRBrowser.Url.ToString();
                    this.WebBrowserLoadCompleted();
                }
                this.WCRBrowser.Stop();
            }
            this.BrowserTimer.Stop();
        }

        private void ButtonLoadInBrowser_Click(object sender, EventArgs e)
        {
            try
            {
                string text = this.txtResendResponseCode.Text;
                if (!string.IsNullOrEmpty(text))
                {
                    Encoding webEncoding = this.mainfrm.CurrentSite.WebEncoding;
                    this.WCRBrowser.Navigate("about:blank");
                    do
                    {
                        Application.DoEvents();
                    }
                    while (this.WCRBrowser.ReadyState != WebBrowserReadyState.Complete);
                    this.mainfrm.CurrentSite.WebEncoding = webEncoding;
                    this.WCRBrowser.Document.Write(text);
                    this.tabBrowserForm.SelectTab(this.tabBrowser);
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void ButtonResend_Click(object sender, EventArgs e)
        {
            try
            {
                this.LabelResponseCode.Text = "";
                string text = this.txtResendPostData.Text;
                string str2 = this.TextBoxResendURL.Text;
                if (!string.IsNullOrEmpty(str2))
                {
                    str2 = str2 + "^" + text;
                    this.mainfrm.DisplayProgress("Resend...");
                    HttpWebResponse httpWebResponse = this.mainfrm.CurrentSite.GetHttpWebResponse(str2, RequestType.POST);
                    string sourceCodeFromHttpWebResponse = this.mainfrm.CurrentSite.GetSourceCodeFromHttpWebResponse(httpWebResponse);
                    this.txtResendResponseCode.Text = sourceCodeFromHttpWebResponse;
                    this.LabelResponseCode.Text = httpWebResponse.StatusCode.ToString();
                    this.mainfrm.DisplayProgress("Done");
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
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

        public void FillInForm(string Expression)
        {
            this.mainfrm.DisplayProgress("Filling Forms...");
            Expression = Expression.Replace("&amp;", "&");
            Expression = HttpUtility.UrlDecode(Expression, this.mainfrm.CurrentSite.WebEncoding);
            foreach (string str in Expression.Split(new char[] { '&' }))
            {
                string[] paraNameValue = WebSite.GetParaNameValue(str, '=');
                try
                {
                    this.WCRBrowser.Document.All[paraNameValue[0]].SetAttribute("value", GlobalObject.unescape(paraNameValue[1]));
                }
                catch
                {
                }
                HtmlWindowCollection frames = this.WCRBrowser.Document.Window.Frames;
                for (int i = 0; i < frames.Count; i++)
                {
                    try
                    {
                        this.WCRBrowser.Document.Window.Frames[i].Document.All[paraNameValue[0]].SetAttribute("value", GlobalObject.unescape(paraNameValue[1]));
                    }
                    catch
                    {
                    }
                }
            }
            this.mainfrm.DisplayProgress("Done");
        }

        public string GetSourceCodeFromWebBrowser()
        {
            try
            {
                StreamReader reader;
                string encoding = this.WCRBrowser.Document.Encoding;
                if (string.IsNullOrEmpty(encoding))
                {
                    reader = new StreamReader(this.WCRBrowser.DocumentStream, Encoding.Default);
                }
                else
                {
                    reader = new StreamReader(this.WCRBrowser.DocumentStream, Encoding.GetEncoding(encoding));
                }
                string str2 = reader.ReadToEnd();
                reader.Close();
                return str2;
            }
            catch
            {
                MessageBox.Show("* Null Source Code: Disabled OR No Page Navigated!\r\n* Try To Get Code From URL.");
                return "";
            }
        }

        public int GetWCRBrowserFrameNum()
        {
            return this.WCRBrowser.Document.Window.Frames.Count;
        }

        public string GetWCRBrowserFrameSource(int i)
        {
            try
            {
                return this.WCRBrowser.Document.Window.Frames[i].Document.Body.OuterHtml;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
                return "";
            }
        }

        public string GetWCRBrowserFrameURL(int i)
        {
            try
            {
                return this.WCRBrowser.Document.Window.Frames[i].Url.AbsoluteUri;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
                return "";
            }
        }

        private void HTTPFileReset()
        {
            this.mainfrm.CurrentSite.InjType = InjectionType.UnKnown;
            this.mainfrm.CurrentSite.BlindInjType = BlindType.UnKnown;
            this.mainfrm.UpdateKeyWordText("");
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormBrowser));
            this.WCRBrowser = new System.Windows.Forms.WebBrowser();
            this.BrowserTimer = new System.Windows.Forms.Timer(this.components);
            this.tabBrowserForm = new TabControl();
            this.tabBrowser = new TabPage();
            this.tabResend = new TabPage();
            this.splitContainerResend = new SplitContainer();
            this.txtResendPostData = new TextBox();
            this.txtResendResponseCode = new TextBox();
            this.toolStripResendResponse = new ToolStrip();
            this.toolStripSeparator5 = new ToolStripSeparator();
            this.LabelResponseCode = new ToolStripLabel();
            this.toolStripSeparator6 = new ToolStripSeparator();
            this.ButtonLoadInBrowser = new ToolStripButton();
            this.toolStripSeparator7 = new ToolStripSeparator();
            this.toolStripResend = new ToolStrip();
            this.toolStripSeparator4 = new ToolStripSeparator();
            this.LabelResendURL = new ToolStripLabel();
            this.toolStripSeparator1 = new ToolStripSeparator();
            this.TextBoxResendURL = new ToolStripTextBox();
            this.toolStripSeparator2 = new ToolStripSeparator();
            this.ButtonResend = new ToolStripButton();
            this.toolStripSeparator3 = new ToolStripSeparator();
            this.ImageListBrowser = new ImageList(this.components);
            this.tabBrowserForm.SuspendLayout();
            this.tabBrowser.SuspendLayout();
            this.tabResend.SuspendLayout();
            this.splitContainerResend.Panel1.SuspendLayout();
            this.splitContainerResend.Panel2.SuspendLayout();
            this.splitContainerResend.SuspendLayout();
            this.toolStripResendResponse.SuspendLayout();
            this.toolStripResend.SuspendLayout();
            base.SuspendLayout();
            this.WCRBrowser.Dock = DockStyle.Fill;
            this.WCRBrowser.Location = new Point(3, 3);
            this.WCRBrowser.Margin = new Padding(3, 4, 3, 4);
            this.WCRBrowser.MinimumSize = new Size(0x17, 0x1b);
            this.WCRBrowser.Name = "WCRBrowser";
            this.WCRBrowser.Size = new Size(0x268, 0x17b);
            this.WCRBrowser.TabIndex = 1;
            this.WCRBrowser.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(this.WCRBrowser_DocumentCompleted);
            this.WCRBrowser.StatusTextChanged += new EventHandler(this.WCRBrowser_StatusTextChanged);
            this.BrowserTimer.Interval = 0x7530;
            this.BrowserTimer.Tick += new EventHandler(this.BrowserTimer_Tick);
            this.tabBrowserForm.Controls.Add(this.tabBrowser);
            this.tabBrowserForm.Controls.Add(this.tabResend);
            this.tabBrowserForm.Dock = DockStyle.Fill;
            this.tabBrowserForm.ImageList = this.ImageListBrowser;
            this.tabBrowserForm.Location = new Point(0, 0);
            this.tabBrowserForm.Name = "tabBrowserForm";
            this.tabBrowserForm.SelectedIndex = 0;
            this.tabBrowserForm.Size = new Size(630, 0x19c);
            this.tabBrowserForm.TabIndex = 2;
            this.tabBrowser.Controls.Add(this.WCRBrowser);
            this.tabBrowser.ImageKey = "ie.png";
            this.tabBrowser.Location = new Point(4, 0x17);
            this.tabBrowser.Name = "tabBrowser";
            this.tabBrowser.Padding = new Padding(3);
            this.tabBrowser.Size = new Size(0x26e, 0x181);
            this.tabBrowser.TabIndex = 0;
            this.tabBrowser.Text = "WebBrowser";
            this.tabBrowser.UseVisualStyleBackColor = true;
            this.tabResend.Controls.Add(this.splitContainerResend);
            this.tabResend.Controls.Add(this.toolStripResendResponse);
            this.tabResend.Controls.Add(this.toolStripResend);
            this.tabResend.ImageKey = "resend.png";
            this.tabResend.Location = new Point(4, 0x17);
            this.tabResend.Name = "tabResend";
            this.tabResend.Padding = new Padding(3);
            this.tabResend.Size = new Size(0x26e, 0x181);
            this.tabResend.TabIndex = 1;
            this.tabResend.Text = "Resend";
            this.tabResend.UseVisualStyleBackColor = true;
            this.splitContainerResend.Dock = DockStyle.Fill;
            this.splitContainerResend.Location = new Point(3, 0x1c);
            this.splitContainerResend.Name = "splitContainerResend";
            this.splitContainerResend.Orientation = Orientation.Horizontal;
            this.splitContainerResend.Panel1.Controls.Add(this.txtResendPostData);
            this.splitContainerResend.Panel2.Controls.Add(this.txtResendResponseCode);
            this.splitContainerResend.Size = new Size(0x268, 0x149);
            this.splitContainerResend.SplitterDistance = 0x69;
            this.splitContainerResend.TabIndex = 3;
            this.txtResendPostData.Dock = DockStyle.Fill;
            this.txtResendPostData.Location = new Point(0, 0);
            this.txtResendPostData.Multiline = true;
            this.txtResendPostData.Name = "txtResendPostData";
            this.txtResendPostData.Size = new Size(0x268, 0x69);
            this.txtResendPostData.TabIndex = 0;
            this.txtResendPostData.Text = "Post Data";
            this.txtResendResponseCode.Dock = DockStyle.Fill;
            this.txtResendResponseCode.Location = new Point(0, 0);
            this.txtResendResponseCode.Multiline = true;
            this.txtResendResponseCode.Name = "txtResendResponseCode";
            this.txtResendResponseCode.ScrollBars = ScrollBars.Both;
            this.txtResendResponseCode.Size = new Size(0x268, 220);
            this.txtResendResponseCode.TabIndex = 0;
            this.txtResendResponseCode.Text = "Response Source Code";
            this.toolStripResendResponse.BackColor = SystemColors.ButtonFace;
            this.toolStripResendResponse.Dock = DockStyle.Bottom;
            this.toolStripResendResponse.GripStyle = ToolStripGripStyle.Hidden;
            this.toolStripResendResponse.Items.AddRange(new ToolStripItem[] { this.toolStripSeparator5, this.LabelResponseCode, this.toolStripSeparator6, this.ButtonLoadInBrowser, this.toolStripSeparator7 });
            this.toolStripResendResponse.Location = new Point(3, 0x165);
            this.toolStripResendResponse.Name = "toolStripResendResponse";
            this.toolStripResendResponse.Size = new Size(0x268, 0x19);
            this.toolStripResendResponse.TabIndex = 2;
            this.toolStripResendResponse.Text = "toolStrip1";
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new Size(6, 0x19);
            this.LabelResponseCode.AutoSize = false;
            this.LabelResponseCode.Name = "LabelResponseCode";
            this.LabelResponseCode.Size = new Size(150, 0x16);
            this.LabelResponseCode.Text = "StatusCode";
            this.LabelResponseCode.TextAlign = ContentAlignment.MiddleLeft;
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new Size(6, 0x19);
            this.ButtonLoadInBrowser.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.ButtonLoadInBrowser.Image = (Image)resources.GetObject("ButtonLoadInBrowser.Image");
            this.ButtonLoadInBrowser.ImageTransparentColor = Color.Magenta;
            this.ButtonLoadInBrowser.Name = "ButtonLoadInBrowser";
            this.ButtonLoadInBrowser.Size = new Size(0x77, 0x16);
            this.ButtonLoadInBrowser.Text = "Load in WebBrowser";
            this.ButtonLoadInBrowser.Click += new EventHandler(this.ButtonLoadInBrowser_Click);
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new Size(6, 0x19);
            this.toolStripResend.BackColor = SystemColors.ButtonFace;
            this.toolStripResend.GripStyle = ToolStripGripStyle.Hidden;
            this.toolStripResend.Items.AddRange(new ToolStripItem[] { this.toolStripSeparator4, this.LabelResendURL, this.toolStripSeparator1, this.TextBoxResendURL, this.toolStripSeparator2, this.ButtonResend, this.toolStripSeparator3 });
            this.toolStripResend.Location = new Point(3, 3);
            this.toolStripResend.Name = "toolStripResend";
            this.toolStripResend.Size = new Size(0x268, 0x19);
            this.toolStripResend.TabIndex = 0;
            this.toolStripResend.Text = "toolStrip1";
            this.toolStripResend.Resize += new EventHandler(this.toolStripResend_Resize);
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new Size(6, 0x19);
            this.LabelResendURL.Name = "LabelResendURL";
            this.LabelResendURL.Size = new Size(0x3f, 0x16);
            this.LabelResendURL.Text = "ActionURL";
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new Size(6, 0x19);
            this.TextBoxResendURL.AutoSize = false;
            this.TextBoxResendURL.Name = "TextBoxResendURL";
            this.TextBoxResendURL.Overflow = ToolStripItemOverflow.Never;
            this.TextBoxResendURL.Size = new Size(450, 0x19);
            this.TextBoxResendURL.DoubleClick += new EventHandler(this.TextBoxResendURL_DoubleClick);
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new Size(6, 0x19);
            this.ButtonResend.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.ButtonResend.Image = (Image)resources.GetObject("ButtonResend.Image");
            this.ButtonResend.ImageTransparentColor = Color.Magenta;
            this.ButtonResend.Name = "ButtonResend";
            this.ButtonResend.Size = new Size(0x31, 0x16);
            this.ButtonResend.Text = "Resend";
            this.ButtonResend.Click += new EventHandler(this.ButtonResend_Click);
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new Size(6, 0x19);
            this.ImageListBrowser.ImageStream = (ImageListStreamer)resources.GetObject("ImageListBrowser.ImageStream");
            this.ImageListBrowser.TransparentColor = Color.Transparent;
            this.ImageListBrowser.Images.SetKeyName(0, "ie.png");
            this.ImageListBrowser.Images.SetKeyName(1, "resend.png");
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.ClientSize = new Size(630, 0x19c);
            base.Controls.Add(this.tabBrowserForm);
            base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            base.Name = "FormBrowser";
            this.Text = "WebBrowser";
            this.tabBrowserForm.ResumeLayout(false);
            this.tabBrowser.ResumeLayout(false);
            this.tabResend.ResumeLayout(false);
            this.tabResend.PerformLayout();
            this.splitContainerResend.Panel1.ResumeLayout(false);
            this.splitContainerResend.Panel1.PerformLayout();
            this.splitContainerResend.Panel2.ResumeLayout(false);
            this.splitContainerResend.Panel2.PerformLayout();
            this.splitContainerResend.ResumeLayout(false);
            this.toolStripResendResponse.ResumeLayout(false);
            this.toolStripResendResponse.PerformLayout();
            this.toolStripResend.ResumeLayout(false);
            this.toolStripResend.PerformLayout();
            base.ResumeLayout(false);
        }

        private bool IsGovWebSite(string Host)
        {
            bool flag = false;
            if (Regex.IsMatch(Host, @"(.gov.\w+$)|(.gov$)"))
            {
                return true;
            }
            if (Regex.IsMatch(Host, @"(.mil.\w+$)|(.mil$)"))
            {
                flag = true;
            }
            return flag;
        }

        private void LoadDefaultPage()
        {
            this.WCRBrowser.Navigate("about:blank");
            string text = "<br>WebCruiser - Web Vulnerability Scanner<br><br>";
            text = (text + "<a href=\"http://sec4app.com\">http://sec4app.com</a><br>") + "<a href=\"http://www.janusec.com\">http://www.janusec.com</a><br>" + "<a href=\"http://twitter.com/janusec\">http://twitter.com/janusec</a><br>";
            this.WCRBrowser.Document.Write(text);
        }

        public void NavigatePage(string sURL, RequestType ReqType, string SubmitData)
        {
            try
            {
                byte[] postData = null;
                if (ReqType == RequestType.POST)
                {
                    postData = Encoding.UTF8.GetBytes(SubmitData);
                }
                else if (ReqType == RequestType.COOKIE)
                {
                    this.mainfrm.CurrentSite.SetSingleCookie(SubmitData);
                }
                this.mainfrm.DisplayProgress("Navigating ...");
                this.BrowserTimer.Start();
                this.WCRBrowser.Navigate(sURL, null, postData, "Content-Type: application/x-www-form-urlencoded\r\n");
                this.tabBrowserForm.SelectTab(this.tabBrowser);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message + "\r\nWebBrowser Closed! Please Exit and Run again!");
            }
        }

        public void SelectTabByName(string TabName)
        {
            this.tabBrowserForm.SelectTab(TabName);
        }

        private void TextBoxResendURL_DoubleClick(object sender, EventArgs e)
        {
            this.TextBoxResendURL.SelectAll();
        }

        private void toolStripResend_Resize(object sender, EventArgs e)
        {
            this.TextBoxResendURL.Width = this.toolStripResend.Width - 150;
        }

        private void wb_BeforeNavigate2(object pDisp, ref object URL, ref object Flags, ref object TargetFrameName, ref object PostData, ref object Headers, ref bool Cancel)
        {
            string submitData = Encoding.ASCII.GetString(PostData as byte[]);
            if (this.mainfrm.ReqType != RequestType.COOKIE)
            {
                this.mainfrm.UpdateSubmitData(submitData);
            }
            this.TextBoxResendURL.Text = URL as string;
            this.txtResendPostData.Text = submitData;
        }

        private void wb_NewWindow3(ref object ppDisp, ref bool Cancel, uint dwFlags, string bstrUrlContext, string bstrUrl)
        {
            try
            {
                if (bstrUrl.IndexOf("http") >= 0)
                {
                    Cancel = true;
                    this.WCRBrowser.Navigate(bstrUrl, false);
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void WCRBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if ((this.WCRBrowser.ReadyState == WebBrowserReadyState.Complete) && (this.WCRBrowser.Url.ToString() != this.LastURL))
            {
                this.LastURL = this.WCRBrowser.Url.ToString();
                this.WebBrowserLoadCompleted();
            }
        }

        private void WCRBrowser_StatusTextChanged(object sender, EventArgs e)
        {
            this.mainfrm.DisplayProgressNoLog(this.WCRBrowser.StatusText.Replace("&", "&&"));
        }

        private void WebBrowserLoadCompleted()
        {
            try
            {
                string str = this.WCRBrowser.Url.ToString();
                if (str.IndexOf("about:") != 0)
                {
                    if (WCRSetting.RefreshURL)
                    {
                        this.mainfrm.URL = str;
                    }
                    string encoding = this.WCRBrowser.Document.Encoding;
                    if (string.IsNullOrEmpty(encoding))
                    {
                        this.mainfrm.CurrentSite.WebEncoding = Encoding.Default;
                    }
                    else
                    {
                        this.mainfrm.CurrentSite.WebEncoding = Encoding.GetEncoding(encoding);
                    }
                    this.mainfrm.CurrentSite.DBEncoding = this.mainfrm.CurrentSite.WebEncoding;
                }
                if (!string.IsNullOrEmpty(this.LastDomainHost))
                {
                    this.HTTPFileReset();
                }
                if (!this.mainfrm.CurrentSite.DomainHost.Equals(this.LastDomainHost))
                {
                    this.mainfrm.CurrentSite.WcrFileName = "";
                }
                this.LastDomainHost = this.mainfrm.CurrentSite.DomainHost;
                this.mainfrm.DisplayProgress("Done");
                string cookie = this.WCRBrowser.Document.Cookie;
                if (!string.IsNullOrEmpty(cookie))
                {
                    cookie = cookie.Replace(';', ',');
                    this.mainfrm.CurrentSite.cc.SetCookies(this.mainfrm.CurrentSite.URI, cookie);
                }
            }
            catch
            {
            }
        }

        public void XPathPOC(string RefURL, string XPathForm, string Parameter)
        {
            this.mainfrm.NavigatePage(RefURL, RequestType.GET, "");
            string[] paraNameValue = WebSite.GetParaNameValue(Parameter, '=');
            string str = paraNameValue[0];
            string expression = paraNameValue[1];
            string[] strArray2 = new string[2];
            if (XPathForm.IndexOf('^') > 0)
            {
                strArray2 = WebSite.GetParaNameValue(XPathForm, '^');
            }
            else if (XPathForm.IndexOf('?') > 0)
            {
                strArray2 = WebSite.GetParaNameValue(XPathForm, '?');
            }
            else
            {
                return;
            }
            string[] strArray3 = strArray2[1].Split(new char[] { '&' });
            MessageBox.Show("* It Will Open The XPath Page And Fill In Input Fields Automatically! \r\n* When Page Load Completed, Click OK To Continue!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            this.mainfrm.DisplayProgress("Preparing XPath Data...");
            foreach (HtmlElement element in this.WCRBrowser.Document.All)
            {
                for (int i = 0; i < strArray3.Length; i++)
                {
                    string[] strArray4 = WebSite.GetParaNameValue(strArray3[i], '=');
                    if (element.Name.Equals(strArray4[0]))
                    {
                        element.SetAttribute("value", GlobalObject.unescape(WebSite.RemoveTestInput(strArray4[1])));
                    }
                }
                if (element.Name.Equals(str))
                {
                    element.SetAttribute("value", GlobalObject.unescape(WebSite.RemoveTestInput(expression) + "%27] | * | user[@role=%27admin"));
                }
            }
            this.mainfrm.DisplayProgress("Done");
            MessageBox.Show("* XPath Data Filled OK, You Can View Or Change It Now!\r\n* Then Click Button To Submit The Form Manually! \r\n* You Will Get The Response Possibly Include Confidential Data!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }
    }
}

