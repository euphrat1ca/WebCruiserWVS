namespace WebCruiserWVS
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Net;
    using System.Windows.Forms;

    public class FormCode : Form
    {
        private ToolStripButton btnGetCode;
        private ToolStripButton btnGetWBCode;
        private IContainer components;
        private FormMain mainfrm;
        private ToolStrip toolStripCode;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripSeparator toolStripSeparator3;
        private TextBox txtCode;

        public FormCode(FormMain fm)
        {
            this.InitializeComponent();
            this.mainfrm = fm;
        }

        private void btnGetCode_Click(object sender, EventArgs e)
        {
            string uRL = this.mainfrm.URL;
            this.txtCode.Clear();
            this.mainfrm.DisplayProgress("Getting Source ...");
            if (this.mainfrm.ReqType != RequestType.GET)
            {
                uRL = uRL + "^" + this.mainfrm.SubmitData;
            }
            try
            {
                HttpWebResponse httpWebResponse = this.mainfrm.CurrentSite.GetHttpWebResponse(uRL, this.mainfrm.ReqType);
                if (httpWebResponse != null)
                {
                    string sourceCodeFromHttpWebResponse = this.mainfrm.CurrentSite.GetSourceCodeFromHttpWebResponse(httpWebResponse);
                    this.txtCode.Text = sourceCodeFromHttpWebResponse;
                    this.mainfrm.DisplayProgress("HTTP Status: " + httpWebResponse.StatusCode.ToString());
                }
            }
            catch
            {
            }
        }

        private void btnGetWBCode_Click(object sender, EventArgs e)
        {
            this.txtCode.Clear();
            string sourceCodeFromWebBrowser = this.mainfrm.GetSourceCodeFromWebBrowser();
            this.txtCode.Text = sourceCodeFromWebBrowser;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormCode));
            this.toolStripCode = new ToolStrip();
            this.btnGetCode = new ToolStripButton();
            this.toolStripSeparator1 = new ToolStripSeparator();
            this.btnGetWBCode = new ToolStripButton();
            this.toolStripSeparator2 = new ToolStripSeparator();
            this.txtCode = new TextBox();
            this.toolStripSeparator3 = new ToolStripSeparator();
            this.toolStripCode.SuspendLayout();
            base.SuspendLayout();
            this.toolStripCode.BackColor = SystemColors.ButtonFace;
            this.toolStripCode.Dock = DockStyle.Bottom;
            this.toolStripCode.GripStyle = ToolStripGripStyle.Hidden;
            this.toolStripCode.Items.AddRange(new ToolStripItem[] { this.toolStripSeparator1, this.btnGetCode, this.toolStripSeparator2, this.btnGetWBCode, this.toolStripSeparator3 });
            this.toolStripCode.Location = new Point(0, 0x155);
            this.toolStripCode.Name = "toolStripCode";
            this.toolStripCode.Size = new Size(0x23f, 0x19);
            this.toolStripCode.TabIndex = 0;
            this.toolStripCode.Text = "toolStrip1";
            this.btnGetCode.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.btnGetCode.Image = (Image)resources.GetObject("btnGetCode.Image");
            this.btnGetCode.ImageTransparentColor = Color.Magenta;
            this.btnGetCode.Name = "btnGetCode";
            this.btnGetCode.Size = new Size(0x9f, 0x16);
            this.btnGetCode.Text = "Get Code From Current URL";
            this.btnGetCode.Click += new EventHandler(this.btnGetCode_Click);
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new Size(6, 0x19);
            this.btnGetWBCode.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.btnGetWBCode.Image = (Image)resources.GetObject("btnGetWBCode.Image");
            this.btnGetWBCode.ImageTransparentColor = Color.Magenta;
            this.btnGetWBCode.Name = "btnGetWBCode";
            this.btnGetWBCode.Size = new Size(0xb7, 0x16);
            this.btnGetWBCode.Text = "Get Code From Current Browser";
            this.btnGetWBCode.Click += new EventHandler(this.btnGetWBCode_Click);
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new Size(6, 0x19);
            this.txtCode.Dock = DockStyle.Fill;
            this.txtCode.HideSelection = false;
            this.txtCode.Location = new Point(0, 0);
            this.txtCode.Multiline = true;
            this.txtCode.Name = "txtCode";
            this.txtCode.ScrollBars = ScrollBars.Both;
            this.txtCode.Size = new Size(0x23f, 0x155);
            this.txtCode.TabIndex = 6;
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new Size(6, 0x19);
            base.AutoScaleDimensions = new SizeF(6f, 12f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.ClientSize = new Size(0x23f, 0x16e);
            base.Controls.Add(this.txtCode);
            base.Controls.Add(this.toolStripCode);
            this.Cursor = Cursors.Arrow;
            base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            base.Name = "FormCode";
            this.Text = "FormCode";
            this.toolStripCode.ResumeLayout(false);
            this.toolStripCode.PerformLayout();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        public void SelectCode(int Location, int Length)
        {
            this.txtCode.Select(Location, Length);
        }

        public void UpdateCodeText(string Code)
        {
            this.txtCode.Text = Code;
        }
    }
}

