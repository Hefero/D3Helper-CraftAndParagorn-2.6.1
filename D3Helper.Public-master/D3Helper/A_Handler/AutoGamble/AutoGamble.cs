using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

using Enigma.D3;
using Enigma.D3.UI;
using Enigma.D3.UI.Controls;
using Enigma.D3.Helpers;
using Enigma.D3.Assets;

namespace D3Helper.A_Handler.AutoGamble
{
    class AutoGamble
    {
        public static bool isGambling = false;


        public static void start_Gamble()
        {
            try
            {
                if (A_Tools.T_D3UI.UIElement.isVisible(A_Enums.UIElements.ShopDialogMainPage))
                {
                    if (isGambleVendor())
                    {
                        if (tryGetMouseCaptureUIReference().x008_Name.ToLower().Contains("shop_item_region"))
                        {
                            tryAutoGamble();
                        }
                    }
                }
            }
            catch { }

        }
        private static void tryAutoGamble()
        {
            try
            {
                isGambling = true;

                UIReference lastClickedUIReference = tryGetLastClickedUIReference();
                int gambleCosts = tryGetBloodShardCosts(lastClickedUIReference);
                UIRect shopItemRect = A_Tools.T_D3UI.UIElement.getRect(lastClickedUIReference.x008_Name);

                
                    while (
                        tryGetLastClickedUIReference().x008_Name.ToLower().Contains("shop_item_region") &&
                        A_Tools.T_D3UI.UIElement.isVisible(A_Enums.UIElements.ShopDialogMainPage) &&
                        get_BloodShardAmount() >= gambleCosts &&
                        !tryCheckInventoryFull() &&
                        tryCheckEnoughShards() &&
                        A_Collection.D3Client.Window.isForeground
                        )
                    {
                        A_Tools.InputSimulator.IS_Mouse.RightCLick(((int)shopItemRect.Left + ((int)shopItemRect.Width * 10 / 100)), ((int)shopItemRect.Top + ((int)shopItemRect.Height * 10 / 100)), ((int)shopItemRect.Right - ((int)shopItemRect.Width * 10 / 100)), ((int)shopItemRect.Bottom - ((int)shopItemRect.Height * 10 / 100)));

                        System.Threading.Thread.Sleep(100);
                    }
                

                isGambling = false;

            }
            catch { isGambling = false; }
        }
        private static bool tryCheckInventoryFull()
        {
            try
            {
                var inventoryItems = ActorCommonDataHelper.EnumerateInventoryItems();

                if (inventoryItems.Count() <= 20)
                    return false;

                string error_notification_uielement = "Root.TopLayer.error_notify.error_text";
                //string inventory_full_text = "Not enough Inventory space to complete this operation.";
                string item_canot_be_picked = "That item cannot be picked up.";

                var errortext = UXHelper.GetControl<UXLabel>(error_notification_uielement);

                if (errortext.xA20_Text_StructStart_Min84Bytes == item_canot_be_picked && errortext.IsVisible())
                {
                    return true;
                }

                return false;

            }
            catch { return true; }
        }
        private static bool isGambleVendor()
        {
            try
            {
                string Currency_String = UXHelper.GetControl<UXLabel>("Root.NormalLayer.shop_dialog_mainPage.gold_label").xA20_Text_StructStart_Min84Bytes;

                if (Currency_String == "Your Available Gold:")
                {
                    return false;
                }

                return true;
            }
            catch { return false; }
        }
        private static bool tryCheckEnoughShards()
        {
            try
            {
                string error_notification_uielement = "Root.TopLayer.error_notify.error_text";
                string not_enough_shards = "Not enough Blood Shards.";


                var errortext = UXHelper.GetControl<UXLabel>(error_notification_uielement);

                if (errortext.xA20_Text_StructStart_Min84Bytes == not_enough_shards && errortext.IsVisible())
                {
                    return false;
                }

                return true;
            }
            catch { return false; }
        }
        private static UIReference tryGetLastClickedUIReference()
        {
            try
            {
                //return ObjectManager.Instance.x9EC_Ptr_10000Bytes_UI.Dereference().x0828_LastClicked;
                return UIManager.Instance.x0828_LastClicked;
            }
            catch { return null; }
        }
        private static UIReference tryGetMouseCaptureUIReference()
        {
            try
            {
                //return ObjectManager.Instance.x9EC_Ptr_10000Bytes_UI.Dereference().x0008_MouseCapture;
                UIReference uiref = UIManager.Instance.x0008_MouseCapture;
                //Console.WriteLine(uiref.x008_Name);
                return uiref;
            }
            catch { return null; }
        }
        private static int tryGetBloodShardCosts(UIReference lastClickedElement)
        {
            try
            {
                int bloodShardCosts = 10000;




                var text = UXHelper.GetControl<UXLabel>(lastClickedElement.x008_Name + ".text_cost").xA20_Text_StructStart_Min84Bytes;

                var shardstring = Regex.Match(text, @"\d+").Groups[0].Value;

                if (int.TryParse(shardstring, out bloodShardCosts))
                {
                    return bloodShardCosts;
                }


                return bloodShardCosts;
            }
            catch { return 10000; }
        }
        private static long get_BloodShardAmount()
        {
            try
            {
                //uint bloodshardGBId = 2603730171;
                lock(A_Collection.Me.HeroGlobals.LocalPlayerData)
                {
                    //long amount =  A_Collection.Me.HeroGlobals.LocalPlayerData.GetCurrency(CurrencyType.X1Shard);

                   
                    //Enigma.D3.Assets.GameBalance.CurrencyConversionTable.CurrencyConversionTableEntry[1]  // - None - = -1, Gold = 0, BloodShards = 1, Platinum = 2, Reusable Parts = 3, Arcane Dust = 4, Veiled Crystal = 5, Deaths Breath = 6, Forgotten Soul = 7, Khanduran Rune = 8, Caldeum Nightshade = 9, Arreat War Tapestry = 10, Corrupted Angel Flesh = 11, Westmarch Holy Water = 12, Demon Organ Diablo = 13, Demon Organ Ghom = 14, Demon Organ Siege Breaker = 15, Demon Organ Skeleton King = 16, Demon Organ Eye = 17, Demon Organ Spine Cord = 18, Demon Organ Tooth = 19


                    //for patch 2.6.0 workaround. since enigmaframework not updated this part
                    int baseOffset = 0x9D08; //for patch 2.6.0 
                    const int size = 24; // { int index, int 0, long value, int 0, int 0 }
                    const int offset = 8;
                    long amount = -1;
                    //while (amount != 114366383051)
                    //{
                    //    baseOffset++;
                        amount = A_Collection.Me.HeroGlobals.LocalPlayerData.Read<long>(baseOffset + size * (int)CurrencyType.X1Shard + offset);
                    //    if (baseOffset > 0xFFFF)
                    //    {
                    //        amount = -1;
                    //        break;
                    //    }
                    //}


                    return amount;
                }


            }
            catch { return 0; }
        }
    }
}
