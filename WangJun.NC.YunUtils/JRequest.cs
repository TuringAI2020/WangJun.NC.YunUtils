using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WangJun.NC.YunUtils
{
    public class JRequest
    {
        public string Method { get; set; }

        public string[] Param { get; set; }

        public static JRequest Parse(string jsonString)
        {
            try
            {
                var inst = new JRequest();
                inst = JSON.ToObject<JRequest>(jsonString);
                return inst;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static JRequest Parse(Stream stream)
        {
            try {
                StreamReader reader = new StreamReader(stream);
                var readTask = reader.ReadToEndAsync();
                var jsonString = readTask.Result;
                //var bytes = new byte[stream.];
                //stream.Read(bytes, 0, bytes.Length);
                //var jsonString = Encoding.UTF8.GetString(bytes);
                var inst = Parse(jsonString);
                return inst;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static string ToJson(string method, string[] param) 
        {
            var inst = new JRequest { Method = method, Param = param };
            return JSON.ToJson(inst);
        }

        public RES Execute(object target)
        {
            if (null != target)
            {
                var res = target.GetType().GetMethod(this.Method).Invoke(target, this.Param) as RES;
                return res;
            }
            return RES.FAIL("调用的对象为空");
        }
    }
}
