using System;
using System.Collections.Generic;
using System.Text;

namespace DesktopUniversalFrame.Common.MappingAttribute
{
    /// <summary>
    /// 唯一主键
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class PrimaryKeyAttribute : Attribute
    {
        public PrimaryKeyAttribute()
        {

        }
    }
}
