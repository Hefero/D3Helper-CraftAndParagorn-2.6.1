using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D3Helper.A_Handler.StatHandler
{
    class StatHandler
    {
        public static bool TotalXP_changed = false;


        public static void handleStats()
        {
            try
            {
                if (TotalXP_changed)
                {
                    get_StatsXP();

                    TotalXP_changed = false;
                }
                get_NewRun();
            }
            catch (Exception e)
            {
                A_Handler.Log.Exception.addExceptionLogEntry(e, A_Enums.ExceptionThread.Handler);
            }
        }


        private static void get_StatsXP()
        {
            try
            {
                int currentparagon = A_Collection.Me.HeroGlobals.Alt_Lvl;

                A_Collection.Stats.Player.NextRoundedHundred = Math.Ceiling((double)currentparagon / 100) * 100;

                if (currentparagon == A_Collection.Stats.Player.NextRoundedHundred)
                {
                    A_Collection.Stats.Player.NextRoundedHundred = A_Collection.Stats.Player.NextRoundedHundred + 100;
                }

                A_Collection.Stats.Player.NextRoundedHundredTotalXP = A_Enums.ParagonXPTable.TotalXp[(int)A_Collection.Stats.Player.NextRoundedHundred];

                double xptotalleft = A_Collection.Stats.Player.NextRoundedHundredTotalXP - A_Collection.Stats.Player.TotalXP;

                A_Collection.Stats.Player.Progression = (A_Collection.Stats.Player.TotalXP / A_Collection.Stats.Player.NextRoundedHundredTotalXP) * 100;
                

            }
            catch (Exception e)
            {
                A_Handler.Log.Exception.addExceptionLogEntry(e, A_Enums.ExceptionThread.Handler);
            }
        }


        private static void get_NewRun()
        {
            try
            {
                if (!A_Collection.Me.HeroStates.isInGame)
                {
                    
                }
            }
            catch (Exception e)
            {
                A_Handler.Log.Exception.addExceptionLogEntry(e, A_Enums.ExceptionThread.Handler);
            }
        }
    }
}
