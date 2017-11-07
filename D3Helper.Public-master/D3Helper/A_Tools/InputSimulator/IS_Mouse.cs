using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Drawing;
using D3Helper.A_Handler.Log;

namespace D3Helper.A_Tools.InputSimulator
{
    class IS_Mouse
    {
        #region user32.dll
        private const uint MOUSEEVENTF_LEFTDOWN = 0x02;
        private const uint MOUSEEVENTF_LEFTUP = 0x04;
        private const uint MOUSEEVENTF_RIGHTDOWN = 0x08;
        private const uint MOUSEEVENTF_RIGHTUP = 0x010;

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);
        #endregion
        public static void RightCLick()
        {

            uint x = (uint)Cursor.Position.X;
            uint y = (uint)Cursor.Position.Y;

            mouse_event(MOUSEEVENTF_RIGHTDOWN | MOUSEEVENTF_RIGHTUP, x, y, 0, 0);

            //-- log action
            A_Handler.Log.LogEntry.addLogEntry("Pressed RMB()");
            //
        }


        public static void LeftClick(int xLow, int yLow, int xHigh, int yHigh)
        {
            
            Random random = new Random();
            uint x = (uint)random.Next(xLow + 1, xHigh - 1);
            uint y = (uint)random.Next(yLow + 1, yHigh - 1);

            Cursor.Position = new Point((int)x, (int)y);

            mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, x, y, 0, 0);

            //-- log action
            A_Handler.Log.LogEntry.addLogEntry("Pressed LMB(" + xLow + "," + yLow + "," + xHigh + "," + yHigh + ")");
            //
        }


        public static void RightCLick(int xLow, int yLow, int xHigh, int yHigh)
        {
            Random random = new Random();
            uint x = (uint)random.Next(xLow + 1, xHigh - 1);
            uint y = (uint)random.Next(yLow + 1, yHigh - 1);

            Cursor.Position = new Point((int)x, (int)y);

            mouse_event(MOUSEEVENTF_RIGHTDOWN | MOUSEEVENTF_RIGHTUP, x, y, 0, 0);

            //-- log action
            A_Handler.Log.LogEntry.addLogEntry("Pressed RMB(" + xLow + "," + yLow + "," + xHigh + "," + yHigh + ")");
            //
        }


        public static void LeftClick()
        {
            uint x = (uint)Cursor.Position.X;
            uint y = (uint)Cursor.Position.Y;

            mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, x, y, 0, 0);

            //-- log action
            A_Handler.Log.LogEntry.addLogEntry("Pressed LMB()");
            //
        }


        public static void LeftClick(uint x, uint y)
        {
            mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, x, y, 0, 0);

            //-- log action
            A_Handler.Log.LogEntry.addLogEntry("Pressed LMB(" + x + "," + y + ")");
            //
        }


        public static void LeftDown(uint x, uint y)
        {
            mouse_event(MOUSEEVENTF_LEFTDOWN, x, y, 0, 0);

            //-- log action
            A_Handler.Log.LogEntry.addLogEntry("KeyDown LMB(" + x + "," + y + ")");
            //
        }


        public static void RightDown(uint x, uint y)
        {
            mouse_event(MOUSEEVENTF_RIGHTDOWN, x, y, 0, 0);

            //-- log action
            A_Handler.Log.LogEntry.addLogEntry("KeyDown RMB(" + x + "," + y + ")");
            //
        }


        public static void LeftUp(uint x, uint y)
        {
            mouse_event(MOUSEEVENTF_LEFTUP, x, y, 0, 0);

            //-- log action
            A_Handler.Log.LogEntry.addLogEntry("KeyUp LMB(" + x + "," + y + ")");
            //
        }


        public static void RightUp(uint x, uint y)
        {
            mouse_event(MOUSEEVENTF_RIGHTUP, x, y, 0, 0);

            //-- log action
            A_Handler.Log.LogEntry.addLogEntry("KeyUp RMB(" + x + "," + y + ")");
            //
        }


        public static void MoveCursor(uint x, uint y)
        {
            Cursor.Position = new Point((int)x, (int)y);

            //-- log action
            A_Handler.Log.LogEntry.addLogEntry("MouseMove (" + x + "," + y + ")");
            //
        }

        public static void MoveCursor(int xLow, int yLow, int xHigh, int yHigh)
        {
            Random random = new Random();
            uint x = (uint) random.Next(xLow + 10, xHigh - 10);
            uint y = (uint) random.Next(yLow + 10, yHigh - 10);

            Cursor.Position = new Point((int) x, (int) y);

            //-- log action
            A_Handler.Log.LogEntry.addLogEntry("MouseMove (" + x + "," + y + ")");
            //
        }
    }
}
