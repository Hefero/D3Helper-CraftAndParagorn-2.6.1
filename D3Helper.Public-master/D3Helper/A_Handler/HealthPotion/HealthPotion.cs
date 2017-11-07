using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D3Helper.A_Handler.HealthPotion
{
    class HealthPotion
    {
        public static void handlePotion()
        {
            try
            {
                if (Properties.Settings.Default.AutoPotionBool && A_Collection.SkillCastConditions.Custom.CustomDefinitions.FirstOrDefault(x => x.Power.PowerSNO == 0) == null)
                {
                    if (!A_Collection.Me.HeroStates.isInTown && A_Collection.Me.HeroGlobals.LocalACD != null &&
                        A_Collection.Me.HeroStates.isInGame && A_Collection.Environment.Scene.GameTick > 1 &&
                        A_Collection.Environment.Scene.Counter_CurrentFrame != 0)
                    {
                        use_Potion();
                    }
                }
            }
            catch (Exception e)
            {
                A_Handler.Log.Exception.addExceptionLogEntry(e, A_Enums.ExceptionThread.Handler);
            }
        }


        private static void use_Potion()
        {
            try
            {
                if(A_Collection.Me.HeroDetails.Hitpoints_Percentage <= Properties.Settings.Default.AutoPotionValue)
                {
                    if(!A_Tools.Skills.Skills.S_Global.isOnCooldown(A_Enums.Powers.DrinkHealthPotion))
                    {
                        A_Tools.InputSimulator.IS_Keyboard.PressKey(A_Enums.ActionBarSlot.Potion);
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
