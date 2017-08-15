using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Enigma.D3;
using Enigma.D3.UI;

namespace D3Helper.A_Handler.AutoCube
{
    class ConvertMaterial
    {
        public static bool IsConvertingMaterial = false;

        private const string BTN_Transmute = "Root.NormalLayer.vendor_dialog_mainPage.transmute_dialog.LayoutRoot.transmute_button";

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

                    if (CubeNearby)
                    {
                        if (Tools.ClickOnCube(CubeStand))
                        {
                            Stopwatch s1 = new Stopwatch(); /////////
                            s1.Start(); ///////////

                            // Get list of Materials to convert from
                            List<ActorCommonData> Materials;
                            var Count_AvailableEnchants = Tools.Get_AvailableMaterial_Convert(FromMaterialQuality, out Materials);

                            // Get list of equipment of the specified quality
                            var ToItemList = Tools.Get_Items(ToMaterialQuality);

                            var Count_Enchants = 0;

                            foreach (var item in ToItemList)
                            {
                                if (Count_Enchants == Count_AvailableEnchants)
                                    break;

                                if (Tools.ClickOnCube(CubeStand))
                                {

                                    // Find the item position and rightclick on the item
                                    UIRect UIRect_item = A_Collection.D3UI.InventoryItemUIRectMesh.FirstOrDefault(x => x.Key.ItemSlotX == item.x118_ItemSlotX && x.Key.ItemSlotY == item.x11C_ItemSlotY).Value;
                                    A_Tools.InputSimulator.IS_Mouse.RightCLick((int)UIRect_item.Left, (int)UIRect_item.Top, (int)UIRect_item.Right, (int)UIRect_item.Bottom);
                                    Thread.Sleep(200);

                                    // Find the positions and rightclick on materials (RP/AD/VC)
                                    foreach (var material in Materials)
                                    {
                                        UIRect UIRect_material =
                                        A_Collection.D3UI.InventoryItemUIRectMesh.FirstOrDefault(
                                            x => x.Key.ItemSlotX == material.x118_ItemSlotX && x.Key.ItemSlotY == material.x11C_ItemSlotY).Value;

                                        A_Tools.InputSimulator.IS_Mouse.RightCLick((int)UIRect_material.Left, (int)UIRect_material.Top, (int)UIRect_material.Right, (int)UIRect_material.Bottom);
                                        Thread.Sleep(100);
                                    }

                                    // Click the Transmute button
                                    UIRect Transmute = A_Tools.T_D3UI.UIElement.getRect(BTN_Transmute);
                                    A_Tools.InputSimulator.IS_Mouse.LeftClick((int)Transmute.Left, (int)Transmute.Top, (int)Transmute.Right - 10, (int)Transmute.Bottom);
                                    Thread.Sleep(500);

                                    // Close Cube window after transmute
                                    while (Tools.IsVendorPage_Visible())
                                    {
                                        A_Tools.InputSimulator.IS_Keyboard.Close_AllWindows();
                                        Thread.Sleep(250);
                                    }

                                    Count_Enchants++;
                                }
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
        }
    }
}
