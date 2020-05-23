using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace DesktopUniversalFrame.Entity
{
    /// <summary>
    /// IOC
    /// </summary>
    public class IOCFactory
    {
        /// <summary>
        /// 创建实例(无参)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="asmName"></param>
        /// <returns></returns>
        public static T CreateInstance<T>(string asmName = "DesktopUniversalFrame")
        {
            Assembly assembly = Assembly.Load(asmName);
            Type type = assembly.GetType(typeof(T).Name);
            T instance = (T)Activator.CreateInstance(type);
            return instance;
        }

        /// <summary>
        /// 创建实例(有参)
        /// </summary>
        /// <typeparam name="T1">抽象类</typeparam>
        /// <typeparam name="T2">目标类</typeparam>
        /// <param name="t1">抽象类实例</param>
        /// <param name="asmName">程序集</param>
        /// <returns>T2</returns>
        public static T2 CreateInstance<T1,T2>(T1 t1 ,string asmName= "DesktopUniversalFrame") where T1 : class where T2 : class
        {
            Assembly assembly = Assembly.Load(asmName);
            Type type = assembly.GetType(typeof(T2).Name);
            T2 instance = (T2)Activator.CreateInstance(type, new object[] { t1 });
            return instance;
        }
    }
}
