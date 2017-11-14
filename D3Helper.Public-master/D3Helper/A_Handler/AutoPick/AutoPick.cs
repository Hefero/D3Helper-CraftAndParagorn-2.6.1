using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using D3Helper.A_Collector;
using Enigma.D3;
using Enigma.D3.Enums;
using Enigma.D3.Helpers;

namespace D3Helper.A_Handler.AutoPick
{
    class AutoPick
    {
        public static bool IsPicking = false;

        public static void DoPickup()
        {
            try
            {
                IsPicking = true;

                // Save Current Cursor Pos
                Point getCursorPos = new Point((int)Cursor.Position.X, (int)Cursor.Position.Y);

                var AllPickableItems = get_AllPickableItems();
                var ItemsToPick = get_ItemsToPick(AllPickableItems);

                if (ItemsToPick.Count > 0)
                {

                    foreach (var pick in ItemsToPick)
                    {
                        int safetycounter = 0;

                        var local = ActorCommonData.Local;

                        while (IsStillOnGround(pick.x08C_ActorId) && safetycounter < 10)
                        {
                            float RX, RY;

                            A_Tools.T_World.ToScreenCoordinate(pick.x0D0_WorldPosX, pick.x0D4_WorldPosY,
                                pick.x0D8_WorldPosZ, out RX, out RY);

                            A_Tools.InputSimulator.IS_Mouse.MoveCursor((uint) RX, (uint) RY);
                            A_Tools.InputSimulator.IS_Mouse.LeftClick((uint) RX, (uint) RY);

                            Thread.Sleep(50);

                            var _local = ActorCommonData.Local;

                            DateTime _start = DateTime.Now;

                            while (_local.x0D0_WorldPosX != local.x0D0_WorldPosX ||
                                   _local.x0D4_WorldPosY != local.x0D4_WorldPosY)
                            {
                                if (_start.AddSeconds(5) < DateTime.Now)
                                    break;

                                Thread.Sleep(10);
                            }

                            safetycounter++;

                        }
                    }

                    // Restore Cursor Pos to previous Pos
                    Cursor.Position = getCursorPos;

                }

                IsPicking = false;
            }
            catch (Exception)
            {
                
                IsPicking = false;
            }
        }

        private static List<ActorCommonData> get_AllPickableItems()
        {
            List<ACD> AllActors;
            lock (A_Collection.Environment.Actors.AllActors)
                AllActors = A_Collection.Environment.Actors.AllActors.ToList();

            List<ActorCommonData> Buffer = new List<ActorCommonData>();

            foreach (var actor in AllActors)
            {
                var acd = actor._ACD;

                if (Filter.IsValid(acd) && Filter.IsGroundItem(acd))
                {
                    Buffer.Add(acd);
                }

            }

            return Buffer;
        }

        private static List<ActorCommonData> get_ItemsToPick(List<ActorCommonData> AllPickableItems)
        {
            List<ActorCommonData> Buffer = new List<ActorCommonData>();

            foreach (var item in AllPickableItems)
            {
                if(A_Tools.T_ACD.get_Distance(item.x0D0_WorldPosX, item.x0D4_WorldPosY) > Properties.Settings.Default.AutoPickSettings_PickupRadius)
                    continue;

                if(Properties.Settings.Default.AutoPickSettings_Gem)
                    if(Filter.IsGem(item))
                    {
                        Buffer.Add(item);
                        continue;
                    }

                if (Properties.Settings.Default.AutoPickSettings_Material)
                    if (Filter.IsMaterial(item))
                    {
                        Buffer.Add(item);
                        continue;
                    }

                if (Properties.Settings.Default.AutoPickSettings_GreaterRiftKeystone)
                    if (Filter.IsGreaterRiftKeystone(item))
                    {
                        Buffer.Add(item);
                        continue;
                    }

                if (Properties.Settings.Default.AutoPickSettings_Legendary)
                    if (!Properties.Settings.Default.AutoPickSettings_LegendaryAncient)
                    {
                        if (Filter.IsLegendary(item))
                        {
                            Buffer.Add(item);
                            continue;
                        }
                    }
                    else
                    {
                        if (Filter.IsLegendary(item) && Filter.IsAncient(item))
                        {
                            Buffer.Add(item);
                            continue;
                        }
                    }

                if (Properties.Settings.Default.AutoPickSettings_Whites)
                    if (Filter.IsWhite(item))
                    {
                        Buffer.Add(item);
                        continue;
                    }

                if (Properties.Settings.Default.AutoPickSettings_Magics)
                    if (Filter.IsMagic(item))
                    {
                        Buffer.Add(item);
                        continue;
                    }

                if (Properties.Settings.Default.AutoPickSettings_Rares)
                    if (Filter.IsRare(item))
                    {
                        Buffer.Add(item);
                        continue;
                    }
            }

            return Buffer;
        }

        private static bool IsStillOnGround(int ActorId)
        {
            return ActorCommonDataHelper.EnumerateGroundItems().FirstOrDefault(x => x.x08C_ActorId == ActorId) != null;
        }

        private class Filter
        {
            public static bool IsValid(ActorCommonData acd)
            {
                return acd.x000_Id != -1;
            }

            public static bool IsAncient(ActorCommonData acd)
            {
                return (int) acd.GetAttributeValue(AttributeId.AncientRank) == 1;
            }

            public static bool IsLegendary(ActorCommonData acd)
            {
                return (int) acd.GetAttributeValue(AttributeId.ItemQualityLevel) == (int) ItemQuality.Legendary;
            }
            public static bool IsWhite(ActorCommonData acd)
            {
                return (int)acd.GetAttributeValue(AttributeId.ItemQualityLevel) == (int)ItemQuality.Inferior ||
                    (int)acd.GetAttributeValue(AttributeId.ItemQualityLevel) == (int)ItemQuality.Normal ||
                    (int)acd.GetAttributeValue(AttributeId.ItemQualityLevel) == (int)ItemQuality.Superior;
            }
            public static bool IsMagic(ActorCommonData acd)
            {
                return (int)acd.GetAttributeValue(AttributeId.ItemQualityLevel) == (int)ItemQuality.Magic1 ||
                    (int)acd.GetAttributeValue(AttributeId.ItemQualityLevel) == (int)ItemQuality.Magic2 ||
                    (int)acd.GetAttributeValue(AttributeId.ItemQualityLevel) == (int)ItemQuality.Magic3;
            }
            public static bool IsRare(ActorCommonData acd)
            {
                return (int)acd.GetAttributeValue(AttributeId.ItemQualityLevel) == (int)ItemQuality.Rare4 ||
                    (int)acd.GetAttributeValue(AttributeId.ItemQualityLevel) == (int)ItemQuality.Rare5 ||
                    (int)acd.GetAttributeValue(AttributeId.ItemQualityLevel) == (int)ItemQuality.Rare6;
            }

            public static bool IsGreaterRiftKeystone(ActorCommonData acd)
            {
                return acd.x090_ActorSnoId == 408416;
            }

            public static bool IsGroundItem(ActorCommonData acd)
            {
                return (int) acd.x114_ItemLocation == -1;
            }

            public static bool IsGem(ActorCommonData acd)
            {
                //string ItemName = acd.x004_Name // Not needed but for easier debug
                switch (acd.x090_ActorSnoId)
                {
                    case 437414: // Amethyst_15 Marquise Amethyst
                    case 437415: // Amethyst_16 Imperial Amethyst
                    case 437459: // Ruby_15 Marquise Ruby
                    case 437460: // Ruby_16 Imperial Ruby
                    case 437481: // x1_Diamond_06 Marquise Diamond
                    case 437482: // x1_Diamond_07 Imperial Diamond
                    case 437448: // x1_Emerald_06 Marquise Emerald
                    case 437449: // x1_Emerald_07 Imperial Emerald
                    case 437469: // x1_Topaz_06 Marquise Topaz
                    case 437470: // x1_Topaz_07 Imperial Topaz
                        return true;

                    default:
                        return false;
                }
            }

            public static bool IsMaterial(ActorCommonData acd)
            {
                //string ItemName = acd.x004_Name // Not needed but for easier debug
                switch (acd.x090_ActorSnoId)
                {
                    case 361988:    // Crafting_Legendary_05
                    case 361989:    // Crafting_Looted_Reagent_05
                    case 361986:    // Crafting_Rare_05
                    case 361985:    // Crafting_Magic_05
                    case 361984:    // Crafting_AssortedParts_05
                    case 137958:    // CraftingMaterials_Flippy_Global
                    case 365020:    // CraftingReagent_Legendary_Set_Borns_X1
                    case 364281:    // CraftingReagent_Legendary_Set_Cains_X1
                    case 364290:    // CraftingReagent_Legendary_Set_Demon_X1
                    case 364305:    // CraftingReagent_Legendary_Set_Hallowed_X1
                    case 364975:    // CraftingReagent_Legendary_Set_CaptainCrimsons_X1
                    case 364725:    // DemonOrgan_Diablo_x1
                    case 364723:    // DemonOrgan_Ghom_x1
                    case 364724:    // DemonOrgan_SiegeBreaker_x1
                    case 364722:    // DemonOrgan_SkeletonKing_x1
                    case 366949:    // InfernalMachine_Diablo_x1
                    case 366947:    // InfernalMachine_Ghom_x1
                    case 366948:    // InfernalMachine_SiegeBreaker_x1
                    case 366946:    // InfernalMachine_SkeletonKing_x1
                    case 359504:    // HoradricRelic
                    case 449044:    // Deathbreath (itemFlippy_deathsBreath_Flippy_Global)
                        return true;
                    default:
                        return false;
                }
            }
        }
    }
}
