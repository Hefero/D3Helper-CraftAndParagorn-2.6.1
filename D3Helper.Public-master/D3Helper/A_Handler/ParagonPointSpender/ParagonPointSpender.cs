﻿using System;
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
            //var me = A_Collector. ;
            var playerParagorn = A_Collection.Me.HeroGlobals.Alt_Lvl;

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

                    bool ResetNeeded = false;

                    foreach (var paragonpoint in ParagonPoints.BonusPoints)
                    {
                        tab = OpenTab_byType(paragonpoint.Type);
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

                                try
                                {
                                    var core0number = 0;
                                    try { core0number = corepoints.Where(c => c.Type == BonusPoints.core0).FirstOrDefault().Value; }
                                    catch { }
                                    var core1number = 0;
                                    try { core1number = corepoints.Where(c => c.Type == BonusPoints.core1).FirstOrDefault().Value; }
                                    catch { }
                                    var core2number = 0;
                                    try { core2number = corepoints.Where(c => c.Type == BonusPoints.core2).FirstOrDefault().Value >= 0 ? corepoints.Where(c => c.Type == BonusPoints.core2).First().Value : 50; }
                                    catch { }
                                    var core3number = 0;
                                    try { core3number = corepoints.Where(c => c.Type == BonusPoints.core3).FirstOrDefault().Value >= 0 ? corepoints.Where(c => c.Type == BonusPoints.core3).First().Value : 50; }
                                    catch { }

                                    int[] alrdySetPoints = new int[] { GetSpentPoints_OnGivenBonus(0), GetSpentPoints_OnGivenBonus(1), GetSpentPoints_OnGivenBonus(2), GetSpentPoints_OnGivenBonus(3) };
                                    int[] neededPoints = new int[] {core0number, core1number, core2number, core3number};

                                    if (neededPoints[0] < 0)
                                    {   
                                        neededPoints[0] = playerParagorn - neededPoints[1] - neededPoints[2] - neededPoints[3] - 600;
                                    }
                                    if (neededPoints[1] < 0)
                                    {
                                        neededPoints[1] = playerParagorn - neededPoints[0] - neededPoints[2] - neededPoints[3] - 600;
                                    }

                                    if (neededPoints[0] != alrdySetPoints[0])
                                    {
                                        ResetNeeded = true;
                                    }
                                    if (neededPoints[1] != alrdySetPoints[1])
                                    {
                                        ResetNeeded = true;
                                    }
                                    if (neededPoints[2] != alrdySetPoints[2])
                                    {
                                        ResetNeeded = true;
                                    }
                                    if (neededPoints[3] != alrdySetPoints[3])
                                    {
                                        ResetNeeded = true;
                                    }
                                    CheckedCore = true;
                                }
                                catch { }
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
                                try
                                {
                                    var offensepoints0number = 0;
                                    try { offensepoints0number = offensepoints.Where(c => c.Type == BonusPoints.offense0).FirstOrDefault().Value; }
                                    catch { }
                                    var offensepoints1number = 0;
                                    try { offensepoints1number = offensepoints.Where(c => c.Type == BonusPoints.offense1).FirstOrDefault().Value; }
                                    catch { }
                                    var offensepoints2number = 0;
                                    try { offensepoints2number = offensepoints.Where(c => c.Type == BonusPoints.offense2).FirstOrDefault().Value >= 0 ? offensepoints.Where(c => c.Type == BonusPoints.offense2).First().Value : 50; }
                                    catch { }
                                    var offensepoints3number = 0;
                                    try { offensepoints3number = offensepoints.Where(c => c.Type == BonusPoints.offense3).FirstOrDefault().Value >= 0 ? offensepoints.Where(c => c.Type == BonusPoints.offense3).First().Value : 50; }
                                    catch { }

                                    int[] alrdySetPoints = new int[] { GetSpentPoints_OnGivenBonus(0), GetSpentPoints_OnGivenBonus(1), GetSpentPoints_OnGivenBonus(2), GetSpentPoints_OnGivenBonus(3) };
                                    int[] neededPoints = new int[] { offensepoints0number, offensepoints1number, offensepoints2number, offensepoints3number };

                                    if (neededPoints[0] < 0)
                                    {
                                        neededPoints[0] = 50;
                                    }
                                    if (neededPoints[1] < 0)
                                    {
                                        neededPoints[1] = 50;
                                    }

                                    if (neededPoints[0] != alrdySetPoints[0])
                                    {
                                        ResetNeeded = true;
                                    }
                                    if (neededPoints[1] != alrdySetPoints[1])
                                    {
                                        ResetNeeded = true;
                                    }
                                    if (neededPoints[2] != alrdySetPoints[2])
                                    {
                                        ResetNeeded = true;
                                    }
                                    if (neededPoints[3] != alrdySetPoints[3])
                                    {
                                        ResetNeeded = true;
                                    }
                                    CheckedOffense = true;
                                }
                                catch { }

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

                                try
                                {
                                    var defensepoints0number = 0;
                                    try { defensepoints0number = defensepoints.Where(c => c.Type == BonusPoints.defense0).FirstOrDefault().Value; }
                                    catch { }
                                    var defensepoints1number = 0;
                                    try { defensepoints1number = defensepoints.Where(c => c.Type == BonusPoints.defense1).FirstOrDefault().Value; }
                                    catch { }
                                    var defensepoints2number = 0;
                                    try { defensepoints2number = defensepoints.Where(c => c.Type == BonusPoints.defense2).FirstOrDefault().Value >= 0 ? defensepoints.Where(c => c.Type == BonusPoints.defense2).First().Value : 50; }
                                    catch { }
                                    var defensepoints3number = 0;
                                    try { defensepoints3number = defensepoints.Where(c => c.Type == BonusPoints.defense3).FirstOrDefault().Value >= 0 ? defensepoints.Where(c => c.Type == BonusPoints.defense3).First().Value : 50; }
                                    catch { }

                                    int[] alrdySetPoints = new int[] { GetSpentPoints_OnGivenBonus(0), GetSpentPoints_OnGivenBonus(1), GetSpentPoints_OnGivenBonus(2), GetSpentPoints_OnGivenBonus(3) };
                                    int[] neededPoints = new int[] { defensepoints0number, defensepoints1number, defensepoints2number, defensepoints3number };

                                    if (neededPoints[0] < 0)
                                    {
                                        neededPoints[0] = 50;
                                    }
                                    if (neededPoints[1] < 0)
                                    {
                                        neededPoints[1] = 50;
                                    }

                                    if (neededPoints[0] != alrdySetPoints[0])
                                    {
                                        ResetNeeded = true;
                                    }
                                    if (neededPoints[1] != alrdySetPoints[1])
                                    {
                                        ResetNeeded = true;
                                    }
                                    if (neededPoints[2] != alrdySetPoints[2])
                                    {
                                        ResetNeeded = true;
                                    }
                                    if (neededPoints[3] != alrdySetPoints[3])
                                    {
                                        ResetNeeded = true;
                                    }
                                    CheckedDefense = true;
                                }
                                catch { }
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

                                try
                                {
                                    var utilitypoints0number = 0;
                                    try { utilitypoints0number = utilitypoints.Where(c => c.Type == BonusPoints.utility0).FirstOrDefault().Value; }
                                    catch { }
                                    var utilitypoints1number = 0;
                                    try { utilitypoints1number = utilitypoints.Where(c => c.Type == BonusPoints.utility1).FirstOrDefault().Value; }
                                    catch { }
                                    var utilitypoints2number = 0;
                                    try { utilitypoints2number = utilitypoints.Where(c => c.Type == BonusPoints.utility2).FirstOrDefault().Value >= 0 ? utilitypoints.Where(c => c.Type == BonusPoints.utility2).First().Value : 50; }
                                    catch { }
                                    var utilitypoints3number = 0;
                                    try { utilitypoints3number = utilitypoints.Where(c => c.Type == BonusPoints.utility3).FirstOrDefault().Value >= 0 ? utilitypoints.Where(c => c.Type == BonusPoints.utility3).First().Value : 50; }
                                    catch { }

                                    int[] alrdySetPoints = new int[] { GetSpentPoints_OnGivenBonus(0), GetSpentPoints_OnGivenBonus(1), GetSpentPoints_OnGivenBonus(2), GetSpentPoints_OnGivenBonus(3) };
                                    int[] neededPoints = new int[] { utilitypoints3number, utilitypoints3number, utilitypoints3number, utilitypoints3number };

                                    if (neededPoints[0] < 0)
                                    {
                                        neededPoints[0] = 50;
                                    }
                                    if (neededPoints[1] < 0)
                                    {
                                        neededPoints[1] = 50;
                                    }

                                    if (neededPoints[0] != alrdySetPoints[0])
                                    {
                                        ResetNeeded = true;
                                    }
                                    if (neededPoints[1] != alrdySetPoints[1])
                                    {
                                        ResetNeeded = true;
                                    }
                                    if (neededPoints[2] != alrdySetPoints[2])
                                    {
                                        ResetNeeded = true;
                                    }
                                    if (neededPoints[3] != alrdySetPoints[3])
                                    {
                                        ResetNeeded = true;
                                    }
                                    CheckedUtility = true;
                                }
                                catch { }
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
                            spentpoints += SpendPoints_byType(paragonpoint.Type, paragonpoint.Value);
                        }
                        
                    }

                    if (spentpoints > 0)
                        Accept_ParagonPoints();


                    if (isParagonWindowVisible())
                        CloseParagonWindow();                    

                    int _x = A_Collection.D3Client.Window.D3ClientRect.Width / 2;
                    int _y = A_Collection.D3Client.Window.D3ClientRect.Height / 2;
                    Cursor.Position = new Point((int)_x, (int)_y);

                    Thread.Sleep(1000);
                }
            }

            A_Collection.Me.ParagonPointSpender.Is_SpendingPoints = false;

        }

        private static bool ResetNeededFunction(BonusPoint bonuspoint)
        {
            try
            {
                switch (bonuspoint.Type)
                {
                    case BonusPoints.core0:
                        if (bonuspoint.Value != -1)
                        {
                            var curValue = GetSpentPoints_OnGivenBonus(0);
                            if (curValue > bonuspoint.Value || (curValue < bonuspoint.Value && Get_UnspentPoints_Core() == 0))
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
            if (Value > 0)
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
                        spentpoints += Spent_Points(0, GetValue_toSpend(Type, Value), 0);
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
                return (int)msleft * 2;
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
                    InputSimulator.SimulateKeyUp(VirtualKeyCode.SHIFT);
                    InputSimulator.SimulateKeyUp(VirtualKeyCode.CONTROL);
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
                    InputSimulator.SimulateKeyUp(VirtualKeyCode.SHIFT);
                    InputSimulator.SimulateKeyUp(VirtualKeyCode.CONTROL);
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
                        if (Value - counter >= 100)
                        {
                            InputSimulator.SimulateKeyDown(VirtualKeyCode.CONTROL);
                            Thread.Sleep(FixSleep);
                            A_Tools.InputSimulator.IS_Mouse.LeftClick((int)Increase.Left, (int)Increase.Top, (int)Increase.Right, (int)Increase.Bottom);
                            counter = counter + 100;
                        }
                        else
                        {
                            if (Value - counter >= 10)
                            {
                                InputSimulator.SimulateKeyUp(VirtualKeyCode.CONTROL);
                                InputSimulator.SimulateKeyDown(VirtualKeyCode.SHIFT);
                                Thread.Sleep(FixSleep);
                                A_Tools.InputSimulator.IS_Mouse.LeftClick((int)Increase.Left, (int)Increase.Top, (int)Increase.Right, (int)Increase.Bottom);
                                counter = counter + 10;
                            }
                            else
                            {
                                InputSimulator.SimulateKeyUp(VirtualKeyCode.SHIFT);
                                InputSimulator.SimulateKeyUp(VirtualKeyCode.CONTROL);
                                Thread.Sleep(FixSleep);
                                A_Tools.InputSimulator.IS_Mouse.LeftClick((int)Increase.Left, (int)Increase.Top, (int)Increase.Right, (int)Increase.Bottom);
                                counter = counter + 1;
                            }
                        }
                    }
                    InputSimulator.SimulateKeyUp(VirtualKeyCode.CONTROL);
                    break;
                case 1:
                    Control = A_Enums.UIElements.ParagonPointSelect_SpentPoint_Bonus1;
                    while (isParagonWindowVisible() && counter < Value && A_Tools.T_D3UI.UIElement.isVisible(Control))
                    {
                        UIRect Increase = A_Tools.T_D3UI.UIElement.getRect(Control);
                        if (Value - counter >= 100)
                        {
                            InputSimulator.SimulateKeyDown(VirtualKeyCode.CONTROL);
                            Thread.Sleep(FixSleep);
                            A_Tools.InputSimulator.IS_Mouse.LeftClick((int)Increase.Left, (int)Increase.Top, (int)Increase.Right, (int)Increase.Bottom);
                            counter = counter + 100;
                        }
                        else
                        {
                            if (Value - counter >= 10)
                            {
                                InputSimulator.SimulateKeyUp(VirtualKeyCode.CONTROL);
                                InputSimulator.SimulateKeyDown(VirtualKeyCode.SHIFT);
                                Thread.Sleep(FixSleep);
                                A_Tools.InputSimulator.IS_Mouse.LeftClick((int)Increase.Left, (int)Increase.Top, (int)Increase.Right, (int)Increase.Bottom);
                                counter = counter + 10;
                            }
                            else
                            {
                                InputSimulator.SimulateKeyUp(VirtualKeyCode.SHIFT);
                                InputSimulator.SimulateKeyUp(VirtualKeyCode.CONTROL);
                                Thread.Sleep(FixSleep);
                                A_Tools.InputSimulator.IS_Mouse.LeftClick((int)Increase.Left, (int)Increase.Top, (int)Increase.Right, (int)Increase.Bottom);
                                counter = counter + 1;
                            }
                        }
                    }
                    InputSimulator.SimulateKeyUp(VirtualKeyCode.CONTROL);
                    break;
                case 2:
                    Control = A_Enums.UIElements.ParagonPointSelect_SpentPoint_Bonus2;
                    while (isParagonWindowVisible() && counter < Value && A_Tools.T_D3UI.UIElement.isVisible(Control))
                    {
                        UIRect Increase = A_Tools.T_D3UI.UIElement.getRect(Control);
                        if (Value - counter >= 100)
                        {
                            InputSimulator.SimulateKeyDown(VirtualKeyCode.CONTROL);
                            Thread.Sleep(FixSleep);
                            A_Tools.InputSimulator.IS_Mouse.LeftClick((int)Increase.Left, (int)Increase.Top, (int)Increase.Right, (int)Increase.Bottom);
                            counter = counter + 100;
                        }
                        else
                        {
                            if (Value - counter >= 10)
                            {
                                InputSimulator.SimulateKeyUp(VirtualKeyCode.CONTROL);
                                InputSimulator.SimulateKeyDown(VirtualKeyCode.SHIFT);
                                Thread.Sleep(FixSleep);
                                A_Tools.InputSimulator.IS_Mouse.LeftClick((int)Increase.Left, (int)Increase.Top, (int)Increase.Right, (int)Increase.Bottom);
                                counter = counter + 10;
                            }
                            else
                            {
                                InputSimulator.SimulateKeyUp(VirtualKeyCode.SHIFT);
                                InputSimulator.SimulateKeyUp(VirtualKeyCode.CONTROL);
                                Thread.Sleep(FixSleep);
                                A_Tools.InputSimulator.IS_Mouse.LeftClick((int)Increase.Left, (int)Increase.Top, (int)Increase.Right, (int)Increase.Bottom);
                                counter = counter + 1;
                            }
                        }
                    }
                    InputSimulator.SimulateKeyUp(VirtualKeyCode.SHIFT);
                    InputSimulator.SimulateKeyUp(VirtualKeyCode.CONTROL);
                    break;
                case 3:
                    Control = A_Enums.UIElements.ParagonPointSelect_SpentPoint_Bonus3;
                    while (isParagonWindowVisible() && counter < Value && A_Tools.T_D3UI.UIElement.isVisible(Control))
                        while (isParagonWindowVisible() && counter < Value && A_Tools.T_D3UI.UIElement.isVisible(Control))
                        {
                            UIRect Increase = A_Tools.T_D3UI.UIElement.getRect(Control);
                            if (Value - counter >= 100)
                            {
                                InputSimulator.SimulateKeyDown(VirtualKeyCode.CONTROL);
                                Thread.Sleep(FixSleep);
                                A_Tools.InputSimulator.IS_Mouse.LeftClick((int)Increase.Left, (int)Increase.Top, (int)Increase.Right, (int)Increase.Bottom);
                                counter = counter + 100;
                            }
                            else
                            {
                                if (Value - counter >= 10)
                                {
                                    InputSimulator.SimulateKeyUp(VirtualKeyCode.CONTROL);
                                    InputSimulator.SimulateKeyDown(VirtualKeyCode.SHIFT);
                                    Thread.Sleep(FixSleep);
                                    A_Tools.InputSimulator.IS_Mouse.LeftClick((int)Increase.Left, (int)Increase.Top, (int)Increase.Right, (int)Increase.Bottom);
                                    counter = counter + 10;
                                }
                                else
                                {
                                    InputSimulator.SimulateKeyUp(VirtualKeyCode.SHIFT);
                                    InputSimulator.SimulateKeyUp(VirtualKeyCode.CONTROL);
                                    Thread.Sleep(FixSleep);
                                    A_Tools.InputSimulator.IS_Mouse.LeftClick((int)Increase.Left, (int)Increase.Top, (int)Increase.Right, (int)Increase.Bottom);
                                    counter = counter + 1;
                                }
                            }
                        }
                    InputSimulator.SimulateKeyUp(VirtualKeyCode.SHIFT);
                    InputSimulator.SimulateKeyUp(VirtualKeyCode.CONTROL);
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
