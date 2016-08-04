// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================
using System;
using System.Drawing;

namespace DotNet.Helper
{
    /// <summary>
    /// 生成验证码图片
    /// </summary>
    public class ValidateCodeDrawHelper
    {
        private Color _backgroundColor = Color.White;
        private bool _chaos = true;
        private Color _chaosColor = Color.LightGray;
        private Color[] _colors = { Color.Black, Color.Red, Color.DarkBlue, Color.Green, Color.Orange, Color.Brown, Color.DarkCyan, Color.Purple };
        private string[] _fonts = { "新宋体", "Arial", "Georgia" };
        private const double PI = 3.1415926535897932384626433832795;
        private const double PI2 = 6.283185307179586476925286766559;
        private int _fontSize = 14;
        private int _padding = 4;

        /// <summary>
        /// 验证码字体大小(为了显示扭曲效果，默认14像素，可以自行修改)
        /// </summary>
        public int FontSize
        {
            get { return _fontSize; }
            set { _fontSize = value; }
        }

        /// <summary>
        /// 边框补(默认4像素)
        /// </summary>
        public int Padding
        {
            get { return _padding; }
            set { _padding = value; }
        }

        /// <summary>
        /// 是否输出燥点(默认输出)
        /// </summary>
        public bool Chaos
        {
            get { return _chaos; }
            set { _chaos = value; }
        }

        /// <summary>
        /// 输出燥点的颜色(默认灰色)
        /// </summary>
        public Color ChaosColor
        {
            get { return _chaosColor; }
            set { _chaosColor = value; }
        }

        /// <summary>
        /// 自定义背景色(默认白色)
        /// </summary>
        public Color BackgroundColor
        {
            get { return _backgroundColor; }
            set { _backgroundColor = value; }
        }

        /// <summary>
        /// 自定义随机颜色数组
        /// </summary>
        public Color[] Colors
        {
            get { return _colors; }
            set { _colors = value; }
        }

        /// <summary>
        /// 自定义字体数组
        /// </summary>
        public string[] Fonts
        {
            get { return _fonts; }
            set { _fonts = value; }
        }

        /// <summary>
        /// 正弦曲线Wave扭曲图片
        /// </summary>
        /// <param name="srcBmp">图片路径</param>
        /// <param name="bXDir">如果扭曲则选择为True</param>
        /// <param name="dMultValue">波形的幅度倍数，越大扭曲的程度越高，一般为3</param>
        /// <param name="dPhase">波形的起始相位，取值区间[0-2*PI)</param>
        /// <returns></returns>
        private Bitmap TwistImage(Bitmap srcBmp, bool bXDir, double dMultValue, double dPhase)
        {
            System.Drawing.Bitmap destBmp = new Bitmap(srcBmp.Width, srcBmp.Height);

            // 将位图背景填充为白色
            System.Drawing.Graphics graph = System.Drawing.Graphics.FromImage(destBmp);
            graph.FillRectangle(new SolidBrush(System.Drawing.Color.White), 0, 0, destBmp.Width, destBmp.Height);
            graph.Dispose();

            double dBaseAxisLen = bXDir ? (double)destBmp.Height : (double)destBmp.Width;

            for (int i = 0; i < destBmp.Width; i++)
            {
                for (int j = 0; j < destBmp.Height; j++)
                {
                    double dx = 0;
                    dx = bXDir ? (PI2 * (double)j) / dBaseAxisLen : (PI2 * (double)i) / dBaseAxisLen;
                    dx += dPhase;
                    double dy = Math.Sin(dx);

                    // 取得当前点的颜色
                    int nOldX = 0, nOldY = 0;
                    nOldX = bXDir ? i + (int)(dy * dMultValue) : i;
                    nOldY = bXDir ? j : j + (int)(dy * dMultValue);

                    System.Drawing.Color color = srcBmp.GetPixel(i, j);
                    if (nOldX >= 0 && nOldX < destBmp.Width
                     && nOldY >= 0 && nOldY < destBmp.Height)
                    {
                        destBmp.SetPixel(nOldX, nOldY, color);
                    }
                }
            }

            return destBmp;
        }

        /// <summary>
        /// 生成验证码图片
        /// </summary>
        /// <param name="code">验证码</param>
        /// <returns>返回验证码图片</returns>
        public Bitmap CreateImage(string code)
        {
            var rnd = new Random(unchecked((int)DateTime.Now.Ticks));
            int fSize = FontSize;
            int fWidth = fSize + Padding;

            int imageWidth = code.Length * fWidth + 4 + Padding * 2;
            int imageHeight = fSize * 2 + Padding * 2;

            System.Drawing.Bitmap image = new System.Drawing.Bitmap(imageWidth, imageHeight);

            Graphics g = Graphics.FromImage(image);

            g.Clear(BackgroundColor);

            //给背景添加随机生成的燥点
            if (this.Chaos)
            {
                Pen pen = new Pen(ChaosColor, 0);
                int c = code.Length * 10;

                for (int i = 0; i < c; i++)
                {
                    int x = rnd.Next(image.Width);
                    int y = rnd.Next(image.Height);

                    g.DrawRectangle(pen, x, y, 1, 1);
                }
                pen.Dispose();
            }

            int top1 = 1, top2 = 1, top3 = 1, top4 = 1;

            int n1 = (imageHeight - FontSize - Padding * 2);
            int n2 = n1 / 2;
            int n3 = n1 / 4;
            top1 = n2;
            top2 = n2 * 2;
            top3 = n3;
            top4 = n3 * 2;

            int[] tops = {top1, top2, top3, top4};

            //随机字体和颜色的验证码字符
            for (int i = 0; i < code.Length; i++)
            {
                int cindex = rnd.Next(Colors.Length - 1);
                int findex = rnd.Next(Fonts.Length - 1);
                int tindex = rnd.Next(tops.Length - 1);

                Font f = new System.Drawing.Font(Fonts[findex], fSize, System.Drawing.FontStyle.Bold);
                Brush b = new System.Drawing.SolidBrush(Colors[cindex]);

                int top = tops[tindex];
                //top = i % 2 == 1 ? top2 : top1;

                int left = i * fWidth;

                g.DrawString(code.Substring(i, 1), f, b, left, top);
            }

            //画一个边框 边框颜色为Color.Gainsboro
            var gpen = new Pen(Color.Gainsboro, 0);
            g.DrawRectangle(gpen, 0, 0, image.Width - 1, image.Height - 1);
            g.Dispose();
            gpen.Dispose();
            //产生波形
            image = TwistImage(image, false, 8, 4);

            return image;
        }
    }
}