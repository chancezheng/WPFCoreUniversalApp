using Chance.DesktopCustomControl.ExposedMethod;
using DesktopUniversalFrame.ViewModel.Login;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace DesktopUniversalFrame.Common
{
    public class AttachedProperty : DependencyObject
    {
        public static Style GetToolTipStyle(DependencyObject obj)
        {
            return (Style)obj.GetValue(ToolTipStyleProperty);
        }
        public static void SetToolTipStyle(DependencyObject obj, Style value)
        {
            obj.SetValue(ToolTipStyleProperty, value);
        }
        //ToolTip样式
        public static readonly DependencyProperty ToolTipStyleProperty =
            DependencyProperty.RegisterAttached("ToolTipStyle", typeof(Style), typeof(AttachedProperty), 
                new PropertyMetadata(ComponentStyle.GetComponentStyle("toolTip")));


        public static UserOperationType GetOperationType(DependencyObject obj)
        {
            return (UserOperationType)obj.GetValue(OperationTypeProperty);
        }

        public static void SetOperationType(DependencyObject obj, UserOperationType value)
        {
            obj.SetValue(OperationTypeProperty, value);
        }

        //用户操作(注册、忘记密码、登陆)
        public static readonly DependencyProperty OperationTypeProperty =
            DependencyProperty.RegisterAttached("OperationType", typeof(UserOperationType), typeof(AttachedProperty), 
                new PropertyMetadata(default(UserOperationType)));
    }
}
