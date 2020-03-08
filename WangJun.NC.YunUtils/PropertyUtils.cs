using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Linq;

namespace WangJun.NC.YunUtils
{
    public static class PropertyUtils
    {
        private static Dictionary<string, List<PropertyInfo>> dict = new Dictionary<string, List<PropertyInfo>>();
        public static List<PropertyInfo> GetProperties<T>() where T : class 
        {
            var classFullName = typeof(T).FullName;
            var properties = typeof(T).GetProperties().ToList() ;

            if (!dict.ContainsKey(classFullName))
            {
                dict.Add(classFullName, properties);
            }
            return dict[classFullName];
        }
    }
}
