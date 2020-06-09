using HelixToolkit.Wpf;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;

namespace DesktopUniversalFrame.GraphicsCutting._3D
{
    /// <summary>
    /// DemoWindow.xaml 的交互逻辑
    /// </summary>
    public partial class DemoWindow : Window
    {
        public DemoWindow()
        {
            InitializeComponent();

            CalculateNormal();
        }

        double x, y, z;
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var builder = new MeshBuilder(false, true);
            builder.AddPipe(new Point3D(1,1,1), new Point3D(20, 20, 10), 0.2, 0.9, 10);
        }

        private void ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            CalculateNormal();
        }

        private void CalculateNormal()
        {
            if (alpha == null || beta == null || PlaneVector3D == null)
                return;

            double a = alpha.Value / 180D * Math.PI; //切平面夹角弧度值
            double b = beta.Value / 180D * Math.PI;  //旋转角弧度值
            double A = Math.Sqrt(1D / (2D + Math.Pow(Math.Tan(a), 2D)));
            x = Math.Sqrt(1 - A) * Math.Cos(b) * Math.Tan(b) + PX.Value;
            y = Math.Sqrt(1 - A) * Math.Cos(b) + PY.Value;
            z = Math.Sqrt(A) + PZ.Value;
            PlaneVector3D.Normal = new Vector3D(x, y, z);
            CuttingPlaneGroup.IsEnabled = false;
            CuttingPlaneGroup.IsEnabled = true;
        }
    }
}
