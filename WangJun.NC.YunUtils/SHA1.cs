using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WangJun.NC.YunUtils
{
    public static class SHA1
    {
        /// <summary>
        /// SHA1 加密
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToSHA1(string input)
        {
            byte[] cleanBytes = Encoding.Default.GetBytes(input);
            byte[] hashedBytes = System.Security.Cryptography.SHA1.Create().ComputeHash(cleanBytes);
            return BitConverter.ToString(hashedBytes).Replace("-", string.Empty);
        }

    }
}
