using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WangJun.NC.YunUtils
{
    public class JRequest
    {
        public string Method { get; set; }

        public string[] Param { get; set; }

        public static JRequest Parse(string jsonString) {
            try
            {
                var inst = new JRequest();
                inst = JSON.ToObject<JRequest>(jsonString);
                return inst;
            }
            catch (Exception ex) {
                return null;
            }
        }
    }
}
