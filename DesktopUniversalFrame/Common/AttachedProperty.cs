using Chance.DesktopCustomControl.ExposedMethod;
using DesktopUniversalFrame.ViewModel.Login;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

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


        public static bool GetIsWindowPrepareClosing(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsWindowPrepareClosingProperty);
        }
        public static void SetIsWindowPrepareClosing(DependencyObject obj, bool value)
        {
            obj.SetValue(IsWindowPrepareClosingProperty, value);
        }
        //Window是否准备关闭
        public static readonly DependencyProperty IsWindowPrepareClosingProperty =
            DependencyProperty.RegisterAttached("IsWindowPrepareClosing", typeof(bool), typeof(AttachedProperty), 
                new PropertyMetadata(false,new PropertyChangedCallback(WindowPrepareClosingChanged)));
        private static void WindowPrepareClosingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var win = d as Window;
            Task.Run(() =>
            {
                Thread.Sleep(1200);
                App.Current.Dispatcher.Invoke(()=> { win.Close(); });
            });
        }
    }
}
