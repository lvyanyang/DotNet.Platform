using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using DotNet.Extensions;
using DotNet.Helper;

namespace DotNet.Mvc
{
    public class KindEditorController : JsonController
    {
        public ActionResult Upload()
        {
            //文件保存目录路径
            var rootDir = "kindeditor";

            //最大文件大小
            var maxSize = 1000000;

            //定义允许上传的文件扩展名
            var extTable = new Dictionary<string, string>();
            extTable.Add("image", "gif,jpg,jpeg,png,bmp");
            extTable.Add("flash", "swf,flv");
            extTable.Add("media", "swf,flv,mp3,mp4,wav,wma,wmv,mid,avi,mpg,asf,rm,rmvb");
            extTable.Add("file", "doc,docx,xls,xlsx,ppt,htm,html,txt,zip,rar,gz,bz2");

            HttpPostedFileBase imgFile;
            try
            {
                imgFile = Request.Files["imgFile"];
            }
            catch (Exception ex)
            {
                return Json(new { error = 1, message = ex.Message });
            }

            if (imgFile == null)
            {
                return Json(new { error = 1, message = "请选择文件。" });
            }

            var dirName = Request.QueryString["dir"];
            if (string.IsNullOrEmpty(dirName))
            {
                dirName = "image";
            }

            if (!extTable.ContainsKey(dirName))
            {
                return Json(new { error = 1, message = "目录名不正确。" });
            }

            var fileName = imgFile.FileName;
            var fileExt = Path.GetExtension(fileName)?.ToLower();

            if (imgFile.InputStream == null || imgFile.InputStream.Length > maxSize)
            {
                return Json(new { error = 1, message = "上传文件大小超过限制。" });
            }

            if (string.IsNullOrEmpty(fileExt) || Array.IndexOf((extTable[dirName]).Split(','), fileExt.Substring(1).ToLower()) == -1)
            {
                return Json(new { error = 1, message = $"上传文件扩展名是不允许的扩展名。\n只允许{extTable[dirName]}格式。" });
            }

            var ymd = DateTime.Now.ToString("yyyyMMdd", DateTimeFormatInfo.InvariantInfo);
            var info = UploadHelper.Upload(imgFile, Path.Combine(rootDir, dirName, ymd));

            return Json(new { error = 0, url = UploadHelper.GetUploadUrl(info.Url) });
        }

        public ActionResult Manager()
        {
            string rootPath = "~/kindeditor";
            string rootUrl = UploadHelper.GetUploadFolderUrl(rootPath);
            string fileTypes = "gif,jpg,jpeg,png,bmp";//图片扩展名
            string currentPath, currentUrl, currentDirPath, moveupDirPath;
            var uploadSetting = UploadHelper.GetUploadSetting();
            string dirPath = uploadSetting.IsAbsolute ?
                Path.Combine(uploadSetting.AbsoluteFolder, "kindeditor") :
                Server.MapPath(rootPath);

            string dirName = Request.QueryString["dir"];
            if (!string.IsNullOrEmpty(dirName))
            {
                if (Array.IndexOf("image,flash,media,file".Split(','), dirName) == -1)
                {
                    return Content("无效的目录名称");
                }
                dirPath += "\\" + dirName + "\\";
                rootUrl += "/" + dirName + "/";
            }

            //根据path参数，设置各路径和URL
            string path = Request.QueryString["path"];
            path = string.IsNullOrEmpty(path) ? "" : path;
            if (string.IsNullOrEmpty(path))
            {
                currentPath = dirPath;
                currentUrl = rootUrl;
                currentDirPath = "";
                moveupDirPath = "";
            }
            else
            {
                currentPath = dirPath + path;
                currentUrl = rootUrl + path;
                currentDirPath = path;
                moveupDirPath = Regex.Replace(currentDirPath, @"(.*?)[^\/]+\/$", "$1");
            }

            //排序形式，name or size or type
            string order = Request.QueryString["order"];
            order = string.IsNullOrEmpty(order) ? "" : order.ToLower();

            //不允许使用..移动到上一级目录
            if (Regex.IsMatch(path, @"\.\."))
            {
                return Content("拒绝访问");
            }
            //最后一个字符不是/
            if (path != "" && !path.EndsWith("/"))
            {
                return Content("无效参数");
            }
            

            //遍历目录取得文件信息
            string[] dirList = new string[0];
            string[] fileList = new string[0];
            if (Directory.Exists(currentPath))
            {
                dirList = Directory.GetDirectories(currentPath);
                fileList = Directory.GetFiles(currentPath);
            }

            switch (order)
            {
                case "size":
                    Array.Sort(dirList, new NameSorter());
                    Array.Sort(fileList, new SizeSorter());
                    break;
                case "type":
                    Array.Sort(dirList, new NameSorter());
                    Array.Sort(fileList, new TypeSorter());
                    break;
                default:
                    Array.Sort(dirList, new NameSorter());
                    Array.Sort(fileList, new NameSorter());
                    break;
            }

            Hashtable result = new Hashtable();
            result["moveup_dir_path"] = moveupDirPath;
            result["current_dir_path"] = currentDirPath;
            result["current_url"] = currentUrl;
            result["total_count"] = dirList.Length + fileList.Length;
            List<Hashtable> dirFileList = new List<Hashtable>();
            result["file_list"] = dirFileList;
            for (int i = 0; i < dirList.Length; i++)
            {
                DirectoryInfo dir = new DirectoryInfo(dirList[i]);
                Hashtable hash = new Hashtable();
                hash["is_dir"] = true;
                hash["has_file"] = (dir.GetFileSystemInfos().Length > 0);
                hash["filesize"] = 0;
                hash["is_photo"] = false;
                hash["filetype"] = "";
                hash["filename"] = dir.Name;
                hash["datetime"] = dir.LastWriteTime.ToString("yyyy-MM-dd HH:mm:ss");
                dirFileList.Add(hash);
            }
            for (int i = 0; i < fileList.Length; i++)
            {
                FileInfo file = new FileInfo(fileList[i]);
                Hashtable hash = new Hashtable();
                hash["is_dir"] = false;
                hash["has_file"] = false;
                hash["filesize"] = file.Length;
                hash["is_photo"] = (Array.IndexOf(fileTypes.Split(','), file.Extension.Substring(1).ToLower()) >= 0);
                hash["filetype"] = file.Extension.Substring(1);
                hash["filename"] = file.Name;
                hash["datetime"] = file.LastWriteTime.ToString("yyyy-MM-dd HH:mm:ss");
                dirFileList.Add(hash);
            }
            return Json(result);
        }
    }

    public class NameSorter : IComparer
    {
        public int Compare(object x, object y)
        {
            if (x == null && y == null)
            {
                return 0;
            }
            if (x == null)
            {
                return -1;
            }
            if (y == null)
            {
                return 1;
            }
            FileInfo xInfo = new FileInfo(x.ToString());
            FileInfo yInfo = new FileInfo(y.ToString());

            return string.Compare(xInfo.FullName, yInfo.FullName, StringComparison.Ordinal);
        }
    }

    public class SizeSorter : IComparer
    {
        public int Compare(object x, object y)
        {
            if (x == null && y == null)
            {
                return 0;
            }
            if (x == null)
            {
                return -1;
            }
            if (y == null)
            {
                return 1;
            }
            FileInfo xInfo = new FileInfo(x.ToString());
            FileInfo yInfo = new FileInfo(y.ToString());

            return xInfo.Length.CompareTo(yInfo.Length);
        }
    }

    public class TypeSorter : IComparer
    {
        public int Compare(object x, object y)
        {
            if (x == null && y == null)
            {
                return 0;
            }
            if (x == null)
            {
                return -1;
            }
            if (y == null)
            {
                return 1;
            }
            FileInfo xInfo = new FileInfo(x.ToString());
            FileInfo yInfo = new FileInfo(y.ToString());

            return string.Compare(xInfo.Extension, yInfo.Extension, StringComparison.Ordinal);
        }
    }
}