using System;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace Zutatensuppe.DiabloInterface
{
    class ApplicationConsole
    {
        // @see https://developercommunity.visualstudio.com/content/problem/12166/console-output-is-gone-in-vs2017-works-fine-when-d.html

        [DllImport("kernel32.dll", EntryPoint = "AllocConsole", SetLastError = true, CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        private static extern int AllocConsole();

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr CreateFile(string lpFileName, uint dwDesiredAccess, uint dwShareMode, uint lpSecurityAttributes, uint dwCreationDisposition, uint dwFlagsAndAttributes, uint hTemplateFile);

        private const int MY_CODE_PAGE = 437;
        private const uint GENERIC_WRITE = 0x40000000;
        private const uint FILE_SHARE_WRITE = 0x2;
        private const uint OPEN_EXISTING = 0x3;

        public static void initialize()
        {
            try
            {
                AllocConsole();
                // Console.OpenStandardOutput eventually calls into GetStdHandle. As per MSDN documentation of GetStdHandle: http://msdn.microsoft.com/en-us/library/windows/desktop/ms683231(v=vs.85).aspx will return the redirected handle and not the allocated console:
                // "The standard handles of a process may be redirected by a call to  SetStdHandle, in which case  GetStdHandle returns the redirected handle. If the standard handles have been redirected, you can specify the CONIN$ value in a call to the CreateFile function to get a handle to a console's input buffer. Similarly, you can specify the CONOUT$ value to get a handle to a console's active screen buffer."
                // Get the handle to CONOUT$.    
                IntPtr stdHandle = CreateFile("CONOUT$", GENERIC_WRITE, FILE_SHARE_WRITE, 0, OPEN_EXISTING, 0, 0);
                SafeFileHandle safeFileHandle = new SafeFileHandle(stdHandle, true);
                FileStream fileStream = new FileStream(safeFileHandle, FileAccess.Write);
                Encoding encoding = Encoding.GetEncoding(MY_CODE_PAGE);
                StreamWriter standardOutput = new StreamWriter(fileStream, encoding);
                standardOutput.AutoFlush = true;
                Console.SetOut(standardOutput);
            }
            catch { }
        }
    }
}
