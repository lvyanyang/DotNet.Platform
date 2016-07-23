// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Management;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;

namespace DotNet.Helper
{
    /// <summary>
    /// 文件目录操作类
    /// </summary>
    public static class FileHelper
    {
        /// <summary>
        /// 判断目标是文件夹还是目录(目录包括磁盘)
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns></returns>
        public static bool IsDirectory(string path)
        {
            FileInfo fi = new FileInfo(path);
            if ((fi.Attributes & FileAttributes.Directory) != 0)
                return true;
            return false;
        }

        /// <summary>
        /// 根据快捷方式获取文件路径
        /// </summary>
        /// <param name="path">快捷方式路径</param>
        public static string GetShortcutPath(string path)
        {
            string fileName = Path.GetFileNameWithoutExtension(path);
            string dir = Path.GetDirectoryName(path);
            if (string.IsNullOrEmpty(dir))
            {
                throw new System.ArgumentException("无效的快捷方式路径", "path");
            }
            string driver = dir.Substring(0, 2);
            dir = dir.Substring(2) + "\\";    //WQL中的文件夹前后都有"\"
            dir = dir.Replace("\\", "\\\\");  //WQL中的文件夹分隔符应是"\\"
            string wsql = "Select * From Win32_ShortcutFile Where Drive='" + driver + "' and Path='" + dir + "' and FileName ='" + fileName + "'";

            ManagementObjectSearcher searcher = new ManagementObjectSearcher(wsql);
            string tar = string.Empty;
            foreach (var managementBaseObject in searcher.Get())
            {
                var o = (ManagementObject)managementBaseObject;
                tar = o.GetPropertyValue("Target").ToString();
            }
            return tar;
        }

        #region 目录操作

        /// <summary>
        /// 获取一个目录下的所有目录(递归实现,不包括自身)
        /// </summary>
        /// <param name="dir">目录路径</param>
        /// <returns>返回目录下所有目录,不包括自身</returns>
        public static string[] GetDirectorys(string dir)
        {
            List<string> list = new List<string>();
            GetAllDirectorys(list, dir);
            return list.ToArray();
        }

        /// <summary>
        /// 获取一个目录下的所有目录(递归实现)
        /// </summary>
        /// <param name="list">存储列表</param>
        /// <param name="dir">目录路径</param>
        /// <returns>返回目录下所有目录,不包括自身</returns>
        private static void GetAllDirectorys(List<string> list, string dir)
        {
            string[] dirs = Directory.GetDirectories(dir);
            if (dirs.Length == 0)
            {
                return;
            }
            foreach (string item in dirs)
            {
                list.Add(item);
                GetAllDirectorys(list, item);
            }
        }

        /// <summary>
        /// 拷贝目录下面的全部文件和文件夹
        /// </summary>
        /// <param name="oldDir">旧目录</param>
        /// <param name="newDir">新目录</param>
        /// <exception cref="System.ArgumentNullException">oldDir不能为空</exception>
        /// <exception cref="System.ArgumentNullException">newDir不能为空</exception>
        public static void CopyDirectory(string oldDir, string newDir)
        {
            DirectoryInfo oldDirectory = new DirectoryInfo(oldDir);
            DirectoryInfo newDirectory = new DirectoryInfo(newDir);
            CopyDirectory(oldDirectory, newDirectory);
        }

        /// <summary>
        /// 拷贝目录下面的全部文件和文件夹
        /// </summary>
        /// <param name="oldDir">旧目录</param>
        /// <param name="newDir">新目录</param>
        /// <exception cref="System.ArgumentNullException">oldDir不能为空</exception>
        /// <exception cref="System.ArgumentNullException">newDir不能为空</exception>
        private static void CopyDirectory(DirectoryInfo oldDir, DirectoryInfo newDir)
        {
            if (oldDir == null) throw new ArgumentNullException("oldDir");
            if (newDir == null) throw new ArgumentNullException("newDir");
            string newDirectoryFullName = newDir.FullName;

            if (!Directory.Exists(newDirectoryFullName))
                Directory.CreateDirectory(newDirectoryFullName);

            FileInfo[] oldFileAry = oldDir.GetFiles();
            foreach (FileInfo aFile in oldFileAry)
                File.Copy(aFile.FullName, newDirectoryFullName + @"\" + aFile.Name, true);

            DirectoryInfo[] oldDirectoryAry = oldDir.GetDirectories();
            foreach (DirectoryInfo aOldDirectory in oldDirectoryAry)
            {
                DirectoryInfo aNewDirectory =
                    new DirectoryInfo(string.Concat(newDirectoryFullName, "\\", aOldDirectory.Name));
                CopyDirectory(aOldDirectory, aNewDirectory);
            }
        }

        /// <summary>
        /// 移动目录下面的全部文件和文件夹
        /// </summary>
        /// <param name="oldDir">旧目录</param>
        /// <param name="newDir">新目录</param>
        /// <exception cref="System.ArgumentNullException">oldDir不能为空</exception>
        /// <exception cref="System.ArgumentNullException">newDir不能为空</exception>
        public static void MoveDirectory(string oldDir, string newDir)
        {
            DirectoryInfo oldDirectory = new DirectoryInfo(oldDir);
            DirectoryInfo newDirectory = new DirectoryInfo(newDir);
            MoveDirectory(oldDirectory, newDirectory);
            Directory.Delete(oldDir, true);
        }

        /// <summary>
        /// 拷贝目录下面的全部文件和文件夹
        /// </summary>
        /// <param name="oldDir">旧目录</param>
        /// <param name="newDir">新目录</param>
        /// <exception cref="System.ArgumentNullException">oldDir不能为空</exception>
        /// <exception cref="System.ArgumentNullException">newDir不能为空</exception>
        private static void MoveDirectory(DirectoryInfo oldDir, DirectoryInfo newDir)
        {
            if (oldDir == null) throw new ArgumentNullException("oldDir");
            if (newDir == null) throw new ArgumentNullException("newDir");
            string newDirectoryFullName = newDir.FullName;

            if (!Directory.Exists(newDirectoryFullName))
            {
                Directory.CreateDirectory(newDirectoryFullName);
            }

            FileInfo[] oldFileAry = oldDir.GetFiles();
            foreach (FileInfo aFile in oldFileAry)
            {
                File.Move(aFile.FullName, newDirectoryFullName + @"\" + aFile.Name);
            }

            DirectoryInfo[] oldDirectoryAry = oldDir.GetDirectories();
            foreach (DirectoryInfo aOldDirectory in oldDirectoryAry)
            {
                DirectoryInfo aNewDirectory =
                    new DirectoryInfo(string.Concat(newDirectoryFullName, "\\", aOldDirectory.Name));
                MoveDirectory(aOldDirectory, aNewDirectory);
            }
        }

        /// <summary>
        /// 删除目录
        /// </summary>
        /// <param name="oldDir">目录路径(绝对路径)</param>
        public static void DeleteDirectory(string oldDir)
        {
            Directory.Delete(oldDir, true);
        }

        /// <summary>
        /// 安全删除文件(检测文件是否存在)
        /// </summary>
        /// <param name="filePath">文件路径</param>
        public static void DeleteFile(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                return;
            }
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        /// <summary>
        /// 根据一个文件路径创建路径中的所有目录
        /// </summary>
        /// <param name="path">要创建的完整路径</param>
        /// <returns>创建成功返回true</returns>
        public static void CreateDirectoryByPath(string path)
        {
            FileInfo file = new FileInfo(path);
            DirectoryInfo dir = file.Directory;
            if (dir != null)
            {
                string dirFullName = dir.FullName;
                if (!Directory.Exists(dirFullName))
                {
                    Directory.CreateDirectory(dirFullName);
                }
            }
        }

        #endregion

        #region 文件操作

        /// <summary>
        /// 获取一个目录下的所有文件(递归实现)
        /// </summary>
        /// <param name="dir">目录路径</param>
        /// <returns>返回目录下所有文件列表</returns>
        public static string[] GetFiles(string dir)
        {
            var list = new List<string>();
            GetFiles(list, dir);
            return list.ToArray();
        }

        /// <summary>
        /// 获取一个目录下的所有文件(递归实现)
        /// </summary>
        /// <param name="list">存储列表</param>
        /// <param name="dir">目录路径</param>
        /// <returns>返回目录下所有文件列表</returns>
        private static void GetFiles(List<string> list, string dir)
        {
            if (!Directory.Exists(dir)) return;
            string[] fileNames = Directory.GetFiles(dir);
            string[] directories = Directory.GetDirectories(dir);
            list.AddRange(fileNames);
            foreach (string item in directories)
            {
                GetFiles(list, item);
            }
        }

        /// <summary>
        /// 文件改名
        /// </summary>
        /// <param name="sourceFileName">原文件路径</param>
        /// <param name="newName">文件新名字(一个名字包含扩展名,不包括路径)</param>
        public static bool RenameFile(string sourceFileName, string newName)
        {
            string dir = new FileInfo(sourceFileName).DirectoryName;
            if (dir != null)
            {
                string newpath = Path.Combine(dir, newName);
                File.Move(sourceFileName, newpath);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 获取文件版本号
        /// </summary>
        /// <param name="filePath">文件路径</param>
        public static string GetFileVersion(string filePath)
        {
            if (!File.Exists(filePath))
                return string.Empty;

            FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(filePath);
            return versionInfo.FileVersion;
        }

        /// <summary>
        /// 获取文件md5
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns>文件md5</returns>
        public static string GetFileMd5(string filePath)
        {
            FileStream fs = new FileStream(filePath, FileMode.Open);
            MD5 md5 = MD5.Create();
            byte[] hash = md5.ComputeHash(fs);
            string hashCode = BitConverter.ToString(hash).Replace("-", "");
            return hashCode;
        }

        /// <summary>
        /// 删除文件或者文件夹到回收站(Win32API实现)
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="isMoveToRecycleBin">是否移动到回收站,true移到回收站,false直接删除</param>
        /// <returns>删除成功返回true</returns>
        public static bool DeleteToRecycleBin(string path, bool isMoveToRecycleBin)
        {
            Shfileopstruct lpFileOp = new Shfileopstruct
            {
                WFunc = NativeConstants.FO_DELETE,
                PFrom = path + "\0",
                FFlags = NativeConstants.FOF_NOCONFIRMATION | NativeConstants.FOF_NOERRORUI | NativeConstants.FOF_SILENT
            };
            if (!isMoveToRecycleBin)
            {
                lpFileOp.FFlags &= ~NativeConstants.FOF_ALLOWUNDO;
            }
            lpFileOp.FAnyOperationsAborted = false;

            return NativeMethods.SHFileOperation(ref lpFileOp);
        }

        #endregion

        #region 流操作

        /// <summary>
        /// 序列化对象(二进制)
        /// </summary>
        /// <param name="data">序列化对象</param>
        /// <returns>字节数组</returns>
        public static byte[] Serialize(object data)
        {
            if (data == null) throw new ArgumentNullException("data");
            MemoryStream stream = new MemoryStream();
            Serialize(stream, data);
            return stream.ToArray();
        }

        /// <summary>
        /// 序列化对象(二进制)
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="data">序列化对象</param>
        public static void Serialize(string path, object data)
        {
            using (FileStream fs = new FileStream(path, FileMode.Create))
            {
                Serialize(fs, data);
            }
        }

        /// <summary>
        /// 序列化对象(二进制)
        /// </summary>
        /// <param name="stream">文件流(把对象序列化到此流中)</param>
        /// <param name="data">序列化对象</param>
        public static void Serialize(Stream stream, object data)
        {
            if (stream == null) throw new ArgumentNullException("stream");
            if (data == null) throw new ArgumentNullException("data");
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(stream, data);
        }

        /// <summary>
        /// 反序列化对象(二进制)
        /// </summary>
        /// <param name="bytes">要反序列化的数组</param>
        /// <returns>序列化对象</returns>
        public static object Deserialize(byte[] bytes)
        {
            if (bytes == null) throw new ArgumentNullException("bytes");
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream(bytes);
            object obj = bf.Deserialize(ms);
            return obj;
        }

        /// <summary>
        /// 反序列化对象(二进制)
        /// </summary>
        /// <param name="path">从指定文件名反序列化对象</param>
        /// <returns>序列化对象</returns>
        public static object Deserialize(string path)
        {
            object result = null;
            if (File.Exists(path))
            {
                using (FileStream fs = new FileStream(path, FileMode.Open))
                {
                    result = Deserialize(fs);
                }
            }
            return result;
        }

        /// <summary>
        /// 反序列化对象(二进制)
        /// </summary>
        /// <param name="stream">序列化流(请自己释放流)</param>
        /// <returns>序列化对象</returns>
        public static object Deserialize(Stream stream)
        {
            if (stream == null) throw new ArgumentNullException("stream");
            BinaryFormatter formatter = new BinaryFormatter();
            return formatter.Deserialize(stream);
        }

        /// <summary>
        /// 将 Stream 转成 byte 数组
        /// </summary>
        /// <param name="stream">流</param>
        /// <returns>字节数组</returns>
        public static byte[] ConvertToBytes(Stream stream)
        {
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);

            // 设置当前流的位置为流的开始
            stream.Seek(0, SeekOrigin.Begin);
            return bytes;
        }

        /// <summary>
        /// 将Stream 转化成 string
        /// </summary>
        /// <param name="stream">流</param>
        public static string ConvertToString(Stream stream)
        {
            #region

            string strResult = "";
            StreamReader sr = new StreamReader(stream, Encoding.UTF8);

            Char[] read = new Char[256];
            // Read 256 charcters at a time.    
            int count = sr.Read(read, 0, 256);

            while (count > 0)
            {
                // Dump the 256 characters on a string and display the string onto the console.
                string str = new String(read, 0, count);
                strResult += str;
                count = sr.Read(read, 0, 256);
            }

            // 释放资源
            sr.Close();
            //sr.Dispose();
            return strResult;

            #endregion
        }

        /// <summary>
        /// 复制Stream
        /// </summary>
        /// <param name="input">复制前Stream</param>
        /// <param name="output">复制后Stream</param>
        public static void CopyStream(Stream input, Stream output)
        {
            byte[] buffer = new byte[8192];
            int read;
            while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                output.Write(buffer, 0, read);
            }
        }

        /// <summary>
        /// 将二进制文件读入byte[]（如图片等）
        /// </summary>
        /// <param name="fileName">文件名与路径</param>
        public static byte[] ReadFile(string fileName)
        {
            FileStream pFileStream = null;
            byte[] pReadByte = new byte[0];

            try
            {
                pFileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                BinaryReader r = new BinaryReader(pFileStream);
                r.BaseStream.Seek(0, SeekOrigin.Begin);//将文件指针设置到文件开
                pReadByte = r.ReadBytes((int)r.BaseStream.Length);
                return pReadByte;
            }
            catch
            {
                return pReadByte;
            }
            finally
            {
                if (pFileStream != null)
                    pFileStream.Close();
            }
        }

        /// <summary>
        /// 写byte[]数据到文件（如图片等二进制数据）
        /// </summary>
        /// <param name="pReadByte">二进制数</param>
        /// <param name="fileName">文件名</param>
        /// <returns>成功返回True 否返回False</returns>
        public static bool WriteFile(byte[] pReadByte, string fileName)
        {
            FileStream pFileStream = null;
            try
            {
                pFileStream = new FileStream(fileName, FileMode.OpenOrCreate);
                pFileStream.Write(pReadByte, 0, pReadByte.Length);
            }
            catch
            {
                return false;
            }
            finally
            {
                if (pFileStream != null)
                    pFileStream.Close();
            }
            return true;
        }

        /// <summary>
        /// 字节数组转换为内存流 
        /// </summary>
        /// <param name="bytes">字节数组</param>
        public static MemoryStream ToMemoryStream(byte[] bytes)
        {
            if (bytes==null)
            {
                return null;
            }
            return new MemoryStream(bytes);
        }


        /// <summary>
        /// 内存流转为字节数组
        /// </summary>
        /// <param name="ms">内存流</param>
        public static byte[] ToByteArray(MemoryStream ms)
        {
            return ms.ToArray();
        }

        #endregion
    }
}