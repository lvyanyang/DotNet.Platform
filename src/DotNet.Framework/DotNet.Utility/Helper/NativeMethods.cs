// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================
using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;

namespace DotNet.Helper
{
    /// <summary>
    /// Windows系统函数
    /// </summary>
    public static class NativeMethods
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hModule"></param>
        /// <param name="nType"></param>
        /// <param name="sName"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        public delegate bool EnumResNameProc(IntPtr hModule, IntPtr nType, StringBuilder sName, IntPtr lParam);

        /// <summary>
        /// 获取cmd窗口
        /// </summary>
        /// <returns>cmd窗口句柄</returns>
        [DllImport("kernel32.dll")]
        public static extern IntPtr GetConsoleWindow();

        /// <summary>
        /// 启动cmd窗口(win32函数)
        /// </summary>
        /// <returns>成功返回True</returns>
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        public static extern bool AllocConsole();

        /// <summary>
        /// 释放cmd窗口(win32函数)
        /// </summary>
        /// <returns>成功返回True</returns>
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        public static extern bool FreeConsole();

        /// <summary>
        /// 启动cmd窗口
        /// </summary>
        /// <returns>成功返回True</returns>
        public static bool ShowConsole()
        {
            var result = AllocConsole();
            IntPtr hwnd = GetConsoleWindow();
            IntPtr hMenu = GetSystemMenu(hwnd, false);
            DeleteMenu(hMenu, NativeConstants.SC_CLOSE, NativeConstants.MF_BYCOMMAND);
            return result;
        }

        /// <summary>
        /// 关闭cmd窗口
        /// </summary>
        /// <returns>成功返回True</returns>
        public static bool CloseConsole()
        {
            return FreeConsole();
        }

        /// <summary>  
        /// 获取输入法说明  
        /// </summary>  
        /// <param name="hkl"></param>  
        /// <param name="sName"></param>  
        /// <param name="nBuffer"></param>  
        /// <returns></returns>  
        [DllImport("Imm32.dll", CharSet = CharSet.Unicode)]
        public extern static int ImmGetDescription(IntPtr hkl, StringBuilder sName, int nBuffer);

        /// <summary>  
        /// 得到输入法的文件名  
        /// </summary>  
        /// <param name="hkl"></param>  
        /// <param name="sFileName"></param>  
        /// <param name="nBuffer"></param>  
        /// <returns></returns>  
        [DllImport("Imm32.dll", CharSet = CharSet.Unicode)]
        public extern static int ImmGetIMEFileName(IntPtr hkl, StringBuilder sFileName, int nBuffer);

        /// <summary>
        /// 检测应用程序是否自动启动
        /// </summary>
        /// <param name="projectName">应用程序名称</param>
        public static bool CheckIsAutoStart(string projectName)
        {
            RegistryKey hklm = Registry.LocalMachine;
            RegistryKey run = hklm.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run");
            if (run == null) return false;
            bool result = false;
            try
            {
                result = run.GetValue(projectName) != null;
                hklm.Close();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            return result;
        }

        /// <summary> 
        /// 是否是开机启动项 
        /// </summary> 
        /// <param name="projectName">应用程序名称</param> 
        public static bool IsAutoStart(string projectName)
        {
            bool result = false;
            RegistryKey hklm = Registry.LocalMachine;
            RegistryKey run = hklm.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run");
            if (run == null) return false;
            if (run.GetValue(projectName) != null)
            {
                result = true;
            }
            run.Close();
            hklm.Close();
            return result;
        }

        /// <summary> 
        /// 添加开机启动项 
        /// </summary> 
        /// <param name="isStarted">是否开机启动</param> 
        /// <param name="projectName">应用程序名称</param> 
        /// <param name="projectPath">应用程序执行文件路径</param> 
        public static void AutoStart(bool isStarted, string projectName, string projectPath)
        {
            RegistryKey hklm = Registry.LocalMachine;
            RegistryKey run = hklm.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run");
            if (run == null) return;

            if (isStarted)
            {
                try
                {
                    //if (Run.GetValue(projectName) == null)
                    run.SetValue(projectName, projectPath);
                    run.Close();
                    hklm.Close();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
            }
            else
            {
                try
                {
                    if (run.GetValue(projectName) != null)
                        run.DeleteValue(projectName);
                    run.Close();
                    hklm.Close();
                }
                catch (Exception exy)
                {
                    System.Diagnostics.Debug.WriteLine(exy.Message);
                }
            }
        }

        /// <summary>
        /// 加载图标
        /// </summary>
        /// <param name="hInstance"></param>
        /// <param name="sId"></param>
        /// <param name="nType"></param>
        /// <param name="cx"></param>
        /// <param name="cy"></param>
        /// <param name="fuLoad"></param>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public extern static IntPtr LoadImage(IntPtr hInstance, string sId, int nType, int cx, int cy, int fuLoad);

        /// <summary>
        /// 加载文件
        /// </summary>
        /// <param name="sFileName"></param>
        /// <param name="hFile"></param>
        /// <param name="dwFlags"></param>
        /// <returns></returns>
        [DllImport("Kernel32.dll", CharSet = CharSet.Unicode)]
        public extern static IntPtr LoadLibraryEx(string sFileName, IntPtr hFile, int dwFlags);


        /// <summary>
        /// 加载图标
        /// </summary>
        /// <param name="hInstance"></param>
        /// <param name="iId"></param>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public extern static IntPtr LoadIcon(IntPtr hInstance, string iId);

        /// <summary>
        /// 释放Dll
        /// </summary>
        /// <param name="hModule"></param>
        /// <returns></returns>
        [DllImport("Kernel32.dll", CharSet = CharSet.Unicode)]
        public extern static bool FreeLibrary(IntPtr hModule);

        /// <summary>
        /// 枚举资源
        /// </summary>
        /// <param name="hModule"></param>
        /// <param name="nType"></param>
        /// <param name="lpEnumFunc"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        [DllImport("Kernel32.dll", CharSet = CharSet.Unicode)]
        public extern static bool EnumResourceNames(IntPtr hModule, IntPtr nType, EnumResNameProc lpEnumFunc, int lParam);

        /// <summary>
        /// 该函数从限定的可执行文件，动态链接库（DLL）；或者图标文件中恢复图标句柄．为恢复大或小的图标句柄数组，使用ExtractlconEx函数。
        /// </summary>
        /// <param name="hInst">调用函数的应用程序的事例句柄。</param>
        /// <param name="lpszExeFileName">代表可执行文件，DLL，或者图标文件的文件名的空结束字符串指针。</param>
        /// <param name="nIconIndex">指定要恢复图标基于零的变址。
        ///     <para>例如，如果值是0,函数返回限定的文件中第一个图标的句柄，如值是O函数返回限定文件中图标的总数；</para>
        ///     <para>如果文件是可执行文件或DLL返回值为RT_GROUP_ICON资源的数目:如果文件是一个.ICO文件，返回值是1；</para>
        ///     <para>在Windows95，WindowsNT4.0和更高版本中，如果值为不等于向-l的负数，函数返回限定文件图标句柄，该文件的资源标识符等于nlconlndex绝对值。</para>
        ///     <para>例如，使用-3来获取资源标识符为3的图标。为获取资源标识符为1的图标，可采用ExtractlconEx函数。</para>
        /// </param>
        /// <returns>返回值是图标句柄。如果限定的文件不是可执行文件，DLL，或者图标文件返回是1；如果发现在文件中没有图标，返回值是NULL。</returns>
        /// <remarks>必须调用Destroyclon函数来清除由Extractlcon函数返回的图标句柄。</remarks>
        [DllImport("shell32.dll", CharSet = CharSet.Unicode)]
        public static extern IntPtr ExtractIcon(IntPtr hInst, string lpszExeFileName, uint nIconIndex);

        /// <summary>
        /// 该函数从限定的可执行文件；动态链接库（DLL），或者图标文件中生成图标句柄数组。
        /// </summary>
        /// <param name="lpszFile">定义可获取图标的可执行文件，DLL,或者图标文件的名字的空结束字符串指针。</param>
        /// <param name="nIconIndex">
        ///     <para>指定抽取第一个图标基于零的变址；</para>
        ///     <para>例如，如果该值是零；函数在限定的文件中抽取第一图标；如该值是C1且phlconLarge和phiconSmall参数均为NULL，函数返回限定文件中图标的总数；</para>
        ///     <para>如果文件是可执行文件或DLL；返回值是RT_GROUP_ICON资源的数目；如果文件是一个ICO文件，返回值是1；</para>
        ///     <para>在Windows95，WindowsNT4.0,和更高版本中，如果值为负数且phlconLarge和phiconSmall均不为NULL，函数从获取图标开始，该图标的资源标识符等于nlconlndex绝对值。</para>
        ///     <para>例如，使用-3来获取资源标识符为3的图标。</para>
        /// </param>
        /// <param name="phiconLarge">指向图标句柄数组的指针，它可接收从文件获取的大图标的句柄。如果该参数是NULL没有从文件抽取大图标。</param>
        /// <param name="phiconSmall">指向图标句柄数组的指针，它可接收从文件获取的小图标的句柄。如果该参数是NULL，没有从文件抽取小图标。</param>
        /// <param name="nIcons">指定要从文件中抽取图标的数目。</param>
        /// <returns>
        ///     <para>如果nlconlndex参数是-1，PhiconLarge和PhiconSmall参数是NULL，返回值是包含在指定文件中的图标数目；</para>
        ///     <para>否则，返回值是成功地从文件中获取图标的数目。</para>
        /// </returns>
        /// <remarks>
        ///     <para>必须调用Destroylcon函数来清除由ExtractlconEx函数返回的图标。</para>
        ///     <para>为恢复大小图标尺寸,可使用SM_CXICON,SM_CYICON,SM_CXSMICON,SM_CYSMICON标记来调用GetSystemMetrics函数。</para>
        ///     <para>Windows CE：nlconlndex参数必须是零或CN（N是指定的资源标识符）；nlcons参数必须是1。</para>
        /// </remarks>
        [DllImport("shell32.dll", CharSet = CharSet.Unicode)]
        public static extern int ExtractIconEx(string lpszFile, int nIconIndex, ref IntPtr phiconLarge,
                                               ref IntPtr phiconSmall, uint nIcons);

        /// <summary>
        /// 该函数清除图标和释放任何被图标占用的存储空间。
        /// </summary>
        /// <param name="hIcon">是要清除留标的句柄。该图标应处于未被使用状态。</param>
        /// <returns>如果函数成功，返回值是非零：如果函数失效，返回值是零。</returns>
        /// <remarks>
        ///     <para>只有利用Createlconlndirect函数创建的图标和光标才能调用Destroylcon函数，不要使用该函数清除一个共享图标。</para>
        ///     <para>只要调入它的模块存在于存储器中，共享图标就一直有效。</para>
        ///     <para>下列函数可获取共享图标：</para>
        ///     <para>Loadlcon;Loadlmage（如果你使用LR_共享标记）；</para>
        ///     <para>copylmage（如果你使用LR_COPYRETURNORG而且hlmage参数为共享目标）。</para>
        ///     <para>Windows CE：Destroylcon函数可以通过图标句柄调用，这些图标句柄来自于Createlconlndirect,ExtractlconEx,Loadlmage或Loadlco函数。</para>
        ///     <para>在调用Destroylcon函数之后这些图标句柄变为无效。</para>
        /// </remarks>
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern bool DestroyIcon(IntPtr hIcon);

        /// <summary>
        /// 提取指定大小的ICO图标
        /// </summary>
        /// <param name="szFileName">文件路径</param>
        /// <param name="nIconIndex">提取的图标索引</param>
        /// <param name="cxIcon">宽度</param>
        /// <param name="cyIcon">高度</param>
        /// <param name="phicon">ICO句柄</param>
        /// <param name="piconid">指针返回资源标识符的图标最适合当前的显示设备</param>
        /// <param name="nIcons"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern int PrivateExtractIcons(string szFileName, int nIconIndex, int cxIcon, int cyIcon,
                                                     ref IntPtr phicon, IntPtr piconid, uint nIcons, uint flags);


        /// <summary>
        /// 该函数设置由不同线程产生的窗口的显示状态
        /// </summary>
        /// <param name="hWnd">窗口句柄</param>
        /// <param name="cmdShow">指定窗口如何显示。查看允许值列表，请查阅ShowWlndow函数的说明部分</param>
        /// <returns>如果函数原来可见，返回值为非零；如果函数原来被隐藏，返回值为零</returns>
        [DllImport("User32.dll", CharSet = CharSet.Unicode)]
        public static extern bool ShowWindowAsync(IntPtr hWnd, int cmdShow);

        /// <summary>
        /// 该函数获得一个顶层窗口的句柄，该窗口的类名和窗口名与给定的字符串相匹配。这个函数不查找子窗口。在查找时不区分大小写。
        /// </summary>
        /// <param name="lpClassName">
        ///     <para>指向一个指定了类名的空结束字符串，或一个标识类名字符串的成员的指针。</para>
        ///     <para>如果该参数为一个成员，则它必须为前次调用theGlobafAddAtom函数产生的全局成员。</para>
        ///     <para>该成员为16位，必须位于IpClassName的低 16位，高位必须为 0。</para>
        /// </param>
        /// <param name="lpWindowName">指向一个指定了窗口名（窗口标题）的空结束字符串。如果该参数为空，则为所有窗口全匹配。</param>
        /// <remarks>Windows CE：若类名是一个成员，它必须是从 RegisterClass返回的成员。</remarks>
        /// <returns>如果函数成功，返回值为具有指定类名和窗口名的窗口句柄；如果函数失败，返回值为NULL。</returns>
        [DllImport("User32.dll", CharSet = CharSet.Unicode)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        /// <summary>
        /// 该函数设置指定窗口的显示状态。
        /// </summary>
        /// <param name="hwnd">窗口句柄。</param>
        /// <param name="nCmdShow">
        ///     <para>指定窗口如何显示。如果发送应用程序的程序提供了STARTUPINFO结构，则应用程序第一次调用ShowWindow时该参数被忽略。</para>
        ///     <para>否则，在第一次调用ShowWindow函数时，该值应为在函数WinMain中nCmdShow参数。在随后的调用中，该参数可以为下列值之一：</para>
        ///     <list type="table">
        ///         <item>
        ///             <term>SW_FORCEMINIMIZE</term>
        ///             <description>在WindowNT5.0中最小化窗口，即使拥有窗口的线程被挂起也会最小化。在从其他线程最小化窗口时才使用这个参数。</description>
        ///         </item>
        ///         <item>
        ///             <term>SW_MIOE</term>
        ///             <description>隐藏窗口并激活其他窗口。</description>
        ///         </item>
        ///         <item>
        ///             <term>SW_MAXIMIZE</term>
        ///             <description>最大化指定的窗口。</description>
        ///         </item>
        ///         <item>
        ///             <term>SW_MINIMIZE</term>
        ///             <description>最小化指定的窗口并且激活在Z序中的下一个顶层窗口。</description>
        ///         </item>
        ///         <item>
        ///             <term>SW_RESTORE</term>
        ///             <description>激活并显示窗口。如果窗口最小化或最大化，则系统将窗口恢复到原来的尺寸和位置。在恢复最小化窗口时，应用程序应该指定这个标志。</description>
        ///         </item>
        ///         <item>
        ///             <term>SW_SHOW</term>
        ///             <description>在窗口原来的位置以原来的尺寸激活和显示窗口。</description>
        ///         </item>
        ///         <item>
        ///             <term>SW_SHOWDEFAULT</term>
        ///             <description>依据在STARTUPINFO结构中指定的SW_FLAG标志设定显示状态，STARTUPINFO 结构是由启动应用程序的程序传递给CreateProcess函数的。</description>
        ///         </item>
        ///         <item>
        ///             <term>SW_SHOWMAXIMIZED</term>
        ///             <description>激活窗口并将其最大化。</description>
        ///         </item>
        ///         <item>
        ///             <term>SW_SHOWMINIMIZED</term>
        ///             <description>激活窗口并将其最小化。</description>
        ///         </item>
        ///         <item>
        ///             <term>SW_SHOWMINNOACTIVATE</term>
        ///             <description>窗口最小化，激活窗口仍然维持激活状态。</description>
        ///         </item>
        ///         <item>
        ///             <term>SW_SHOWNA</term>
        ///             <description>以窗口原来的状态显示窗口。激活窗口仍然维持激活状态。</description>
        ///         </item>
        ///         <item>
        ///             <term>SW_SHOWNOACTIVATE</term>
        ///             <description>以窗口最近一次的大小和状态显示窗口。激活窗口仍然维持激活状态。</description>
        ///         </item>
        ///         <item>
        ///             <term>SW_SHOWNOMAL</term>
        ///             <description>激活并显示一个窗口。如果窗口被最小化或最大化，系统将其恢复到原来的尺寸和大小。应用程序在第一次显示窗口的时候应该指定此标志。</description>
        ///         </item>
        ///     </list>
        /// </param>
        /// <returns>如果窗口以前可见，则返回值为非零。如果窗口以前被隐藏，则返回值为零。</returns>
        /// <remarks>
        ///     <para>应用程序第一次调用ShowWindow时，应该使用WinMain函数的nCmdshow参数作为它的nCmdShow参数。</para>
        ///     <para>在随后调用ShowWindow函数时，必须使用列表中的一个给定值，而不是由WinMain函数的nCmdSHow参数指定的值。</para>
        ///     <para>正如在nCmdShow参数中声明的，如果调用应用程序的程序使用了在STARTUPINFO结构中指定的信息来显示窗口，则在第一次调用ShowWindow函数时nCmdShow参数就被忽略。</para>
        ///     <para>在这种情况下，ShowWindow函数使用STARTUPINFO结构中的信息来显示窗口。在随后的调用中，应用程序必须调用ShowWindow 函数（将其中nCmdShow参数设为SW_SHOWDEFAULT）来使用由程序调用该应用程序时提供的启动信息。</para>
        ///     <para>这个处理在下列情况下发生：</para>
        ///     <para>应用程序通过调用带WS_VISIBLE标志的函数来创建它们的主窗口函数；</para>
        ///     <para>应用程序通过调用清除了WS_VISIBLE标志的CteateWindow函数来创建主窗口函数，并且随后调用带SW_SHOW标志的ShowWindow函数来显示窗口；</para>
        ///     <para>Windows CE：nCmdShow参数不支持下列值：</para>
        ///     <para>SW_MAXIMINZE；SW_MINIMIZE；SW_RESTORE；SW_SHOWDEFAULT</para>
        ///     <para>SW_SHOWMAXIMIZED；SW_SHOWMINIMIZED；SW_SHOWMININOACTIVATE</para>
        /// </remarks>
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern int ShowWindow(IntPtr hwnd, int nCmdShow);

        /// <summary>
        /// 该函数将创建指定窗口的线程设置到前台，并且激活该窗口。键盘输入转向该窗口，并为用户改各种可视的记号。
        ///     <para>系统给创建前台窗口的线程分配的权限稍高于其他线程。 </para>
        /// </summary>
        /// <param name="hWnd">将被激活并被调入前台的窗口句柄</param>
        /// <returns>如果窗口设入了前台，返回值为非零；如果窗口未被设入前台，返回值为零</returns>
        [DllImport("User32.dll", CharSet = CharSet.Unicode)]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        /// <summary>
        /// 该函数返回前台窗口（用户当前工作的窗口）。系统分配给产生前台窗口的线程一个稍高一点的优先级。
        /// </summary>
        /// <returns>函数返回前台窗回的句柄。</returns>
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern IntPtr GetForegroundWindow();

        /// <summary>
        /// 取得指定窗口的系统菜单的句柄
        /// </summary>
        /// <param name="hWnd">窗口的句柄</param>
        /// <param name="bRevert">如设为TRUE 恢复原始的系统菜单</param>
        /// <returns>如执行成功，返回系统菜单的句柄；零意味着出错。如bRevert设为TRUE，也会返回零</returns>
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        /// <summary>
        /// 该函数使指定的菜单项有效、无效或变灰。
        /// </summary>
        /// <param name="hMenu">菜单句柄。</param>
        /// <param name="uIdEnableItem">指定将使其有效、无效或变灰的菜单项，按参数uEnable确定的含义。此参数可指定菜单条、菜单或子菜单里的菜单项。</param>
        /// <param name="uEnable">指定控制参数uIDEnableltem如何解释的标志，指示菜单项有效、无效或者变灰。
        /// 此参数必须是MF_BYCOMMAND或MF_BYPOSITION，MF_ENABLED和MF_DISABLE或MF_GRAYED的组合。
        /// <para>
        /// MF_BYCOMMAND：表明参数uIDEnableltem给出了菜单项的标识符。如果MF_BYCOMMAND和MF_POSITION都没被指定，则MF_BYCOMMAND为缺省标志。
        /// 
        /// MF_BYPOSITION：表明参数uIDEnableltem给出了菜单项的以零为基准的相对位置。
        /// 
        /// MF_DISABLED：表明菜单项无效，但没变灰，因此不能被选择。
        /// 
        /// MF_ENABLED：表明菜单项有效，并从变灰的状态恢复，因此可被选择。
        /// 
        /// MF_GRAYED：表明菜单项无效并且变灰，因此不能被选择。
        /// </para>
        /// </param>
        /// <returns>返回值指定菜单项的前一个状态（MF_DISABLED，MF_ENABLED或MF_GRAYED）。如果此菜单项不存在，则返回值是OXFFFFFFFF。</returns>
        /// <remarks>
        /// 备注：一个应用程序必须用MF_BYPOSITION来指定正确的菜单句柄。如果菜单条的菜单句柄被指定，顶层菜单项（菜单条上的菜单项）将受到影响。若要根据位置来设置下拉菜单中的菜单项或子菜单的状态，应用程序指定下拉菜单或子菜单的句柄。当应用程序指定MF_BYCOMMAND标志时，系统在由指定菜单句柄标识的菜单里选取那些打开了子菜单的菜单项。因此除非要复制菜单项，指定菜单条的句柄就足够了。函数InsertMenu，InsertMenultem，LoadMenulndirect，ModifyMenu和SetMenultemlnfo也可设置菜单项的状态（有效、无效或变灰）。Windows CE：Windows CE不支持参数uEnable取MF_DISABLED标志。如果没有变灰，菜单项不能无效。要使菜单项无效，用MF_RAYED标志。
        /// 速查：Windows NT：3.1及以上版本;Windows：95的及以上版本；Windows CE：1.0及以上版本；头文件：winuser.h；输入库：user32.lib。
        /// </remarks>
        [DllImport("User32.dll", CharSet = CharSet.Unicode)]
        internal static extern bool EnableMenuItem(IntPtr hMenu, int uIdEnableItem, int uEnable);

        /// <summary>
        /// 该函数从指定菜单里删除一个菜单项。
        ///     <para>如果此菜单项打开了一个菜单或子菜单，则此函数销毁该菜单或子菜单的句柄，并释放该菜单或子菜单使用的存储器。</para>
        /// </summary>
        /// <param name="hMenu">被的修改菜单的句柄</param>
        /// <param name="uPosition">指定将被删除的菜单项，按参数uFlagS确定的含义。</param>
        /// <param name="uFlags">
        ///     确定参数UPosition加如何被解释。此参数可取下列值之一：
        ///     <list type="table">
        ///         <item>
        ///             <term>MF_BYCOMMAND</term>
        ///             <description>
        ///                 表示uPosition给出菜单项的标识符。如果MF_BYCOMMAND和MF_BYPOSITION都没被指定，则MF_BYCOMMAND为缺省的标志。
        ///             </description>
        ///         </item>
        ///         <item>
        ///             <term>MF_BYPOSITION</term>
        ///             <description>
        ///                 表示uPosition给出菜单项基于零的相对位置。
        ///             </description>
        ///         </item>
        ///     </list>
        /// </param>
        /// <remarks>
        /// 只要一个菜单被修改，无论它是否被显示在窗口里，应用程序都应调用DrawMenubar。
        /// </remarks>
        /// <returns>如果函数调用成功，返回值非零；如果函数调用失败，返回值是零。若想获得更多的错误信息， 请调用GetLastError函数。</returns>
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern bool DeleteMenu(IntPtr hMenu, uint uPosition, uint uFlags);

        /// <summary>
        /// 该函数将指定的位图与一个菜单项相联系。无论该菜单项是否被选取，系统都将适当的位图显示在菜单项旁边。
        /// </summary>
        /// <param name="hMenu">其菜单项将接受新选取标记位图的菜单的句柄。</param>
        /// <param name="nPosition">指定将被修改的菜单项。其含义由参数uFlags决定。</param>
        /// <param name="wFlags">指定参数uPosition将如何解释。此参数必须是下列值之一：
        /// <para>
        ///  MF_BYCOMMAND：表示参数uPosition给出菜单项的标识符。如果MF_BYCOMMAND和MF_POSITION都没被指定，则MF_BYCOMMAND是缺省标志。
        ///  MF_BYPOSITION：表示参数uPosition给出菜单项相对于零的位置。
        /// </para>
        /// </param>
        /// <param name="hBitmapUnchecked">当菜单项没被选取时显示的位图的句柄。</param>
        /// <param name="hBitmapChecked">当菜单项被选取时显示的位图的句柄。</param>
        /// <returns></returns>
        /// <remarks> 
        /// 备注：如果参数hBitmapUnchecked或hBitmapChecked的值为NULL，系统将不为相应选取状态显示任何位图到菜单项旁边。如果两参数值均为NULL，系统在菜单项被选取时显示缺省的选取标志位图，菜单项未被选取时删除位图。当菜单项被销毁时，位图并没被销毁，需要应用程序来将其销毁。
        ///
        ///已选取或未选取的位图应当是单色的。系统将用布尔AND运算符组合位图和菜单。这样，位图中白色部分变成透明的，而黑色部分成为菜单项的颜色。如果使用彩色位图，结果会不符合需要。以CXMENUCHECK和CYMENUCHECK来使用函数GetSystemMetrics将取得位图的尺寸。</remarks>
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern int SetMenuItemBitmaps(IntPtr hMenu, IntPtr nPosition, int wFlags, IntPtr hBitmapUnchecked, IntPtr hBitmapChecked);

        /// <summary>
        /// 该函数重画指定菜单的菜单条。如果系统创建窗口以后菜单条被修改，则必须调用此函数来画修改了的菜单条。
        /// </summary>
        /// <param name="hWnd">其菜单条需要被重画的窗口的句柄。</param>
        /// <returns>如果函数调用成功，返回非零值：如果函数调用失败，返回值是零。若想获得更多的错误信息，请调用GetLastError函数。</returns>
        [DllImport("User32.dll", CharSet = CharSet.Unicode)]
        public static extern int DrawMenuBar(IntPtr hWnd);

        /// <summary>
        /// 在菜单的指定位置处插入一个菜单条目，并根据需要将其他条目向下移动
        /// </summary>
        /// <param name="hMenu">菜单的句柄</param>
        /// <param name="wPosition">
        ///     <para>定义了新条目插入点的一个现有菜单条目的标志符。</para>
        ///     <para>如果在wFlags中指定了MF_BYCOMMAND标志，这个参数就代表欲改变的菜单条目的命令ID。</para>
        ///     <para>如设置的是MF_BYPOSITION标志，这个参数就代表菜单条目在菜单中的位置，第一个条目的位置为零</para>
        /// </param>
        /// <param name="wFlags">一系列常数标志的组合</param>
        /// <param name="wIdNewItem">指定菜单条目的新菜单ID。如果在wFlags中指定了MF_POPUP标志，就应该指定弹出式菜单的一个句柄</param>
        /// <param name="lpNewItem">
        ///     <para>如果在wFlags参数中设置了MF_STRING标志，就代表要设置到菜单中的字串（String）。</para>
        ///     <para>如设置的是MF_BITMAP标志，就代表一个Long型变量，其中包含了一个位图句柄 </para>
        /// </param>
        /// <remarks>
        ///     <para>一旦菜单被修改，无论它是否在显示窗口里，应用程序必须调用函数DrawMenuBar。</para>
        ///     <para>下列标志可被设置在参数uFlagS里：</para>
        ///     <para>MF_BITMAP:将一个位图用作菜单项。参数IpNewltem里含有该位图的句柄。</para>
        ///     <para>MF_CHECKED:在菜单项旁边放置一个选取标记。如果应用程序提供一个选取标记位图（参见SetMenultemBitmaps），则将选取标记位图放置在菜单项旁边。</para>
        ///     <para>MF_DISABLED：使菜单项无效，使该项不能被选择，但不使菜单项变灰。</para>
        ///     <para>MF_ENABLED：使菜单项有效，使该项能被选择，并使其从变灰的状态恢复。</para>
        ///     <para>MF_GRAYED：使莱单项无效并变灰，使其不能被选择。</para>
        ///     <para>MF_MENUBARBREAK：对菜单条的功能同MF_MENUBREAK标志。对下拉式菜单、子菜单或快捷菜单，新列和旧列被垂直线分开。</para>
        ///     <para>MF_MENUBREAK：将菜单项放置于新行（对菜单条），或新列（对下拉式菜单、子菜单或快捷菜单）且无分割列。</para>
        ///     <para>MF_OWNERDRAW：指定该菜单项为自绘制菜单项。菜单第一次显示前，拥有菜单的窗口接收一个WM_MEASUREITEM消息来得到菜单项的宽和高。然后，只要菜单项被修改，都将发送WM_DRAWITEM消息给菜单拥有者的窗口程序。</para>
        ///     <para>MF_POPUP:指定菜单打开一个下拉式菜单或子菜单。参数uIDNewltem下拉式菜单或子菜单的句柄。此标志用来给菜单条、打开一个下拉式菜单或子菜单的菜单项、子菜单或快捷菜单加一个名字。</para>
        ///     <para>MF_SEPARATOR：画一条水平区分线。此标志只被下拉式菜单、子菜单或快捷菜单使用。此区分线不能被变灰、无效或加亮。参数IpNewltem和uIDNewltem无用。</para>
        ///     <para>MF_STRING：指定菜单项是一个正文字符串：参数IpNewltem指向该字符串。</para>
        ///     <para>MF_UNCHECKED:不放置选取标记在菜单项旁边（缺省）。如果应用程序提供一个选取标记位图（参见SetMenultemBitmaps），则将选取标记位图放置在菜单项旁边。</para>
        ///     <para>下列标志组不能被一起使用：</para>
        ///     <para>MF_BYCOMMAND和MF_BYPOSITION</para>
        ///     <para>MF_DISABLED，MF_ENABLED和MF_GRAYED</para>
        ///     <para>MF_BITMAJP,MF_STRING，MF_OWNERDRAW和MF_SEPARATOR</para>
        ///     <para>MF_MENUBARBREAK和MF_MENUBREAK</para>
        ///     <para>MF_CHECKED和MF_UNCHECKED</para>
        ///     <para>Windows CE环境下，不支持参数fuFlags使用下列标志：</para>
        ///     <para>MF_BTMAP；MF_DISABLE</para>
        ///     <para>参数项如果没变灰，不能使其无效。要使菜单项无效，用MF_GRAYED标志。</para>
        ///     <para>Windows CE 1．0不支持层叠式菜单。在使用Windows CE 1．0时，不能将一个MF_POPUP菜单插入到另一个下拉式菜单中。</para>
        /// </remarks>
        /// <returns>非零表示成功，零表示失败</returns>
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern bool InsertMenu(IntPtr hMenu, int wPosition, int wFlags, int wIdNewItem, string lpNewItem);

        /// <summary>
        /// 在指定的菜单里添加一个菜单项
        /// </summary>
        /// <param name="hMenu">菜单句柄</param>
        /// <param name="uFlags">控制新菜单项的外观和性能的标志。此参数可以是备注里所列值的组合。</param>
        /// <param name="uIdNewItem">指定菜单条目的新命令ID。如果在wFlags参数中指定了MF_POPUP字段，那么这应该是指向一个弹出式菜单的句柄</param>
        /// <param name="lpNewItem">
        ///     <para>如果在wFlags参数中指定了MF_STRING标志，这就代表在菜单中设置的字串。</para>
        ///     <para>如设置了MF_BITMAP标志，这就代表一个Long型变量，其中包含了一个位图句柄。</para>
        ///     <para>如设置了MF_OWNERDRAW，这个值就会包括在DRAWITEMSTRUCT和MEASUREITEMSTRUCT结构中，在条目需要重画的时候由windows发送出去 </para>
        /// </param>
        /// <remarks>
        ///     <para>一旦菜单被修改，无论它是否在显示窗口里，应用程序必须调用函数DrawMenuBar。</para>
        ///     <para>为了使键盘加速键能控制位留或自己绘制的菜单项，菜单的拥有者必须处理WM_MENUCHAR消息。</para>
        ///     <para>参见自绘制菜单和WM_MENUCHAR消息。</para>
        ///     <para>下列标志可被设置在参数uFlags里：</para>
        ///     <para>MF_BITMAP：将一个位图用作菜单项。参数lpNewltem里含有该位图的句柄。</para>
        ///     <para>MF_CHECKED：在菜单项旁边放置一个选取标记。如果应用程序提供一个选取标记，位图（参见SetMenultemBitmaps），则将选取标记位图放置在菜单项旁边。</para>
        ///     <para>MF_DISABLED：使菜单项无效，使该项不能被选择，但不使菜单项变灰。</para>
        ///     <para>MF_ENABLED：使菜单项有效，使该项能被选择，并使其从变灰的状态恢复。</para>
        ///     <para>MF_GRAYED：使菜单项无效并变灰，使其不能被选择。</para>
        ///     <para>MF_MENUBARBREAK：对菜单条的功能同MF_MENUBREAK标志。对下拉式菜单、子菜单或快捷菜单，新列和旧列被垂直线分开。</para>
        ///     <para>MF_MENUBREAK：将菜单项放置于新行（对菜单条），或新列（对下拉式菜单、子菜单或快捷菜单）且无分割列。</para>
        ///     <para>MF_OWNERDRAW：指定该菜单项为自绘制菜单项。菜单第一次显示前，拥有菜单的窗口接收一个WM_MEASUREITEM消息来得到菜单项的宽和高。然后，只要菜单项被修改，都将发送WM_DRAWITEM消息给菜单拥有者的窗口程序。</para>
        ///     <para>MF_POPUP：指定菜单打开一个下拉式菜单或子菜单。参数uIDNewltem下拉式菜单或子菜单的句柄。此标志用来给菜单条、打开一个下拉式菜单或于菜单的菜单项、子菜单或快捷菜单加一个名字。</para>
        ///     <para>MF_SEPARATOR：画一条水平区分线。此标志只被下拉式菜单、于菜单或快捷菜单使用。此区分线不能被变灰、无效或加亮。参数IpNewltem和uIDNewltem无用。</para>
        ///     <para>MF_STRING：指定菜单项是一个正文字符串；参数lpNewltem指向该字符串。</para>
        ///     <para>MF_UNCHECKED：不放置选取标记在菜单项旁边（缺省）。如果应用程序提供一个选取标记位图（参见SetMenultemBitmaps），则将选取标记位图放置在菜单项旁边。</para>
        ///     <para>下列标志组不能被一起使用：</para>
        ///     <para>MF_DISABLED，MF_ENABLED和MF_GRAYED；MF_BITMAP,MF_STRING和MF_OWNERDRAW</para>
        ///     <para>MF_MENUBARBREAK和MF_MENUBREAK；MF_CHECKED和MF_UNCHECKED</para>
        ///     <para>Windows CE环境下，不支持参数fuFlags使用下列标志：</para>
        ///     <para>MF_BITMAP；MF_DOSABLE；MF_GRAYED</para>
        ///     <para>MF_GRAYED可用来代替MF_DISABLED和MFS_GRAYED。</para>
        ///     <para>Windows CE 1.0不支持层叠式菜单。在使用Windows CE 1.0时，不能将一个MF_POPUP菜单插入到另一个下拉式菜单中。Window CE 1.0不支持下列标志：</para>
        ///     <para>MF_POPUP；MF_MENUBREAK；MF_MENUBARBREAK</para>
        ///     <para>Windows CE 2.0或更高版本中，支持上述标志，也支持层叠式菜单。</para>
        /// </remarks>
        /// <returns>
        ///     如果函数调用成功，返回非零值；如果函数调用失败，返回值是零。若想获得更多的错误信息，请调用GetLastError函数。
        /// </returns>
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern bool AppendMenu(IntPtr hMenu, int uFlags, int uIdNewItem, string lpNewItem);

        /// <summary>
        /// 该函数返回指定窗口的边框矩形的尺寸。该尺寸以相对于屏幕坐标左上角的屏幕坐标给出
        /// </summary>
        /// <param name="hwnd">窗口句柄</param>
        /// <param name="lpRectInfo">向一个RECT结构的指针，该结构接收窗口的左上角和右下角的屏幕坐标</param>
        /// <returns>如果函数成功，返回值为非零：如果函数失败，返回值为零。</returns>
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern int GetWindowRect(IntPtr hwnd, out RectInfo lpRectInfo);

        /// <summary>
        /// 在屏幕的任意地方显示一个弹出式菜单
        /// </summary>
        /// <param name="hMenu">弹出式菜单的句柄</param>
        /// <param name="uFlags">
        ///     位置标志和鼠标追踪标志的组合，见下表
        ///     <list type="table">
        ///         <item>
        ///             <term>TPM_CENTERALIGN</term>
        ///             <description>菜单在指定位置水平居中</description>
        ///         </item>
        ///         <item>
        ///             <term>TPM_LEFTALIGN</term>
        ///             <description>菜单的左侧置于水平x坐标处</description>
        ///         </item>
        ///         <item>
        ///             <term>TPM_RIGHTALIGN</term>
        ///             <description>菜单的右侧置于水平x坐标处 </description>
        ///         </item>
        ///         <item>
        ///             <term>TPM_LEFTBUTTON</term>
        ///             <description>鼠标左键标准运作方式</description>
        ///         </item>
        ///         <item>
        ///             <term>TPM_RIGHTBUTTON</term>
        ///             <description>用鼠标右键进行菜单追踪</description>
        ///         </item>
        ///     </list>
        /// </param>
        /// <param name="x">这个点指定了弹出式菜单在屏幕坐标系统中的位置</param>
        /// <param name="y">这个点指定了弹出式菜单在屏幕坐标系统中的位置</param>
        /// <param name="nReserved">未使用，设为零</param>
        /// <param name="hWnd">用于接收弹出式菜单命令的窗口的句柄。应该使用窗体的窗口句柄——窗体中有一个菜单能象弹出式菜单那样接收相同的命令ID集</param>
        /// <param name="prcRect">用屏幕坐标定义的一个矩形，如用户在这个矩形的范围内单击，则弹出式菜单不会关闭。如单击弹出式菜单之外的任何一个地方，则会关闭菜单。可以设为NULL</param>
        /// <returns>非零表示成功，零表示失败</returns>
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern int TrackPopupMenu(IntPtr hMenu, uint uFlags, int x, int y, int nReserved, IntPtr hWnd,
                                                IntPtr prcRect);

        /// <summary>
        /// 创建一个空的弹出式菜单。可用AppendMenu或InsertMenu函数在窗口中添加条目，或者为一个现成的菜单添加弹出式菜单，并在新建的菜单中添加条目
        /// </summary>
        /// <returns>如成功，返回一个菜单句柄；零意味着错误</returns>
        [DllImport("user32", CharSet = CharSet.Unicode)]
        public static extern IntPtr CreatePopupMenu();

        /// <summary>
        /// 调用一个窗口的窗口函数，将一条消息发给那个窗口。除非消息处理完毕，否则该函数不会返回
        /// </summary>
        /// <param name="hWnd">要接收消息的那个窗口的句柄</param>
        /// <param name="msg">消息的标识符</param>
        /// <param name="wParam">具体取决于消息</param>
        /// <param name="lParam">具体取决于消息</param>
        /// <returns>返回值指定消息处理的结果，依赖于所发送的消息。</returns>
        /// <remarks>
        ///     <para>需要用HWND_BROADCAST通信的应用程序应当使用函数RegisterWindowMessage来为应用程序间的通信取得一个唯一的消息。</para>
        ///     <para>如果指定的窗口是由调用线程创建的，则窗口程序立即作为子程序调用。如果指定的窗口是由不同线程创建的，则系统切换到该线程并调用恰当的窗口程序</para>
        ///     <para>线程间的消息只有在线程执行消息检索代码时才被处理。发送线程被阻塞直到接收线程处理完消息为止。</para>
        ///     <para>Windows CE：Windows CE不支持Windows桌面平台支持的所有消息。使用SendMesssge之前，要检查发送的消息是否被支持。</para>
        /// </remarks>
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern bool SendMessage(IntPtr hWnd, uint msg, int wParam, int lParam);

        /// <summary>
        /// 将一条消息投递到指定窗口的消息队列。投递的消息会在Windows事件处理过程中得到处理。
        ///     <para>在那个时候，会随同投递的消息调用指定窗口的窗口函数。特别适合那些不需要立即处理的窗口消息的发送 </para>
        /// </summary>
        /// <param name="hWnd">
        ///     <para>接收消息的那个窗口的句柄。如设为HWND_BROADCAST，表示投递给系统中的所有顶级窗口。</para>
        ///     <para>如设为零，表示投递一条线程消息（参考PostThreadMessage）</para>
        /// </param>
        /// <param name="msg">消息标识符</param>
        /// <param name="wParam">具体取决于消息</param>
        /// <param name="lParam">具体取决于消息</param>
        /// <returns>如果函数调用成功，返回非零值：如果函数调用失败，返回值是零。</returns>
        /// <remarks>
        ///     <para>需要以 HWND_BROADCAST方式通信的应用程序应当用函数 RegisterwindwosMessage来获得应用程序间通信的独特的消息。</para>
        ///     <para>如果发送一个低于WM_USER范围的消息给异步消息函数（PostMessage.SendNotifyMessage，SendMesssgeCallback），消息参数不能包含指针。</para>
        ///     <para>否则，操作将会失败。函数将再接收线程处理消息之前返回，发送者将在内存被使用之前释放。</para>
        /// </remarks>
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern bool PostMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        /// <summary>
        /// 该函数合成一次击键事件。
        ///     <para>系统可使用这种合成的击键事件来产生WM_KEYUP或WM_KEYDOWN消息，键盘驱动程序的中断处理程序调用keybd_event函数。</para>
        ///     <para>在Windows NT中该函数己被使用Sendlhput来替代它。</para>
        /// </summary>
        /// <param name="bVk">定义一个虚据拟键码。键码值必须在1～254之间。
        /// //VK_LBUTTON      01      Left mouse button 
        /// //VK_RBUTTON      02      Right mouse button 
        /// //VK_CANCEL       03      Used for control-break processing 
        /// //VK_MBUTTON      04      Middle mouse button (three-button mouse) 
        /// //--              05-07   Undefined 
        /// //VK_BACK         08      BACKSPACE key 
        /// //VK_TAB          09      TAB key 
        /// //--              0A0B    Undefined 
        /// //VK_CLEAR        0C      CLEAR key 
        /// //VK_RETURN       0D      ENTER key 
        /// //--              0E0F    Undefined 
        /// //VK_SHIFT        10      SHIFT key 
        /// //VK_CONTROL      11      CTRL key 
        /// //VK_MENU         12      ALT key 
        /// //VK_PAUSE        13      PAUSE key 
        /// //VK_CAPITAL      14      CAPS LOCK key 
        /// //--              15-19   Reserved for Kanji systems 
        /// //--              1A      Undefined 
        /// //VK_ESCAPE       1B      ESC key 
        /// //--              1C1F    Reserved for Kanji systems 
        /// //VK_SPACE        20      SPACEBAR 
        /// //VK_PRIOR        21      PAGE UP key 
        /// //VK_NEXT         22      PAGE DOWN key 
        /// //VK_END          23      END key 
        /// //VK_HOME         24      HOME key 
        /// //VK_LEFT         25      LEFT ARROW key 
        /// //VK_UP           26      UP ARROW key 
        /// //VK_RIGHT        27      RIGHT ARROW key 
        /// //VK_DOWN         28      DOWN ARROW key 
        /// //VK_SELECT       29      SELECT key 
        /// //--              2A      OEM specific 
        /// //VK_EXECUTE      2B      EXECUTE key 
        /// //VK_SNAPSHOT     2C      PRINT SCREEN key for Windows 3.0 and later 
        /// //VK_INSERT       2D      INS key 
        /// //VK_DELETE       2E      DEL key 
        /// //VK_HELP         2F      HELP key 
        /// //VK_0            30      0 key 
        /// //VK_1            31      1 key 
        /// //VK_2            32      2 key 
        /// //VK_3            33      3 key 
        /// //VK_4            34      4 key 
        /// //VK_5            35      5 key 
        /// //VK_6            36      6 key 
        /// //VK_7            37      7 key 
        /// //VK_8            38      8 key 
        /// //VK_9            39      9 key 
        /// //--              3A40    Undefined 
        /// //VK_A            41      A key 
        /// //VK_B            42      B key 
        /// //VK_C            43      C key 
        /// //VK_D            44      D key 
        /// //VK_E            45      E key 
        /// //VK_F            46      F key 
        /// //VK_G            47      G key 
        /// //VK_H            48      H key 
        /// //VK_I            49      I key 
        /// //VK_J            4A      J key 
        /// //VK_K            4B      K key 
        /// //VK_L            4C      L key 
        /// //VK_M            4D      M key 
        /// //VK_N            4E      N key 
        /// //VK_O            4F      O key 
        /// //VK_P            50      P key 
        /// //VK_Q            51      Q key 
        /// //VK_R            52      R key 
        /// //VK_S            53      S key 
        /// //VK_T            54      T key 
        /// //VK_U            55      U key 
        /// //VK_V            56      V key 
        /// //VK_W            57      W key 
        /// //VK_X            58      X key 
        /// //VK_Y            59      Y key 
        /// //VK_Z            5A      Z key 
        /// //--              5B5F    Undefined 
        /// //VK_NUMPAD0      60      Numeric keypad 0 key 
        /// //VK_NUMPAD1      61      Numeric keypad 1 key 
        /// //VK_NUMPAD2      62      Numeric keypad 2 key 
        /// //VK_NUMPAD3      63      Numeric keypad 3 key 
        /// //VK_NUMPAD4      64      Numeric keypad 4 key 
        /// //VK_NUMPAD5      65      Numeric keypad 5 key 
        /// //VK_NUMPAD6      66      Numeric keypad 6 key 
        /// //VK_NUMPAD7      67      Numeric keypad 7 key 
        /// //VK_NUMPAD8      68      Numeric keypad 8 key 
        /// //VK_NUMPAD9      69      Numeric keypad 9 key 
        /// //VK_MULTIPLY     6A      Multiply key 
        /// //VK_ADD  6B      Add key 
        /// //VK_SEPARATOR    6C      Separator key 
        /// //VK_SUBTRACT     6D      Subtract key 
        /// //VK_DECIMAL      6E      Decimal key 
        /// //VK_DIVIDE       6F      Divide key 
        /// //VK_F1           70      F1 key 
        /// //VK_F2           71      F2 key 
        /// //VK_F3           72      F3 key 
        /// //VK_F4           73      F4 key 
        /// //VK_F5           74      F5 key 
        /// //VK_F6           75      F6 key 
        /// //VK_F7           76      F7 key 
        /// //VK_F8           77      F8 key 
        /// //VK_F9           78      F9 key 
        /// //VK_F10          79      F10 key 
        /// //VK_F11          7A      F11 key 
        /// //VK_F12          7B      F12 key 
        /// //VK_F13          7C      F13 key 
        /// //VK_F14          7D      F14 key 
        /// //VK_F15          7E      F15 key 
        /// //VK_F16          7F      F16 key 
        /// //VK_F17          80H     F17 key 
        /// //VK_F18          81H     F18 key 
        /// //VK_F19          82H     F19 key 
        /// //VK_F20          83H     F20 key 
        /// //VK_F21          84H     F21 key 
        /// //VK_F22          85H     F22 key 
        /// //VK_F23          86H     F23 key 
        /// //VK_F24          87H     F24 key 
        /// //--              88-8F   Unassigned 
        /// //VK_NUMLOCK      90      NUM LOCK key 
        /// //VK_SCROLL       91      SCROLL LOCK key 
        /// </param>
        /// <param name="bScan">定义该键的硬件扫描码。</param>
        /// <param name="dwFlags">
        ///     定义函数操作的名个方面的一个标志位集。应用程序可使用如下一些预定义常数的组合设置标志位。
        ///     <list type="table">
        ///         <item>
        ///             <term>KEYEVENTF_EXETENDEDKEY</term>
        ///             <description>
        ///                 若指定该值，则扫描码前一个值为OXEO（224）的前缀字节。DEYEVENTF_KEYUP：
        ///                 <para>若指定该值，该键将被释放；若未指定该值，该键交被接下。</para>
        ///             </description>
        ///         </item>
        ///     </list>
        /// </param>
        /// <param name="dwExtraInfo">定义与击键相关的附加的32位值。</param>
        /// <remarks>
        /// //模拟键盘操作
        /// //Win32Helper.keybd_event(VK_RWIN, 0, KEYEVENTF_EXTENDEDKEY, 0);
        /// //Win32Helper.keybd_event(VK_PAUSE, 0, KEYEVENTF_EXTENDEDKEY, 0);
        /// //Win32Helper.keybd_event(VK_PAUSE, 0, KEYEVENTF_KEYUP, 0);
        /// //Win32Helper.keybd_event(VK_RWIN, 0, KEYEVENTF_KEYUP, 0);
        /// const byte VK_LWIN = 0x5B;
        /// const byte VK_RWIN = 0x5C;
        /// const byte VK_PAUSE = 0x13;
        /// 
        /// const byte VK_D = 0x44;
        /// const byte VK_R = 0x52;
        /// const byte KEYEVENTF_KEYUP = 0x2;
        /// const byte KEYEVENTF_EXTENDEDKEY = 0x1;
        /// </remarks>
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern void keybd_event(byte bVk, byte bScan, long dwFlags, long dwExtraInfo);

        /// <summary>
        /// 激活键盘布局。
        ///     <para>在Windows NT中ActivateKeyboadLayout函数激活一种不同的键盘布局，同时在整个系统中而不仅仅是调用该函数的进程中将该键盘布局设为活动的。</para>
        /// </summary>
        /// <param name="hkl">
        ///     将被激活的键盘布局的句柄。该布局必须先调用LeadKeyboadLayout函数装入，该参数必须是键盘分局的句柄，或是如下的值中的一种：
        ///     <list type="table">
        ///         <item>
        ///             <term>HKL_NEXT</term>
        ///             <description>在系统保持的，己装入的布局的循环链表中，选择下一布局。</description>
        ///         </item>
        ///         <item>
        ///             <term>HKL_PREV</term>
        ///             <description>在系统保持的，已装入的布局的循环链表中，选择前一布局。</description>
        ///         </item>
        ///     </list>
        /// </param>
        /// <param name="flags">
        ///     <list type="table">
        ///         <item>
        ///             <term>LFREORDER</term>
        ///             <description>
        ///                 <para>若该位被设置，则已装入的键盘布局的循环链路表将被重新排序。若该位没有设置，则循环链路表的顺序不变。例如，若用户激活了英语键盘布局，同时依序装入了法语、德语、西班牙语键盘布局，然后通过设置KLF_REORDE位激活德语键盘布局，则会产生如下顺序：德语、英语、法语、西牙语键盘布局。</para>
        ///                 <para>若激活德语键盘布局时未设置KLF_REORDER位，则产生如下的键盘布局的键盘布局序列：德语、西班牙语、英语、法语。若装入的键盘布局少于三种，则该标志域的值不起作用。</para>
        ///             </description>
        ///         </item>
        ///         <item>
        ///             <term>KLF_SETFORPROCESS</term>
        ///             <description>在Windows NT 5.0以上版本中使用。该参数用于整个进程中激活指定的键盘布，并向当前进程的所有线程发送WM_INPUTLANGCHANGE消息。</description>
        ///         </item>
        ///         <item>
        ///             <term>KLF_UNLOADPREVLOUS</term>
        ///             <description>卸载先前活动的键盘布局。</description>
        ///         </item>
        ///     </list>
        /// </param>
        /// <remarks>在任一时刻可以装入多种键盘布局，但一次仅能激活一种布局。装入多种键盘布局使得可以快速地在多种布局之间切换。Windows 95 ACtivateKeyboadLayout函数为当前线程设置输入语言。该函数接受一个键盘布局句柄，该句柄标识键盘的一个局部的和物理布局。</remarks>
        /// <returns>如果函数调用成功，返回值为前一键盘布局的句柄。否则，返回值为零。若要获得更多多错误信息，可调用GetLastError函数。</returns>
        [DllImport("user32", CharSet = CharSet.Unicode)]
        public static extern IntPtr ActivateKeyboardLayout(uint hkl, uint flags);

        /// <summary>
        /// 该函数给系统中装入一种新的键盘布局，可以同时装入几种不同的键盘布局，
        ///     <para>任一时刻仅有一个进程是活动的，装入多个键盘布局使得在多种布局间快速切换。</para>
        /// </summary>
        /// <param name="pwszKlid">
        ///     <para>缓冲区中的存放装入的键盘布局名称，名称是由语言标识符（低位字）和设备标识符（高位字）组成的十六进制值串，</para>
        ///     <para>例如 U.S.英语对应的语言标识符为DX0409，则基本的U.S.英语键盘布局命名为“0000409”。U.S.英语键盘布局的变种（例如Dvorak布局）命名为“00010409”，“00020409”等。</para>
        /// </param>
        /// <param name="flags">
        ///     <list type="table">
        ///         指定如何装入键盘布局，该参数可以是如下的值。
        ///         <item>
        ///             <term>KLF_ACTIVATE</term>
        ///             <description>若指定布局尚未装入，该函数为当前线程装入并激活它。</description>
        ///         </item>
        ///         <item>
        ///             <term>KLF_NOTELLSHELL</term>
        ///             <description>
        ///                 <para>当装入新的键盘布局时，禁止一个ShellProe过程接收一个HSHELL_LANGUAGE代码。</para>
        ///                 <para>当应用程序依次装入多个键盘布局时，对除最后一个键盘布局外的所有键盘布局使用该值，将会延迟Shell的处理直到所有的键盘布局均己被装入。</para>
        ///             </description>
        ///         </item>
        ///         <item>
        ///             <term>KLF_RECOROER</term>
        ///             <description>将指定键盘布局移动到布局表的头部，使得对于当前线程，该布局的活动的。若不提供DLF_ACTIVATE值，则该值记录键盘布局表。</description>
        ///         </item>
        ///         <item>
        ///             <term>KLF_REPLACE_LANG</term>
        ///             <description>
        ///                 <para>Windows NT 4.0或Windows 95以上支持，若新布局与当前布局有同样的语言标识符，那么新布局替代当前布局作为那种语言的键盘布局</para>
        ///                 <para>若未提供该值，而键盘布局又有同样的标识符，则当前布局不被替换，函数返回NULL值。</para>
        ///             </description>
        ///         </item>
        ///         <item>
        ///             <term>KLF_SUBSTITUTE_OK</term>
        ///             <description>
        ///                 <para>用用户喜欢的键盘布局来替换给定布局，系统初始时设置该标志，并且建议始终设置该标志，仅当在注册HKEY_CURRENT_USER/Keyboard Layout/Substitate下定义了一个替代布局时，才发生替换。</para>
        ///                 <para>例如，在名为00000409的部分中有一个多于00010409的值，则设置该标志装入U.S.英语键盘布局会导致Dvorak US.英语键盘布局的装入。系统引导时使用该参数，建议在所有应用程序装入键盘布局时使用该值，以确保用户喜欢的键盘布局被选取。</para>
        ///             </description>
        ///         </item>
        ///         <item>
        ///             <term>KLF_SETFORPROCESS</term>
        ///             <description>Windows NT 5.0该位仅法与KLF_ACTIVATE一起使用时才有效，为整个进程激活指定键盘布局，且发送WM_INPUTLANGCHANGE消息以当前进程的所有线程。典型的LoadKeyboardLayWut仅为当前线程激活一个键盘布局。</description>
        ///         </item>
        ///         <item>
        ///             <term>KLF_UNLOADPREVIOS</term>
        ///             <description>WindowsNT5.0，Windows95，Windows98都不支持，仅当与KLF_ACTIVATE一起使用时才有效，仅当装入且激活指定键盘布局成功，先前的布局才能被卸载，建议使用unLoadKeyboardLayout函数。</description>
        ///         </item>
        ///     </list>
        /// </param>
        /// <remarks>
        ///     <para>应用程序可以通过仅定义语言标识符的串来装入该语言的IME向缺省键盘布局。</para>
        ///     <para>若应用程序想装入IME的指定键盘布局，就必须读注册信息以确定传递给LoadKeyboardLayout返回的键盘布局句柄来激活。</para>
        ///     <para>Windows 95和Windows 98：若装载与原先键盘布局使用同种语言的布局，且KLF_REPLACELANG标志未被设置，</para>
        ///     <para>则函数调用失败，仅有一个键盘布局可与给定语言相关联。（对于装载与同一语言相关的多IME也是可接受的）。</para>
        /// </remarks>
        /// <returns>若函数调用成功，返回与要求的名字匹配的键盘布局句柄。若没有匹配的布局，则返回NULL。</returns>
        [DllImport("user32", CharSet = CharSet.Unicode)]
        public static extern IntPtr LoadKeyboardLayout(string pwszKlid, uint flags);

        /// <summary>
        /// 该函数可以获得与系统中输入点的当前集相对应的键盘布局句柄。该函数将句柄拷贝到指定的缓冲区中。
        /// </summary>
        /// <param name="nBuff">指定缓冲区中可以存放的最大句柄数目。</param>
        /// <param name="lpList">缓冲区指针，缓冲区中存放着键盘布局句柄数组。</param>
        /// <returns>
        ///     <para>函数调用成功，则返回值为拷贝到缓冲区的键盘布局句柄的数目。</para>
        ///     <para>或者，若nBuff为0，则运回值为接受所有当前键盘布局的缓冲区中的大小（以数组成员为单位）。</para>
        ///     <para>若函数调用失败，返回值为0。若想获得更多错误信息，可调用GetLastError函数。</para>
        /// </returns>
        [DllImport("user32", CharSet = CharSet.Unicode)]
        public static extern int GetKeyboardLayoutList(int nBuff, uint[] lpList);

        /// <summary>
        /// 获取当前进程的一个句柄
        /// </summary>
        /// <returns>返回当前进程的句柄</returns>
        [DllImport("kernel32.dll", ExactSpelling = true)]
        public static extern IntPtr GetCurrentProcess();

        /// <summary>
        /// 用来退出、重启或注销系统。
        /// </summary>
        /// <param name="uFlags">
        ///     指定关闭的类型。此参数必须有下列值的组合：
        ///     <list type="table">
        ///         <item>
        ///             <term>EWX_LOGOFF</term>
        ///             <description>关闭所有进程，然后注销用户。</description>
        ///         </item>
        ///         <item>
        ///             <term>EWX_SHUTDOWN</term>
        ///             <description>
        ///                 关闭系统，安全地关闭电源。所有文件缓冲区已经刷新到磁盘上，所有正在运行的进程已经停止。
        ///                 <para>Windows要求：</para>
        ///                 <para>Windows NT中：调用进程必须有SE_SHUTDOWN_NAME特权。</para>
        ///                 <para>Windows 9X中：可以直接调用。</para>
        ///             </description>
        ///         </item>
        ///         <item>
        ///             <term>EWX_REBOOT</term>
        ///             <description>
        ///                 关闭系统，然后重新启动系统。
        ///                 <para>Windows要求：</para>
        ///                 <para>Windows NT中：调用进程必须有SE_SHUTDOWN_NAME特权。</para>
        ///                 <para>Windows 9X中：可以直接调用。</para>
        ///             </description>
        ///         </item>
        ///         <item>
        ///             <term>EWX_FORCE</term>
        ///             <description>强制终止进程。当此标志设置，Windows不会发送消息WM_QUERYENDSESSION和WM_ENDSESSION的消息给目前在系统中运行的程序。这可能会导致应用程序丢失数据。因此，你应该只在紧急情况下使用此标志。</description>
        ///         </item>
        ///         <item>
        ///             <term>EWX_POWEROFF</term>
        ///             <description>
        ///                 关闭系统并关闭电源。该系统必须支持断电。
        ///                 <para>Windows要求：</para>
        ///                 <para>Windows NT中调用进程必须有 SE_SHUTDOWN_NAME 特权。</para>
        ///                 <para>Windows 9X中：可以直接调用。</para>
        ///             </description>
        ///         </item>
        ///         <item>
        ///             <term>EWX_FORCEIFHUNG</term>
        ///             <description>Forces processes to terminate if they do not respond to the WM_QUERYENDSESSION or WM_ENDSESSION message within the timeout interval. For more information, see the Remarks.</description>
        ///         </item>
        ///     </list>
        /// </param>
        /// <param name="dwReserved">系统保留，一般取0</param>
        /// <returns>如果函数成功，返回值为非零。如果函数失败，返回值是零。</returns>
        /// <remarks>
        /// 在关机或登录操作中，应用程序在允许关闭的时间具体数额内回应关机请求。如果时间到期时，Windows会显示一个对话框，允许用户强行关闭应用程序：关闭、重试，或取消关机要求。如果存在EWX_FORCE指定值，Windows会关闭应用程序而不显示该对话框。
        /// </remarks>
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern int ExitWindowsEx(int uFlags, int dwReserved);

        /// <summary>
        /// 锁定计算机
        /// </summary>
        [DllImport("User32.DLL", CharSet = CharSet.Unicode)]
        public static extern void LockWorkStation();

        /// <summary>
        /// 该函数检取光标的位置，以屏幕坐标表示。
        /// </summary>
        /// <param name="lpPointInfo">POINT结构指针，该结构接收光标的屏幕坐标。</param>
        /// <returns>如果成功，返回值非零；如果失败，返回值为零。</returns>
        /// <remarks>光标的位置通常以屏幕坐标的形式给出，它并不受包含该光标的窗口的映射模式的影响。该调用过程必须具有对窗口站的WINSTA_READATTRIBUTES访问权限。</remarks>
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern int GetCursorPos(out PointInfo lpPointInfo);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern bool SetCursorPos(int x, int y);

        /// <summary>
        /// 获取上次输入操作的时间。
        /// </summary>
        /// <param name="plii">一个指向接收到最后一个输入事件时间的LASTINPUTINFO结构指针。</param>
        /// <returns>如果调用函数成功，返回值为非零。如果调用函数失败，返回值为零。</returns>
        /// <remarks>此函数用来检测系统的输入空闲时间，然而GetLastInputInfo不提供全系统所有正在运行的会话用户输入信息。相反，GetLastInputInfo 仅提供调用的会话功能会话特定的用户输入的信息。</remarks>
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern bool GetLastInputInfo(out Lastinputinfo plii);

        /// <summary>
        /// 获取上次键盘或者鼠标移动到现在的毫秒数
        /// </summary>
        /// <returns>返回上次键盘或者鼠标移动到现在的毫秒数</returns>
        public static long GetLastInputTime()
        {
            Lastinputinfo structure = new Lastinputinfo();
            structure.cbSize = Marshal.SizeOf(structure);
            if (!GetLastInputInfo(out structure))
            {
                return 0L;
            }
            return (Environment.TickCount - structure.dwTime);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="process"></param>
        /// <param name="minSize"></param>
        /// <param name="maxSize"></param>
        /// <returns></returns>
        [DllImport("kernel32.dll", EntryPoint = "SetProcessWorkingSetSize")]
        public static extern int SetProcessWorkingSetSize(IntPtr process, int minSize, int maxSize);

        /// <summary>
        /// 设置本地日期时间(Win32API函数)
        /// </summary>
        /// <param name="lpSystemTime">日期时间结构体</param>
        /// <returns>成功返回true</returns>
        [DllImport("Kernel32.dll", CharSet = CharSet.Unicode)]
        public static extern bool SetLocalTime(ref Systemtime lpSystemTime);

        /// <summary>
        /// 设置本地日期时间
        /// </summary>
        /// <param name="dataTime">日期时间</param>
        public static void SetLocalDateTime(DateTime dataTime)
        {
            Systemtime dt = new Systemtime
            {
                wYear = ushort.Parse(dataTime.Year.ToString(CultureInfo.InvariantCulture)),
                wMonth = ushort.Parse(dataTime.Month.ToString(CultureInfo.InvariantCulture)),
                wDay = ushort.Parse(dataTime.Day.ToString(CultureInfo.InvariantCulture)),
                wHour = ushort.Parse(dataTime.Hour.ToString(CultureInfo.InvariantCulture)),
                wMinute = ushort.Parse(dataTime.Minute.ToString(CultureInfo.InvariantCulture)),
                wSecond = ushort.Parse(dataTime.Second.ToString(CultureInfo.InvariantCulture))
            };
            SetLocalTime(ref dt);
        }

        /// <summary>
        /// 获取本地日期时间
        /// </summary>
        /// <param name="sysTime">日期时间结构体</param>
        [DllImport("Kernel32.dll", CharSet = CharSet.Unicode)]
        public static extern void GetLocalTime(ref Systemtime sysTime);

        /// <summary>
        /// 该函数改变一个子窗口，弹出式窗口式顶层窗口的尺寸，位置和Z序。子窗口，弹出式窗口，及顶层窗口根据它们在屏幕上出现的顺序排序、顶层窗口设置的级别最高，并且被设置为Z序的第一个窗口。
        /// </summary>
        /// <param name="hWnd">窗口句柄。</param>
        /// <param name="hWndInsertAfter">
        ///     在z序中的位于被置位的窗口前的窗口句柄。该参数必须为一个窗口句柄，或下列值之一：
        ///     <list type="table">
        ///         <item>
        ///             <term>HWND_BOTTOM</term>
        ///             <description>将窗口置于Z序的底部。如果参数hWnd标识了一个顶层窗口，则窗口失去顶级位置，并且被置在其他窗口的底部。</description>
        ///         </item>
        ///         <item>
        ///             <term>HWND_DOTTOPMOST</term>
        ///             <description>将窗口置于所有非顶层窗口之上（即在所有顶层窗口之后）。如果窗口己经是非顶层窗口则该标志不起作用。</description>
        ///         </item>
        ///         <item>
        ///             <term>HWND_TOP</term>
        ///             <description>将窗口置于Z序的顶部。</description>
        ///         </item>
        ///         <item>
        ///             <term>HWND_TOPMOST</term>
        ///             <description>将窗口置于所有非顶层窗口之上。即使窗口未被激活窗口也将保持顶级位置。</description>
        ///         </item>
        ///     </list>
        ///     <para>查g看该参数的使用方法，请看说明部分。</para>
        /// </param>
        /// <param name="x">以客户坐标指定窗口新位置的左边界。</param>
        /// <param name="y">以客户坐标指定窗口新位置的顶边界。</param>
        /// <param name="cx">以像素指定窗口的新的宽度。</param>
        /// <param name="cy">以像素指定窗口的新的高度。</param>
        /// <param name="uFlags">
        ///     窗口尺寸和定位的标志。该参数可以是下列值的组合：
        ///     <list type="table">
        ///         <item>
        ///             <term>SWP_ASNCWINDOWPOS</term>
        ///             <description>如果调用进程不拥有窗口，系统会向拥有窗口的线程发出需求。这就防止调用线程在其他线程处理需求的时候发生死锁。</description>
        ///         </item>
        ///         <item>
        ///             <term>SWP_DEFERERASE</term>
        ///             <description>防止产生WM_SYNCPAINT消息。</description>
        ///         </item>
        ///         <item>
        ///             <term>SWP_DRAWFRAME</term>
        ///             <description>在窗口周围画一个边框（定义在窗口类描述中）。</description>
        ///         </item>
        ///         <item>
        ///             <term>SWP_FRAMECHANGED</term>
        ///             <description>给窗口发送WM_NCCALCSIZE消息，即使窗口尺寸没有改变也会发送该消息。如果未指定这个标志，只有在改变了窗口尺寸时才发送WM_NCCALCSIZE。</description>
        ///         </item>
        ///         <item>
        ///             <term>SWP_HIDEWINDOW</term>
        ///             <description>隐藏窗口。</description>
        ///         </item>
        ///         <item>
        ///             <term>SWP_NOACTIVATE</term>
        ///             <description>不激活窗口。如果未设置标志，则窗口被激活，并被设置到其他最高级窗口或非最高级组的顶部（根据参数hWndlnsertAfter设置）。</description>
        ///         </item>
        ///         <item>
        ///             <term>SWP_NOCOPYBITS</term>
        ///             <description>清除客户区的所有内容。如果未设置该标志，客户区的有效内容被保存并且在窗口尺寸更新和重定位后拷贝回客户区。</description>
        ///         </item>
        ///         <item>
        ///             <term>SWP_NOMOVE</term>
        ///             <description>维持当前位置（忽略X和Y参数）。</description>
        ///         </item>
        ///         <item>
        ///             <term>SWP_NOOWNERZORDER</term>
        ///             <description>不改变z序中的所有者窗口的位置。</description>
        ///         </item>
        ///         <item>
        ///             <term>SWP_NOREDRAW</term>
        ///             <description>不重画改变的内容。如果设置了这个标志，则不发生任何重画动作。适用于客户区和非客户区（包括标题栏和滚动条）和任何由于窗回移动而露出的父窗口的所有部分。如果设置了这个标志，应用程序必须明确地使窗口无效并区重画窗口的任何部分和父窗口需要重画的部分。</description>
        ///         </item>
        ///         <item>
        ///             <term>SWP_NOREPOSITION</term>
        ///             <description>与SWP_NOOWNERZORDER标志相同。</description>
        ///         </item>
        ///         <item>
        ///             <term>SWP_NOSENDCHANGING</term>
        ///             <description>防止窗口接收WM_WINDOWPOSCHANGING消息。</description>
        ///         </item>
        ///         <item>
        ///             <term>SWP_NOSIZE</term>
        ///             <description>维持当前尺寸（忽略cx和Cy参数）。</description>
        ///         </item>
        ///         <item>
        ///             <term>SWP_NOZORDER</term>
        ///             <description>维持当前Z序（忽略hWndlnsertAfter参数）。</description>
        ///         </item>
        ///         <item>
        ///             <term>SWP_SHOWWINDOW</term>
        ///             <description>显示窗口。</description>
        ///         </item>
        ///     </list>
        /// </param>
        /// <returns>如果函数成功，返回值为非零；如果函数失败，返回值为零。</returns>
        /// <remarks>
        ///     如果设置了SWP_SHOWWINDOW和SWP_HIDEWINDOW标志，则窗口不能被移动和改变大小。如果使用SetWindowLoog改变了窗口的某些数据，则必须调用函数SetWindowPos来作真正的改变。使用下列的组合标志：SWP_NOMOVEISWP_NOSIZEISWP_FRAMECHANGED。
        ///     <para>有两种方法将窗口设为最顶层窗口：一种是将参数hWndlnsertAfter设置为HWND_TOPMOST并确保没有设置SWP_NOZORDER标志；另一种是设置窗口在Z序中的位置以使其在其他存在的窗口之上。当一个窗口被置为最顶层窗口时，属于它的所有窗口均为最顶层窗口，而它的所有者的z序并不改变。</para>
        ///     <para>如果HWND_TOPMOST和HWND_NOTOPMOST标志均未指定，即应用程序要求窗口在激活的同时改变其在Z序中的位置时，在参数hWndinsertAfter中指定的值只有在下列条件中才使用：</para>
        ///     <para>在hWndlnsertAfter参数中没有设定HWND_NOTOPMOST和HWND_TOPMOST标志。</para>
        ///     <para>由hWnd参数标识的窗口不是激活窗口。</para>
        ///     <para>如果未将一个非激活窗口设定到z序的顶端，应用程序不能激活该窗口。应用程序可以无任何限制地改变被激活窗口在Z序中的位置，或激活一个窗口并将其移到最高级窗口的顶部或非最高级窗口的顶部。</para>
        ///     <para>如果一个顶层窗口被重定位到z序的底部（HWND_BOTTOM）或在任何非最高序的窗口之后，该窗口就不再是最顶层窗口。当一个最顶层窗口被置为非最顶级，则它的所有者窗口和所属者窗口均为非最顶层窗口。</para>
        ///     <para>一个非最顶端窗口可以拥有一个最顶端窗口，但反之则不可以。任何属于顶层窗口的窗口（例如一个对话框）本身就被置为顶层窗口，以确保所有被属窗口都在它们的所有者之上。</para>
        ///     <para>如果应用程序不在前台，但应该位于前台，就应调用SetForegroundWindow函数来设置。</para>
        ///     <para>Windows CE：如果这是一个可见的顶层窗口，并且未指定SWP_NOACTIVATE标志，则这个函数将激活窗口、如果这是当前的激活窗口，并且指定了SWP_NOACTIVATE或SWP_HIDEWINDOW标志，则激活另外一个可见的顶层窗口。</para>
        ///     <para>当在这个函数中的nFlags参数里指定了SWP_FRAMECHANGED标志时，WindowsCE重画窗口的整个非客户区，这可能会改变客户区的大小。这也是重新计算客户区的唯一途径，也是通过调用SetwindowLong函数改变窗口风格后通常使用的方法。</para>
        ///     <para>SetWindowPos将使WM_WINDOWPOSCHANGED消息向窗口发送，在这个消息中传递的标志与传递给函数的相同。这个函数不传递其他消息。</para>
        ///     <para>Windows CE 1.0不支持在hWndlnsertAber参数中的HWND_TOPMOST和HWND_NOTOPMOST常量。</para>
        ///     <para>Windows CE1.0不支持在fuFags参数中的SWP_DRAWFRAME和SWP_NOCOPYBITS标志。</para>
        /// </remarks>
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy,
                                               uint uFlags);

        /// <summary>
        /// 此函数返回本地系统的网络连接状态。
        /// </summary>
        /// <param name="connectionDescription">
        ///     指向一个变量，该变量接收连接描述内容。该参数在函数返回FLASE时仍可以返回一个有效的标记。该参数可以为下列值的一个或多个。
        ///     <list type="table">
        ///         <item>
        ///             <term>INTERNET_CONNECTION_CONFIGURED 0x40</term>
        ///             <description>Local system has a valid connection to the Internet, but it might or might not be currently connected.</description>
        ///         </item>
        ///         <item>
        ///             <term>INTERNET_CONNECTION_LAN 0x02</term>
        ///             <description>Local system uses a local area network to connect to the Internet.</description>
        ///         </item>
        ///         <item>
        ///             <term>INTERNET_CONNECTION_MODEM 0x01</term>
        ///             <description>Local system uses a modem to connect to the Internet.</description>
        ///         </item>
        ///         <item>
        ///             <term>INTERNET_CONNECTION_MODEM_BUSY 0x08</term>
        ///             <description>No longer used.</description>
        ///         </item>
        ///         <item>
        ///             <term>INTERNET_CONNECTION_OFFLINE 0x20</term>
        ///             <description>Local system is in offline mode.</description>
        ///         </item>
        ///         <item>
        ///             <term>INTERNET_CONNECTION_PROXY 0x04</term>
        ///             <description>Local system uses a proxy server to connect to the Internet.</description>
        ///         </item>
        ///         <item>
        ///             <term>INTERNET_RAS_INSTALLED 0x10</term>
        ///             <description>Local system has RAS installed.</description>
        ///         </item>
        ///     </list>
        /// </param>
        /// <param name="reservedValue">保留值。必须为0。</param>
        /// <returns>当存在一个modem或一个LAN连接时，返回TRUE，当不存在internet连接或所有的连接当前未被激活时，返回false。</returns>
        /// <remarks>
        ///     该函数如果返回TRUE，表明至少有一个连接是有效的。它并不能保证这个有效的连接是连向一个指定的主机。程序应该经常检查利用API连接到服务器的返回错误代码，用以判断连接状态。使用InternetCheckConnection函数可以判断一个连接到指定主机的连接是否建立。
        ///     <para>返回值为TRUE也表明一个modem连接处于激活状态或一个LAN连接处于激活状态。而FALSE代表modem和LAN均不处于连接状态。如果返回FALSE，INTERNET_CONNECTION_CONFIGURED 标识将被设置，以表明自动拨号被设置为“总是拨号”，但当前不处于激活状态。如果自动拨号未被设置，函数返回FALSE。</para>
        ///     <para>正像WinINet API的其他其它函数，此函数不能从DLLMain或者全局构造函数、析构函数安全调用。</para>
        ///     <para>WinINet不支持实现服务器。另外，它也不应该用来作为服务。实现服务器或者服务可用Microsoft Windows HTTP Services (WinHTTP)。</para>
        /// </remarks>
        [DllImport("wininet.dll", CharSet = CharSet.Unicode)]
        public static extern bool InternetGetConnectedState(out int connectionDescription, int reservedValue);

        /// <summary>
        /// 设置Internet 选项
        /// </summary>
        /// <param name="hInternet">hInet 句柄</param>
        /// <param name="dmOption">
        ///     dwOption Internet 选项
        ///     <list type="table">
        ///         <item>
        ///             <term>INTERNET_OPTION_SEND_TIMEOUT</term>
        ///             <description>发送请求和连接时的超时时间</description>
        ///         </item>
        ///         <item>
        ///             <term>INTERNET_OPTION_RECEIVE_TIMEOUT</term>
        ///             <description>接收请求和连接时的超时间间</description>
        ///         </item>
        ///     </list>
        /// </param>
        /// <param name="lpBuffer"></param>
        /// <param name="dwBufferLength"></param>
        /// <returns></returns>
        [DllImport(@"wininet", CallingConvention = CallingConvention.StdCall)]
        public static extern bool InternetSetOption(int hInternet, int dmOption, IntPtr lpBuffer, int dwBufferLength);

        /// <summary>
        /// 在初始化文件指定小节内设置一个字串
        /// </summary>
        /// <param name="lpAppName">要在其中写入新字串的小节名称。这个字串不区分大小写</param>
        /// <param name="lpKeyName">要设置的项名或条目名。这个字串不区分大小写。</param>
        /// <param name="lpString">指定为这个项写入的字串值。</param>
        /// <param name="lpFileName">文件路径。如果文件没有找到，则函数会创建它</param>
        /// <returns>成功返回true</returns>
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        public static extern bool WritePrivateProfileString(string lpAppName, string lpKeyName, string lpString,
                                                            string lpFileName);

        /// <summary>
        /// 为初始化文件中指定的条目取得字串
        /// </summary>
        /// <param name="lpAppName">欲在其中查找条目的小节名称。这个字串不区分大小写。</param>
        /// <param name="lpKeyName">欲获取的项名或条目名。这个字串不区分大小写。</param>
        /// <param name="lpDefault">指定的条目没有找到时返回的默认值。可设为空（""）</param>
        /// <param name="lpReturnedString">指定一个字串缓冲区，长度至少为nSize</param>
        /// <param name="nSize">指定装载到lpReturnedString缓冲区的最大字符数量</param>
        /// <param name="lpFileName">文件路径。</param>
        /// <returns>复制到lpReturnedString缓冲区的字节数量，其中不包括那些NULL中止字符。</returns>
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        public static extern int GetPrivateProfileString(string lpAppName, string lpKeyName, string lpDefault,
                                                         StringBuilder lpReturnedString, int nSize, string lpFileName);

        /// <summary>
        /// 文件操作
        /// </summary>
        /// <param name="lpFileOp">文件操作结构体</param>
        /// <returns>成功返回true</returns>
        [DllImport("shell32.dll", CharSet = CharSet.Unicode)]
        public static extern bool SHFileOperation(ref Shfileopstruct lpFileOp);

        /// <summary>
        /// 清空指定驱动器的回收站。
        /// </summary>
        /// <param name="hwnd">A handle to the parent window of any dialog boxes that might be displayed during the operation. This parameter can be NULL. </param>
        /// <param name="pszRootPath">The address of a null-terminated string of maximum length MAX_PATH that contains the path of the root drive on which the Recycle Bin is located. This parameter can contain the address of a string formatted with the drive, folder, and subfolder names, for example c:\windows\system\, etc. It can also contain an empty string or NULL. If this value is an empty string or NULL, all Recycle Bins on all drives will be emptied. </param>
        /// <param name="dwFlags">
        ///     One or more of the following values.
        ///     <list type="table">
        ///         <item>
        ///             <term>SHERB_NOCONFIRMATION</term>
        ///             <description>No dialog box confirming the deletion of the objects will be displayed. </description>
        ///         </item>
        ///         <item>
        ///             <term>SHERB_NOPROGRESSUI</term>
        ///             <description>No dialog box indicating the progress will be displayed. </description>
        ///         </item>
        ///         <item>
        ///             <term>SHERB_NOSOUND</term>
        ///             <description>No sound will be played when the operation is complete.</description>
        ///         </item>
        ///     </list>
        /// </param>
        /// <returns>成功返回true</returns>
        [DllImport("shell32.dll", CharSet = CharSet.Unicode)]
        public static extern bool SHEmptyRecycleBin(IntPtr hwnd, string pszRootPath, uint dwFlags);

        /// <summary>
        /// 该函数定义一个系统范围的热键。
        /// </summary>
        /// <param name="hWnd">接收热键产生WM_HOTKEY消息的窗口句柄。若该参数NULL，传递给调用线程的WM_HOTKEY消息必须在消息循环中中进行处理。</param>
        /// <param name="id">定义热键的标识符。调用线程中的其他热键不能使用同样的标识符。应用功能程序必须定义一个0X0000-0xBFFF范围的值。一个共享的动态链接库（DLL）必须定义一个0xC000-0xFFFF范围的值伯GlobalAddAtom函数返回该范围）。为了避免与其他动态链接库定义的热键冲突，一个DLL必须使用GlobalAddAtom函数获得热键的标识符。</param>
        /// <param name="fsModifiers">
        /// 定义为了产生WM_HOTKEY消息而必须与由nVirtKey参数定义的键一起按下的键。该参数可以是如下值的组合：
        /// <list type="table">
        /// <item>
        /// <term>MOD_ALT</term>
        /// <description>按下的可以是任一Alt键。</description>
        /// </item>
        /// <item>
        /// <term>MOD_CONTROL</term>
        /// <description>按下的可以是任一Ctrl键。</description>
        /// </item>
        /// <item>
        /// <term>MOD_SHIFT</term>
        /// <description>按下的可以是任一Shift键。</description>
        /// </item>
        /// <item>
        /// <term>MOD_WIN</term>
        /// <description>按下的可以是任一Windows按键。这些键可以用Microsoft Windows日志记录下来。</description>
        /// </item>
        /// </list>
        /// </param>
        /// <param name="vk">定义热键的虚拟键码。</param>
        /// <returns>若函数调用成功，返回一个非O值。</returns>
        /// <remarks>
        /// <para>当某键被接下时，系统在所有的热键中寻找匹配者。一旦找到一个匹配的热键，系统将把WM_HOTKEY消息传递给登记了该热键的线程的消息队列。该消息被传送到队列头部，因此它将在下一轮消息循环中被移去。该函数不能将热键同其他线程创建的窗口关联起来。</para>
        /// 
        /// <para>若为一热键定义的击键己被其他热键所定义，则RegisterHotKey函数调用失败。</para>
        /// 
        /// <para>若hWnd参数标识的窗口已用与id参数定义的相同的标识符登记了一个热键，则参数fsModifiers和vk的新值将替代这些参数先前定义的值。</para>
        /// 
        /// <para>Windows CE：Windows CE 2.0以上版本对于参数fsModifiers支持一个附加的标志位。叫做MOD_KEYUP。</para>
        /// 
        /// <para>若设置MOD_KEYUP位，则当发生键被按下或被弹起的事件时，窗口将发送WM_HOTKEY消息。</para>
        /// 
        /// <para>RegisterHotKey可以被用来在线程之间登记热键。</para>
        /// </remarks>
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, Keys vk);

        /// <summary>
        /// 该函数释放调用线程先前登记的热键。
        /// </summary>
        /// <param name="hWnd">与被释放的热键相关的窗口句柄。若热键不与窗口相关，则该参数为NULL。</param>
        /// <param name="id">定义被释放的热键的标识符。</param>
        /// <returns>若函数调用成功，返回值不为0。</returns>
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        /// <summary>
        /// 获取文件信息
        /// 可见通过调用SHGetFileInfo()可以由psfi参数得到文件的图标句柄。但要注意在uFlags参数中不使用SHGFI_PIDL时,SHGetFileInfo()不能获得“我的电脑”等虚似文件夹的信息
        /// </summary>
        /// <param name="pszPath">指定的文件名。
        ///  当uFlags的取值中不包含 SHGFI_PIDL时,可直接指定;
        ///  当uFlags的取值中包含 SHGFI_PIDL时pszPath要通过计算获得, 不能直接指定;
        /// </param>
        /// <param name="dwFileAttributes">文件属性。
        /// 仅当uFlags的取值中包含SHGFI_USEFILEATTRIBUTES时有效,一般不用此参数;
        /// </param>
        /// <param name="psfi">返回获得的文件信息,是一个记录类型,有以下字段:
        ///  _SHFILEINFOA = record
        ///      hIcon: HICON;    { out: icon }  //文件的图标句柄
        ///      iIcon: Integer;  { out: icon index }     //图标的系统索引号
        ///      dwAttributes: DWORD; { out: SFGAO_ flags }    //文件的属性值
        ///      szDisplayName: array [0..MAX_PATH-1] of  AnsiChar;    { out: display name (or path) }  //文件的显示名
        ///      szTypeName: array [0..79] of AnsiChar;{ out: type name }      //文件的类型名
        ///    end;
        /// </param>
        /// <param name="cbfileInfo">psfi 指向的结构大小（字节数）;</param>
        /// <param name="uFlags">指明需要返回的文件信息标识符,常用的有以下常数:
        /// SHGFI_ICON;                    //获得图标
        ///     SHGFI_DISPLAYNAME;    //获得显示名
        ///     SHGFI_TYPENAME;        //获得类型名
        ///     SHGFI_ATTRIBUTES;     //获得属性
        ///     SHGFI_LARGEICON;      //获得大图标
        ///     SHGFI_SMALLICON;      //获得小图标
        ///     SHGFI_PIDL;                  // pszPath是一个标识符
        /// 函数SHGetFileInfo()的返回值也随uFlags的取值变化而有所不同。
        /// </param>
        /// <returns></returns>
        [DllImport("Shell32.dll")]
        public static extern int SHGetFileInfo(string pszPath, uint dwFileAttributes, out SHFILEINFO psfi, uint cbfileInfo, SHGFI uFlags);

        /// <summary>
        /// 获取客户端操作系统版本
        /// </summary>
        public static string GetOsVersion()
        {
            try
            {
                const string subKey = "Software\\Microsoft\\Windows NT\\CurrentVersion";
                string productName = RegistryHelper.GetValue("ProductName", subKey, RegDomain.LocalMachine).ToString();
                string currentBuildNumber = RegistryHelper.GetValue("CurrentBuildNumber", subKey, RegDomain.LocalMachine).ToString();
                string currentVersion = RegistryHelper.GetValue("CurrentVersion", subKey, RegDomain.LocalMachine).ToString();
                //string InstallationType = RegEditHelper.GetValue("InstallationType", subKey, RegDomain.LocalMachine).ToString();
                return string.Concat(productName, " ", currentVersion, " Build ", currentBuildNumber);
            }
            catch
            {
                return "未知操作系统";
            }
        }

        /// <summary>
        /// 获取客户端操作系统名称
        /// </summary>
        public static string GetWindowsName()
        {
            try
            {
                const string subKey = "Software\\Microsoft\\Windows NT\\CurrentVersion";
                object proObj = RegistryHelper.GetValue("ProductName", subKey, RegDomain.LocalMachine);
                if (proObj!=null)
                {
                    return proObj.ToString();
                }
                return string.Empty;
            }
            catch
            {
                return "未知操作系统";
            }
        }

        /// <summary>
        /// 打开显示器
        /// </summary>
        public static void TurnOnMonitorLed()
        {
            SendMessage(new IntPtr(0xffff), NativeConstants.WM_SYSCOMMAND, NativeConstants.SC_MONITORPOWER, -1);
        }

        /// <summary>
        /// 关闭显示器
        /// </summary>
        public static void TurnOffMonitorLed()
        {
            SendMessage(new IntPtr(0xffff), NativeConstants.WM_SYSCOMMAND, NativeConstants.SC_MONITORPOWER, 2);
        }
    }

    /// <summary>
    /// 系统常量
    /// </summary>
    public static class NativeConstants
    {
        //public const int MF_ENABLED = 0x00000000;
        //public const int MF_GRAYED = 0x00000001;
        /// <summary>
        /// 
        /// </summary>
        public const int MF_DISABLED = 0x00000002;

        //public const int MF_BYCOMMAND = 0x00000000;
        //public const int MF_BYPOSITION = 0x00000400;

        /// <summary>
        /// No dialog box confirming the deletion of the objects will be displayed.
        /// </summary>
        public const int SHERB_NOCONFIRMATION = 0x000001;

        /// <summary>
        /// No dialog box indicating the progress will be displayed.
        /// </summary>
        public const int SHERB_NOPROGRESSUI = 0x000002;

        /// <summary>
        /// No sound will be played when the operation is complete.
        /// </summary>
        public const int SHERB_NOSOUND = 0x000004;

        //public const int ICON_BIG = 1;
        //public const int ICON_SMALL = 0;
        //public const int IDC_APPSTARTING = 0x7f8a;
        //public const int IDC_ARROW = 0x7f00;
        //public const int IDC_CROSS = 0x7f03;
        //public const int IDC_HELP = 0x7f8b;
        //public const int IDC_IBEAM = 0x7f01;
        //public const int IDC_NO = 0x7f88;
        //public const int IDC_SIZEALL = 0x7f86;
        //public const int IDC_SIZENESW = 0x7f83;
        //public const int IDC_SIZENS = 0x7f85;
        //public const int IDC_SIZENWSE = 0x7f82;
        //public const int IDC_SIZEWE = 0x7f84;
        //public const int IDC_UPARROW = 0x7f04;
        //public const int IDC_WAIT = 0x7f02;
        //public const int IDM_PAGESETUP = 0x7d4;
        //public const int IDM_PRINT = 0x1b;
        //public const int IDM_PRINTPREVIEW = 0x7d3;
        //public const int IDM_PROPERTIES = 0x1c;
        //public const int IDM_SAVEAS = 0x47;
        //public const int MA_ACTIVATE = 1;
        //public const int MA_ACTIVATEANDEAT = 2;
        //public const int MA_NOACTIVATE = 3;
        //public const int MA_NOACTIVATEANDEAT = 4;
        //public const int MAX_PATH = 260;

        /// <summary>
        /// 滚动条消息参数
        /// </summary>
        public const int EM_LINESCROLL = 0xb6;

        /// <summary>
        /// 关闭所有进程，然后注销用户。
        /// </summary>
        public const int EWX_LOGOFF = 0x00000000;

        /// <summary>
        /// 关闭系统，安全地关闭电源。所有文件缓冲区已经刷新到磁盘上，所有正在运行的进程已经停止。
        ///     <para>Windows要求：</para>
        ///     <para>Windows NT中：调用进程必须有SE_SHUTDOWN_NAME特权。</para>
        ///     <para>Windows 9X中：可以直接调用。</para>
        /// </summary>
        public const int EWX_SHUTDOWN = 0x00000001;

        /// <summary>
        /// 关闭系统，然后重新启动系统。
        ///     <para>Windows要求：</para>
        ///     <para>Windows NT中：调用进程必须有SE_SHUTDOWN_NAME特权。</para>
        ///     <para>Windows 9X中：可以直接调用。</para>
        /// </summary>
        public const int EWX_REBOOT = 0x00000002;

        /// <summary>
        /// 强制终止进程。当此标志设置，Windows不会发送消息WM_QUERYENDSESSION和WM_ENDSESSION的消息给目前在系统中运行的程序。这可能会导致应用程序丢失数据。因此，你应该只在紧急情况下使用此标志。
        /// </summary>
        public const int EWX_FORCE = 0x00000004;

        /// <summary>
        /// 关闭系统并关闭电源。该系统必须支持断电。
        ///     <para>Windows要求：</para>
        ///     <para>Windows NT中调用进程必须有 SE_SHUTDOWN_NAME 特权。</para>
        ///     <para>Windows 9X中：可以直接调用。</para>
        /// </summary>
        public const int EWX_POWEROFF = 0x00000008;

        /// <summary>
        /// Forces processes to terminate if they do not respond to the WM_QUERYENDSESSION or WM_ENDSESSION message within the timeout interval. For more information, see the Remarks.
        /// </summary>
        public const int EWX_FORCEIFHUNG = 0x00000010;

        /// <summary>
        /// The message box contains one push button: OK. This is the default.
        /// </summary>
        public const int MB_OK = 0;

        /// <summary>
        /// Indicates that uIDEnableItem gives the identifier of the menu item. If neither the MF_BYCOMMAND nor MF_BYPOSITION flag is specified, the MF_BYCOMMAND flag is the default flag.
        /// </summary>
        public const int MF_BYCOMMAND = 0;

        /// <summary>
        /// Indicates that uIDEnableItem gives the zero-based relative position of the menu item.
        /// </summary>
        public const int MF_BYPOSITION = 0x400;

        /// <summary>
        /// Indicates that the menu item is enabled and restored from a grayed state so that it can be selected.
        /// </summary>
        public const int MF_ENABLED = 0;

        /// <summary>
        /// Indicates that the menu item is disabled and grayed so that it cannot be selected.
        /// </summary>
        public const int MF_GRAYED = 1;

        /// <summary>
        /// 
        /// </summary>
        public const int MF_SEPARATOR = 0x800;

        /// <summary>
        /// 
        /// </summary>
        public const int MF_STRING = 0x0;

        /// <summary>
        /// 
        /// </summary>
        public const int TPM_RIGHTBUTTON = 0x0002;

        /// <summary>
        /// 
        /// </summary>
        public const int TPM_RETURNCMD = 0x0100;

        //public const int MF_POPUP = 0x10;
        //public const int MF_SYSMENU = 0x2000;


        //public const int MFS_DISABLED = 3;
        //public const int MK_CONTROL = 8;
        //public const int MK_LBUTTON = 1;
        //public const int MK_MBUTTON = 0x10;
        //public const int MK_RBUTTON = 2;
        //public const int MK_SHIFT = 4;
        //public const int MM_ANISOTROPIC = 8;
        //public const int MM_HIMETRIC = 3;
        //public const int MM_TEXT = 1;
        //public const int MNC_EXECUTE = 2;
        //public const int MNC_SELECT = 3;
        //public const int MWT_IDENTITY = 1;
        //public const int NM_CLICK = -2;
        //public const int NM_CUSTOMDRAW = -12;
        //public const int NM_DBLCLK = -3;
        //public const int NM_RCLICK = -5;
        //public const int NM_RDBLCLK = -6;
        //public const int NM_RELEASEDCAPTURE = -16;
        //public const int NONANTIALIASED_QUALITY = 3;
        //public const int NOTSRCCOPY = 0x330008;
        //public const int PAGE_READONLY = 2;
        //public const int PAGE_READWRITE = 4;
        //public const int PAGE_WRITECOPY = 8;
        //public const int PATCOPY = 0xf00021;
        //public const int PATINVERT = 0x5a0049;
        //public const int S_FALSE = 1;
        //public const int S_OK = 0;

        /// <summary>
        ///     Closes the window.
        /// </summary>
        public const int SC_CLOSE = 0xf060;

        //public const int SC_CONTEXTHELP = 0xf180;

        /// <summary>
        ///     Retrieves the window menu as a result of a keystroke. For more information, see the Remarks section.
        /// </summary>
        public const int SC_KEYMENU = 0xf100;

        /// <summary>
        ///     Maximizes the window.
        /// </summary>
        public const int SC_MAXIMIZE = 0xf030;

        /// <summary>
        ///     Minimizes the window.
        /// </summary>
        public const int SC_MINIMIZE = 0xf020;

        /// <summary>
        ///     Moves the window.
        /// </summary>
        public const int SC_MOVE = 0xf010;

        /// <summary>
        ///     Restores the window to its normal position and size.
        /// </summary>
        public const int SC_RESTORE = 0xf120;

        /// <summary>
        ///     Sizes the window.
        /// </summary>
        public const int SC_SIZE = 0xf000;

        //public const int SIF_ALL = 0x17;
        //public const int SIF_PAGE = 2;
        //public const int SIF_POS = 4;
        //public const int SIF_RANGE = 1;
        //public const int SIF_TRACKPOS = 0x10;
        //public const int SIZE_MAXIMIZED = 2;
        //public const int SIZE_MINIMIZED = 1;
        //public const int SIZE_RESTORED = 0;
        //public const int SM_ARRANGE = 0x38;
        //public const int SM_CLEANBOOT = 0x43;
        //public const int SM_CMONITORS = 80;
        //public const int SM_CMOUSEBUTTONS = 0x2b;
        //public const int SM_CXBORDER = 5;
        //public const int SM_CXCURSOR = 13;
        //public const int SM_CXDOUBLECLK = 0x24;
        //public const int SM_CXDRAG = 0x44;
        //public const int SM_CXEDGE = 0x2d;
        //public const int SM_CXFIXEDFRAME = 7;
        //public const int SM_CXFOCUSBORDER = 0x53;
        //public const int SM_CXFRAME = 0x20;
        //public const int SM_CXHSCROLL = 0x15;
        //public const int SM_CXHTHUMB = 10;
        //public const int SM_CXICON = 11;
        //public const int SM_CXICONSPACING = 0x26;
        //public const int SM_CXMAXIMIZED = 0x3d;
        //public const int SM_CXMAXTRACK = 0x3b;
        //public const int SM_CXMENUCHECK = 0x47;
        //public const int SM_CXMENUSIZE = 0x36;
        //public const int SM_CXMIN = 0x1c;
        //public const int SM_CXMINIMIZED = 0x39;
        //public const int SM_CXMINSPACING = 0x2f;
        //public const int SM_CXMINTRACK = 0x22;
        //public const int SM_CXSCREEN = 0;
        //public const int SM_CXSIZE = 30;
        //public const int SM_CXSIZEFRAME = 0x20;
        //public const int SM_CXSMICON = 0x31;
        //public const int SM_CXSMSIZE = 0x34;
        //public const int SM_CXVIRTUALSCREEN = 0x4e;
        //public const int SM_CXVSCROLL = 2;
        //public const int SM_CYBORDER = 6;
        //public const int SM_CYCAPTION = 4;
        //public const int SM_CYCURSOR = 14;
        //public const int SM_CYDOUBLECLK = 0x25;
        //public const int SM_CYDRAG = 0x45;
        //public const int SM_CYEDGE = 0x2e;
        //public const int SM_CYFIXEDFRAME = 8;
        //public const int SM_CYFOCUSBORDER = 0x54;
        //public const int SM_CYFRAME = 0x21;
        //public const int SM_CYHSCROLL = 3;
        //public const int SM_CYICON = 12;
        //public const int SM_CYICONSPACING = 0x27;
        //public const int SM_CYKANJIWINDOW = 0x12;
        //public const int SM_CYMAXIMIZED = 0x3e;
        //public const int SM_CYMAXTRACK = 60;
        //public const int SM_CYMENU = 15;
        //public const int SM_CYMENUCHECK = 0x48;
        //public const int SM_CYMENUSIZE = 0x37;
        //public const int SM_CYMIN = 0x1d;
        //public const int SM_CYMINIMIZED = 0x3a;
        //public const int SM_CYMINSPACING = 0x30;
        //public const int SM_CYMINTRACK = 0x23;
        //public const int SM_CYSCREEN = 1;
        //public const int SM_CYSIZE = 0x1f;
        //public const int SM_CYSIZEFRAME = 0x21;
        //public const int SM_CYSMCAPTION = 0x33;
        //public const int SM_CYSMICON = 50;
        //public const int SM_CYSMSIZE = 0x35;
        //public const int SM_CYVIRTUALSCREEN = 0x4f;
        //public const int SM_CYVSCROLL = 20;
        //public const int SM_CYVTHUMB = 9;
        //public const int SM_DBCSENABLED = 0x2a;
        //public const int SM_DEBUG = 0x16;
        //public const int SM_MENUDROPALIGNMENT = 40;
        //public const int SM_MIDEASTENABLED = 0x4a;
        //public const int SM_MOUSEPRESENT = 0x13;
        //public const int SM_MOUSEWHEELPRESENT = 0x4b;
        //public const int SM_NETWORK = 0x3f;
        //public const int SM_PENWINDOWS = 0x29;
        //public const int SM_REMOTESESSION = 0x1000;
        //public const int SM_SAMEDISPLAYFORMAT = 0x51;
        //public const int SM_SECURE = 0x2c;
        //public const int SM_SHOWSOUNDS = 70;
        //public const int SM_SWAPBUTTON = 0x17;
        //public const int SM_XVIRTUALSCREEN = 0x4c;
        //public const int SM_YVIRTUALSCREEN = 0x4d;
        //public const int STREAM_SEEK_CUR = 1;
        //public const int STREAM_SEEK_END = 2;
        //public const int STREAM_SEEK_SET = 0;
        //public const int SUBLANG_DEFAULT = 1;
        //public const int SW_ERASE = 4;

        /// <summary>
        ///     Hides this window and passes activation to another window.
        /// </summary>
        public const int SW_HIDE = 0;

        //public const int SW_INVALIDATE = 2;
        //public const int SW_MAX = 10;
        //public const int SW_MAXIMIZE = 3;

        /// <summary>
        ///     Minimizes the window and activates the top-level window in the system's list.
        /// </summary>
        public const int SW_MINIMIZE = 6;

        /// <summary>
        /// </summary>
        public const int SW_NORMAL = 1;

        /// <summary>
        ///     Activates and displays the window. If the window is minimized or maximized, Windows restores it to its original size and position.
        /// </summary>
        public const int SW_RESTORE = 9;

        /// <summary>
        /// </summary>
        public const int SW_SCROLLCHILDREN = 1;

        /// <summary>
        ///     Activates the window and displays it in its current size and position.
        /// </summary>
        public const int SW_SHOW = 5;

        /// <summary>
        ///     Activates the window and displays it as a maximized window.
        /// </summary>
        public const int SW_SHOWMAXIMIZED = 3;

        /// <summary>
        ///     Activates the window and displays it as an icon.
        /// </summary>
        public const int SW_SHOWMINIMIZED = 2;

        /// <summary>
        ///     Displays the window as an icon. The window that is currently active remains active.
        /// </summary>
        public const int SW_SHOWMINNOACTIVE = 7;

        /// <summary>
        ///     Displays the window in its current state. The window that is currently active remains active.
        /// </summary>
        public const int SW_SHOWNA = 8;

        /// <summary>
        ///     Displays the window in its most recent size and position. The window that is currently active remains active.
        /// </summary>
        public const int SW_SHOWNOACTIVATE = 4;

        /// <summary>
        ///     Activates and displays the window. If the window is minimized or maximized, Windows restores it to its original size and position.
        /// </summary>
        public const int SW_SMOOTHSCROLL = 0x10;

        /// <summary>
        ///     Deactivated.
        /// </summary>
        public const int WA_INACTIVE = 0;

        /// <summary>
        ///     Activated by some method other than a mouse click (for example, by a call to the SetActiveWindow function or by use of the keyboard interface to select the window).
        /// </summary>
        public const int WA_ACTIVE = 1;

        /// <summary>
        ///     Activated by a mouse click.
        /// </summary>
        public const int WA_CLICKACTIVE = 2;

        /// <summary>
        /// 监视器
        /// </summary>
        public const int SC_MONITORPOWER = 0xf170;

        #region SHFILEOPSTRUCT

        /// <summary>
        ///     指定了多个目标文件，而不是单个目录
        /// </summary>
        public const int FOF_MULTIDESTFILES = 0x0001;

        /// <summary>
        ///     Not used.
        /// </summary>
        public const int FOF_CONFIRMMOUSE = 0x0002;

        /// <summary>
        ///     不显示一个进度对话框
        /// </summary>
        public const int FOF_SILENT = 0x0044;

        /// <summary>
        ///     碰到有抵触的名字时，自动分配前缀
        /// </summary>
        public const int FOF_RENAMEONCOLLISION = 0x0008;

        /// <summary>
        ///     不对用户显示提示
        /// </summary>
        public const int FOF_NOCONFIRMATION = 0x10;

        /// <summary>
        ///     填充 hNameMappings 字段，必须使用 SHFreeNameMappings 释放
        /// </summary>
        public const int FOF_WANTMAPPINGHANDLE = 0x0020;

        /// <summary>
        ///     允许撤销
        /// </summary>
        public const int FOF_ALLOWUNDO = 0x40;

        /// <summary>
        ///     使用 *.* 时, 只对文件操作
        /// </summary>
        public const int FOF_FILESONLY = 0x0080;

        /// <summary>
        ///     简单进度条，意味者不显示文件名。
        /// </summary>
        public const int FOF_SIMPLEPROGRESS = 0x0100;

        /// <summary>
        ///     建新目录时不需要用户确定
        /// </summary>
        public const int FOF_NOCONFIRMMKDIR = 0x0200;

        /// <summary>
        ///     不显示出错用户界面
        /// </summary>
        public const int FOF_NOERRORUI = 0x0400;

        /// <summary>
        ///     不复制 NT 文件的安全属性
        /// </summary>
        public const int FOF_NOCOPYSECURITYATTRIBS = 0x0800;

        /// <summary>
        ///     不递归目录
        /// </summary>
        public const int FOF_NORECURSION = 0x1000;

        /// <summary>
        ///     移动
        /// </summary>
        public const int FO_MOVE = 0x0001;

        /// <summary>
        ///     复制
        /// </summary>
        public const int FO_COPY = 0x0002;

        /// <summary>
        ///     删除
        /// </summary>
        public const int FO_DELETE = 0x0003;

        /// <summary>
        ///     重命名
        /// </summary>
        public const int FO_RENAME = 0x0004;

        #endregion

        //public const int WH_GETMESSAGE = 3;
        //public const int WH_JOURNALPLAYBACK = 1;
        //public const int WH_MOUSE = 7;
        //public const int WHEEL_DELTA = 120;
        //public const int WINDING = 2;
        //public const int WPF_SETMINPOSITION = 1;
        //public const int WS_BORDER = 0x800000;
        //public const int WS_CAPTION = 0xc00000;
        //public const int WS_CHILD = 0x40000000;
        //public const int WS_CLIPCHILDREN = 0x2000000;
        //public const int WS_CLIPSIBLINGS = 0x4000000;
        //public const int WS_DISABLED = 0x8000000;
        //public const int WS_DLGFRAME = 0x400000;
        //public const int WS_EX_APPWINDOW = 0x40000;
        //public const int WS_EX_CLIENTEDGE = 0x200;
        //public const int WS_EX_CONTEXTHELP = 0x400;
        //public const int WS_EX_CONTROLPARENT = 0x10000;
        //public const int WS_EX_DLGMODALFRAME = 1;
        //public const int WS_EX_LAYERED = 0x80000;
        //public const int WS_EX_LAYOUTRTL = 0x400000;
        //public const int WS_EX_LEFT = 0;
        //public const int WS_EX_LEFTSCROLLBAR = 0x4000;
        //public const int WS_EX_LTRREADING = 0;
        //public const int WS_EX_MDICHILD = 0x40;
        //public const int WS_EX_NOINHERITLAYOUT = 0x100000;
        //public const int WS_EX_NOPARENTNOTIFY = 4;
        //public const int WS_EX_RIGHT = 0x1000;
        //public const int WS_EX_RIGHTSCROLLBAR = 0;
        //public const int WS_EX_RTLREADING = 0x2000;
        //public const int WS_EX_STATICEDGE = 0x20000;
        //public const int WS_EX_TOOLWINDOW = 0x80;
        //public const int WS_EX_TOPMOST = 8;
        //public const int WS_EX_TRANSPARENT = 0x20;
        //public const int WS_HSCROLL = 0x100000;
        //public const int WS_MAXIMIZE = 0x1000000;
        //public const int WS_MAXIMIZEBOX = 0x10000;
        //public const int WS_MINIMIZE = 0x20000000;
        //public const int WS_MINIMIZEBOX = 0x20000;
        //public const int WS_OVERLAPPED = 0;
        //public const int WS_POPUP = -2147483648;
        //public const int WS_SYSMENU = 0x80000;
        //public const int WS_TABSTOP = 0x10000;
        //public const int WS_THICKFRAME = 0x40000;
        //public const int WS_VISIBLE = 0x10000000;
        //public const int WS_VSCROLL = 0x200000;
        //public const int WSF_VISIBLE = 1;
        //public const int XBUTTON1 = 1;
        //public const int XBUTTON2 = 2;

        #region 基本消息

        /// <summary>
        ///     The WM_NULL message performs no operation. An application sends the WM_NULL message if it wants to post a message that the recipient window will ignore.
        /// </summary>
        /// <remarks>
        ///     <para>For example, if an application has installed a WH_GETMESSAGE hook and wants to prevent a message from being processed, the GetMsgProc callback function can change the message number to WM_NULL so the recipient will ignore it. </para>
        ///     <para>As another example, an application can check if a window is responding to messages by sending the WM_NULL message with the SendMessageTimeout function.</para>
        /// </remarks>
        public const int WM_NULL = 0x0000;

        /// <summary>
        ///     应用程序创建一个窗口
        /// </summary>
        public const int WM_CREATE = 0x0001;

        /// <summary>
        ///     销毁一个窗口
        /// </summary>
        public const int WM_DESTROY = 0x0002;

        /// <summary>
        ///     移动一个窗口
        /// </summary>
        public const int WM_MOVE = 0x0003;

        /// <summary>
        /// </summary>
        public const int WM_SIZEWAIT = 0x0004;

        /// <summary>
        ///     改变一个窗口的大小
        /// </summary>
        public const int WM_SIZE = 0x0005;

        /// <summary>
        ///     一个窗口被激活或失去激活状态；
        /// </summary>
        public const int WM_ACTIVATE = 0x0006;

        /// <summary>
        ///     获得焦点后
        /// </summary>
        public const int WM_SETFOCUS = 0x0007;

        /// <summary>
        ///     失去焦点
        /// </summary>
        public const int WM_KILLFOCUS = 0x0008;

        /// <summary>
        /// </summary>
        public const int WM_SETVISIBLE = 0x0009;

        /// <summary>
        ///     改变enable状态
        /// </summary>
        public const int WM_ENABLE = 0x000A;

        /// <summary>
        ///     设置窗口是否能重画
        /// </summary>
        public const int WM_SETREDRAW = 0x000B;

        /// <summary>
        ///     应用程序发送此消息来设置一个窗口的文本
        /// </summary>
        public const int WM_SETTEXT = 0x000C;

        /// <summary>
        ///     应用程序发送此消息来复制对应窗口的文本到缓冲区
        /// </summary>
        public const int WM_GETTEXT = 0x000D;

        /// <summary>
        ///     得到与一个窗口有关的文本的长度（不包含空字符）
        /// </summary>
        public const int WM_GETTEXTLENGTH = 0x000E;

        /// <summary>
        ///     要求一个窗口重画自己
        /// </summary>
        public const int WM_PAINT = 0x000F;

        /// <summary>
        ///     当一个窗口或应用程序要关闭时发送一个信号
        /// </summary>
        public const int WM_CLOSE = 0x0010;

        /// <summary>
        ///     当用户选择结束对话框或程序自己调用ExitWindows函数
        /// </summary>
        public const int WM_QUERYENDSESSION = 0x0011;

        /// <summary>
        ///     用来结束程序运行或当程序调用postquitmessage函数
        /// </summary>
        public const int WM_QUIT = 0x0012;

        /// <summary>
        ///     当用户窗口恢复以前的大小位置时，把此消息发送给某个图标
        /// </summary>
        public const int WM_QUERYOPEN = 0x0013;

        /// <summary>
        ///     当窗口背景必须被擦除时（例在窗口改变大小时）
        /// </summary>
        public const int WM_ERASEBKGND = 0x0014;

        /// <summary>
        ///     当系统颜色改变时，发送此消息给所有顶级窗口
        /// </summary>
        public const int WM_SYSCOLORCHANGE = 0x0015;

        /// <summary>
        ///     当系统进程发出WM_QUERYENDSESSION消息后，此消息发送给应用程序，通知它对话是否结束
        /// </summary>
        public const int WM_ENDSESSION = 0x0016;

        /// <summary>
        ///     系统错误
        /// </summary>
        public const int WM_SYSTEMERROR = 0x0017;

        /// <summary>
        ///     当隐藏或显示窗口是发送此消息给这个窗口
        /// </summary>
        public const int WM_SHOWWINDOW = 0x0018;

        /// <summary>
        /// </summary>
        public const int WM_CTLCOLOR = 0x0019;

        /// <summary>
        /// </summary>
        public const int WM_WININICHANGE = 0x001A;

        /// <summary>
        /// </summary>
        public const int WM_SETTINGCHANGE = WM_WININICHANGE;

        /// <summary>
        /// </summary>
        public const int WM_DEVMODECHANGE = 0x001B;

        /// <summary>
        ///     发此消息给应用程序哪个窗口是激活的，哪个是非激活的；
        /// </summary>
        public const int WM_ACTIVATEAPP = 0x001C;

        /// <summary>
        ///     当系统的字体资源库变化时发送此消息给所有顶级窗口
        /// </summary>
        public const int WM_FONTCHANGE = 0x001D;

        /// <summary>
        ///     当系统的时间变化时发送此消息给所有顶级窗口
        /// </summary>
        public const int WM_TIMECHANGE = 0x001E;

        /// <summary>
        ///     发送此消息来取消某种正在进行的摸态（操作）
        /// </summary>
        public const int WM_CANCELMODE = 0x001F;

        /// <summary>
        ///     如果鼠标引起光标在某个窗口中移动且鼠标输入没有被捕获时，就发消息给某个窗口
        /// </summary>
        public const int WM_SETCURSOR = 0x0020;

        /// <summary>
        ///     当光标在某个非激活的窗口中而用户正按着鼠标的某个键发送此消息给当前窗口
        /// </summary>
        public const int WM_MOUSEACTIVATE = 0x0021;

        /// <summary>
        ///     发送此消息给MDI子窗口当用户点击此窗口的标题栏，或当窗口被激活，移动，改变大小
        /// </summary>
        public const int WM_CHILDACTIVATE = 0x0022;

        /// <summary>
        ///     此消息由基于计算机的训练程序发送，通过WH_JOURNALPALYBACK的hook程序 分离出用户输入消息
        /// </summary>
        public const int WM_QUEUESYNC = 0x0023;

        /// <summary>
        ///     此消息发送给窗口当它将要改变大小或位置；
        /// </summary>
        public const int WM_GETMINMAXINFO = 0x0024;

        /// <summary>
        ///     发送给最小化窗口当它图标将要被重画
        /// </summary>
        public const int WM_PAINTICON = 0x0026;

        /// <summary>
        ///     此消息发送给某个最小化窗口，仅当它在画图标前它的背景必须被重画
        /// </summary>
        public const int WM_ICONERASEBKGND = 0x0027;

        /// <summary>
        ///     发送此消息给一个对话框程序去更改焦点位置
        /// </summary>
        public const int WM_NEXTDLGCTL = 0x0028;

        /// <summary>
        ///     每当打印管理列队增加或减少一条作业时发出此消息
        /// </summary>
        public const int WM_SPOOLERSTATUS = 0x002A;

        /// <summary>
        ///     当button，combobox，listbox，menu的可视外观改变时发送此消息给这些控件的所有者
        /// </summary>
        public const int WM_DRAWITEM = 0x002B;

        /// <summary>
        ///     当button, combo box, list box, list view control, or menu item 被创建时发送此消息给控件的所有者
        /// </summary>
        public const int WM_MEASUREITEM = 0x002C;

        /// <summary>
        ///     当the list box 或 combo box 被销毁 或 当 某些项被删除通过LB_DELETESTRING, LB_RESETCONTENT, CB_DELETESTRING, or CB_RESETCONTENT 消息
        /// </summary>
        public const int WM_DELETEITEM = 0x002D;

        /// <summary>
        ///     此消息有一个LBS_WANTKEYBOARDINPUT风格的发出给它的所有者来响应WM_KEYDOWN消息
        /// </summary>
        public const int WM_VKEYTOITEM = 0x002E;

        /// <summary>
        ///     此消息由一个LBS_WANTKEYBOARDINPUT风格的列表框发送给他的所有者来响应WM_CHAR消息
        /// </summary>
        public const int WM_CHARTOITEM = 0x002F;

        /// <summary>
        ///     当绘制文本时程序发送此消息得到控件要用的颜色
        /// </summary>
        public const int WM_SETFONT = 0x0030;

        /// <summary>
        ///     应用程序发送此消息得到当前控件绘制文本的字体
        /// </summary>
        public const int WM_GETFONT = 0x0031;

        /// <summary>
        ///     应用程序发送此消息让一个窗口与一个热键相关连
        /// </summary>
        public const int WM_SETHOTKEY = 0x0032;

        /// <summary>
        ///     应用程序发送此消息来判断热键与某个窗口是否有关联
        /// </summary>
        public const int WM_GETHOTKEY = 0x0033;

        /// <summary>
        ///     此消息发送给最小化窗口，当此窗口将要被拖放而它的类中没有定义图标，应用程序能返回一个图标或光标的句柄，当用户拖放图标时系统显示这个图标或光标
        /// </summary>
        public const int WM_QUERYDRAGICON = 0x0037;

        /// <summary>
        ///     发送此消息来判定combobox或listbox新增加的项的相对位置
        /// </summary>
        public const int WM_COMPAREITEM = 0x0039;

        /// <summary>
        /// </summary>
        public const int WM_GETOBJECT = 0x003D;

        /// <summary>
        ///     显示内存已经很少了
        /// </summary>
        public const int WM_COMPACTING = 0x0041;

        /// <summary>
        /// </summary>
        public const int WM_COMMNOTIFY = 0x0044;

        /// <summary>
        ///     发送此消息给那个窗口的大小和位置将要被改变时，来调用setwindowpos函数或其它窗口管理函数
        /// </summary>
        public const int WM_WINDOWPOSCHANGING = 0x0046;

        /// <summary>
        ///     发送此消息给那个窗口的大小和位置已经被改变时，来调用setwindowpos函数或其它窗口管理函数
        /// </summary>
        public const int WM_WINDOWPOSCHANGED = 0x0047;

        /// <summary>
        ///     当系统将要进入暂停状态时发送此消息
        /// </summary>
        public const int WM_POWER = 0x0048;

        /// <summary>
        ///     当一个应用程序传递数据给另一个应用程序时发送此消息
        /// </summary>
        public const int WM_COPYDATA = 0x004A;

        /// <summary>
        ///     当某个用户取消程序日志激活状态，提交此消息给程序
        /// </summary>
        public const int WM_CANCELJOURNAL = 0x004B;

        /// <summary>
        ///     当某个控件的某个事件已经发生或这个控件需要得到一些信息时，发送此消息给它的父窗口
        /// </summary>
        public const int WM_NOTIFY = 0x004E;

        /// <summary>
        ///     当用户选择某种输入语言，或输入语言的热键改变
        /// </summary>
        public const int WM_INPUTLANGCHANGEREQUEST = 0x0050;

        /// <summary>
        ///     当平台现场已经被改变后发送此消息给受影响的最顶级窗口
        /// </summary>
        public const int WM_INPUTLANGCHANGE = 0x0051;

        /// <summary>
        ///     当程序已经初始化windows帮助例程时发送此消息给应用程序
        /// </summary>
        public const int WM_TCARD = 0x0052;

        /// <summary>
        ///     此消息显示用户按下了F1，如果某个菜单是激活的，就发送此消息个此窗口关联的菜单，否则就 发送给有焦点的窗口，如果当前都没有焦点，就把此消息发送给当前激活的窗口
        /// </summary>
        public const int WM_HELP = 0x0053;

        /// <summary>
        ///     当用户已经登入或退出后发送此消息给所有的窗口，当用户登入或退出时系统更新用户的具体设置信息，在用户更新设置时系统马上发送此消息；
        /// </summary>
        public const int WM_USERCHANGED = 0x0054;

        /// <summary>
        ///     公用控件，自定义控件和他们的父窗口通过此消息来判断控件是使用ANSI还是UNICODE结构 在WM_NOTIFY消息，使用此控件能使某个控件与它的父控件之间进行相互通信
        /// </summary>
        public const int WM_NOTIFYFORMAT = 0x0055;

        /// <summary>
        ///     当用户某个窗口中点击了一下右键就发送此消息给这个窗口
        /// </summary>
        public const int WM_CONTEXTMENU = 0x007B;

        /// <summary>
        ///     当调用SETWINDOWLONG函数将要改变一个或多个 窗口的风格时发送此消息给那个窗口
        /// </summary>
        public const int WM_STYLECHANGING = 0x007C;

        /// <summary>
        ///     当调用SETWINDOWLONG函数一个或多个 窗口的风格后发送此消息给那个窗口
        /// </summary>
        public const int WM_STYLECHANGED = 0x007D;

        /// <summary>
        ///     当显示器的分辨率改变后发送此消息给所有的窗口
        /// </summary>
        public const int WM_DISPLAYCHANGE = 0x007E;

        /// <summary>
        ///     此消息发送给某个窗口来返回与某个窗口有关连的大图标或小图标的句柄；
        /// </summary>
        public const int WM_GETICON = 0x007F;

        /// <summary>
        ///     程序发送此消息让一个新的大图标或小图标与某个窗口关联；
        /// </summary>
        public const int WM_SETICON = 0x0080;

        /// <summary>
        ///     当某个窗口第一次被创建时，此消息在WM_CREATE消息发送前发送；
        /// </summary>
        public const int WM_NCCREATE = 0x0081;

        /// <summary>
        ///     此消息通知某个窗口，非客户区正在销毁
        /// </summary>
        public const int WM_NCDESTROY = 0x0082;

        /// <summary>
        ///     当某个窗口的客户区域必须被核算时发送此消息
        /// </summary>
        public const int WM_NCCALCSIZE = 0x0083;

        /// <summary>
        ///     移动鼠标，按住或释放鼠标时发生
        /// </summary>
        public const int WM_NCHITTEST = 0x0084;

        /// <summary>
        ///     程序发送此消息给某个窗口当它（窗口）的框架必须被绘制时；
        /// </summary>
        public const int WM_NCPAINT = 0x0085;

        /// <summary>
        ///     此消息发送给某个窗口 仅当它的非客户区需要被改变来显示是激活还是非激活状态；
        /// </summary>
        public const int WM_NCACTIVATE = 0x0086;

        /// <summary>
        ///     发送此消息给某个与对话框程序关联的控件，widdows控制方位键和TAB键使输入进入此控件 通过响应WM_GETDLGCODE消息，应用程序可以把他当成一个特殊的输入控件并能处理它
        /// </summary>
        public const int WM_GETDLGCODE = 0x0087;

        /// <summary>
        ///     当光标在一个窗口的非客户区内移动时发送此消息给这个窗口 //非客户区为：窗体的标题栏及窗 的边框体
        /// </summary>
        public const int WM_NCMOUSEMOVE = 0x00A0;

        /// <summary>
        ///     当光标在一个窗口的非客户区同时按下鼠标左键时提交此消息
        /// </summary>
        public const int WM_NCLBUTTONDOWN = 0x00A1;

        /// <summary>
        ///     当用户释放鼠标左键同时光标某个窗口在非客户区十发送此消息
        /// </summary>
        public const int WM_NCLBUTTONUP = 0x00A2;

        /// <summary>
        ///     当用户双击鼠标左键同时光标某个窗口在非客户区十发送此消息
        /// </summary>
        public const int WM_NCLBUTTONDBLCLK = 0x00A3;

        /// <summary>
        ///     当用户按下鼠标右键同时光标又在窗口的非客户区时发送此消息
        /// </summary>
        public const int WM_NCRBUTTONDOWN = 0x00A4;

        /// <summary>
        ///     当用户释放鼠标右键同时光标又在窗口的非客户区时发送此消息
        /// </summary>
        public const int WM_NCRBUTTONUP = 0x00A5;

        /// <summary>
        ///     当用户双击鼠标右键同时光标某个窗口在非客户区十发送此消息
        /// </summary>
        public const int WM_NCRBUTTONDBLCLK = 0x00A6;

        /// <summary>
        ///     当用户按下鼠标中键同时光标又在窗口的非客户区时发送此消息
        /// </summary>
        public const int WM_NCMBUTTONDOWN = 0x00A7;

        /// <summary>
        ///     当用户释放鼠标中键同时光标又在窗口的非客户区时发送此消息
        /// </summary>
        public const int WM_NCMBUTTONUP = 0x00A8;

        /// <summary>
        ///     当用户双击鼠标中键同时光标又在窗口的非客户区时发送此消息
        /// </summary>
        public const int WM_NCMBUTTONDBLCLK = 0x00A9;

        /// <summary>
        /// </summary>
        public const int WM_NCXBUTTONDOWN = 0x00AB;

        /// <summary>
        /// </summary>
        public const int WM_NCXBUTTONUP = 0x00AC;

        /// <summary>
        /// </summary>
        public const int WM_NCXBUTTONDBLCLK = 0x00AD;

        /// <summary>
        /// </summary>
        public const int WM_INPUT = 0x00FF;

        /// <summary>
        ///     按下一个键
        /// </summary>
        public const int WM_KEYFIRST = 0x0100;

        /// <summary>
        ///     按下一个键
        /// </summary>
        public const int WM_KEYDOWN = 0x0100;

        /// <summary>
        ///     释放一个键
        /// </summary>
        public const int WM_KEYUP = 0x0101;

        /// <summary>
        ///     按下某键，并已发出WM_KEYDOWN， WM_KEYUP消息
        /// </summary>
        public const int WM_CHAR = 0x0102;

        /// <summary>
        ///     当用translatemessage函数翻译WM_KEYUP消息时发送此消息给拥有焦点的窗口
        /// </summary>
        public const int WM_DEADCHAR = 0x0103;

        /// <summary>
        ///     当用户按住ALT键同时按下其它键时提交此消息给拥有焦点的窗口；
        /// </summary>
        public const int WM_SYSKEYDOWN = 0x0104;

        /// <summary>
        ///     当用户释放一个键同时ALT 键还按着时提交此消息给拥有焦点的窗口
        /// </summary>
        public const int WM_SYSKEYUP = 0x0105;

        /// <summary>
        ///     当WM_SYSKEYDOWN消息被TRANSLATEMESSAGE函数翻译后提交此消息给拥有焦点的窗口
        /// </summary>
        public const int WM_SYSCHAR = 0x0106;

        /// <summary>
        ///     当WM_SYSKEYDOWN消息被TRANSLATEMESSAGE函数翻译后发送此消息给拥有焦点的窗口
        /// </summary>
        public const int WM_SYSDEADCHAR = 0x0107;

        /// <summary>
        /// </summary>
        public const int WM_KEYLAST = 0x0108;

        /// <summary>
        ///     在一个对话框程序被显示前发送此消息给它，通常用此消息初始化控件和执行其它任务
        /// </summary>
        public const int WM_INITDIALOG = 0x0110;

        /// <summary>
        ///     当用户选择一条菜单命令项或当某个控件发送一条消息给它的父窗口，一个快捷键被翻译
        /// </summary>
        public const int WM_COMMAND = 0x0111;

        /// <summary>
        ///     当用户选择窗口菜单的一条命令或当用户选择最大化或最小化时那个窗口会收到此消息
        /// </summary>
        public const int WM_SYSCOMMAND = 0x0112;

        /// <summary>
        ///     发生了定时器事件
        /// </summary>
        public const int WM_TIMER = 0x0113;

        /// <summary>
        ///     当一个窗口标准水平滚动条产生一个滚动事件时发送此消息给那个窗口，也发送给拥有它的控件
        /// </summary>
        public const int WM_HSCROLL = 0x0114;

        /// <summary>
        ///     当一个窗口标准垂直滚动条产生一个滚动事件时发送此消息给那个窗口也，发送给拥有它的控件
        /// </summary>
        public const int WM_VSCROLL = 0x0115;

        /// <summary>
        ///     当一个菜单将要被激活时发送此消息，它发生在用户菜单条中的某项或按下某个菜单键，它允许程序在显示前更改菜单
        /// </summary>
        public const int WM_INITMENU = 0x0116;

        /// <summary>
        ///     当一个下拉菜单或子菜单将要被激活时发送此消息，它允许程序在它显示前更改菜单，而不要改变全部
        /// </summary>
        public const int WM_INITMENUPOPUP = 0x0117;

        /// <summary>
        ///     当用户选择一条菜单项时发送此消息给菜单的所有者（一般是窗口）
        /// </summary>
        public const int WM_MENUSELECT = 0x011F;

        /// <summary>
        ///     当菜单已被激活用户按下了某个键（不同于加速键），发送此消息给菜单的所有者；
        /// </summary>
        public const int WM_MENUCHAR = 0x0120;

        /// <summary>
        ///     当一个模态对话框或菜单进入空载状态时发送此消息给它的所有者，一个模态对话框或菜单进入空载状态就是在处理完一条或几条先前的消息后没有消息它的列队中等待
        /// </summary>
        public const int WM_ENTERIDLE = 0x0121;

        /// <summary>
        /// </summary>
        public const int WM_MENURBUTTONUP = 0x0122;

        /// <summary>
        /// </summary>
        public const int WM_MENUDRAG = 0x0123;

        /// <summary>
        /// </summary>
        public const int WM_MENUGETOBJECT = 0x0124;

        /// <summary>
        /// </summary>
        public const int WM_UNINITMENUPOPUP = 0x0125;

        /// <summary>
        /// </summary>
        public const int WM_MENUCOMMAND = 0x0126;

        /// <summary>
        /// </summary>
        public const int WM_CHANGEUISTATE = 0x0127;

        /// <summary>
        /// </summary>
        public const int WM_UPDATEUISTATE = 0x0128;

        /// <summary>
        /// </summary>
        public const int WM_QUERYUISTATE = 0x0129;

        /// <summary>
        ///     在windows绘制消息框前发送此消息给消息框的所有者窗口，通过响应这条消息，所有者窗口可以通过使用给定的相关显示设备的句柄来设置消息框的文本和背景颜色
        /// </summary>
        public const int WM_CTLCOLORMSGBOX = 0x0132;

        /// <summary>
        ///     当一个编辑型控件将要被绘制时发送此消息给它的父窗口；通过响应这条消息，所有者窗口可以通过使用给定的相关显示设备的句柄来设置编辑框的文本和背景颜色
        /// </summary>
        public const int WM_CTLCOLOREDIT = 0x0133;

        /// <summary>
        ///     当一个列表框控件将要被绘制前发送此消息给它的父窗口；通过响应这条消息，所有者窗口可以通过使用给定的相关显示设备的句柄来设置列表框的文本和背景颜色
        /// </summary>
        public const int WM_CTLCOLORLISTBOX = 0x0134;

        /// <summary>
        ///     当一个按钮控件将要被绘制时发送此消息给它的父窗口；通过响应这条消息，所有者窗口可以通过使用给定的相关显示设备的句柄来设置按纽的文本和背景颜色
        /// </summary>
        public const int WM_CTLCOLORBTN = 0x0135;

        /// <summary>
        ///     当一个对话框控件将要被绘制前发送此消息给它的父窗口；通过响应这条消息，所有者窗口可以通过使用给定的相关显示设备的句柄来设置对话框的文本背景颜色
        /// </summary>
        public const int WM_CTLCOLORDLG = 0x0136;

        /// <summary>
        ///     当一个滚动条控件将要被绘制时发送此消息给它的父窗口；通过响应这条消息，所有者窗口可以通过使用给定的相关显示设备的句柄来设置滚动条的背景颜色
        /// </summary>
        public const int WM_CTLCOLORSCROLLBAR = 0x0137;

        /// <summary>
        ///     当一个静态控件将要被绘制时发送此消息给它的父窗口；通过响应这条消息，所有者窗口可以通过使用给定的相关显示设备的句柄来设置静态控件的文本和背景颜色
        /// </summary>
        public const int WM_CTLCOLORSTATIC = 0x0138;

        /// <summary>
        /// </summary>
        public const int WM_MOUSEFIRST = 0x0200;

        /// <summary>
        ///     移动鼠标
        /// </summary>
        public const int WM_MOUSEMOVE = 0x0200;

        /// <summary>
        ///     按下鼠标左键
        /// </summary>
        public const int WM_LBUTTONDOWN = 0x0201;

        /// <summary>
        ///     释放鼠标左键
        /// </summary>
        public const int WM_LBUTTONUP = 0x0202;

        /// <summary>
        ///     双击鼠标左键
        /// </summary>
        public const int WM_LBUTTONDBLCLK = 0x0203;

        /// <summary>
        ///     按下鼠标右键
        /// </summary>
        public const int WM_RBUTTONDOWN = 0x0204;

        /// <summary>
        ///     释放鼠标右键
        /// </summary>
        public const int WM_RBUTTONUP = 0x0205;

        /// <summary>
        ///     双击鼠标右键
        /// </summary>
        public const int WM_RBUTTONDBLCLK = 0x0206;

        /// <summary>
        ///     按下鼠标中键
        /// </summary>
        public const int WM_MBUTTONDOWN = 0x0207;

        /// <summary>
        ///     释放鼠标中键
        /// </summary>
        public const int WM_MBUTTONUP = 0x0208;

        /// <summary>
        ///     双击鼠标中键
        /// </summary>
        public const int WM_MBUTTONDBLCLK = 0x0209;

        /// <summary>
        ///     当鼠标轮子转动时发送此消息个当前有焦点的控件
        /// </summary>
        public const int WM_MOUSEWHEEL = 0x020A;

        /// <summary>
        ///     当鼠标轮子转动时发送此消息个当前有焦点的控件
        /// </summary>
        public const int WM_MOUSELAST = 0x020A;

        /// <summary>
        ///     当MDI子窗口被创建或被销毁，或用户按了一下鼠标键而光标在子窗口上时发送此消息给它的父窗口
        /// </summary>
        public const int WM_PARENTNOTIFY = 0x0210;

        /// <summary>
        ///     发送此消息通知应用程序的主窗口that已经进入了菜单循环模式
        /// </summary>
        public const int WM_ENTERMENULOOP = 0x0211;

        /// <summary>
        ///     发送此消息通知应用程序的主窗口that已退出了菜单循环模式
        /// </summary>
        public const int WM_EXITMENULOOP = 0x0212;

        /// <summary>
        /// </summary>
        public const int WM_NEXTMENU = 0x0213;

        /// <summary>
        ///     当用户正在调整窗口大小时发送此消息给窗口；通过此消息应用程序可以监视窗口大小和位置也可以修改他们
        /// </summary>
        public const int WM_SIZING = 532;

        /// <summary>
        ///     发送此消息 给窗口当它失去捕获的鼠标时
        /// </summary>
        public const int WM_CAPTURECHANGED = 533;

        /// <summary>
        ///     当用户在移动窗口时发送此消息，通过此消息应用程序可以监视窗口大小和位置也可以修改他们
        /// </summary>
        public const int WM_MOVING = 534;

        /// <summary>
        ///     此消息发送给应用程序来通知它有关电源管理事件
        /// </summary>
        public const int WM_POWERBROADCAST = 536;

        /// <summary>
        ///     当设备的硬件配置改变时发送此消息给应用程序或设备驱动程序
        /// </summary>
        public const int WM_DEVICECHANGE = 537;

        /// <summary>
        /// </summary>
        public const int WM_IME_STARTCOMPOSITION = 0x010D;

        /// <summary>
        /// </summary>
        public const int WM_IME_ENDCOMPOSITION = 0x010E;

        /// <summary>
        /// </summary>
        public const int WM_IME_COMPOSITION = 0x010F;

        /// <summary>
        /// </summary>
        public const int WM_IME_KEYLAST = 0x010F;

        /// <summary>
        /// </summary>
        public const int WM_IME_SETCONTEXT = 0x0281;

        /// <summary>
        /// </summary>
        public const int WM_IME_NOTIFY = 0x0282;

        /// <summary>
        /// </summary>
        public const int WM_IME_CONTROL = 0x0283;

        /// <summary>
        /// </summary>
        public const int WM_IME_COMPOSITIONFULL = 0x0284;

        /// <summary>
        /// </summary>
        public const int WM_IME_SELECT = 0x0285;

        /// <summary>
        /// </summary>
        public const int WM_IME_CHAR = 0x0286;

        /// <summary>
        /// </summary>
        public const int WM_IME_REQUEST = 0x0288;

        /// <summary>
        /// </summary>
        public const int WM_IME_KEYDOWN = 0x0290;

        /// <summary>
        /// </summary>
        public const int WM_IME_KEYUP = 0x0291;

        /// <summary>
        ///     应用程序发送此消息给多文档的客户窗口来创建一个MDI 子窗口
        /// </summary>
        public const int WM_MDICREATE = 0x0220;

        /// <summary>
        ///     应用程序发送此消息给多文档的客户窗口来关闭一个MDI 子窗口
        /// </summary>
        public const int WM_MDIDESTROY = 0x0221;

        /// <summary>
        ///     应用程序发送此消息给多文档的客户窗口通知客户窗口激活另一个MDI子窗口，当客户窗口收到此消息后，它发出WM_MDIACTIVE消息给MDI子窗口（未激活）激活它；
        /// </summary>
        public const int WM_MDIACTIVATE = 0x0222;

        /// <summary>
        ///     程序 发送此消息给MDI客户窗口让子窗口从最大最小化恢复到原来大小
        /// </summary>
        public const int WM_MDIRESTORE = 0x0223;

        /// <summary>
        ///     程序 发送此消息给MDI客户窗口激活下一个或前一个窗口
        /// </summary>
        public const int WM_MDINEXT = 0x0224;

        /// <summary>
        ///     程序发送此消息给MDI客户窗口来最大化一个MDI子窗口；
        /// </summary>
        public const int WM_MDIMAXIMIZE = 0x0225;

        /// <summary>
        ///     程序 发送此消息给MDI客户窗口以平铺方式重新排列所有MDI子窗口
        /// </summary>
        public const int WM_MDITILE = 0x0226;

        /// <summary>
        ///     程序 发送此消息给MDI客户窗口以层叠方式重新排列所有MDI子窗口
        /// </summary>
        public const int WM_MDICASCADE = 0x0227;

        /// <summary>
        ///     程序 发送此消息给MDI客户窗口重新排列所有最小化的MDI子窗口
        /// </summary>
        public const int WM_MDIICONARRANGE = 0x0228;

        /// <summary>
        ///     程序 发送此消息给MDI客户窗口来找到激活的子窗口的句柄
        /// </summary>
        public const int WM_MDIGETACTIVE = 0x0229;

        /// <summary>
        ///     程序 发送此消息给MDI客户窗口用MDI菜单代替子窗口的菜单
        /// </summary>
        public const int WM_MDISETMENU = 0x0230;

        /// <summary>
        /// </summary>
        public const int WM_ENTERSIZEMOVE = 0x0231;

        /// <summary>
        /// </summary>
        public const int WM_EXITSIZEMOVE = 0x0232;

        /// <summary>
        /// </summary>
        public const int WM_DROPFILES = 0x0233;

        /// <summary>
        /// </summary>
        public const int WM_MDIREFRESHMENU = 0x0234;

        /// <summary>
        /// </summary>
        public const int WM_MOUSEHOVER = 0x02A1;

        /// <summary>
        /// </summary>
        public const int WM_MOUSELEAVE = 0x02A3;

        /// <summary>
        /// </summary>
        public const int WM_NCMOUSEHOVER = 0x02A0;

        /// <summary>
        /// </summary>
        public const int WM_NCMOUSELEAVE = 0x02A2;

        /// <summary>
        /// </summary>
        public const int WM_WTSSESSION_CHANGE = 0x02B1;

        /// <summary>
        /// </summary>
        public const int WM_TABLET_FIRST = 0x02C0;

        /// <summary>
        /// </summary>
        public const int WM_TABLET_LAST = 0x02DF;

        /// <summary>
        ///     程序发送此消息给一个编辑框或combobox来删除当前选择的文本
        /// </summary>
        public const int WM_CUT = 0x0300;

        /// <summary>
        ///     程序发送此消息给一个编辑框或combobox来复制当前选择的文本到剪贴板
        /// </summary>
        public const int WM_COPY = 0x0301;

        /// <summary>
        ///     程序发送此消息给editcontrol或combobox从剪贴板中得到数据
        /// </summary>
        public const int WM_PASTE = 0x0302;

        /// <summary>
        ///     程序发送此消息给editcontrol或combobox清除当前选择的内容；
        /// </summary>
        public const int WM_CLEAR = 0x0303;

        /// <summary>
        ///     程序发送此消息给editcontrol或combobox撤消最后一次操作
        /// </summary>
        public const int WM_UNDO = 0x0304;

        /// <summary>
        /// </summary>
        public const int WM_RENDERFORMAT = 0x0305;

        /// <summary>
        /// </summary>
        public const int WM_RENDERALLFORMATS = 0x0306;

        /// <summary>
        ///     当调用ENPTYCLIPBOARD函数时 发送此消息给剪贴板的所有者
        /// </summary>
        public const int WM_DESTROYCLIPBOARD = 0x0307;

        /// <summary>
        ///     当剪贴板的内容变化时发送此消息给剪贴板观察链的第一个窗口；它允许用剪贴板观察窗口来显示剪贴板的新内容；
        /// </summary>
        public const int WM_DRAWCLIPBOARD = 0x0308;

        /// <summary>
        ///     当剪贴板包含CF_OWNERDIPLAY格式的数据并且剪贴板观察窗口的客户区需要重画；
        /// </summary>
        public const int WM_PAINTCLIPBOARD = 0x0309;

        /// <summary>
        /// </summary>
        public const int WM_VSCROLLCLIPBOARD = 0x030A;

        /// <summary>
        ///     当剪贴板包含CF_OWNERDIPLAY格式的数据并且剪贴板观察窗口的客户区域的大小已经改变是此消息通过剪贴板观察窗口发送给剪贴板的所有者；
        /// </summary>
        public const int WM_SIZECLIPBOARD = 0x030B;

        /// <summary>
        ///     通过剪贴板观察窗口发送此消息给剪贴板的所有者来请求一个CF_OWNERDISPLAY格式的剪贴板的名字
        /// </summary>
        public const int WM_ASKCBFORMATNAME = 0x030C;

        /// <summary>
        ///     当一个窗口从剪贴板观察链中移去时发送此消息给剪贴板观察链的第一个窗口；
        /// </summary>
        public const int WM_CHANGECBCHAIN = 0x030D;

        /// <summary>
        ///     此消息通过一个剪贴板观察窗口发送给剪贴板的所有者 ；它发生在当剪贴板包含CFOWNERDISPALY格式的数据并且有个事件在剪贴板观察窗的水平滚动条上；所有者应滚动剪贴板图象并更新滚动条的值；
        /// </summary>
        public const int WM_HSCROLLCLIPBOARD = 0x030E;

        /// <summary>
        ///     此消息发送给将要收到焦点的窗口，此消息能使窗口在收到焦点时同时有机会实现他的逻辑调色板
        /// </summary>
        public const int WM_QUERYNEWPALETTE = 0x030F;

        /// <summary>
        ///     当一个应用程序正要实现它的逻辑调色板时发此消息通知所有的应用程序
        /// </summary>
        public const int WM_PALETTEISCHANGING = 0x0310;

        /// <summary>
        ///     此消息在一个拥有焦点的窗口实现它的逻辑调色板后发送此消息给所有顶级并重叠的窗口，以此来改变系统调色板
        /// </summary>
        public const int WM_PALETTECHANGED = 0x0311;

        /// <summary>
        ///     当用户按下由REGISTERHOTKEY函数注册的热键时提交此消息
        /// </summary>
        public const int WM_HOTKEY = 0x0312;

        /// <summary>
        ///     应用程序发送此消息仅当WINDOWS或其它应用程序发出一个请求要求绘制一个应用程序的一部分；
        /// </summary>
        public const int WM_PRINT = 791;

        /// <summary>
        /// </summary>
        public const int WM_PRINTCLIENT = 792;

        /// <summary>
        /// </summary>
        public const int WM_APPCOMMAND = 0x0319;

        /// <summary>
        /// </summary>
        public const int WM_THEMECHANGED = 0x031A;

        /// <summary>
        /// </summary>
        public const int WM_HANDHELDFIRST = 856;

        /// <summary>
        /// </summary>
        public const int WM_HANDHELDLAST = 863;

        /// <summary>
        /// </summary>
        public const int WM_PENWINFIRST = 0x0380;

        /// <summary>
        /// </summary>
        public const int WM_PENWINLAST = 0x038F;

        /// <summary>
        /// </summary>
        public const int WM_COALESCE_FIRST = 0x0390;

        /// <summary>
        /// </summary>
        public const int WM_COALESCE_LAST = 0x039F;

        /// <summary>
        /// </summary>
        public const int WM_DDE_FIRST = 0x03E0;

        /// <summary>
        ///     一个DDE客户程序提交此消息开始一个与服务器程序的会话来响应那个指定的程序和主题名；
        /// </summary>
        public const int WM_DDE_INITIATE = WM_DDE_FIRST + 0;

        /// <summary>
        ///     一个DDE应用程序（无论是客户还是服务器）提交此消息来终止一个会话；
        /// </summary>
        public const int WM_DDE_TERMINATE = WM_DDE_FIRST + 1;

        /// <summary>
        ///     一个DDE客户程序提交此消息给一个DDE服务程序来请求服务器每当数据项改变时更新它
        /// </summary>
        public const int WM_DDE_ADVISE = WM_DDE_FIRST + 2;

        /// <summary>
        ///     一个DDE客户程序通过此消息通知一个DDE服务程序不更新指定的项或一个特殊的剪贴板格式的项
        /// </summary>
        public const int WM_DDE_UNADVISE = WM_DDE_FIRST + 3;

        /// <summary>
        ///     此消息通知一个DDE（动态数据交换）程序已收到并正在处理WM_DDE_POKE, WM_DDE_EXECUTE, WM_DDE_DATA, WM_DDE_ADVISE, WM_DDE_UNADVISE, or WM_DDE_INITIAT消息
        /// </summary>
        public const int WM_DDE_ACK = WM_DDE_FIRST + 4;

        /// <summary>
        ///     一个DDE服务程序提交此消息给DDE客户程序来传递个一数据项给客户或通知客户的一条可用数据项
        /// </summary>
        public const int WM_DDE_DATA = WM_DDE_FIRST + 5;

        /// <summary>
        ///     一个DDE客户程序提交此消息给一个DDE服务程序来请求一个数据项的值；
        /// </summary>
        public const int WM_DDE_REQUEST = WM_DDE_FIRST + 6;

        /// <summary>
        ///     一个DDE客户程序提交此消息给一个DDE服务程序，客户使用此消息来请求服务器接收一个未经同意的数据项；服务器通过答复WM_DDE_ACK消息提示是否它接收这个数据项；
        /// </summary>
        public const int WM_DDE_POKE = WM_DDE_FIRST + 7;

        /// <summary>
        ///     一个DDE客户程序提交此消息给一个DDE服务程序来发送一个字符串给服务器让它象串行命令一样被处理，服务器通过提交WM_DDE_ACK消息来作回应；
        /// </summary>
        public const int WM_DDE_EXECUTE = WM_DDE_FIRST + 8;

        /// <summary>
        ///     一个DDE客户程序提交此消息给一个DDE服务程序来发送一个字符串给服务器让它象串行命令一样被处理，服务器通过提交WM_DDE_ACK消息来作回应；
        /// </summary>
        public const int WM_DDE_LAST = WM_DDE_FIRST + 8;

        /// <summary>
        /// </summary>
        public const int WM_APP = 0x8000;

        /// <summary>
        /// </summary>
        public const int WM_USER = 0x0400;

        #endregion
    }

    /// <summary>
    /// 矩形结构体
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct RectInfo
    {
        /// <summary>
        /// 左
        /// </summary>
        public int left;

        /// <summary>
        /// 上
        /// </summary>
        public int top;

        /// <summary>
        /// 右
        /// </summary>
        public int right;

        /// <summary>
        /// 下
        /// </summary>
        public int bottom;
    }

    /// <summary>
    /// 点结构体
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct PointInfo
    {
        /// <summary>
        /// X坐标
        /// </summary>
        public int x;

        /// <summary>
        /// Y坐标
        /// </summary>
        public int y;
    }

    /// <summary>
    /// 上次输入操作的结构体
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Lastinputinfo
    {
        /// <summary>
        /// </summary>
        [MarshalAs(UnmanagedType.U4)]
        public int cbSize;

        /// <summary>
        /// 上次输入事件发生时的系统运行时间
        /// </summary>
        [MarshalAs(UnmanagedType.U4)]
        public uint dwTime;
    }

    /// <summary>
    /// 日期时间结构体
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Systemtime
    {
        /// <summary>
        /// 年
        /// </summary>
        public ushort wYear;

        /// <summary>
        /// 月
        /// </summary>
        public ushort wMonth;

        /// <summary>
        /// 星期
        /// </summary>
        public ushort wDayOfWeek;

        /// <summary>
        /// 日
        /// </summary>
        public ushort wDay;

        /// <summary>
        /// 小时
        /// </summary>
        public ushort wHour;

        /// <summary>
        /// 分钟
        /// </summary>
        public ushort wMinute;

        /// <summary>
        /// 秒
        /// </summary>
        public ushort wSecond;

        /// <summary>
        /// 毫秒
        /// </summary>
        public ushort wMilliseconds;
    }

    /// <summary>
    /// </summary>
    public struct Shfileopstruct
    {
        /// <summary>
        /// 是否可被中断
        /// </summary>
        public bool FAnyOperationsAborted;

        /// <summary>
        /// 标志，附加选项
        /// </summary>
        public int FFlags;

        /// <summary>
        /// 文件映射名字，可在其它 Shell 函数中使用
        /// </summary>
        public int HNameMappings;

        /// <summary>
        /// 父窗口句柄, 0为桌面
        /// </summary>
        public int Hwnd;

        /// <summary>
        /// 只在 FOF_SIMPLEPROGRESS 时，指定对话框的标题。
        /// </summary>
        public string LpszProgressTitle;

        /// <summary>
        /// 源文件路径，可以是多个文件
        /// </summary>
        public string PFrom;

        /// <summary>
        /// 目标路径，可以是路径或文件名
        /// </summary>
        public string PTo;

        /// <summary>
        /// 设置操作方式，移动：FO_MOVE，复制：FO_COPY，删除：FO_DELETE
        /// </summary>
        public int WFunc;
    }

    /// <summary>
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct SHFILEINFO
    {
        ///// <summary>
        ///// </summary>
        //public SHFILEINFO()
        //{
        //    hIcon = IntPtr.Zero;
        //    iIcon = 0;
        //    dwAttributes = 0;
        //    szDisplayName = "";
        //    szTypeName = "";
        //}

        /// <summary>
        /// </summary>
        public IntPtr hIcon;

        /// <summary>
        /// </summary>
        public int iIcon;

        /// <summary>
        /// </summary>
        public uint dwAttributes;

        /// <summary>
        /// </summary>
        [MarshalAs(UnmanagedType.LPStr, SizeConst = 260)]
        public string szDisplayName;

        /// <summary>
        /// </summary>
        [MarshalAs(UnmanagedType.LPStr, SizeConst = 80)]
        public string szTypeName;
    };

    /// <summary>
    /// </summary>
    [Flags]
    public enum SHGFI
    {
        /// <summary>
        /// </summary>
        SmallIcon = 0x00000001,

        /// <summary>
        /// </summary>
        LargeIcon = 0x00000000,

        /// <summary>
        /// </summary>
        Icon = 0x00000100,

        /// <summary>
        /// </summary>
        DisplayName = 0x00000200,

        /// <summary>
        /// </summary>
        Typename = 0x00000400,

        /// <summary>
        /// </summary>
        SysIconIndex = 0x00004000,

        /// <summary>
        /// </summary>
        UseFileAttributes = 0x00000010
    }
}