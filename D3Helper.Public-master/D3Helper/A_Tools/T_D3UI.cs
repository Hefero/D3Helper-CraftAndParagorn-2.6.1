using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Enigma.D3.Helpers;
using Enigma.D3.UI.Controls;
using Enigma.D3;
using Enigma.D3.UI;
using D3Helper.A_Enums;

namespace D3Helper.A_Tools
{
    public class InventoryItemSlot
    {
        public InventoryItemSlot(int itemSlotX, int itemSlotY)
        {
            this.ItemSlotX = itemSlotX;
            this.ItemSlotY = itemSlotY;

        }

        public int ItemSlotX { get; set; }
        public int ItemSlotY { get; set; }

    }
    class T_D3UI
    {
        public class UIElement
        {
            public static bool isVisible(string ControlName)
            {
                try
                {
                    return UXHelper.GetControl<UXChatEditLine>(ControlName).IsVisible();
                }
                catch { return false; }
            }
            public static UIRect getRect(string control)
            {
                
                try
                {
                    if (A_Collection.Environment.Scene.GameTick > 1 && A_Collection.Environment.Scene.Counter_CurrentFrame != 0)
                    {
                        
                        UXItemsControl _control = UXHelper.GetControl<UXItemsControl>(control);

                        int video_width = A_Collection.D3Client.Window.D3ClientRect.Width;
                        int video_height = A_Collection.D3Client.Window.D3ClientRect.Height;

                        UIRect rect = _control.x468_UIRect.TranslateToClientRect(video_width, video_height);
                        return rect;
                    }
                    return default(UIRect);
                }
                catch { return default(UIRect); }

            }


            public static void leftClick(UIRect uirect)
            {
                try
                {
                    int center_x = (int)(uirect.Left + uirect.Width / 2);
                    int center_y = (int)(uirect.Top + uirect.Height / 2);

                    A_Tools.InputSimulator.IS_Mouse.LeftClick(center_x - 2, center_y - 2, center_x + 2, center_y + 2);

                    Random random = new Random();
                    int MinDelayClick = Properties.Settings.Default.MinDelayClick;
                    int MaxDelayClick = Properties.Settings.Default.MaxDelayClick;
                    int random_sleep = (int)random.Next(MinDelayClick, MaxDelayClick);
                    System.Threading.Thread.Sleep(random_sleep);
                }
                catch { }
            }

            public static void leftClick(string control)
            {
                try
                {
                    UIRect uirect = A_Tools.T_D3UI.UIElement.getRect(control);
                    leftClick(uirect);
                }
                catch { }
            }


            public static void rightClick(UIRect uirect)
            {
                try
                {
                    int center_x = (int)(uirect.Left + uirect.Width / 2);
                    int center_y = (int)(uirect.Top + uirect.Height / 2);

                    A_Tools.InputSimulator.IS_Mouse.RightCLick(center_x - 2, center_y - 2, center_x +2, center_y + 2);

                    Random random = new Random();
                    int random_sleep = (int)random.Next(200, 250);
                    System.Threading.Thread.Sleep(random_sleep);
                }
                catch { }
            }


            public static void rightClick(string control)
            {
                try
                {
                    UIRect uirect = A_Tools.T_D3UI.UIElement.getRect(control);
                    rightClick(uirect);
                }
                catch { }
            }


        }
        public class Inventory
        {
            public static UIRect get_InventoryUIRect()
            {
                var Inventory = "Root.NormalLayer.inventory_dialog_mainPage.inventory_button_backpack";
                return UIElement.getRect(Inventory);
            }
            public static void create_InventoryMesh()
            {
                var Inventory = "Root.NormalLayer.inventory_dialog_mainPage.inventory_button_backpack";
                var inventoryUIRect = UIElement.getRect(Inventory);
                var inventoryRectLeft = inventoryUIRect.Left;
                var inventoryRectTop = inventoryUIRect.Top;
                var inventoryRectRight = inventoryUIRect.Right;
                var inventoryRectBottom = inventoryUIRect.Bottom;

                var itemSlotX = (inventoryRectRight - inventoryRectLeft) / 10;
                var itemSlotY = (inventoryRectBottom - inventoryRectTop) / 6;

                for (int iX = 0; iX <= 9; iX++)
                {
                    for (int iY = 0; iY <= 5; iY++)
                    {
                        UIRect newItemSlot = new UIRect();
                        newItemSlot.Left = inventoryRectLeft + (itemSlotX * iX);
                        newItemSlot.Right = inventoryRectLeft + (itemSlotX * (iX + 1));

                        newItemSlot.Top = inventoryRectTop + (itemSlotY * iY);
                        newItemSlot.Bottom = inventoryRectTop + (itemSlotY * (iY + 1));

                        A_Collection.D3UI.InventoryItemUIRectMesh.Add(new InventoryItemSlot(iX, iY), newItemSlot);
                    }
                }


            }
        }
    }
}
