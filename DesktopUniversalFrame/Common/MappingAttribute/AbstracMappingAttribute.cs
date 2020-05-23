using System;
using System.Collections.Generic;
using System.Text;

namespace DesktopUniversalFrame.Common.MappingAttribute
{
    /// <summary>
    /// 特性基类
    /// </summary>
    public abstract class AbstracMappingAttribute : Attribute
    {
        private string _mappingName = null;

        public AbstracMappingAttribute(string name)
        {
            this._mappingName = name;
        }

        public string GetMappingName()
        {
            return _mappingName;
        } 
    }
}
