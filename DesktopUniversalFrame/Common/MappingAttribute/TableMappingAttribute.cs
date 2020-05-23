using System;
using System.Collections.Generic;
using System.Text;

namespace DesktopUniversalFrame.Common.MappingAttribute
{
    /// <summary>
    /// 表特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class TableMappingAttribute : AbstracMappingAttribute
    {
        public TableMappingAttribute(string name) : base(name)
        {

        }
    }
}
