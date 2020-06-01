using Chance.DesktopCustomControl.CustomComponent;
using DesktopUniversalFrame.ViewModel.Login;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace DesktopUniversalFrame.Common.ValueConverter
{
    /// <summary>
    /// 转换器（注册、登录、忘记密码）
    /// </summary>
    public class LoginRegisterVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var operationType = (UserOperationType)value;
            string operationName = parameter.ToString();
            if (operationName.Contains(operationType.ToString())) //注意大小写，不然要会找不到界面报错
                return Visibility.Visible;
            else
                return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 转换器（窗口大小切换时）
    /// </summary>
    public class SwitchWindowSizeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int windowState = (int)value;
            if (windowState == int.Parse(parameter.ToString()))
                return Visibility.Visible;
            else
                return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 按钮是否可用(提交病人挂号信息)
    /// </summary>
    public class SubmitPatientInfoButtonIsEnabled : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            foreach (var item in values)
            {
                if (string.IsNullOrEmpty(item as string))
                    return false;
            }
            return true;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 按钮背景色(提交病人挂号信息)
    /// </summary>
    public class SubmitPatientInfoButtonBackground : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value == true ? Brushes.LimeGreen : Brushes.DarkGray;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
