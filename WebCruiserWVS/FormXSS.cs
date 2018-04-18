namespace WebCruiserWVS
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Text.RegularExpressions;
    using System.Windows.Forms;

    public class FormXSS : Form
    {
        private Button btnAutoXSSTest;
        private Button btnGetFormFromBrowser;
        private Button btnOpenRefPage;
        private Button btnXSSTest;
        private ColumnHeader columnAction;
        private ColumnHeader columnPostData;
        private IContainer components;
        private Label label19;
        private Label label20;
        private Label label23;
        private ListView listViewForm;
        private FormMain mainfrm;
        private TextBox txtActionURL;
        private TextBox txtRefPage;
        private TextBox txtXSSUsage;

        public FormXSS(FormMain fm)
        {
            this.InitializeComponent();
            this.mainfrm = fm;
        }

        private void AddItem2ListViewForm(string ItemText)
        {
            if (!this.listViewForm.InvokeRequired)
            {
                string[] strArray = ItemText.Split(new char[] { '^' });
                ListViewItem item = new ListViewItem(strArray[0]);
                string str = "";
                for (int i = 1; i < strArray.Length; i++)
                {
                    if (!string.IsNullOrEmpty(str))
                    {
                        str = str + "^";
                    }
                    str = str + strArray[i];
                }
                item.SubItems.Add(str);
                this.listViewForm.Items.Add(item);
                this.listViewForm.Refresh();
            }
            else
            {
                dd method = new dd(this.AddItem2ListViewForm);
                base.Invoke(method, new object[] { ItemText });
            }
        }

        private void btnAutoXSSTest_Click(object sender, EventArgs e)
        {
            string uRL = this.mainfrm.URL;
            if (!string.IsNullOrEmpty(uRL))
            {
                this.txtRefPage.Text = uRL;
                string sourceCode = this.mainfrm.CurrentSite.GetSourceCode(uRL, RequestType.GET);
                foreach (string str3 in this.mainfrm.CurrentSite.GetFormInfo(sourceCode, uRL))
                {
                    string itemText = str3.Replace("!S!", "").Replace("!E!", "");
                    this.AddItem2ListViewForm(itemText);
                }
                foreach (string str5 in this.GetXSSURLInfo(uRL))
                {
                    if (!string.IsNullOrEmpty(str5))
                    {
                        this.mainfrm.AddItem2ListViewWVS(str5 + "^Cross Site Scripting(URL)");
                    }
                }
                if (WebSite.CurrentStatus != TaskStatus.Stop)
                {
                    foreach (string str6 in this.mainfrm.CurrentSite.GetFormVuls(uRL))
                    {
                        if (!string.IsNullOrEmpty(str6))
                        {
                            this.mainfrm.AddItem2ListViewWVS(str6);
                        }
                    }
                    this.mainfrm.SelectTool("Scanner");
                }
            }
        }

        private void btnGetFormFromBrowser_Click(object sender, EventArgs e)
        {
            string uRL = this.mainfrm.URL;
            string sourceCodeFromWebBrowser = this.mainfrm.GetSourceCodeFromWebBrowser();
            foreach (string str3 in this.mainfrm.CurrentSite.GetFormInfo(sourceCodeFromWebBrowser, uRL))
            {
                string itemText = WebSite.RemoveTestInput(str3);
                this.AddItem2ListViewForm(itemText);
            }
            int wCRBrowserFrameNum = this.mainfrm.GetWCRBrowserFrameNum();
            for (int i = 0; i < wCRBrowserFrameNum; i++)
            {
                try
                {
                    string wCRBrowserFrameSource = this.mainfrm.GetWCRBrowserFrameSource(i);
                    string wCRBrowserFrameURL = this.mainfrm.GetWCRBrowserFrameURL(i);
                    foreach (string str7 in this.mainfrm.CurrentSite.GetFormInfo(wCRBrowserFrameSource, wCRBrowserFrameURL))
                    {
                        string str8 = WebSite.RemoveTestInput(str7);
                        this.AddItem2ListViewForm(str8);
                    }
                }
                catch
                {
                }
            }
        }

        private void btnOpenRefPage_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.txtRefPage.Text))
            {
                this.mainfrm.NavigatePage(this.txtRefPage.Text, RequestType.GET, "");
            }
        }

        private void btnXSSTest_Click(object sender, EventArgs e)
        {
            string source = "";
            string str2 = this.txtActionURL.Text.Trim();
            if (string.IsNullOrEmpty(str2))
            {
                string uRL = this.mainfrm.URL;
                if (!string.IsNullOrEmpty(uRL))
                {
                    this.txtRefPage.Text = uRL;
                    if (uRL.IndexOf('?') > 0)
                    {
                        this.AddItem2ListViewForm(uRL);
                    }
                    source = this.mainfrm.CurrentSite.GetSourceCode(uRL, RequestType.GET);
                    foreach (string str4 in this.mainfrm.CurrentSite.GetFormInfo(source, uRL))
                    {
                        str2 = WebSite.RemoveTestInput(str4);
                        this.AddItem2ListViewForm(str2);
                    }
                }
            }
            else
            {
                RequestType pOST;
                str2.IndexOf('^');
                str2.IndexOf('=');
                if (str2.IndexOf('^') > 0)
                {
                    pOST = RequestType.POST;
                }
                else
                {
                    pOST = RequestType.GET;
                }
                string str5 = "";
                if (str2.IndexOf("<>") > 0)
                {
                    str5 = "<>";
                }
                else if (str2.IndexOf("<script>") > 0)
                {
                    int startIndex = str2.IndexOf("<script>");
                    int num2 = str2.IndexOf("</script>");
                    str5 = str2.Substring(startIndex, (num2 + 9) - startIndex);
                }
                source = this.mainfrm.CurrentSite.GetSourceCode(str2, pOST);
                this.mainfrm.UpdateCodeText(source);
                int index = source.IndexOf(str5);
                if (index >= 0)
                {
                    this.mainfrm.SelectTool("Code");
                    this.mainfrm.SelectCode(index, str5.Length);
                    MessageBox.Show("* XSS Data Has Been Submitted, Please Check Your Input In The Response Code Box !\r\n", "Done", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
                else
                {
                    MessageBox.Show("* XSS Data Has Been Submitted, Please Check The Response In The Refer Page !\r\n* Click the button \"Open Refer Page\" to view its response !", "Done", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
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

        private string GetKeyTextFromSource(string SourceCode, int Index)
        {
            string str = "!S!WCRTESTINPUT" + string.Format("{0:D6}", Index);
            if (SourceCode.IndexOf(str) < 0)
            {
                str = "!S!WCRTESTTEXTAREA" + string.Format("{0:D6}", Index);
            }
            Regex regex = new Regex("(?<=(" + str + @"))[.\s\S]*?(?=(!E!))", RegexOptions.Singleline | RegexOptions.Multiline);
            return regex.Match(SourceCode).Value;
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
                    if (!this.mainfrm.CurrentSite.IsScannedParameter(uRLPara))
                    {
                        this.mainfrm.CurrentSite.AddScannedParameter(uRLPara);
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
                        string sourceCode = this.mainfrm.CurrentSite.GetSourceCode(uRL, RequestType.GET);
                        string keyTextFromSource = this.GetKeyTextFromSource(sourceCode, i);
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormXSS));
            this.label23 = new Label();
            this.listViewForm = new ListView();
            this.columnAction = new ColumnHeader();
            this.columnPostData = new ColumnHeader();
            this.txtRefPage = new TextBox();
            this.label19 = new Label();
            this.txtActionURL = new TextBox();
            this.label20 = new Label();
            this.btnAutoXSSTest = new Button();
            this.txtXSSUsage = new TextBox();
            this.btnOpenRefPage = new Button();
            this.btnXSSTest = new Button();
            this.btnGetFormFromBrowser = new Button();
            base.SuspendLayout();
            this.label23.AutoSize = true;
            this.label23.Location = new Point(5, 0x36);
            this.label23.Name = "label23";
            this.label23.Size = new Size(0x4d, 12);
            this.label23.TabIndex = 0x15;
            this.label23.Text = "URL / Forms:";
            this.listViewForm.Columns.AddRange(new ColumnHeader[] { this.columnAction, this.columnPostData });
            this.listViewForm.FullRowSelect = true;
            this.listViewForm.Location = new Point(5, 0x48);
            this.listViewForm.Name = "listViewForm";
            this.listViewForm.Size = new Size(0x273, 0x74);
            this.listViewForm.TabIndex = 20;
            this.listViewForm.UseCompatibleStateImageBehavior = false;
            this.listViewForm.View = View.Details;
            this.listViewForm.Click += new EventHandler(this.listViewForm_Click);
            this.columnAction.Text = "ActionURL";
            this.columnAction.Width = 230;
            this.columnPostData.Text = "PostData";
            this.columnPostData.Width = 380;
            this.txtRefPage.Location = new Point(5, 0x1d);
            this.txtRefPage.Name = "txtRefPage";
            this.txtRefPage.ReadOnly = true;
            this.txtRefPage.Size = new Size(0x273, 0x15);
            this.txtRefPage.TabIndex = 0x13;
            this.label19.AutoSize = true;
            this.label19.Location = new Point(5, 9);
            this.label19.Name = "label19";
            this.label19.Size = new Size(0x47, 12);
            this.label19.TabIndex = 0x12;
            this.label19.Text = "Refer Page:";
            this.txtActionURL.Location = new Point(5, 0xce);
            this.txtActionURL.Multiline = true;
            this.txtActionURL.Name = "txtActionURL";
            this.txtActionURL.Size = new Size(0x273, 0x53);
            this.txtActionURL.TabIndex = 0x11;
            this.label20.AutoSize = true;
            this.label20.Location = new Point(5, 0xbf);
            this.label20.Name = "label20";
            this.label20.Size = new Size(0xef, 12);
            this.label20.TabIndex = 0x10;
            this.label20.Text = "ActionURL( ? For Get, and  ^ For POST):";
            this.btnAutoXSSTest.Location = new Point(5, 0x126);
            this.btnAutoXSSTest.Name = "btnAutoXSSTest";
            this.btnAutoXSSTest.Size = new Size(0x61, 0x17);
            this.btnAutoXSSTest.TabIndex = 0x19;
            this.btnAutoXSSTest.Text = "Auto XSS Test";
            this.btnAutoXSSTest.UseVisualStyleBackColor = true;
            this.btnAutoXSSTest.Click += new EventHandler(this.btnAutoXSSTest_Click);
            this.txtXSSUsage.Location = new Point(5, 0x141);
            this.txtXSSUsage.Multiline = true;
            this.txtXSSUsage.Name = "txtXSSUsage";
            this.txtXSSUsage.ReadOnly = true;
            this.txtXSSUsage.Size = new Size(0x273, 0x5f);
            this.txtXSSUsage.TabIndex = 0x18;
            this.txtXSSUsage.Text = resources.GetString("txtXSSUsage.Text");
            this.btnOpenRefPage.Location = new Point(0x200, 0x126);
            this.btnOpenRefPage.Name = "btnOpenRefPage";
            this.btnOpenRefPage.Size = new Size(120, 0x17);
            this.btnOpenRefPage.TabIndex = 0x17;
            this.btnOpenRefPage.Text = "Open Refer Page";
            this.btnOpenRefPage.UseVisualStyleBackColor = true;
            this.btnOpenRefPage.Click += new EventHandler(this.btnOpenRefPage_Click);
            this.btnXSSTest.Location = new Point(0x66, 0x126);
            this.btnXSSTest.Name = "btnXSSTest";
            this.btnXSSTest.Size = new Size(0xeb, 0x17);
            this.btnXSSTest.TabIndex = 0x16;
            this.btnXSSTest.Text = " Get Forms From URL / Manual XSS Test";
            this.btnXSSTest.UseVisualStyleBackColor = true;
            this.btnXSSTest.Click += new EventHandler(this.btnXSSTest_Click);
            this.btnGetFormFromBrowser.Location = new Point(0x151, 0x126);
            this.btnGetFormFromBrowser.Name = "btnGetFormFromBrowser";
            this.btnGetFormFromBrowser.Size = new Size(0xaf, 0x17);
            this.btnGetFormFromBrowser.TabIndex = 0x1a;
            this.btnGetFormFromBrowser.Text = "Get Forms From WebBrowser";
            this.btnGetFormFromBrowser.UseVisualStyleBackColor = true;
            this.btnGetFormFromBrowser.Click += new EventHandler(this.btnGetFormFromBrowser_Click);
            base.AutoScaleDimensions = new SizeF(6f, 12f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.ClientSize = new Size(750, 0x1a9);
            base.Controls.Add(this.btnGetFormFromBrowser);
            base.Controls.Add(this.btnAutoXSSTest);
            base.Controls.Add(this.txtXSSUsage);
            base.Controls.Add(this.btnOpenRefPage);
            base.Controls.Add(this.btnXSSTest);
            base.Controls.Add(this.label23);
            base.Controls.Add(this.listViewForm);
            base.Controls.Add(this.txtRefPage);
            base.Controls.Add(this.label19);
            base.Controls.Add(this.txtActionURL);
            base.Controls.Add(this.label20);
            base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            base.Name = "FormXSS";
            this.Text = "FormXSS";
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void listViewForm_Click(object sender, EventArgs e)
        {
            try
            {
                string text = this.listViewForm.SelectedItems[0].Text;
                if ((this.listViewForm.SelectedItems[0].SubItems.Count > 1) && !string.IsNullOrEmpty(this.listViewForm.SelectedItems[0].SubItems[1].Text))
                {
                    text = text + "^" + this.listViewForm.SelectedItems[0].SubItems[1].Text;
                }
                this.txtActionURL.Text = text;
            }
            catch
            {
            }
        }

        public void XSSPOC(string RefPage, string ActionURL)
        {
            this.txtRefPage.Text = RefPage;
            this.txtActionURL.Text = ActionURL;
            this.listViewForm.Items.Clear();
            this.AddItem2ListViewForm(ActionURL);
        }

        private delegate void dd(string s);
    }
}

