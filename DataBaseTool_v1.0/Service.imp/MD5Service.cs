using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace DataBaseTool.Service.imp
{
    public class MD5Service
    {
        #region MD5加解密


        /// <summary>
        /// MD5管理
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="pwd">字符串</param>
        /// <param name="type">类型:0加密;1:解密</param>
        /// <returns>item1:密码;item2:mdkey</returns>
        public Tuple<string, string> MD5Manager(string key, string pwd, int type)
        {
            string reVal = string.Empty;
            string mdkey = string.Empty;
            MD5Service md = new MD5Service();
            switch (type)
            {
                case 0://加密
                    mdkey = md.GenerateKey();
                    reVal = md.MD5Encrypt(pwd, mdkey);
                    break;
                case 1://解密
                    reVal = md.MD5Decrypt(pwd, key);
                    break;
                default:
                    reVal = "";
                    break;
            }
            return new Tuple<string, string>(reVal, mdkey);
        }

        // 创建Key  
        public string GenerateKey()
        {
            DESCryptoServiceProvider desCrypto = (DESCryptoServiceProvider)DESCryptoServiceProvider.Create();
            return ASCIIEncoding.ASCII.GetString(desCrypto.Key);
        }
        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="pToEncrypt"></param>
        /// <param name="sKey">key</param>
        public string MD5Encrypt(string pToEncrypt, string sKey)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] inputByteArray = Encoding.Default.GetBytes(pToEncrypt);
            try
            {
                des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
                des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
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
            catch (Exception ex)
            {
                Logout.WriteLog(string.Format("MD5加密出错：{0}",ex.Message),LogType.ERROR,false);
                return "";
            }
        }

        /// <summary>
        /// MD5解密  
        /// </summary>
        /// <param name="pToDecrypt"></param>
        /// <param name="sKey">key</param>
        /// <returns></returns>
        public string MD5Decrypt(string pToDecrypt, string sKey)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();

            byte[] inputByteArray = new byte[pToDecrypt.Length / 2];
            try
            {
                for (int x = 0; x < pToDecrypt.Length / 2; x++)
                {
                    int i = (Convert.ToInt32(pToDecrypt.Substring(x * 2, 2), 16));
                    inputByteArray[x] = (byte)i;
                }

                des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
                des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();

                StringBuilder ret = new StringBuilder();

                return System.Text.Encoding.Default.GetString(ms.ToArray());
            }
            catch (Exception ex)
            {
                
                Logout.WriteLog(string.Format("MD5解密出错：{0}",ex.Message),LogType.ERROR,false);
                return "";
            }
        }
        #endregion
    }
}
