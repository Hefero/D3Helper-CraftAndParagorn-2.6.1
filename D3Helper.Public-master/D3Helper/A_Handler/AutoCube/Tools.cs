using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using D3Helper.A_Collector;
using Enigma.D3;
using Enigma.D3.DataTypes;
using Enigma.D3.Enums;
using Enigma.D3.Helpers;
using Enigma.D3.UI.Controls;
using D3Helper.A_Enums;

namespace D3Helper.A_Handler.AutoCube
{
    class Tools
    {
        private const int KanaiCube_Stand = 439975;

        public static bool IsCubeNearby(out ActorCommonData CubeStand)
        {
            CubeStand = new ActorCommonData();

            try
            {
                List<ACD> AllActors;
                lock (A_Collection.Environment.Actors.AllActors) AllActors = A_Collection.Environment.Actors.AllActors;

                var acd = AllActors.FirstOrDefault(x => x._ACD.x090_ActorSnoId == KanaiCube_Stand)._ACD;

                if (acd != null)
                {
                    CubeStand = acd;

                    return true;
                }

                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool IsVendorPage_Visible()
        {
            try
            {
                string vendor_mainpage = "Root.NormalLayer.vendor_dialog_mainPage.text_category";

                return A_Tools.T_D3UI.UIElement.isVisible(vendor_mainpage);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool IsKanaisCube_MainPage_Visible()
        {
            try
            {
                string Text = "KANAI'S CUBE";
                string vendor_mainpage = "Root.NormalLayer.vendor_dialog_mainPage.text_category";
                string last_vendor_text = UXHelper.GetControl<UXLabel>(vendor_mainpage).xA20_Text_StructStart_Min84Bytes;

                return last_vendor_text == Text;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static List<ActorCommonData> Get_Items(string inputQuality)
        {
            int QualityRange1 = 0;
            int QualityRange2 = 0;
            bool ancient = false;
            bool primal = false;

            List<ActorCommonData> Items = new List<ActorCommonData>();

            switch (inputQuality)
            {
                case "normal":
                    QualityRange1 = 0;
                    QualityRange2 = 2;
                    break;
                case "magic":
                    QualityRange1 = 3;
                    QualityRange2 = 5;
                    break;
                case "rare":
                    QualityRange1 = 6;
                    QualityRange2 = 8;
                    break;
                case "legendary":
                    QualityRange1 = 9;
                    QualityRange2 = 9;
                    break;
                case "ancient":
                    QualityRange1 = 9;
                    QualityRange2 = 9;
                    ancient = true;
                    break;
                case "primal":
                    QualityRange1 = 9;
                    QualityRange2 = 9;
                    primal = true;
                    break;
            }

            try
            {
                var inventory = ActorCommonDataHelper.EnumerateInventoryItems();

                foreach (var item in inventory)
                {
                    // Ignore these materials (these have "normal" quality)
                    switch (item.x090_ActorSnoId)
                    {
                        case 361988: //Crafting_Legendary_05
                        case 361989: //Crafting_Looted_Reagent_05
                        case 361986: //Crafting_Rare_05
                        case 361985: //Crafting_Magic_05
                        case 361984: //Crafting_AssortedParts_05
                        case 137958: //CraftingMaterials_Flippy_Global
                        case 365020: //CraftingReagent_Legendary_Set_Borns_X1
                        case 364281: //CraftingReagent_Legendary_Set_Cains_X1
                        case 364290: //CraftingReagent_Legendary_Set_Demon_X1
                        case 364305: //CraftingReagent_Legendary_Set_Hallowed_X1
                        case 364975: //CraftingReagent_Legendary_Set_CaptainCrimsons_X1
                        case 364725: //DemonOrgan_Diablo_x1
                        case 364723: //DemonOrgan_Ghom_x1
                        case 364724: //DemonOrgan_SiegeBreaker_x1
                        case 364722: //DemonOrgan_SkeletonKing_x1
                        case 366949: //InfernalMachine_Diablo_x1
                        case 366947: //InfernalMachine_Ghom_x1
                        case 366948: //InfernalMachine_SiegeBreaker_x1
                        case 366946: //InfernalMachine_SkeletonKing_x1
                        case 359504: //HoradricRelic                 
                        case 437414: //Amethyst_15 Marquise Amethyst
                        case 437415: //Amethyst_16 Imperial Amethyst
                        case 437459: //Ruby_15 Marquise Ruby
                        case 437460: //Ruby_16 Imperial Ruby
                        case 437481: //x1_Diamond_06 Marquise Diamond
                        case 437482: //x1_Diamond_07 Imperial Diamond
                        case 437448: //x1_Emerald_06 Marquise Emerald
                        case 437449: //x1_Emerald_07 Imperial Emerald
                        case 437469: //x1_Topaz_06 Marquise Topaz
                        case 437470: //x1_Topaz_07 Imperial Topaz           
                            break;
                        default:
                            var name = item.x004_Name; // not needed but nice for debug    
                            var quality = item.GetAttributeValue(AttributeId.ItemQualityLevel);
                            if (quality >= QualityRange1 && quality <= QualityRange2) // Magic
                                if (ancient)
                                {
                                    var ancientRank = item.GetAttributeValue(AttributeId.AncientRank);
                                    if(ancientRank == 1)
                                    {
                                        Items.Add(item);
                                    }
                                }
                                else if (primal)
                                {
                                    var ancientRank = item.GetAttributeValue(AttributeId.AncientRank);
                                    if (ancientRank == 2)
                                    {
                                        Items.Add(item);
                                    }
                                }
                                else
                                {
                                    Items.Add(item);
                                }
                            break;
                    }
                }
                return Items;
            }
            catch (Exception)
            {
                return Items;
            }
        }

        private static int[] Costs_UpgradeRare = new int[] { 25, 50, 50, 50 }; // Deaths Breath | Reusable Parts | Arcane Dust | Veiled Crystal

        public static double Get_AvailableEnchants_UpgradeRare(out List<ActorCommonData> Materials)
        {
            Materials = new List<ActorCommonData>();
            try
            {
                var inventory = ActorCommonDataHelper.EnumerateInventoryItems().ToList();

                ActorCommonData acd;

                int Count_DB = GetMaterial_DeathBreath(inventory, out acd);
                Materials.Add(acd);

                int Count_RP = GetMaterial_ReusableParts(inventory, out acd);
                Materials.Add(acd);

                int Count_AD = GetMaterial_ArcaneDust(inventory, out acd);
                Materials.Add(acd);

                int Count_VC = GetMaterial_VeiledCrystal(inventory, out acd);
                Materials.Add(acd);

                double Enchants_DB = Count_DB / Costs_UpgradeRare[0];
                double Enchants_RP = Count_RP / Costs_UpgradeRare[1];
                double Enchants_AD = Count_AD / Costs_UpgradeRare[2];
                double Enchants_VC = Count_VC / Costs_UpgradeRare[3];

                double[] x = new[] { Enchants_DB, Enchants_RP, Enchants_AD, Enchants_VC };

                double possibleEnchants = x.OrderBy(y => y).First();

                return possibleEnchants;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static double Get_AvailableMaterial_Convert(string inputQuality, out List<ActorCommonData> Materials)
        {
            Materials = new List<ActorCommonData>();
            try
            {
                int ConvertMaterialCost = 100;
                int ConvertMaterialDBCost = 1;
                int CountMaterial = 0;

                var inventory = ActorCommonDataHelper.EnumerateInventoryItems().ToList();
                ActorCommonData acd;

                switch (inputQuality)
                {
                    case "normal":
                        CountMaterial = GetMaterial_ReusableParts(inventory, out acd);
                        Materials.Add(acd);
                        break;
                    case "magic":
                        CountMaterial = GetMaterial_ArcaneDust(inventory, out acd);
                        Materials.Add(acd);
                        break;
                    case "rare":
                        CountMaterial = GetMaterial_VeiledCrystal(inventory, out acd);
                        Materials.Add(acd);
                        break;
                }

                int Count_DB = GetMaterial_DeathBreath(inventory, out acd);
                Materials.Add(acd);

                double Enchants_DB = Count_DB / ConvertMaterialDBCost;
                double Enchants = CountMaterial / ConvertMaterialCost;
                double[] x = new[] { Enchants_DB, Enchants };
                double possibleEnchants = x.OrderBy(y => y).First();

                return Enchants;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        private static int GetMaterial_DeathBreath(List<ActorCommonData> Inventory, out ActorCommonData acd)
        {
            acd = new ActorCommonData();
            try
            {
                int ActorSno = 361989;

                var material = Inventory.FirstOrDefault(x => x.x090_ActorSnoId == ActorSno);

                if (material == null)
                    return 0;

                acd = material;

                return (int)material.GetAttributeValue(AttributeId.ItemStackQuantityLo);
            }
            catch (Exception)
            {
                return 0;
            }
        }
        private static int GetMaterial_ReusableParts(List<ActorCommonData> Inventory, out ActorCommonData acd)
        {
            acd = new ActorCommonData();

            try
            {
                int ActorSno = 361984;

                var material = Inventory.FirstOrDefault(x => x.x090_ActorSnoId == ActorSno);

                if (material == null)
                    return 0;

                acd = material;

                return (int)material.GetAttributeValue(AttributeId.ItemStackQuantityLo);
            }
            catch (Exception)
            {
                return 0;
            }
        }
        private static int GetMaterial_ArcaneDust(List<ActorCommonData> Inventory, out ActorCommonData acd)
        {
            acd = new ActorCommonData();

            try
            {
                int ActorSno = 361985;

                var material = Inventory.FirstOrDefault(x => x.x090_ActorSnoId == ActorSno);

                if (material == null)
                    return 0;

                acd = material;

                return (int)material.GetAttributeValue(AttributeId.ItemStackQuantityLo);
            }
            catch (Exception)
            {
                return 0;
            }
        }
        private static int GetMaterial_VeiledCrystal(List<ActorCommonData> Inventory, out ActorCommonData acd)
        {
            acd = new ActorCommonData();

            try
            {
                int ActorSno = 361986;

                var material = Inventory.FirstOrDefault(x => x.x090_ActorSnoId == ActorSno);

                if (material == null)
                    return 0;

                acd = material;

                return (int)material.GetAttributeValue(AttributeId.ItemStackQuantityLo);
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static bool ClickOnCube(ActorCommonData inputCubeStand)
        {
            bool FoundCube = false;
            int LoopCounter = 0;

            // Attempt to click on Cube, wait 2 sec (10x200ms)
            while (!FoundCube && LoopCounter <= 10)
            {
                float RX_Cube, RY_Cube;

                LoopCounter += 1;

                // Try to find where the cube is?
                A_Tools.T_World.ToScreenCoordinate(inputCubeStand.x0D0_WorldPosX, inputCubeStand.x0D4_WorldPosY, inputCubeStand.x0D8_WorldPosZ, out RX_Cube, out RY_Cube);

                // If vendor page or kanai page is not already visible, click it
                bool IsVendorPageVisible = Tools.IsVendorPage_Visible();
                bool IsKanaiCubeMainPageVisible = Tools.IsKanaisCube_MainPage_Visible();

                if (!IsVendorPageVisible)
                {
                    // Move mouse cursor to the cube location coord and click it
                    A_Tools.InputSimulator.IS_Mouse.MoveCursor((uint)RX_Cube, (uint)RY_Cube);
                    A_Tools.InputSimulator.IS_Mouse.LeftClick();

                    Thread.Sleep(200);
                }

                if (IsVendorPageVisible && IsKanaiCubeMainPageVisible)
                {
                    FoundCube = true;
                }
            }
            return FoundCube;
        }
    }
}
