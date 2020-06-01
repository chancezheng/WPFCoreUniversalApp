using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DesktopUniversalFrame.CustomControl
{
    /// <summary>
    /// CustomRadioButton
    /// </summary>
    public class CustomRadioButton : RadioButton
    {
        static CustomRadioButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CustomRadioButton), new FrameworkPropertyMetadata(typeof(CustomRadioButton)));
        }


        /// <summary>
        /// CornerRadius
        /// </summary>
        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(CustomRadioButton), new PropertyMetadata());


        /// <summary>
        /// StrokeThickness
        /// </summary>
        public double StrokeThickness
        {
            get { return (double)GetValue(StrokeThicknessProperty); }
            set { SetValue(StrokeThicknessProperty, value); }
        }
        public static readonly DependencyProperty StrokeThicknessProperty =
            DependencyProperty.Register("StrokeThickness", typeof(double), typeof(CustomRadioButton), new PropertyMetadata(0D));


        /// <summary>
        /// Path路径
        /// </summary>
        public Geometry ShapePath
        {
            get { return (Geometry)GetValue(ShapePathProperty); }
            set { SetValue(ShapePathProperty, value); }
        }
        public static readonly DependencyProperty ShapePathProperty =
            DependencyProperty.Register("ShapePath", typeof(Geometry), typeof(CustomRadioButton), new PropertyMetadata(default(Geometry)));


        /// <summary>
        /// 开启跑马灯
        /// </summary>
        public bool StartRunningLight
        {
            get { return (bool)GetValue(StartRunningLightProperty); }
            set { SetValue(StartRunningLightProperty, value); }
        }
        public static readonly DependencyProperty StartRunningLightProperty =
            DependencyProperty.Register("StartRunningLight", typeof(bool), typeof(CustomRadioButton), new PropertyMetadata(true));
    }
}
