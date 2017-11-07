using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using D3Helper.A_Tools;
using D3Helper.A_Collection;
using D3Helper.A_Enums;

namespace D3Helper.A_Handler.EventHandler
{
    class EventHandler
    {
        public static void handleEvents()
        {
            try
            {
                start_AutoGamble();
            }
            catch (Exception e)
            {
                A_Handler.Log.Exception.addExceptionLogEntry(e, A_Enums.ExceptionThread.Handler);
            }
        }


        private static void start_AutoGamble()
        {
            try
            {
                if (Properties.Settings.Default.AutoGambleBool)
                {
                    if (A_Collection.Me.HeroStates.isInTown)
                    {
                        A_Handler.AutoGamble.AutoGamble.start_Gamble();
                    }
                }
            }
            catch (Exception e)
            {
                A_Handler.Log.Exception.addExceptionLogEntry(e, A_Enums.ExceptionThread.Handler);
            }
        }
    }
}
