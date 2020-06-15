using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Brushes = System.Drawing.Brushes;
using Color = System.Drawing.Color;
using Image = System.Drawing.Image;

namespace DesktopUniversalFrame.Graphics.GraphicsCuttingStitching
{
    public class ImageConverterHelper
    {
        /// <summary>
        /// 拼接图片
        /// </summary>
        /// <param name="images"></param>
        /// <param name="destWidth"></param>
        /// <param name="destHeight"></param>
        /// <returns></returns>
        public static Image StitchingImages(Image[] images, int destWidth = 1024, int destHeight = 1024)
        {
            var width = destWidth;
            var height = destHeight;
            var format = PixelFormat.Format32bppArgb;
            using (Bitmap bp = new Bitmap(width, height, format))
            {
                using (var gr = System.Drawing.Graphics.FromImage(bp))
                {
                    gr.FillRectangle(Brushes.White, new Rectangle(0, 0, width, height));
                    gr.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    gr.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                    int width1 = 0, height1 = 0; int valueStandard = destWidth / 2;
                    foreach (var img in images)
                    {
                        using (var newImage = ScaleTransformImage(img, valueStandard, valueStandard))
                        {
                            if(width1>=width)
                            {
                                width1 = 0;
                                height1 += newImage.Height;
                            }
                            gr.DrawImage(newImage, new Rectangle(width1, height1, valueStandard, valueStandard), new Rectangle(0, 0, newImage.Width, newImage.Height), GraphicsUnit.Pixel);
                            width1 += newImage.Width;
                        }
                    }
                    gr.Save();
                    gr.Dispose();
                    foreach (var img in images)
                    {
                        img.Dispose();
                    }                   
                }

                using (var ms = new MemoryStream())
                {
                    bp.Save(ms, ImageFormat.Png);
                    var buffer = ms.ToArray();
                    return ConvertToImage(buffer);
                }
            }
        }

        /// <summary>
        /// 伸缩图片
        /// </summary>
        /// <param name="image"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static Image ScaleTransformImage(Image image, int w = 150, int h = 150)
        {
            int width = w;
            int height = h;
            var bitmap = new Bitmap(width, height);
            using (var g = System.Drawing.Graphics.FromImage(bitmap))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;//设置质量
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;//设置质量
                g.Clear(Color.White);//置背景色
                g.DrawImage(image, new Rectangle(0, 0, bitmap.Width, bitmap.Height), new Rectangle(0, 0, image.Width, image.Height), GraphicsUnit.Pixel);  //画图
                image.Dispose();//释放原图
                return bitmap;
            }
        }


        /// <summary>
        /// 得到图片集
        /// </summary>
        /// <param name="imageUrl"></param>
        public static Image[] GetImages(string imageFile)
        {
            var list = new List<Image>();
            foreach (var path in Directory.GetFiles(imageFile))
            {
                var image = Image.FromFile(path);
                list.Add(image);
            }
            return list.ToArray();
        }


        /// <summary>
        /// Image转Byte数组
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public static byte[] ConvertToBytes(Image image)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, image.RawFormat);
                return ms.ToArray();
            }
        }

        /// <summary>
        /// Byte数组转Image
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static Image ConvertToImage(byte[] buffer)
        {
            using (MemoryStream ms = new MemoryStream(buffer))
            {
                return Image.FromStream(ms);
            }
        }
    }
}
