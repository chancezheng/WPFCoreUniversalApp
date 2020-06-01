using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace DesktopUniversalFrame.Common.MappingAttribute
{
    public static class KeyFilter
    {
        /// <summary>
        /// 除去键值
        /// </summary>
        /// <param name="propertyInfos"></param>
        /// <returns></returns>
        public static IEnumerable<PropertyInfo> ExceptKey(this IEnumerable<PropertyInfo> propertyInfos)
        {
            return propertyInfos.Where(p => !p.IsDefined(typeof(KeyAttribute), true));
        }

        /// <summary>
        /// 得到键值
        /// </summary>
        /// <param name="propertyInfos"></param>
        /// <returns></returns>
        public static PropertyInfo GetKeyInfo(this IEnumerable<PropertyInfo> propertyInfos)
        {
            return propertyInfos.Where(p => p.IsDefined(typeof(KeyAttribute), true)).FirstOrDefault();
        }

        /// <summary>
        /// 除去忽略属性
        /// </summary>
        /// <param name="propertyInfos"></param>
        /// <returns></returns>
        public static IEnumerable<PropertyInfo>ExcepteIgnoreProperty(this IEnumerable<PropertyInfo> propertyInfos)
        {
            return propertyInfos.Where(p => !p.IsDefined(typeof(IgnoreSomePropertyAttribute), true));
        }
    }
}
