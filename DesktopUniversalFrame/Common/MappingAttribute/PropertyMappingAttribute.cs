using System;
using System.Collections.Generic;
using System.Text;

namespace DesktopUniversalFrame.Common.MappingAttribute
{
    /// <summary>
    /// 属性特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class PropertyMappingAttribute : AbstracMappingAttribute
    {
        public PropertyMappingAttribute(string name) : base(name)
        {

        }
    }
}
