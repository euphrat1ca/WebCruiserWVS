namespace WebCruiserWVS
{
    using System;
    using System.CodeDom.Compiler;
    using System.Configuration;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    [GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "9.0.0.0"), CompilerGenerated]
    internal sealed class WebCruiserWVS : ApplicationSettingsBase
    {
        private static WebCruiserWVS defaultInstance = ((WebCruiserWVS) SettingsBase.Synchronized(new WebCruiserWVS()));

        [UserScopedSetting, DefaultSettingValue("id:uid:user:username:password:access:account:accounts:admin_id:admin_name:admin_pass:admin_passwd:admin_password:admin_psw:admin_pwd:admin_user:admin_userid:admin_username:adminemail:adminid:administrator_name:adminlogin:adminmail:adminname:adminpass:adminpassword:adminpsw:adminpwd:AdminUID:adminuser:adminuserid:adminusername:address:ArticleID:cid:city:client:clientname:clientpassword:clients:clientusername:content:customer:customers:customers_password:data:db_hostname:db_password:db_username:dw:E-mail:e_mail:email:emailaddress:group:hash:index:isadmin:key:keywords:last_login:lastname:log:login:login_admin:login_name:login_pass:login_passwd:login_password:login_pw:login_pwd:login_user:login_username:mail:md5hash:member_id:member_name:memberid:membername:mempassword:name:newsid:number:pass:pass1word:pass_hash:pass_word:passwd:phone:POWER:pwd:pwd1:pword:sid:telephone:temp_pass:temp_password:temppass:temppasword:text:uname:user1:user_admin:user_email:user_id:user_ip:user_level:user_login:user_name:user_pass:user_passw:user_passwd:user_password:user_pw:user_pwd:user_pword:user_pwrd:user_uname:user_username:user_usernm:user_usernun:user_usrnm:useradmin:userid:userip:Userlogin:usernm:userpass:userpasswd:userPassword:userpw:userpwd:usr_name:usr_nusr:usr_pass:usr_pw:usrname:usrpass:usrs"), DebuggerNonUserCode]
        public string AccessColumns
        {
            get
            {
                return (string) this["AccessColumns"];
            }
            set
            {
                this["AccessColumns"] = value;
            }
        }

        [DefaultSettingValue("admin:admins:admin_login:adm:Dv_admin:Superuser:sys:sysadmin:sysadmins:System:sysuser:sysusers:account:accounts:admin_name:admin_user:admin_userinfo:administrator:administrators:adminuser:BBS:cms_admin:cms_admins:cms_user:cms_users:company:config:Contact:content:customer:customers:dbadmins:group:guanli:guanliyuan:info:key:keywords:login:logon:logs:m_admin:main:manage:manager:member:memberlist:members:name:names:reg_user:reg_users:reguser:regusers:root:roots:session:setting:settings:site_login:site_logins:sitelogin:sitelogins:Subjects:tb_admin:tb_administrator:tb_login:tb_member:tb_members:tb_user:tbl_user:tbl_users:tbladmins:tblUser:test:user:user_admin:user_login:user_name:userinfo:users:usr:vip:WebAdmin:webadmins:Webmaster:webmasters:webuser:webusers"), UserScopedSetting, DebuggerNonUserCode]
        public string AccessTables
        {
            get
            {
                return (string) this["AccessTables"];
            }
            set
            {
                this["AccessTables"] = value;
            }
        }

        [DebuggerNonUserCode, UserScopedSetting, DefaultSettingValue("index:admin_login:admin_main:ad_login:ad_manage:adm_login:admin123:admin1:admin888:admin_admin:admin_edit:admin_index:admin_user:admindefault:adminindex:administrator:adminlogin:adminmanage:adminmember:adminuserlogin:adminuser:admin:ad:chkadmin:chklogin:config:conn:default:denglu:guanli:houtaiguanli:houtai:htgl:index_admin:index_manage:index:login_admin:login:main:manage_index:manager:manage")]
        public string AdminPage
        {
            get
            {
                return (string) this["AdminPage"];
            }
            set
            {
                this["AdminPage"] = value;
            }
        }

        [DefaultSettingValue("admin/:admin_login/:admin/login/:adm/:adm_login/:admin/manage/:aadmin/:ad/:ad_login/:ad_manage/:forum/admin/:admin/default/:admin/index/:admin1/:admin123/:admin888/:admin_admin/:admin_index/:admin_main/:admin_user/:adminadmin/:adminindex/:administrator/:adminlogin/:adminmember/:adminuser/:adminuserlogin/:bbs/admin/login/:chkadmin/:chklogin/:config/:database/:databases/:db/:denglu/:devel/:guanli/:houtai/:houtaiguanli/:htgl/:idea/:ideas/:index_admin/:index_manage/:indexadmin/:login/login/:login/super/:login1/:login_admin/:main/login/:manage/:manage_index/:manager/:manager/login/:private/:root/:secret/:secrets/:setting/:setup/:super/:superadmin/:sys_admin/:webadmin/:webmaster/"), UserScopedSetting, DebuggerNonUserCode]
        public string AdminPath
        {
            get
            {
                return (string) this["AdminPath"];
            }
            set
            {
                this["AdminPath"] = value;
            }
        }

        [DefaultSettingValue("False"), UserScopedSetting, DebuggerNonUserCode]
        public bool chkReplace1
        {
            get
            {
                return (bool) this["chkReplace1"];
            }
            set
            {
                this["chkReplace1"] = value;
            }
        }

        [DebuggerNonUserCode, DefaultSettingValue("False"), UserScopedSetting]
        public bool chkReplace2
        {
            get
            {
                return (bool) this["chkReplace2"];
            }
            set
            {
                this["chkReplace2"] = value;
            }
        }

        [DebuggerNonUserCode, DefaultSettingValue("False"), UserScopedSetting]
        public bool chkReplace3
        {
            get
            {
                return (bool) this["chkReplace3"];
            }
            set
            {
                this["chkReplace3"] = value;
            }
        }

        [DefaultSettingValue("htm:html:shtml:asp:aspx:jsp:php:do:cfm:cgi:pl:txt:action:js"), DebuggerNonUserCode, UserScopedSetting]
        public string CrawlableExt
        {
            get
            {
                return (string) this["CrawlableExt"];
            }
            set
            {
                this["CrawlableExt"] = value;
            }
        }

        [DefaultSettingValue("http://sec4app.com/test/info.txt"), UserScopedSetting, DebuggerNonUserCode]
        public string CrossSiteRecord
        {
            get
            {
                return (string) this["CrossSiteRecord"];
            }
            set
            {
                this["CrossSiteRecord"] = value;
            }
        }

        [DefaultSettingValue("http://sec4app.com/test/info.php?id="), UserScopedSetting, DebuggerNonUserCode]
        public string CrossSiteURL
        {
            get
            {
                return (string) this["CrossSiteURL"];
            }
            set
            {
                this["CrossSiteURL"] = value;
            }
        }

        public static WebCruiserWVS Default
        {
            get
            {
                return defaultInstance;
            }
        }

        [DefaultSettingValue("Standard"), UserScopedSetting, DebuggerNonUserCode]
        public string Edition
        {
            get
            {
                return (string) this["Edition"];
            }
            set
            {
                this["Edition"] = value;
            }
        }

        [DefaultSettingValue("%20"), DebuggerNonUserCode, UserScopedSetting]
        public string FiltExpr1
        {
            get
            {
                return (string) this["FiltExpr1"];
            }
            set
            {
                this["FiltExpr1"] = value;
            }
        }

        [DebuggerNonUserCode, DefaultSettingValue("%27"), UserScopedSetting]
        public string FiltExpr2
        {
            get
            {
                return (string) this["FiltExpr2"];
            }
            set
            {
                this["FiltExpr2"] = value;
            }
        }

        [UserScopedSetting, DebuggerNonUserCode, DefaultSettingValue("select")]
        public string FiltExpr3
        {
            get
            {
                return (string) this["FiltExpr3"];
            }
            set
            {
                this["FiltExpr3"] = value;
            }
        }

        [UserScopedSetting, DefaultSettingValue("1900-01-01"), DebuggerNonUserCode]
        public DateTime InitDate
        {
            get
            {
                return (DateTime) this["InitDate"];
            }
            set
            {
                this["InitDate"] = value;
            }
        }

        [DefaultSettingValue("5"), UserScopedSetting, DebuggerNonUserCode]
        public int MaxHTTPThread
        {
            get
            {
                return (int) this["MaxHTTPThread"];
            }
            set
            {
                this["MaxHTTPThread"] = value;
            }
        }

        [UserScopedSetting, DebuggerNonUserCode, DefaultSettingValue("3")]
        public int MultiSitesNum
        {
            get
            {
                return (int) this["MultiSitesNum"];
            }
            set
            {
                this["MultiSitesNum"] = value;
            }
        }

        [UserScopedSetting, DefaultSettingValue(""), DebuggerNonUserCode]
        public string ProxyAddress
        {
            get
            {
                return (string) this["ProxyAddress"];
            }
            set
            {
                this["ProxyAddress"] = value;
            }
        }

        [UserScopedSetting, DefaultSettingValue(""), DebuggerNonUserCode]
        public string ProxyPassword
        {
            get
            {
                return (string) this["ProxyPassword"];
            }
            set
            {
                this["ProxyPassword"] = value;
            }
        }

        [DefaultSettingValue("8080"), UserScopedSetting, DebuggerNonUserCode]
        public int ProxyPort
        {
            get
            {
                return (int) this["ProxyPort"];
            }
            set
            {
                this["ProxyPort"] = value;
            }
        }

        [DefaultSettingValue(""), UserScopedSetting, DebuggerNonUserCode]
        public string ProxyUsername
        {
            get
            {
                return (string) this["ProxyUsername"];
            }
            set
            {
                this["ProxyUsername"] = value;
            }
        }

        [DefaultSettingValue("UnRegistered"), UserScopedSetting, DebuggerNonUserCode]
        public string RegisterInfo
        {
            get
            {
                return (string) this["RegisterInfo"];
            }
            set
            {
                this["RegisterInfo"] = value;
            }
        }

        [UserScopedSetting, DebuggerNonUserCode, DefaultSettingValue("/**/")]
        public string RepExpr1
        {
            get
            {
                return (string) this["RepExpr1"];
            }
            set
            {
                this["RepExpr1"] = value;
            }
        }

        [UserScopedSetting, DebuggerNonUserCode, DefaultSettingValue("%2527")]
        public string RepExpr2
        {
            get
            {
                return (string) this["RepExpr2"];
            }
            set
            {
                this["RepExpr2"] = value;
            }
        }

        [DefaultSettingValue("SeLselecteCt"), DebuggerNonUserCode, UserScopedSetting]
        public string RepExpr3
        {
            get
            {
                return (string) this["RepExpr3"];
            }
            set
            {
                this["RepExpr3"] = value;
            }
        }

        [DebuggerNonUserCode, UserScopedSetting, DefaultSettingValue("True")]
        public bool ScanCookieSQL
        {
            get
            {
                return (bool) this["ScanCookieSQL"];
            }
            set
            {
                this["ScanCookieSQL"] = value;
            }
        }

        [DefaultSettingValue("5"), UserScopedSetting, DebuggerNonUserCode]
        public int ScanDepth
        {
            get
            {
                return (int) this["ScanDepth"];
            }
            set
            {
                this["ScanDepth"] = value;
            }
        }

        [UserScopedSetting, DebuggerNonUserCode, DefaultSettingValue("True")]
        public bool ScanPostSQL
        {
            get
            {
                return (bool) this["ScanPostSQL"];
            }
            set
            {
                this["ScanPostSQL"] = value;
            }
        }

        [UserScopedSetting, DefaultSettingValue("True"), DebuggerNonUserCode]
        public bool ScanSQLInjection
        {
            get
            {
                return (bool) this["ScanSQLInjection"];
            }
            set
            {
                this["ScanSQLInjection"] = value;
            }
        }

        [DebuggerNonUserCode, UserScopedSetting, DefaultSettingValue("True")]
        public bool ScanURLSQL
        {
            get
            {
                return (bool) this["ScanURLSQL"];
            }
            set
            {
                this["ScanURLSQL"] = value;
            }
        }

        [DebuggerNonUserCode, DefaultSettingValue("True"), UserScopedSetting]
        public bool ScanXPathInjection
        {
            get
            {
                return (bool) this["ScanXPathInjection"];
            }
            set
            {
                this["ScanXPathInjection"] = value;
            }
        }

        [DebuggerNonUserCode, UserScopedSetting, DefaultSettingValue("True")]
        public bool ScanXSS
        {
            get
            {
                return (bool) this["ScanXSS"];
            }
            set
            {
                this["ScanXSS"] = value;
            }
        }

        [UserScopedSetting, DebuggerNonUserCode, DefaultSettingValue("0")]
        public int SecondsDelay
        {
            get
            {
                return (int) this["SecondsDelay"];
            }
            set
            {
                this["SecondsDelay"] = value;
            }
        }

        [DefaultSettingValue("False"), DebuggerNonUserCode, UserScopedSetting]
        public bool UseProxy
        {
            get
            {
                return (bool) this["UseProxy"];
            }
            set
            {
                this["UseProxy"] = value;
            }
        }

        [DebuggerNonUserCode, DefaultSettingValue("Mozilla/4.0"), UserScopedSetting]
        public string UserAgent
        {
            get
            {
                return (string) this["UserAgent"];
            }
            set
            {
                this["UserAgent"] = value;
            }
        }
    }
}

