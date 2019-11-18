using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WangJun.NC.YunUtils
{
    public static class MD5
    {
        /// <summary>
        /// MD5 加密
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToMD5(string input)
        {
            byte[] cleanBytes = Encoding.Default.GetBytes(input);
            byte[] hashedBytes = System.Security.Cryptography.MD5.Create().ComputeHash(cleanBytes);
            return BitConverter.ToString(hashedBytes).Replace("-", string.Empty);
        }
        public static byte[] ToMD5Bytes(string input)
        {
            byte[] cleanBytes = Encoding.Default.GetBytes(input);
            byte[] hashedBytes = System.Security.Cryptography.MD5.Create().ComputeHash(cleanBytes);
            return hashedBytes;
        }

        /// <summary>
        /// 转换为16位的MD5
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToMD5_16(string input)
        {
            var md5_32 = MD5.ToMD5(input);
            return md5_32.Substring(8, 16);
        }


        public static string ToMD5(byte[] input)
        {
            byte[] hashedBytes = System.Security.Cryptography.MD5.Create().ComputeHash(input);
            return BitConverter.ToString(hashedBytes).Replace("-", string.Empty);
        }
    }
}
