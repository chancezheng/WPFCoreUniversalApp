using System;
using System.Collections.Generic;
using System.Text;

namespace DesktopUniversalFrame.Common.MappingAttribute
{
    /// <summary>
    /// 忽略某些属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class IgnoreSomePropertyAttribute : AbstracMappingAttribute
    {
        public IgnoreSomePropertyAttribute(string name) : base(name)
        {

        }
    }
}
