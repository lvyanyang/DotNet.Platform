// ===============================================================================
// DotNet.Platform ������� 2016 ��Ȩ����
// ===============================================================================
using System;
using System.Threading;

namespace DotNet.Helper
{
    /// <summary>
    /// ���̲߳�����
    /// </summary>
    public static class ThreadHelper
    {
        /// <summary>
        /// ��ʼһ�����߳�
        /// </summary>
        /// <param name="action">ִ�еĴ���</param>
        /// <param name="isBackground">�Ƿ��̨����</param>
        public static Thread StartThread(Action action, bool isBackground)
        {
            return StartThread(action, isBackground, null);
        }

        /// <summary>
        /// ��ʼһ�����߳�
        /// </summary>
        /// <param name="action">ִ�еĴ���</param>
        /// <param name="isBackground">�Ƿ��̨����</param>
        /// <param name="name">�߳�����</param>
        public static Thread StartThread(Action action, bool isBackground, string name)
        {
            Thread thread = new Thread(new ThreadStart(action));
            if (!string.IsNullOrEmpty(name))
            {
                thread.Name = name;
            }
            thread.IsBackground = isBackground;
            thread.Start();
            return thread;
        }

        /// <summary>
        /// ��ʼһ���µĺ�̨�����߳�
        /// </summary>
        /// <param name="action">ִ�еĴ���</param>
        public static Thread StartThread(Action action)
        {
            return StartThread(action, true);
        }
    }
}