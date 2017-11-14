using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;

namespace D3Helper.A_Tools
{
    class T_D3Client
    {
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        private static IntPtr GetDiabloWindowHandle()
        {
            try
            {
                Process[] p = Process.GetProcessesByName("Diablo III");
                if(p == null || !p.Any())
                {
                    p = Process.GetProcessesByName("Diablo III64");
                }

                IntPtr hWnd = p[0].MainWindowHandle;
                
                return hWnd;
            }
            catch { return new IntPtr(); }
        }
        private static class Win32
        {
            private const string User32 = "user32.dll";

            [DllImport(User32)]
            internal static extern bool GetClientRect(IntPtr windowHandle, out Int32Rect clientRect);

            [DllImport(User32)]
            internal static extern bool ClientToScreen(IntPtr windowHandle, ref Int32Rect point);
        }
        public static Int32Rect getClient_Rect(IntPtr windowHandle)
        {
            try
            {
                Int32Rect clientRect;
                Win32.GetClientRect(windowHandle, out clientRect);
                Win32.ClientToScreen(windowHandle, ref clientRect);
                return clientRect;
            }
            catch { return new Int32Rect(); }
        }
        public static bool IsForeground()
        {
            try
            {
                IntPtr ForeGroundWindow = new IntPtr();

                try
                {
                    ForeGroundWindow = GetForegroundWindow();
                }
                catch { }

                if (ForeGroundWindow == GetDiabloWindowHandle())
                {
                    return true;
                }
                return false;
            }
            catch { return false; }
        }
    }
}
