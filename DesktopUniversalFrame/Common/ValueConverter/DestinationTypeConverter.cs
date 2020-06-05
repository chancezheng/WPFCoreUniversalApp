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
            var diagnoseState = DiagnoseState.Undiagnose;
            if (Enum.TryParse(typeof(DiagnoseState), value.ToString(), out var state))
                diagnoseState = (DiagnoseState)(state ?? 0);

            return diagnoseState;
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            return (int)value;
        }
    }

    /// <summary>
    /// 性别转换
    /// </summary>
    public class GenderIntConverter: TypeConverter
    {
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            var gender = Gender.Male;
            if (Enum.TryParse(typeof(Gender), value.ToString(), out var gd))
                gender = (Gender)(gd ?? 0);

            return gender;
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            return (int)value;
        }
    }
}
