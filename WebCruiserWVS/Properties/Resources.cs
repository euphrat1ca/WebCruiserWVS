namespace WebCruiserWVS.Properties
{
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.Globalization;
    using System.Resources;
    using System.Runtime.CompilerServices;

    [DebuggerNonUserCode, CompilerGenerated, GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "2.0.0.0")]
    internal class Resources
    {
        private static CultureInfo resourceCulture;
        private static System.Resources.ResourceManager resourceMan;

        internal Resources()
        {
        }

        internal static Bitmap cookie
        {
            get
            {
                return (Bitmap) ResourceManager.GetObject("cookie", resourceCulture);
            }
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        internal static CultureInfo Culture
        {
            get
            {
                return resourceCulture;
            }
            set
            {
                resourceCulture = value;
            }
        }

        internal static Bitmap db
        {
            get
            {
                return (Bitmap) ResourceManager.GetObject("db", resourceCulture);
            }
        }

        internal static Bitmap file
        {
            get
            {
                return (Bitmap) ResourceManager.GetObject("file", resourceCulture);
            }
        }

        internal static Bitmap fill
        {
            get
            {
                return (Bitmap) ResourceManager.GetObject("fill", resourceCulture);
            }
        }

        internal static Bitmap ie
        {
            get
            {
                return (Bitmap) ResourceManager.GetObject("ie", resourceCulture);
            }
        }

        internal static Bitmap report
        {
            get
            {
                return (Bitmap) ResourceManager.GetObject("report", resourceCulture);
            }
        }

        internal static Bitmap resend
        {
            get
            {
                return (Bitmap) ResourceManager.GetObject("resend", resourceCulture);
            }
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        internal static System.Resources.ResourceManager ResourceManager
        {
            get
            {
                if (object.ReferenceEquals(resourceMan, null))
                {
                    System.Resources.ResourceManager manager = new System.Resources.ResourceManager("WebCruiserWVS.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = manager;
                }
                return resourceMan;
            }
        }

        internal static Bitmap scan
        {
            get
            {
                return (Bitmap) ResourceManager.GetObject("scan", resourceCulture);
            }
        }

        internal static Bitmap tool
        {
            get
            {
                return (Bitmap) ResourceManager.GetObject("tool", resourceCulture);
            }
        }

        internal static Bitmap xss
        {
            get
            {
                return (Bitmap) ResourceManager.GetObject("xss", resourceCulture);
            }
        }
    }
}

