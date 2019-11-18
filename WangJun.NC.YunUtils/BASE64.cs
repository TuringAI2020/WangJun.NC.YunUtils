using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WangJun.NC.YunUtils
{
    public static class BASE64
    {
        public static string ToBase64String(string input,string encoding="utf8") {
            if (!string.IsNullOrWhiteSpace(input)) {
                var bytes = new byte[0];
                if (!string.IsNullOrWhiteSpace(encoding)&&( "utf8" == encoding.ToLower().Trim() || "utf-8" == encoding.ToLower().Trim())) {
                    bytes = Encoding.UTF8.GetBytes(input);
                }
                else if (!string.IsNullOrWhiteSpace(encoding) && ("gbk" == encoding.ToLower().Trim() || "gb2312" == encoding.ToLower().Trim()))
                {
                    bytes = Encoding.GetEncoding("GB2312").GetBytes(input);
                }

                var res = Convert.ToBase64String(bytes);
                return res;
            }
            return string.Empty;
        }

        public static string FromBase64String(string input, string encoding = "utf8") {
            if (!string.IsNullOrWhiteSpace(input))
            {
                var bytes = Convert.FromBase64String(input);
                var res = string.Empty;
                if (!string.IsNullOrWhiteSpace(encoding) && ("utf8" == encoding.ToLower().Trim() || "utf-8" == encoding.ToLower().Trim()))
                {
                    res = Encoding.UTF8.GetString(bytes);
                }
                else if (!string.IsNullOrWhiteSpace(encoding) && ("gbk" == encoding.ToLower().Trim() || "gb2312" == encoding.ToLower().Trim()))
                {
                    res = Encoding.GetEncoding("GB2312").GetString(bytes);
                }
                
                return res;
            }
            return string.Empty;
        }
    }
}
