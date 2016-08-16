using System.Collections.Generic;
using System.IO;
using System.Web;
using DotNet.Configuration;
using DotNet.Utility;

namespace DotNet.Helper
{
    /// <summary>
    /// 文件上传帮助类
    /// </summary>
    public static class UploadHelper
    {
        /// <summary>
        /// 获取上传文件配置
        /// </summary>
        public static UploadSetting GetUploadSetting()
        {
            return ConfigManager.GetSetting("Upload", () => new UploadSetting());
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="files">上传文件对象集合</param>
        /// <param name="subFolder">子文件夹</param>
        /// <returns>返回上传文件信息</returns>
        public static List<UploadInfo> Upload(HttpFileCollectionBase files, string subFolder = null)
        {
            List<UploadInfo> result = new List<UploadInfo>();
            if (files == null || files.Count <= 0) return result;
            foreach (string key in files.AllKeys)
            {
                HttpPostedFileBase file = files[key];
                result.Add(Upload(file, subFolder));
            }
            return result;
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="postFile">上传文件对象</param>
        /// <param name="subFolder">子文件夹</param>
        /// <returns>返回上传文件信息</returns>
        public static UploadInfo Upload(HttpPostedFileBase postFile, string subFolder = null)
        {
            if (postFile == null || postFile.ContentLength == 0) return null;
            const string defaultExtensionName = ".rar";
            var uploadSetting = GetUploadSetting();
            var virtualFolder = uploadSetting.UploadFolder;
            if (uploadSetting.IsAbsolute)
            {
                virtualFolder = "~/";
            }
            virtualFolder = VirtualPathUtility.ToAppRelative(virtualFolder);//转换为应用程序相对路径
            virtualFolder = VirtualPathUtility.AppendTrailingSlash(virtualFolder);//末尾添加斜杠,如果存在则不处理
            if (!string.IsNullOrEmpty(subFolder))
            {
                virtualFolder = VirtualPathUtility.Combine(virtualFolder, subFolder);//合并路径
                virtualFolder = VirtualPathUtility.AppendTrailingSlash(virtualFolder);
            }
            string fileName = Path.GetFileNameWithoutExtension(postFile.FileName.Replace("&", "").Replace("?", "")).Replace("&", "").Replace("?", "");
            string extName = Path.GetExtension(postFile.FileName);
            string extensionName = string.IsNullOrEmpty(extName) ? defaultExtensionName : extName;
            string targetFileName = $"{StringHelper.Guid()}{extensionName}";
            string targetFileVirtualPath = Path.Combine(virtualFolder, targetFileName).Replace("\\", "/");
            string targetFilePath;
            if (uploadSetting.IsAbsolute)
            {
                var vp = VirtualPathUtility.Combine(VirtualPathUtility.ToAbsolute(virtualFolder), targetFileName);
                vp = vp.Replace("/","\\");
                targetFilePath = uploadSetting.AbsoluteFolder + vp;
            }
            else
            {
                targetFilePath = HttpContext.Current.Server.MapPath(targetFileVirtualPath);
            }

            FileHelper.CreateDirectoryByPath(targetFilePath);
            postFile.SaveAs(targetFilePath);

            var uploadFileInfo = new UploadInfo();
            uploadFileInfo.Name = fileName;
            uploadFileInfo.Url = targetFileVirtualPath;
            uploadFileInfo.Size = postFile.ContentLength;
            return uploadFileInfo;
        }

        /// <summary>
        /// 获取Web浏览地址
        /// </summary>
        /// <param name="virtualPath">虚拟路径</param>
        public static string GetUploadUrl(string virtualPath)
        {
            var uploadSetting = GetUploadSetting();
            var absolutePath = VirtualPathUtility.ToAbsolute(virtualPath);
            if (uploadSetting.IsAbsolute)
            {
                return uploadSetting.UploadServer + absolutePath;
            }
            var ourl = HttpContext.Current.Request.Url.OriginalString;
            var pq = HttpContext.Current.Request.Url.PathAndQuery;
            return ourl.Replace(pq,string.Empty) + absolutePath;
        }
    }
}