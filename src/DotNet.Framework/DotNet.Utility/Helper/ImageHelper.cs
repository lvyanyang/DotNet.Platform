// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;

namespace DotNet.Helper
{
    /// <summary>
    /// 图片管理
    /// </summary>
    /// <example>
    /// <code>
    /// using XCI.Helper;
    /// 
    /// const string originalImagePath = @"C:\demo.jpg";
    /// const string watermarkImagePath = @"C:\watermark.jpg";
    /// const string multipleImagePath = @"C:\multiplehorizontaldemo.png";
    /// 
    /// ImageHelper.CreateImageWatermark(ImageHelper.LoadImage(originalImagePath), ImageHelper.LoadImage(watermarkImagePath), @"C:\demoImageWatermark.jpg", 1, 100, 5);//加图片水印 
    /// ImageHelper.CreateTextWatermark(ImageHelper.LoadImage(originalImagePath), "西安交通信息", @"C:\demoTextWatermark.jpg", 1, 100, new Font("微软雅黑", 30F, FontStyle.Bold, GraphicsUnit.Pixel));//增加文字水印
    /// ImageHelper.CreateThumbnail(ImageHelper.LoadImage(originalImagePath), 200, 200, Color.Transparent).Save(@"C:\demoThumbnail.jpg");//创建图片缩略图 
    /// 
    /// Image originalImage = ImageHelper.LoadImage(originalImagePath);
    /// ImageHelper.FanSe(originalImage).Save(@"C:\demoFanSe.jpg");//以反色方式显示图像 
    /// ImageHelper.FuDiao(originalImage).Save(@"C:\demoFuDiao.jpg");//以浮雕方式显示图像
    /// ImageHelper.HeiBai(originalImage, 0).Save(@"C:\demoHeiBai.jpg");//以黑白方式显示图像 
    /// ImageHelper.RouHua(originalImage).Save(@"C:\demoRouHua.jpg");//以柔化方式显示图像 高斯模板法 
    /// ImageHelper.RuiHua(originalImage).Save(@"C:\demoRuiHua.jpg");//以锐化方式显示图像 拉普拉斯模板法
    /// ImageHelper.WuHua(originalImage).Save(@"C:\demoWuHua.jpg");//以雾化方式显示图像
    /// 
    /// ImageHelper.CreateNewImageCut(originalImage, 200, 200).Save(@"C:\demoCut.jpg");//根据源图片裁切为新图片 
    /// ImageHelper.CreateNewImageFill(originalImage, 200, 200, Color.Red).Save(@"C:\demoFill.jpg");//根据源图片填充为新图片
    /// ImageHelper.CreateNewImageScale(originalImage, 200, 200).Save(@"C:\demoScale.jpg");//根据源图片比例缩放为新图片 
    /// 
    /// var imgeList = ImageHelper.SplitImage(ImageHelper.LoadImage(multipleImagePath), 32, 32, true);//分割图片
    /// for (int index = 0; index &lt; imgeList.Length; index++)
    /// {
    ///     Image image = imgeList[index];
    ///     image.Save(string.Format("C:\\demoSplit{0}.jpg", index));
    /// } 
    /// </code>
    ///     <table border="0">
    ///         <tr>
    ///             <td align="center" valign="middle">
    ///                 <img src="images/ImageHelper/watermark.jpg" />
    ///             </td>
    ///         </tr>
    ///         <tr>
    ///             <td align="center" valign="middle">水印图片</td>
    ///         </tr>
    ///     </table>
    ///     <h1>水印</h1>
    ///     <table border="0">
    ///         <tr>
    ///             <td align="center" valign="middle">
    ///                 <img src="images/ImageHelper/demo.jpg" width="320" height="480" />
    ///             </td>
    ///             <td align="center" valign="middle">
    ///                 <img src="images/ImageHelper/demoImageWatermark.jpg" width="320" height="480" />
    ///             </td>
    ///             <td align="center" valign="middle">
    ///                 <img src="images/ImageHelper/demoTextWatermark.jpg" width="320" height="480" />
    ///             </td>
    ///             <td align="center" valign="middle">
    ///                 <img src="images/ImageHelper/demoThumbnail.jpg" />
    ///             </td>
    ///         </tr>
    ///         <tr>
    ///             <td align="center" valign="middle">原图</td>
    ///             <td align="center" valign="middle">图片水印</td>
    ///             <td align="center" valign="middle">文字水印</td>
    ///             <td align="center" valign="middle">缩略图</td>
    ///         </tr>
    ///     </table>
    ///     <h1>图片效果处理</h1>
    ///     <table width="388" border="0">
    ///         <tr>
    ///             <td align="center" valign="middle">
    ///                 <img src="images/ImageHelper/demo.jpg" width="160" height="240" />
    ///             </td>
    ///             <td align="center" valign="middle">
    ///                 <img src="images/ImageHelper/demoFanSe.jpg" width="160" height="240" />
    ///             </td>
    ///             <td align="center" valign="middle">
    ///                 <img src="images/ImageHelper/demoFuDiao.jpg" width="160" height="240" />
    ///             </td>
    ///             <td align="center" valign="middle">
    ///                 <img src="images/ImageHelper/demoHeiBai.jpg" width="160" height="240" />
    ///             </td>
    ///             <td align="center" valign="middle">
    ///                 <img src="images/ImageHelper/demoRouHua.jpg" width="160" height="240" />
    ///             </td>
    ///             <td align="center" valign="middle">
    ///                 <img src="images/ImageHelper/demoRuiHua.jpg" width="160" height="240" />
    ///             </td>
    ///             <td align="center" valign="middle">
    ///                 <img src="images/ImageHelper/demoWuHua.jpg" width="160" height="240" />
    ///             </td>
    ///         </tr>
    ///         <tr>
    ///             <td align="center" valign="middle">
    ///                 <strong>原图</strong>
    ///             </td>
    ///             <td align="center" valign="middle">反色</td>
    ///             <td align="center" valign="middle">浮雕</td>
    ///             <td align="center" valign="middle">黑白</td>
    ///             <td align="center" valign="middle">柔化</td>
    ///             <td align="center" valign="middle">锐化</td>
    ///             <td align="center" valign="middle">雾化</td>
    ///         </tr>
    ///     </table>
    ///     <h1>图片裁切</h1>
    ///     <table border="0">
    ///         <tr>
    ///             <td align="center" valign="middle">
    ///                 <img src="images/ImageHelper/demo.jpg" alt="原图" width="160" height="240" />
    ///             </td>
    ///             <td align="center" valign="middle">
    ///                 <img src="images/ImageHelper/demoCut.jpg" alt="裁切" />
    ///             </td>
    ///             <td align="center" valign="middle">
    ///                 <img src="images/ImageHelper/demoFill.jpg" alt="填充" />
    ///             </td>
    ///             <td align="center" valign="middle">
    ///                 <img src="images/ImageHelper/demoScale.jpg" alt="缩放" />
    ///             </td>
    ///         </tr>
    ///         <tr>
    ///             <td align="center" valign="middle">原图</td>
    ///             <td align="center" valign="middle">裁切</td>
    ///             <td align="center" valign="middle">填充</td>
    ///             <td align="center" valign="middle">缩放</td>
    ///         </tr>
    ///     </table>
    ///     <h1>图片分割</h1>
    ///     <table border="0">
    ///         <tr>
    ///             <td align="center" valign="middle">
    ///                 <img src="images/ImageHelper/multiplehorizontaldemo.png" />
    ///             </td>
    ///             <td align="center" valign="middle">
    ///                 <img src="images/ImageHelper/multipleverticaldemo.png" />
    ///             </td>
    ///         </tr>
    ///         <tr>
    ///             <td align="center" valign="middle">原图(水平)</td>
    ///             <td align="center" valign="middle">原图(垂直)</td>
    ///         </tr>
    ///     </table>
    ///     <h1>分割后图片</h1>
    ///     <table border="0">
    ///         <tr>
    ///             <td align="center" valign="middle">
    ///                 <img src="images/ImageHelper/demoSplit0.jpg" />
    ///             </td>
    ///             <td align="center" valign="middle">
    ///                 <img src="images/ImageHelper/demoSplit1.jpg" />
    ///             </td>
    ///             <td align="center" valign="middle">
    ///                 <img src="images/ImageHelper/demoSplit2.jpg" />
    ///             </td>
    ///             <td align="center" valign="middle">
    ///                 <img src="images/ImageHelper/demoSplit3.jpg" />
    ///             </td>
    ///             <td align="center" valign="middle">
    ///                 <img src="images/ImageHelper/demoSplit4.jpg" />
    ///             </td>
    ///             <td align="center" valign="middle">
    ///                 <img src="images/ImageHelper/demoSplit5.jpg" />
    ///             </td>
    ///             <td align="center" valign="middle">
    ///                 <img src="images/ImageHelper/demoSplit6.jpg" />
    ///             </td>
    ///         </tr>
    ///     </table>
    /// </example>
    public static class ImageHelper
    {
        /// <summary>
        /// 获取文件的默认图标,可以只是文件名，甚至只是文件的扩展名(.*)；
        /// 如果想获得.ICO文件所表示的图标，则必须是文件的完整路径。
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="largeIcon">是否大图标</param>
        public static Icon GetFileIcon(string fileName, bool largeIcon)
        {
            SHFILEINFO info = new SHFILEINFO();
            int cbFileInfo = Marshal.SizeOf(info); SHGFI flags;
            if (largeIcon)
            {
                flags = SHGFI.Icon | SHGFI.LargeIcon | SHGFI.UseFileAttributes;
            }
            else
            {
                flags = SHGFI.Icon | SHGFI.SmallIcon | SHGFI.UseFileAttributes;
            }
            NativeMethods.SHGetFileInfo(fileName, 256, out info, (uint)cbFileInfo, flags);
            return Icon.FromHandle(info.hIcon);
        }

        /// <summary>
        /// 获取icon图标
        /// </summary>
        /// <param name="path">路径,例如:%SystemRoot%\System32\imageres.dll,202</param>
        /// <param name="size">图标大小</param>
        public static Image GetExeImage(string path, int size)
        {
            if (string.IsNullOrEmpty(path))
            {
                return null;
            }
            IntPtr defaultIcon = (IntPtr)0;
            IntPtr iconid = (IntPtr)0;
            string[] defalutsz = path.Split(',');
            try
            {
                int iconIndex = 0;
                if (defalutsz.Length > 1)
                {
                    var index = defalutsz[1];
                    int.TryParse(index, out iconIndex);
                }
                NativeMethods.PrivateExtractIcons(defalutsz[0], iconIndex, size, size, ref defaultIcon,
                                                iconid, 1, 0);
                Icon icon = defaultIcon == IntPtr.Zero ? GetFileIcon(path, true) : Icon.FromHandle(defaultIcon);
                if (icon == null)
                {
                    return null;
                }
                Image img = icon.ToBitmap();
                NativeMethods.DestroyIcon(defaultIcon);
                return img;
            }
            catch
            {
                return null;
            }
        }

        #region 图片水印

        /// <summary>
        /// 加图片水印
        /// </summary>
        /// <param name="originalImage">图像(自动释放)</param>
        /// <param name="watermarkImage">水印文件</param>
        /// <param name="savePath">目标文件保存路径</param>
        /// <param name="watermarkPosition">图片水印位置 0=不使用 1=左上 2=中上 3=右上 4=左中  9=右下</param>
        /// <param name="quality">附加图像质量，1-100</param>
        /// <param name="watermarkTransparency">水印的透明度 1--10 10为不透明</param>
        public static void CreateImageWatermark(Image originalImage, Image watermarkImage, string savePath,
                                                int watermarkPosition, int quality, int watermarkTransparency)
        {
            if (originalImage == null) throw new ArgumentNullException("originalImage");
            if (watermarkImage == null) throw new ArgumentNullException("watermarkImage");
            if (savePath == null) throw new ArgumentNullException("savePath");
            Graphics g = Graphics.FromImage(originalImage);
            //设置高质量插值法
            g.InterpolationMode = InterpolationMode.High;
            //设置高质量,低速度呈现平滑程度
            g.SmoothingMode = SmoothingMode.HighQuality;
            Image watermark = new Bitmap(watermarkImage);

            if (watermark.Height >= originalImage.Height || watermark.Width >= originalImage.Width)
            {
                throw new Exception("水印图片的高度或者宽度不能超过原始图片的宽度或者高度");
            }

            ImageAttributes imageAttributes = new ImageAttributes();
            ColorMap colorMap = new ColorMap
                {
                    OldColor = Color.FromArgb(255, 0, 255, 0),
                    NewColor = Color.FromArgb(0, 0, 0, 0)
                };

            ColorMap[] remapTable = {colorMap};

            imageAttributes.SetRemapTable(remapTable, ColorAdjustType.Bitmap);

            float transparency = 0.5F;
            if (watermarkTransparency >= 1 && watermarkTransparency <= 10)
            {
                transparency = (watermarkTransparency/10.0F);
            }

            float[][] colorMatrixElements =
                {
                    new[] {1.0f, 0.0f, 0.0f, 0.0f, 0.0f},
                    new[] {0.0f, 1.0f, 0.0f, 0.0f, 0.0f},
                    new[] {0.0f, 0.0f, 1.0f, 0.0f, 0.0f},
                    new[] {0.0f, 0.0f, 0.0f, transparency, 0.0f},
                    new[] {0.0f, 0.0f, 0.0f, 0.0f, 1.0f}
                };

            ColorMatrix colorMatrix = new ColorMatrix(colorMatrixElements);

            imageAttributes.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

            int xpos = 0;
            int ypos = 0;

            switch (watermarkPosition)
            {
                case 1:
                    xpos = (int) (originalImage.Width*(float) .01);
                    ypos = (int) (originalImage.Height*(float) .01);
                    break;
                case 2:
                    xpos = (int) ((originalImage.Width*(float) .50) - ((float) watermark.Width/2));
                    ypos = (int) (originalImage.Height*(float) .01);
                    break;
                case 3:
                    xpos = (int) ((originalImage.Width*(float) .99) - (watermark.Width));
                    ypos = (int) (originalImage.Height*(float) .01);
                    break;
                case 4:
                    xpos = (int) (originalImage.Width*(float) .01);
                    ypos = (int) ((originalImage.Height*(float) .50) - ((float) watermark.Height/2));
                    break;
                case 5:
                    xpos = (int) ((originalImage.Width*(float) .50) - ((float) watermark.Width/2));
                    ypos = (int) ((originalImage.Height*(float) .50) - ((float) watermark.Height/2));
                    break;
                case 6:
                    xpos = (int) ((originalImage.Width*(float) .99) - (watermark.Width));
                    ypos = (int) ((originalImage.Height*(float) .50) - ((float) watermark.Height/2));
                    break;
                case 7:
                    xpos = (int) (originalImage.Width*(float) .01);
                    ypos = (int) ((originalImage.Height*(float) .99) - watermark.Height);
                    break;
                case 8:
                    xpos = (int) ((originalImage.Width*(float) .50) - ((float) watermark.Width/2));
                    ypos = (int) ((originalImage.Height*(float) .99) - watermark.Height);
                    break;
                case 9:
                    xpos = (int) ((originalImage.Width*(float) .99) - (watermark.Width));
                    ypos = (int) ((originalImage.Height*(float) .99) - watermark.Height);
                    break;
            }

            g.DrawImage(watermark, new Rectangle(xpos, ypos, watermark.Width, watermark.Height), 0, 0, watermark.Width,
                        watermark.Height, GraphicsUnit.Pixel, imageAttributes);
            //g.DrawImage(watermark, new System.Drawing.Rectangle(xpos, ypos, watermark.Width, watermark.Height), 0, 0, watermark.Width, watermark.Height, System.Drawing.GraphicsUnit.Pixel);

            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
            ImageCodecInfo ici = null;
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.MimeType.IndexOf("jpeg", StringComparison.Ordinal) > -1)
                {
                    ici = codec;
                }
            }
            EncoderParameters encoderParams = new EncoderParameters();
            long[] qualityParam = new long[1];
            if (quality < 0 || quality > 100)
            {
                quality = 100;
            }
            qualityParam[0] = quality;

            EncoderParameter encoderParam = new EncoderParameter(Encoder.Quality, qualityParam);
            encoderParams.Param[0] = encoderParam;

            if (ici != null)
            {
                originalImage.Save(savePath, ici, encoderParams);
            }
            else
            {
                originalImage.Save(savePath);
            }

            g.Dispose();
            originalImage.Dispose();
            watermark.Dispose();
            imageAttributes.Dispose();
        }


        /// <summary>
        /// 增加文字水印
        /// </summary>
        /// <param name="originalImage">图像(自动释放)</param>
        /// <param name="watermarkText">水印文字</param>
        /// <param name="savePath">目标文件保存路径</param>
        /// <param name="watermarkPosition">图片水印位置 0=不使用 1=左上 2=中上 3=右上 4=左中  9=右下</param>
        /// <param name="quality">附加图像质量质量，1-100</param>
        /// <param name="font">字体</param>
        public static void CreateTextWatermark(Image originalImage, string watermarkText, string savePath,
                                               int watermarkPosition, int quality, Font font)
        {
            //System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(image);
            //    .FromFile(filename);
            Graphics g = Graphics.FromImage(originalImage);
            SizeF crSize = g.MeasureString(watermarkText, font);

            float xpos = 0;
            float ypos = 0;

            switch (watermarkPosition)
            {
                case 1:
                    xpos = originalImage.Width*(float) .01;
                    ypos = originalImage.Height*(float) .01;
                    break;
                case 2:
                    xpos = (originalImage.Width*(float) .50) - (crSize.Width/2);
                    ypos = originalImage.Height*(float) .01;
                    break;
                case 3:
                    xpos = (originalImage.Width*(float) .99) - crSize.Width;
                    ypos = originalImage.Height*(float) .01;
                    break;
                case 4:
                    xpos = originalImage.Width*(float) .01;
                    ypos = (originalImage.Height*(float) .50) - (crSize.Height/2);
                    break;
                case 5:
                    xpos = (originalImage.Width*(float) .50) - (crSize.Width/2);
                    ypos = (originalImage.Height*(float) .50) - (crSize.Height/2);
                    break;
                case 6:
                    xpos = (originalImage.Width*(float) .99) - crSize.Width;
                    ypos = (originalImage.Height*(float) .50) - (crSize.Height/2);
                    break;
                case 7:
                    xpos = originalImage.Width*(float) .01;
                    ypos = (originalImage.Height*(float) .99) - crSize.Height;
                    break;
                case 8:
                    xpos = (originalImage.Width*(float) .50) - (crSize.Width/2);
                    ypos = (originalImage.Height*(float) .99) - crSize.Height;
                    break;
                case 9:
                    xpos = (originalImage.Width*(float) .99) - crSize.Width;
                    ypos = (originalImage.Height*(float) .99) - crSize.Height;
                    break;
            }

            //            System.Drawing.StringFormat StrFormat = new System.Drawing.StringFormat();
            //            StrFormat.Alignment = System.Drawing.StringAlignment.Center;
            //
            //            g.DrawString(watermarkText, drawFont, new System.Drawing.SolidBrush(System.Drawing.Color.White), xpos + 1, ypos + 1, StrFormat);
            //            g.DrawString(watermarkText, drawFont, new System.Drawing.SolidBrush(System.Drawing.Color.Black), xpos, ypos, StrFormat);
            var whiteBrush =new SolidBrush(Color.White);
            var blackBrush =new SolidBrush(Color.Black);
            g.DrawString(watermarkText, font, whiteBrush, xpos + 1, ypos + 1);
            g.DrawString(watermarkText, font, blackBrush, xpos, ypos);

            whiteBrush.Dispose();
            blackBrush.Dispose();

            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
            ImageCodecInfo ici = null;
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.MimeType.IndexOf("jpeg", StringComparison.Ordinal) > -1)
                {
                    ici = codec;
                }
            }
            EncoderParameters encoderParams = new EncoderParameters();
            long[] qualityParam = new long[1];
            if (quality < 0 || quality > 100)
            {
                quality = 100;
            }
            qualityParam[0] = quality;

            EncoderParameter encoderParam = new EncoderParameter(Encoder.Quality, qualityParam);
            encoderParams.Param[0] = encoderParam;

            if (ici != null)
            {
                originalImage.Save(savePath, ici, encoderParams);
            }
            else
            {
                originalImage.Save(savePath);
            }
            originalImage.Save(savePath);
            g.Dispose();
            //bmp.Dispose();
            originalImage.Dispose();
        }

        #endregion

        #region 图片序列化

        /// <summary>
        /// 把图片进行序列化
        /// </summary>
        /// <param name="imagePath">图片路径</param>
        /// <param name="serializePath">序列化路径</param>
        public static void SerializeImage(string imagePath, string serializePath)
        {
            if (File.Exists(serializePath))
            {
                File.Delete(serializePath);
            }
            Stream targetImageStream = new FileStream(serializePath, FileMode.Create, FileAccess.Write, FileShare.None);
            Image originalImage = Image.FromFile(imagePath);
            FileHelper.Serialize(targetImageStream, originalImage);
            originalImage.Dispose();
            targetImageStream.Close();
            //targetImageStream.Dispose();
        }

        /// <summary>
        /// 把图片进行反序列化
        /// </summary>
        /// <param name="serializePath">序列化路径</param>
        /// <returns>返回反序列化后的图片</returns>
        public static Image DeserializeImage(string serializePath)
        {
            Stream stream = new FileStream(serializePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            Image img = (Image)FileHelper.Deserialize(stream);
            stream.Close();
            return img;
        }

        #endregion

        #region 图片特效

        /// <summary>
        /// 以反色方式显示图像
        /// </summary>
        /// <param name="image">源图像</param>
        /// <returns>反色处理后的图像</returns>
        public static Bitmap FanSe(Image image)
        {
            int height = image.Height;
            int width = image.Width;
            Bitmap bitmap = new Bitmap(width, height);
            Bitmap myBitmap = (Bitmap) image;
            for (int x = 1; x < width; x++)
            {
                for (int y = 1; y < height; y++)
                {
                    Color pixel = myBitmap.GetPixel(x, y);
                    int r = 255 - pixel.R;
                    int g = 255 - pixel.G;
                    int b = 255 - pixel.B;
                    bitmap.SetPixel(x, y, Color.FromArgb(r, g, b));
                }
            }
            return bitmap;
        }

        /// <summary>
        /// 以浮雕方式显示图像
        /// </summary>
        /// <param name="image">源图像</param>
        /// <returns>浮雕效果处理后的图像</returns>
        public static Bitmap FuDiao(Image image)
        {
            int height = image.Height;
            int width = image.Width;
            Bitmap bitmap = new Bitmap(width, height);
            Bitmap myBitmap = (Bitmap) image;
            for (int x = 0; x < width - 1; x++)
            {
                for (int y = 0; y < height - 1; y++)
                {
                    Color pixel1 = myBitmap.GetPixel(x, y);
                    Color pixel2 = myBitmap.GetPixel(x + 1, y + 1);
                    int r = pixel1.R - pixel2.R + 128;
                    int g = pixel1.G - pixel2.G + 128;
                    int b = pixel1.B - pixel2.B + 128;
                    if (r > 255)
                        r = 255;
                    if (r < 0)
                        r = 0;
                    if (g > 255)
                        g = 255;
                    if (g < 0)
                        g = 0;
                    if (b > 255)
                        b = 255;
                    if (b < 0)
                        b = 0;
                    bitmap.SetPixel(x, y, Color.FromArgb(r, g, b));
                }
            }
            return bitmap;
        }

        /// <summary>
        /// 以黑白方式显示图像
        /// </summary>
        /// <param name="image">源图像</param>
        /// <param name="iType">黑白处理的方法参数,0-平均值法;1-最大值法;2-加权平均值法</param>
        /// <returns>黑白效果处理后的图像</returns>
        public static Bitmap HeiBai(Image image, int iType)
        {
            int height = image.Height;
            int width = image.Width;
            Bitmap bitmap = new Bitmap(width, height);
            Bitmap myBitmap = (Bitmap) image;
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Color pixel = myBitmap.GetPixel(x, y);
                    int result = 0;
                    int r = pixel.R;
                    int g = pixel.G;
                    int b = pixel.B;

                    switch (iType)
                    {
                        case 0://平均值法
                            result = ((r + g + b)/3);
                            break;
                        case 1://最大值法
                            result = r > g ? r : g;
                            result = result > b ? result : b;
                            break;
                        case 2://加权平均值法
                            result = ((int) (0.7*r) + (int) (0.2*g) + (int) (0.1*b));
                            break;
                    }
                    bitmap.SetPixel(x, y, Color.FromArgb(result, result, result));
                }
            }
            return bitmap;
        }

        /// <summary>
        /// 以柔化方式显示图像 高斯模板法
        /// </summary>
        /// <param name="image">源图像</param>
        /// <returns>柔化处理后的图像</returns>
        public static Bitmap RouHua(Image image)
        {
            int height = image.Height;
            int width = image.Width;
            Bitmap bitmap = new Bitmap(width, height);
            Bitmap myBitmap = (Bitmap) image;
            //高斯模板
            int[] gauss = {1, 2, 1, 2, 4, 2, 1, 2, 1};
            for (int x = 1; x < width - 1; x++)
            {
                for (int y = 1; y < height - 1; y++)
                {
                    int r = 0, g = 0, b = 0;
                    int index = 0;
                    //int a=0;
                    for (int col = -1; col <= 1; col++)
                        for (int row = -1; row <= 1; row++)
                        {
                            Color pixel = myBitmap.GetPixel(x + row, y + col);
                            r += pixel.R*gauss[index];
                            g += pixel.G*gauss[index];
                            b += pixel.B*gauss[index];
                            index++;
                        }
                    r /= 16;
                    g /= 16;
                    b /= 16;
                    //处理颜色值溢出
                    r = r > 255 ? 255 : r;
                    r = r < 0 ? 0 : r;
                    g = g > 255 ? 255 : g;
                    g = g < 0 ? 0 : g;
                    b = b > 255 ? 255 : b;
                    b = b < 0 ? 0 : b;
                    bitmap.SetPixel(x - 1, y - 1, Color.FromArgb(r, g, b));
                }
            }
            return bitmap;
        }

        /// <summary>
        /// 以锐化方式显示图像 拉普拉斯模板法
        /// </summary>
        /// <param name="image">源图像</param>
        /// <returns>锐化处理后的图像</returns>
        public static Bitmap RuiHua(Image image)
        {
            int height = image.Height;
            int width = image.Width;
            Bitmap bitmap = new Bitmap(width, height);
            Bitmap myBitmap = (Bitmap) image;
            //拉普拉斯模板
            int[] laplacian = {-1, -1, -1, -1, 9, -1, -1, -1, -1};
            for (int x = 1; x < width - 1; x++)
            {
                for (int y = 1; y < height - 1; y++)
                {
                    int r = 0, g = 0, b = 0;
                    int index = 0;
                    //int a = 0;
                    for (int col = -1; col <= 1; col++)
                        for (int row = -1; row <= 1; row++)
                        {
                            Color pixel = myBitmap.GetPixel(x + row, y + col);
                            r += pixel.R*laplacian[index];
                            g += pixel.G*laplacian[index];
                            b += pixel.B*laplacian[index];
                            index++;
                        }
                    //处理颜色值溢出
                    r = r > 255 ? 255 : r;
                    r = r < 0 ? 0 : r;
                    g = g > 255 ? 255 : g;
                    g = g < 0 ? 0 : g;
                    b = b > 255 ? 255 : b;
                    b = b < 0 ? 0 : b;
                    bitmap.SetPixel(x - 1, y - 1, Color.FromArgb(r, g, b));
                }
            }
            return bitmap;
        }

        /// <summary>
        /// 以雾化方式显示图像
        /// </summary>
        /// <param name="image">源图像</param>
        /// <returns>雾化处理后的图像</returns>
        public static Bitmap WuHua(Image image)
        {
            int height = image.Height;
            int width = image.Width;
            Bitmap bitmap = new Bitmap(width, height);
            Bitmap myBitmap = (Bitmap) image;
            for (int x = 1; x < width - 1; x++)
            {
                for (int y = 1; y < height - 1; y++)
                {
                    Random myRandom = new Random();
                    int k = myRandom.Next(123456);
                    //像素块大小
                    int dx = x + k%19;
                    int dy = y + k%19;
                    if (dx >= width)
                        dx = width - 1;
                    if (dy >= height)
                        dy = height - 1;
                    Color pixel = myBitmap.GetPixel(dx, dy);
                    bitmap.SetPixel(x, y, pixel);
                }
            }
            return bitmap;
        }

        #endregion

        #region 创建图片

        /// <summary>
        /// 根据源图片裁切为新图片
        /// </summary>
        /// <param name="orgImage">源图片</param>
        /// <param name="width">宽度</param>
        /// <param name="height">高度</param>
        /// <returns>返回新的图片</returns>
        public static Image CreateNewImageCut(Image orgImage, int width, int height)
        {
            float num = width/((float) orgImage.Width);
            if ((orgImage.Height*num) < height)
            {
                num = height/(float) orgImage.Height;
            }
            float num4 = orgImage.Width*num;
            float num5 = orgImage.Height*num;
            float x = (width - num4)/2f;
            float y = (height - num5)/2f;
            Bitmap image = new Bitmap(width, height);
            Graphics graphics = Graphics.FromImage(image);
            graphics.CompositingMode = CompositingMode.SourceOver;
            graphics.CompositingQuality = CompositingQuality.HighQuality;
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            graphics.DrawImage(orgImage, x, y, num4, num5);
            graphics.Save();
            graphics.Dispose();
            return image;
        }

        /// <summary>
        /// 根据源图片填充为新图片
        /// </summary>
        /// <param name="orgImage">源图片</param>
        /// <param name="width">宽度</param>
        /// <param name="height">高度</param>
        /// <param name="fillColor">填充色</param>
        /// <returns>返回新的图片</returns>
        public static Image CreateNewImageFill(Image orgImage, int width, int height, Color fillColor)
        {
            float num = width/((float) orgImage.Width);
            if ((orgImage.Height*num) > height)
            {
                num = height/((float) orgImage.Height);
            }
            float num4 = orgImage.Width*num;
            float num5 = orgImage.Height*num;
            float x = (width - num4)/2f;
            float y = (height - num5)/2f;
            Bitmap image = new Bitmap(width, height);
            Graphics graphics = Graphics.FromImage(image);
            graphics.CompositingMode = CompositingMode.SourceOver;
            graphics.CompositingQuality = CompositingQuality.HighQuality;
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            graphics.Clear(fillColor);
            graphics.DrawImage(orgImage, x, y, num4, num5);
            graphics.Save();
            graphics.Dispose();
            return image;
        }

        /// <summary>
        /// 根据源图片比例缩放为新图片
        /// </summary>
        /// <param name="orgImage">源图片</param>
        /// <param name="width">宽度</param>
        /// <param name="height">高度</param>
        /// <returns>返回新的图片</returns>
        public static Image CreateNewImageScale(Image orgImage, int width, int height)
        {
            Bitmap image = new Bitmap(width, height);
            Graphics graphics = Graphics.FromImage(image);
            graphics.CompositingMode = CompositingMode.SourceOver;
            graphics.CompositingQuality = CompositingQuality.HighQuality;
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            graphics.DrawImage(orgImage, 0, 0, width, height);
            graphics.Save();
            graphics.Dispose();
            return image;
        }

        /// <summary>
        /// 创建图片缩略图
        /// </summary>
        /// <param name="originalImage">图片(自动释放)</param>
        /// <param name="targetWidth">缩略图宽度</param>
        /// <param name="targetHeight">缩略图高度</param>
        /// <returns>返回指定尺寸的缩略图</returns>
        public static Image CreateThumbnail(Image originalImage, int targetWidth, int targetHeight)
        {
            return CreateThumbnail(originalImage, targetWidth, targetHeight, Color.Transparent);
        }

        /// <summary>
        /// 创建图片缩略图
        /// </summary>
        /// <param name="originalImage">图片(自动释放)</param>
        /// <param name="targetWidth">缩略图宽度</param>
        /// <param name="targetHeight">缩略图高度</param>
        /// <param name="backgroundColor">背景色</param>
        /// <returns>返回指定尺寸的缩略图</returns>
        public static Image CreateThumbnail(Image originalImage, int targetWidth, int targetHeight,
                                            Color backgroundColor)
        {
            int width = originalImage.Width;
            int height = originalImage.Height;
            int newWidth, newHeight;

            float targetRatio = targetWidth/(float) targetHeight;
            float imageRatio = width/(float) height;

            if (targetRatio > imageRatio)
            {
                newHeight = targetHeight;
                newWidth = (int) Math.Floor(imageRatio*targetHeight);
            }
            else
            {
                newHeight = (int) Math.Floor(targetWidth/imageRatio);
                newWidth = targetWidth;
            }

            newWidth = newWidth > targetWidth ? targetWidth : newWidth;
            newHeight = newHeight > targetHeight ? targetHeight : newHeight;

            //Image thumbnailImage = originalImage.GetThumbnailImage(newWidth, newHeight, null, IntPtr.Zero);

            Bitmap finalImage = new Bitmap(targetWidth, targetHeight);
            Graphics graphic = Graphics.FromImage(finalImage);
            graphic.SmoothingMode = SmoothingMode.HighQuality;
            graphic.CompositingQuality = CompositingQuality.HighQuality;
            graphic.InterpolationMode = InterpolationMode.High;
            graphic.Clear(backgroundColor);//清空画布并以透明背景色填充    
            //graphic.FillRectangle(new SolidBrush(Color.White), new Rectangle(0, 0, targetWidth, targetHeight));
            int pasteX = (targetWidth - newWidth)/2;
            int pasteY = (targetHeight - newHeight)/2;
            graphic.DrawImage(originalImage, pasteX, pasteY, newWidth, newHeight);

            graphic.Dispose();
            originalImage.Dispose();

            return finalImage;
        }

        /// <summary>
        /// 从磁盘加载图片到内存 自动释放文件
        /// </summary>
        /// <param name="path">图片路径</param>
        /// <returns>返回图片对象</returns>
        public static Image LoadImage(string path)
        {
            if (File.Exists(path))
            {
                Image img = Image.FromFile(path);
                Image bmp = new Bitmap(img);
                img.Dispose();
                return bmp;
            }
            return null;
        }

        ///// <summary>
        ///// 把图片保存为Jpeg格式
        ///// </summary>
        ///// <param name="image">源图片</param>
        ///// <param name="serializePath">保存路径</param>
        ///// <param name="quality">品质</param>
        //private static void SaveJpegQualityCodecsInfo(Image image, string serializePath, int quality)
        //{
        //    EncoderParameters encoderParams = new EncoderParameters(1);
        //    encoderParams.Param[0] = new EncoderParameter(Encoder.Quality, quality);
        //    foreach (ImageCodecInfo info in ImageCodecInfo.GetImageEncoders())
        //    {
        //        if (info.MimeType == "image/jpeg")
        //        {
        //            image.Save(serializePath, info, encoderParams);
        //            return;
        //        }
        //    }
        //}

        /// <summary>
        /// 分割图片
        /// </summary>
        /// <param name="path">源图片路径</param>
        /// <param name="width">宽度</param>
        /// <param name="height">高度</param>
        /// <param name="isHorizontal">分割方式。true水平分割,false垂直分割</param>
        /// <returns>返回分割后的图片数组</returns>
        public static Image[] SplitImage(string path, int width, int height, bool isHorizontal)
        {
            return SplitImage(LoadImage(path), width, height, isHorizontal);
        }

        /// <summary>
        /// 分割图片
        /// </summary>
        /// <param name="image">源图片</param>
        /// <param name="width">宽度</param>
        /// <param name="height">高度</param>
        /// <param name="isHorizontal">分割方式。true水平分割,false垂直分割</param>
        /// <exception cref="System.ArgumentNullException">源图片能为空</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">指定的宽度大于源图片宽度</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">指定的高度大于源图片高度</exception>
        /// <returns>返回分割后的图片数组</returns>
        public static Image[] SplitImage(Image image, int width, int height, bool isHorizontal)
        {
            if (image == null)
            {
                throw new ArgumentNullException("image", "源图片能为空");
            }
            if (image.Width < width)
            {
                throw new ArgumentOutOfRangeException("width", "指定的宽度大于源图片宽度");
            }
            if (image.Height < height)
            {
                throw new ArgumentOutOfRangeException("height", "指定的高度大于源图片高度");
            }
            List<Image> images = new List<Image>();
            int x = 0;
            int y = 0;
            do
            {
                if (isHorizontal && x >= image.Width)
                {
                    break;
                }
                if (y >= image.Height)
                {
                    break;
                }

                Image bitmap = new Bitmap(width, height);
                Graphics graphic = Graphics.FromImage(bitmap);
                graphic.DrawImage(image, 0, 0, new RectangleF(x, y, width, height), GraphicsUnit.Pixel);
                graphic.Flush();
                graphic.Dispose();
                images.Add(bitmap);
                if (isHorizontal)
                {
                    x += width;
                }
                else
                {
                    y += height;
                }
            } while (true);
            return images.ToArray();
        }

        #endregion

        #region 图片 字节数组 转换

        private static readonly object ToArrayLockObject = new object();
        private static readonly object SaveImageToStreamObject = new object();

        /// <summary>
        /// 把图片转为字节数组
        /// </summary>
        /// <param name="image">图片</param>
        /// <returns>返回图片的字节数组</returns>
        public static byte[] ToArray(Image image)
        {
            if (image == null)
            {
                return new byte[0];
            }
            lock (ToArrayLockObject)
            {
                return ToArray(image, image.RawFormat);
            }
        }

        /// <summary>
        /// 把图片转为字节数组
        /// </summary>
        /// <param name="image">图片</param>
        /// <param name="format">格式</param>
        /// <returns>返回图片的字节数组</returns>
        public static byte[] ToArray(Image image, ImageFormat format)
        {
            return ToArrayCore(image, format);
        }

        /// <summary>
        /// 把图片转为字节数组
        /// </summary>
        /// <param name="image">图片</param>
        /// <param name="format">格式</param>
        /// <returns>返回图片的字节数组</returns>
        private static byte[] ToArrayCore(Image image, ImageFormat format)
        {
            MemoryStream stream = new MemoryStream();
            try
            {
                SaveImageToStream(image, stream, format);
                return stream.ToArray();
            }
            catch
            {
                return new byte[] {};
            }
            finally
            {
                stream.Close();
                //((IDisposable) stream).Dispose();
            }
        }

        /// <summary>
        /// 把图像保存到指定的流中
        /// </summary>
        /// <param name="image">图片</param>
        /// <param name="stream">流</param>
        /// <param name="format">格式</param>
        private static void SaveImageToStream(Image image, Stream stream, ImageFormat format)
        {
            ImageCodecInfo info = FindEncoder(format) ?? FindEncoder(ImageFormat.Png);
            lock (SaveImageToStreamObject)
            {
                image.Save(stream, info, null);
            }
        }

        /// <summary>
        /// 把字节数组转为图片
        /// </summary>
        /// <param name="buffer">字节数字</param>
        /// <returns>返回图片对象</returns>
        public static Image FromArray(byte[] buffer)
        {
            if (buffer == null)
                return null;
            Image img = null;

            if (buffer.Length > 78)
            {
                if (buffer[0] == 0x15 && buffer[1] == 0x1c)
                    img = FromArrayCore(buffer, 78);
            }
            return img ?? (FromArrayCore(buffer, 0));
        }

        /// <summary>
        /// 把字节数组转为图片
        /// </summary>
        /// <param name="buffer">字节数字</param>
        /// <param name="offset">开始位置</param>
        /// <returns>返回图片对象</returns>
        private static Image FromArrayCore(byte[] buffer, int offset)
        {
            if (buffer == null || buffer.Length==0)
            {
                return null;
            }
            try
            {
                MemoryStream stream = new MemoryStream(buffer, offset, buffer.Length - offset);
                return Image.FromStream(stream);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 获取图像解码器
        /// </summary>
        /// <param name="format">图片格式</param>
        /// <returns>返回图像解码器</returns>
        private static ImageCodecInfo FindEncoder(ImageFormat format)
        {
            ImageCodecInfo[] infos = ImageCodecInfo.GetImageEncoders();
            foreach (ImageCodecInfo t in infos)
            {
                if (t.FormatID.Equals(format.Guid))
                {
                    return t;
                }
            }
            return null;
        }

        #endregion
    }
}