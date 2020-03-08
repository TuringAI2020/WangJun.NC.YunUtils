using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace TestYunDB
{
    public static class FastMethodInvoker
    {
        private static MethodInfo _InvokeByReflectionCache = null;
        public static object InvokeByReflection(object targetObject,string methodName , object[] paramArray)
        {
            var res = targetObject.GetType().GetMethod(methodName).Invoke(targetObject, paramArray);
            return res;
        }

        public static object InvokeByReflectionCache(object targetObject, string methodName, object[] paramArray)
        {
            if (null == _InvokeByReflectionCache)
            {
                _InvokeByReflectionCache = targetObject.GetType().GetMethod(methodName);
            }
            var res = _InvokeByReflectionCache.Invoke(targetObject, paramArray);
            return res;
        }

        public static Delegate InvokeByDelegate<FunT>(object targetObject,string methodName)
        {
            var typeInfo = targetObject.GetType() ;
            var methodInfo = typeInfo.GetMethod(methodName);
            var res = Delegate.CreateDelegate(typeof(FunT), targetObject, methodInfo);
            return res;
        }


    }
}
