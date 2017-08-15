using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using D3Helper.A_Collection;
using D3Helper.A_Collector;
using D3Helper.A_Enums;
using D3Helper.A_Handler.Log;
using D3Helper.A_Tools;
using Enigma.D3;
using Environment = D3Helper.A_Collection.Environment;
using Exception = System.Exception;

namespace D3Helper.A_Handler.SkillHandler
{
    class Aggregator_CastConditions
    {
        public static bool CanCast(SkillPower Skill, int EquippedRune, out bool IsTimedCast, out bool ShouldChannel, out int ChannelTicks)
        {
            IsTimedCast = false;
            ShouldChannel = false;
            ChannelTicks = 0;

            try
            {
                
                //-- get Default Definition
                //List<SkillData> Default_Definition = SkillCastConditions.Default.DefaultDefinitions.Where(x => x.Power.PowerSNO == Skill.PowerSNO).ToList();
                //
                //-- try get Custom Definition
                List<SkillData> Custom_Definitions = SkillCastConditions.Custom.CustomDefinitions.Where(x => x.Power.PowerSNO == Skill.PowerSNO && x.SelectedRune.RuneIndex == -1).ToList();
                //
                //-- try get Custom Definition rune specific
                List<SkillData> RuneSpecific_Custom_Definitions = SkillCastConditions.Custom.CustomDefinitions.Where(x => x.Power.PowerSNO == Skill.PowerSNO && x.SelectedRune.RuneIndex == EquippedRune).ToList();
                //
                
                if (RuneSpecific_Custom_Definitions.Count > 0)
                    return CouldCast(RuneSpecific_Custom_Definitions, EquippedRune, out IsTimedCast, out ShouldChannel, out ChannelTicks);

                if (Custom_Definitions.Count > 0)
                    return CouldCast(Custom_Definitions, EquippedRune, out IsTimedCast, out ShouldChannel, out ChannelTicks);
                
                
                return false;
            }
            catch { return false; }
        }
        private static bool CouldCast(List<SkillData> Definitions, int EquippedRune, out bool IsTimedCast, out bool ShouldChannel, out int ChannelTicks)
        {
            IsTimedCast = false;
            ShouldChannel = false;
            ChannelTicks = 0;

            try
            {
               
                foreach (var definition in Definitions)
                {
                    Dictionary<int, List<CastCondition>> ConditionGroups = get_ConditionGroups(definition.CastConditions);

                    ConditionGroups = SortGroups(ConditionGroups);

                    foreach (var group in ConditionGroups)
                    {
                        bool validgroup = true;

                        
                        foreach(var condition in group.Value)
                        {
                            validgroup = evaluate_Condition(condition, group.Value, definition.Power, EquippedRune, out IsTimedCast, out ShouldChannel, out ChannelTicks);

                            if (!validgroup)
                                break;
                        }

                        if (validgroup)
                        {
                            //-- log action
                            //if (Properties.Settings.Default.Logger_extendedLog)
                            //{
                            //    lock (A_Handler.Log.Exception.HandlerLog)
                            //    {
                            //        A_Handler.Log.Exception.HandlerLog.Add(new LogEntry(DateTime.Now,
                            //            "Valid Conditions to cast Skill (" + definition.Power.Name + ")"));
                            //        A_Handler.Log.Exception.HandlerLog.Add(new LogEntry(DateTime.Now,
                            //            "using Definition (" + definition.Name + ")"));
                            //    }
                            //}
                            //

                            return true;
                        }
                    }
                }

                return false;
            }
            catch { return false; }
        }

        private static Dictionary<int, List<CastCondition>> SortGroups(
            Dictionary<int, List<CastCondition>> ConditionGroups)
        {
            var Buffer = ConditionGroups.ToDictionary(x => x.Key, y => y.Value);

            foreach (var group in Buffer)
            {
                if(group.Value.FirstOrDefault(x => x.Type.ToString().Contains("Property")) == null)
                    continue;

                if(group.Value.Last().Type.ToString().Contains("Property"))
                    continue;

                var Properties = group.Value.Where(x => x.Type.ToString().Contains("Property")).ToList();

                if (Properties.Count() > 0)
                {
                    ConditionGroups.FirstOrDefault(x => x.Key == group.Key)
                        .Value.RemoveAll(x => x.Type.ToString().Contains("Property"));

                    ConditionGroups.FirstOrDefault(x => x.Key == group.Key)
                        .Value.AddRange(Properties);
                }
            }

            return ConditionGroups;
        }

        private static Dictionary<int, List<CastCondition>> get_ConditionGroups(List<CastCondition> Conditions)
        {
            try
            {
                Dictionary<int,List<CastCondition>> ConditionGroups = new Dictionary<int,List<CastCondition>> ();
                
                foreach (var condition in Conditions)
                {
                    if(!ConditionGroups.ContainsKey(condition.ConditionGroup))
                        ConditionGroups.Add(condition.ConditionGroup, new List<CastCondition>());

                    ConditionGroups[condition.ConditionGroup].Add(condition);
                }

                return ConditionGroups;
            }
            catch { return new Dictionary<int, List<CastCondition>>(); }
        }


        private static bool evaluate_Condition(CastCondition Condition, List<CastCondition> ConditionGroup , SkillPower _SkillPower, int EquippedRune, out bool IsTimedCast, out bool ShouldChannel, out int ChannelTicks)
        {
            IsTimedCast = false;
            ShouldChannel = false;
            ChannelTicks = 0;

            try
            {
                //
                SkillPower SkillPower = new SkillPower(_SkillPower.PowerSNO, _SkillPower.Name, _SkillPower.Runes, _SkillPower.ResourceCost, _SkillPower.IsPrimaryResource, _SkillPower.IsCooldownSpell);

                SkillPower.PowerSNO = getRealPowerSNO(SkillPower.PowerSNO);
                //

                List<ActorCommonData> SelectedMonster_MonstersInRange;
                lock (A_Collection.Environment.Actors.SelectedMonster_MonstersInRange)
                    SelectedMonster_MonstersInRange = A_Collection.Environment.Actors.SelectedMonster_MonstersInRange.ToList();

                List<ActorCommonData> MonstersInRange;
                lock (A_Collection.Environment.Actors.MonstersInRange)
                    MonstersInRange = A_Collection.Environment.Actors.MonstersInRange.ToList();

                //bool isFireReduce = false;

                //List<SkillPower> AllSkillPowers;
                //lock (A_Collection.Presets.SkillPowers.AllSkillPowers)
                //    AllSkillPowers = A_Collection.Presets.SkillPowers.AllSkillPowers.ToList();

                //if (
                //    AllSkillPowers.FirstOrDefault(
                //        x =>
                //            x.PowerSNO == SkillPower.PowerSNO &&
                //            x.Runes.FirstOrDefault(y => y.RuneIndex == EquippedRune)._DamageType == DamageType.Fire) !=
                //    null && A_Tools.T_LocalPlayer.isBuff((int)318790))
                //    isFireReduce = true;


                //-----------------------------------------------------------------------

                //return false if whole conditiongroup is disabled
                bool allConditionsDisabled = true;
                foreach(CastCondition c in ConditionGroup)
                {
                    if (c.enabled)
                    {
                        allConditionsDisabled = false;
                        break;
                    }
                }

                if (allConditionsDisabled)
                {
                    return false;
                }



                //ignore this condition only?
                if (!Condition.enabled)
                {
                    return true;
                }

                //-----------------------------------------------------------------------

                
                List<double> Values = Condition.Values.ToList();

                switch (Condition.Type)
                {
                    case ConditionType.MonstersInRange_IsBuffActive:
                        switch ((int) Values[2])
                        {
                            case 0:
                                return
                                    MonstersInRange.Count(monster => T_ACD.isBuff((int) Values[0], monster)) <=
                                    Values[1];

                            case 1:
                                return MonstersInRange.Count(monster => T_ACD.isBuff((int) Values[0], monster)) >=
                                       Values[1];

                            default:
                                return false;
                        }

                    case ConditionType.MonstersInRange_IsBuffNotActive:
                        switch ((int) Values[2])
                        {
                            case 0:
                                return
                                    MonstersInRange.Count(monster => !T_ACD.isBuff((int) Values[0], monster)) <=
                                    Values[1];

                            case 1:
                                return MonstersInRange.Count(monster => !T_ACD.isBuff((int) Values[0], monster)) >=
                                       Values[1];

                            default:
                                return false;
                        }

                    case ConditionType.Player_BuffTicksLeft:
                        int ticks = A_Tools.T_LocalPlayer.get_BuffTicksLeft((int) Values[0], (int) Values[1]);
                        return ticks <= Values[2];

                    case ConditionType.Player_IsBuffActive:
                        return A_Tools.T_LocalPlayer.isBuff((int) Values[0], (int) Values[1]);

                    case ConditionType.Player_IsBuffNotActive:
                        return !A_Tools.T_LocalPlayer.isBuff((int) Values[0], (int) Values[1]);

                    case ConditionType.Player_IsMonsterSelected:
                        if (Values[0] == 1)
                            return Me.HeroDetails.SelectedMonsterACD != null;
                        return Me.HeroDetails.SelectedMonsterACD == null;

                    case ConditionType.Player_MinPrimaryResource:
                        //if (isFireReduce)
                        //    return Me.HeroDetails.ResourcePrimary*1.3 >= Values[0];
                        return Me.HeroDetails.ResourcePrimary >= Values[0];

                    case ConditionType.Player_MinSecondaryResource:
                        //if (isFireReduce)
                        //    return Me.HeroDetails.ResourceSecondary*1.3 >= Values[0];
                        return Me.HeroDetails.ResourceSecondary >= Values[0];

                    case ConditionType.Player_Skill_IsNotOnCooldown:
                        switch ((int) Values[0])
                        {
                            case 1:
                                return !A_Tools.Skills.Skills.S_Global.isOnCooldown(SkillPower.PowerSNO);

                            case 0:
                                return A_Tools.Skills.Skills.S_Global.isOnCooldown(SkillPower.PowerSNO);

                            default:
                                return false;
                        }


                    case ConditionType.SelectedMonster_IsBuffActive:
                        if (Me.HeroDetails.SelectedMonsterACD != null)
                        {
                            if (A_Tools.T_ACD.isBuff((int) Values[0], Me.HeroDetails.SelectedMonsterACD))
                                return true;
                        }
                        return false;

                    case ConditionType.SelectedMonster_IsBuffNotActive:
                        if (Me.HeroDetails.SelectedMonsterACD != null)
                        {
                            if (!A_Tools.T_ACD.isBuff((int) Values[0], Me.HeroDetails.SelectedMonsterACD))
                                return true;
                        }
                        return false;

                    case ConditionType.World_BossInRange:
                        int _bosses = A_Tools.T_ACD.get_MonstersInRange((int) Values[0], false, true,
                            out A_Collection.Environment.Actors.MonstersInRange);
                        switch ((int) Values[2])
                        {
                            case 0:
                                return _bosses <= Values[1];

                            case 1:
                                return _bosses >= Values[1];

                            default:
                                return false;
                        }

                    case ConditionType.World_EliteInRange:
                        int elites = A_Tools.T_ACD.get_MonstersInRange((int) Values[0], true, false,
                            out A_Collection.Environment.Actors.MonstersInRange);
                        switch ((int) Values[2])
                        {
                            case 0:
                                return elites <= Values[1];

                            case 1:
                                return elites >= Values[1];

                            default:
                                return false;
                        }


                    case ConditionType.World_MonstersInRange:
                        int all = A_Tools.T_ACD.get_MonstersInRange((int) Values[0], false, false,
                            out A_Collection.Environment.Actors.MonstersInRange);
                        switch ((int) Values[2])
                        {
                            case 0:
                                return all <= Values[1];

                            case 1:
                                return all >= Values[1];

                            default:
                                return false;
                        }


                    case ConditionType.Player_MinPrimaryResourcePercentage:
                        //if (isFireReduce)
                        //    return Me.HeroDetails.ResourcePrimary_Percentage*1.3 >= Values[0];
                        return Me.HeroDetails.ResourcePrimary_Percentage >= Values[0];

                    case ConditionType.Player_MinSecondaryResourcePercentage:
                        //if (isFireReduce)
                        //    return Me.HeroDetails.ResourceSecondary_Percentage*1.3 >= Values[0];
                        return Me.HeroDetails.ResourceSecondary_Percentage >= Values[0];

                    case ConditionType.Player_MaxHitpointsPercentage:
                        return Me.HeroDetails.Hitpoints_Percentage <= Values[0];

                    case ConditionType.SelectedMonster_MonstersInRange:
                        if (Me.HeroDetails.SelectedMonsterACD != null)
                        {
                            switch ((int) Values[2])
                            {
                                case 0:
                                    return
                                        A_Tools.T_ACD.get_MonstersInRangeOfSelectedMonster(
                                            Me.HeroDetails.SelectedMonsterACD,
                                            Values[0],
                                            out A_Collection.Environment.Actors.SelectedMonster_MonstersInRange) <=
                                        Values[1];

                                case 1:
                                    return
                                        A_Tools.T_ACD.get_MonstersInRangeOfSelectedMonster(
                                            Me.HeroDetails.SelectedMonsterACD,
                                            Values[0],
                                            out A_Collection.Environment.Actors.SelectedMonster_MonstersInRange) >=
                                        Values[1];

                                default:
                                    return false;
                            }

                        }
                        return false;

                    case ConditionType.Player_IsBuffCount:
                        return A_Tools.T_LocalPlayer.getBuffCount((int) Values[0], (int) Values[1]) >= Values[2];

                    case ConditionType.Player_IsNotBuffCount:
                        return A_Tools.T_LocalPlayer.getBuffCount((int) Values[0], (int) Values[1]) < Values[2]; //Condition description says less or equal. code or description wrong? probably description

                    case ConditionType.PartyMember_InRangeIsBuff:
                        int Count_PartyMembersInRange = 0;
                        int actives = A_Tools.Buffs.Buffs.B_Global.get_PartyMembers_WithBuff((int)Values[0],
                                    (int)Values[1],Values[2], out Count_PartyMembersInRange);

                        if (Values[3] == -1)
                        {
                            return actives >= Count_PartyMembersInRange;
                        }
                        else if (Values[3] != -1)
                        {
                            switch ((int) Values[4])
                            {
                                case 0:
                                    return actives <= Values[3];

                                case 1:
                                    return actives >= Values[3];
                            }
                        }
                        return false;

                    case ConditionType.PartyMember_InRangeIsNotBuff:
                        int _Count_PartyMembersInRange = 0;
                        int inactives = A_Tools.Buffs.Buffs.B_Global.get_PartyMembers_WithoutBuff((int)Values[0],
                                    (int)Values[1], Values[2], out _Count_PartyMembersInRange);

                        if (Values[3] == -1)
                        {
                            return inactives >= _Count_PartyMembersInRange;
                        }
                        else if (Values[3] != -1)
                        {
                            switch ((int)Values[4])
                            {
                                case 0:
                                    return inactives <= Values[3];

                                case 1:
                                    return inactives >= Values[3];
                            }
                        }
                        return false;

                    case ConditionType.Party_AllInRange:
                        return A_Tools.T_LocalPlayer.get_PartyMemberInRange(Values[0]) >= Me.Party.PlayersInGame - 1;

                    case ConditionType.Party_NotAllInRange:
                        return A_Tools.T_LocalPlayer.get_PartyMemberInRange(Values[0]) < Me.Party.PlayersInGame - 1;

                    case ConditionType.Party_AllAlive:

                        bool allPartyMemberAlive = A_Tools.T_LocalPlayer.get_PartyMemberAlive() >= Me.Party.PlayersInGame - 1;

                        switch ((int)Values[0])
                        {
                            case 1:
                                return allPartyMemberAlive;

                            case 0:
                                return !allPartyMemberAlive;

                            default:
                                return false;
                        }


                    case ConditionType.World_IsRift:
                        switch ((int) Values[0])
                        {
                            case 1:
                                return A_Tools.T_LevelArea.IsRift();

                            case 0:
                                return !A_Tools.T_LevelArea.IsRift();

                            default:
                                return false;
                        }


                    case ConditionType.World_IsGRift:
                        switch ((int) Values[0])
                        {
                            case 1:
                                return A_Tools.T_LevelArea.IsGRift();

                            case 0:
                                return !A_Tools.T_LevelArea.IsGRift();

                            default:
                                return false;
                        }


                    case ConditionType.PartyMember_InRangeMinHitpoints:
                        return A_Tools.T_LocalPlayer.IsPartyMemberInRange_MinHitpoints(Values[0], (int) Values[1]);

                    case ConditionType.Player_Skill_MinCharges:
                        return A_Tools.Skills.Skills.S_Global.get_Charges(SkillPower.PowerSNO) >= Values[0];

                    case ConditionType.Player_Skill_MinResource:

                        //calculate real SkillResourceCost with ResourceCostReduction
                        double SkillResourceCost = SkillPower.ResourceCost;
                        double RealSkillResourceCost = SkillResourceCost - (SkillResourceCost * A_Collection.Me.HeroDetails.ResourceCostReduction);




                        // if cindercoat equipped. then resource cost -30% for fire skills
                        bool isCindercoatActive = false;

                        List<SkillPower> AllSkillPowers;
                        lock (A_Collection.Presets.SkillPowers.AllSkillPowers)
                            AllSkillPowers = A_Collection.Presets.SkillPowers.AllSkillPowers.ToList();

                        if (
                            AllSkillPowers.FirstOrDefault(
                                x =>
                                    x.PowerSNO == SkillPower.PowerSNO &&
                                    x.Runes.FirstOrDefault(y => y.RuneIndex == EquippedRune)._DamageType == DamageType.Fire) !=
                            null && A_Tools.T_LocalPlayer.isBuff((int)318790))
                            isCindercoatActive = true;

                        if (isCindercoatActive)
                        {
                            RealSkillResourceCost = RealSkillResourceCost - (RealSkillResourceCost * 0.3); //-30%
                        }



                        switch ((int) Values[0])
                        {
                            case 1:
                                switch (SkillPower.IsPrimaryResource)
                                {
                                    case true:
                                        //if (isFireReduce)
                                        //    return A_Collection.Me.HeroDetails.ResourcePrimary*1.3 >=
                                        //           SkillPower.ResourceCost;
                                        //return A_Collection.Me.HeroDetails.ResourcePrimary >= SkillPower.ResourceCost;
                                        return A_Collection.Me.HeroDetails.ResourcePrimary >= RealSkillResourceCost;


                                    case false:
                                    //if (isFireReduce)
                                    //    return A_Collection.Me.HeroDetails.ResourceSecondary*1.3 >=
                                    //           SkillPower.ResourceCost*-1;
                                    //return A_Collection.Me.HeroDetails.ResourceSecondary >=
                                    //       SkillPower.ResourceCost*-1;
                                    return A_Collection.Me.HeroDetails.ResourceSecondary >=
                                           RealSkillResourceCost * -1;

                                    default:
                                        return false;
                                }

                            case 0:
                                switch (SkillPower.IsPrimaryResource)
                                {
                                    case true:
                                        //if (isFireReduce)
                                        //    return A_Collection.Me.HeroDetails.ResourcePrimary*1.3 <
                                        //           SkillPower.ResourceCost;
                                        //return A_Collection.Me.HeroDetails.ResourcePrimary < SkillPower.ResourceCost;
                                        return A_Collection.Me.HeroDetails.ResourcePrimary < RealSkillResourceCost;

                                    case false:
                                        //if (isFireReduce)
                                        //    return A_Collection.Me.HeroDetails.ResourceSecondary*1.3 <
                                        //           SkillPower.ResourceCost*-1;
                                        //return A_Collection.Me.HeroDetails.ResourceSecondary <
                                        //       SkillPower.ResourceCost*-1;
                                        return A_Collection.Me.HeroDetails.ResourceSecondary <
                                               RealSkillResourceCost * -1;

                                    default:
                                        return false;
                                }

                            default:
                                return false;
                        }

                    case ConditionType.SelectedMonster_MinDistance:
                        if (Me.HeroDetails.SelectedMonsterACD != null)
                        {
                            return A_Collection.Me.HeroDetails.Distance_SelectedMonsterACD >= Values[0];
                        }
                        return false;

                    case ConditionType.SelectedMonster_MaxDistance:
                        if (Me.HeroDetails.SelectedMonsterACD != null)
                        {
                            return A_Collection.Me.HeroDetails.Distance_SelectedMonsterACD <= Values[0];
                        }
                        return false;

                    case ConditionType.Player_MaxPrimaryResource:
                        //if (isFireReduce)
                        //    return Me.HeroDetails.ResourcePrimary*1.3 <= Values[0];
                        return Me.HeroDetails.ResourcePrimary <= Values[0];

                    case ConditionType.Player_MaxSecondaryResource:
                        //if (isFireReduce)
                        //    return Me.HeroDetails.ResourceSecondary*1.3 <= Values[0];
                        return Me.HeroDetails.ResourceSecondary <= Values[0];

                    case ConditionType.Player_MaxPrimaryResourcePercentage:
                        //if (isFireReduce)
                        //    return Me.HeroDetails.ResourcePrimary_Percentage*1.3 <= Values[0];
                        return Me.HeroDetails.ResourcePrimary_Percentage <= Values[0];

                    case ConditionType.Player_MaxSecondaryResourcePercentage:
                        //if (isFireReduce)
                        //    return Me.HeroDetails.ResourceSecondary_Percentage*1.3 <= Values[0];
                        return Me.HeroDetails.ResourceSecondary_Percentage <= Values[0];

                    case ConditionType.Player_IsMoving:
                        switch ((int) Values[0])
                        {
                            case 1:
                                return A_Collection.Me.HeroStates.isMoving;

                            case 0:
                                return !A_Collection.Me.HeroStates.isMoving;

                            default:
                                return false;
                        }

                    case ConditionType.Player_Pet_MinFetishesCount:
                        return A_Tools.Buffs.Buffs.B_WitchDoctor.get_FetishCount() >= Values[0];

                    case ConditionType.Player_Pet_MinZombieDogsCount:
                        return A_Tools.Buffs.Buffs.B_WitchDoctor.get_ZombieDogCount() >= Values[0];

                    case ConditionType.Player_Pet_MinGargantuanCount:
                        return A_Tools.Buffs.Buffs.B_WitchDoctor.get_GargantuanCount() >= Values[0];

                    case ConditionType.Player_Pet_MaxFetishesCount:
                        return A_Tools.Buffs.Buffs.B_WitchDoctor.get_FetishCount() <= Values[0];

                    case ConditionType.Player_Pet_MaxZombieDogsCount:
                        return A_Tools.Buffs.Buffs.B_WitchDoctor.get_ZombieDogCount() <= Values[0];

                    case ConditionType.Player_Pet_MaxGargantuanCount:
                        return A_Tools.Buffs.Buffs.B_WitchDoctor.get_GargantuanCount() <= Values[0];

                    case ConditionType.Player_Pet_MinSkeletalMageCount:
                        return A_Tools.Buffs.Buffs.B_Necromancer.get_SkeletalMageCount() >= Values[0];

                    case ConditionType.Player_Pet_MaxSkeletalMageCount:
                        return A_Tools.Buffs.Buffs.B_Necromancer.get_SkeletalMageCount() <= Values[0];

                    case ConditionType.SelectedMonster_IsElite:
                        if (A_Collection.Me.HeroDetails.SelectedMonsterACD != null)
                        {
                            switch ((int) Values[0])
                            {
                                case 0:
                                    return !A_Tools.T_ACD.isElite(A_Collection.Me.HeroDetails.SelectedMonsterACD);

                                case 1:
                                    return A_Tools.T_ACD.isElite(A_Collection.Me.HeroDetails.SelectedMonsterACD);
                            }
                        }
                        return false;

                    case ConditionType.SelectedMonster_IsBoss:
                        if (A_Collection.Me.HeroDetails.SelectedMonsterACD != null)
                        {
                            switch ((int) Values[0])
                            {
                                case 0:
                                    return !A_Tools.T_ACD.isBoss(A_Collection.Me.HeroDetails.SelectedMonsterACD);

                                case 1:
                                    return A_Tools.T_ACD.isBoss(A_Collection.Me.HeroDetails.SelectedMonsterACD);
                            }
                        }
                        return false;

                    case ConditionType.Player_Power_IsNotOnCooldown:
                        return !A_Tools.Skills.Skills.S_Global.isOnCooldown((int) Values[0]);

                    case ConditionType.Player_Power_IsOnCooldown:
                        return A_Tools.Skills.Skills.S_Global.isOnCooldown((int) Values[0]);

                    case ConditionType.Player_HasSkillEquipped:
                        return A_Collection.Me.HeroDetails.ActiveSkills.ContainsKey((int) Values[0]);

                    case ConditionType.Player_HasSkillNotEquipped:
                        return !A_Collection.Me.HeroDetails.ActiveSkills.ContainsKey((int) Values[0]);

                    case ConditionType.MonstersInRange_HaveArcaneEnchanted:
                        if (Values[0] == 1)
                            return MonstersInRange.Any(T_ACD.hasAffix_ArcaneEnchanted);
                        return !MonstersInRange.Any(T_ACD.hasAffix_ArcaneEnchanted);

                    case ConditionType.MonstersInRange_HaveAvenger:
                        if (Values[0] == 1)
                            return MonstersInRange.Any(T_ACD.hasAffix_Avenger);
                        return !MonstersInRange.Any(T_ACD.hasAffix_Avenger);

                    case ConditionType.MonstersInRange_HaveDesecrator:
                        if (Values[0] == 1)
                            return MonstersInRange.Any(T_ACD.hasAffix_Desecrator);
                        return !MonstersInRange.Any(T_ACD.hasAffix_Desecrator);

                    case ConditionType.MonstersInRange_HaveElectrified:
                        if (Values[0] == 1)
                            return MonstersInRange.Any(T_ACD.hasAffix_Electrified);
                        return !MonstersInRange.Any(T_ACD.hasAffix_Electrified);

                    case ConditionType.MonstersInRange_HaveExtraHealth:
                        if (Values[0] == 1)
                            return MonstersInRange.Any(T_ACD.hasAffix_ExtraHealth);
                        return !MonstersInRange.Any(T_ACD.hasAffix_ExtraHealth);

                    case ConditionType.MonstersInRange_HaveFast:
                        if (Values[0] == 1)
                            return MonstersInRange.Any(T_ACD.hasAffix_Fast);
                        return !MonstersInRange.Any(T_ACD.hasAffix_Fast);

                    case ConditionType.MonstersInRange_HaveFrozen:
                        if (Values[0] == 1)
                            return MonstersInRange.Any(T_ACD.hasAffix_Frozen);
                        return !MonstersInRange.Any(T_ACD.hasAffix_Frozen);

                    case ConditionType.MonstersInRange_HaveHealthlink:
                        if (Values[0] == 1)
                            return MonstersInRange.Any(T_ACD.hasAffix_Healthlink);
                        return !MonstersInRange.Any(T_ACD.hasAffix_Healthlink);

                    case ConditionType.MonstersInRange_HaveIllusionist:
                        if (Values[0] == 1)
                            return MonstersInRange.Any(T_ACD.hasAffix_Illusionist);
                        return !MonstersInRange.Any(T_ACD.hasAffix_Illusionist);

                    case ConditionType.MonstersInRange_HaveJailer:
                        if (Values[0] == 1)
                            return MonstersInRange.Any(T_ACD.hasAffix_Jailer);
                        return !MonstersInRange.Any(T_ACD.hasAffix_Jailer);

                    case ConditionType.MonstersInRange_HaveKnockback:
                        if (Values[0] == 1)
                            return MonstersInRange.Any(T_ACD.hasAffix_Knockback);
                        return !MonstersInRange.Any(T_ACD.hasAffix_Knockback);

                    case ConditionType.MonstersInRange_HaveFirechains:
                        if (Values[0] == 1)
                            return MonstersInRange.Any(T_ACD.hasAffix_Firechains);
                        return !MonstersInRange.Any(T_ACD.hasAffix_Firechains);

                    case ConditionType.MonstersInRange_HaveMolten:
                        if (Values[0] == 1)
                            return MonstersInRange.Any(T_ACD.hasAffix_Molten);
                        return !MonstersInRange.Any(T_ACD.hasAffix_Molten);

                    case ConditionType.MonstersInRange_HaveMortar:
                        if (Values[0] == 1)
                            return MonstersInRange.Any(T_ACD.hasAffix_Mortar);
                        return !MonstersInRange.Any(T_ACD.hasAffix_Mortar);

                    case ConditionType.MonstersInRange_HaveNightmarish:
                        if (Values[0] == 1)
                            return MonstersInRange.Any(T_ACD.hasAffix_Nightmarish);
                        return !MonstersInRange.Any(T_ACD.hasAffix_Nightmarish);

                    case ConditionType.MonstersInRange_HavePlagued:
                        if (Values[0] == 1)
                            return MonstersInRange.Any(T_ACD.hasAffix_Plagued);
                        return !MonstersInRange.Any(T_ACD.hasAffix_Plagued);

                    case ConditionType.MonstersInRange_HaveReflectsDamage:
                        if (Values[0] == 1)
                            return MonstersInRange.Any(T_ACD.hasAffix_ReflectsDamage);
                        return !MonstersInRange.Any(T_ACD.hasAffix_ReflectsDamage
                            );
                    case ConditionType.MonstersInRange_HaveShielding:
                        if (Values[0] == 1)
                            return MonstersInRange.Any(T_ACD.hasAffix_Shielding);
                        return !MonstersInRange.Any(T_ACD.hasAffix_Shielding);

                    case ConditionType.MonstersInRange_HaveTeleporter:
                        if (Values[0] == 1)
                            return MonstersInRange.Any(T_ACD.hasAffix_Teleporter);
                        return !MonstersInRange.Any(T_ACD.hasAffix_Teleporter);

                    case ConditionType.MonstersInRange_HaveThunderstorm:
                        if (Values[0] == 1)
                            return MonstersInRange.Any(T_ACD.hasAffix_Thunderstorm);
                        return !MonstersInRange.Any(T_ACD.hasAffix_Thunderstorm);

                    case ConditionType.MonstersInRange_HaveVortex:
                        if (Values[0] == 1)
                            return MonstersInRange.Any(T_ACD.hasAffix_Vortex);
                        return !MonstersInRange.Any(T_ACD.hasAffix_Vortex);

                    case ConditionType.MonstersInRange_HaveWaller:
                        if (Values[0] == 1)
                            return MonstersInRange.Any(T_ACD.hasAffix_Waller);
                        return !MonstersInRange.Any(T_ACD.hasAffix_Waller);

                    case ConditionType.MonstersInRange_HaveJuggernaut:
                        if (Values[0] == 1)
                            return MonstersInRange.Any(T_ACD.hasAffix_Juggernaut);
                        return !MonstersInRange.Any(T_ACD.hasAffix_Juggernaut);

                    case ConditionType.SelectedMonster_IsBuffCount:
                        if (A_Collection.Me.HeroDetails.SelectedMonsterACD != null)
                            return
                                A_Tools.T_ACD.getBuffCount((int) Values[0], (int) Values[1],
                                    A_Collection.Me.HeroDetails.SelectedMonsterACD) >= Values[2];
                        return false;

                    case ConditionType.SelectedMonster_IsNotBuffCount:
                        if (A_Collection.Me.HeroDetails.SelectedMonsterACD != null)
                            return
                                A_Tools.T_ACD.getBuffCount((int) Values[0], (int) Values[1],
                                    A_Collection.Me.HeroDetails.SelectedMonsterACD) < Values[2];
                        return false;

                    case ConditionType.MonstersInRange_IsBuffCount:
                        return
                            MonstersInRange.Any(
                                x => T_ACD.getBuffCount((int) Values[0], (int) Values[1], x) >= Values[2]);

                    case ConditionType.MonstersInRange_IsNotBuffCount:
                        return
                            MonstersInRange.Any(x => T_ACD.getBuffCount((int) Values[0], (int) Values[1], x) < Values[2]);

                    case ConditionType.Player_IsDestructableSelected:
                        if (Values[0] == 1)
                            return Me.HeroDetails.SelectedDestructibleACD != null;
                        return Me.HeroDetails.SelectedDestructibleACD == null;

                    case ConditionType.Key_ForceStandStill:
                        if (Values[0] == 1)
                            return Hotkeys.IngameKeys.IsForceStandStill;
                        return !Hotkeys.IngameKeys.IsForceStandStill;

                    case ConditionType.Key_ForceMove:
                        if (Values[0] == 1)
                            return Hotkeys.IngameKeys.isForceMove;
                        return !Hotkeys.IngameKeys.isForceMove;

                    case ConditionType.Add_Property_TimedUse:
                        IsTimedCast = true;
                        if (A_Handler.SkillHandler.SkillHandler._CastTimes.ContainsKey(SkillPower.PowerSNO))
                        {
                            if (A_Handler.SkillHandler.SkillHandler._CastTimes[SkillPower.PowerSNO].AddSeconds(
                                (double) Values[0]/60) >
                                DateTime.Now)
                                return false;
                            else
                                return true;

                        }
                        else
                        {
                            return true;
                        }

                    case ConditionType.SelectedMonster_MonstersInRange_IsBuffActive:
                        switch ((int) Values[2])
                        {
                            case 0:
                                return
                                    SelectedMonster_MonstersInRange.Count(
                                        monster => T_ACD.isBuff((int) Values[0], monster)) <= Values[1];

                            case 1:
                                return
                                    SelectedMonster_MonstersInRange.Count(
                                        monster => T_ACD.isBuff((int) Values[0], monster)) >= Values[1];

                            default:
                                return false;
                        }


                    case ConditionType.SelectedMonster_MonstersInRange_IsBuffNotActive:
                        switch ((int) Values[2])
                        {
                            case 0:
                                return
                                    SelectedMonster_MonstersInRange.Count(
                                        monster => !T_ACD.isBuff((int) Values[0], monster)) <= Values[1];

                            case 1:
                                return
                                    SelectedMonster_MonstersInRange.Count(
                                        monster => !T_ACD.isBuff((int) Values[0], monster)) >= Values[1];

                            default:
                                return false;
                        }


                    case ConditionType.Player_MinAPS:
                        return A_Collection.Me.HeroDetails.AttacksPerSecondTotal >= Values[0];

                    case ConditionType.Add_Property_Channeling:
                        ShouldChannel = true;
                        ChannelTicks = (int) Values[1];
                        return true;

                    case ConditionType.Add_Property_APSSnapShot:
                        if (ConditionGroup.FirstOrDefault(x => x.Type == ConditionType.Player_MinAPS) != null)
                        {
                            double APS_ToSnap =
                                ConditionGroup.FirstOrDefault(x => x.Type == ConditionType.Player_MinAPS).Values[0];

                            if (A_Collection.Me.HeroDetails.SnapShotted_APS < APS_ToSnap)
                                return true;
                        }
                        return false;

                    case ConditionType.MonstersInRange_MinHitpointsPercentage:
                        switch ((int) Values[2])
                        {
                            case 0:
                                return MonstersInRange.Count(x => T_ACD.get_HitpointsPercentage(x) >= Values[0]) <=
                                       Values[1];

                            case 1:
                                return MonstersInRange.Count(x => T_ACD.get_HitpointsPercentage(x) >= Values[0]) >=
                                       Values[1];
                        }
                        return false;

                    case ConditionType.MonstersInRange_MaxHitpointsPercentage:
                        switch ((int) Values[2])
                        {
                            case 0:
                                return MonstersInRange.Count(x => T_ACD.get_HitpointsPercentage(x) <= Values[0]) <=
                                       Values[1];

                            case 1:
                                return MonstersInRange.Count(x => T_ACD.get_HitpointsPercentage(x) <= Values[0]) >=
                                       Values[1];
                        }
                        return false;

                    case ConditionType.SelectedMonster_MonstersInRange_MinHitpointsPercentage:
                        switch ((int) Values[2])
                        {
                            case 0:
                                return
                                    SelectedMonster_MonstersInRange.Count(
                                        x => T_ACD.get_HitpointsPercentage(x) >= Values[0]) <= Values[1];

                            case 1:
                                return
                                    SelectedMonster_MonstersInRange.Count(
                                        x => T_ACD.get_HitpointsPercentage(x) >= Values[0]) >= Values[1];
                        }
                        return false;

                    case ConditionType.SelectedMonster_MonstersInRange_MaxHitpointsPercentage:
                        switch ((int) Values[2])
                        {
                            case 0:
                                return
                                    SelectedMonster_MonstersInRange.Count(
                                        x => T_ACD.get_HitpointsPercentage(x) <= Values[0]) <= Values[1];

                            case 1:
                                return
                                    SelectedMonster_MonstersInRange.Count(
                                        x => T_ACD.get_HitpointsPercentage(x) <= Values[0]) >= Values[1];
                        }
                        return false;

                    case ConditionType.SelectedMonster_MinHitpointsPercentage:
                        if (A_Collection.Me.HeroDetails.SelectedMonsterACD != null)
                            return
                                A_Tools.T_ACD.get_HitpointsPercentage(A_Collection.Me.HeroDetails.SelectedMonsterACD) >
                                Values[0];
                        return false;

                    case ConditionType.SelectedMonster_MaxHitpointsPercentage:
                        if (A_Collection.Me.HeroDetails.SelectedMonsterACD != null)
                            return
                                A_Tools.T_ACD.get_HitpointsPercentage(A_Collection.Me.HeroDetails.SelectedMonsterACD) <
                                Values[0];
                        return false;

                    case ConditionType.Player_StandStillTime:
                        switch ((int) Values[1])
                        {
                            case 0:
                                return !A_Collection.Me.HeroStates.isMoving &&
                                       new TimeSpan(DateTime.Now.Ticks -
                                                    A_Collection.Me.HeroDetails.StandStill_Start.Ticks).TotalSeconds*60 <=
                                       Values[0];

                            case 1:
                                return !A_Collection.Me.HeroStates.isMoving &&
                                       new TimeSpan(DateTime.Now.Ticks -
                                                    A_Collection.Me.HeroDetails.StandStill_Start.Ticks).TotalSeconds*60 >=
                                       Values[0];
                        }
                        return false;

                        case ConditionType.MonstersInRange_RiftProgress:
                        switch ((int) Values[1])
                        {
                            case 0:
                                return A_Tools.T_ACD.convert_ProgressPointsToPercentage(A_Tools.T_ACD.get_RiftProgress(MonstersInRange)) <= Values[0];

                            case 1:
                                return A_Tools.T_ACD.convert_ProgressPointsToPercentage(A_Tools.T_ACD.get_RiftProgress(MonstersInRange)) >= Values[0];

                        }
                        return false;

                    case ConditionType.SelectedMonster_MonstersInRange_RiftProgress:
                        switch ((int)Values[1])
                        {
                            case 0:
                                return A_Tools.T_ACD.convert_ProgressPointsToPercentage(A_Tools.T_ACD.get_RiftProgress(SelectedMonster_MonstersInRange)) <= Values[0];

                            case 1:
                                return A_Tools.T_ACD.convert_ProgressPointsToPercentage(A_Tools.T_ACD.get_RiftProgress(SelectedMonster_MonstersInRange)) >= Values[0];

                        }
                        return false;

                    case ConditionType.SelectedMonster_RiftProgress:
                        if (A_Collection.Me.HeroDetails.SelectedMonsterACD != null)
                        {
                            switch ((int) Values[1])
                            {
                                case 0:
                                    return
                                        A_Tools.T_ACD.convert_ProgressPointsToPercentage(A_Tools.T_ACD.get_RiftProgress(A_Collection.Me.HeroDetails.SelectedMonsterACD)) <=
                                        Values[0];

                                case 1:
                                    return
                                        A_Tools.T_ACD.convert_ProgressPointsToPercentage(A_Tools.T_ACD.get_RiftProgress(A_Collection.Me.HeroDetails.SelectedMonsterACD)) >=
                                        Values[0];

                            }
                        }
                        return false;

                }

                return false;
            }
            catch { return false; }
        }

        private static int getRealPowerSNO(int PowerSNO)
        {
            try
            {
                List<ActivePower> ActivePowers;
                lock (A_Collection.Me.HeroDetails.ActivePowers)
                    ActivePowers = A_Collection.Me.HeroDetails.ActivePowers.ToList();

                switch (PowerSNO)
                {
                    case 167355:
                        if (ActivePowers.FirstOrDefault(x => x.Value == 392883) != null)
                            return 392883;
                        if (ActivePowers.FirstOrDefault(x => x.Value == 392884) != null)
                            return 392884;
                        if (ActivePowers.FirstOrDefault(x => x.Value == 392885) != null)
                            return 392885;
                        return 167355;

                    case 135166:
                        if (ActivePowers.FirstOrDefault(x => x.Value == 392886) != null)
                            return 392886;
                        if (ActivePowers.FirstOrDefault(x => x.Value == 392887) != null)
                            return 392887;
                        if (ActivePowers.FirstOrDefault(x => x.Value == 392888) != null)
                            return 392888;
                        return 135166;

                    case 135238:
                        if (ActivePowers.FirstOrDefault(x => x.Value == 392889) != null)
                            return 392889;
                        if (ActivePowers.FirstOrDefault(x => x.Value == 392890) != null)
                            return 392890;
                        if (ActivePowers.FirstOrDefault(x => x.Value == 392891) != null)
                            return 392891;
                        return 135238;

                    default:
                        return PowerSNO;
                }
            }
            catch (Exception)
            {
                return PowerSNO;
            }
        }
    }
}
