using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DesktopUniversalFrame.Graphics.GraphicsCuttingStitching
{
    /// <summary>
    /// GraphicsCuttingStitching.xaml 的交互逻辑
    /// </summary>
    public partial class GraphicsCuttingStitching : RibbonWindow
    {
        public GraphicsCuttingStitching()
        {
            InitializeComponent();
        }

        private void TabItem_Loaded(object sender, RoutedEventArgs e)
        {
            //var path = @"\\DesktopUniversalFrame//Graphics//GraphicsCuttingStitching//DemoImages";
            var path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DemoImages");
            //var ss = Directory.GetParent(AppContext.BaseDirectory);
            var imgs = ImageConverterHelper.GetImages(path);
            var image = ImageConverterHelper.StitchingImages(imgs);
            image.Save(Directory.GetParent(path) + "//DemoResult//stitching.png");



            Bitmap bitmap = new Bitmap(image);

            MemoryStream ms = new MemoryStream();
            bitmap.Save(ms, ImageFormat.Png);
            ImageSourceConverter imageSourceConverter = new ImageSourceConverter();
            imgStitching.Source = (ImageSource)imageSourceConverter.ConvertFrom(ms);
        }
    }
}
