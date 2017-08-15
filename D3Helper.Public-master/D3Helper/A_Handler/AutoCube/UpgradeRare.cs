using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Enigma.D3;
using Enigma.D3.UI;
using D3Helper.A_Enums;

namespace D3Helper.A_Handler.AutoCube
{
    class UpgradeRare
    {
        public static bool IsUpgrading_Rare = false;


        public static void DoUpgrade()
        {
            try
            {
                IsUpgrading_Rare = true;

                ActorCommonData CubeStand;

                bool CubeNearby = Tools.IsCubeNearby(out CubeStand);

                if (CubeNearby)
                {
                    if (Tools.ClickOnCube(CubeStand))
                    {
                        Stopwatch s1 = new Stopwatch(); /////////
                        s1.Start(); ///////////

                        var UpgradableItems = Tools.Get_Items("rare");
                        var Count_AvailableEnchants = 50;

                        var Count_Enchants = 0;

                        foreach (var item in UpgradableItems)
                        {
                            if (Count_Enchants == Count_AvailableEnchants)
                                break;

                            if (Tools.ClickOnCube(CubeStand))
                            {
                                //receipe button
                                A_Tools.T_D3UI.UIElement.leftClick(UIElements.Kanai_Cube_Recipe_Button);

                                //press next button 2x for upgrade rare menu
                                A_Tools.T_D3UI.UIElement.leftClick(UIElements.Kanai_Cube_Page_Next);
                                A_Tools.T_D3UI.UIElement.leftClick(UIElements.Kanai_Cube_Page_Next);
                                
                                
                                //put item in cube
                                UIRect UIRect_item =
                                    A_Collection.D3UI.InventoryItemUIRectMesh.FirstOrDefault(
                                        x => x.Key.ItemSlotX == item.x118_ItemSlotX && x.Key.ItemSlotY == item.x11C_ItemSlotY).Value;

                                A_Tools.T_D3UI.UIElement.rightClick(UIRect_item);


                                //fill
                                A_Tools.T_D3UI.UIElement.leftClick(UIElements.Kanai_Cube_Fill_Button);

                                //transmute
                                A_Tools.T_D3UI.UIElement.leftClick(UIElements.Kanai_Cube_Transmute_Button);


                                //close all windows
                                int close_timeout = 2;
                                while (Tools.IsVendorPage_Visible())
                                {
                                    //A_Tools.InputSimulator.IS_Keyboard.Close_AllWindows(); //doesnt work if not assigned :D

                                    //Press "X" - Button in Kanais Cube
                                    A_Tools.T_D3UI.UIElement.leftClick(UIElements.Kanai_Cube_Exit_Button);

                                    close_timeout--;
                                    if(close_timeout == 0)
                                    {
                                        break;
                                    }
                                }


                                Count_Enchants++;
                            }
                        }

                        s1.Stop(); /////////
                        TimeSpan t1 = s1.Elapsed; //////
                        Console.WriteLine(t1.TotalSeconds); ////////
                    }
                }

                IsUpgrading_Rare = false;
            }
            catch (Exception)
            {
                IsUpgrading_Rare = false;
            }
        }
    }
}
