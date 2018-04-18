namespace WebCruiserWVS
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Web.UI;
    using System.Windows.Forms;
    using System.Xml;

    public class FormReport : Form
    {
        private Button btnGenReport;
        private IContainer components;
        private Label label22;
        private FormMain mainfrm;
        private TextBox txtReportAuthor;

        public FormReport(FormMain fm)
        {
            this.InitializeComponent();
            this.mainfrm = fm;
        }

        private void btnGenReport_Click(object sender, EventArgs e)
        {
            this.mainfrm.UpdateXMLData(this.mainfrm.CurrentSite.WcrXml);
            MessageBox.Show("Generate Report " + this.GenReport(this.mainfrm.CurrentSite.WcrXml) + " OK!", "Done", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private string GenReport(XmlDocument WcrXml)
        {
            string path = this.mainfrm.CurrentSite.DomainHost + "_Scan_Report_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".html";
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
                writer2.Write(this.mainfrm.CurrentSite.DomainHost + " Scan Report<br>");
                writer2.RenderEndTag();
                string str2 = this.txtReportAuthor.Text.Trim();
                if (!string.IsNullOrEmpty(str2))
                {
                    writer2.Write("<br><br><br><br><br><br><br><br>Made By " + str2);
                }
                writer2.Write("<br><br><br><br><br><br><br><br>Created By WebCruiser - Web Vulnerability Scanner<br>" + DateTime.Now.ToString("yyyy-MM-dd"));
                writer2.Write("<div style=\"page-break-after:always\">&nbsp;</div>");
                XmlNodeList list = WcrXml.SelectNodes("//ROOT/SiteSQLEnv/EnvRow");
                if (list.Count > 0)
                {
                    writer2.RenderBeginTag(HtmlTextWriterTag.H2);
                    writer2.Write("Basic Information");
                    writer2.RenderEndTag();
                    writer2.AddAttribute("border", "1");
                    writer2.AddAttribute("width", "640");
                    writer2.AddAttribute("cellspacing", "0");
                    writer2.AddAttribute("bordercolordark", "009099");
                    writer2.RenderBeginTag(HtmlTextWriterTag.Table);
                    string str3 = "";
                    foreach (XmlNode node in list)
                    {
                        string innerText = node.ChildNodes[1].InnerText;
                        if (string.IsNullOrEmpty(innerText))
                        {
                            innerText = "&nbsp;";
                        }
                        string str8 = str3;
                        str3 = str8 + "<tr><td width=\"130\">" + node.ChildNodes[0].InnerText + "</td><td> " + innerText + " </td></tr>";
                    }
                    writer2.Write(str3);
                    writer2.RenderEndTag();
                    writer2.Write("<br>");
                }
                XmlNodeList list2 = WcrXml.SelectNodes("//ROOT/SiteVulList/VulRow");
                if (list2.Count > 0)
                {
                    writer2.RenderBeginTag(HtmlTextWriterTag.H2);
                    writer2.Write("Vulnerability Result");
                    writer2.RenderEndTag();
                    for (int i = 0; i < list2.Count; i++)
                    {
                        writer2.AddAttribute("border", "1");
                        writer2.AddAttribute("width", "640");
                        writer2.AddAttribute("cellspacing", "0");
                        writer2.AddAttribute("bordercolordark", "009099");
                        writer2.RenderBeginTag(HtmlTextWriterTag.Table);
                        writer2.Write("<tr><td width=\"150\">No.</td><td>" + ((i + 1)).ToString() + "</td></tr>");
                        XmlNode node2 = list2[i];
                        for (int j = 0; j < node2.ChildNodes.Count; j++)
                        {
                            writer2.Write("<tr><td width=\"150\">");
                            writer2.Write(node2.ChildNodes[j].Name);
                            writer2.Write("</td><td>");
                            writer2.Write(node2.ChildNodes[j].InnerText.Replace("<>", "&lt;&gt;"));
                            writer2.Write("</td></tr>");
                        }
                        writer2.RenderEndTag();
                        writer2.Write("<br>");
                    }
                }
                if ((this.mainfrm.CurrentSite.InjType != InjectionType.UnKnown) && (this.mainfrm.CurrentSite.BlindInjType != BlindType.UnKnown))
                {
                    writer2.RenderBeginTag(HtmlTextWriterTag.H2);
                    writer2.Write("Proof Of Concept - SQL INJECTION");
                    writer2.RenderEndTag();
                    writer2.AddAttribute("border", "1");
                    writer2.AddAttribute("width", "640");
                    writer2.AddAttribute("cellspacing", "0");
                    writer2.AddAttribute("bordercolordark", "009099");
                    writer2.RenderBeginTag(HtmlTextWriterTag.Table);
                    writer2.Write("<tr><td width=\"140\">Parameter</td><td>Value</td></tr>");
                    writer2.Write("<tr><td>URL</td><td>" + this.mainfrm.URL + "</td></tr>");
                    writer2.Write("<tr><td>RequestType</td><td>" + this.mainfrm.ReqType.ToString() + "</td></tr>");
                    writer2.Write("<tr><td>DatabaseType</td><td>" + this.mainfrm.CurrentSite.DatabaseType.ToString() + "</td></tr>");
                    writer2.Write("<tr><td>InjectionType</td><td>" + this.mainfrm.CurrentSite.InjType.ToString() + "</td></tr>");
                    writer2.Write("<tr><td>GettingDataBy</td><td>" + this.mainfrm.CurrentSite.BlindInjType.ToString() + "</td></tr>");
                    writer2.RenderEndTag();
                    writer2.Write("<br>");
                }
                XmlNodeList list3 = WcrXml.SelectNodes("//ROOT/SiteDBStructure/Database");
                if (list3.Count > 0)
                {
                    string str5 = "";
                    foreach (XmlNode node3 in list3)
                    {
                        str5 = str5 + node3.Attributes["Text"].Value + "<br>";
                        foreach (XmlNode node4 in node3.ChildNodes)
                        {
                            str5 = str5 + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + node4.Attributes["Text"].Value + "<br>";
                            foreach (XmlNode node5 in node4.ChildNodes)
                            {
                                str5 = str5 + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + node5.Attributes["Text"].Value + "<br>";
                            }
                        }
                    }
                    writer2.RenderBeginTag(HtmlTextWriterTag.H2);
                    writer2.Write("Proof Of Concept - Getting Database Structure ");
                    writer2.RenderEndTag();
                    string str6 = "DB-----Table---Column\r\n";
                    str6 = (str6 + str5).Replace("\r\n", "<br>").Replace(" ", "&nbsp;");
                    writer2.AddAttribute("border", "1");
                    writer2.AddAttribute("width", "640");
                    writer2.AddAttribute("cellspacing", "0");
                    writer2.AddAttribute("bordercolordark", "009099");
                    writer2.RenderBeginTag(HtmlTextWriterTag.Table);
                    writer2.Write("<tr><td>");
                    writer2.Write(str6);
                    writer2.Write("</td></tr>");
                    writer2.RenderEndTag();
                    writer2.Write("<br>");
                }
                writer2.RenderEndTag();
                writer2.RenderEndTag();
                writer2.RenderEndTag();
                File.WriteAllText(path, writer.ToString());
                return path;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
                File.WriteAllText(path, writer.ToString());
                return path;
            }
        }

        private void InitializeComponent()
        {
            this.txtReportAuthor = new TextBox();
            this.label22 = new Label();
            this.btnGenReport = new Button();
            base.SuspendLayout();
            this.txtReportAuthor.Location = new Point(80, 0x36);
            this.txtReportAuthor.Name = "txtReportAuthor";
            this.txtReportAuthor.Size = new Size(210, 0x15);
            this.txtReportAuthor.TabIndex = 5;
            this.txtReportAuthor.Text = "WebCruiser";
            this.label22.AutoSize = true;
            this.label22.Location = new Point(20, 0x39);
            this.label22.Name = "label22";
            this.label22.Size = new Size(0x2f, 12);
            this.label22.TabIndex = 4;
            this.label22.Text = "Made By";
            this.btnGenReport.Location = new Point(0x13d, 0x34);
            this.btnGenReport.Name = "btnGenReport";
            this.btnGenReport.Size = new Size(0x7f, 0x17);
            this.btnGenReport.TabIndex = 3;
            this.btnGenReport.Text = "Generate Report";
            this.btnGenReport.UseVisualStyleBackColor = true;
            this.btnGenReport.Click += new EventHandler(this.btnGenReport_Click);
            base.AutoScaleDimensions = new SizeF(6f, 12f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = SystemColors.Window;
            base.ClientSize = new Size(0x1e6, 0x13d);
            base.Controls.Add(this.txtReportAuthor);
            base.Controls.Add(this.label22);
            base.Controls.Add(this.btnGenReport);
            base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            base.Name = "FormReport";
            this.Text = "FormReport";
            base.ResumeLayout(false);
            base.PerformLayout();
        }
    }
}

