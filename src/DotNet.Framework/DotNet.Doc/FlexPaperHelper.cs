// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================
using System;
using System.Diagnostics;
using DotNet.Utility;

namespace DotNet.Doc
{
    /// <summary>
    /// 
    /// </summary>
    public class FlexPaperHelper
    {
        ///// <summary>  
        ///// 把Word文件转换成为PDF格式文件  
        ///// </summary>  
        ///// <param name="sourcePath">源文件路径</param>  
        ///// <param name="targetPath">目标文件路径</param>   
        ///// <returns>true=转换成功</returns>  
        //public static bool WordToPDF(string sourcePath, string targetPath)
        //{
        //    bool result = false;
        //    Microsoft.Office.Interop.Word.WdExportFormat exportFormat = Microsoft.Office.Interop.Word.WdExportFormat.wdExportFormatPDF;
        //    Microsoft.Office.Interop.Word.ApplicationClass application = null;

        //    Microsoft.Office.Interop.Word.Document document = null;
        //    try
        //    {
        //        application = new Microsoft.Office.Interop.Word.ApplicationClass();
        //        application.Visible = false;
        //        document = application.Documents.Open(sourcePath);
        //        document.SaveAs();
        //        document.ExportAsFixedFormat(targetPath, exportFormat);
        //        result = true;
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e.Message);
        //        result = false;
        //    }
        //    finally
        //    {
        //        if (document != null)
        //        {
        //            document.Close();
        //            document = null;
        //        }
        //        if (application != null)
        //        {
        //            application.Quit();
        //            application = null;
        //        }
        //        GC.Collect();
        //        GC.WaitForPendingFinalizers();
        //        GC.Collect();
        //        GC.WaitForPendingFinalizers();
        //    }
        //    return result;
        //}

        ///// <summary>  
        ///// 把Microsoft.Office.Interop.Excel文件转换成PDF格式文件  
        ///// </summary>  
        ///// <param name="sourcePath">源文件路径</param>  
        ///// <param name="targetPath">目标文件路径</param>   
        ///// <returns>true=转换成功</returns>  
        //public static bool ExcelToPDF(string sourcePath, string targetPath)
        //{
        //    bool result = false;
        //    Microsoft.Office.Interop.Excel.XlFixedFormatType targetType = Microsoft.Office.Interop.Excel.XlFixedFormatType.xlTypePDF;
        //    object missing = Type.Missing;
        //    Microsoft.Office.Interop.Excel.ApplicationClass application = null;
        //    Microsoft.Office.Interop.Excel.Workbook workBook = null;
        //    try
        //    {
        //        application = new Microsoft.Office.Interop.Excel.ApplicationClass();
        //        application.Visible = false;
        //        workBook = application.Workbooks.Open(sourcePath);
        //        workBook.SaveAs();
        //        workBook.ExportAsFixedFormat(targetType, targetPath);
        //        result = true;
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e.Message);
        //        result = false;
        //    }
        //    finally
        //    {
        //        if (workBook != null)
        //        {
        //            workBook.Close(true, missing, missing);
        //            workBook = null;
        //        }
        //        if (application != null)
        //        {
        //            application.Quit();
        //            application = null;
        //        }
        //        GC.Collect();
        //        GC.WaitForPendingFinalizers();
        //        GC.Collect();
        //        GC.WaitForPendingFinalizers();
        //    }
        //    return result;
        //}
        ///// <summary>  
        ///// 把PowerPoint文件转换成PDF格式文件  
        ///// </summary>  
        ///// <param name="sourcePath">源文件路径</param>  
        ///// <param name="targetPath">目标文件路径</param>   
        ///// <returns>true=转换成功</returns>  
        //public static bool PowerPointToPDF(string sourcePath, string targetPath)
        //{
        //    bool result;
        //    Microsoft.Office.Interop.PowerPoint.PpSaveAsFileType targetFileType = Microsoft.Office.Interop.PowerPoint.PpSaveAsFileType.ppSaveAsPDF;
        //    object missing = Type.Missing;
        //    Microsoft.Office.Interop.PowerPoint.ApplicationClass application = null;
        //    Microsoft.Office.Interop.PowerPoint.Presentation persentation = null;
        //    try
        //    {
        //        application = new Microsoft.Office.Interop.PowerPoint.ApplicationClass();
        //        //application.Visible = MsoTriState.msoFalse;  
        //        persentation = application.Presentations.Open(sourcePath, MsoTriState.msoTrue, MsoTriState.msoFalse, MsoTriState.msoFalse);
        //        persentation.SaveAs(targetPath, targetFileType, Microsoft.Office.Core.MsoTriState.msoTrue);

        //        result = true;
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e.Message);
        //        result = false;
        //    }
        //    finally
        //    {
        //        if (persentation != null)
        //        {
        //            persentation.Close();
        //            persentation = null;
        //        }
        //        if (application != null)
        //        {
        //            application.Quit();
        //            application = null;
        //        }
        //        GC.Collect();
        //        GC.WaitForPendingFinalizers();
        //        GC.Collect();
        //        GC.WaitForPendingFinalizers();
        //    }
        //    return result;
        //}

        /// <summary>
        /// 把PDF文件转化为SWF文件
        /// </summary>
        /// <param name="toolPah">pdf2swf工具路径</param>
        /// <param name="sourcePath">源文件路径</param>
        /// <param name="targetPath">目标文件路径</param>
        /// <returns>true=转化成功</returns>
        public static BoolMessage PDFToSWF(string toolPah, string sourcePath, string targetPath)
        {
            Process pc = new Process();

            string cmd = toolPah;
            string args = " -t " + sourcePath + " -s flashversion=9 -o " + targetPath;
            try
            {
                ProcessStartInfo psi = new ProcessStartInfo(cmd, args);
                psi.WindowStyle = ProcessWindowStyle.Hidden;
                pc.StartInfo = psi;
                pc.Start();
                pc.WaitForExit();
                return BoolMessage.True;
            }
            catch (Exception ex)
            {
                return new BoolMessage(false, ex.Message);
            }
            finally
            {
                pc.Close();
                pc.Dispose();
            }
        }

        /// <summary>
        /// png、jpg和jpeg文件的转化
        /// </summary>
        /// <param name="toolPah"></param>
        /// <param name="sourcePath"></param>
        /// <param name="targetPath"></param>
        /// <returns></returns>
        public static BoolMessage PicturesToSwf(string toolPah, string sourcePath, string targetPath)
        {
            Process pc = new Process();

            string cmd = toolPah;
            string args = " " + sourcePath + " -o " + targetPath + " -T 9";
            //如果是多个图片转化为swf 格式为 ..jpeg2swf.exe C:\1.jpg C:\2.jpg -o C:\swf1.swf
            try
            {
                ProcessStartInfo psi = new ProcessStartInfo(cmd, args);
                psi.WindowStyle = ProcessWindowStyle.Hidden;
                pc.StartInfo = psi;
                pc.Start();
                pc.WaitForExit();
                return BoolMessage.True;
            }
            catch (Exception ex)
            {
                return new BoolMessage(false, ex.Message);
            }
            finally
            {
                pc.Close();
                pc.Dispose();
            }
        }

        /// <summary>
        /// Gif文件转化为swf
        /// </summary>
        /// <param name="toolPah"></param>
        /// <param name="sourcePath"></param>
        /// <param name="targetPath"></param>
        /// <returns></returns>
        public static BoolMessage GifPicturesToSwf(string toolPah, string sourcePath, string targetPath)
        {
            Process pc = new Process();

            string cmd = toolPah;
            string args = " " + sourcePath + " -o " + targetPath;
            try
            {
                ProcessStartInfo psi = new ProcessStartInfo(cmd, args);
                psi.WindowStyle = ProcessWindowStyle.Hidden;
                pc.StartInfo = psi;
                pc.Start();
                pc.WaitForExit();
                return BoolMessage.True;
            }
            catch (Exception ex)
            {
                return new BoolMessage(false, ex.Message);
            }
            finally
            {
                pc.Close();
                pc.Dispose();
            }
        }
    }
}