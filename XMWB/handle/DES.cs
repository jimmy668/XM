using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace XMWB.handle
{
    public class DES
    {
        //DES加密
        public static string Encode(string str)
        {
            return DESEncode(str);
        }

        //DES解密
        public static string Decode(string str)
        {
            return DESDecode(str);
        }

        public static string GetLoginString(string userid)
        {
            string str = "{userId:'" + userid + "',loginTime:'" + DateTime.Now + "'}";

            return DESEncode(str);
        }

        //DES加密
        private static string DESEncode(string encodeStr)
        {
            string key = "nimeiaaa";

            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] inputByteArray = Encoding.Default.GetBytes(encodeStr);
            des.Key = ASCIIEncoding.ASCII.GetBytes(key);
            des.IV = ASCIIEncoding.ASCII.GetBytes(key);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            StringBuilder ret = new StringBuilder();
            foreach (byte b in ms.ToArray())
            {
                ret.AppendFormat("{0:X2}", b);
            }

            return ret.ToString();
        }

        //DES解密
        private static string DESDecode(string decodeStr)
        {
            string key = "nimeiaaa";

            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] inputByteArray = new byte[decodeStr.Length / 2];
            for (int x = 0; x < decodeStr.Length / 2; x++)
            {
                int i = (Convert.ToInt32(decodeStr.Substring(x * 2, 2), 16));
                inputByteArray[x] = (byte)i;
            }
            des.Key = ASCIIEncoding.ASCII.GetBytes(key);
            des.IV = ASCIIEncoding.ASCII.GetBytes(key);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            StringBuilder ret = new StringBuilder();

            return System.Text.Encoding.Default.GetString(ms.ToArray());
        }
    }
}