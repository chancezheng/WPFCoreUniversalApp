using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Controls;

namespace DesktopUniversalFrame.Entity.CustomValidationRules
{
    /// <summary>
    /// NotEmptyValidationRule
    /// </summary>
    public class NotEmptyValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            return string.IsNullOrWhiteSpace((value ?? "").ToString())
                ? new ValidationResult(false, "不能为空")
                : ValidationResult.ValidResult;
        }
    }
}
