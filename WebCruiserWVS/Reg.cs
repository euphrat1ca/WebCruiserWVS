namespace WebCruiserWVS
{
    using System;
    using System.Globalization;
    using System.Security.Cryptography;
    using System.Text;
    using System.Windows.Forms;

    internal class Reg
    {
        private static bool _RegOK = false;
        public static int LeftDays = 30;
        public static string RegUser = "";

        public static string Decrypt(string toDecrypt)
        {
            byte[] bytes = Encoding.UTF8.GetBytes("WebCruiser1.00ByHttp:Sec4app.com");
            byte[] inputBuffer = Convert.FromBase64String(toDecrypt);
            RijndaelManaged managed = new RijndaelManaged {
                Key = bytes,
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };
            byte[] buffer3 = managed.CreateDecryptor().TransformFinalBlock(inputBuffer, 0, inputBuffer.Length);
            return Encoding.UTF8.GetString(buffer3);
        }

        public static string Encrypt(string toEncrypt)
        {
            byte[] bytes = Encoding.UTF8.GetBytes("WebCruiser1.00ByHttp:Sec4app.com");
            byte[] inputBuffer = Encoding.UTF8.GetBytes(toEncrypt);
            RijndaelManaged managed = new RijndaelManaged {
                Key = bytes,
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };
            byte[] inArray = managed.CreateEncryptor().TransformFinalBlock(inputBuffer, 0, inputBuffer.Length);
            return Convert.ToBase64String(inArray, 0, inArray.Length);
        }

        private static string GetHash(string Source)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(Source);
            SHA512 sha = new SHA512Managed();
            char[] chArray = BitConverter.ToString(sha.ComputeHash(bytes)).Replace("-", "").ToCharArray();
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < 0x80; i++)
            {
                if ((i % 8) == 0)
                {
                    builder.Append(chArray[i].ToString());
                }
            }
            return builder.ToString();
        }

        private static string GetMD5Hash(string Source)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(Source);
            MD5 md = new MD5CryptoServiceProvider();
            return BitConverter.ToString(md.ComputeHash(bytes)).Replace("-", "").Substring(8, 0x10);
        }

        private static ulong Hash2UInt64(string Str)
        {
            try
            {
                return ulong.Parse(Str, NumberStyles.HexNumber);
            }
            catch
            {
                return 0L;
            }
        }

        private static uint String2UInt32(string Str)
        {
            try
            {
                return (uint) (ulong.Parse(GetMD5Hash(Str), NumberStyles.HexNumber) % ((ulong) 0xf4240L));
            }
            catch
            {
                return 0;
            }
        }

        public static bool ValidateRegCode(string Username, string RegCode)
        {
            try
            {
                if (RegCode.Length == 0x13)
                {
                    char[] chArray = RegCode.ToCharArray();
                    if (chArray[4] != '-')
                    {
                        return false;
                    }
                    if (chArray[9] != '-')
                    {
                        return false;
                    }
                    if (chArray[14] != '-')
                    {
                        return false;
                    }
                    RegCode = RegCode.Replace("-", "");
                    ulong num = Hash2UInt64(RegCode);
                    ulong num2 = Hash2UInt64(GetHash(Username));
                    ulong num3 = num - num2;
                    if (GetHash(num3.ToString()).Equals("1FEDF23C6CB786AA"))
                    {
                        _RegOK = true;
                        RegUser = Username;
                        return true;
                    }
                }
                return false;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
                return false;
            }
        }

        public static bool ValidateRegCode2(string Username, string RegCode)
        {
            try
            {
                if (RegCode.IndexOf('-') > 0)
                {
                    string[] strArray = RegCode.Split(new char[] { '-' });
                    if (strArray.Length != 2)
                    {
                        return false;
                    }
                    string str = strArray[0];
                    string s = strArray[1];
                    uint num = String2UInt32(str);
                    uint num3 = uint.Parse(s) - num;
                    if (GetMD5Hash(num3.ToString()).Equals("B1B77A53F0264B1D"))
                    {
                        _RegOK = true;
                        RegUser = Username;
                        return true;
                    }
                }
                return false;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
                return false;
            }
        }

        public static bool A1K3
        {
            get
            {
                return _RegOK;
            }
            set
            {
                _RegOK = value;
            }
        }
    }
}

