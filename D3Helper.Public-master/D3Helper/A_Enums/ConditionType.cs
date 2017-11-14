using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ProtoBuf;

namespace D3Helper.A_Enums
{
    [ProtoContract]
    public enum ConditionType
    {
        Player_Skill_IsNotOnCooldown = 0,
        Player_IsBuffActive = 1,
        Player_IsBuffNotActive = 2,
        Player_MinPrimaryResource = 3,
        Player_MinSecondaryResource = 4,
        World_MonstersInRange = 5,
        World_EliteInRange = 6,
        World_BossInRange = 7,
        Player_BuffTicksLeft = 8,
        Player_IsMonsterSelected = 9,
        SelectedMonster_IsBuffActive = 10,
        SelectedMonster_IsBuffNotActive = 11,
        MonstersInRange_IsBuffActive = 12,
        MonstersInRange_IsBuffNotActive = 13,
        Player_MinPrimaryResourcePercentage = 14,
        Player_MinSecondaryResourcePercentage = 15,
        Player_MaxHitpointsPercentage = 16,
        SelectedMonster_MonstersInRange = 17,
        Player_IsBuffCount = 18,
        Player_IsNotBuffCount = 19,
        PartyMember_InRangeIsBuff = 20,
        PartyMember_InRangeIsNotBuff = 21,
        Party_AllInRange = 22,
        Party_NotAllInRange = 23,
        World_IsRift = 24,
        World_IsGRift = 25,
        PartyMember_InRangeMinHitpoints = 26,
        Player_Skill_MinCharges = 27,
        Player_Skill_MinResource = 28,
        SelectedMonster_MinDistance = 29,
        SelectedMonster_MaxDistance = 30,
        Player_MaxPrimaryResource = 31,
        Player_MaxSecondaryResource = 32,
        Player_MaxPrimaryResourcePercentage = 33,
        Player_MaxSecondaryResourcePercentage = 34,
        Player_IsMoving = 35,
        Player_Pet_MinFetishesCount = 36,
        Player_Pet_MinZombieDogsCount = 37,
        Player_Pet_MinGargantuanCount = 38,
        Player_Pet_MaxFetishesCount = 39,
        Player_Pet_MaxZombieDogsCount = 40,
        Player_Pet_MaxGargantuanCount = 41,
        SelectedMonster_IsElite = 42,
        SelectedMonster_IsBoss = 43,
        Player_Power_IsOnCooldown = 44,
        Player_Power_IsNotOnCooldown = 45,
        Player_HasSkillEquipped = 46,
        MonstersInRange_HaveArcaneEnchanted = 47,
        MonstersInRange_HaveAvenger = 48,
        MonstersInRange_HaveDesecrator = 49,
        MonstersInRange_HaveElectrified = 50,
        MonstersInRange_HaveExtraHealth = 51,
        MonstersInRange_HaveFast = 52,
        MonstersInRange_HaveFrozen = 53,
        MonstersInRange_HaveHealthlink = 54,
        MonstersInRange_HaveIllusionist = 55,
        MonstersInRange_HaveJailer = 56,
        MonstersInRange_HaveKnockback = 57,
        MonstersInRange_HaveFirechains = 58,
        MonstersInRange_HaveMolten = 59,
        MonstersInRange_HaveMortar = 60,
        MonstersInRange_HaveNightmarish = 61,
        MonstersInRange_HavePlagued = 62,
        MonstersInRange_HaveReflectsDamage = 63,
        MonstersInRange_HaveShielding = 64,
        MonstersInRange_HaveTeleporter = 65,
        MonstersInRange_HaveThunderstorm = 66,
        MonstersInRange_HaveVortex = 67,
        MonstersInRange_HaveWaller = 68,
        Player_HasSkillNotEquipped = 69,
        SelectedMonster_IsBuffCount = 70,
        SelectedMonster_IsNotBuffCount = 71,
        MonstersInRange_IsBuffCount = 72,
        MonstersInRange_IsNotBuffCount = 73,
        Player_IsDestructableSelected = 74,
        Key_ForceStandStill = 75,
        Add_Property_TimedUse = 76,
        SelectedMonster_MonstersInRange_IsBuffActive = 77,
        SelectedMonster_MonstersInRange_IsBuffNotActive = 78,
        Player_MinAPS = 79,
        Add_Property_Channeling = 80,
        Add_Property_APSSnapShot = 81,
        MonstersInRange_MinHitpointsPercentage = 82,
        MonstersInRange_MaxHitpointsPercentage = 83,
        SelectedMonster_MonstersInRange_MinHitpointsPercentage = 84,
        SelectedMonster_MonstersInRange_MaxHitpointsPercentage = 85,
        SelectedMonster_MinHitpointsPercentage = 86,
        SelectedMonster_MaxHitpointsPercentage = 87,
        Player_StandStillTime = 88,
        MonstersInRange_RiftProgress = 89,
        SelectedMonster_MonstersInRange_RiftProgress = 90,
        SelectedMonster_RiftProgress = 91,
        Party_AllAlive = 92,
        Key_ForceMove = 93,
        MonstersInRange_HaveJuggernaut = 94,
        Player_Pet_MinSkeletalMageCount = 95,
        Player_Pet_MaxSkeletalMageCount = 96

    }

    static class ConditionTypeHelper
    {
        public static string getTooltip(string ItemText)
        {

            ConditionType type = (ConditionType)Enum.Parse(typeof(ConditionType), ItemText);

            string tooltip;
            if (!D3Helper.A_Collection.Presets.Manual.Tooltips.ConditionTypes.TryGetValue(type, out tooltip))
                tooltip = $"!ERROR! NO TOOLTIP DEFINED FOR CONDITION TYPE '{ItemText}'";

            if (tooltip.Length >= 80)
                tooltip = tooltip.Insert(80, "\n");

            return tooltip;

    
        }
    }
}


