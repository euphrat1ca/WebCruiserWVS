namespace WebCruiserWVS
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class FormSetting : Form
    {
        private ToolStripButton ButtonSaveSetting;
        private CheckBox CheckBoxScanCookieSQL;
        private CheckBox CheckBoxScanPostSQL;
        private CheckBox CheckBoxScanURLSQL;
        private CheckBox CheckBoxSQLInjection;
        private CheckBox CheckBoxXPath;
        private CheckBox CheckBoxXSS;
        private CheckBox chkReplace1;
        private CheckBox chkReplace2;
        private CheckBox chkReplace3;
        private CheckBox chkUseProxy;
        private IContainer components;
        private GroupBox GroupBoxScanSetting;
        private GroupBox GroupBoxVulSetting;
        private Label label1;
        private Label label10;
        private Label label11;
        private Label label12;
        private Label label13;
        private Label label14;
        private Label label16;
        private Label label17;
        private Label label18;
        private Label label19;
        private Label label2;
        private Label label20;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private Label label8;
        private Label label9;
        private Label lblDelay;
        private TabPage tabAccess;
        private TabPage tabAdmin;
        private TabPage tabAdvanced;
        private TabPage tabProxy;
        private TabPage tabScanner;
        private TabControl tabSetting;
        private TabPage tabXSInj;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStrip toolStripSetting;
        private ToolTip ToolTipSetting;
        private TextBox txtAccessColumns;
        private TextBox txtAccessTables;
        private TextBox txtAdminPage;
        private TextBox txtAdminPath;
        private TextBox txtCrawlableExt;
        private TextBox txtDelay;
        private TextBox txtFiltExpr1;
        private TextBox txtFiltExpr2;
        private TextBox txtFiltExpr3;
        private TextBox txtMaxThread;
        private TextBox txtMultiSitesNum;
        private TextBox txtProxyAddr;
        private TextBox txtProxyPassword;
        private TextBox txtProxyPort;
        private TextBox txtProxyUsername;
        private TextBox txtRepExpr1;
        private TextBox txtRepExpr2;
        private TextBox txtRepExpr3;
        private TextBox txtScanDepth;
        private TextBox txtUserAgent;
        private TextBox txtXSInjURL;
        private TextBox txtXSinjUsage;
        private TextBox txtXSRecord;

        public FormSetting()
        {
            this.InitializeComponent();
            this.chkUseProxy.Checked = WebCruiserWVS.Default.UseProxy;
            this.txtProxyAddr.Text = WebCruiserWVS.Default.ProxyAddress;
            this.txtProxyPort.Text = WebCruiserWVS.Default.ProxyPort.ToString();
            this.txtProxyUsername.Text = WebCruiserWVS.Default.ProxyUsername;
            this.txtProxyPassword.Text = WebCruiserWVS.Default.ProxyPassword;
            this.txtUserAgent.Text = WebCruiserWVS.Default.UserAgent;
            this.txtMaxThread.Text = WebCruiserWVS.Default.MaxHTTPThread.ToString();
            this.txtDelay.Text = WebCruiserWVS.Default.SecondsDelay.ToString();
            this.chkReplace1.Checked = WebCruiserWVS.Default.chkReplace1;
            this.txtFiltExpr1.Text = WebCruiserWVS.Default.FiltExpr1;
            this.txtRepExpr1.Text = WebCruiserWVS.Default.RepExpr1;
            this.chkReplace2.Checked = WebCruiserWVS.Default.chkReplace2;
            this.txtFiltExpr2.Text = WebCruiserWVS.Default.FiltExpr2;
            this.txtRepExpr2.Text = WebCruiserWVS.Default.RepExpr2;
            this.chkReplace3.Checked = WebCruiserWVS.Default.chkReplace3;
            this.txtFiltExpr3.Text = WebCruiserWVS.Default.FiltExpr3;
            this.txtRepExpr3.Text = WebCruiserWVS.Default.RepExpr3;
            this.txtCrawlableExt.Text = WebCruiserWVS.Default.CrawlableExt;
            this.txtScanDepth.Text = WebCruiserWVS.Default.ScanDepth.ToString();
            this.txtMultiSitesNum.Text = WebCruiserWVS.Default.MultiSitesNum.ToString();
            this.CheckBoxSQLInjection.Checked = WebCruiserWVS.Default.ScanSQLInjection;
            this.CheckBoxScanURLSQL.Checked = WebCruiserWVS.Default.ScanURLSQL;
            this.CheckBoxScanPostSQL.Checked = WebCruiserWVS.Default.ScanPostSQL;
            this.CheckBoxScanCookieSQL.Checked = WebCruiserWVS.Default.ScanCookieSQL;
            this.EnableSQLInjectionCheckBox(this.CheckBoxSQLInjection.Checked);
            this.CheckBoxXSS.Checked = WebCruiserWVS.Default.ScanXSS;
            this.CheckBoxXPath.Checked = WebCruiserWVS.Default.ScanXPathInjection;
            foreach (string str in WebCruiserWVS.Default.AccessTables.Split(new char[] { ':' }))
            {
                this.txtAccessTables.Text = this.txtAccessTables.Text + str + "\r\n";
            }
            foreach (string str2 in WebCruiserWVS.Default.AccessColumns.Split(new char[] { ':' }))
            {
                this.txtAccessColumns.Text = this.txtAccessColumns.Text + str2 + "\r\n";
            }
            foreach (string str3 in WebCruiserWVS.Default.AdminPath.Split(new char[] { ':' }))
            {
                this.txtAdminPath.Text = this.txtAdminPath.Text + str3 + "\r\n";
            }
            foreach (string str4 in WebCruiserWVS.Default.AdminPage.Split(new char[] { ':' }))
            {
                this.txtAdminPage.Text = this.txtAdminPage.Text + str4 + "\r\n";
            }
            this.txtXSInjURL.Text = WebCruiserWVS.Default.CrossSiteURL;
            this.txtXSRecord.Text = WebCruiserWVS.Default.CrossSiteRecord;
        }

        private void ButtonSaveSetting_Click(object sender, EventArgs e)
        {
            try
            {
                WCRSetting.UseProxy = this.chkUseProxy.Checked;
                WebCruiserWVS.Default.UseProxy = WCRSetting.UseProxy;
                WCRSetting.ProxyAddress = this.txtProxyAddr.Text;
                WebCruiserWVS.Default.ProxyAddress = WCRSetting.ProxyAddress;
                WCRSetting.ProxyPort = int.Parse(this.txtProxyPort.Text);
                WebCruiserWVS.Default.ProxyPort = WCRSetting.ProxyPort;
                WCRSetting.ProxyUsername = this.txtProxyUsername.Text;
                WebCruiserWVS.Default.ProxyUsername = WCRSetting.ProxyUsername;
                WCRSetting.ProxyPassword = this.txtProxyPassword.Text;
                WebCruiserWVS.Default.ProxyPassword = WCRSetting.ProxyPassword;
                WCRSetting.chkReplace1 = this.chkReplace1.Checked;
                WebCruiserWVS.Default.chkReplace1 = WCRSetting.chkReplace1;
                WCRSetting.FiltExpr1 = this.txtFiltExpr1.Text;
                WebCruiserWVS.Default.FiltExpr1 = WCRSetting.FiltExpr1;
                WCRSetting.RepExpr1 = this.txtRepExpr1.Text;
                WebCruiserWVS.Default.RepExpr1 = WCRSetting.RepExpr1;
                WCRSetting.chkReplace2 = this.chkReplace2.Checked;
                WebCruiserWVS.Default.chkReplace2 = WCRSetting.chkReplace2;
                WCRSetting.FiltExpr2 = this.txtFiltExpr2.Text;
                WebCruiserWVS.Default.FiltExpr2 = WCRSetting.FiltExpr2;
                WCRSetting.RepExpr2 = this.txtRepExpr2.Text;
                WebCruiserWVS.Default.RepExpr2 = WCRSetting.RepExpr2;
                WCRSetting.chkReplace3 = this.chkReplace3.Checked;
                WebCruiserWVS.Default.chkReplace3 = WCRSetting.chkReplace3;
                WCRSetting.FiltExpr3 = this.txtFiltExpr3.Text;
                WebCruiserWVS.Default.FiltExpr3 = WCRSetting.FiltExpr3;
                WCRSetting.RepExpr3 = this.txtRepExpr3.Text;
                WebCruiserWVS.Default.RepExpr3 = WCRSetting.RepExpr3;
                WCRSetting.UserAgent = this.txtUserAgent.Text;
                WebCruiserWVS.Default.UserAgent = WCRSetting.UserAgent;
                WCRSetting.MaxHTTPThreadNum = int.Parse(this.txtMaxThread.Text);
                WebCruiserWVS.Default.MaxHTTPThread = WCRSetting.MaxHTTPThreadNum;
                WCRSetting.SecondsDelay = int.Parse(this.txtDelay.Text);
                WebCruiserWVS.Default.SecondsDelay = WCRSetting.SecondsDelay;
                WCRSetting.ScanDepth = int.Parse(this.txtScanDepth.Text);
                WebCruiserWVS.Default.ScanDepth = WCRSetting.ScanDepth;
                WCRSetting.CrawlableExt = this.txtCrawlableExt.Text;
                WCRSetting.CrawlableExt = WCRSetting.CrawlableExt.Replace(" ", "").Trim();
                WebCruiserWVS.Default.CrawlableExt = WCRSetting.CrawlableExt;
                WCRSetting.MultiSitesNum = int.Parse(this.txtMultiSitesNum.Text);
                WebCruiserWVS.Default.MultiSitesNum = WCRSetting.MultiSitesNum;
                WCRSetting.ScanSQLInjection = this.CheckBoxSQLInjection.Checked;
                WebCruiserWVS.Default.ScanSQLInjection = WCRSetting.ScanSQLInjection;
                if (WCRSetting.ScanSQLInjection)
                {
                    WCRSetting.ScanURLSQL = this.CheckBoxScanURLSQL.Checked;
                    WebCruiserWVS.Default.ScanURLSQL = WCRSetting.ScanURLSQL;
                    WCRSetting.ScanPostSQL = this.CheckBoxScanPostSQL.Checked;
                    WebCruiserWVS.Default.ScanPostSQL = WCRSetting.ScanPostSQL;
                    WCRSetting.ScanCookieSQL = this.CheckBoxScanCookieSQL.Checked;
                    WebCruiserWVS.Default.ScanCookieSQL = WCRSetting.ScanCookieSQL;
                }
                else
                {
                    WCRSetting.ScanURLSQL = false;
                    WebCruiserWVS.Default.ScanURLSQL = false;
                    WCRSetting.ScanPostSQL = false;
                    WebCruiserWVS.Default.ScanPostSQL = false;
                    WCRSetting.ScanCookieSQL = false;
                    WebCruiserWVS.Default.ScanCookieSQL = false;
                }
                WCRSetting.ScanXSS = this.CheckBoxXSS.Checked;
                WebCruiserWVS.Default.ScanXSS = WCRSetting.ScanXSS;
                WCRSetting.ScanXPathInjection = this.CheckBoxXPath.Checked;
                WebCruiserWVS.Default.ScanXPathInjection = WCRSetting.ScanXPathInjection;
                string str = this.txtAccessTables.Text.Trim().Replace("\r\n", ":").Replace(" ", "");
                WebCruiserWVS.Default.AccessTables = str;
                string str2 = this.txtAccessColumns.Text.Trim().Replace("\r\n", ":").Replace(" ", "");
                WebCruiserWVS.Default.AccessColumns = str2;
                string str3 = this.txtAdminPath.Text.Trim().Replace("\r\n", ":").Replace(" ", "");
                WebCruiserWVS.Default.AdminPath = str3;
                string str4 = this.txtAdminPage.Text.Trim().Replace("\r\n", ":").Replace(" ", "");
                WebCruiserWVS.Default.AdminPage = str4;
                string str5 = this.txtXSInjURL.Text.Trim();
                WCRSetting.CrossSiteURL = str5;
                WebCruiserWVS.Default.CrossSiteURL = str5;
                string str6 = this.txtXSRecord.Text.Trim();
                WCRSetting.CrossSiteRecord = str6;
                WebCruiserWVS.Default.CrossSiteRecord = str6;
                WebCruiserWVS.Default.Save();
                if (WCRSetting.UseProxy)
                {
                    WCRSetting.RefreshIESettings(WCRSetting.ProxyAddress + ":" + WCRSetting.ProxyPort.ToString());
                }
                else
                {
                    WCRSetting.RefreshIESettings("");
                }
                MessageBox.Show("Save/Apply OK!", "Done", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            catch (Exception exception)
            {
                MessageBox.Show("Please Check your Input!\r\n" + exception.ToString());
            }
        }

        private void CheckBoxSQLInjection_CheckedChanged(object sender, EventArgs e)
        {
            this.EnableSQLInjectionCheckBox(this.CheckBoxSQLInjection.Checked);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void EnableSQLInjectionCheckBox(bool CheckedState)
        {
            this.CheckBoxScanURLSQL.Enabled = CheckedState;
            this.CheckBoxScanPostSQL.Enabled = CheckedState;
            this.CheckBoxScanCookieSQL.Enabled = CheckedState;
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSetting));
            this.toolStripSetting = new System.Windows.Forms.ToolStrip();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.ButtonSaveSetting = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tabSetting = new System.Windows.Forms.TabControl();
            this.tabProxy = new System.Windows.Forms.TabPage();
            this.label10 = new System.Windows.Forms.Label();
            this.chkUseProxy = new System.Windows.Forms.CheckBox();
            this.txtProxyPassword = new System.Windows.Forms.TextBox();
            this.txtProxyUsername = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtProxyPort = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtProxyAddr = new System.Windows.Forms.TextBox();
            this.tabScanner = new System.Windows.Forms.TabPage();
            this.GroupBoxScanSetting = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtUserAgent = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.txtScanDepth = new System.Windows.Forms.TextBox();
            this.lblDelay = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.txtMaxThread = new System.Windows.Forms.TextBox();
            this.txtCrawlableExt = new System.Windows.Forms.TextBox();
            this.txtDelay = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.txtMultiSitesNum = new System.Windows.Forms.TextBox();
            this.GroupBoxVulSetting = new System.Windows.Forms.GroupBox();
            this.CheckBoxScanCookieSQL = new System.Windows.Forms.CheckBox();
            this.CheckBoxScanPostSQL = new System.Windows.Forms.CheckBox();
            this.CheckBoxScanURLSQL = new System.Windows.Forms.CheckBox();
            this.CheckBoxXPath = new System.Windows.Forms.CheckBox();
            this.CheckBoxXSS = new System.Windows.Forms.CheckBox();
            this.CheckBoxSQLInjection = new System.Windows.Forms.CheckBox();
            this.tabAdvanced = new System.Windows.Forms.TabPage();
            this.txtRepExpr3 = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.txtFiltExpr3 = new System.Windows.Forms.TextBox();
            this.chkReplace3 = new System.Windows.Forms.CheckBox();
            this.txtRepExpr2 = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtFiltExpr2 = new System.Windows.Forms.TextBox();
            this.chkReplace2 = new System.Windows.Forms.CheckBox();
            this.txtRepExpr1 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtFiltExpr1 = new System.Windows.Forms.TextBox();
            this.chkReplace1 = new System.Windows.Forms.CheckBox();
            this.tabAccess = new System.Windows.Forms.TabPage();
            this.txtAccessColumns = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.txtAccessTables = new System.Windows.Forms.TextBox();
            this.tabXSInj = new System.Windows.Forms.TabPage();
            this.txtXSRecord = new System.Windows.Forms.TextBox();
            this.txtXSInjURL = new System.Windows.Forms.TextBox();
            this.label20 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.txtXSinjUsage = new System.Windows.Forms.TextBox();
            this.tabAdmin = new System.Windows.Forms.TabPage();
            this.label17 = new System.Windows.Forms.Label();
            this.txtAdminPage = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.txtAdminPath = new System.Windows.Forms.TextBox();
            this.ToolTipSetting = new System.Windows.Forms.ToolTip(this.components);
            this.toolStripSetting.SuspendLayout();
            this.tabSetting.SuspendLayout();
            this.tabProxy.SuspendLayout();
            this.tabScanner.SuspendLayout();
            this.GroupBoxScanSetting.SuspendLayout();
            this.GroupBoxVulSetting.SuspendLayout();
            this.tabAdvanced.SuspendLayout();
            this.tabAccess.SuspendLayout();
            this.tabXSInj.SuspendLayout();
            this.tabAdmin.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripSetting
            // 
            this.toolStripSetting.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.toolStripSetting.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolStripSetting.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripSetting.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator1,
            this.ButtonSaveSetting,
            this.toolStripSeparator2});
            this.toolStripSetting.Location = new System.Drawing.Point(0, 425);
            this.toolStripSetting.Name = "toolStripSetting";
            this.toolStripSetting.Size = new System.Drawing.Size(1036, 25);
            this.toolStripSetting.TabIndex = 0;
            this.toolStripSetting.Text = "toolStrip1";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // ButtonSaveSetting
            // 
            this.ButtonSaveSetting.AutoSize = false;
            this.ButtonSaveSetting.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.ButtonSaveSetting.Image = ((System.Drawing.Image)(resources.GetObject("ButtonSaveSetting.Image")));
            this.ButtonSaveSetting.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ButtonSaveSetting.Name = "ButtonSaveSetting";
            this.ButtonSaveSetting.Size = new System.Drawing.Size(150, 22);
            this.ButtonSaveSetting.Text = "Save / Apply Settings";
            this.ButtonSaveSetting.Click += new System.EventHandler(this.ButtonSaveSetting_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // tabSetting
            // 
            this.tabSetting.Controls.Add(this.tabProxy);
            this.tabSetting.Controls.Add(this.tabScanner);
            this.tabSetting.Controls.Add(this.tabAdvanced);
            this.tabSetting.Controls.Add(this.tabAccess);
            this.tabSetting.Controls.Add(this.tabXSInj);
            this.tabSetting.Controls.Add(this.tabAdmin);
            this.tabSetting.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabSetting.Location = new System.Drawing.Point(0, 0);
            this.tabSetting.Name = "tabSetting";
            this.tabSetting.SelectedIndex = 0;
            this.tabSetting.Size = new System.Drawing.Size(1036, 425);
            this.tabSetting.TabIndex = 1;
            // 
            // tabProxy
            // 
            this.tabProxy.Controls.Add(this.label10);
            this.tabProxy.Controls.Add(this.chkUseProxy);
            this.tabProxy.Controls.Add(this.txtProxyPassword);
            this.tabProxy.Controls.Add(this.txtProxyUsername);
            this.tabProxy.Controls.Add(this.label4);
            this.tabProxy.Controls.Add(this.label3);
            this.tabProxy.Controls.Add(this.txtProxyPort);
            this.tabProxy.Controls.Add(this.label2);
            this.tabProxy.Controls.Add(this.label1);
            this.tabProxy.Controls.Add(this.txtProxyAddr);
            this.tabProxy.Location = new System.Drawing.Point(4, 22);
            this.tabProxy.Name = "tabProxy";
            this.tabProxy.Padding = new System.Windows.Forms.Padding(3);
            this.tabProxy.Size = new System.Drawing.Size(1028, 399);
            this.tabProxy.TabIndex = 0;
            this.tabProxy.Text = "Proxy";
            this.tabProxy.UseVisualStyleBackColor = true;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(6, 134);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(323, 12);
            this.label10.TabIndex = 9;
            this.label10.Text = "* Leave Username/Password Blank For Active Directory!";
            // 
            // chkUseProxy
            // 
            this.chkUseProxy.AutoSize = true;
            this.chkUseProxy.Location = new System.Drawing.Point(68, 17);
            this.chkUseProxy.Name = "chkUseProxy";
            this.chkUseProxy.Size = new System.Drawing.Size(132, 16);
            this.chkUseProxy.TabIndex = 8;
            this.chkUseProxy.Text = "Use a Proxy Server";
            this.chkUseProxy.UseVisualStyleBackColor = true;
            // 
            // txtProxyPassword
            // 
            this.txtProxyPassword.Location = new System.Drawing.Point(67, 105);
            this.txtProxyPassword.Name = "txtProxyPassword";
            this.txtProxyPassword.PasswordChar = '*';
            this.txtProxyPassword.Size = new System.Drawing.Size(129, 21);
            this.txtProxyPassword.TabIndex = 7;
            // 
            // txtProxyUsername
            // 
            this.txtProxyUsername.Location = new System.Drawing.Point(67, 76);
            this.txtProxyUsername.Name = "txtProxyUsername";
            this.txtProxyUsername.Size = new System.Drawing.Size(129, 21);
            this.txtProxyUsername.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 109);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 12);
            this.label4.TabIndex = 5;
            this.label4.Text = "Password:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 81);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "Username:";
            // 
            // txtProxyPort
            // 
            this.txtProxyPort.Location = new System.Drawing.Point(241, 47);
            this.txtProxyPort.Name = "txtProxyPort";
            this.txtProxyPort.Size = new System.Drawing.Size(44, 21);
            this.txtProxyPort.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(203, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "Port:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 51);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "Address:";
            // 
            // txtProxyAddr
            // 
            this.txtProxyAddr.Location = new System.Drawing.Point(67, 47);
            this.txtProxyAddr.Name = "txtProxyAddr";
            this.txtProxyAddr.Size = new System.Drawing.Size(129, 21);
            this.txtProxyAddr.TabIndex = 0;
            // 
            // tabScanner
            // 
            this.tabScanner.Controls.Add(this.GroupBoxScanSetting);
            this.tabScanner.Controls.Add(this.GroupBoxVulSetting);
            this.tabScanner.Location = new System.Drawing.Point(4, 22);
            this.tabScanner.Name = "tabScanner";
            this.tabScanner.Size = new System.Drawing.Size(1028, 399);
            this.tabScanner.TabIndex = 2;
            this.tabScanner.Text = "Scanner";
            this.tabScanner.UseVisualStyleBackColor = true;
            // 
            // GroupBoxScanSetting
            // 
            this.GroupBoxScanSetting.Controls.Add(this.label6);
            this.GroupBoxScanSetting.Controls.Add(this.txtUserAgent);
            this.GroupBoxScanSetting.Controls.Add(this.label13);
            this.GroupBoxScanSetting.Controls.Add(this.txtScanDepth);
            this.GroupBoxScanSetting.Controls.Add(this.lblDelay);
            this.GroupBoxScanSetting.Controls.Add(this.label14);
            this.GroupBoxScanSetting.Controls.Add(this.txtMaxThread);
            this.GroupBoxScanSetting.Controls.Add(this.txtCrawlableExt);
            this.GroupBoxScanSetting.Controls.Add(this.txtDelay);
            this.GroupBoxScanSetting.Controls.Add(this.label18);
            this.GroupBoxScanSetting.Controls.Add(this.label7);
            this.GroupBoxScanSetting.Controls.Add(this.txtMultiSitesNum);
            this.GroupBoxScanSetting.Location = new System.Drawing.Point(13, 5);
            this.GroupBoxScanSetting.Name = "GroupBoxScanSetting";
            this.GroupBoxScanSetting.Size = new System.Drawing.Size(490, 201);
            this.GroupBoxScanSetting.TabIndex = 35;
            this.GroupBoxScanSetting.TabStop = false;
            this.GroupBoxScanSetting.Text = "Parameter Setting";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(18, 28);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(71, 12);
            this.label6.TabIndex = 33;
            this.label6.Text = "User-Agent:";
            // 
            // txtUserAgent
            // 
            this.txtUserAgent.Location = new System.Drawing.Point(95, 25);
            this.txtUserAgent.Name = "txtUserAgent";
            this.txtUserAgent.Size = new System.Drawing.Size(358, 21);
            this.txtUserAgent.TabIndex = 34;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(18, 178);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(95, 12);
            this.label13.TabIndex = 0;
            this.label13.Text = "Scanning Depth:";
            // 
            // txtScanDepth
            // 
            this.txtScanDepth.Location = new System.Drawing.Point(114, 174);
            this.txtScanDepth.Name = "txtScanDepth";
            this.txtScanDepth.Size = new System.Drawing.Size(39, 21);
            this.txtScanDepth.TabIndex = 1;
            // 
            // lblDelay
            // 
            this.lblDelay.AutoSize = true;
            this.lblDelay.Location = new System.Drawing.Point(174, 178);
            this.lblDelay.Name = "lblDelay";
            this.lblDelay.Size = new System.Drawing.Size(233, 12);
            this.lblDelay.TabIndex = 32;
            this.lblDelay.Text = "Seconds Delay For Each Search Request:";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(18, 121);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(299, 12);
            this.label14.TabIndex = 2;
            this.label14.Text = "Crawling Pages With Extension (Seperated By \':\'):";
            // 
            // txtMaxThread
            // 
            this.txtMaxThread.Location = new System.Drawing.Point(413, 61);
            this.txtMaxThread.Name = "txtMaxThread";
            this.txtMaxThread.Size = new System.Drawing.Size(40, 21);
            this.txtMaxThread.TabIndex = 30;
            // 
            // txtCrawlableExt
            // 
            this.txtCrawlableExt.Location = new System.Drawing.Point(18, 139);
            this.txtCrawlableExt.Name = "txtCrawlableExt";
            this.txtCrawlableExt.Size = new System.Drawing.Size(435, 21);
            this.txtCrawlableExt.TabIndex = 3;
            // 
            // txtDelay
            // 
            this.txtDelay.Location = new System.Drawing.Point(413, 174);
            this.txtDelay.MaxLength = 4;
            this.txtDelay.Name = "txtDelay";
            this.txtDelay.Size = new System.Drawing.Size(39, 21);
            this.txtDelay.TabIndex = 31;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(18, 94);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(389, 12);
            this.label18.TabIndex = 5;
            this.label18.Text = "Maximum Site Thread Number for Multi-Site Scanning (Default: 3):";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(18, 64);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(389, 12);
            this.label7.TabIndex = 29;
            this.label7.Text = "Maximum HTTP Thread Number for Single-Site Scanning(Default: 5):";
            // 
            // txtMultiSitesNum
            // 
            this.txtMultiSitesNum.Location = new System.Drawing.Point(413, 88);
            this.txtMultiSitesNum.Name = "txtMultiSitesNum";
            this.txtMultiSitesNum.Size = new System.Drawing.Size(40, 21);
            this.txtMultiSitesNum.TabIndex = 6;
            // 
            // GroupBoxVulSetting
            // 
            this.GroupBoxVulSetting.Controls.Add(this.CheckBoxScanCookieSQL);
            this.GroupBoxVulSetting.Controls.Add(this.CheckBoxScanPostSQL);
            this.GroupBoxVulSetting.Controls.Add(this.CheckBoxScanURLSQL);
            this.GroupBoxVulSetting.Controls.Add(this.CheckBoxXPath);
            this.GroupBoxVulSetting.Controls.Add(this.CheckBoxXSS);
            this.GroupBoxVulSetting.Controls.Add(this.CheckBoxSQLInjection);
            this.GroupBoxVulSetting.Location = new System.Drawing.Point(13, 210);
            this.GroupBoxVulSetting.Name = "GroupBoxVulSetting";
            this.GroupBoxVulSetting.Size = new System.Drawing.Size(490, 158);
            this.GroupBoxVulSetting.TabIndex = 7;
            this.GroupBoxVulSetting.TabStop = false;
            this.GroupBoxVulSetting.Text = "Vulnerabilities Setting";
            // 
            // CheckBoxScanCookieSQL
            // 
            this.CheckBoxScanCookieSQL.AutoSize = true;
            this.CheckBoxScanCookieSQL.Location = new System.Drawing.Point(38, 92);
            this.CheckBoxScanCookieSQL.Name = "CheckBoxScanCookieSQL";
            this.CheckBoxScanCookieSQL.Size = new System.Drawing.Size(174, 16);
            this.CheckBoxScanCookieSQL.TabIndex = 5;
            this.CheckBoxScanCookieSQL.Text = "Scan Cookie SQL Injection";
            this.CheckBoxScanCookieSQL.UseVisualStyleBackColor = true;
            // 
            // CheckBoxScanPostSQL
            // 
            this.CheckBoxScanPostSQL.AutoSize = true;
            this.CheckBoxScanPostSQL.Location = new System.Drawing.Point(38, 67);
            this.CheckBoxScanPostSQL.Name = "CheckBoxScanPostSQL";
            this.CheckBoxScanPostSQL.Size = new System.Drawing.Size(162, 16);
            this.CheckBoxScanPostSQL.TabIndex = 4;
            this.CheckBoxScanPostSQL.Text = "Scan Post SQL Injection";
            this.CheckBoxScanPostSQL.UseVisualStyleBackColor = true;
            // 
            // CheckBoxScanURLSQL
            // 
            this.CheckBoxScanURLSQL.AutoSize = true;
            this.CheckBoxScanURLSQL.Location = new System.Drawing.Point(38, 42);
            this.CheckBoxScanURLSQL.Name = "CheckBoxScanURLSQL";
            this.CheckBoxScanURLSQL.Size = new System.Drawing.Size(156, 16);
            this.CheckBoxScanURLSQL.TabIndex = 3;
            this.CheckBoxScanURLSQL.Text = "Scan URL SQL Injection";
            this.CheckBoxScanURLSQL.UseVisualStyleBackColor = true;
            // 
            // CheckBoxXPath
            // 
            this.CheckBoxXPath.AutoSize = true;
            this.CheckBoxXPath.Location = new System.Drawing.Point(18, 139);
            this.CheckBoxXPath.Name = "CheckBoxXPath";
            this.CheckBoxXPath.Size = new System.Drawing.Size(144, 16);
            this.CheckBoxXPath.TabIndex = 2;
            this.CheckBoxXPath.Text = "Scan XPath Injection";
            this.CheckBoxXPath.UseVisualStyleBackColor = true;
            // 
            // CheckBoxXSS
            // 
            this.CheckBoxXSS.AutoSize = true;
            this.CheckBoxXSS.Location = new System.Drawing.Point(18, 114);
            this.CheckBoxXSS.Name = "CheckBoxXSS";
            this.CheckBoxXSS.Size = new System.Drawing.Size(174, 16);
            this.CheckBoxXSS.TabIndex = 1;
            this.CheckBoxXSS.Text = "Scan Cross Site Scripting";
            this.CheckBoxXSS.UseVisualStyleBackColor = true;
            // 
            // CheckBoxSQLInjection
            // 
            this.CheckBoxSQLInjection.AutoSize = true;
            this.CheckBoxSQLInjection.Location = new System.Drawing.Point(18, 20);
            this.CheckBoxSQLInjection.Name = "CheckBoxSQLInjection";
            this.CheckBoxSQLInjection.Size = new System.Drawing.Size(132, 16);
            this.CheckBoxSQLInjection.TabIndex = 0;
            this.CheckBoxSQLInjection.Text = "Scan SQL Injection";
            this.CheckBoxSQLInjection.UseVisualStyleBackColor = true;
            this.CheckBoxSQLInjection.CheckedChanged += new System.EventHandler(this.CheckBoxSQLInjection_CheckedChanged);
            // 
            // tabAdvanced
            // 
            this.tabAdvanced.Controls.Add(this.txtRepExpr3);
            this.tabAdvanced.Controls.Add(this.label12);
            this.tabAdvanced.Controls.Add(this.txtFiltExpr3);
            this.tabAdvanced.Controls.Add(this.chkReplace3);
            this.tabAdvanced.Controls.Add(this.txtRepExpr2);
            this.tabAdvanced.Controls.Add(this.label9);
            this.tabAdvanced.Controls.Add(this.txtFiltExpr2);
            this.tabAdvanced.Controls.Add(this.chkReplace2);
            this.tabAdvanced.Controls.Add(this.txtRepExpr1);
            this.tabAdvanced.Controls.Add(this.label5);
            this.tabAdvanced.Controls.Add(this.txtFiltExpr1);
            this.tabAdvanced.Controls.Add(this.chkReplace1);
            this.tabAdvanced.Location = new System.Drawing.Point(4, 22);
            this.tabAdvanced.Name = "tabAdvanced";
            this.tabAdvanced.Padding = new System.Windows.Forms.Padding(3);
            this.tabAdvanced.Size = new System.Drawing.Size(1028, 399);
            this.tabAdvanced.TabIndex = 1;
            this.tabAdvanced.Text = "Advanced";
            this.tabAdvanced.UseVisualStyleBackColor = true;
            // 
            // txtRepExpr3
            // 
            this.txtRepExpr3.Location = new System.Drawing.Point(169, 88);
            this.txtRepExpr3.Name = "txtRepExpr3";
            this.txtRepExpr3.Size = new System.Drawing.Size(90, 21);
            this.txtRepExpr3.TabIndex = 23;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(145, 92);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(17, 12);
            this.label12.TabIndex = 22;
            this.label12.Text = "By";
            // 
            // txtFiltExpr3
            // 
            this.txtFiltExpr3.Location = new System.Drawing.Point(88, 88);
            this.txtFiltExpr3.Name = "txtFiltExpr3";
            this.txtFiltExpr3.Size = new System.Drawing.Size(50, 21);
            this.txtFiltExpr3.TabIndex = 21;
            // 
            // chkReplace3
            // 
            this.chkReplace3.AutoSize = true;
            this.chkReplace3.Location = new System.Drawing.Point(24, 90);
            this.chkReplace3.Name = "chkReplace3";
            this.chkReplace3.Size = new System.Drawing.Size(66, 16);
            this.chkReplace3.TabIndex = 20;
            this.chkReplace3.Text = "Replace";
            this.chkReplace3.UseVisualStyleBackColor = true;
            // 
            // txtRepExpr2
            // 
            this.txtRepExpr2.Location = new System.Drawing.Point(169, 59);
            this.txtRepExpr2.Name = "txtRepExpr2";
            this.txtRepExpr2.Size = new System.Drawing.Size(90, 21);
            this.txtRepExpr2.TabIndex = 18;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(145, 63);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(17, 12);
            this.label9.TabIndex = 17;
            this.label9.Text = "By";
            // 
            // txtFiltExpr2
            // 
            this.txtFiltExpr2.Location = new System.Drawing.Point(88, 59);
            this.txtFiltExpr2.Name = "txtFiltExpr2";
            this.txtFiltExpr2.Size = new System.Drawing.Size(50, 21);
            this.txtFiltExpr2.TabIndex = 16;
            // 
            // chkReplace2
            // 
            this.chkReplace2.AutoSize = true;
            this.chkReplace2.Location = new System.Drawing.Point(24, 61);
            this.chkReplace2.Name = "chkReplace2";
            this.chkReplace2.Size = new System.Drawing.Size(66, 16);
            this.chkReplace2.TabIndex = 15;
            this.chkReplace2.Text = "Replace";
            this.chkReplace2.UseVisualStyleBackColor = true;
            // 
            // txtRepExpr1
            // 
            this.txtRepExpr1.Location = new System.Drawing.Point(169, 30);
            this.txtRepExpr1.Name = "txtRepExpr1";
            this.txtRepExpr1.Size = new System.Drawing.Size(90, 21);
            this.txtRepExpr1.TabIndex = 13;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(145, 34);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(17, 12);
            this.label5.TabIndex = 12;
            this.label5.Text = "By";
            // 
            // txtFiltExpr1
            // 
            this.txtFiltExpr1.Location = new System.Drawing.Point(88, 30);
            this.txtFiltExpr1.Name = "txtFiltExpr1";
            this.txtFiltExpr1.Size = new System.Drawing.Size(50, 21);
            this.txtFiltExpr1.TabIndex = 11;
            // 
            // chkReplace1
            // 
            this.chkReplace1.AutoSize = true;
            this.chkReplace1.Location = new System.Drawing.Point(24, 32);
            this.chkReplace1.Name = "chkReplace1";
            this.chkReplace1.Size = new System.Drawing.Size(66, 16);
            this.chkReplace1.TabIndex = 10;
            this.chkReplace1.Text = "Replace";
            this.chkReplace1.UseVisualStyleBackColor = true;
            // 
            // tabAccess
            // 
            this.tabAccess.Controls.Add(this.txtAccessColumns);
            this.tabAccess.Controls.Add(this.label11);
            this.tabAccess.Controls.Add(this.label8);
            this.tabAccess.Controls.Add(this.txtAccessTables);
            this.tabAccess.Location = new System.Drawing.Point(4, 22);
            this.tabAccess.Name = "tabAccess";
            this.tabAccess.Size = new System.Drawing.Size(1028, 399);
            this.tabAccess.TabIndex = 4;
            this.tabAccess.Text = "Access";
            this.tabAccess.UseVisualStyleBackColor = true;
            // 
            // txtAccessColumns
            // 
            this.txtAccessColumns.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.txtAccessColumns.Location = new System.Drawing.Point(241, 19);
            this.txtAccessColumns.Multiline = true;
            this.txtAccessColumns.Name = "txtAccessColumns";
            this.txtAccessColumns.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtAccessColumns.Size = new System.Drawing.Size(200, 362);
            this.txtAccessColumns.TabIndex = 3;
            this.ToolTipSetting.SetToolTip(this.txtAccessColumns, "Editable");
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(243, 4);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(119, 12);
            this.label11.TabIndex = 2;
            this.label11.Text = "Columns Dictionary:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(4, 4);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(113, 12);
            this.label8.TabIndex = 1;
            this.label8.Text = "Tables Dictionary:";
            // 
            // txtAccessTables
            // 
            this.txtAccessTables.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.txtAccessTables.Location = new System.Drawing.Point(3, 19);
            this.txtAccessTables.Multiline = true;
            this.txtAccessTables.Name = "txtAccessTables";
            this.txtAccessTables.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtAccessTables.Size = new System.Drawing.Size(200, 362);
            this.txtAccessTables.TabIndex = 0;
            this.ToolTipSetting.SetToolTip(this.txtAccessTables, "Editable");
            // 
            // tabXSInj
            // 
            this.tabXSInj.Controls.Add(this.txtXSRecord);
            this.tabXSInj.Controls.Add(this.txtXSInjURL);
            this.tabXSInj.Controls.Add(this.label20);
            this.tabXSInj.Controls.Add(this.label19);
            this.tabXSInj.Controls.Add(this.txtXSinjUsage);
            this.tabXSInj.Location = new System.Drawing.Point(4, 22);
            this.tabXSInj.Name = "tabXSInj";
            this.tabXSInj.Size = new System.Drawing.Size(1028, 399);
            this.tabXSInj.TabIndex = 6;
            this.tabXSInj.Text = "CrossSiteInjection";
            this.tabXSInj.UseVisualStyleBackColor = true;
            // 
            // txtXSRecord
            // 
            this.txtXSRecord.Location = new System.Drawing.Point(130, 41);
            this.txtXSRecord.Name = "txtXSRecord";
            this.txtXSRecord.Size = new System.Drawing.Size(351, 21);
            this.txtXSRecord.TabIndex = 4;
            // 
            // txtXSInjURL
            // 
            this.txtXSInjURL.Location = new System.Drawing.Point(130, 15);
            this.txtXSInjURL.Name = "txtXSInjURL";
            this.txtXSInjURL.Size = new System.Drawing.Size(351, 21);
            this.txtXSInjURL.TabIndex = 3;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(9, 44);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(113, 12);
            this.label20.TabIndex = 2;
            this.label20.Text = "Cross Site Record:";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(9, 17);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(95, 12);
            this.label19.TabIndex = 1;
            this.label19.Text = "Cross Site URL:";
            // 
            // txtXSinjUsage
            // 
            this.txtXSinjUsage.Location = new System.Drawing.Point(9, 68);
            this.txtXSinjUsage.Multiline = true;
            this.txtXSinjUsage.Name = "txtXSinjUsage";
            this.txtXSinjUsage.ReadOnly = true;
            this.txtXSinjUsage.Size = new System.Drawing.Size(472, 203);
            this.txtXSinjUsage.TabIndex = 0;
            this.txtXSinjUsage.Text = resources.GetString("txtXSinjUsage.Text");
            // 
            // tabAdmin
            // 
            this.tabAdmin.Controls.Add(this.label17);
            this.tabAdmin.Controls.Add(this.txtAdminPage);
            this.tabAdmin.Controls.Add(this.label16);
            this.tabAdmin.Controls.Add(this.txtAdminPath);
            this.tabAdmin.Location = new System.Drawing.Point(4, 22);
            this.tabAdmin.Name = "tabAdmin";
            this.tabAdmin.Size = new System.Drawing.Size(1028, 399);
            this.tabAdmin.TabIndex = 5;
            this.tabAdmin.Text = "AdminEntrance";
            this.tabAdmin.UseVisualStyleBackColor = true;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(240, 4);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(155, 12);
            this.label17.TabIndex = 3;
            this.label17.Text = "Admin Page(No Extension):";
            // 
            // txtAdminPage
            // 
            this.txtAdminPage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.txtAdminPage.Location = new System.Drawing.Point(241, 19);
            this.txtAdminPage.Multiline = true;
            this.txtAdminPage.Name = "txtAdminPage";
            this.txtAdminPage.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtAdminPage.Size = new System.Drawing.Size(200, 365);
            this.txtAdminPage.TabIndex = 2;
            this.ToolTipSetting.SetToolTip(this.txtAdminPage, "Editable");
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(5, 4);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(155, 12);
            this.label16.TabIndex = 1;
            this.label16.Text = "Admin Path(End with \'/\'):";
            // 
            // txtAdminPath
            // 
            this.txtAdminPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.txtAdminPath.Location = new System.Drawing.Point(3, 19);
            this.txtAdminPath.Multiline = true;
            this.txtAdminPath.Name = "txtAdminPath";
            this.txtAdminPath.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtAdminPath.Size = new System.Drawing.Size(200, 365);
            this.txtAdminPath.TabIndex = 0;
            this.ToolTipSetting.SetToolTip(this.txtAdminPath, "Editable");
            // 
            // FormSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1036, 450);
            this.Controls.Add(this.tabSetting);
            this.Controls.Add(this.toolStripSetting);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormSetting";
            this.Text = "FormSetting";
            this.toolStripSetting.ResumeLayout(false);
            this.toolStripSetting.PerformLayout();
            this.tabSetting.ResumeLayout(false);
            this.tabProxy.ResumeLayout(false);
            this.tabProxy.PerformLayout();
            this.tabScanner.ResumeLayout(false);
            this.GroupBoxScanSetting.ResumeLayout(false);
            this.GroupBoxScanSetting.PerformLayout();
            this.GroupBoxVulSetting.ResumeLayout(false);
            this.GroupBoxVulSetting.PerformLayout();
            this.tabAdvanced.ResumeLayout(false);
            this.tabAdvanced.PerformLayout();
            this.tabAccess.ResumeLayout(false);
            this.tabAccess.PerformLayout();
            this.tabXSInj.ResumeLayout(false);
            this.tabXSInj.PerformLayout();
            this.tabAdmin.ResumeLayout(false);
            this.tabAdmin.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}

