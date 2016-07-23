// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================
using System.Drawing;
using System.Text;
using DevExpress.BarCodes;

namespace DotNet.Doc
{
    /// <summary>
    /// 条码操作
    /// </summary>
    public class BarCodeHelper
    {
        /// <summary>
        /// 生成条码对象
        /// </summary>
        /// <param name="text">条码内容</param>
        /// <param name="showCodeText">是否显示内容</param>
        public static BarCode BuildQRCode(string text, bool showCodeText)
        {
            BarCode barCode = new BarCode();
            barCode.Symbology = Symbology.QRCode;
            barCode.CodeText = text;
            barCode.BackColor = Color.White;
            barCode.ForeColor = Color.Black;
            barCode.RotationAngle = 0;
            barCode.CodeBinaryData = Encoding.UTF8.GetBytes(barCode.CodeText);
            barCode.Options.QRCode.CompactionMode = QRCodeCompactionMode.Byte;
            barCode.Options.QRCode.ErrorLevel = QRCodeErrorLevel.Q;
            barCode.Options.QRCode.ShowCodeText = false;
            return barCode;
        }
    }
}