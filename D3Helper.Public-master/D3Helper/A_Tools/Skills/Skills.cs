using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Enigma.D3.Helpers;

namespace D3Helper.A_Tools.Skills
{
    class Skills
    {
        public class S_Global
        {
            public static int get_EquippedSkillRune(int SkillPowerSnoId)
            {
                try
                {
                    lock (A_Collection.Me.HeroDetails.ActiveSkills)
                    {
                        if (A_Collection.Me.HeroDetails.ActiveSkills.Count >= 5)
                        {
                            int Rune = A_Collection.Me.HeroDetails.ActiveSkills[SkillPowerSnoId];

                            return Rune;
                        }
                        return - 1;
                    }
                }
                catch { return -1; }
            }
            public static bool isOnCooldown(int PowerSnoId)
            {
                try
                {
                    lock(A_Collection.Me.HeroGlobals.LocalACD)
                    {
                        if (A_Collection.Me.HeroGlobals.LocalACD != null && A_Collection.Me.HeroGlobals.LocalACD.x180_Hitpoints > 0)
                        {
                            var cooldown = A_Collection.Me.HeroGlobals.LocalACD.GetAttributeValue(Enigma.D3.Enums.AttributeId.PowerCooldown, PowerSnoId);
                            
                            if (cooldown == -1)
                            {
                                return false;
                            }
                        }
                        return true;
                    }
                }
                catch { return true; }
            }
            public static int get_Charges(int PowerSnoId)
            {
                try
                {
                    lock(A_Collection.Me.HeroGlobals.LocalACD)
                    {
                        return (int)A_Collection.Me.HeroGlobals.LocalACD.GetAttributeValue(Enigma.D3.Enums.AttributeId.SkillCharges, PowerSnoId);
                    }
                }
                catch { return 0; }
            }
        }
    }
}
