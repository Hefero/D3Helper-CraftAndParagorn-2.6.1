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
using D3Helper.A_Collector;
using SlimDX.DirectInput;

namespace D3Helper.A_Handler.AutoCube
{
    class RosBotUpgradeKadala
    {
        public bool kadalaVisible { get { return Tools.IsKadalaPage_Visible(); } }
        public bool visitedKadala { get; set; }
        public bool closedKadala { get; set; }
        public bool checkKadala { get; set; }
        public bool enabled { get { return Properties.Settings.Default.RosBotUpgradeKadalaBool; } }
        public bool kanaiVisible { get { return Tools.IsKanaisCube_MainPage_Visible(); } }        
        public bool isInTown { get { return A_Collection.Me.HeroStates.isInTown;  } }
        public bool isInGame { get { return A_Collection.Me.HeroStates.isInGame; } }

        public RosBotUpgradeKadala()
        {
            visitedKadala = false;
            closedKadala = false;
            checkKadala = true;
            while (true)
            {
                int sleepWait = 200;
                if (!visitedKadala)
                {
                    sleepWait = 800;
                }
                else
                {
                    sleepWait = 80;
                }
                if (!isInTown)
                {
                    sleepWait = 6000;
                }
                checkUpgradeKadala();
                Thread.Sleep(sleepWait);
            }
        }

        public void checkUpgradeKadala()
        {
            if (enabled)
            {
                if (isInTown & isInGame)
                {
                    if (checkKadala & kadalaVisible)
                    {
                        visitedKadala = true;
                        closedKadala = false;
                        checkKadala = false;
                    }
                    if (visitedKadala & !kadalaVisible)
                    {
                        visitedKadala = false;
                        closedKadala = true;
                    }
                    if (closedKadala)
                    {
                        visitedKadala = false;
                        closedKadala = false;
                        checkKadala = true;
                        WindowsInput.InputSimulator.SimulateKeyPress(WindowsInput.VirtualKeyCode.F6);
                        Thread.Sleep(400);
                        A_Tools.InputSimulator.IS_Keyboard.Close_AllWindows();
                        Thread.Sleep(300);
                        A_Handler.AutoCube.UpgradeRare.DoUpgrade();
                    }
                }
            }
        }
    }
}
