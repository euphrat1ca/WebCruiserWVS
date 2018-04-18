namespace WebCruiserWVS
{
    using Microsoft.Win32;
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Threading;
    using System.Windows.Forms;
    using System.Xml;
    //using WebCruiserWVS.Properties;

    public class FormMain : Form
    {
        private FormAbout AboutForm;
        private FormAdmin AdminForm;
        private System.Windows.Forms.Timer AdTimer;
        private FormBrowser BrowserForm;
        private ToolStripButton ButtonAutoFill;
        private ToolStripButton ButtonCookie;
        private ToolStripButton ButtonReport;
        private ToolStripButton ButtonResend;
        private ToolStripButton ButtonScanSite;
        private ToolStripButton ButtonScanURL;
        private ToolStripButton ButtonSetting;
        private ToolStripComboBox cmbReqType;
        private FormCode CodeForm;
        private IContainer components;
        private FormCookie CookieForm;
        private RequestType CurrentRequestType;
        public WebSite CurrentSite = new WebSite("");
        private ToolStripLabel lblSubmitData;
        private ToolStripStatusLabel lblThreadNum;
        private ToolStripMenuItem MenuItemAbout;
        private ToolStripMenuItem MenuItemCheckUpdate;
        private ToolStripMenuItem MenuItemConfig;
        private ToolStripMenuItem MenuItemCookie;
        private ToolStripMenuItem MenuItemDBEncoding;
        private ToolStripMenuItem MenuItemDBEncodingBIG5;
        private ToolStripMenuItem MenuItemDBEncodingGB2312;
        private ToolStripMenuItem MenuItemDBEncodingISO8859;
        private ToolStripMenuItem MenuItemDBEncodingUTF16;
        private ToolStripMenuItem MenuItemDBEncodingUTF8;
        private ToolStripMenuItem MenuItemEscapeCookie;
        private ToolStripMenuItem MenuItemExit;
        private ToolStripMenuItem MenuItemFeedback;
        private ToolStripMenuItem MenuItemFile;
        private ToolStripMenuItem MenuItemHelp;
        private ToolStripMenuItem MenuItemNew;
        private ToolStripMenuItem MenuItemOnlineHelp;
        private ToolStripMenuItem MenuItemOpen;
        private ToolStripMenuItem MenuItemOrder;
        private ToolStripMenuItem MenuItemRefreshURL;
        private ToolStripMenuItem MenuItemReport;
        private ToolStripMenuItem MenuItemResend;
        private ToolStripMenuItem MenuItemSave;
        private ToolStripMenuItem MenuItemSaveAs;
        private ToolStripMenuItem MenuItemScanner;
        private ToolStripMenuItem MenuItemSettings;
        private ToolStripMenuItem MenuItemSQLInjection;
        private ToolStripMenuItem MenuItemTextAd;
        private ToolStripMenuItem MenuItemTool;
        private ToolStripMenuItem MenuItemView;
        private ToolStripMenuItem MenuItemWebBrowser;
        private ToolStripMenuItem MenuItemWebEncoding;
        private ToolStripMenuItem MenuItemWebEncodingBIG5;
        private ToolStripMenuItem MenuItemWebEncodingGB2312;
        private ToolStripMenuItem MenuItemWebEncodingISO8859;
        private ToolStripMenuItem MenuItemWebEncodingUTF16;
        private ToolStripMenuItem MenuItemWebEncodingUTF8;
        private ToolStripMenuItem MenuItemWebsite;
        private ToolStripMenuItem MenuItemXSS;
        private MenuStrip menuStripMain;
        private FormReport ReportForm;
        private FormScanner ScannerForm;
        private System.Windows.Forms.Timer ScanTimer;
        private FormSetting SettingForm;
        private SplitContainer splitMain;
        private FormSQL SQLForm;
        private StatusStrip statusStripMain;
        private string TextAdURL = "http://www.janusec.com/";
        private ToolStripButton toolStripBtnGo;
        private ToolStripButton toolStripBtnPause;
        private ToolStripButton toolStripBtnStop;
        private ToolStripButton toolStripButtonBrowser;
        private ToolStripButton toolStripButtonNew;
        private ToolStripButton toolStripButtonOpen;
        private ToolStripButton toolStripButtonSave;
        private ToolStripButton toolStripButtonScanner;
        private ToolStripButton toolStripButtonSQL;
        private ToolStripButton toolStripButtonXSS;
        private ToolStrip toolStripData;
        private ToolStripLabel toolStripLabel1;
        private ToolStrip toolStripMain;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripSeparator toolStripSeparator10;
        private ToolStripSeparator toolStripSeparator11;
        private ToolStripSeparator toolStripSeparator12;
        private ToolStripSeparator toolStripSeparator13;
        private ToolStripSeparator toolStripSeparator14;
        private ToolStripSeparator toolStripSeparator15;
        private ToolStripSeparator toolStripSeparator16;
        private ToolStripSeparator toolStripSeparator17;
        private ToolStripSeparator toolStripSeparator18;
        private ToolStripSeparator toolStripSeparator19;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripSeparator toolStripSeparator20;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripSeparator toolStripSeparator4;
        private ToolStripSeparator toolStripSeparator5;
        private ToolStripSeparator toolStripSeparator6;
        private ToolStripSeparator toolStripSeparator7;
        private ToolStripSeparator toolStripSeparator8;
        private ToolStripSeparator toolStripSeparator9;
        private ToolStripStatusLabel toolStripStatusProgress;
        private ToolStripStatusLabel toolStripStatusSep1;
        private ToolStrip toolStripURL;
        private TreeView treeViewToolTree;
        private ToolStripTextBox txtSubmitData;
        private ToolStripTextBox txtURL;
        private ImageList WCRImageList;
        private FormXSS XSSForm;

        public FormMain()
        {
            this.InitializeComponent();
            this.cmbReqType.SelectedIndex = 0;
            this.treeViewToolTree.ExpandAll();
            this.InitForm();
            this.InitSetting();
            this.CheckRegistration();
            this.toolStripURL.ImageList = this.WCRImageList;
            this.InitTextAd();
        }

        public void AddItem2ListViewWVS(string Text)
        {
            this.ScannerForm.AddItem2ListViewWVS(Text);
        }

        private void AdTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                string str = this.CurrentSite.GetSourceCode("http://sec4app.com/files/textad.php", RequestType.GET, Encoding.UTF8);
                if (!string.IsNullOrEmpty(str))
                {
                    string[] strArray = str.Split(new char[] { '^' });
                    this.MenuItemTextAd.Text = strArray[0];
                    this.TextAdURL = strArray[1];
                }
            }
            catch
            {
            }
        }

        private void ButtonAutoFill_Click(object sender, EventArgs e)
        {
            this.BrowserForm.FillInForm(this.txtSubmitData.Text);
        }

        private void ButtonCookie_Click(object sender, EventArgs e)
        {
            this.SelectTool("Cookie");
        }

        private void ButtonResend_Click(object sender, EventArgs e)
        {
            this.SelectTool("Resend");
        }

        private void ButtonScanSite_Click(object sender, EventArgs e)
        {
            this.SelectTool("Scanner");
            this.ScannerForm.ScanCurrentSite();
        }

        private void ButtonScanURL_Click(object sender, EventArgs e)
        {
            this.SelectTool("Scanner");
            this.ScannerForm.ScanCurrentURL();
        }

        private void ButtonTest_Click(object sender, EventArgs e)
        {
        }

        private void CheckRegistration()
        {
            try
            {
                RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Sec4App\WebCruiser", true);
                if (key == null)
                {
                    key = Registry.CurrentUser.CreateSubKey(@"Software\Sec4App\WebCruiser");
                    string str = Reg.Encrypt(DateTime.Now.ToString("yyyy-MM-dd"));
                    key.SetValue("InitDate", str);
                    this.AboutForm.InitRegControl();
                }
                else
                {
                    string str2 = (string) key.GetValue("Username");
                    string str3 = (string) key.GetValue("RegCode");
                    if ((!string.IsNullOrEmpty(str2) && !string.IsNullOrEmpty(str3)) && (Reg.ValidateRegCode(str2, str3) || Reg.ValidateRegCode2(str2, str3)))
                    {
                        Reg.A1K3 = true;
                    }
                    if (!Reg.A1K3)
                    {
                        int days;
                        string str4 = (string) key.GetValue("InitDate");
                        if (string.IsNullOrEmpty(str4))
                        {
                            days = 30;
                        }
                        else
                        {
                            DateTime time = DateTime.ParseExact(Reg.Decrypt(str4), "yyyy-MM-dd", null);
                            days = DateTime.Now.Subtract(time).Days;
                        }
                        Reg.LeftDays = 30 - days;
                    }
                    this.AboutForm.InitRegControl();
                    string str5 = (string) key.GetValue("Edition");
                    if (!string.IsNullOrEmpty(str5) && str5.Equals("Debug"))
                    {
                        WebSite.LogScannedURL = true;
                    }
                    key.Close();
                }
                if (WCRSetting.UseProxy)
                {
                    WCRSetting.RefreshIESettings(WCRSetting.ProxyAddress + ":" + WCRSetting.ProxyPort.ToString());
                }
            }
            catch
            {
                Reg.A1K3 = false;
                this.AboutForm.InitRegControl();
            }
        }

        private void CheckUpdate(object data)
        {
            string strA = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            string sourceCode = this.CurrentSite.GetSourceCode("http://sec4app.com/files/version.xml", RequestType.GET);
            if (!string.IsNullOrEmpty(sourceCode))
            {
                XmlDocument document = new XmlDocument();
                document.LoadXml(sourceCode);
                string strB = document.SelectSingleNode("//ROOT/Version").Attributes["Value"].Value;
                if (string.Compare(strA, strB) >= 0)
                {
                    MessageBox.Show("Current version is up-to-date!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
                else if (MessageBox.Show("Found New Version: " + strB + " , Update Now?", "Information", MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk) == DialogResult.OK)
                {
                    string str4 = "Ent";
                    string sURL = document.SelectSingleNode("//ROOT/Download/URL[@Edition=\"" + str4 + "\"]").Attributes["Value"].Value;
                    this.NavigatePage(sURL, RequestType.GET, "");
                }
            }
            else
            {
                this.SelectTool("WebBrowser");
                this.NavigatePage("http://sec4app.com/", RequestType.GET, "");
            }
        }

        private void cmbReqType_DropDownClosed(object sender, EventArgs e)
        {
            if (this.cmbReqType.Text.Equals("GET"))
            {
                this.CurrentRequestType = RequestType.GET;
            }
            else if (this.cmbReqType.Text.Equals("POST"))
            {
                this.CurrentRequestType = RequestType.POST;
            }
            else if (this.cmbReqType.Text.Equals("COOKIE"))
            {
                this.CurrentRequestType = RequestType.COOKIE;
            }
            this.InitByRequestType(this.CurrentRequestType);
        }

        public void DisplayProgress(string Text)
        {
            if (!this.statusStripMain.InvokeRequired)
            {
                this.toolStripStatusProgress.Text = Text;
                this.statusStripMain.Refresh();
                if (Text.Length > 5)
                {
                    WebSite.LogScannedData(Text);
                }
            }
            else
            {
                dd method = new dd(this.DisplayProgress);
                base.Invoke(method, new object[] { Text });
            }
        }

        public void DisplayProgressNoLog(string Text)
        {
            if (!this.statusStripMain.InvokeRequired)
            {
                this.toolStripStatusProgress.Text = Text;
                this.statusStripMain.Refresh();
            }
            else
            {
                dd method = new dd(this.DisplayProgress);
                base.Invoke(method, new object[] { Text });
            }
        }

        public void DisplayThreadNum(string Text)
        {
            if (!this.statusStripMain.InvokeRequired)
            {
                this.lblThreadNum.Text = Text;
                this.statusStripMain.Refresh();
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

        public void DisposeSubForm(FormScanner SubForm)
        {
            if (!SubForm.InvokeRequired)
            {
                SubForm.Dispose();
            }
            else
            {
                ddd method = new ddd(this.DisposeSubForm);
                base.Invoke(method, new object[] { SubForm });
            }
        }

        public void EnableFunction(bool EnableFunc)
        {
            this.toolStripBtnGo.Enabled = EnableFunc;
            this.ScannerForm.EnableFunc(EnableFunc);
            this.SQLForm.EnableFunc(EnableFunc);
        }

        private void EnableTxtSubmitData(bool TrueFalse)
        {
            if (!this.toolStripData.InvokeRequired)
            {
                this.toolStripData.Visible = TrueFalse;
                this.txtSubmitData.Width = this.toolStripData.Width - 0x55;
            }
            else
            {
                EnableTextBox method = new EnableTextBox(this.EnableTxtSubmitData);
                base.Invoke(method, new object[] { TrueFalse });
            }
        }

        public void EscapeCookie(bool IsEscape)
        {
            if (IsEscape)
            {
                this.MenuItemEscapeCookie.Checked = true;
            }
            else
            {
                this.MenuItemEscapeCookie.Checked = false;
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if ((WebSite.MultiProcessNum > 0) && (MessageBox.Show("* Multi-Site Scanning Task Is Not Complete.\r\n* Site Number: " + WebSite.MultiProcessNum.ToString() + "\r\n* Continue Exit?\r\n", "Confirm", MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk) == DialogResult.Cancel))
            {
                e.Cancel = true;
            }
        }

        private void FormMain_Resize(object sender, EventArgs e)
        {
            this.txtURL.Width = this.toolStripMain.Width - 0xd3;
            this.txtSubmitData.Width = this.toolStripData.Width - 0x55;
            this.toolStripStatusProgress.Width = this.statusStripMain.Width - 150;
        }

        private XmlDocument GetCurrentSiteXml()
        {
            XmlDocument document = new XmlDocument();
            XmlNode newChild = document.CreateXmlDeclaration("1.0", "utf-8", "");
            document.AppendChild(newChild);
            XmlElement element = document.CreateElement("ROOT");
            document.AppendChild(element);
            XmlElement element2 = document.CreateElement("CurrentSite");
            element.AppendChild(element2);
            XmlElement element3 = document.CreateElement("URL");
            element3.SetAttribute("Value", this.CurrentSite.URL);
            element2.AppendChild(element3);
            XmlElement element4 = document.CreateElement("RequestType");
            element4.SetAttribute("Value", this.CurrentRequestType.ToString());
            element2.AppendChild(element4);
            XmlElement element5 = document.CreateElement("SubmitData");
            element5.SetAttribute("Value", this.txtSubmitData.Text);
            element2.AppendChild(element5);
            XmlElement element6 = document.CreateElement("DatabaseType");
            element6.SetAttribute("Value", this.CurrentSite.DatabaseType.ToString());
            element2.AppendChild(element6);
            XmlElement element7 = document.CreateElement("CurrentKeyWord");
            element7.SetAttribute("Value", this.CurrentSite.CurrentKeyWord);
            element2.AppendChild(element7);
            XmlElement element8 = document.CreateElement("CurrentInjType");
            element8.SetAttribute("Value", this.CurrentSite.InjType.ToString());
            element2.AppendChild(element8);
            XmlElement element9 = document.CreateElement("CurrentBlindInjType");
            element9.SetAttribute("Value", this.CurrentSite.BlindInjType.ToString());
            element9.SetAttribute("CurrentFieldEchoIndex", this.CurrentSite.CurrentFieldEchoIndex.ToString());
            element9.SetAttribute("CurrentFieldNum", this.CurrentSite.CurrentFieldNum.ToString());
            element2.AppendChild(element9);
            XmlElement element10 = document.CreateElement("WebRoot");
            element10.SetAttribute("Value", this.CurrentSite.WebRoot);
            element2.AppendChild(element10);
            XmlElement element11 = document.CreateElement("EscapeCookie");
            element11.SetAttribute("Value", WebSite.EscapeCookie.ToString());
            element2.AppendChild(element11);
            return document;
        }

        private string GetCurrentURL()
        {
            if (!this.toolStripMain.InvokeRequired)
            {
                return this.txtURL.Text.Trim();
            }
            ds method = new ds(this.GetCurrentURL);
            return (string) base.Invoke(method, new object[0]);
        }

        public string GetSourceCodeFromWebBrowser()
        {
            return this.BrowserForm.GetSourceCodeFromWebBrowser();
        }

        private string GetSubmitData()
        {
            if (!this.toolStripData.InvokeRequired)
            {
                return this.txtSubmitData.Text;
            }
            ds method = new ds(this.GetSubmitData);
            return (string) base.Invoke(method, new object[0]);
        }

        public int GetWCRBrowserFrameNum()
        {
            return this.BrowserForm.GetWCRBrowserFrameNum();
        }

        public string GetWCRBrowserFrameSource(int i)
        {
            return this.BrowserForm.GetWCRBrowserFrameSource(i);
        }

        public string GetWCRBrowserFrameURL(int i)
        {
            return this.BrowserForm.GetWCRBrowserFrameURL(i);
        }

        private void HideAllToolForm()
        {
            this.BrowserForm.Hide();
            this.ScannerForm.Hide();
            this.SQLForm.Hide();
            this.XSSForm.Hide();
            this.CodeForm.Hide();
            this.CookieForm.Hide();
            this.SettingForm.Hide();
            this.AdminForm.Hide();
            this.ReportForm.Hide();
            this.AboutForm.Hide();
        }

        public void InitByInjectionType(InjectionType InjType, string sURL)
        {
            this.SQLForm.InitByInjectionType(InjType, sURL);
        }

        public void InitByRequestType(RequestType ReqType)
        {
            if (ReqType == RequestType.GET)
            {
                this.UpdateComboReqType("GET");
                this.EnableTxtSubmitData(false);
            }
            else if (ReqType == RequestType.POST)
            {
                this.UpdateComboReqType("POST");
                this.EnableTxtSubmitData(true);
            }
            else if (ReqType == RequestType.COOKIE)
            {
                this.UpdateComboReqType("COOKIE");
                this.EnableTxtSubmitData(true);
            }
        }

        private void InitForm()
        {
            this.Text = "WebCruiser - Web Vulnerability Scanner Enterprise Edition";
            this.BrowserForm = new FormBrowser(this);
            this.BrowserForm.MdiParent = this;
            this.splitMain.Panel2.Controls.Add(this.BrowserForm);
            this.BrowserForm.Dock = DockStyle.Fill;
            base.LayoutMdi(MdiLayout.Cascade);
            this.BrowserForm.Show();
            this.ScannerForm = new FormScanner(this);
            this.ScannerForm.MdiParent = this;
            this.splitMain.Panel2.Controls.Add(this.ScannerForm);
            this.ScannerForm.Dock = DockStyle.Fill;
            this.SQLForm = new FormSQL(this);
            this.SQLForm.MdiParent = this;
            this.splitMain.Panel2.Controls.Add(this.SQLForm);
            this.SQLForm.Dock = DockStyle.Fill;
            this.XSSForm = new FormXSS(this);
            this.XSSForm.MdiParent = this;
            this.splitMain.Panel2.Controls.Add(this.XSSForm);
            this.XSSForm.Dock = DockStyle.Fill;
            this.CodeForm = new FormCode(this);
            this.CodeForm.MdiParent = this;
            this.splitMain.Panel2.Controls.Add(this.CodeForm);
            this.CodeForm.Dock = DockStyle.Fill;
            this.CookieForm = new FormCookie(this);
            this.CookieForm.MdiParent = this;
            this.splitMain.Panel2.Controls.Add(this.CookieForm);
            this.CookieForm.Dock = DockStyle.Fill;
            this.SettingForm = new FormSetting();
            this.SettingForm.MdiParent = this;
            this.splitMain.Panel2.Controls.Add(this.SettingForm);
            this.SettingForm.Dock = DockStyle.Fill;
            this.AdminForm = new FormAdmin(this);
            this.AdminForm.MdiParent = this;
            this.splitMain.Panel2.Controls.Add(this.AdminForm);
            this.AdminForm.Dock = DockStyle.Fill;
            this.ReportForm = new FormReport(this);
            this.ReportForm.MdiParent = this;
            this.splitMain.Panel2.Controls.Add(this.ReportForm);
            this.ReportForm.Dock = DockStyle.Fill;
            this.AboutForm = new FormAbout(this);
            this.AboutForm.MdiParent = this;
            this.splitMain.Panel2.Controls.Add(this.AboutForm);
            this.AboutForm.Dock = DockStyle.Fill;
        }

        public void InitFunctionByRegistration(bool RegOK, int LeftDays)
        {
            bool enableFunc = false;
            if (RegOK)
            {
                enableFunc = true;
            }
            else if (LeftDays > 0)
            {
                enableFunc = true;
            }
            else
            {
                enableFunc = false;
            }
            this.EnableFunction(enableFunc);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("WebBrowser");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("VulnerabilityScanner");
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("SQL Injection");
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("Cross Site Scripting");
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("AdministrationEntrance");
            System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode("POC(Proof Of Concept)", new System.Windows.Forms.TreeNode[] {
            treeNode3,
            treeNode4,
            treeNode5});
            System.Windows.Forms.TreeNode treeNode7 = new System.Windows.Forms.TreeNode("ResendTool");
            System.Windows.Forms.TreeNode treeNode8 = new System.Windows.Forms.TreeNode("CookieTool");
            System.Windows.Forms.TreeNode treeNode9 = new System.Windows.Forms.TreeNode("CodeTool");
            System.Windows.Forms.TreeNode treeNode10 = new System.Windows.Forms.TreeNode("StringTool");
            System.Windows.Forms.TreeNode treeNode11 = new System.Windows.Forms.TreeNode("Settings");
            System.Windows.Forms.TreeNode treeNode12 = new System.Windows.Forms.TreeNode("Report");
            System.Windows.Forms.TreeNode treeNode13 = new System.Windows.Forms.TreeNode("SystemTool", new System.Windows.Forms.TreeNode[] {
            treeNode7,
            treeNode8,
            treeNode9,
            treeNode10,
            treeNode11,
            treeNode12});
            System.Windows.Forms.TreeNode treeNode14 = new System.Windows.Forms.TreeNode("About");
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.splitMain = new System.Windows.Forms.SplitContainer();
            this.treeViewToolTree = new System.Windows.Forms.TreeView();
            this.WCRImageList = new System.Windows.Forms.ImageList(this.components);
            this.menuStripMain = new System.Windows.Forms.MenuStrip();
            this.MenuItemFile = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemNew = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.MenuItemSave = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemSaveAs = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.MenuItemExit = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemTool = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemWebBrowser = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemScanner = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
            this.MenuItemSQLInjection = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemXSS = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.MenuItemResend = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemCookie = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator12 = new System.Windows.Forms.ToolStripSeparator();
            this.MenuItemReport = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemView = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemWebEncoding = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemWebEncodingUTF8 = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemWebEncodingUTF16 = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemWebEncodingISO8859 = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemWebEncodingGB2312 = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemWebEncodingBIG5 = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemDBEncoding = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemDBEncodingUTF8 = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemDBEncodingUTF16 = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemDBEncodingISO8859 = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemDBEncodingGB2312 = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemDBEncodingBIG5 = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemConfig = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator19 = new System.Windows.Forms.ToolStripSeparator();
            this.MenuItemRefreshURL = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemEscapeCookie = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemOnlineHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemOrder = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemCheckUpdate = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemFeedback = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemWebsite = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.MenuItemAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemTextAd = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMain = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonNew = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonOpen = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonSave = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonBrowser = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonScanner = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator10 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonSQL = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator20 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonXSS = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator11 = new System.Windows.Forms.ToolStripSeparator();
            this.ButtonResend = new System.Windows.Forms.ToolStripButton();
            this.ButtonCookie = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator17 = new System.Windows.Forms.ToolStripSeparator();
            this.ButtonReport = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator18 = new System.Windows.Forms.ToolStripSeparator();
            this.ButtonSetting = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator13 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator15 = new System.Windows.Forms.ToolStripSeparator();
            this.ButtonScanURL = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator14 = new System.Windows.Forms.ToolStripSeparator();
            this.ButtonScanSite = new System.Windows.Forms.ToolStripButton();
            this.statusStripMain = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusProgress = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusSep1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblThreadNum = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripURL = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.txtURL = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.cmbReqType = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripBtnGo = new System.Windows.Forms.ToolStripButton();
            this.toolStripBtnPause = new System.Windows.Forms.ToolStripButton();
            this.toolStripBtnStop = new System.Windows.Forms.ToolStripButton();
            this.toolStripData = new System.Windows.Forms.ToolStrip();
            this.lblSubmitData = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.txtSubmitData = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripSeparator16 = new System.Windows.Forms.ToolStripSeparator();
            this.ButtonAutoFill = new System.Windows.Forms.ToolStripButton();
            this.ScanTimer = new System.Windows.Forms.Timer(this.components);
            this.AdTimer = new System.Windows.Forms.Timer(this.components);
            this.splitMain.Panel1.SuspendLayout();
            this.splitMain.SuspendLayout();
            this.menuStripMain.SuspendLayout();
            this.toolStripMain.SuspendLayout();
            this.statusStripMain.SuspendLayout();
            this.toolStripURL.SuspendLayout();
            this.toolStripData.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitMain
            // 
            this.splitMain.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.splitMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitMain.Location = new System.Drawing.Point(0, 75);
            this.splitMain.Name = "splitMain";
            // 
            // splitMain.Panel1
            // 
            this.splitMain.Panel1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.splitMain.Panel1.Controls.Add(this.treeViewToolTree);
            // 
            // splitMain.Panel2
            // 
            this.splitMain.Panel2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.splitMain.Panel2.Paint += new System.Windows.Forms.PaintEventHandler(this.splitMain_Panel2_Paint);
            this.splitMain.Size = new System.Drawing.Size(792, 419);
            this.splitMain.SplitterDistance = 151;
            this.splitMain.TabIndex = 4;
            // 
            // treeViewToolTree
            // 
            this.treeViewToolTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewToolTree.ImageIndex = 0;
            this.treeViewToolTree.ImageList = this.WCRImageList;
            this.treeViewToolTree.Location = new System.Drawing.Point(0, 0);
            this.treeViewToolTree.Name = "treeViewToolTree";
            treeNode1.ImageKey = "ie.png";
            treeNode1.Name = "WebBrowser";
            treeNode1.Text = "WebBrowser";
            treeNode2.ImageKey = "scan.png";
            treeNode2.Name = "Scanner";
            treeNode2.Text = "VulnerabilityScanner";
            treeNode3.ImageKey = "db.png";
            treeNode3.Name = "SQL";
            treeNode3.Text = "SQL Injection";
            treeNode4.ImageKey = "xss.png";
            treeNode4.Name = "XSS";
            treeNode4.Text = "Cross Site Scripting";
            treeNode5.ImageKey = "admin.png";
            treeNode5.Name = "Admin";
            treeNode5.Text = "AdministrationEntrance";
            treeNode6.ImageKey = "tool.png";
            treeNode6.Name = "POCTool";
            treeNode6.Text = "POC(Proof Of Concept)";
            treeNode7.ImageKey = "resend.png";
            treeNode7.Name = "Resend";
            treeNode7.Text = "ResendTool";
            treeNode8.ImageKey = "cookie.png";
            treeNode8.Name = "Cookie";
            treeNode8.Text = "CookieTool";
            treeNode9.ImageKey = "code.png";
            treeNode9.Name = "Code";
            treeNode9.Text = "CodeTool";
            treeNode10.ImageKey = "encode.png";
            treeNode10.Name = "StringTool";
            treeNode10.Text = "StringTool";
            treeNode11.ImageKey = "tool.png";
            treeNode11.Name = "Setting";
            treeNode11.Text = "Settings";
            treeNode12.ImageKey = "report.png";
            treeNode12.Name = "Report";
            treeNode12.Text = "Report";
            treeNode13.ImageKey = "tool.png";
            treeNode13.Name = "SystemTool";
            treeNode13.Text = "SystemTool";
            treeNode14.ImageKey = "about.png";
            treeNode14.Name = "About";
            treeNode14.Text = "About";
            this.treeViewToolTree.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2,
            treeNode6,
            treeNode13,
            treeNode14});
            this.treeViewToolTree.SelectedImageIndex = 0;
            this.treeViewToolTree.Size = new System.Drawing.Size(151, 419);
            this.treeViewToolTree.TabIndex = 0;
            this.treeViewToolTree.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeViewToolTree_NodeMouseClick);
            // 
            // WCRImageList
            // 
            this.WCRImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("WCRImageList.ImageStream")));
            this.WCRImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.WCRImageList.Images.SetKeyName(0, "select.png");
            this.WCRImageList.Images.SetKeyName(1, "ie.png");
            this.WCRImageList.Images.SetKeyName(2, "scan.png");
            this.WCRImageList.Images.SetKeyName(3, "env.png");
            this.WCRImageList.Images.SetKeyName(4, "db.png");
            this.WCRImageList.Images.SetKeyName(5, "xss.png");
            this.WCRImageList.Images.SetKeyName(6, "cmd.png");
            this.WCRImageList.Images.SetKeyName(7, "admin.png");
            this.WCRImageList.Images.SetKeyName(8, "file.png");
            this.WCRImageList.Images.SetKeyName(9, "tool.png");
            this.WCRImageList.Images.SetKeyName(10, "code.png");
            this.WCRImageList.Images.SetKeyName(11, "about.png");
            this.WCRImageList.Images.SetKeyName(12, "go.png");
            this.WCRImageList.Images.SetKeyName(13, "start.png");
            this.WCRImageList.Images.SetKeyName(14, "pause.png");
            this.WCRImageList.Images.SetKeyName(15, "stop.png");
            this.WCRImageList.Images.SetKeyName(16, "table.png");
            this.WCRImageList.Images.SetKeyName(17, "column.png");
            this.WCRImageList.Images.SetKeyName(18, "vul.png");
            this.WCRImageList.Images.SetKeyName(19, "xml.png");
            this.WCRImageList.Images.SetKeyName(20, "report.png");
            this.WCRImageList.Images.SetKeyName(21, "cookie.png");
            this.WCRImageList.Images.SetKeyName(22, "resend.png");
            this.WCRImageList.Images.SetKeyName(23, "encode.png");
            // 
            // menuStripMain
            // 
            this.menuStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuItemFile,
            this.MenuItemTool,
            this.MenuItemView,
            this.MenuItemConfig,
            this.MenuItemHelp,
            this.MenuItemTextAd});
            this.menuStripMain.Location = new System.Drawing.Point(0, 0);
            this.menuStripMain.Name = "menuStripMain";
            this.menuStripMain.Size = new System.Drawing.Size(792, 25);
            this.menuStripMain.TabIndex = 0;
            this.menuStripMain.Text = "menuStrip1";
            // 
            // MenuItemFile
            // 
            this.MenuItemFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuItemNew,
            this.MenuItemOpen,
            this.toolStripSeparator8,
            this.MenuItemSave,
            this.MenuItemSaveAs,
            this.toolStripSeparator6,
            this.MenuItemExit});
            this.MenuItemFile.Name = "MenuItemFile";
            this.MenuItemFile.Size = new System.Drawing.Size(39, 21);
            this.MenuItemFile.Text = "&File";
            // 
            // MenuItemNew
            // 
            this.MenuItemNew.Image = ((System.Drawing.Image)(resources.GetObject("MenuItemNew.Image")));
            this.MenuItemNew.Name = "MenuItemNew";
            this.MenuItemNew.Size = new System.Drawing.Size(130, 22);
            this.MenuItemNew.Text = "New";
            this.MenuItemNew.Click += new System.EventHandler(this.MenuItemNew_Click);
            // 
            // MenuItemOpen
            // 
            this.MenuItemOpen.Image = ((System.Drawing.Image)(resources.GetObject("MenuItemOpen.Image")));
            this.MenuItemOpen.Name = "MenuItemOpen";
            this.MenuItemOpen.Size = new System.Drawing.Size(130, 22);
            this.MenuItemOpen.Text = "Open";
            this.MenuItemOpen.Click += new System.EventHandler(this.MenuItemOpen_Click);
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size(127, 6);
            // 
            // MenuItemSave
            // 
            this.MenuItemSave.Image = ((System.Drawing.Image)(resources.GetObject("MenuItemSave.Image")));
            this.MenuItemSave.Name = "MenuItemSave";
            this.MenuItemSave.Size = new System.Drawing.Size(130, 22);
            this.MenuItemSave.Text = "Save";
            this.MenuItemSave.Click += new System.EventHandler(this.MenuItemSave_Click);
            // 
            // MenuItemSaveAs
            // 
            this.MenuItemSaveAs.Name = "MenuItemSaveAs";
            this.MenuItemSaveAs.Size = new System.Drawing.Size(130, 22);
            this.MenuItemSaveAs.Text = "Save As...";
            this.MenuItemSaveAs.Click += new System.EventHandler(this.MenuItemSaveAs_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(127, 6);
            // 
            // MenuItemExit
            // 
            this.MenuItemExit.Name = "MenuItemExit";
            this.MenuItemExit.Size = new System.Drawing.Size(130, 22);
            this.MenuItemExit.Text = "E&xit";
            this.MenuItemExit.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // MenuItemTool
            // 
            this.MenuItemTool.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuItemWebBrowser,
            this.MenuItemScanner,
            this.toolStripSeparator9,
            this.MenuItemSQLInjection,
            this.MenuItemXSS,
            this.toolStripSeparator5,
            this.MenuItemResend,
            this.MenuItemCookie,
            this.toolStripSeparator12,
            this.MenuItemReport});
            this.MenuItemTool.Name = "MenuItemTool";
            this.MenuItemTool.Size = new System.Drawing.Size(52, 21);
            this.MenuItemTool.Text = "&Tools";
            // 
            // MenuItemWebBrowser
            // 
            this.MenuItemWebBrowser.Image = global::Properties.Resources.Image_2;
            this.MenuItemWebBrowser.Name = "MenuItemWebBrowser";
            this.MenuItemWebBrowser.Size = new System.Drawing.Size(197, 22);
            this.MenuItemWebBrowser.Text = "WebBrowser";
            this.MenuItemWebBrowser.Click += new System.EventHandler(this.MenuItemWebBrowser_Click);
            // 
            // MenuItemScanner
            // 
            this.MenuItemScanner.Image = global::Properties.Resources.Image_3;
            this.MenuItemScanner.Name = "MenuItemScanner";
            this.MenuItemScanner.Size = new System.Drawing.Size(197, 22);
            this.MenuItemScanner.Text = "Vulnerability Scanner";
            this.MenuItemScanner.Click += new System.EventHandler(this.MenuItemScanner_Click);
            // 
            // toolStripSeparator9
            // 
            this.toolStripSeparator9.Name = "toolStripSeparator9";
            this.toolStripSeparator9.Size = new System.Drawing.Size(194, 6);
            // 
            // MenuItemSQLInjection
            // 
            this.MenuItemSQLInjection.Image = global::Properties.Resources.Image_5;
            this.MenuItemSQLInjection.Name = "MenuItemSQLInjection";
            this.MenuItemSQLInjection.Size = new System.Drawing.Size(197, 22);
            this.MenuItemSQLInjection.Text = "SQL Injection";
            this.MenuItemSQLInjection.Click += new System.EventHandler(this.MenuItemSQLInjection_Click);
            // 
            // MenuItemXSS
            // 
            this.MenuItemXSS.Image = global::Properties.Resources.Image_6;
            this.MenuItemXSS.Name = "MenuItemXSS";
            this.MenuItemXSS.Size = new System.Drawing.Size(197, 22);
            this.MenuItemXSS.Text = "Cross Site Scripting";
            this.MenuItemXSS.Click += new System.EventHandler(this.MenuItemXSS_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(194, 6);
            // 
            // MenuItemResend
            // 
            this.MenuItemResend.Image = global::Properties.Resources.Image_23;
            this.MenuItemResend.Name = "MenuItemResend";
            this.MenuItemResend.Size = new System.Drawing.Size(197, 22);
            this.MenuItemResend.Text = "POST Resend";
            this.MenuItemResend.Click += new System.EventHandler(this.MenuItemResend_Click);
            // 
            // MenuItemCookie
            // 
            this.MenuItemCookie.Image = global::Properties.Resources.Image_22;
            this.MenuItemCookie.Name = "MenuItemCookie";
            this.MenuItemCookie.Size = new System.Drawing.Size(197, 22);
            this.MenuItemCookie.Text = "Cookie";
            this.MenuItemCookie.Click += new System.EventHandler(this.MenuItemCookie_Click);
            // 
            // toolStripSeparator12
            // 
            this.toolStripSeparator12.Name = "toolStripSeparator12";
            this.toolStripSeparator12.Size = new System.Drawing.Size(194, 6);
            // 
            // MenuItemReport
            // 
            this.MenuItemReport.Image = global::Properties.Resources.Image_21;
            this.MenuItemReport.Name = "MenuItemReport";
            this.MenuItemReport.Size = new System.Drawing.Size(197, 22);
            this.MenuItemReport.Text = "Report";
            this.MenuItemReport.Click += new System.EventHandler(this.MenuItemReport_Click);
            // 
            // MenuItemView
            // 
            this.MenuItemView.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuItemWebEncoding,
            this.MenuItemDBEncoding});
            this.MenuItemView.Name = "MenuItemView";
            this.MenuItemView.Size = new System.Drawing.Size(47, 21);
            this.MenuItemView.Text = "&View";
            // 
            // MenuItemWebEncoding
            // 
            this.MenuItemWebEncoding.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuItemWebEncodingUTF8,
            this.MenuItemWebEncodingUTF16,
            this.MenuItemWebEncodingISO8859,
            this.MenuItemWebEncodingGB2312,
            this.MenuItemWebEncodingBIG5});
            this.MenuItemWebEncoding.Name = "MenuItemWebEncoding";
            this.MenuItemWebEncoding.Size = new System.Drawing.Size(189, 22);
            this.MenuItemWebEncoding.Text = "Web Encoding";
            // 
            // MenuItemWebEncodingUTF8
            // 
            this.MenuItemWebEncodingUTF8.Name = "MenuItemWebEncodingUTF8";
            this.MenuItemWebEncodingUTF8.Size = new System.Drawing.Size(142, 22);
            this.MenuItemWebEncodingUTF8.Text = "UTF-8";
            this.MenuItemWebEncodingUTF8.Click += new System.EventHandler(this.MenuItemWebEncoding_Click);
            // 
            // MenuItemWebEncodingUTF16
            // 
            this.MenuItemWebEncodingUTF16.Name = "MenuItemWebEncodingUTF16";
            this.MenuItemWebEncodingUTF16.Size = new System.Drawing.Size(142, 22);
            this.MenuItemWebEncodingUTF16.Text = "UTF-16";
            this.MenuItemWebEncodingUTF16.Click += new System.EventHandler(this.MenuItemWebEncoding_Click);
            // 
            // MenuItemWebEncodingISO8859
            // 
            this.MenuItemWebEncodingISO8859.Name = "MenuItemWebEncodingISO8859";
            this.MenuItemWebEncodingISO8859.Size = new System.Drawing.Size(142, 22);
            this.MenuItemWebEncodingISO8859.Text = "ISO-8859-1";
            this.MenuItemWebEncodingISO8859.Click += new System.EventHandler(this.MenuItemWebEncoding_Click);
            // 
            // MenuItemWebEncodingGB2312
            // 
            this.MenuItemWebEncodingGB2312.Name = "MenuItemWebEncodingGB2312";
            this.MenuItemWebEncodingGB2312.Size = new System.Drawing.Size(142, 22);
            this.MenuItemWebEncodingGB2312.Text = "GB2312";
            this.MenuItemWebEncodingGB2312.Click += new System.EventHandler(this.MenuItemWebEncoding_Click);
            // 
            // MenuItemWebEncodingBIG5
            // 
            this.MenuItemWebEncodingBIG5.Name = "MenuItemWebEncodingBIG5";
            this.MenuItemWebEncodingBIG5.Size = new System.Drawing.Size(142, 22);
            this.MenuItemWebEncodingBIG5.Text = "BIG5";
            this.MenuItemWebEncodingBIG5.Click += new System.EventHandler(this.MenuItemWebEncoding_Click);
            // 
            // MenuItemDBEncoding
            // 
            this.MenuItemDBEncoding.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuItemDBEncodingUTF8,
            this.MenuItemDBEncodingUTF16,
            this.MenuItemDBEncodingISO8859,
            this.MenuItemDBEncodingGB2312,
            this.MenuItemDBEncodingBIG5});
            this.MenuItemDBEncoding.Name = "MenuItemDBEncoding";
            this.MenuItemDBEncoding.Size = new System.Drawing.Size(189, 22);
            this.MenuItemDBEncoding.Text = "Database Encoding";
            // 
            // MenuItemDBEncodingUTF8
            // 
            this.MenuItemDBEncodingUTF8.Name = "MenuItemDBEncodingUTF8";
            this.MenuItemDBEncodingUTF8.Size = new System.Drawing.Size(142, 22);
            this.MenuItemDBEncodingUTF8.Text = "UTF-8";
            this.MenuItemDBEncodingUTF8.Click += new System.EventHandler(this.MenuItemDBEncoding_Click);
            // 
            // MenuItemDBEncodingUTF16
            // 
            this.MenuItemDBEncodingUTF16.Name = "MenuItemDBEncodingUTF16";
            this.MenuItemDBEncodingUTF16.Size = new System.Drawing.Size(142, 22);
            this.MenuItemDBEncodingUTF16.Text = "UTF-16";
            this.MenuItemDBEncodingUTF16.Click += new System.EventHandler(this.MenuItemDBEncoding_Click);
            // 
            // MenuItemDBEncodingISO8859
            // 
            this.MenuItemDBEncodingISO8859.Name = "MenuItemDBEncodingISO8859";
            this.MenuItemDBEncodingISO8859.Size = new System.Drawing.Size(142, 22);
            this.MenuItemDBEncodingISO8859.Text = "ISO-8859-1";
            this.MenuItemDBEncodingISO8859.Click += new System.EventHandler(this.MenuItemDBEncoding_Click);
            // 
            // MenuItemDBEncodingGB2312
            // 
            this.MenuItemDBEncodingGB2312.Name = "MenuItemDBEncodingGB2312";
            this.MenuItemDBEncodingGB2312.Size = new System.Drawing.Size(142, 22);
            this.MenuItemDBEncodingGB2312.Text = "GB2312";
            this.MenuItemDBEncodingGB2312.Click += new System.EventHandler(this.MenuItemDBEncoding_Click);
            // 
            // MenuItemDBEncodingBIG5
            // 
            this.MenuItemDBEncodingBIG5.Name = "MenuItemDBEncodingBIG5";
            this.MenuItemDBEncodingBIG5.Size = new System.Drawing.Size(142, 22);
            this.MenuItemDBEncodingBIG5.Text = "BIG5";
            this.MenuItemDBEncodingBIG5.Click += new System.EventHandler(this.MenuItemDBEncoding_Click);
            // 
            // MenuItemConfig
            // 
            this.MenuItemConfig.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuItemSettings,
            this.toolStripSeparator19,
            this.MenuItemRefreshURL,
            this.MenuItemEscapeCookie});
            this.MenuItemConfig.Name = "MenuItemConfig";
            this.MenuItemConfig.Size = new System.Drawing.Size(99, 21);
            this.MenuItemConfig.Text = "&Configuration";
            // 
            // MenuItemSettings
            // 
            this.MenuItemSettings.Image = global::Properties.Resources.Image_10;
            this.MenuItemSettings.Name = "MenuItemSettings";
            this.MenuItemSettings.Size = new System.Drawing.Size(287, 22);
            this.MenuItemSettings.Text = "Settings";
            this.MenuItemSettings.Click += new System.EventHandler(this.MenuItemSettings_Click);
            // 
            // toolStripSeparator19
            // 
            this.toolStripSeparator19.Name = "toolStripSeparator19";
            this.toolStripSeparator19.Size = new System.Drawing.Size(284, 6);
            // 
            // MenuItemRefreshURL
            // 
            this.MenuItemRefreshURL.Checked = true;
            this.MenuItemRefreshURL.CheckState = System.Windows.Forms.CheckState.Checked;
            this.MenuItemRefreshURL.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.MenuItemRefreshURL.Name = "MenuItemRefreshURL";
            this.MenuItemRefreshURL.Size = new System.Drawing.Size(287, 22);
            this.MenuItemRefreshURL.Text = "Refresh URL when Navigating";
            this.MenuItemRefreshURL.Click += new System.EventHandler(this.MenuItemRefreshURL_Click);
            // 
            // MenuItemEscapeCookie
            // 
            this.MenuItemEscapeCookie.Checked = true;
            this.MenuItemEscapeCookie.CheckState = System.Windows.Forms.CheckState.Checked;
            this.MenuItemEscapeCookie.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.MenuItemEscapeCookie.Name = "MenuItemEscapeCookie";
            this.MenuItemEscapeCookie.Size = new System.Drawing.Size(287, 22);
            this.MenuItemEscapeCookie.Text = "Escape Special Characters in Cookie";
            this.MenuItemEscapeCookie.Click += new System.EventHandler(this.MenuItemEscapeCookie_Click);
            // 
            // MenuItemHelp
            // 
            this.MenuItemHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuItemOnlineHelp,
            this.MenuItemOrder,
            this.MenuItemCheckUpdate,
            this.MenuItemFeedback,
            this.MenuItemWebsite,
            this.toolStripSeparator7,
            this.MenuItemAbout});
            this.MenuItemHelp.Name = "MenuItemHelp";
            this.MenuItemHelp.Size = new System.Drawing.Size(47, 21);
            this.MenuItemHelp.Text = "&Help";
            // 
            // MenuItemOnlineHelp
            // 
            this.MenuItemOnlineHelp.Name = "MenuItemOnlineHelp";
            this.MenuItemOnlineHelp.Size = new System.Drawing.Size(195, 22);
            this.MenuItemOnlineHelp.Text = "Online Help";
            this.MenuItemOnlineHelp.Click += new System.EventHandler(this.MenuItemOnlineHelp_Click);
            // 
            // MenuItemOrder
            // 
            this.MenuItemOrder.Name = "MenuItemOrder";
            this.MenuItemOrder.Size = new System.Drawing.Size(195, 22);
            this.MenuItemOrder.Text = "Order WebCruiser";
            this.MenuItemOrder.Click += new System.EventHandler(this.MenuItemOrder_Click);
            // 
            // MenuItemCheckUpdate
            // 
            this.MenuItemCheckUpdate.Name = "MenuItemCheckUpdate";
            this.MenuItemCheckUpdate.Size = new System.Drawing.Size(195, 22);
            this.MenuItemCheckUpdate.Text = "Check Updates";
            this.MenuItemCheckUpdate.Click += new System.EventHandler(this.MenuItemCheckUpdate_Click);
            // 
            // MenuItemFeedback
            // 
            this.MenuItemFeedback.Name = "MenuItemFeedback";
            this.MenuItemFeedback.Size = new System.Drawing.Size(195, 22);
            this.MenuItemFeedback.Text = "Feedback to Author";
            this.MenuItemFeedback.Click += new System.EventHandler(this.MenuItemFeedback_Click);
            // 
            // MenuItemWebsite
            // 
            this.MenuItemWebsite.Name = "MenuItemWebsite";
            this.MenuItemWebsite.Size = new System.Drawing.Size(195, 22);
            this.MenuItemWebsite.Text = "WebCruiser Website";
            this.MenuItemWebsite.Click += new System.EventHandler(this.MenuItemWebsite_Click);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(192, 6);
            // 
            // MenuItemAbout
            // 
            this.MenuItemAbout.Name = "MenuItemAbout";
            this.MenuItemAbout.Size = new System.Drawing.Size(195, 22);
            this.MenuItemAbout.Text = "About WebCruiser";
            this.MenuItemAbout.Click += new System.EventHandler(this.MenuItemAbout_Click);
            // 
            // MenuItemTextAd
            // 
            this.MenuItemTextAd.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.MenuItemTextAd.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MenuItemTextAd.ForeColor = System.Drawing.Color.Blue;
            this.MenuItemTextAd.Name = "MenuItemTextAd";
            this.MenuItemTextAd.Size = new System.Drawing.Size(150, 21);
            this.MenuItemTextAd.Text = "Janus Security Software";
            this.MenuItemTextAd.Click += new System.EventHandler(this.MenuItemTextAd_Click);
            this.MenuItemTextAd.MouseLeave += new System.EventHandler(this.MenuItemTextAd_MouseLeave);
            this.MenuItemTextAd.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MenuItemTextAd_MouseMove);
            // 
            // toolStripMain
            // 
            this.toolStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonNew,
            this.toolStripButtonOpen,
            this.toolStripButtonSave,
            this.toolStripSeparator4,
            this.toolStripButtonBrowser,
            this.toolStripButtonScanner,
            this.toolStripSeparator10,
            this.toolStripButtonSQL,
            this.toolStripSeparator20,
            this.toolStripButtonXSS,
            this.toolStripSeparator11,
            this.ButtonResend,
            this.ButtonCookie,
            this.toolStripSeparator17,
            this.ButtonReport,
            this.toolStripSeparator18,
            this.ButtonSetting,
            this.toolStripSeparator13,
            this.toolStripSeparator15,
            this.ButtonScanURL,
            this.toolStripSeparator14,
            this.ButtonScanSite});
            this.toolStripMain.Location = new System.Drawing.Point(0, 25);
            this.toolStripMain.Name = "toolStripMain";
            this.toolStripMain.Size = new System.Drawing.Size(792, 25);
            this.toolStripMain.TabIndex = 1;
            this.toolStripMain.Text = "toolStrip1";
            // 
            // toolStripButtonNew
            // 
            this.toolStripButtonNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonNew.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonNew.Image")));
            this.toolStripButtonNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonNew.Name = "toolStripButtonNew";
            this.toolStripButtonNew.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonNew.Text = "toolStripButton1";
            this.toolStripButtonNew.ToolTipText = "New Scan";
            this.toolStripButtonNew.Click += new System.EventHandler(this.toolStripButtonNew_Click);
            // 
            // toolStripButtonOpen
            // 
            this.toolStripButtonOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonOpen.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonOpen.Image")));
            this.toolStripButtonOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonOpen.Name = "toolStripButtonOpen";
            this.toolStripButtonOpen.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonOpen.Text = "toolStripButton2";
            this.toolStripButtonOpen.ToolTipText = "Open Existed Data";
            this.toolStripButtonOpen.Click += new System.EventHandler(this.toolStripButtonOpen_Click);
            // 
            // toolStripButtonSave
            // 
            this.toolStripButtonSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonSave.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonSave.Image")));
            this.toolStripButtonSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSave.Name = "toolStripButtonSave";
            this.toolStripButtonSave.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonSave.Text = "toolStripButton3";
            this.toolStripButtonSave.ToolTipText = "Save Current Data";
            this.toolStripButtonSave.Click += new System.EventHandler(this.toolStripButtonSave_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButtonBrowser
            // 
            this.toolStripButtonBrowser.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonBrowser.Image")));
            this.toolStripButtonBrowser.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonBrowser.Name = "toolStripButtonBrowser";
            this.toolStripButtonBrowser.Size = new System.Drawing.Size(76, 22);
            this.toolStripButtonBrowser.Text = "Browser";
            this.toolStripButtonBrowser.ToolTipText = "Web Browser";
            this.toolStripButtonBrowser.Click += new System.EventHandler(this.toolStripButtonBrowser_Click);
            // 
            // toolStripButtonScanner
            // 
            this.toolStripButtonScanner.Image = global::Properties.Resources.Image_3;
            this.toolStripButtonScanner.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonScanner.Name = "toolStripButtonScanner";
            this.toolStripButtonScanner.Size = new System.Drawing.Size(74, 22);
            this.toolStripButtonScanner.Text = "Scanner";
            this.toolStripButtonScanner.ToolTipText = "Vulnerability Scanner";
            this.toolStripButtonScanner.Click += new System.EventHandler(this.toolStripButtonScanner_Click);
            // 
            // toolStripSeparator10
            // 
            this.toolStripSeparator10.Name = "toolStripSeparator10";
            this.toolStripSeparator10.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButtonSQL
            // 
            this.toolStripButtonSQL.Image = global::Properties.Resources.Image_5;
            this.toolStripButtonSQL.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSQL.Name = "toolStripButtonSQL";
            this.toolStripButtonSQL.Size = new System.Drawing.Size(51, 22);
            this.toolStripButtonSQL.Text = "SQL";
            this.toolStripButtonSQL.ToolTipText = "SQL Injection";
            this.toolStripButtonSQL.Click += new System.EventHandler(this.toolStripButtonSQL_Click);
            // 
            // toolStripSeparator20
            // 
            this.toolStripSeparator20.Name = "toolStripSeparator20";
            this.toolStripSeparator20.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButtonXSS
            // 
            this.toolStripButtonXSS.Image = global::Properties.Resources.Image_6;
            this.toolStripButtonXSS.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonXSS.Name = "toolStripButtonXSS";
            this.toolStripButtonXSS.Size = new System.Drawing.Size(50, 22);
            this.toolStripButtonXSS.Text = "XSS";
            this.toolStripButtonXSS.ToolTipText = "Cross Site Scripting";
            this.toolStripButtonXSS.Click += new System.EventHandler(this.toolStripButtonXSS_Click);
            // 
            // toolStripSeparator11
            // 
            this.toolStripSeparator11.Name = "toolStripSeparator11";
            this.toolStripSeparator11.Size = new System.Drawing.Size(6, 25);
            // 
            // ButtonResend
            // 
            this.ButtonResend.Image = global::Properties.Resources.Image_23;
            this.ButtonResend.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ButtonResend.Name = "ButtonResend";
            this.ButtonResend.Size = new System.Drawing.Size(71, 22);
            this.ButtonResend.Text = "Resend";
            this.ButtonResend.ToolTipText = "Resend Tool";
            this.ButtonResend.Click += new System.EventHandler(this.ButtonResend_Click);
            // 
            // ButtonCookie
            // 
            this.ButtonCookie.Image = ((System.Drawing.Image)(resources.GetObject("ButtonCookie.Image")));
            this.ButtonCookie.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ButtonCookie.Name = "ButtonCookie";
            this.ButtonCookie.Size = new System.Drawing.Size(69, 22);
            this.ButtonCookie.Text = "Cookie";
            this.ButtonCookie.Click += new System.EventHandler(this.ButtonCookie_Click);
            // 
            // toolStripSeparator17
            // 
            this.toolStripSeparator17.Name = "toolStripSeparator17";
            this.toolStripSeparator17.Size = new System.Drawing.Size(6, 25);
            // 
            // ButtonReport
            // 
            this.ButtonReport.Image = global::Properties.Resources.Image_21;
            this.ButtonReport.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ButtonReport.Name = "ButtonReport";
            this.ButtonReport.Size = new System.Drawing.Size(68, 22);
            this.ButtonReport.Text = "Report";
            this.ButtonReport.Click += new System.EventHandler(this.toolStripButtonReport_Click);
            // 
            // toolStripSeparator18
            // 
            this.toolStripSeparator18.Name = "toolStripSeparator18";
            this.toolStripSeparator18.Size = new System.Drawing.Size(6, 25);
            // 
            // ButtonSetting
            // 
            this.ButtonSetting.Image = global::Properties.Resources.Image_10;
            this.ButtonSetting.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ButtonSetting.Name = "ButtonSetting";
            this.ButtonSetting.Size = new System.Drawing.Size(68, 22);
            this.ButtonSetting.Text = "Setting";
            this.ButtonSetting.Click += new System.EventHandler(this.toolStripButtonSetting_Click);
            // 
            // toolStripSeparator13
            // 
            this.toolStripSeparator13.Name = "toolStripSeparator13";
            this.toolStripSeparator13.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripSeparator15
            // 
            this.toolStripSeparator15.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripSeparator15.Name = "toolStripSeparator15";
            this.toolStripSeparator15.Size = new System.Drawing.Size(6, 25);
            // 
            // ButtonScanURL
            // 
            this.ButtonScanURL.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.ButtonScanURL.Image = global::Properties.Resources.Image_3;
            this.ButtonScanURL.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ButtonScanURL.Name = "ButtonScanURL";
            this.ButtonScanURL.Size = new System.Drawing.Size(82, 22);
            this.ButtonScanURL.Text = "Scan URL";
            this.ButtonScanURL.ToolTipText = "Scan Current URL";
            this.ButtonScanURL.Click += new System.EventHandler(this.ButtonScanURL_Click);
            // 
            // toolStripSeparator14
            // 
            this.toolStripSeparator14.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripSeparator14.Name = "toolStripSeparator14";
            this.toolStripSeparator14.Size = new System.Drawing.Size(6, 25);
            // 
            // ButtonScanSite
            // 
            this.ButtonScanSite.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.ButtonScanSite.Image = global::Properties.Resources.Image_3;
            this.ButtonScanSite.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ButtonScanSite.Name = "ButtonScanSite";
            this.ButtonScanSite.Size = new System.Drawing.Size(80, 21);
            this.ButtonScanSite.Text = "Scan Site";
            this.ButtonScanSite.ToolTipText = "Scan Current Site";
            this.ButtonScanSite.Click += new System.EventHandler(this.ButtonScanSite_Click);
            // 
            // statusStripMain
            // 
            this.statusStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusProgress,
            this.toolStripStatusSep1,
            this.lblThreadNum});
            this.statusStripMain.Location = new System.Drawing.Point(0, 494);
            this.statusStripMain.Name = "statusStripMain";
            this.statusStripMain.Size = new System.Drawing.Size(792, 22);
            this.statusStripMain.TabIndex = 2;
            this.statusStripMain.Text = "statusStrip1";
            // 
            // toolStripStatusProgress
            // 
            this.toolStripStatusProgress.AutoSize = false;
            this.toolStripStatusProgress.Name = "toolStripStatusProgress";
            this.toolStripStatusProgress.Size = new System.Drawing.Size(642, 17);
            this.toolStripStatusProgress.Text = "Done";
            this.toolStripStatusProgress.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // toolStripStatusSep1
            // 
            this.toolStripStatusSep1.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.toolStripStatusSep1.Name = "toolStripStatusSep1";
            this.toolStripStatusSep1.Size = new System.Drawing.Size(11, 17);
            this.toolStripStatusSep1.Text = "|";
            // 
            // lblThreadNum
            // 
            this.lblThreadNum.AutoSize = false;
            this.lblThreadNum.Name = "lblThreadNum";
            this.lblThreadNum.Size = new System.Drawing.Size(125, 17);
            this.lblThreadNum.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // toolStripURL
            // 
            this.toolStripURL.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.toolStripSeparator1,
            this.txtURL,
            this.toolStripSeparator3,
            this.cmbReqType,
            this.toolStripBtnGo,
            this.toolStripBtnPause,
            this.toolStripBtnStop});
            this.toolStripURL.Location = new System.Drawing.Point(0, 50);
            this.toolStripURL.Name = "toolStripURL";
            this.toolStripURL.Size = new System.Drawing.Size(792, 25);
            this.toolStripURL.TabIndex = 3;
            this.toolStripURL.Text = "toolStrip1";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(34, 22);
            this.toolStripLabel1.Text = "URL:";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // txtURL
            // 
            this.txtURL.AutoSize = false;
            this.txtURL.Name = "txtURL";
            this.txtURL.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.txtURL.Size = new System.Drawing.Size(581, 25);
            this.txtURL.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtURL_KeyPress);
            this.txtURL.DoubleClick += new System.EventHandler(this.txtURL_DoubleClick);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // cmbReqType
            // 
            this.cmbReqType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbReqType.Items.AddRange(new object[] {
            "GET",
            "POST",
            "COOKIE"});
            this.cmbReqType.Name = "cmbReqType";
            this.cmbReqType.Size = new System.Drawing.Size(75, 25);
            this.cmbReqType.ToolTipText = "Request Type";
            this.cmbReqType.DropDownClosed += new System.EventHandler(this.cmbReqType_DropDownClosed);
            // 
            // toolStripBtnGo
            // 
            this.toolStripBtnGo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripBtnGo.Image = ((System.Drawing.Image)(resources.GetObject("toolStripBtnGo.Image")));
            this.toolStripBtnGo.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripBtnGo.Name = "toolStripBtnGo";
            this.toolStripBtnGo.Size = new System.Drawing.Size(23, 22);
            this.toolStripBtnGo.Text = "toolStripButton1";
            this.toolStripBtnGo.ToolTipText = "Browser";
            this.toolStripBtnGo.Click += new System.EventHandler(this.toolStripBtnGo_Click);
            // 
            // toolStripBtnPause
            // 
            this.toolStripBtnPause.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripBtnPause.Image = ((System.Drawing.Image)(resources.GetObject("toolStripBtnPause.Image")));
            this.toolStripBtnPause.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripBtnPause.Name = "toolStripBtnPause";
            this.toolStripBtnPause.Size = new System.Drawing.Size(23, 22);
            this.toolStripBtnPause.Text = "toolStripButton1";
            this.toolStripBtnPause.ToolTipText = "Pause";
            this.toolStripBtnPause.Click += new System.EventHandler(this.toolStripBtnPause_Click);
            // 
            // toolStripBtnStop
            // 
            this.toolStripBtnStop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripBtnStop.Image = ((System.Drawing.Image)(resources.GetObject("toolStripBtnStop.Image")));
            this.toolStripBtnStop.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripBtnStop.Name = "toolStripBtnStop";
            this.toolStripBtnStop.Size = new System.Drawing.Size(23, 22);
            this.toolStripBtnStop.Text = "toolStripButton1";
            this.toolStripBtnStop.ToolTipText = "Stop";
            this.toolStripBtnStop.Click += new System.EventHandler(this.toolStripBtnStop_Click);
            // 
            // toolStripData
            // 
            this.toolStripData.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblSubmitData,
            this.toolStripSeparator2,
            this.txtSubmitData,
            this.toolStripSeparator16,
            this.ButtonAutoFill});
            this.toolStripData.Location = new System.Drawing.Point(0, 75);
            this.toolStripData.Name = "toolStripData";
            this.toolStripData.Size = new System.Drawing.Size(792, 25);
            this.toolStripData.TabIndex = 6;
            this.toolStripData.Visible = false;
            // 
            // lblSubmitData
            // 
            this.lblSubmitData.Name = "lblSubmitData";
            this.lblSubmitData.Size = new System.Drawing.Size(35, 22);
            this.lblSubmitData.Text = "Data";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // txtSubmitData
            // 
            this.txtSubmitData.AutoSize = false;
            this.txtSubmitData.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtSubmitData.Name = "txtSubmitData";
            this.txtSubmitData.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.txtSubmitData.Size = new System.Drawing.Size(650, 25);
            this.txtSubmitData.ToolTipText = resources.GetString("txtSubmitData.ToolTipText");
            this.txtSubmitData.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSubmitData_KeyPress);
            this.txtSubmitData.DoubleClick += new System.EventHandler(this.txtSubmitData_DoubleClick);
            // 
            // toolStripSeparator16
            // 
            this.toolStripSeparator16.Name = "toolStripSeparator16";
            this.toolStripSeparator16.Size = new System.Drawing.Size(6, 25);
            // 
            // ButtonAutoFill
            // 
            this.ButtonAutoFill.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ButtonAutoFill.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ButtonAutoFill.Name = "ButtonAutoFill";
            this.ButtonAutoFill.Size = new System.Drawing.Size(23, 22);
            this.ButtonAutoFill.Text = "Fill in Form";
            this.ButtonAutoFill.Click += new System.EventHandler(this.ButtonAutoFill_Click);
            // 
            // ScanTimer
            // 
            this.ScanTimer.Enabled = true;
            this.ScanTimer.Interval = 2500;
            this.ScanTimer.Tick += new System.EventHandler(this.ScanTimer_Tick);
            // 
            // AdTimer
            // 
            this.AdTimer.Enabled = true;
            this.AdTimer.Interval = 90000;
            this.AdTimer.Tick += new System.EventHandler(this.AdTimer_Tick);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(792, 516);
            this.Controls.Add(this.splitMain);
            this.Controls.Add(this.toolStripData);
            this.Controls.Add(this.toolStripURL);
            this.Controls.Add(this.statusStripMain);
            this.Controls.Add(this.toolStripMain);
            this.Controls.Add(this.menuStripMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStripMain;
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "WebCruiser - Web Vulnerability Scanner Personal Edition";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            this.Resize += new System.EventHandler(this.FormMain_Resize);
            this.splitMain.Panel1.ResumeLayout(false);
            this.splitMain.ResumeLayout(false);
            this.menuStripMain.ResumeLayout(false);
            this.menuStripMain.PerformLayout();
            this.toolStripMain.ResumeLayout(false);
            this.toolStripMain.PerformLayout();
            this.statusStripMain.ResumeLayout(false);
            this.statusStripMain.PerformLayout();
            this.toolStripURL.ResumeLayout(false);
            this.toolStripURL.PerformLayout();
            this.toolStripData.ResumeLayout(false);
            this.toolStripData.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void InitSetting()
        {
            WCRSetting.UseProxy = WebCruiserWVS.Default.UseProxy;
            WCRSetting.ProxyAddress = WebCruiserWVS.Default.ProxyAddress;
            WCRSetting.ProxyPort = WebCruiserWVS.Default.ProxyPort;
            WCRSetting.ProxyUsername = WebCruiserWVS.Default.ProxyUsername;
            WCRSetting.ProxyPassword = WebCruiserWVS.Default.ProxyPassword;
            WCRSetting.UserAgent = WebCruiserWVS.Default.UserAgent;
            WCRSetting.MaxHTTPThreadNum = WebCruiserWVS.Default.MaxHTTPThread;
            ThreadPool.SetMaxThreads(WCRSetting.MaxHTTPThreadNum + 10, (WCRSetting.MaxHTTPThreadNum + 10) * 2);
            WCRSetting.SecondsDelay =WebCruiserWVS.Default.SecondsDelay;
            WCRSetting.ScanSQLInjection =WebCruiserWVS.Default.ScanSQLInjection;
            WCRSetting.ScanXSS =WebCruiserWVS.Default.ScanXSS;
            WCRSetting.ScanXPathInjection =WebCruiserWVS.Default.ScanXPathInjection;
            WCRSetting.ScanURLSQL =WebCruiserWVS.Default.ScanURLSQL;
            WCRSetting.ScanPostSQL =WebCruiserWVS.Default.ScanPostSQL;
            WCRSetting.ScanCookieSQL =WebCruiserWVS.Default.ScanCookieSQL;
            WCRSetting.chkReplace1 =WebCruiserWVS.Default.chkReplace1;
            WCRSetting.FiltExpr1 =WebCruiserWVS.Default.FiltExpr1;
            WCRSetting.RepExpr1 =WebCruiserWVS.Default.RepExpr1;
            WCRSetting.chkReplace2 =WebCruiserWVS.Default.chkReplace2;
            WCRSetting.FiltExpr2 =WebCruiserWVS.Default.FiltExpr2;
            WCRSetting.RepExpr2 =WebCruiserWVS.Default.RepExpr2;
            WCRSetting.chkReplace3 =WebCruiserWVS.Default.chkReplace3;
            WCRSetting.FiltExpr3 =WebCruiserWVS.Default.FiltExpr3;
            WCRSetting.RepExpr3 =WebCruiserWVS.Default.RepExpr3;
            WCRSetting.Edition =WebCruiserWVS.Default.Edition;
            WCRSetting.ScanDepth =WebCruiserWVS.Default.ScanDepth;
            WCRSetting.CrawlableExt =WebCruiserWVS.Default.CrawlableExt;
            WCRSetting.MultiSitesNum =WebCruiserWVS.Default.MultiSitesNum;
            WCRSetting.CrossSiteURL =WebCruiserWVS.Default.CrossSiteURL;
            WCRSetting.CrossSiteRecord =WebCruiserWVS.Default.CrossSiteRecord;
        }

        private void InitTextAd()
        {
            this.AdTimer.Enabled = false;
            this.MenuItemTextAd.Visible = false;
        }

        private void LoadFromXmlDocument(XmlDocument WcrXml)
        {
            try
            {
                this.ScannerForm.LoadFromXmlDocument(WcrXml);
                this.SQLForm.LoadFromXmlDocument(WcrXml);
                this.URL = WcrXml.SelectSingleNode("//ROOT/CurrentSite/URL").Attributes["Value"].Value;
                string reqType = WcrXml.SelectSingleNode("//ROOT/CurrentSite/RequestType").Attributes["Value"].Value;
                this.UpdateComboReqType(reqType);
                this.ReqType = (RequestType) Enum.Parse(typeof(RequestType), reqType);
                this.SubmitData = WcrXml.SelectSingleNode("//ROOT/CurrentSite/SubmitData").Attributes["Value"].Value;
                this.CurrentSite.WebRoot = WcrXml.SelectSingleNode("//ROOT/CurrentSite/WebRoot").Attributes["Value"].Value;
                WebSite.EscapeCookie = bool.Parse(WcrXml.SelectSingleNode("//ROOT/CurrentSite/EscapeCookie").Attributes["Value"].Value);
                this.CookieForm.EscapeCookie(WebSite.EscapeCookie);
            }
            catch
            {
            }
        }

        private void MenuItemAbout_Click(object sender, EventArgs e)
        {
            this.SelectTool("About");
        }

        private void MenuItemCheckUpdate_Click(object sender, EventArgs e)
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(this.CheckUpdate));
        }

        private void MenuItemCookie_Click(object sender, EventArgs e)
        {
            this.SelectTool("Cookie");
        }

        private void MenuItemDBEncoding_Click(object sender, EventArgs e)
        {
            try
            {
                this.MenuItemDBEncodingUTF8.Checked = false;
                this.MenuItemDBEncodingUTF16.Checked = false;
                this.MenuItemDBEncodingISO8859.Checked = false;
                this.MenuItemDBEncodingGB2312.Checked = false;
                this.MenuItemDBEncodingBIG5.Checked = false;
                ((ToolStripMenuItem) sender).Checked = true;
                this.CurrentSite.DBEncoding = Encoding.GetEncoding(((ToolStripMenuItem) sender).Text);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void MenuItemEscapeCookie_Click(object sender, EventArgs e)
        {
            if (this.MenuItemEscapeCookie.Checked)
            {
                this.MenuItemEscapeCookie.Checked = false;
                WebSite.EscapeCookie = false;
                this.CookieForm.EscapeCookie(false);
            }
            else
            {
                this.MenuItemEscapeCookie.Checked = true;
                WebSite.EscapeCookie = true;
                this.CookieForm.EscapeCookie(true);
            }
        }

        private void MenuItemFeedback_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start("mailto:janusecurity@gmail.com");
            }
            catch
            {
            }
        }

        private void MenuItemNew_Click(object sender, EventArgs e)
        {
            this.NewScan();
        }

        private void MenuItemOnlineHelp_Click(object sender, EventArgs e)
        {
            string fileName = "http://sec4app.com/help.htm";
            try
            {
                Process.Start(fileName);
            }
            catch
            {
                this.NavigatePage(fileName, RequestType.GET, "");
            }
        }

        private void MenuItemOpen_Click(object sender, EventArgs e)
        {
            this.OpenWVSData();
        }

        private void MenuItemOrder_Click(object sender, EventArgs e)
        {
            string fileName = "http://sec4app.com/order.htm";
            try
            {
                Process.Start(fileName);
            }
            catch
            {
                this.NavigatePage(fileName, RequestType.GET, "");
            }
        }

        private void MenuItemRefreshURL_Click(object sender, EventArgs e)
        {
            if (this.MenuItemRefreshURL.Checked)
            {
                this.MenuItemRefreshURL.Checked = false;
                WCRSetting.RefreshURL = false;
            }
            else
            {
                this.MenuItemRefreshURL.Checked = true;
                WCRSetting.RefreshURL = true;
            }
        }

        private void MenuItemReport_Click(object sender, EventArgs e)
        {
            this.SelectTool("Report");
        }

        private void MenuItemResend_Click(object sender, EventArgs e)
        {
            this.SelectTool("Resend");
        }

        private void MenuItemSave_Click(object sender, EventArgs e)
        {
            this.SaveWVSData(false);
        }

        private void MenuItemSaveAs_Click(object sender, EventArgs e)
        {
            this.SaveWVSData(true);
        }

        private void MenuItemScanner_Click(object sender, EventArgs e)
        {
            this.SelectTool("Scanner");
        }

        private void MenuItemSetting_Click(object sender, EventArgs e)
        {
            this.SelectTool("Setting");
        }

        private void MenuItemSettings_Click(object sender, EventArgs e)
        {
            this.SelectTool("Setting");
        }

        private void MenuItemSQLInjection_Click(object sender, EventArgs e)
        {
            this.SelectTool("SQL");
        }

        private void MenuItemTextAd_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start(this.TextAdURL);
            }
            catch
            {
            }
            try
            {
                this.CurrentSite.GetSourceCode("http://sec4app.com/files/textadclick.php?id=" + this.MenuItemTextAd.Text, RequestType.GET);
            }
            catch
            {
            }
        }

        private void MenuItemTextAd_MouseLeave(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Arrow;
        }

        private void MenuItemTextAd_MouseMove(object sender, MouseEventArgs e)
        {
            this.Cursor = Cursors.Hand;
        }

        private void MenuItemWebBrowser_Click(object sender, EventArgs e)
        {
            this.SelectTool("WebBrowser");
        }

        private void MenuItemWebEncoding_Click(object sender, EventArgs e)
        {
            try
            {
                this.MenuItemWebEncodingUTF8.Checked = false;
                this.MenuItemWebEncodingUTF16.Checked = false;
                this.MenuItemWebEncodingISO8859.Checked = false;
                this.MenuItemWebEncodingGB2312.Checked = false;
                this.MenuItemWebEncodingBIG5.Checked = false;
                ((ToolStripMenuItem) sender).Checked = true;
                this.CurrentSite.WebEncoding = Encoding.GetEncoding(((ToolStripMenuItem) sender).Text);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void MenuItemWebsite_Click(object sender, EventArgs e)
        {
            string fileName = "http://sec4app.com/";
            try
            {
                Process.Start(fileName);
            }
            catch
            {
                this.NavigatePage(fileName, RequestType.GET, "");
            }
        }

        private void MenuItemXSS_Click(object sender, EventArgs e)
        {
            this.SelectTool("XSS");
        }

        public void NavigatePage(string sURL, RequestType ReqType, string SubmitData)
        {
            this.SelectTool("WebBrowser");
            this.BrowserForm.NavigatePage(sURL, ReqType, SubmitData);
        }

        private void NewScan()
        {
            string processName = Process.GetCurrentProcess().ProcessName;
            Process.Start(Application.ExecutablePath);
        }

        private void OpenWVSData()
        {
            OpenFileDialog dialog = new OpenFileDialog {
                Filter = "XML File(*.xml)|*.xml",
                InitialDirectory = Application.StartupPath
            };
            DialogResult result = dialog.ShowDialog();
            string fileName = dialog.FileName;
            dialog.Dispose();
            if (result == DialogResult.OK)
            {
                this.CurrentSite.WcrXml.Load(fileName);
                this.CurrentSite.WcrFileName = fileName;
                this.LoadFromXmlDocument(this.CurrentSite.WcrXml);
                this.WebBrowserGo();
            }
        }

        private void SaveWVSData(bool IsSaveAs)
        {
            this.UpdateXMLData(this.CurrentSite.WcrXml);
            string str = this.CurrentSite.Save(IsSaveAs);
            if (!string.IsNullOrEmpty(str))
            {
                this.DisplayProgress(str + " Saved!");
            }
        }

        private void ScanTimer_Tick(object sender, EventArgs e)
        {
            this.DisplayThreadNum("HTTP Thread: " + this.CurrentSite.HTTPThreadNum.ToString());
            DateTime now = DateTime.Now;
            if (now.Subtract(WebSite.StopTime).Seconds > 8)
            {
                if (WebSite.CurrentStatus == TaskStatus.Stop)
                {
                    WebSite.CurrentStatus = TaskStatus.Ready;
                    this.CurrentSite.HTTPThreadNum = 0;
                }
                this.toolStripBtnStop.Enabled = true;
            }
            TimeSpan span = now.Subtract(this.CurrentSite.LastGetTime);
            if ((span.Seconds > 30) && (this.CurrentSite.HTTPThreadNum == 0))
            {
                this.DisplayProgress("Done");
            }
            if ((span.Seconds > 30) && (this.CurrentSite.HTTPThreadNum > 0))
            {
                this.CurrentSite.HTTPThreadNum = 0;
            }
        }

        public void SelectCode(int Location, int Length)
        {
            this.CodeForm.SelectCode(Location, Length);
        }

        public void SelectTool(string ToolName)
        {
            switch (ToolName)
            {
                case "WebBrowser":
                    this.HideAllToolForm();
                    this.BrowserForm.Show();
                    this.BrowserForm.SelectTabByName("tabBrowser");
                    return;

                case "Scanner":
                    this.HideAllToolForm();
                    this.ScannerForm.Show();
                    return;

                case "POCTool":
                case "SystemTool":
                    break;

                case "SQL":
                    this.HideAllToolForm();
                    this.SQLForm.Show();
                    this.SQLForm.SelectTabByName("tabEnv");
                    return;

                case "XSS":
                    this.HideAllToolForm();
                    this.XSSForm.Show();
                    return;

                case "Code":
                    this.HideAllToolForm();
                    this.CodeForm.Show();
                    return;

                case "Cookie":
                    this.HideAllToolForm();
                    this.CookieForm.Show();
                    return;

                case "Setting":
                    this.HideAllToolForm();
                    this.SettingForm.Show();
                    return;

                case "Admin":
                    this.HideAllToolForm();
                    this.AdminForm.Show();
                    return;

                case "Report":
                    this.HideAllToolForm();
                    this.ReportForm.Show();
                    return;

                case "Resend":
                    this.HideAllToolForm();
                    this.BrowserForm.Show();
                    this.BrowserForm.SelectTabByName("tabResend");
                    return;

                case "StringTool":
                    this.HideAllToolForm();
                    this.SQLForm.Show();
                    this.SQLForm.SelectTabByName("tabEscapeString");
                    return;

                case "About":
                    this.HideAllToolForm();
                    this.AboutForm.Show();
                    return;

                default:
                    MessageBox.Show("Not Handled");
                    break;
            }
        }

        private void SetTextBoxText(TxtBoxInfo txtBoxInfo)
        {
            try
            {
                if (!txtBoxInfo.txtBox.InvokeRequired)
                {
                    txtBoxInfo.txtBox.Text = txtBoxInfo.Text;
                }
                else
                {
                    ddSetTextBox method = new ddSetTextBox(this.SetTextBoxText);
                    base.Invoke(method, new object[] { txtBoxInfo });
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void toolStripBtnGo_Click(object sender, EventArgs e)
        {
            this.WebBrowserGo();
        }

        private void toolStripBtnPause_Click(object sender, EventArgs e)
        {
            if (WebSite.CurrentStatus == TaskStatus.Ready)
            {
                WebSite.CurrentStatus = TaskStatus.Pause;
                this.toolStripBtnPause.ImageKey = "start.png";
                this.DisplayProgress("PAUSE");
            }
            else if (WebSite.CurrentStatus == TaskStatus.Pause)
            {
                WebSite.CurrentStatus = TaskStatus.Ready;
                this.toolStripBtnPause.ImageKey = "pause.png";
            }
        }

        private void toolStripBtnStop_Click(object sender, EventArgs e)
        {
            WebSite.CurrentStatus = TaskStatus.Stop;
            this.toolStripBtnStop.Enabled = false;
            this.DisplayProgress("Terminating Threads... ");
            WebSite.StopTime = DateTime.Now;
        }

        private void toolStripButtonBrowser_Click(object sender, EventArgs e)
        {
            this.SelectTool("WebBrowser");
 
        }

        private void toolStripButtonNew_Click(object sender, EventArgs e)
        {
            this.NewScan();
        }

        private void toolStripButtonOpen_Click(object sender, EventArgs e)
        {
            this.OpenWVSData();
        }

        private void toolStripButtonReport_Click(object sender, EventArgs e)
        {
            this.SelectTool("Report");
        }

        private void toolStripButtonSave_Click(object sender, EventArgs e)
        {
            this.SaveWVSData(false);
        }

        private void toolStripButtonScanner_Click(object sender, EventArgs e)
        {
            this.SelectTool("Scanner");
        }

        private void toolStripButtonSetting_Click(object sender, EventArgs e)
        {
            this.SelectTool("Setting");
        }

        private void toolStripButtonSQL_Click(object sender, EventArgs e)
        {
            this.SelectTool("SQL");
        }

        private void toolStripButtonXSS_Click(object sender, EventArgs e)
        {
            this.SelectTool("XSS");
        }

        private void treeViewToolTree_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            Point pt = new Point(e.X, e.Y);
            string name = this.treeViewToolTree.GetNodeAt(pt).Name;
            this.SelectTool(name);
        }

        private void txtSubmitData_DoubleClick(object sender, EventArgs e)
        {
            this.txtSubmitData.SelectAll();
        }

        private void txtSubmitData_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                this.WebBrowserGo();
            }
        }

        private void txtURL_DoubleClick(object sender, EventArgs e)
        {
            this.txtURL.SelectAll();
        }

        private void txtURL_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                this.WebBrowserGo();
            }
        }

        public void UpdateCodeText(string Code)
        {
            this.CodeForm.UpdateCodeText(Code);
        }

        private void UpdateComboReqType(string ReqType)
        {
            if (!this.toolStripURL.InvokeRequired)
            {
                this.cmbReqType.SelectedIndex = this.cmbReqType.FindString(ReqType);
            }
            else
            {
                dd method = new dd(this.UpdateComboReqType);
                base.Invoke(method, new object[] { ReqType });
            }
        }

        public void UpdateKeyWordText(string ItemText)
        {
            this.SQLForm.UpdateKeyWordText(ItemText);
        }

        public void UpdateSubmitData(string SubmitData)
        {
            if (!this.toolStripData.InvokeRequired)
            {
                this.txtSubmitData.Text = SubmitData;
            }
            else
            {
                dd method = new dd(this.UpdateSubmitData);
                base.Invoke(method, new object[] { SubmitData });
            }
        }

        private void UpdateTextBoxText(TextBox txtBox, string Text)
        {
            TxtBoxInfo info;
            info.txtBox = txtBox;
            info.Text = Text;
            this.SetTextBoxText(info);
        }

        public void UpdateURLText(string URLText)
        {
            if (!this.toolStripURL.InvokeRequired)
            {
                this.txtURL.Text = URLText;
            }
            else
            {
                dd method = new dd(this.UpdateURLText);
                base.Invoke(method, new object[] { URLText });
            }
        }

        public void UpdateXMLData(XmlDocument WcrXml)
        {
            try
            {
                XmlNode node = WcrXml.SelectSingleNode("//ROOT");
                XmlDocument currentSiteXml = this.GetCurrentSiteXml();
                XmlNode newChild = WcrXml.ImportNode(currentSiteXml.SelectSingleNode("//ROOT/CurrentSite"), true);
                XmlNode oldChild = WcrXml.SelectSingleNode("//ROOT/CurrentSite");
                if (oldChild == null)
                {
                    node.AppendChild(newChild);
                }
                else
                {
                    node.ReplaceChild(newChild, oldChild);
                }
                XmlDocument xmlDocumentFromDirTree = this.ScannerForm.GetXmlDocumentFromDirTree();
                XmlNode node4 = WcrXml.ImportNode(xmlDocumentFromDirTree.SelectSingleNode("//ROOT/SiteDirTree"), true);
                XmlNode node5 = WcrXml.SelectSingleNode("//ROOT/SiteDirTree");
                if (node5 == null)
                {
                    node.AppendChild(newChild);
                }
                else
                {
                    node.ReplaceChild(node4, node5);
                }
                XmlDocument xmlDocumentFromWVS = this.ScannerForm.GetXmlDocumentFromWVS();
                XmlNode node6 = WcrXml.ImportNode(xmlDocumentFromWVS.SelectSingleNode("//ROOT/SiteVulList"), true);
                XmlNode node7 = WcrXml.SelectSingleNode("//ROOT/SiteVulList");
                if (node7 == null)
                {
                    node.AppendChild(node6);
                }
                else
                {
                    node.ReplaceChild(node6, node7);
                }
                XmlDocument xmlDocumentFromDBTree = this.SQLForm.GetXmlDocumentFromDBTree();
                XmlNode node8 = WcrXml.ImportNode(xmlDocumentFromDBTree.SelectSingleNode("//ROOT/SiteDBStructure"), true);
                XmlNode node9 = WcrXml.SelectSingleNode("//ROOT/SiteDBStructure");
                if (node9 == null)
                {
                    node.AppendChild(node8);
                }
                else
                {
                    node.ReplaceChild(node8, node9);
                }
                XmlDocument xmlDocumentFromEnv = this.SQLForm.GetXmlDocumentFromEnv();
                XmlNode node10 = WcrXml.ImportNode(xmlDocumentFromEnv.SelectSingleNode("//ROOT/SiteSQLEnv"), true);
                XmlNode node11 = WcrXml.SelectSingleNode("//ROOT/SiteSQLEnv");
                if (node11 == null)
                {
                    node.AppendChild(node10);
                }
                else
                {
                    node.ReplaceChild(node10, node11);
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        public void URLTextBoxFocus()
        {
            this.txtURL.Focus();
        }

        private void WebBrowserGo()
        {
            this.SelectTool("WebBrowser");
            string uRLText = this.txtURL.Text.Trim();
            this.CurrentSite.URL = uRLText;
            string postData = "";
            string scanData = this.CurrentRequestType.ToString() + "  " + uRLText;
            if (this.CurrentRequestType != RequestType.GET)
            {
                postData = this.txtSubmitData.Text;
                scanData = scanData + "^" + postData;
                if (this.CurrentRequestType == RequestType.POST)
                {
                    postData = this.CurrentSite.ConvertPostData(postData);
                }
            }
            else if (uRLText.IndexOf('^') > 0)
            {
                if (MessageBox.Show("* URL is not a valid for GET Request.\r\n* Do you want it switch to POST?\r\n", "Confirm", MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk) == DialogResult.Cancel)
                {
                    return;
                }
                string[] strArray = uRLText.Split(new char[] { '^' });
                uRLText = strArray[0];
                string str4 = "";
                for (int i = 1; i < strArray.Length; i++)
                {
                    if (!string.IsNullOrEmpty(str4))
                    {
                        str4 = str4 + "^";
                    }
                    str4 = str4 + strArray[i];
                }
                this.CurrentRequestType = RequestType.POST;
                this.InitByRequestType(this.CurrentRequestType);
                this.UpdateURLText(uRLText);
                this.UpdateSubmitData(str4);
                postData = this.txtSubmitData.Text;
                postData = this.CurrentSite.ConvertPostData(postData);
            }
            this.BrowserForm.NavigatePage(uRLText, this.CurrentRequestType, postData);
            if (WebSite.LogScannedURL)
            {
                WebSite.LogScannedData(scanData);
            }
        }

        public void XPathPOC(string RefURL, string XPathForm, string Parameter)
        {
            this.BrowserForm.XPathPOC(RefURL, XPathForm, Parameter);
        }

        public void XSSPOC(string RefPage, string ActionURL)
        {
            this.XSSForm.XSSPOC(RefPage, ActionURL);
        }

        public RequestType ReqType
        {
            get
            {
                return this.CurrentRequestType;
            }
            set
            {
                this.CurrentRequestType = value;
                this.InitByRequestType(this.CurrentRequestType);
            }
        }

        public string SubmitData
        {
            get
            {
                return this.GetSubmitData();
            }
            set
            {
                this.UpdateSubmitData(value);
            }
        }

        public string URL
        {
            get
            {
                return this.GetCurrentURL();
            }
            set
            {
                this.CurrentSite.URL = value;
                this.UpdateURLText(value);
            }
        }

        private delegate void dd(string s);

        private delegate void ddd(FormScanner fm);

        private delegate void ddSetTextBox(FormMain.TxtBoxInfo txtBoxInfo);

        private delegate string ds();

        private delegate void EnableTextBox(bool TrueFalse);

        [StructLayout(LayoutKind.Sequential)]
        public struct TxtBoxInfo
        {
            public TextBox txtBox;
            public string Text;
        }

        private void splitMain_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}

