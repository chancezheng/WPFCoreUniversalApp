using DesktopUniversalFrame.Model.MedicalModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Text;

namespace DesktopUniversalFrame.Common.ValueConverter
{
    /// <summary>
    /// 诊断状态类型转换
    /// </summary>
    public class EnumToDiagnoseStateConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return true;
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            int res = int.Parse(value.ToString());
            if (res == 0)
                return DiagnoseState.Undiagnose;
            else if (res == 1)
                return DiagnoseState.Diagnosed;
            else if (res == 2)
                return DiagnoseState.Reviewered;
            else
                return null;
        }
    }
}
