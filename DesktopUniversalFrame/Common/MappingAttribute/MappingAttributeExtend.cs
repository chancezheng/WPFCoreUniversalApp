using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Text;

namespace DesktopUniversalFrame.Common.MappingAttribute
{
    /// <summary>
    /// 特性方法扩展
    /// </summary>
    public static class MappingAttributeExtend
    {
        /// <summary>
        /// 获取Type的名称
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetAttributeMappingName<T>(this T type) where T : MemberInfo
        {
            if (type.IsDefined(typeof(TableAttribute), true))
            {
                TableAttribute tableAttribute = type.GetCustomAttribute<TableAttribute>();
                return tableAttribute.Name;
            }
            else if (type.IsDefined(typeof(AbstracMappingAttribute), true))
            {
                AbstracMappingAttribute attribute = type.GetCustomAttribute<AbstracMappingAttribute>();
                return attribute.GetMappingName();
            }
            else
            {
                return type.Name;
            }
        }
    }
}
