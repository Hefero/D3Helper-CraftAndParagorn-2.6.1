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
    class ConvertMaterial
    {
        public static bool IsConvertingMaterial = false;
        public static bool StopConversion = false;

        public static void DoConvert(string FromMaterialQuality, string ToMaterialQuality)
        {
            // check input
            if ((FromMaterialQuality == "normal" || FromMaterialQuality == "magic" || FromMaterialQuality == "rare") &&
                (ToMaterialQuality == "normal" || ToMaterialQuality == "magic" || ToMaterialQuality == "rare"))
            {
                try
                {
                    IsConvertingMaterial = true;
                    ActorCommonData CubeStand;
                    bool CubeNearby = Tools.IsCubeNearby(out CubeStand);
                    // Get list of equipment of the specified quality
                    var ToItemList = Tools.Get_Items(ToMaterialQuality);

                    int Count_AvailableConversions = (int)Tools.Get_AvailableMaterial_Convert(FromMaterialQuality);

                    if (CubeNearby & ToItemList.Count > 0 & Count_AvailableConversions > 0)
                    {
                        if (Tools.ClickOnCube(CubeStand))
                        {
                            Stopwatch s1 = new Stopwatch(); /////////
                            s1.Start(); ///////////
                            int Count_Conversions = 0;

                            // Get list of Materials to convert from
                            //receipe button
                            A_Tools.T_D3UI.UIElement.leftClick(UIElements.Kanai_Cube_Recipe_Button);

                            //press next button 2x for upgrade rare menu - white (6) -blue (7) - yellow (8) [from]
                            var nextClicks = 0;
                            switch (FromMaterialQuality)
                            {
                                case "normal":
                                    nextClicks = 6;
                                    break;
                                case "magic":
                                    nextClicks = 7;
                                    break;
                                case "rare":
                                    nextClicks = 8;
                                    break;
                            }
                            while (nextClicks > 0)
                            {
                                A_Tools.T_D3UI.UIElement.leftClick(UIElements.Kanai_Cube_Page_Next);
                                Thread.Sleep(120);
                                nextClicks--;
                            }

                            int numberOfItemsBeforeTrying = ToItemList.Count;
                            foreach (var item in ToItemList)
                            {
                                if (StopConversion)
                                {
                                    break;
                                }
                                if (Count_Conversions == Count_AvailableConversions)
                                {
                                    break;
                                }
                                Count_Conversions++;
                                if (true)
                                {

                                    // Find the item position and rightclick on the item
                                    UIRect UIRect_item = A_Collection.D3UI.InventoryItemUIRectMesh.FirstOrDefault(x => x.Key.ItemSlotX == item.x118_ItemSlotX && x.Key.ItemSlotY == item.x11C_ItemSlotY).Value;
                                    A_Tools.InputSimulator.IS_Mouse.RightCLick((int)UIRect_item.Left, (int)UIRect_item.Top, (int)UIRect_item.Right, (int)UIRect_item.Bottom);
                                    //fill
                                    A_Tools.T_D3UI.UIElement.leftClick(UIElements.Kanai_Cube_Fill_Button);
                                    //transmute
                                    A_Tools.T_D3UI.UIElement.leftClick(UIElements.Kanai_Cube_Transmute_Button);
                                    Thread.Sleep(Properties.Settings.Default.SleepTransmute);
                                    //click next and back to reset
                                    A_Tools.T_D3UI.UIElement.leftClick(UIElements.Kanai_Cube_Page_Next);
                                    A_Tools.T_D3UI.UIElement.leftClick(UIElements.Kanai_Cube_Page_Previous);
                                }
                            }
                            // Close Cube window after transmute
                            int close_timeout =5;
                            while (Tools.IsKanaisCube_MainPage_Visible())
                            {
                                //A_Tools.InputSimulator.IS_Keyboard.Close_AllWindows(); //doesnt work if not assigned :D
                                //Press "X" - Button in Kanais Cube
                                A_Tools.T_D3UI.UIElement.leftClick(UIElements.Kanai_Cube_Exit_Button);

                                close_timeout--;
                                if (close_timeout == 0)
                                {
                                    break;
                                }
                                Thread.Sleep(200);
                            }
                            s1.Stop(); /////////
                            TimeSpan t1 = s1.Elapsed; //////
                            Console.WriteLine(t1.TotalSeconds); ////////
                        }
                    }

                    IsConvertingMaterial = false;
                }
                catch (Exception)
                {
                    IsConvertingMaterial = false;
                }
            }
        }//end method doconvert


    }//end class
}
