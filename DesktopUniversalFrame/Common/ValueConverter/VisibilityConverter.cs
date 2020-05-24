using DesktopUniversalFrame.ViewModel.Login;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Data;

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
}
