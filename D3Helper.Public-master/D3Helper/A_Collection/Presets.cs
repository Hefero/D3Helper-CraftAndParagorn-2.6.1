using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using D3Helper.A_Enums;

using ProtoBuf;
using System.Windows.Forms;

namespace D3Helper.A_Collection
{
    
    public class Rune
    {
        public Rune(string name, int runeindex, DamageType damagetype)
        {
            this.Name = name;
            this.RuneIndex = runeindex;
            this._DamageType = damagetype;
        }
        
        public string Name { get; set; }
        
        public int RuneIndex { get; set; }
        
        public DamageType _DamageType { get; set; }
    }
    
    public class SkillPower
    {
        public SkillPower(int powerSNO, string name, List<Rune> runes, int resourceCost, bool isPrimaryResource, bool isCooldownSpell)
        {
            this.PowerSNO = powerSNO;
            this.Name = name;
            this.Runes = runes;
            this.ResourceCost = resourceCost;
            this.IsPrimaryResource = isPrimaryResource;
            this.IsCooldownSpell = isCooldownSpell;
        }
        
        public int PowerSNO { get; set; }
        
        public string Name { get; set; }
        
        public List<Rune> Runes { get; set; }
        
        public int ResourceCost { get; set; }
        
        public bool IsPrimaryResource { get; set; }

        public bool IsCooldownSpell { get; set; }

    }
    
    public class SkillData
    {
        public SkillData(SkillPower power, string name, Rune selectedRune, List<CastCondition> castConditions)
        {
            this.Power = power;
            this.Name = name;
            this.SelectedRune = selectedRune;
            this.CastConditions = castConditions;
        }
        
        public SkillPower Power { get; set; }
        
        public string Name { get; set; }
        
        public Rune SelectedRune { get; set; }
        
        public List<CastCondition> CastConditions { get; set; } //contains all castconditions! :S



        private static List<CastCondition> SortGroup(List<CastCondition> ConditionGroup)
        {
            if (ConditionGroup.FirstOrDefault(x => x.Type.ToString().Contains("Property")) == null)
                return ConditionGroup;

            if (ConditionGroup.Last().Type.ToString().Contains("Property"))
                return ConditionGroup;

            var Properties = ConditionGroup.Where(x => x.Type.ToString().Contains("Property")).ToList();

            if (Properties.Count() > 0)
            {
                ConditionGroup.RemoveAll(x => x.Type.ToString().Contains("Property"));

                ConditionGroup.AddRange(Properties);
            }


            return ConditionGroup;
        }

        public void removeGroup(int groupId)
        {
            //iterate in reverse order to remove item from List
            for (int i = CastConditions.Count - 1; i >= 0; i--)
            {
                if (CastConditions[i].ConditionGroup == groupId)
                    CastConditions.RemoveAt(i);
            }
        }

        public int getNewGroupId()
        {
            int new_group_id = -1;

            foreach(CastCondition c in CastConditions)
            {
                if(c.ConditionGroup > new_group_id)
                {
                    new_group_id = c.ConditionGroup;
                }
            }

            return new_group_id + 1;
        }

        public List<ConditionGroup> getConditionGroups()
        {

            List<ConditionGroup> list = new List<ConditionGroup>();

            this.CastConditions = SortGroup(this.CastConditions);

            foreach (CastCondition c in CastConditions)
            {
                if (c.ConditionGroup != -1)
                {
                    List<CastCondition> c_list_by_group = CastConditions.Where(x => x.ConditionGroup == c.ConditionGroup).ToList();

                    ConditionGroup group = new ConditionGroup();
                    group.CastConditions = c_list_by_group;
                    group.ConditionGroupValue = c.ConditionGroup;
                    group.Name = "Conditiongroup " + group.ConditionGroupValue.ToString();

                    //condition group already exist?
                    if (list.FindIndex(x => x.ConditionGroupValue == c.ConditionGroup) < 0)
                    {
                        list.Add(group);
                    }
                }
            };

            return list;
        }

        public System.Drawing.Image getIcon()
        {

            try{
                System.Drawing.Image icon = Properties.Resources.ResourceManager.GetObject(this.Power.Name.ToLower()) as System.Drawing.Image;
                return icon;
            }
            catch(Exception e){
                A_Handler.Log.Exception.addExceptionLogEntry(e, A_Enums.ExceptionThread.Handler);
            }
            return null;
        }

    }
    
    public class CastCondition
    {
        
        public CastCondition(int conditionGroup, ConditionType type, double[] values, ConditionValueName[] valuenames)
        {
            this.ConditionGroup = conditionGroup;
            this.Type = type;
            this.Values = values;
            this.ValueNames = valuenames;

            this.comment = null;
            this.enabled = true;

            this.version = 260;
        }
        
        public int ConditionGroup { get; set; }
        
        public ConditionType Type { get; set; }
        
        public double[] Values { get; set; }
        
        public ConditionValueName[] ValueNames { get; set; }

        public string comment { get; set; }

        public bool enabled { get; set; }

        public int version { get; set; }


        //displaytext for listbox with some extended condition information in brackets
        public string DisplayText
        {
            get
            {
                try
                {
                    List<string> value_strings = new List<string>();

                    int i = 0;
                    foreach (double v in Values)
                    {
                        if (ValueNames[i].Equals(ConditionValueName.PowerSNO))
                        {
                            var _s = ValueNames[i] + ":" +  getSNOTooltipText(); //show resolved sno text if possible
                            value_strings.Add(_s);
                        }else
                        {
                            var _s = ValueNames[i] + ":" + v;
                            value_strings.Add(_s);
                        }
                        i++;
                    }

                    string display_text =  Type + " => [" + string.Join(" / ", value_strings) + "]";

                    if(comment!= null && comment.Trim().Length > 0)
                    {
                        display_text += " //" + comment;
                    }

                    return display_text;
                }catch(Exception) { }

                return Type.ToString();
            }
        }


        public string getSNOTooltipText()
        {
            try
            {
                if (this.ValueNames.Contains(ConditionValueName.PowerSNO))
                {
                    return A_Collection.Presets.SNOPowers.getPowerName((int)this.Values.First());
                }
            }catch(Exception e)
            {
                A_Handler.Log.Exception.addExceptionLogEntry(e, A_Enums.ExceptionThread.Handler);
            }

            return "";
        }


    }


    public class ConditionGroup
    {
        public string Name { get; set; }

        public int ConditionGroupValue { get; set; }

        public List<CastCondition> CastConditions { get; set; }

    }


    public class Presets
    {
        public class DefaultCastConditions
        {
            public static List<CastCondition> _Default_CastConditions = new List<CastCondition>()
        {
            new CastCondition(0, ConditionType.Player_Skill_IsNotOnCooldown, new double[] {0}, new ConditionValueName[] {ConditionValueName.Bool}),
            new CastCondition(0, ConditionType.Player_IsBuffActive, new double[] {0,0}, new ConditionValueName[] {ConditionValueName.PowerSNO, ConditionValueName.AttribID}),
            new CastCondition(0, ConditionType.Player_IsBuffNotActive, new double[] {0,0}, new ConditionValueName[] {ConditionValueName.PowerSNO, ConditionValueName.AttribID}),
            new CastCondition(0, ConditionType.Player_MinPrimaryResource, new double[] {0}, new ConditionValueName[] {ConditionValueName.Value}),
            new CastCondition(0, ConditionType.Player_MinSecondaryResource, new double[] {0}, new ConditionValueName[] {ConditionValueName.Value}),
            new CastCondition(0, ConditionType.World_MonstersInRange, new double[] {0,0,0}, new ConditionValueName[] {ConditionValueName.Distance, ConditionValueName.Value, ConditionValueName.Bool}),
            new CastCondition(0, ConditionType.World_EliteInRange, new double[] {0,0,0}, new ConditionValueName[] {ConditionValueName.Distance, ConditionValueName.Value, ConditionValueName.Bool, }),
            new CastCondition(0, ConditionType.World_BossInRange, new double[] {0,0,0}, new ConditionValueName[] {ConditionValueName.Distance, ConditionValueName.Value , ConditionValueName.Bool, }),
            new CastCondition(0, ConditionType.Player_BuffTicksLeft, new double[] {0,0,0}, new ConditionValueName[] {ConditionValueName.PowerSNO, ConditionValueName.AttribID, ConditionValueName.Value}),
            new CastCondition(0, ConditionType.Player_IsMonsterSelected, new double[] {0}, new ConditionValueName[] {ConditionValueName.Bool}),
            new CastCondition(0, ConditionType.SelectedMonster_IsBuffActive, new double[] {0}, new ConditionValueName[] {ConditionValueName.PowerSNO}),
            new CastCondition(0, ConditionType.SelectedMonster_IsBuffNotActive, new double[] {0}, new ConditionValueName[] {ConditionValueName.PowerSNO}),
            new CastCondition(0, ConditionType.MonstersInRange_IsBuffActive, new double[] {0,0,0}, new ConditionValueName[] {ConditionValueName.PowerSNO, ConditionValueName.Value, ConditionValueName.Bool, }),
            new CastCondition(0, ConditionType.MonstersInRange_IsBuffNotActive, new double[] {0,0,0}, new ConditionValueName[] {ConditionValueName.PowerSNO, ConditionValueName.Value, ConditionValueName.Bool, }),
            new CastCondition(0, ConditionType.Player_MinPrimaryResourcePercentage, new double[] {0}, new ConditionValueName[] {ConditionValueName.Value}),
            new CastCondition(0, ConditionType.Player_MinSecondaryResourcePercentage, new double[] {0}, new ConditionValueName[] {ConditionValueName.Value}),
            new CastCondition(0, ConditionType.Player_MaxHitpointsPercentage, new double[] {0}, new ConditionValueName[] {ConditionValueName.Value}),
            new CastCondition(0, ConditionType.SelectedMonster_MonstersInRange, new double[] {0,0,0}, new ConditionValueName[] {ConditionValueName.Distance, ConditionValueName.Value, ConditionValueName.Bool, }),
            new CastCondition(0, ConditionType.Player_IsBuffCount, new double[] {0,0,0}, new ConditionValueName[] {ConditionValueName.PowerSNO, ConditionValueName.AttribID, ConditionValueName.Value}),
            new CastCondition(0, ConditionType.Player_IsNotBuffCount, new double[] {0,0,0}, new ConditionValueName[] {ConditionValueName.PowerSNO, ConditionValueName.AttribID, ConditionValueName.Value}),
            new CastCondition(0, ConditionType.PartyMember_InRangeIsBuff, new double[] {0,0,0,0,0}, new ConditionValueName[] {ConditionValueName.PowerSNO, ConditionValueName.AttribID, ConditionValueName.Distance, ConditionValueName.Value, ConditionValueName.Bool, }),
            new CastCondition(0, ConditionType.PartyMember_InRangeIsNotBuff, new double[] {0,0,0,0,0}, new ConditionValueName[] {ConditionValueName.PowerSNO, ConditionValueName.AttribID, ConditionValueName.Distance, ConditionValueName.Value, ConditionValueName.Bool,}),
            new CastCondition(0, ConditionType.Party_AllInRange, new double[] {0}, new ConditionValueName[] {ConditionValueName.Distance}),
            new CastCondition(0, ConditionType.Party_NotAllInRange, new double[] {0}, new ConditionValueName[] {ConditionValueName.Distance}),
            new CastCondition(0, ConditionType.Party_AllAlive, new double[] {0}, new ConditionValueName[] {ConditionValueName.Bool}),
            new CastCondition(0, ConditionType.World_IsRift, new double[] {0}, new ConditionValueName[] {ConditionValueName.Bool}),
            new CastCondition(0, ConditionType.World_IsGRift, new double[] {0}, new ConditionValueName[] {ConditionValueName.Bool}),
            new CastCondition(0, ConditionType.PartyMember_InRangeMinHitpoints, new double[] {0,0}, new ConditionValueName[] {ConditionValueName.Distance, ConditionValueName.Value}),
            new CastCondition(0, ConditionType.Player_Skill_MinCharges, new double[] {0}, new ConditionValueName[] {ConditionValueName.Value}),
            new CastCondition(0, ConditionType.Player_Skill_MinResource, new double[] {0}, new ConditionValueName[] {ConditionValueName.Bool}),
            new CastCondition(0, ConditionType.SelectedMonster_MinDistance, new double[] {0}, new ConditionValueName[] {ConditionValueName.Distance}),
            new CastCondition(0, ConditionType.SelectedMonster_MaxDistance, new double[] {0}, new ConditionValueName[] {ConditionValueName.Distance}),
            new CastCondition(0, ConditionType.Player_MaxPrimaryResource, new double[] {0}, new ConditionValueName[] {ConditionValueName.Value}),
            new CastCondition(0, ConditionType.Player_MaxSecondaryResource, new double[] {0}, new ConditionValueName[] {ConditionValueName.Value}),
            new CastCondition(0, ConditionType.Player_MaxPrimaryResourcePercentage, new double[] {0}, new ConditionValueName[] {ConditionValueName.Value}),
            new CastCondition(0, ConditionType.Player_MaxSecondaryResourcePercentage, new double[] {0}, new ConditionValueName[] {ConditionValueName.Value}),
            new CastCondition(0, ConditionType.Player_IsMoving, new double[] {0}, new ConditionValueName[] {ConditionValueName.Bool}),
            new CastCondition(0, ConditionType.Player_Pet_MinFetishesCount, new double[] {0}, new ConditionValueName[] {ConditionValueName.Value}),
            new CastCondition(0, ConditionType.Player_Pet_MinZombieDogsCount, new double[] {0}, new ConditionValueName[] {ConditionValueName.Value}),
            new CastCondition(0, ConditionType.Player_Pet_MinGargantuanCount, new double[] {0}, new ConditionValueName[] {ConditionValueName.Value}),
            new CastCondition(0, ConditionType.Player_Pet_MaxFetishesCount, new double[] {0}, new ConditionValueName[] {ConditionValueName.Value}),
            new CastCondition(0, ConditionType.Player_Pet_MaxZombieDogsCount, new double[] {0}, new ConditionValueName[] {ConditionValueName.Value}),
            new CastCondition(0, ConditionType.Player_Pet_MaxGargantuanCount, new double[] {0}, new ConditionValueName[] {ConditionValueName.Value}),
            new CastCondition(0, ConditionType.Player_Pet_MinSkeletalMageCount, new double[] {0}, new ConditionValueName[] {ConditionValueName.Value}),
            new CastCondition(0, ConditionType.Player_Pet_MaxSkeletalMageCount, new double[] {0}, new ConditionValueName[] {ConditionValueName.Value}),
            new CastCondition(0, ConditionType.SelectedMonster_IsElite, new double[] {0}, new ConditionValueName[] {ConditionValueName.Bool}),
            new CastCondition(0, ConditionType.SelectedMonster_IsBoss, new double[] {0}, new ConditionValueName[] {ConditionValueName.Bool}),
            new CastCondition(0, ConditionType.Player_Power_IsNotOnCooldown, new double[] {0}, new ConditionValueName[] {ConditionValueName.PowerSNO}),
            new CastCondition(0, ConditionType.Player_Power_IsOnCooldown, new double[] {0}, new ConditionValueName[] {ConditionValueName.PowerSNO}),
            new CastCondition(0, ConditionType.Player_HasSkillEquipped, new double[] {0}, new ConditionValueName[] {ConditionValueName.PowerSNO}),
            new CastCondition(0, ConditionType.MonstersInRange_HaveArcaneEnchanted, new double[] {0}, new ConditionValueName[] {ConditionValueName.Bool}),
            new CastCondition(0, ConditionType.MonstersInRange_HaveAvenger, new double[] {0}, new ConditionValueName[] {ConditionValueName.Bool}),
            new CastCondition(0, ConditionType.MonstersInRange_HaveDesecrator, new double[] {0}, new ConditionValueName[] {ConditionValueName.Bool}),
            new CastCondition(0, ConditionType.MonstersInRange_HaveElectrified, new double[] {0}, new ConditionValueName[] {ConditionValueName.Bool}),
            new CastCondition(0, ConditionType.MonstersInRange_HaveExtraHealth, new double[] {0}, new ConditionValueName[] {ConditionValueName.Bool}),
            new CastCondition(0, ConditionType.MonstersInRange_HaveFast, new double[] {0}, new ConditionValueName[] {ConditionValueName.Bool}),
            new CastCondition(0, ConditionType.MonstersInRange_HaveFrozen, new double[] {0}, new ConditionValueName[] {ConditionValueName.Bool}),
            new CastCondition(0, ConditionType.MonstersInRange_HaveHealthlink, new double[] {0}, new ConditionValueName[] {ConditionValueName.Bool}),
            new CastCondition(0, ConditionType.MonstersInRange_HaveIllusionist, new double[] {0}, new ConditionValueName[] {ConditionValueName.Bool}),
            new CastCondition(0, ConditionType.MonstersInRange_HaveJailer, new double[] {0}, new ConditionValueName[] {ConditionValueName.Bool}),
            new CastCondition(0, ConditionType.MonstersInRange_HaveKnockback, new double[] {0}, new ConditionValueName[] {ConditionValueName.Bool}),
            new CastCondition(0, ConditionType.MonstersInRange_HaveFirechains, new double[] {0}, new ConditionValueName[] {ConditionValueName.Bool}),
            new CastCondition(0, ConditionType.MonstersInRange_HaveMolten, new double[] {0}, new ConditionValueName[] {ConditionValueName.Bool}),
            new CastCondition(0, ConditionType.MonstersInRange_HaveMortar, new double[] {0}, new ConditionValueName[] {ConditionValueName.Bool}),
            new CastCondition(0, ConditionType.MonstersInRange_HaveNightmarish, new double[] {0}, new ConditionValueName[] {ConditionValueName.Bool}),
            new CastCondition(0, ConditionType.MonstersInRange_HavePlagued, new double[] {0}, new ConditionValueName[] {ConditionValueName.Bool}),
            new CastCondition(0, ConditionType.MonstersInRange_HaveReflectsDamage, new double[] {0}, new ConditionValueName[] {ConditionValueName.Bool}),
            new CastCondition(0, ConditionType.MonstersInRange_HaveShielding, new double[] {0}, new ConditionValueName[] {ConditionValueName.Bool}),
            new CastCondition(0, ConditionType.MonstersInRange_HaveTeleporter, new double[] {0}, new ConditionValueName[] {ConditionValueName.Bool}),
            new CastCondition(0, ConditionType.MonstersInRange_HaveThunderstorm, new double[] {0}, new ConditionValueName[] {ConditionValueName.Bool}),
            new CastCondition(0, ConditionType.MonstersInRange_HaveVortex, new double[] {0}, new ConditionValueName[] {ConditionValueName.Bool}),
            new CastCondition(0, ConditionType.MonstersInRange_HaveWaller, new double[] {0}, new ConditionValueName[] {ConditionValueName.Bool}),
            new CastCondition(0, ConditionType.MonstersInRange_HaveJuggernaut, new double[] {0}, new ConditionValueName[] {ConditionValueName.Bool}),
            new CastCondition(0, ConditionType.Player_HasSkillNotEquipped, new double[] {0}, new ConditionValueName[] {ConditionValueName.PowerSNO}),
            new CastCondition(0, ConditionType.SelectedMonster_IsBuffCount, new double[] {0,0,0}, new ConditionValueName[] {ConditionValueName.PowerSNO, ConditionValueName.AttribID, ConditionValueName.Value}),
            new CastCondition(0, ConditionType.SelectedMonster_IsNotBuffCount, new double[] {0,0,0}, new ConditionValueName[] {ConditionValueName.PowerSNO, ConditionValueName.AttribID, ConditionValueName.Value}),
            new CastCondition(0, ConditionType.MonstersInRange_IsBuffCount, new double[] {0,0,0}, new ConditionValueName[] {ConditionValueName.PowerSNO, ConditionValueName.AttribID, ConditionValueName.Value}),
            new CastCondition(0, ConditionType.MonstersInRange_IsNotBuffCount, new double[] {0,0,0}, new ConditionValueName[] {ConditionValueName.PowerSNO, ConditionValueName.AttribID, ConditionValueName.Value}),
            new CastCondition(0, ConditionType.Player_IsDestructableSelected, new double[] {0}, new ConditionValueName[] {ConditionValueName.Bool}),
            new CastCondition(0, ConditionType.Key_ForceStandStill, new double[] {0}, new ConditionValueName[] {ConditionValueName.Bool}),
            new CastCondition(0, ConditionType.Key_ForceMove, new double[] {0}, new ConditionValueName[] {ConditionValueName.Bool}),
            new CastCondition(0, ConditionType.Add_Property_TimedUse, new double[] {0}, new ConditionValueName[] {ConditionValueName.Value}),
            new CastCondition(0, ConditionType.SelectedMonster_MonstersInRange_IsBuffActive, new double[] {0,0,0}, new ConditionValueName[] {ConditionValueName.PowerSNO, ConditionValueName.Value, ConditionValueName.Bool, }),
            new CastCondition(0, ConditionType.SelectedMonster_MonstersInRange_IsBuffNotActive, new double[] {0,0,0}, new ConditionValueName[] {ConditionValueName.PowerSNO, ConditionValueName.Value, ConditionValueName.Bool, }),
            new CastCondition(0, ConditionType.Player_MinAPS, new double[] {0}, new ConditionValueName[] {ConditionValueName.Value}),
            new CastCondition(0, ConditionType.Add_Property_Channeling, new double[] {1, -1}, new ConditionValueName[] {ConditionValueName.Bool, ConditionValueName.Value, }),
            new CastCondition(0, ConditionType.Add_Property_APSSnapShot, new double[] {1}, new ConditionValueName[] {ConditionValueName.Bool, }),
            new CastCondition(0, ConditionType.MonstersInRange_MinHitpointsPercentage, new double[] {0,0,0}, new ConditionValueName[] {ConditionValueName.Value, ConditionValueName.Value, ConditionValueName.Bool, }),
            new CastCondition(0, ConditionType.MonstersInRange_MaxHitpointsPercentage, new double[] {0,0,0}, new ConditionValueName[] {ConditionValueName.Value, ConditionValueName.Value, ConditionValueName.Bool,  }),
            new CastCondition(0, ConditionType.SelectedMonster_MonstersInRange_MinHitpointsPercentage, new double[] {0,0,0}, new ConditionValueName[] {ConditionValueName.Value, ConditionValueName.Value, ConditionValueName.Bool, }),
            new CastCondition(0, ConditionType.SelectedMonster_MonstersInRange_MaxHitpointsPercentage, new double[] {0,0,0}, new ConditionValueName[] {ConditionValueName.Value, ConditionValueName.Value, ConditionValueName.Bool,  }),
            new CastCondition(0, ConditionType.SelectedMonster_MinHitpointsPercentage, new double[] {0}, new ConditionValueName[] {ConditionValueName.Value}),
            new CastCondition(0, ConditionType.SelectedMonster_MaxHitpointsPercentage, new double[] {0}, new ConditionValueName[] {ConditionValueName.Value}),
            new CastCondition(0, ConditionType.Player_StandStillTime, new double[] {0, 0}, new ConditionValueName[] {ConditionValueName.Value, ConditionValueName.Bool, }),
            new CastCondition(0, ConditionType.MonstersInRange_RiftProgress, new double[] {0, 0}, new ConditionValueName[] {ConditionValueName.Value, ConditionValueName.Bool, }),
            new CastCondition(0, ConditionType.SelectedMonster_MonstersInRange_RiftProgress, new double[] {0, 0}, new ConditionValueName[] {ConditionValueName.Value, ConditionValueName.Bool, }),
            new CastCondition(0, ConditionType.SelectedMonster_RiftProgress, new double[] {0, 0}, new ConditionValueName[] {ConditionValueName.Value, ConditionValueName.Bool, }),
            };
        }

        public class SkillPowers
        {
            public static List<SkillPower> AllSkillPowers = new List<SkillPower>(); // All Skill Powers from power_stats.txt
        }

        public class SNOPowers
        {
            public static Dictionary<int, string> AllPowers = new Dictionary<int, string>();    // All Powers from Powers.txt
            public static Dictionary<int, string> CustomPowerNames = new Dictionary<int, string>(); 

            public static string getPowerName(int PowerSNO)
            {
                var DefaultName = A_Collection.Presets.SNOPowers.AllPowers.FirstOrDefault(x => x.Key == PowerSNO);
                var CustomName = A_Collection.Presets.SNOPowers.CustomPowerNames.FirstOrDefault(x => x.Key == PowerSNO);

                string Name = DefaultName.Value;
                if (CustomName.Key != default(int))
                    Name = CustomName.Value;

                return Name;
            }

        }
        public class Monsters
        {
            public static Dictionary<int, double> Monsters_RiftProgress = new Dictionary<int, double>(); // MonsterSNO + ProgressPoints from monster.txt 
        }

        public class Manual
        {
            public class Tooltips
            {
                public static Dictionary<ConditionType, string> ConditionTypes = new Dictionary<ConditionType, string>(); 
            }
        }

        public class ConventionElements
        {
            public static readonly A_Enums.DamageType[] Convention_ElementRotation = new A_Enums.DamageType[]
            {
                A_Enums.DamageType.Arcane,
                A_Enums.DamageType.Cold,
                A_Enums.DamageType.Fire,
                A_Enums.DamageType.Holy,
                A_Enums.DamageType.Lightning,
                A_Enums.DamageType.Physical,
                A_Enums.DamageType.Poison
            };

            public static readonly int[] Convention_BuffcountAttribIdRotation = new[]
            {
                747,
                748,
                749,
                750,
                751,
                752,
                753
            };

            public static readonly int[] Convention_BuffEndtickAttribIdRotation = new[]
            {
                610,
                611,
                612,
                613,
                614,
                615,
                616
            };

            public static readonly Dictionary<A_Enums.HeroClass, A_Enums.DamageType[]> Convention_ClassElements = new Dictionary<A_Enums.HeroClass, A_Enums.DamageType[]>()
            {
                {A_Enums.HeroClass.Barbarian, new A_Enums.DamageType[] { A_Enums.DamageType.Cold, A_Enums.DamageType.Fire, A_Enums.DamageType.Lightning, A_Enums.DamageType.Physical, }},
                {A_Enums.HeroClass.Crusader, new A_Enums.DamageType[] { A_Enums.DamageType.Fire, A_Enums.DamageType.Holy, A_Enums.DamageType.Lightning, A_Enums.DamageType.Physical, }},
                {A_Enums.HeroClass.DemonHunter, new A_Enums.DamageType[] { A_Enums.DamageType.Cold, A_Enums.DamageType.Fire, A_Enums.DamageType.Lightning, A_Enums.DamageType.Physical, }},
                {A_Enums.HeroClass.Monk, new A_Enums.DamageType[] { A_Enums.DamageType.Cold, A_Enums.DamageType.Fire, A_Enums.DamageType.Holy, A_Enums.DamageType.Lightning, A_Enums.DamageType.Physical, }},
                {A_Enums.HeroClass.Witchdoctor, new A_Enums.DamageType[] { A_Enums.DamageType.Cold, A_Enums.DamageType.Fire, A_Enums.DamageType.Physical, A_Enums.DamageType.Poison, }},
                {A_Enums.HeroClass.Wizard, new A_Enums.DamageType[] {A_Enums.DamageType.Arcane, A_Enums.DamageType.Cold, A_Enums.DamageType.Fire, A_Enums.DamageType.Lightning}}
            };
        }
    }
}
