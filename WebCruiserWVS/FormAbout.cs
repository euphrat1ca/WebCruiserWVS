namespace WebCruiserWVS
{
    using Microsoft.Win32;
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.Windows.Forms;

    public class FormAbout : Form
    {
        private Button btnReg;
        private IContainer components;
        private Label lblRegCode;
        private Label lblRegInfo;
        private Label lblRegUsername;
        private LinkLabel linkLabelMail;
        private LinkLabel linkLabelSite2;
        private LinkLabel linkLblBuy;
        private LinkLabel linkLblSite;
        private LinkLabel linkPaypal;
        private FormMain mainfrm;
        private SplitContainer splitAbout;
        private TextBox txtHelp;
        private TextBox txtRegCode;
        private TextBox txtRegUsername;

        public FormAbout(FormMain fm)
        {
            this.InitializeComponent();
            this.mainfrm = fm;
        }

        private void btnReg_Click(object sender, EventArgs e)
        {
            string str = this.txtRegUsername.Text.Trim();
            if (string.IsNullOrEmpty(str) || (str.Length < 2))
            {
                MessageBox.Show("Username should has at least 2 letters!");
            }
            else
            {
                string text = this.txtRegCode.Text;
                if (string.IsNullOrEmpty(text))
                {
                    MessageBox.Show("RegCode can not be null!");
                }
                else if (Reg.ValidateRegCode(str, text) || Reg.ValidateRegCode2(str, text))
                {
                    RegistryKey key = Registry.CurrentUser.CreateSubKey(@"Software\Sec4App\WebCruiser");
                    key.SetValue("Username", str);
                    key.SetValue("RegCode", text);
                    string str3 = Reg.Encrypt(DateTime.Now.ToString("yyyy-MM-dd"));
                    key.SetValue("InitDate", str3);
                    this.lblRegInfo.Text = "This copy of WebCruiser is licensed to: " + str;
                    MessageBox.Show("Thank You For Registration!", "Registration OK!");
                    this.lblRegUsername.Visible = false;
                    this.lblRegCode.Visible = false;
                    this.txtRegUsername.Visible = false;
                    this.txtRegCode.Visible = false;
                    this.btnReg.Visible = false;
                    this.linkLblBuy.Visible = false;
                    this.linkPaypal.Visible = false;
                    Reg.A1K3 = true;
                    Reg.RegUser = str;
                }
                else
                {
                    MessageBox.Show("Invalid RegCode !");
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

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAbout));
            this.splitAbout = new SplitContainer();
            this.txtHelp = new TextBox();
            this.linkPaypal = new LinkLabel();
            this.linkLabelMail = new LinkLabel();
            this.linkLabelSite2 = new LinkLabel();
            this.linkLblBuy = new LinkLabel();
            this.btnReg = new Button();
            this.txtRegCode = new TextBox();
            this.txtRegUsername = new TextBox();
            this.lblRegCode = new Label();
            this.lblRegUsername = new Label();
            this.linkLblSite = new LinkLabel();
            this.lblRegInfo = new Label();
            this.splitAbout.Panel1.SuspendLayout();
            this.splitAbout.Panel2.SuspendLayout();
            this.splitAbout.SuspendLayout();
            base.SuspendLayout();
            this.splitAbout.Dock = DockStyle.Fill;
            this.splitAbout.Location = new Point(0, 0);
            this.splitAbout.Name = "splitAbout";
            this.splitAbout.Orientation = Orientation.Horizontal;
            this.splitAbout.Panel1.Controls.Add(this.txtHelp);
            this.splitAbout.Panel2.Controls.Add(this.linkPaypal);
            this.splitAbout.Panel2.Controls.Add(this.linkLabelMail);
            this.splitAbout.Panel2.Controls.Add(this.linkLabelSite2);
            this.splitAbout.Panel2.Controls.Add(this.linkLblBuy);
            this.splitAbout.Panel2.Controls.Add(this.btnReg);
            this.splitAbout.Panel2.Controls.Add(this.txtRegCode);
            this.splitAbout.Panel2.Controls.Add(this.txtRegUsername);
            this.splitAbout.Panel2.Controls.Add(this.lblRegCode);
            this.splitAbout.Panel2.Controls.Add(this.lblRegUsername);
            this.splitAbout.Panel2.Controls.Add(this.linkLblSite);
            this.splitAbout.Panel2.Controls.Add(this.lblRegInfo);
            this.splitAbout.Size = new Size(0x27a, 0x192);
            this.splitAbout.SplitterDistance = 280;
            this.splitAbout.TabIndex = 0;
            this.txtHelp.BackColor = SystemColors.Control;
            this.txtHelp.Dock = DockStyle.Fill;
            this.txtHelp.Location = new Point(0, 0);
            this.txtHelp.Margin = new Padding(2);
            this.txtHelp.Multiline = true;
            this.txtHelp.Name = "txtHelp";
            this.txtHelp.ReadOnly = true;
            this.txtHelp.Size = new Size(0x27a, 280);
            this.txtHelp.TabIndex = 9;
            this.txtHelp.Text = resources.GetString("txtHelp.Text");
            this.linkPaypal.AutoSize = true;
            this.linkPaypal.Location = new Point(540, 0x5d);
            this.linkPaypal.Name = "linkPaypal";
            this.linkPaypal.Size = new Size(40, 13);
            this.linkPaypal.TabIndex = 0x15;
            this.linkPaypal.TabStop = true;
            this.linkPaypal.Text = "PayPal";
            this.linkPaypal.LinkClicked += new LinkLabelLinkClickedEventHandler(this.linkPaypal_LinkClicked);
            this.linkLabelMail.AutoSize = true;
            this.linkLabelMail.Location = new Point(3, 0x59);
            this.linkLabelMail.Name = "linkLabelMail";
            this.linkLabelMail.Size = new Size(0x79, 13);
            this.linkLabelMail.TabIndex = 20;
            this.linkLabelMail.TabStop = true;
            this.linkLabelMail.Text = "janusecurity@gmail.com";
            this.linkLabelMail.LinkClicked += new LinkLabelLinkClickedEventHandler(this.linkLabelMail_LinkClicked);
            this.linkLabelSite2.AutoSize = true;
            this.linkLabelSite2.Location = new Point(3, 0x40);
            this.linkLabelSite2.Name = "linkLabelSite2";
            this.linkLabelSite2.Size = new Size(0x7d, 13);
            this.linkLabelSite2.TabIndex = 0x13;
            this.linkLabelSite2.TabStop = true;
            this.linkLabelSite2.Text = "http://www.janusec.com";
            this.linkLabelSite2.LinkClicked += new LinkLabelLinkClickedEventHandler(this.linkLabelSite2_LinkClicked);
            this.linkLblBuy.AutoSize = true;
            this.linkLblBuy.Location = new Point(0x1e4, 0x5d);
            this.linkLblBuy.Name = "linkLblBuy";
            this.linkLblBuy.Size = new Size(0x34, 13);
            this.linkLblBuy.TabIndex = 0x12;
            this.linkLblBuy.TabStop = true;
            this.linkLblBuy.Text = "RegNow!";
            this.linkLblBuy.LinkClicked += new LinkLabelLinkClickedEventHandler(this.linkLblBuy_LinkClicked);
            this.btnReg.Location = new Point(0x18a, 0x58);
            this.btnReg.Name = "btnReg";
            this.btnReg.Size = new Size(0x4b, 0x19);
            this.btnReg.TabIndex = 0x11;
            this.btnReg.Text = "Register";
            this.btnReg.UseVisualStyleBackColor = true;
            this.btnReg.Click += new EventHandler(this.btnReg_Click);
            this.txtRegCode.Location = new Point(0x18a, 0x3d);
            this.txtRegCode.Name = "txtRegCode";
            this.txtRegCode.Size = new Size(0xc1, 20);
            this.txtRegCode.TabIndex = 0x10;
            this.txtRegUsername.Location = new Point(0x18a, 0x21);
            this.txtRegUsername.Name = "txtRegUsername";
            this.txtRegUsername.Size = new Size(0xc1, 20);
            this.txtRegUsername.TabIndex = 15;
            this.lblRegCode.AutoSize = true;
            this.lblRegCode.Location = new Point(0x141, 0x40);
            this.lblRegCode.Name = "lblRegCode";
            this.lblRegCode.Size = new Size(0x37, 13);
            this.lblRegCode.TabIndex = 14;
            this.lblRegCode.Text = "RegCode:";
            this.lblRegUsername.AutoSize = true;
            this.lblRegUsername.Location = new Point(0x141, 0x24);
            this.lblRegUsername.Name = "lblRegUsername";
            this.lblRegUsername.Size = new Size(0x3a, 13);
            this.lblRegUsername.TabIndex = 13;
            this.lblRegUsername.Text = "Username:";
            this.linkLblSite.AutoSize = true;
            this.linkLblSite.Location = new Point(3, 0x29);
            this.linkLblSite.Name = "linkLblSite";
            this.linkLblSite.Size = new Size(0x66, 13);
            this.linkLblSite.TabIndex = 11;
            this.linkLblSite.TabStop = true;
            this.linkLblSite.Text = "http://sec4app.com";
            this.linkLblSite.LinkClicked += new LinkLabelLinkClickedEventHandler(this.linkLblSite_LinkClicked);
            this.lblRegInfo.AutoSize = true;
            this.lblRegInfo.Location = new Point(3, 11);
            this.lblRegInfo.Name = "lblRegInfo";
            this.lblRegInfo.Size = new Size(0x175, 13);
            this.lblRegInfo.TabIndex = 1;
            this.lblRegInfo.Text = "This Copy of WebCruiser is UnRegistered! You can use it for another 30 days.";
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.ClientSize = new Size(0x27a, 0x192);
            base.Controls.Add(this.splitAbout);
            base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            base.Name = "FormAbout";
            this.Text = "FormAbout";
            this.splitAbout.Panel1.ResumeLayout(false);
            this.splitAbout.Panel1.PerformLayout();
            this.splitAbout.Panel2.ResumeLayout(false);
            this.splitAbout.Panel2.PerformLayout();
            this.splitAbout.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        public void InitRegControl()
        {
            if (Reg.A1K3)
            {
                this.lblRegInfo.Text = "This copy of WebCruiser is licensed to: " + Reg.RegUser;
                this.lblRegUsername.Visible = false;
                this.lblRegCode.Visible = false;
                this.txtRegUsername.Visible = false;
                this.txtRegCode.Visible = false;
                this.btnReg.Visible = false;
                this.linkLblBuy.Visible = false;
                this.linkPaypal.Visible = false;
            }
            else if (Reg.LeftDays > 0)
            {
                this.lblRegInfo.Text = "This Copy of WebCruiser is UnRegistered! You can use it for " + Reg.LeftDays.ToString() + " days.";
            }
            else
            {
                this.lblRegInfo.Text = "This Copy of WebCruiser is UnRegistered and Expired! Please Register If You Would Like To Continue Using It!";
                MessageBox.Show("This Copy of WebCruiser is UnRegistered and Expired! Please Register If You Would Like To Continue Using It!", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            this.mainfrm.InitFunctionByRegistration(Reg.A1K3, Reg.LeftDays);
        }

        private void linkLabelMail_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string fileName = "mailto:janusecurity@gmail.com?subject=WebCruiserWVS";
            try
            {
                Process.Start(fileName);
            }
            catch
            {
            }
        }

        private void linkLabelSite2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string fileName = "http://www.janusec.com/";
            try
            {
                Process.Start(fileName);
            }
            catch
            {
                this.mainfrm.NavigatePage(fileName, RequestType.GET, "");
            }
        }

        private void linkLblBuy_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.linkLblBuy.LinkVisited = true;
            string fileName = "";
            fileName = "https://www.regnow.com/softsell/nph-softsell.cgi?item=25854-2";
            try
            {
                Process.Start(fileName);
            }
            catch
            {
                Clipboard.SetText(fileName);
                MessageBox.Show("* Default browser not found, please open the following URL for order information.\r\n* " + fileName + "\r\n* This URL has been set to ClipBoard and you can paste it to you browser directly.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }

        private void linkLblSite_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string fileName = "http://sec4app.com/";
            try
            {
                Process.Start(fileName);
            }
            catch
            {
                this.mainfrm.NavigatePage(fileName, RequestType.GET, "");
            }
        }

        private void linkPaypal_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.linkPaypal.LinkVisited = true;
            string fileName = "";
            fileName = "http://www.janusec.com/product/2/webcruiser+web+vulnerability+scanner+enterprise+edition";
            try
            {
                Process.Start(fileName);
            }
            catch
            {
                Clipboard.SetText(fileName);
                MessageBox.Show("* Default browser not found, please open the following URL for order information.\r\n* " + fileName + "\r\n* This URL has been set to ClipBoard and you can paste it to you browser directly.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }
    }
}

