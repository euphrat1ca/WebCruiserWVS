namespace WebCruiserWVS
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Net;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Windows.Forms;

    public class FormAdmin : Form
    {
        private ToolStripButton btnGetAdmin;
        private ToolStripButton btnSearchEngine;
        private ColumnHeader columnHeader4;
        private IContainer components;
        private ListView listViewAdminEntrance;
        private FormMain mainfrm;
        private ToolStrip toolStripAdmin;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripSeparator toolStripSeparator3;

        public FormAdmin(FormMain fm)
        {
            this.InitializeComponent();
            this.mainfrm = fm;
        }

        private void AddItem2listViewAdminEntrance(string ItemText)
        {
            if (!this.listViewAdminEntrance.InvokeRequired)
            {
                this.listViewAdminEntrance.Items.Add(ItemText);
            }
            else
            {
                dd method = new dd(this.AddItem2listViewAdminEntrance);
                base.Invoke(method, new object[] { ItemText });
            }
        }

        private void btnGetAdmin_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.mainfrm.URL))
            {
                MessageBox.Show("Please input the URL at first!", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else
            {
                this.listViewAdminEntrance.Items.Clear();
                if (!string.IsNullOrEmpty(this.mainfrm.CurrentSite.GetFileExt(this.mainfrm.URL)) || (MessageBox.Show("* Open a URL with a filename will help to find the admin entrance.\r\n* Such as: http://127.0.0.1/index.asp \r\n* Continue?\r\n", "Confirm", MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk) != DialogResult.Cancel))
                {
                    this.mainfrm.DisplayProgress("Getting Potential Adminstration Entrance...");
                    string[] strArray = WebCruiserWVS.Default.AdminPath.Split(new char[] { ':' });
                    int length = strArray.Length;
                    for (int i = 0; i < length; i++)
                    {
                        string state = strArray[i];
                        ThreadPool.QueueUserWorkItem(new WaitCallback(this.GetAdminDoWork), state);
                    }
                }
            }
        }

        private void btnSearchEngine_Click(object sender, EventArgs e)
        {
            try
            {
                this.mainfrm.DisplayProgress("Get Potential Admin Entrance By Search Engine...");
                string uRL = "http://www.google.com/search?as_occt=url&as_oq=admin&as_sitesearch=" + this.mainfrm.CurrentSite.DomainHost;
                string sourceCode = this.mainfrm.CurrentSite.GetSourceCode(uRL, RequestType.GET);
                foreach (string str3 in this.mainfrm.CurrentSite.GetLinksFromSource(sourceCode, "http://www.google.com/", ""))
                {
                    if (((str3.IndexOf(this.mainfrm.CurrentSite.HTTPRoot) >= 0) && (str3.IndexOf(this.mainfrm.CurrentSite.DomainHost) <= 10)) && (str3.IndexOf("admin") > 0))
                    {
                        this.AddItem2listViewAdminEntrance(str3);
                    }
                }
                this.mainfrm.DisplayProgress("Done");
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

        private void GetAdminDoWork(object data)
        {
            string str = (string) data;
            string sURL = this.mainfrm.CurrentSite.HTTPRoot + str;
            try
            {
                if (WebSite.CurrentStatus == TaskStatus.Stop)
                {
                    Thread.CurrentThread.Abort();
                }
                if (this.mainfrm.CurrentSite.GetHttpWebResponse(sURL, RequestType.GET).StatusCode == HttpStatusCode.OK)
                {
                    this.AddItem2listViewAdminEntrance(sURL);
                    if (!string.IsNullOrEmpty(this.mainfrm.CurrentSite.FileExt))
                    {
                        string[] strArray = WebCruiserWVS.Default.AdminPage.Split(new char[] { ':' });
                        int length = strArray.Length;
                        for (int i = 0; i < length; i++)
                        {
                            if (WebSite.CurrentStatus == TaskStatus.Stop)
                            {
                                return;
                            }
                            string str3 = sURL + strArray[i] + this.mainfrm.CurrentSite.FileExt;
                            if (this.mainfrm.CurrentSite.GetHttpWebResponse(str3, RequestType.GET).StatusCode == HttpStatusCode.OK)
                            {
                                this.AddItem2listViewAdminEntrance(str3);
                                this.mainfrm.DisplayProgress("Checking: " + str3 + " OK!");
                            }
                            else
                            {
                                this.mainfrm.DisplayProgress("Checking: " + str3 + " NotFound!");
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAdmin));
            this.toolStripAdmin = new ToolStrip();
            this.toolStripSeparator1 = new ToolStripSeparator();
            this.btnGetAdmin = new ToolStripButton();
            this.toolStripSeparator2 = new ToolStripSeparator();
            this.btnSearchEngine = new ToolStripButton();
            this.toolStripSeparator3 = new ToolStripSeparator();
            this.listViewAdminEntrance = new ListView();
            this.columnHeader4 = new ColumnHeader();
            this.toolStripAdmin.SuspendLayout();
            base.SuspendLayout();
            this.toolStripAdmin.BackColor = SystemColors.ButtonFace;
            this.toolStripAdmin.Dock = DockStyle.Bottom;
            this.toolStripAdmin.GripStyle = ToolStripGripStyle.Hidden;
            this.toolStripAdmin.Items.AddRange(new ToolStripItem[] { this.toolStripSeparator1, this.btnGetAdmin, this.toolStripSeparator2, this.btnSearchEngine, this.toolStripSeparator3 });
            this.toolStripAdmin.Location = new Point(0, 0x12a);
            this.toolStripAdmin.Name = "toolStripAdmin";
            this.toolStripAdmin.Size = new Size(0x1e5, 0x19);
            this.toolStripAdmin.TabIndex = 0;
            this.toolStripAdmin.Text = "toolStrip1";
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new Size(6, 0x19);
            this.btnGetAdmin.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.btnGetAdmin.Image = (Image)resources.GetObject("btnGetAdmin.Image");
            this.btnGetAdmin.ImageTransparentColor = Color.Magenta;
            this.btnGetAdmin.Name = "btnGetAdmin";
            this.btnGetAdmin.Size = new Size(0xab, 0x16);
            this.btnGetAdmin.Text = "Get Administration Entrance";
            this.btnGetAdmin.Click += new EventHandler(this.btnGetAdmin_Click);
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new Size(6, 0x19);
            this.btnSearchEngine.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.btnSearchEngine.Image = (Image)resources.GetObject("btnSearchEngine.Image");
            this.btnSearchEngine.ImageTransparentColor = Color.Magenta;
            this.btnSearchEngine.Name = "btnSearchEngine";
            this.btnSearchEngine.Size = new Size(0x93, 0x16);
            this.btnSearchEngine.Text = "Search By Search Engine";
            this.btnSearchEngine.Click += new EventHandler(this.btnSearchEngine_Click);
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new Size(6, 0x19);
            this.listViewAdminEntrance.Columns.AddRange(new ColumnHeader[] { this.columnHeader4 });
            this.listViewAdminEntrance.Dock = DockStyle.Fill;
            this.listViewAdminEntrance.Location = new Point(0, 0);
            this.listViewAdminEntrance.Name = "listViewAdminEntrance";
            this.listViewAdminEntrance.Size = new Size(0x1e5, 0x12a);
            this.listViewAdminEntrance.TabIndex = 2;
            this.listViewAdminEntrance.UseCompatibleStateImageBehavior = false;
            this.listViewAdminEntrance.View = View.Details;
            this.listViewAdminEntrance.MouseClick += new MouseEventHandler(this.listViewAdminEntrance_MouseClick);
            this.columnHeader4.Text = "Potential Adminstration Entrance";
            this.columnHeader4.Width = 710;
            base.AutoScaleDimensions = new SizeF(6f, 12f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.ClientSize = new Size(0x1e5, 0x143);
            base.Controls.Add(this.listViewAdminEntrance);
            base.Controls.Add(this.toolStripAdmin);
            base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            base.Name = "FormAdmin";
            this.Text = "FormAdmin";
            this.toolStripAdmin.ResumeLayout(false);
            this.toolStripAdmin.PerformLayout();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void listViewAdminEntrance_MouseClick(object sender, MouseEventArgs e)
        {
            if (this.listViewAdminEntrance.SelectedItems.Count >= 1)
            {
                ContextMenuStrip strip = new ContextMenuStrip();
                strip.Items.Add("Copy URL To ClipBoard", null, new EventHandler(this.listViewAdminEntranceItemClick));
                this.listViewAdminEntrance.ContextMenuStrip = strip;
            }
        }

        private void listViewAdminEntranceItemClick(object sender, EventArgs e)
        {
            try
            {
                string str;
                if (((str = ((ToolStripMenuItem) sender).Text) != null) && (str == "Copy URL To ClipBoard"))
                {
                    Clipboard.SetText(this.listViewAdminEntrance.SelectedItems[0].Text);
                }
            }
            catch
            {
            }
        }

        private delegate void dd(string s);
    }
}

