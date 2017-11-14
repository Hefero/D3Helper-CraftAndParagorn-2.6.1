using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using Enigma.D3;
using Enigma.D3.Helpers;

namespace D3Helper.A_Collector
{
    class H_D3Client
    {
        public static void Collect()
        {
            try
            {
                get_isForeground();
                get_D3ClientRect();
            }
            catch (Exception e)
            {
                A_Handler.Log.Exception.addExceptionLogEntry(e, A_Enums.ExceptionThread.ECollector);
            }
        }

        private static void get_isForeground()
        {
            try
            {
                A_Collection.D3Client.Window.isForeground = A_Tools.T_D3Client.IsForeground();
            }
            catch (Exception e)
            {
                A_Handler.Log.Exception.addExceptionLogEntry(e, A_Enums.ExceptionThread.ECollector);
            }
        }
        
        private static void get_D3ClientRect()
        {
            try
            {
                A_Collection.D3Client.Window.D3ClientRect = A_Tools.T_D3Client.getClient_Rect(Engine.Current.Process.MainWindowHandle);
            }
            catch (Exception e)
            {
                A_Handler.Log.Exception.addExceptionLogEntry(e, A_Enums.ExceptionThread.ECollector);
            }
        }
    }
}
