namespace WebCruiserWVS
{
    using System;
    using System.Runtime.InteropServices;

    public class WCRSetting
    {
        public static bool chkReplace1;
        public static bool chkReplace2;
        public static bool chkReplace3;
        public static string CrawlableExt;
        public static string CrossSiteRecord;
        public static string CrossSiteURL;
        public static string Edition;
        public static string FiltExpr1;
        public static string FiltExpr2;
        public static string FiltExpr3;
        public static int MaxHTTPThreadNum;
        public static int MultiSitesNum;
        public static string ProxyAddress;
        public static string ProxyPassword;
        public static int ProxyPort;
        public static string ProxyUsername;
        public static bool RefreshURL = true;
        public static string RepExpr1;
        public static string RepExpr2;
        public static string RepExpr3;
        public static bool ScanCookieSQL;
        public static int ScanDepth;
        public static bool ScanPostSQL;
        public static bool ScanSQLInjection;
        public static bool ScanURLSQL;
        public static bool ScanXPathInjection;
        public static bool ScanXSS;
        public static int SecondsDelay;
        public static bool UseProxy;
        public static string UserAgent;

        [DllImport("wininet.dll", SetLastError=true)]
        private static extern bool InternetSetOption(IntPtr hInternet, int dwOption, IntPtr lpBuffer, int lpdwBufferLength);
        public static void RefreshIESettings(string strProxy)
        {
            Struct_INTERNET_PROXY_INFO t_internet_proxy_info;
            if (string.IsNullOrEmpty(strProxy))
            {
                t_internet_proxy_info.dwAccessType = 1;
            }
            else
            {
                t_internet_proxy_info.dwAccessType = 3;
            }
            t_internet_proxy_info.proxy = Marshal.StringToHGlobalAnsi(strProxy);
            t_internet_proxy_info.proxyBypass = Marshal.StringToHGlobalAnsi("local");
            IntPtr ptr = Marshal.AllocCoTaskMem(Marshal.SizeOf(t_internet_proxy_info));
            Marshal.StructureToPtr(t_internet_proxy_info, ptr, true);
            InternetSetOption(IntPtr.Zero, 0x26, ptr, Marshal.SizeOf(t_internet_proxy_info));
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Struct_INTERNET_PROXY_INFO
        {
            public int dwAccessType;
            public IntPtr proxy;
            public IntPtr proxyBypass;
        }
    }
}

