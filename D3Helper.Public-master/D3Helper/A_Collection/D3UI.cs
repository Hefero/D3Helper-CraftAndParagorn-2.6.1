using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Enigma.D3.UI;

using D3Helper.A_Tools;

namespace D3Helper.A_Collection
{
    class D3UI
    {
        public static bool isChatting;                              // default: false
        public static bool isOpenMap;                               // default: false
        public static bool isOpenFriendlist;                        // default: false
        public static bool isOpenInventory;                         // default: false
        public static bool isOpenSkillPanel;                        // default: false
        public static bool isOpenBountyMap;                         // default: false
        public static bool isOpenPlayerContextMenu;                 // default: false
        public static bool isOpenGameMenu;
        public static bool isOpenGuildMain;
        public static bool isOpenLeaderboardsMain;
        public static bool isOpenAchievementsMain;
        public static bool isLeavingGame;
        public static string UIElement_MouseOver;
        public static Dictionary<InventoryItemSlot, UIRect> InventoryItemUIRectMesh = new Dictionary<InventoryItemSlot, UIRect>();
        
    }
}
