namespace WebCruiserWVS
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class FormCookie : Form
    {
        private ToolStripButton btnGetCookie;
        private ToolStripButton btnSetCookie;
        private ToolStripComboBox cmbEscapeCookie;
        private IContainer components;
        private FormMain mainfrm;
        private ToolStrip toolStripCookei;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripSeparator toolStripSeparator4;
        private TextBox txtCookie;

        public FormCookie(FormMain fm)
        {
            this.InitializeComponent();
            this.cmbEscapeCookie.SelectedIndex = 0;
            this.mainfrm = fm;
        }

        private void btnGetCookie_Click(object sender, EventArgs e)
        {
            try
            {
                this.txtCookie.Text = this.mainfrm.CurrentSite.GetCookieStrFromCC();
                this.btnSetCookie.Enabled = true;
            }
            catch (Exception)
            {
                MessageBox.Show("Please Navigate the Web site at first!  ", "Information");
            }
        }

        private void btnSetCookie_Click(object sender, EventArgs e)
        {
            char[] trimChars = new char[] { ' ', ';' };
            string str = this.txtCookie.Text.Trim(trimChars);
            if (!string.IsNullOrEmpty(str))
            {
                this.txtCookie.Text = str;
                this.mainfrm.CurrentSite.SetCookie(str);
            }
        }

        private void cmbEscapeCookie_DropDownClosed(object sender, EventArgs e)
        {
            if (this.cmbEscapeCookie.SelectedIndex == 0)
            {
                WebSite.EscapeCookie = true;
                this.mainfrm.EscapeCookie(true);
            }
            else
            {
                WebSite.EscapeCookie = false;
                this.mainfrm.EscapeCookie(false);
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

        public void EscapeCookie(bool IsEscape)
        {
            if (IsEscape)
            {
                this.cmbEscapeCookie.SelectedIndex = 0;
            }
            else
            {
                this.cmbEscapeCookie.SelectedIndex = 1;
            }
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormCookie));
            this.toolStripCookei = new ToolStrip();
            this.txtCookie = new TextBox();
            this.toolStripSeparator1 = new ToolStripSeparator();
            this.cmbEscapeCookie = new ToolStripComboBox();
            this.toolStripSeparator2 = new ToolStripSeparator();
            this.btnGetCookie = new ToolStripButton();
            this.toolStripSeparator3 = new ToolStripSeparator();
            this.btnSetCookie = new ToolStripButton();
            this.toolStripSeparator4 = new ToolStripSeparator();
            this.toolStripCookei.SuspendLayout();
            base.SuspendLayout();
            this.toolStripCookei.BackColor = SystemColors.ButtonFace;
            this.toolStripCookei.GripStyle = ToolStripGripStyle.Hidden;
            this.toolStripCookei.Items.AddRange(new ToolStripItem[] { this.toolStripSeparator1, this.cmbEscapeCookie, this.toolStripSeparator2, this.btnGetCookie, this.toolStripSeparator3, this.btnSetCookie, this.toolStripSeparator4 });
            this.toolStripCookei.Location = new Point(0, 0);
            this.toolStripCookei.Name = "toolStripCookei";
            this.toolStripCookei.Size = new Size(0x22b, 0x19);
            this.toolStripCookei.TabIndex = 4;
            this.toolStripCookei.Text = "toolStrip1";
            this.txtCookie.Dock = DockStyle.Fill;
            this.txtCookie.Location = new Point(0, 0x19);
            this.txtCookie.Margin = new Padding(3, 4, 3, 4);
            this.txtCookie.Multiline = true;
            this.txtCookie.Name = "txtCookie";
            this.txtCookie.Size = new Size(0x22b, 0x149);
            this.txtCookie.TabIndex = 5;
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new Size(6, 0x19);
            this.cmbEscapeCookie.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbEscapeCookie.Items.AddRange(new object[] { "Escape Special Characters(Default)", "Not Escape" });
            this.cmbEscapeCookie.Name = "cmbEscapeCookie";
            this.cmbEscapeCookie.Size = new Size(230, 0x19);
            this.cmbEscapeCookie.DropDownClosed += new EventHandler(this.cmbEscapeCookie_DropDownClosed);
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new Size(6, 0x19);
            this.btnGetCookie.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.btnGetCookie.Image = (Image)resources.GetObject("btnGetCookie.Image");
            this.btnGetCookie.ImageTransparentColor = Color.Magenta;
            this.btnGetCookie.Name = "btnGetCookie";
            this.btnGetCookie.Size = new Size(0x3f, 0x16);
            this.btnGetCookie.Text = "GetCookie";
            this.btnGetCookie.Click += new EventHandler(this.btnGetCookie_Click);
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new Size(6, 0x19);
            this.btnSetCookie.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.btnSetCookie.Enabled = false;
            this.btnSetCookie.Image = (Image)resources.GetObject("btnSetCookie.Image");
            this.btnSetCookie.ImageTransparentColor = Color.Magenta;
            this.btnSetCookie.Name = "btnSetCookie";
            this.btnSetCookie.Size = new Size(0x3f, 0x16);
            this.btnSetCookie.Text = "SetCookie";
            this.btnSetCookie.Click += new EventHandler(this.btnSetCookie_Click);
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new Size(6, 0x19);
            base.AutoScaleDimensions = new SizeF(6f, 12f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.ClientSize = new Size(0x22b, 0x162);
            base.Controls.Add(this.txtCookie);
            base.Controls.Add(this.toolStripCookei);
            base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            base.Name = "FormCookie";
            this.Text = "FormCookie";
            this.toolStripCookei.ResumeLayout(false);
            this.toolStripCookei.PerformLayout();
            base.ResumeLayout(false);
            base.PerformLayout();
        }
    }
}

