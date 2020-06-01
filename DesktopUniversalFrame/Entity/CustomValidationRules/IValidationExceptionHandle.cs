using DesktopUniversalFrame.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace DesktopUniversalFrame.Entity.CustomValidationRules
{
    public interface IValidationExceptionHandle
    {
        /// <summary>
        /// 是否有错误
        /// </summary>
        public bool HasValidationError { get; set; }
    }
}
