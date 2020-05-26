using Chance.DesktopCustomControl.CustomComponent;
using Chance.DesktopCustomControl.NotifycationObject;
using Chance.DesktopCustomControl.Resource.Transitions;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace DesktopUniversalFrame.ViewModel
{
    /// <summary>
    /// 窗口操作基类命令绑定
    /// </summary>
    public class WindowCommandBaseModel : NotifyPropertyChanged
    {
        //拖动窗口
        public DelegateCommand<Window> MoveWindowCommand
        {
            get => new DelegateCommand<Window>(win =>
            {
                if (win != null)
                {
                    win.MouseMove += (sender, e) =>
                    {
                        var point = e.GetPosition(win);
                        if (e.LeftButton == MouseButtonState.Pressed && point.Y <= 30)
                        {
                            win.DragMove();
                            e.Handled = true;
                        }
                    };
                }
            });
        }

        //最小化
        public DelegateCommand<Window> MinWindowCommand
        {
            get => new DelegateCommand<Window>(win =>
            {
                if (win != null)
                    win.WindowState = WindowState.Minimized;
            });
        }

        //最大化
        public DelegateCommand<Window> MaxWindowCommand
        {
            get => new DelegateCommand<Window>(win =>
            {
                if (win != null)
                {
                    if (win.WindowState == WindowState.Normal)
                        win.WindowState = WindowState.Maximized;
                    else
                        win.WindowState = WindowState.Normal;
                }
            });
        }

        //还原
        public DelegateCommand<Window> NormalWindowCommand
        {
            get => new DelegateCommand<Window>(win =>
            {
                if (win != null)
                {
                    win.WindowState = WindowState.Normal;
                }
            });
        }

        //关闭窗口
        public DelegateCommand<Window> CloseWindowCommand
        {
            get => new DelegateCommand<Window>(win =>
            {
                if (win != null)
                {
                    //1.附加属性控制动画
                    Common.AttachedProperty.SetIsWindowPrepareClosing(win, true);

                    //2.利用动画Completed事件
                    //Storyboard EndStoryboard = win.Resources["CloseStoryboard"] as Storyboard;
                    //EndStoryboard.Begin();
                }
            });
        }

        //窗口加载
        private DelegateCommand<Window> _loadedWindowCommand;
        public DelegateCommand<Window> LoadedWindowCommand
        {
            get { return _loadedWindowCommand; }
            set { _loadedWindowCommand = value; }
        }

        //Popup关闭
        public DelegateCommand<CustomPopupEx> PopupCloseCommand
        {
            get => new DelegateCommand<CustomPopupEx>(p => 
            {
                if (p != null)
                    p.IsOpen = false;
            });
        }
    }
}
