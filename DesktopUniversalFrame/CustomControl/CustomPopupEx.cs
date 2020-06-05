using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DesktopUniversalFrame.CustomControl
{
    /// <summary>
    /// Popup控件
    /// </summary>
    public class CustomPopupEx : Popup
    {
        static CustomPopupEx()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CustomPopupEx), new FrameworkPropertyMetadata(typeof(CustomPopupEx)));
        }


        /// <summary>
        /// 阴影效果
        /// </summary>
        public Effect PopupEffect
        {
            get { return (Effect)GetValue(PopupEffectProperty); }
            set { SetValue(PopupEffectProperty, value); }
        }

        public static readonly DependencyProperty PopupEffectProperty =
            DependencyProperty.Register("PopupEffect", typeof(Effect), typeof(CustomPopupEx), new PropertyMetadata(dropEffect));

        static DropShadowEffect dropEffect = new DropShadowEffect()
        {
            BlurRadius = 10,
            ShadowDepth = 1,
            Color = Colors.Gray,
            Direction = 90,
            Opacity = 0.9,
        };


        #region CustomPopupEx随窗口移动

        public CustomPopupEx()
        {
            this.Loaded += CustomPopupEx_Loaded;
        }

        /// <summary>
        /// 是否随窗口移动
        /// </summary>
        public bool IsPositionUpdate
        {
            get { return (bool)GetValue(IsPositionUpdateProperty); }
            set { SetValue(IsPositionUpdateProperty, value); }
        }

        public static readonly DependencyProperty IsPositionUpdateProperty =
            DependencyProperty.Register("IsPositionUpdate", typeof(bool), typeof(CustomPopupEx), new PropertyMetadata(true, new PropertyChangedCallback(IsPositionUpdateChanged)));

        private static void IsPositionUpdateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as CustomPopupEx).CustomPopupEx_Loaded(d as CustomPopupEx, null);
        }

        private void CustomPopupEx_Loaded(object sender, RoutedEventArgs e)
        {
            Popup pop = sender as Popup;
            var win = VisualTreeHelper.GetParent(pop);
            while (win != null && (win as Window) == null)
            {
                win = VisualTreeHelper.GetParent(win);
            }
            if ((win as Window) != null)
            {
                (win as Window).LocationChanged -= PositionChanged;
                (win as Window).SizeChanged -= PositionChanged;
                if (IsPositionUpdate)
                {
                    (win as Window).LocationChanged += PositionChanged;
                    (win as Window).SizeChanged += PositionChanged;
                }
            }
        }

        /// <summary>
        /// 位置刷新
        /// </summary>
        private void PositionChanged(object sender, EventArgs e)
        {
            try
            {
                var method = typeof(Popup).GetMethod("UpdatePosition", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                if (this.IsOpen)
                {
                    method.Invoke(this, null);
                }
            }
            catch 
            {
                return;
            }
        }


        /// <summary>
        /// Popup是否置最前
        /// </summary>
        public bool Topmost
        {
            get { return (bool)GetValue(TopmostProperty); }
            set { SetValue(TopmostProperty, value); }
        }

        public static readonly DependencyProperty TopmostProperty =
            DependencyProperty.Register("Topmost", typeof(bool), typeof(CustomPopupEx), new PropertyMetadata(false, OnTopmostChanged));

        private static void OnTopmostChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as CustomPopupEx).UpdateWindow();
        }

        //重写拉开方法，置于非最前
        protected override void OnOpened(EventArgs e)
        {
            UpdateWindow();
        }

        /// <summary>
        /// 刷新Popup层级
        /// </summary>
        private void UpdateWindow()
        {
            IntPtr intPtr = new IntPtr();
            if(Child != null)
                intPtr = ((HwndSource)PresentationSource.FromVisual(this.Child)).Handle;

            RECT rect;
            if(NativeMethods.GetWindowRect(intPtr, out rect))
            {
                NativeMethods.SetWindowPos(intPtr, Topmost ? -1 : -2, rect.Left, rect.Top, (int)Width, (int)Height, 0);
            }
        }

        private struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        private static class NativeMethods
        {
            [DllImport("user32.dll")]
            [return: MarshalAs(UnmanagedType.Bool)]

            internal static extern bool GetWindowRect(IntPtr hwnd, out RECT lpRect);

            [DllImport("user32", EntryPoint = "SetWindowPos")]
            internal static extern int SetWindowPos(IntPtr hwnd, int hwndInsertAfter, int x, int y, int cx, int cy, int wFlags);
        }

        #endregion
    }
}
