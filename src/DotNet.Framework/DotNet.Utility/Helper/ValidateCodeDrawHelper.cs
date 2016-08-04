// ===============================================================================
// DotNet.Platform ������� 2016 ��Ȩ����
// ===============================================================================
using System;
using System.Drawing;

namespace DotNet.Helper
{
    /// <summary>
    /// ������֤��ͼƬ
    /// </summary>
    public class ValidateCodeDrawHelper
    {
        private Color _backgroundColor = Color.White;
        private bool _chaos = true;
        private Color _chaosColor = Color.LightGray;
        private Color[] _colors = { Color.Black, Color.Red, Color.DarkBlue, Color.Green, Color.Orange, Color.Brown, Color.DarkCyan, Color.Purple };
        private string[] _fonts = { "������", "Arial", "Georgia" };
        private const double PI = 3.1415926535897932384626433832795;
        private const double PI2 = 6.283185307179586476925286766559;
        private int _fontSize = 14;
        private int _padding = 4;

        /// <summary>
        /// ��֤�������С(Ϊ����ʾŤ��Ч����Ĭ��14���أ����������޸�)
        /// </summary>
        public int FontSize
        {
            get { return _fontSize; }
            set { _fontSize = value; }
        }

        /// <summary>
        /// �߿�(Ĭ��4����)
        /// </summary>
        public int Padding
        {
            get { return _padding; }
            set { _padding = value; }
        }

        /// <summary>
        /// �Ƿ�������(Ĭ�����)
        /// </summary>
        public bool Chaos
        {
            get { return _chaos; }
            set { _chaos = value; }
        }

        /// <summary>
        /// ���������ɫ(Ĭ�ϻ�ɫ)
        /// </summary>
        public Color ChaosColor
        {
            get { return _chaosColor; }
            set { _chaosColor = value; }
        }

        /// <summary>
        /// �Զ��屳��ɫ(Ĭ�ϰ�ɫ)
        /// </summary>
        public Color BackgroundColor
        {
            get { return _backgroundColor; }
            set { _backgroundColor = value; }
        }

        /// <summary>
        /// �Զ��������ɫ����
        /// </summary>
        public Color[] Colors
        {
            get { return _colors; }
            set { _colors = value; }
        }

        /// <summary>
        /// �Զ�����������
        /// </summary>
        public string[] Fonts
        {
            get { return _fonts; }
            set { _fonts = value; }
        }

        /// <summary>
        /// ��������WaveŤ��ͼƬ
        /// </summary>
        /// <param name="srcBmp">ͼƬ·��</param>
        /// <param name="bXDir">���Ť����ѡ��ΪTrue</param>
        /// <param name="dMultValue">���εķ��ȱ�����Խ��Ť���ĳ̶�Խ�ߣ�һ��Ϊ3</param>
        /// <param name="dPhase">���ε���ʼ��λ��ȡֵ����[0-2*PI)</param>
        /// <returns></returns>
        private Bitmap TwistImage(Bitmap srcBmp, bool bXDir, double dMultValue, double dPhase)
        {
            System.Drawing.Bitmap destBmp = new Bitmap(srcBmp.Width, srcBmp.Height);

            // ��λͼ�������Ϊ��ɫ
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

                    // ȡ�õ�ǰ�����ɫ
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
        /// ������֤��ͼƬ
        /// </summary>
        /// <param name="code">��֤��</param>
        /// <returns>������֤��ͼƬ</returns>
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

            //���������������ɵ����
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

            //����������ɫ����֤���ַ�
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

            //��һ���߿� �߿���ɫΪColor.Gainsboro
            var gpen = new Pen(Color.Gainsboro, 0);
            g.DrawRectangle(gpen, 0, 0, image.Width - 1, image.Height - 1);
            g.Dispose();
            gpen.Dispose();
            //��������
            image = TwistImage(image, false, 8, 4);

            return image;
        }
    }
}