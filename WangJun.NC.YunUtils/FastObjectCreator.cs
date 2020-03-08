using System;
using System.Collections.Generic;
using System.Text;
using System.Linq.Expressions;
using System.Reflection;
using System.Linq;

namespace TestYunDB
{
    public static class FastObjectCreator
    {
        private static object _CreateByExpressionCache = null;
        private static Assembly _CreateByAssemblyCache = null;
        public static T CreateByNew<T>() where T : class, new()
        {
            var instance =  new T();
            return instance;
        }

        public static object CreateByExpression<T>()
        {
            var @new = Expression.New(typeof(T));
            var lambda = Expression.Lambda<Func<T>>(@new).Compile();
            var instance = lambda.Invoke();
            return instance;
        }

        public static object CreateByExpressionCache<T>()
        {
            if (null == _CreateByExpressionCache)
            {
                var @new = Expression.New(typeof(T));
                var lambda = Expression.Lambda<Func<T>>(@new).Compile();
                _CreateByExpressionCache = lambda;
            }
            var instance = ((Func<T>)_CreateByExpressionCache).Invoke();
            return instance;
        }

        public static T CreateByActivator<T>()
        {
            var instance = Activator.CreateInstance<T>();
            return instance;
        }

        public static object CreateByAssembly<T>()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var instance = assembly.CreateInstance(typeof(T).FullName);
            return instance;
        }

        public static object CreateByAssemblyCache<T>()
        {
            if (null == _CreateByAssemblyCache)
            {
                _CreateByAssemblyCache = Assembly.GetExecutingAssembly();
            }
            var instance = _CreateByAssemblyCache.CreateInstance(typeof(T).FullName);
            return instance;
        }

        public static object CreateByAssembly(Assembly assembly,string classFullName,object[] constructorArgs)
        {
            var instance = assembly.CreateInstance(classFullName,true, BindingFlags.Default, null,constructorArgs,null,null);
            return instance;
        }

        public static dynamic CreateByDynamic<T>() where T : class, new()
        {
            dynamic target = new T();
            return target;
        }

        public static object CreateByDynamic(string classFullName, object[] constructorParam)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var typeInfo = assembly.GetType(classFullName);
            var paramTypeArr = new List<Type>();
            if (null != constructorParam) {
                constructorParam.ToList().ForEach(p=> {
                    paramTypeArr.Add(p.GetType());
                });
            }
            var inst = typeInfo.GetConstructor(paramTypeArr.ToArray()).Invoke(constructorParam);
            return inst;
        }
    }
}
