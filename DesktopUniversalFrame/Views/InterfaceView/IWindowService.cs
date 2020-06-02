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
        void ShowWindow(object ViewModel, FrameworkElement frameworkElement);
    }
}
