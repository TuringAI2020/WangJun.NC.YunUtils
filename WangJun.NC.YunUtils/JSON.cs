using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WangJun.NC.YunUtils
{
    public class JSON
    {
        public static T ToObject<T>(string json){
            return JsonConvert.DeserializeObject<T>(json);
        }

        public static string ToJson(object data) {
            return JsonConvert.SerializeObject(data);
        }

        public static string StreamToJson(Stream stream,Encoding encoding) {
            var count = (int)(stream.Length % int.MaxValue);
            var bytes = new byte[stream.Length % int.MaxValue];
            stream.Read(bytes, 0, count);
            var str = encoding.GetString(bytes);
            return str;
        }

        public static string GetValue(string json,string keyName1, string keyName2 = null) {
            var dict = JSON.ToObject<Dictionary<string, object>>(json);
            var value1 = dict[keyName1] as JObject;
            if (!string.IsNullOrWhiteSpace(keyName2) && null != value1)
            {
                var value2 = value1[keyName2];
                return JsonConvert.SerializeObject( value2);
            } 
                return JsonConvert.SerializeObject(value1);
 
        }
    }
}
