using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using System.Drawing;
using System.Runtime.InteropServices;
using Enigma.D3;
using Enigma.D3.Helpers;
using Enigma.D3.UI;
using Enigma.D3.UI.Controls;

using WindowsInput;
using D3Helper.A_Enums;
using D3Helper.A_Tools;
using Enigma.D3.Enums;

namespace D3Helper.A_Handler.ParagonPointSpender
{
    class ParagonPointSpender
    {
        public static void Set_ParagonPoints()
        {

            A_Collection.Me.ParagonPointSpender.Is_SpendingPoints = true;

            var Setup =
                A_Collection.Me.ParagonPointSpender.Setups.FirstOrDefault(x => x.Key == A_Collection.Me.HeroGlobals.HeroID);

            if (Setup.Value != null)
            {
                var ParagonPoints =
                    Setup.Value.FirstOrDefault(
                        x => x.ID == A_Collection.Me.ParagonPointSpender.SelectedParagonPoints_Setup - 1);

                if (ParagonPoints != null)
                {
                    OpenParagonWindow();

                    bool ResetedCore = false;
                    bool ResetedOffense = false;
                    bool ResetedDefense = false;
                    bool ResetedUtility = false;

                    bool CheckedCore = false;
                    bool CheckedOffense = false;
                    bool CheckedDefense = false;
                    bool CheckedUtility = false;

                    int spentpoints = 0;
                    int tab = 0;

                    foreach (var paragonpoint in ParagonPoints.BonusPoints)
                    {
                        tab = OpenTab_byType(paragonpoint.Type);

                        bool ResetNeeded = false;

                        if (!CheckedCore)
                        {
                            if (paragonpoint.Type == BonusPoints.core0 ||
                                paragonpoint.Type == BonusPoints.core1 ||
                                paragonpoint.Type == BonusPoints.core2 ||
                                paragonpoint.Type == BonusPoints.core3)
                            {
                                var corepoints =
                                    ParagonPoints.BonusPoints.Where(
                                        x =>
                                            x.Type == BonusPoints.core0 || x.Type == BonusPoints.core1 ||
                                            x.Type == BonusPoints.core2 || x.Type == BonusPoints.core3);

                                
                                foreach (var _core in corepoints)
                                {
                                    if (ParagonPointSpender.ResetNeeded(_core))
                                    {
                                        ResetNeeded = true;
                                        break;
                                    }
                                }

                                int[] alrdySetPoints = new int[] { GetSpentPoints_OnGivenBonus(0), GetSpentPoints_OnGivenBonus(1), GetSpentPoints_OnGivenBonus(2), GetSpentPoints_OnGivenBonus(3) };

                                if (corepoints.Where(x => x.Value != 0).Count() < alrdySetPoints.Where(x => x > 0).Count() || (corepoints.Where(x => x.Value != 0).Count() > alrdySetPoints.Where(x => x > 0).Count() && Get_UnspentPoints_Core() == 0))
                                    ResetNeeded = true;

                                CheckedCore = true;
                            }
                        }

                        if (!CheckedOffense)
                        {
                            if (paragonpoint.Type == BonusPoints.offense0 ||
                                paragonpoint.Type == BonusPoints.offense1 ||
                                paragonpoint.Type == BonusPoints.offense2 ||
                                paragonpoint.Type == BonusPoints.offense3)
                            {
                                var offensepoints =
                                    ParagonPoints.BonusPoints.Where(
                                        x =>
                                            x.Type == BonusPoints.offense0 || x.Type == BonusPoints.offense1 ||
                                            x.Type == BonusPoints.offense2 || x.Type == BonusPoints.offense3);

                                foreach (var _offense in offensepoints)
                                {
                                    if (ParagonPointSpender.ResetNeeded(_offense))
                                    {
                                        ResetNeeded = true;
                                        break;
                                    }
                                }

                                int[] alrdySetPoints = new int[] { GetSpentPoints_OnGivenBonus(0), GetSpentPoints_OnGivenBonus(1), GetSpentPoints_OnGivenBonus(2), GetSpentPoints_OnGivenBonus(3) };

                                if (offensepoints.Where(x => x.Value != 0).Count() < alrdySetPoints.Where(x => x > 0).Count() || (offensepoints.Where(x => x.Value != 0).Count() > alrdySetPoints.Where(x => x > 0).Count() && Get_UnspentPoints_Offense() == 0))
                                    ResetNeeded = true;

                                CheckedOffense = true;
                            }
                        }

                        if (!CheckedDefense)
                        {
                            if (paragonpoint.Type == BonusPoints.defense0 ||
                                paragonpoint.Type == BonusPoints.defense1 ||
                                paragonpoint.Type == BonusPoints.defense2 ||
                                paragonpoint.Type == BonusPoints.defense3)
                            {
                                var defensepoints =
                                    ParagonPoints.BonusPoints.Where(
                                        x =>
                                            x.Type == BonusPoints.defense0 || x.Type == BonusPoints.defense1 ||
                                            x.Type == BonusPoints.defense2 || x.Type == BonusPoints.defense3);

                                foreach (var _defense in defensepoints)
                                {
                                    if (ParagonPointSpender.ResetNeeded(_defense))
                                    {
                                        ResetNeeded = true;
                                        break;
                                    }
                                }

                                int[] alrdySetPoints = new int[] { GetSpentPoints_OnGivenBonus(0), GetSpentPoints_OnGivenBonus(1), GetSpentPoints_OnGivenBonus(2), GetSpentPoints_OnGivenBonus(3) };

                                if (defensepoints.Where(x => x.Value != 0).Count() < alrdySetPoints.Where(x => x > 0).Count() || (defensepoints.Where(x => x.Value != 0).Count() > alrdySetPoints.Where(x => x > 0).Count() && Get_UnspentPoints_Defense() == 0))
                                    ResetNeeded = true;

                                CheckedDefense = true;
                            }
                        }

                        if (!CheckedUtility)
                        {
                            if (paragonpoint.Type == BonusPoints.utility0 ||
                                paragonpoint.Type == BonusPoints.utility1 ||
                                paragonpoint.Type == BonusPoints.utility2 ||
                                paragonpoint.Type == BonusPoints.utility3)
                            {
                                var utilitypoints =
                                    ParagonPoints.BonusPoints.Where(
                                        x =>
                                            x.Type == BonusPoints.utility0 || x.Type == BonusPoints.utility1 ||
                                            x.Type == BonusPoints.utility2 || x.Type == BonusPoints.utility3);

                                foreach (var _utility in utilitypoints)
                                {
                                    if (ParagonPointSpender.ResetNeeded(_utility))
                                    {
                                        ResetNeeded = true;
                                        break;
                                    }
                                }

                                int[] alrdySetPoints = new int[] { GetSpentPoints_OnGivenBonus(0), GetSpentPoints_OnGivenBonus(1), GetSpentPoints_OnGivenBonus(2), GetSpentPoints_OnGivenBonus(3) };

                                if (utilitypoints.Where(x => x.Value != 0).Count() < alrdySetPoints.Where(x => x > 0).Count() || (utilitypoints.Where(x => x.Value != 0).Count() > alrdySetPoints.Where(x => x > 0).Count() && Get_UnspentPoints_Utility() == 0))
                                    ResetNeeded = true;

                                CheckedUtility = true;
                            }
                        }

                        if (ResetNeeded)
                        {
                            switch (tab)
                            {
                                case 0:
                                    if (!ResetedCore)
                                    {
                                        Reset_ParagonPoints(0);
                                        ResetedCore = true;
                                    }
                                    break;

                                case 1:
                                    if (!ResetedOffense)
                                    {
                                        Reset_ParagonPoints(1);
                                        ResetedOffense = true;
                                    }
                                    break;

                                case 2:
                                    if (!ResetedDefense)
                                    {
                                        Reset_ParagonPoints(2);
                                        ResetedDefense = true;
                                    }
                                    break;

                                case 3:
                                    if (!ResetedUtility)
                                    {
                                        Reset_ParagonPoints(3);
                                        ResetedUtility = true;
                                    }
                                    break;
                            }

                            
                        }

                        spentpoints += SpendPoints_byType(paragonpoint.Type, paragonpoint.Value);
                    }

                    if (spentpoints > 0)
                        Accept_ParagonPoints();


                    if (isParagonWindowVisible())
                        CloseParagonWindow();

                    int _x = A_Collection.D3Client.Window.D3ClientRect.Width/2;
                    int _y = A_Collection.D3Client.Window.D3ClientRect.Height/2;

                    Cursor.Position = new Point((int) _x, (int) _y);
                }
            }

            A_Collection.Me.ParagonPointSpender.Is_SpendingPoints = false;

        }

        private static bool ResetNeeded(BonusPoint bonuspoint)
        {
            try
            {
                switch (bonuspoint.Type)
                {
                        case BonusPoints.core0:
                        if (bonuspoint.Value != -1)
                        {
                            var curValue = GetSpentPoints_OnGivenBonus(0);
                            if(curValue > bonuspoint.Value || (curValue < bonuspoint.Value && Get_UnspentPoints_Core() == 0))
                                return true;
                        }
                        return false;

                    case BonusPoints.core1:
                        if (bonuspoint.Value != -1)
                        {
                            var curValue = GetSpentPoints_OnGivenBonus(1);
                            if (curValue > bonuspoint.Value || (curValue < bonuspoint.Value && Get_UnspentPoints_Core() == 0))
                                return true;
                        }
                        return false;

                    case BonusPoints.core2:
                        if (bonuspoint.Value != -1)
                        {
                            var curValue = GetSpentPoints_OnGivenBonus(2);
                            if (curValue > bonuspoint.Value || (curValue < bonuspoint.Value && Get_UnspentPoints_Core() == 0))
                                return true;
                        }
                        else if (bonuspoint.Value == -1)
                        {
                            var curValue = GetSpentPoints_OnGivenBonus(2);
                            var pointsneeded = GetPointsNeeded_ToMaxMovement();
                            if (pointsneeded < 0 || (pointsneeded > 0 && Get_UnspentPoints_Core() < pointsneeded))
                                return true;
                        }
                        return false;

                    case BonusPoints.core3:
                        if (bonuspoint.Value != -1)
                        {
                            var curValue = GetSpentPoints_OnGivenBonus(3);
                            if (curValue > bonuspoint.Value || (curValue < bonuspoint.Value && Get_UnspentPoints_Core() == 0))
                                return true;
                        }
                        return false;

                    case BonusPoints.offense0:
                        if (bonuspoint.Value != -1)
                        {
                            var curValue = GetSpentPoints_OnGivenBonus(0);
                            if (curValue > bonuspoint.Value || (curValue < bonuspoint.Value && Get_UnspentPoints_Offense() == 0))
                                return true;
                        }
                        return false;

                    case BonusPoints.offense1:
                        if (bonuspoint.Value != -1)
                        {
                            var curValue = GetSpentPoints_OnGivenBonus(1);
                            if (curValue > bonuspoint.Value || (curValue < bonuspoint.Value && Get_UnspentPoints_Offense() == 0))
                                return true;
                        }
                        return false;

                    case BonusPoints.offense2:
                        if (bonuspoint.Value != -1)
                        {
                            var curValue = GetSpentPoints_OnGivenBonus(2);
                            if (curValue > bonuspoint.Value || (curValue < bonuspoint.Value && Get_UnspentPoints_Offense() == 0))
                                return true;
                        }
                        return false;

                    case BonusPoints.offense3:
                        if (bonuspoint.Value != -1)
                        {
                            var curValue = GetSpentPoints_OnGivenBonus(3);
                            if (curValue > bonuspoint.Value || (curValue < bonuspoint.Value && Get_UnspentPoints_Offense() == 0))
                                return true;
                        }
                        return false;

                    case BonusPoints.defense0:
                        if (bonuspoint.Value != -1)
                        {
                            var curValue = GetSpentPoints_OnGivenBonus(0);
                            if (curValue > bonuspoint.Value || (curValue < bonuspoint.Value && Get_UnspentPoints_Defense() == 0))
                                return true;
                        }
                        return false;

                    case BonusPoints.defense1:
                        if (bonuspoint.Value != -1)
                        {
                            var curValue = GetSpentPoints_OnGivenBonus(1);
                            if (curValue > bonuspoint.Value || (curValue < bonuspoint.Value && Get_UnspentPoints_Defense() == 0))
                                return true;
                        }
                        return false;

                    case BonusPoints.defense2:
                        if (bonuspoint.Value != -1)
                        {
                            var curValue = GetSpentPoints_OnGivenBonus(2);
                            if (curValue > bonuspoint.Value || (curValue < bonuspoint.Value && Get_UnspentPoints_Defense() == 0))
                                return true;
                        }
                        return false;

                    case BonusPoints.defense3:
                        if (bonuspoint.Value != -1)
                        {
                            var curValue = GetSpentPoints_OnGivenBonus(3);
                            if (curValue > bonuspoint.Value || (curValue < bonuspoint.Value && Get_UnspentPoints_Defense() == 0))
                                return true;
                        }
                        return false;

                    case BonusPoints.utility0:
                        if (bonuspoint.Value != -1)
                        {
                            var curValue = GetSpentPoints_OnGivenBonus(0);
                            if (curValue > bonuspoint.Value || (curValue < bonuspoint.Value && Get_UnspentPoints_Utility() == 0))
                                return true;
                        }
                        return false;

                    case BonusPoints.utility1:
                        if (bonuspoint.Value != -1)
                        {
                            var curValue = GetSpentPoints_OnGivenBonus(1);
                            if (curValue > bonuspoint.Value || (curValue < bonuspoint.Value && Get_UnspentPoints_Utility() == 0))
                                return true;
                        }
                        return false;

                    case BonusPoints.utility2:
                        if (bonuspoint.Value != -1)
                        {
                            var curValue = GetSpentPoints_OnGivenBonus(2);
                            if (curValue > bonuspoint.Value || (curValue < bonuspoint.Value && Get_UnspentPoints_Utility() == 0))
                                return true;
                        }
                        return false;

                    case BonusPoints.utility3:
                        if (bonuspoint.Value != -1)
                        {
                            var curValue = GetSpentPoints_OnGivenBonus(3);
                            if (curValue > bonuspoint.Value || (curValue < bonuspoint.Value && Get_UnspentPoints_Utility() == 0))
                                return true;
                        }
                        return false;

                    default:
                        return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static int GetValue_toSpend(BonusPoints Type, int Value)
        {
            if(Value > 0)
                return Value;

            if (Value == 0)
                return 0;

            switch (Type)
            {
                case BonusPoints.core0:
                case BonusPoints.core1:
                    return Get_UnspentPoints_Core();

                case BonusPoints.core2:
                    return GetPointsNeeded_ToMaxMovement();

                case BonusPoints.core3:
                    return Get_UnspentPoints_Core();

                case BonusPoints.offense0:
                case BonusPoints.offense1:
                case BonusPoints.offense2:
                case BonusPoints.offense3:
                    return Get_UnspentPoints_Offense();

                case BonusPoints.defense0:
                case BonusPoints.defense1:
                case BonusPoints.defense2:
                case BonusPoints.defense3:
                    return Get_UnspentPoints_Defense();

                case BonusPoints.utility0:
                case BonusPoints.utility1:
                case BonusPoints.utility2:
                case BonusPoints.utility3:
                    return Get_UnspentPoints_Utility();


                default:
                    return 0;
            }
        }
        private static int SpendPoints_byType(BonusPoints Type, int Value)
        {
            try
            {
                int spentpoints = 0;

                switch (Type)
                {
                        case BonusPoints.core0:
                        spentpoints += Spent_Points(0, GetValue_toSpend(Type,Value), 0);
                        break;

                    case BonusPoints.core1:
                        spentpoints += Spent_Points(1, GetValue_toSpend(Type, Value), 0);
                        break;

                    case BonusPoints.core2:
                        spentpoints += Spent_Points(2, GetValue_toSpend(Type, Value), 0);
                        break;

                    case BonusPoints.core3:
                        spentpoints += Spent_Points(3, GetValue_toSpend(Type, Value), 0);
                        break;

                    case BonusPoints.offense0:
                        spentpoints += Spent_Points(0, GetValue_toSpend(Type, Value), 1);
                        break;

                    case BonusPoints.offense1:
                        spentpoints += Spent_Points(1, GetValue_toSpend(Type, Value), 1);
                        break;

                    case BonusPoints.offense2:
                        spentpoints += Spent_Points(2, GetValue_toSpend(Type, Value), 1);
                        break;

                    case BonusPoints.offense3:
                        spentpoints += Spent_Points(3, GetValue_toSpend(Type, Value), 1);
                        break;

                    case BonusPoints.defense0:
                        spentpoints += Spent_Points(0, GetValue_toSpend(Type, Value), 2);
                        break;

                    case BonusPoints.defense1:
                        spentpoints += Spent_Points(1, GetValue_toSpend(Type, Value), 2);
                        break;

                    case BonusPoints.defense2:
                        spentpoints += Spent_Points(2, GetValue_toSpend(Type, Value), 2);
                        break;

                    case BonusPoints.defense3:
                        spentpoints += Spent_Points(3, GetValue_toSpend(Type, Value), 2);
                        break;

                    case BonusPoints.utility0:
                        spentpoints += Spent_Points(0, GetValue_toSpend(Type, Value), 3);
                        break;

                    case BonusPoints.utility1:
                        spentpoints += Spent_Points(1, GetValue_toSpend(Type, Value), 3);
                        break;

                    case BonusPoints.utility2:
                        spentpoints += Spent_Points(2, GetValue_toSpend(Type, Value), 3);
                        break;

                    case BonusPoints.utility3:
                        spentpoints += Spent_Points(3, GetValue_toSpend(Type, Value), 3);
                        break;
                }

                return spentpoints;
            }
            catch (Exception)
            {
                return 0;
            }
        }
        private static int OpenTab_byType(BonusPoints Type)
        {
            try
            {
                switch (Type)
                {
                        case BonusPoints.core0:
                        case BonusPoints.core1:
                        case BonusPoints.core2:
                        case BonusPoints.core3:
                        OpenCore_Tab();
                        return 0;

                        case BonusPoints.offense0:
                        case BonusPoints.offense1:
                        case BonusPoints.offense2:
                        case BonusPoints.offense3:
                        OpenOffense_Tab();
                        return 1;

                        case BonusPoints.defense0:
                        case BonusPoints.defense1:
                        case BonusPoints.defense2:
                        case BonusPoints.defense3:
                        OpenDefense_Tab();
                        return 2;

                        case BonusPoints.utility0:
                        case BonusPoints.utility1:
                        case BonusPoints.utility2:
                        case BonusPoints.utility3:
                        OpenUtility_Tab();
                        return 3;

                }
                return 0;
            }
            catch (Exception)
            {
                return 0;
            }
        }
        private static int GetPointsNeeded_ToMaxMovement()
        {
            try
            {
                ActorCommonData local;
                lock (A_Collection.Me.HeroGlobals.LocalACD) local = A_Collection.Me.HeroGlobals.LocalACD;

                double scalar = local.GetAttributeValue(AttributeId.MovementScalar);
                
                double msBonus = Math.Round((scalar - 1) * 100);

                double msleft = 25 - msBonus;

                //if(msleft > 0)
                return (int)msleft*2;
                //else
                //{
                //    return 0;
                //}
            }
            catch (Exception)
            {
                return 0;
            }
        }
        private static int GetSpentPoints_OnGivenBonus(int Bonus) // 0-3
        {
            try
            {
                string spentPoints = UXHelper.GetControl<UXLabel>(A_Enums.UIElements.ParagonPointSelect_BonusX_Base + Bonus + A_Enums.UIElements.ParagonPointSelect_BonusX_Extension).xA20_Text_StructStart_Min84Bytes;

                if (spentPoints.Contains("/"))
                {
                    spentPoints = spentPoints.Split('/')[0];
                }

                int value = int.Parse(spentPoints);

                return value;

            }
            catch { return 90000; }
        }
        private static void CloseParagonWindow()
        {
            try
            {
                while (isParagonWindowVisible())
                {
                    A_Tools.InputSimulator.IS_Keyboard.ParagonWindow();

                    Thread.Sleep(50);
                }
            }
            catch { }
        }
        private static void OpenParagonWindow()
        {
            try
            {
                while (!isParagonWindowVisible())
                {
                    A_Tools.InputSimulator.IS_Keyboard.ParagonWindow();

                    Thread.Sleep(50);
                }
            }
            catch { }
        }
        private static bool OpenCore_Tab()
        {
            try
            {
                while (isParagonWindowVisible())
                {
                    if (UXHelper.GetControl<UXLabel>("Root.NormalLayer.Paragon_main.LayoutRoot.ParagonPointSelect.Bonuses.bonus2.Stat").xA20_Text_StructStart_Min84Bytes == "Movement Speed")
                    {
                        return true;
                    }

                    UIRect Tab = A_Tools.T_D3UI.UIElement.getRect(A_Enums.UIElements.ParagonPointSelect_TabCore);

                    A_Tools.InputSimulator.IS_Mouse.LeftClick((int)Tab.Left, (int)Tab.Top, (int)Tab.Right, (int)Tab.Bottom);

                    Thread.Sleep(50);
                }

                return false;
            }
            catch { return false; }
        }
        private static bool OpenOffense_Tab()
        {
            try
            {
                while (isParagonWindowVisible())
                {
                    if (UXHelper.GetControl<UXLabel>("Root.NormalLayer.Paragon_main.LayoutRoot.ParagonPointSelect.Bonuses.bonus2.Stat").xA20_Text_StructStart_Min84Bytes == "Critical Hit Chance")
                    {
                        return true;
                    }

                    UIRect Tab = A_Tools.T_D3UI.UIElement.getRect(A_Enums.UIElements.ParagonPointSelect_TabOffense);

                    A_Tools.InputSimulator.IS_Mouse.LeftClick((int)Tab.Left, (int)Tab.Top, (int)Tab.Right, (int)Tab.Bottom);

                    Thread.Sleep(50);
                }
                return false;
            }
            catch { return false; }
        }
        private static bool OpenDefense_Tab()
        {
            try
            {
                while (isParagonWindowVisible())
                {
                    if (UXHelper.GetControl<UXLabel>("Root.NormalLayer.Paragon_main.LayoutRoot.ParagonPointSelect.Bonuses.bonus2.Stat").xA20_Text_StructStart_Min84Bytes == "Resist All")
                    {
                        return true;
                    }

                    UIRect Tab = A_Tools.T_D3UI.UIElement.getRect(A_Enums.UIElements.ParagonPointSelect_TabDefense);

                    A_Tools.InputSimulator.IS_Mouse.LeftClick((int)Tab.Left, (int)Tab.Top, (int)Tab.Right, (int)Tab.Bottom);

                    Thread.Sleep(50);
                }

                return false;
            }
            catch { return false; }
        }
        private static bool OpenUtility_Tab()
        {
            try
            {
                while (isParagonWindowVisible())
                {
                    if (UXHelper.GetControl<UXLabel>("Root.NormalLayer.Paragon_main.LayoutRoot.ParagonPointSelect.Bonuses.bonus2.Stat").xA20_Text_StructStart_Min84Bytes == "Life On Hit")
                    {
                        return true;
                    }

                    UIRect Tab = A_Tools.T_D3UI.UIElement.getRect(A_Enums.UIElements.ParagonPointSelect_TabUtility);

                    A_Tools.InputSimulator.IS_Mouse.LeftClick((int)Tab.Left, (int)Tab.Top, (int)Tab.Right, (int)Tab.Bottom);

                    Thread.Sleep(50);
                }

                return false;

            }
            catch { return false; }
        }
        private static bool Reset_ParagonPoints(int tab)
        {
            try
            {
                int TotalPoints_CurrentTab = Get_TotalPoints_CurrentTab();

                while (isParagonWindowVisible())
                {

                    switch (tab)
                    {
                        case 0:
                            if (Get_UnspentPoints_Core() == TotalPoints_CurrentTab)
                            {
                                return true;
                            }
                            break;
                        case 1:
                            if (Get_UnspentPoints_Offense() == TotalPoints_CurrentTab)
                            {
                                return true;
                            }
                            break;
                        case 2:
                            if (Get_UnspentPoints_Defense() == TotalPoints_CurrentTab)
                            {
                                return true;
                            }
                            break;
                        case 3:
                            if (Get_UnspentPoints_Utility() == TotalPoints_CurrentTab)
                            {
                                return true;
                            }
                            break;

                    }

                    UIRect Reset = A_Tools.T_D3UI.UIElement.getRect(A_Enums.UIElements.ParagonPointSelect_Reset_Button);

                    A_Tools.InputSimulator.IS_Mouse.LeftClick((int)Reset.Left, (int)Reset.Top, (int)Reset.Right, (int)Reset.Bottom);

                    Thread.Sleep(100);
                }

                return false;
            }
            catch { return false; }
        }
        private static void Accept_ParagonPoints()
        {
            try
            {
                while (isParagonWindowVisible())
                {
                    UIRect Accept = A_Tools.T_D3UI.UIElement.getRect(A_Enums.UIElements.ParagonPointSelect_Accept_Button);

                    A_Tools.InputSimulator.IS_Mouse.LeftClick((int)Accept.Left, (int)Accept.Top, (int)Accept.Right, (int)Accept.Bottom);

                    Thread.Sleep(100);
                }
            }
            catch { }
        }
        private static int Spent_Points(int Bonus, int Value, int Tab)
        {

            string Control = "";

            int counter = 0;

            int FixSleep = 1;

            switch (Bonus)
            {
                case 0:
                    Control = A_Enums.UIElements.ParagonPointSelect_SpentPoint_Bonus0;


                    while (isParagonWindowVisible() && counter < Value && A_Tools.T_D3UI.UIElement.isVisible(Control))
                    {


                        UIRect Increase = A_Tools.T_D3UI.UIElement.getRect(Control);

                        if (Value - counter >= 10)
                        {
                            InputSimulator.SimulateKeyDown(VirtualKeyCode.SHIFT);

                            A_Tools.InputSimulator.IS_Mouse.LeftClick((int)Increase.Left, (int)Increase.Top, (int)Increase.Right, (int)Increase.Bottom);

                            InputSimulator.SimulateKeyUp(VirtualKeyCode.SHIFT);

                            counter = counter + 10;
                        }
                        else
                        {
                            A_Tools.InputSimulator.IS_Mouse.LeftClick((int)Increase.Left, (int)Increase.Top, (int)Increase.Right, (int)Increase.Bottom);

                            counter = counter + 1;
                        }
                        Thread.Sleep(FixSleep);
                    }

                    break;
                case 1:
                    Control = A_Enums.UIElements.ParagonPointSelect_SpentPoint_Bonus1;



                    while (isParagonWindowVisible() && counter < Value && A_Tools.T_D3UI.UIElement.isVisible(Control))
                    {


                        UIRect Increase = A_Tools.T_D3UI.UIElement.getRect(Control);

                        if (Value - counter >= 10)
                        {
                            InputSimulator.SimulateKeyDown(VirtualKeyCode.SHIFT);

                            A_Tools.InputSimulator.IS_Mouse.LeftClick((int)Increase.Left, (int)Increase.Top, (int)Increase.Right, (int)Increase.Bottom);

                            InputSimulator.SimulateKeyUp(VirtualKeyCode.SHIFT);

                            counter = counter + 10;
                        }
                        else
                        {
                            A_Tools.InputSimulator.IS_Mouse.LeftClick((int)Increase.Left, (int)Increase.Top, (int)Increase.Right, (int)Increase.Bottom);

                            counter = counter + 1;
                        }
                        Thread.Sleep(FixSleep);
                    }

                    break;
                case 2:
                    Control = A_Enums.UIElements.ParagonPointSelect_SpentPoint_Bonus2;



                    while (isParagonWindowVisible() && counter < Value && A_Tools.T_D3UI.UIElement.isVisible(Control))
                    {


                        UIRect Increase = A_Tools.T_D3UI.UIElement.getRect(Control);

                        if (Value - counter >= 10)
                        {
                            InputSimulator.SimulateKeyDown(VirtualKeyCode.SHIFT);

                            A_Tools.InputSimulator.IS_Mouse.LeftClick((int)Increase.Left, (int)Increase.Top, (int)Increase.Right, (int)Increase.Bottom);

                            InputSimulator.SimulateKeyUp(VirtualKeyCode.SHIFT);

                            counter = counter + 10;
                        }
                        else
                        {
                            A_Tools.InputSimulator.IS_Mouse.LeftClick((int)Increase.Left, (int)Increase.Top, (int)Increase.Right, (int)Increase.Bottom);

                            counter = counter + 1;
                        }
                        Thread.Sleep(FixSleep);
                    }

                    break;
                case 3:
                    Control = A_Enums.UIElements.ParagonPointSelect_SpentPoint_Bonus3;



                    while (isParagonWindowVisible() && counter < Value && A_Tools.T_D3UI.UIElement.isVisible(Control))
                    {


                        UIRect Increase = A_Tools.T_D3UI.UIElement.getRect(Control);

                        if (Value - counter >= 10)
                        {
                            InputSimulator.SimulateKeyDown(VirtualKeyCode.SHIFT);

                            A_Tools.InputSimulator.IS_Mouse.LeftClick((int)Increase.Left, (int)Increase.Top, (int)Increase.Right, (int)Increase.Bottom);

                            InputSimulator.SimulateKeyUp(VirtualKeyCode.SHIFT);

                            counter = counter + 10;
                        }
                        else
                        {
                            A_Tools.InputSimulator.IS_Mouse.LeftClick((int)Increase.Left, (int)Increase.Top, (int)Increase.Right, (int)Increase.Bottom);

                            counter = counter + 1;
                        }
                        Thread.Sleep(FixSleep);
                    }

                    break;
            }

            return counter;

        }
        private static int Get_UnspentPoints_Core()
        {
            try
            {
                int UnspentPoints = int.Parse(UXHelper.GetControl<UXLabel>(A_Enums.UIElements.ParagonPointsAvailable_Core).xA20_Text_StructStart_Min84Bytes);

                return UnspentPoints;
            }
            catch { return 0; }
        }
        private static int Get_UnspentPoints_Offense()
        {
            try
            {
                return int.Parse(UXHelper.GetControl<UXLabel>(A_Enums.UIElements.ParagonPointsAvailable_Offense).xA20_Text_StructStart_Min84Bytes);
            }
            catch { return 0; }
        }
        private static int Get_UnspentPoints_Defense()
        {
            try
            {
                return int.Parse(UXHelper.GetControl<UXLabel>(A_Enums.UIElements.ParagonPointsAvailable_Defense).xA20_Text_StructStart_Min84Bytes);
            }
            catch { return 0; }
        }
        private static int Get_UnspentPoints_Utility()
        {
            try
            {
                return int.Parse(UXHelper.GetControl<UXLabel>(A_Enums.UIElements.ParagonPointsAvailable_Utility).xA20_Text_StructStart_Min84Bytes);
            }
            catch { return 0; }
        }
        private static int Get_TotalPoints_CurrentTab()
        {
            try
            {
                string RawText = UXHelper.GetControl<UXLabel>("Root.NormalLayer.Paragon_main.LayoutRoot.ParagonPointSelect.TotalPoints").xA20_Text_StructStart_Min84Bytes;

                int PointsTotal = int.Parse(Regex.Match(RawText, @"\d+").Groups[0].Value);

                return PointsTotal;
            }
            catch { return 0; }
        }
        private static bool isParagonWindowVisible()
        {
            try
            {
                return A_Tools.T_D3UI.UIElement.isVisible(A_Enums.UIElements.ParagonPointSelect);
            }
            catch { return false; }
        }
    }
}
