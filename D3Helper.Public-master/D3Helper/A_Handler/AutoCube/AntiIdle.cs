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
    class AntiIdle
    {
        public bool enabled { get { return Properties.Settings.Default.AntiIdleBool; } }
        public int timer_ms { get { return Properties.Settings.Default.AntiIdle_Timer * 1000; } }        
        public bool isInTown { get { return A_Collection.Me.HeroStates.isInTown;  } }
        public bool isInGame { get { return A_Collection.Me.HeroStates.isInGame; } }
        public bool isAlive { get { return A_Collection.Me.HeroStates.isAlive; } }
        public bool isTeleporting { get { return A_Collection.Me.HeroStates.isTeleporting; } }
        public bool isUrshiNearby { get { ActorCommonData UrshiActor;  return Tools.IsUrshiNearby(out UrshiActor); } }
        public ActorCommonData LocalACD { get { return A_Collection.Me.HeroGlobals.LocalACD; } }
        public double posX { get { return LocalACD.x0D0_WorldPosX; } }
        public double posY { get { return LocalACD.x0D4_WorldPosY; } }
        public double posZ { get { return LocalACD.x0D8_WorldPosZ; } }


        public AntiIdle()
        {
            while (true)
            {
                if (enabled & !isInTown & isInGame)
                {
                    double lastPosXroof = posX * 1.02;
                    double lastPosXfloor = posX * 0.98;
                    double lastPosYroof = posY * 1.02;
                    double lastPosYfloor = posY * 0.98;
                    double lastPosZroof = posZ * 1.02;
                    double lastPosZfloor = posZ * 0.98;

                    bool moved = false;

                    Stopwatch threadTimer = new Stopwatch();
                    threadTimer.Start();
                    while (threadTimer.ElapsedMilliseconds < timer_ms)
                    {
                        Thread.Sleep(1000);
                        if (posX < lastPosXfloor | posX > lastPosXroof)
                        {
                            moved = true;
                            break;
                        }
                        if (posY < lastPosYfloor | posY > lastPosYroof)
                        {
                            moved = true;
                            break;
                        }
                        if (posZ < lastPosZfloor | posZ > lastPosZroof)
                        {
                            moved = true;
                            break;
                        }
                    }
                    threadTimer.Reset();
                    if (!moved & isInGame & !isInTown & !isTeleporting & isAlive & !isUrshiNearby)
                    {
                        uint middleScreenX = (uint)A_Collection.D3Client.Window.D3ClientRect.Width / 2;
                        uint middleScreenY = (uint)A_Collection.D3Client.Window.D3ClientRect.Height / 2;
                        uint offsetX = (uint)(A_Collection.D3Client.Window.D3ClientRect.Width * 0.12);
                        uint offsetY = (uint)(A_Collection.D3Client.Window.D3ClientRect.Height * 0.12);
                        A_Tools.InputSimulator.IS_Mouse.MoveCursor(middleScreenX, middleScreenY - offsetY); //up
                        A_Tools.InputSimulator.IS_Keyboard.execute_ForceMove();
                        Thread.Sleep(100);
                        A_Tools.InputSimulator.IS_Mouse.MoveCursor(middleScreenX + offsetX, middleScreenY - offsetY); //right up corner
                        A_Tools.InputSimulator.IS_Keyboard.execute_ForceMove();
                        Thread.Sleep(100);
                        A_Tools.InputSimulator.IS_Mouse.MoveCursor(middleScreenX + offsetX, middleScreenY); //right
                        A_Tools.InputSimulator.IS_Keyboard.execute_ForceMove();
                        Thread.Sleep(100);
                        A_Tools.InputSimulator.IS_Mouse.MoveCursor(middleScreenX + offsetX, middleScreenY + offsetY);//right down corner
                        A_Tools.InputSimulator.IS_Keyboard.execute_ForceMove();
                        Thread.Sleep(100);
                        A_Tools.InputSimulator.IS_Mouse.MoveCursor(middleScreenX, middleScreenY + offsetY);//down
                        A_Tools.InputSimulator.IS_Keyboard.execute_ForceMove();
                        Thread.Sleep(100);
                        A_Tools.InputSimulator.IS_Mouse.MoveCursor(middleScreenX - offsetX, middleScreenY + offsetY);//down left corner
                        A_Tools.InputSimulator.IS_Keyboard.execute_ForceMove();
                        Thread.Sleep(100);
                        A_Tools.InputSimulator.IS_Mouse.MoveCursor(middleScreenX - offsetX, middleScreenY);//left
                        A_Tools.InputSimulator.IS_Keyboard.execute_ForceMove();
                        Thread.Sleep(100);
                        A_Tools.InputSimulator.IS_Mouse.MoveCursor(middleScreenX - offsetY, middleScreenY - offsetY);//up left corner
                        A_Tools.InputSimulator.IS_Keyboard.execute_ForceMove();
                    }
                }
            }
        }
    }
}
