using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WangJun.NC.YunUtils
{
    public static class GUID
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsGuid(string input)
        {
            var guid = Guid.Empty;
            return Guid.TryParse(input, out guid);
        }

        public static Guid FromStringToGuid(string input)
        {
            var bytes = MD5.ToMD5Bytes(input); 
            var guid = new Guid(bytes);
            return guid;
        }
    }
}
