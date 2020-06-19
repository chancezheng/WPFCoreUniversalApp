using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace DesktopUniversalFrame.Views.InterfaceView
{
    /// <summary>
    /// 窗口
    /// </summary>
    public interface IWindowService
    {
        void ShowWindow<T>(object ViewModel, FrameworkElement frameworkElement) where T : Window;
    }
}
