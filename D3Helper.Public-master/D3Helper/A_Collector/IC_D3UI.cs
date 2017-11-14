using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Enigma.D3;
using Enigma.D3.UI;

namespace D3Helper.A_Collector
{
    class IC_D3UI
    {
        public static void Collect()
        {
            try
            {
                get_UIElementMouseOver();

                if (A_Collection.Me.HeroStates.isInGame)
                {
                    get_isPlayerContextMenu();
                    get_isChatting();
                    get_isOpenMap();
                    get_isOpenFriendlist();
                    get_isOpenInventory();
                    get_isOpenSkillPanel();
                    get_isOpenBountyMap();
                    get_InventoryMesh();
                    get_isOpenGameMenu();
                    get_isOpenGuildMain();
                    get_isOpenLeaderboards();
                    get_isOpenAchievements();
                    get_isLeavingGame();
                }
            }
            catch (Exception e)
            {
                A_Handler.Log.Exception.addExceptionLogEntry(e, A_Enums.ExceptionThread.ICollector);
            }
        }


        private static void get_UIElementMouseOver()
        {
            try
            {
               //A_Collection.D3UI.UIElement_MouseOver = UIManager.Instance.x0A30_MouseOver.x008_Name;
                
                string current_hoverelementname = Engine.Current.ObjectManager.xA1C_Ptr_10000Bytes_UI.Dereference().x0A30_MouseOver.x008_Name;

                //write hover element to console
                if(A_Collection.D3UI.UIElement_MouseOver != null && current_hoverelementname != null)
                {
                    if(!A_Collection.D3UI.UIElement_MouseOver.Equals(current_hoverelementname))
                    {
                        Console.WriteLine(current_hoverelementname);
                    }
                }

                A_Collection.D3UI.UIElement_MouseOver = current_hoverelementname;

            }
            catch (Exception e)
            {
                A_Handler.Log.Exception.addExceptionLogEntry(e, A_Enums.ExceptionThread.ICollector);
            }
            
        }


        private static void get_isOpenGameMenu()
        {
            try
            {
                A_Collection.D3UI.isOpenGameMenu = A_Tools.T_D3UI.UIElement.isVisible(A_Enums.UIElements.GameMenu);
            }
            catch (Exception e)
            {
                A_Handler.Log.Exception.addExceptionLogEntry(e, A_Enums.ExceptionThread.ICollector);
            }
        }


        private static void get_isOpenAchievements()
        {
            try
            {
                A_Collection.D3UI.isOpenAchievementsMain = A_Tools.T_D3UI.UIElement.isVisible(A_Enums.UIElements.AchievementsMain);
            }
            catch (Exception e)
            {
                A_Handler.Log.Exception.addExceptionLogEntry(e, A_Enums.ExceptionThread.ICollector);
            }
        }


        private static void get_isLeavingGame()
        {
            try
            {
                A_Collection.D3UI.isLeavingGame = A_Tools.T_D3UI.UIElement.isVisible(A_Enums.UIElements.LogoutContainerMain);
            }
            catch (Exception e)
            {
                A_Handler.Log.Exception.addExceptionLogEntry(e, A_Enums.ExceptionThread.ICollector);
            }
        }


        private static void get_isOpenLeaderboards()
        {
            try
            {
                A_Collection.D3UI.isOpenLeaderboardsMain = A_Tools.T_D3UI.UIElement.isVisible(A_Enums.UIElements.LeaderboardsMain);
            }
            catch (Exception e)
            {
                A_Handler.Log.Exception.addExceptionLogEntry(e, A_Enums.ExceptionThread.ICollector);
            }
        }


        private static void get_isOpenGuildMain()
        {
            try
            {
                A_Collection.D3UI.isOpenGuildMain = A_Tools.T_D3UI.UIElement.isVisible(A_Enums.UIElements.GuildMain);
            }
            catch (Exception e)
            {
                A_Handler.Log.Exception.addExceptionLogEntry(e, A_Enums.ExceptionThread.ICollector);
            }
        }


        private static void get_isPlayerContextMenu()
        {
            try
            {
                if (A_Collection.D3UI.UIElement_MouseOver.Contains(A_Enums.UIElements.contextMenu))
                {
                    A_Collection.D3UI.isOpenPlayerContextMenu =
                        A_Tools.T_D3UI.UIElement.isVisible(A_Enums.UIElements.contextMenu);
                }
                else
                {
                    A_Collection.D3UI.isOpenPlayerContextMenu = false;
                }
            }
            catch (Exception e)
            {
                A_Handler.Log.Exception.addExceptionLogEntry(e, A_Enums.ExceptionThread.ICollector);
            }
        }


        private static void get_isChatting()
        {
            try
            {
                A_Collection.D3UI.isChatting = A_Tools.T_D3UI.UIElement.isVisible(A_Enums.UIElements.ChatEditLine);
            }
            catch (Exception e)
            {
                A_Handler.Log.ExceptionLogEntry newEntry = new A_Handler.Log.ExceptionLogEntry(e, DateTime.Now, A_Enums.ExceptionThread.ICollector);

                lock (A_Handler.Log.Exception.ExceptionLog) A_Handler.Log.Exception.ExceptionLog.Add(newEntry);
            }
        }


        private static void get_isOpenMap()
        {
            try
            {
                A_Collection.D3UI.isOpenMap = A_Tools.T_D3UI.UIElement.isVisible(A_Enums.UIElements.LocalMap);
            }
            catch (Exception e)
            {
                A_Handler.Log.Exception.addExceptionLogEntry(e, A_Enums.ExceptionThread.ICollector);
            }
        }


        private static void get_isOpenFriendlist()
        {
            try
            {
                A_Collection.D3UI.isOpenFriendlist = A_Tools.T_D3UI.UIElement.isVisible(A_Enums.UIElements.FriendListContent);
            }
            catch (Exception e)
            {
                A_Handler.Log.Exception.addExceptionLogEntry(e, A_Enums.ExceptionThread.ICollector);
            }
        }


        private static void get_isOpenInventory()
        {
            try
            {
                A_Collection.D3UI.isOpenInventory = A_Tools.T_D3UI.UIElement.isVisible(A_Enums.UIElements.Inventory);
            }
            catch (Exception e)
            {
                A_Handler.Log.Exception.addExceptionLogEntry(e, A_Enums.ExceptionThread.ICollector);
            }
        }


        private static void get_isOpenSkillPanel()
        {
            try
            {
                if (A_Tools.T_D3UI.UIElement.isVisible(A_Enums.UIElements.SkillPanel) || A_Tools.T_D3UI.UIElement.isVisible(A_Enums.UIElements.ActiveSkillList))
                {
                    A_Collection.D3UI.isOpenSkillPanel = true;
                }

                A_Collection.D3UI.isOpenSkillPanel = false;
            }
            catch (Exception e)
            {
                A_Handler.Log.Exception.addExceptionLogEntry(e, A_Enums.ExceptionThread.ICollector);
            }
        }

        private static long bountyOpenTimestamp = 0;

        private static void get_isOpenBountyMap()
        {
            try
            {
                A_Collection.D3UI.isOpenBountyMap = A_Tools.T_D3UI.UIElement.isVisible(A_Enums.UIElements.BountyMap);

                //fix: isOpenBountyMap stays true 1 second after its closed to prevent cast on teleport from bountymap
                if (A_Collection.D3UI.isOpenBountyMap)
                {
                    bountyOpenTimestamp = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
                }
                else
                {
                    long now_milliseconds = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
                    long time_after_bountymap_close = now_milliseconds - bountyOpenTimestamp;
                    if(time_after_bountymap_close < 1000)
                    {
                        A_Collection.D3UI.isOpenBountyMap = true;
                    }
                }

            }
            catch (Exception e)
            {
                A_Handler.Log.Exception.addExceptionLogEntry(e, A_Enums.ExceptionThread.ICollector);
            }
        }


        private static void get_InventoryMesh()
        {
            try
            {
                if(A_Collection.D3UI.InventoryItemUIRectMesh.Count < 1)
                {
                    A_Tools.T_D3UI.Inventory.create_InventoryMesh();
                }
            }
            catch (Exception e)
            {
                A_Handler.Log.Exception.addExceptionLogEntry(e, A_Enums.ExceptionThread.ICollector);
            }
        }
    }
}
