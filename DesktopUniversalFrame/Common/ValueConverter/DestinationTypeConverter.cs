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
            //int res = int.Parse(value.ToString());
            //if (res == 0)
            //    return DiagnoseState.Undiagnose;
            //else if (res == 1)
            //    return DiagnoseState.Diagnosed;
            //else if (res == 2)
            //    return DiagnoseState.Reviewered;
            //else
            //    return null;

            var diagnoseState = DiagnoseState.Undiagnose;
            if (Enum.TryParse(typeof(DiagnoseState), value.ToString(), out var state))
                diagnoseState = (DiagnoseState)(state ?? 0);

            return diagnoseState;
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            //DiagnoseState diagnoseState = (DiagnoseState)value;
            //if (diagnoseState == DiagnoseState.Undiagnose)
            //    return 0;
            //else if (diagnoseState == DiagnoseState.Diagnosed)
            //    return 1;
            //else if (diagnoseState == DiagnoseState.Reviewered)
            //    return 2;
            //else
            //    return null;

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
