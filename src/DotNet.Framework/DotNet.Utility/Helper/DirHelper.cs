// ===============================================================================
// DotNet.Platform ������� 2016 ��Ȩ����
// ===============================================================================
using System;
using System.IO;
using System.Web;

namespace DotNet.Helper
{
    /// <summary>
    /// Ӧ�ó���·��������
    /// </summary>
    public static class DirHelper
    {
        /// <summary>
        /// ��Ŀ��Ŀ¼,����Web��Ŀ����վ��Ŀ¼,����Win��Ŀ������exe����Ŀ¼
        /// </summary>
        public static string RootDirectory
        {
            get { return AppDomain.CurrentDomain.BaseDirectory; }
        }

        /// <summary>
        /// ������Ŀ¼,����Web��Ŀ����վbinĿ¼,����Win��Ŀ������exe����Ŀ¼
        /// </summary>
        public static string OutDirectory
        {
            get
            {
                return IsWebApp ?
                    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin") : AppDomain.CurrentDomain.BaseDirectory;
            }
        }

        /// <summary>
        /// �Ƿ���WindowsӦ�ó���
        /// </summary>
        public static bool IsWinApp
        {
            get { return HttpContext.Current == null; }
        }

        /// <summary>
        /// �Ƿ���WebӦ�ó���
        /// </summary>
        public static bool IsWebApp
        {
            get { return HttpContext.Current != null; }
        }

        /// <summary>
        /// �Ƚ�Ŀ¼·���Ƿ���ͬ
        /// </summary>
        /// <param name="dir1">Ŀ¼1</param>
        /// <param name="dir2">Ŀ¼2</param>
        /// <returns>�����ͬһ��Ŀ¼,����true</returns>
        public static bool EqualsDirectory(string dir1, string dir2)
        {
            var d1 = new DirectoryInfo(dir1);
            var d2 = new DirectoryInfo(dir2);
            return d1.Name.Equals(d2.Name);
        }

        /// <summary>
        /// ��ȡ��Ŀ¼��ָ�����Ƶ�Ŀ¼·��
        /// </summary>
        /// <param name="dirName">Ŀ¼����</param>
        public static string GetRootDirectory(string dirName)
        {
            return Path.Combine(RootDirectory, dirName);
        }

        /// <summary>
        /// ��ȡ��Ŀ¼��ָ�����Ƶ��ļ�·��
        /// </summary>
        /// <param name="fileName">�ļ�����</param>
        public static string GetRootFilePath(string fileName)
        {
            return Path.Combine(RootDirectory, fileName);
        }

        /// <summary>
        /// ��ȡ��Ŀ¼��ָ��Ŀ¼��ָ�����Ƶ��ļ�·��
        /// </summary>
        /// <param name="dirName">Ŀ¼����</param>
        /// <param name="fileName">�ļ�����</param>
        public static string GetRootFilePath(string dirName, string fileName)
        {
            return Path.Combine(RootDirectory, dirName, fileName);
        }

        /// <summary>
        /// ��ȡ������Ŀ¼��ָ�����Ƶ�Ŀ¼·��
        /// </summary>
        /// <param name="dirName">Ŀ¼����</param>
        public static string GetOutDirectory(string dirName)
        {
            return Path.Combine(OutDirectory, dirName);
        }

        /// <summary>
        /// ��ȡ������Ŀ¼��ָ�����Ƶ��ļ�·��
        /// </summary>
        /// <param name="fileName">�ļ�����</param>
        public static string GetOutFilePath(string fileName)
        {
            return Path.Combine(OutDirectory, fileName);
        }

        /// <summary>
        /// ��ȡ������Ŀ¼��ָ��Ŀ¼��ָ�����Ƶ��ļ�·��
        /// </summary>
        /// <param name="dirName">Ŀ¼����</param>
        /// <param name="fileName">�ļ�����</param>
        public static string GetOutFilePath(string dirName, string fileName)
        {
            return Path.Combine(OutDirectory, dirName, fileName);
        }
    }
}