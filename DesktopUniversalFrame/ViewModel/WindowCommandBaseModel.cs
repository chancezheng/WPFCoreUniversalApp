using DesktopUniversalFrame.Common;
using DesktopUniversalFrame.CustomControl;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
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




        //Popup加载
        private DelegateCommand<CustomPopupEx> _loadedPopupCommand;
        public DelegateCommand<CustomPopupEx> LoadedPopupCommand
        {
            get { return _loadedPopupCommand; }
            set { _loadedPopupCommand = value; }
        }

        //Popup关闭
        public DelegateCommand<CustomPopupEx> PopupCloseCommand
        {
            get => new DelegateCommand<CustomPopupEx>(p => 
            {
                if (p != null)
                {
                    ScaleTransform scaleTransform = new ScaleTransform();
                    (p.Child as Grid).RenderTransform = scaleTransform;
                    Duration duration = new Duration(TimeSpan.FromSeconds(0.5));
                    DoubleAnimation doubleAnimationX = new DoubleAnimation(0.2D, duration);
                    doubleAnimationX.From = 1.0D;
                    doubleAnimationX.To = 0.2;
                    scaleTransform.BeginAnimation(ScaleTransform.ScaleXProperty, doubleAnimationX);

                    DoubleAnimation doubleAnimationY = new DoubleAnimation(0.2D, duration);
                    doubleAnimationY.From = 1.0D;
                    doubleAnimationY.To = 0.2;
                    scaleTransform.BeginAnimation(ScaleTransform.ScaleYProperty, doubleAnimationY);

                    Task.Run(() =>
                    {
                        Thread.Sleep(700);
                        App.Current.Dispatcher.Invoke(() => 
                        {                        
                            p.IsOpen = false;
                            scaleTransform = new ScaleTransform(1.0, 1.0);
                            (p.Child as Grid).RenderTransform = scaleTransform;
                        });
                    });
                }
            });
        }

        //Popup拖动（暂时无效）
        public DelegateCommand<CustomPopupEx> MovePopupCommand
        {
            get => new DelegateCommand<CustomPopupEx>(p =>
            {
                if (p != null)
                {
                    p.Child.MouseMove += (sender, e) =>
                    {
                        var point = e.GetPosition(p.Child);
                        if (e.LeftButton == MouseButtonState.Pressed && point.Y <= 35)
                        {
                            //p.IsPositionUpdate = false;
                            try
                            {
                                var method = typeof(Popup).GetMethod("UpdatePosition", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                                if (p.IsOpen)
                                {
                                    method.Invoke(p, null);
                                }
                            }
                            catch (Exception ex)
                            {
                                var ss = ex.Message;
                                return;
                            }

                            e.Handled = true;
                        }
                    };
                }
            });
        }
    }
}
