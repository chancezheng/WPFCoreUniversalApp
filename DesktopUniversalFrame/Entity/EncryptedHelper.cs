using DesktopUniversalFrame.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace DesktopUniversalFrame.Entity
{
    public class EncryptedHelper
    {
        private static ConfigrationOperation ConfigrationOperation;
        public EncryptedHelper()
        {
            ConfigrationOperation = new ConfigrationOperation(); ;
        }

        /// <summary>
        /// MD5加密(不可逆)
        /// </summary>
        /// <param name="original">初始值</param>
        /// <param name="bit">默认32位</param>
        /// <returns></returns>
        public static string EncryptedInMD5(string original, int bit = 32)
        {
            byte[] input = Encoding.Default.GetBytes(original.Trim());
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] output = md5.ComputeHash(input);
            StringBuilder encryptedStr = new StringBuilder(BitConverter.ToString(output).Replace("-", ""));
            if (bit == 16)
                return encryptedStr.ToString().Substring(8, 16);
            else if (bit == 32) //默认
                return encryptedStr.ToString();
            else if (bit == 64)
                return Convert.ToBase64String(output);
            else
                return string.Empty;
        }

        /// <summary>
        /// DES加密
        /// </summary>
        /// <param name="original">初始值</param>
        /// <param name="key">密钥</param>
        /// <returns></returns>
        public static string EncryptedInDES(string original, string key)
        {
            if (string.IsNullOrEmpty(original)) return string.Empty;

            var des = new DESCryptoServiceProvider();
            byte[] inputArray = Encoding.Default.GetBytes(original);
            des.Key = Encoding.ASCII.GetBytes(ConfigrationOperation.GetUserConfiguration("DesKey").Substring(0,8));
            des.IV = Encoding.ASCII.GetBytes(ConfigrationOperation.GetUserConfiguration("DesIV").Substring(0, 8));

            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(inputArray, 0, inputArray.Length);
            cs.FlushFinalBlock();
            StringBuilder encrytedStr = new StringBuilder(BitConverter.ToString(ms.ToArray()));
            return encrytedStr.ToString();
        }

        /// <summary>
        /// DES解密
        /// </summary>
        /// <param name="enceyptedStr">密码</param>
        /// <param name="key">密钥</param>
        /// <returns></returns>
        public static string DecryptedInDES(string enceyptedStr, string key)
        {
            if (string.IsNullOrEmpty(enceyptedStr)) return string.Empty;

            var des = new DESCryptoServiceProvider();
            int len = enceyptedStr.Length / 2;
            var inputArray = new byte[len];
            int j = 0;
            for (int i = 0; i < len; i++)
            {
                j = Convert.ToInt32(enceyptedStr.Substring(j * 2, 2), 16);
                inputArray[i] = (byte)j;
            }

            //配置文件获取Key
            des.Key = Encoding.ASCII.GetBytes(ConfigrationOperation.GetUserConfiguration("DesKey").Substring(0, 8));
            //配置文件获取IV
            des.IV = Encoding.ASCII.GetBytes(ConfigrationOperation.GetUserConfiguration("DesIV").Substring(0, 8));

            //内存流
            MemoryStream ms = new MemoryStream();
            //加密流
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(inputArray, 0, inputArray.Length);
            cs.FlushFinalBlock();
          
            return Encoding.Default.GetString(ms.ToArray());
        }
    }
}
