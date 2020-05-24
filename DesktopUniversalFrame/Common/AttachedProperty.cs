using Chance.DesktopCustomControl.ExposedMethod;
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
    }
}
