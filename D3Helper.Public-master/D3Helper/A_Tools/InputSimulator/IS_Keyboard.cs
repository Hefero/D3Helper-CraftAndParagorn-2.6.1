using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

using Enigma.D3.Enums;

using WindowsInput;
using D3Helper.A_Enums;
using D3Helper.A_Handler.Log;
using Exception = System.Exception;


namespace D3Helper.A_Tools.InputSimulator
{
    class IS_Keyboard
    {
        public class ChannelDuration
        {
            public ChannelDuration(DateTime start, DateTime stop, int ticks)
            {
                this.Start = start;
                this.Stop = stop;
                this.Ticks = ticks;
            }
            public DateTime Start { get; set; }
            public DateTime Stop { get; set; }
            public int Ticks { get; set; }
        }

        private static DateTime _lastKey = DateTime.Now;
        private const int _safeSleep = 10; // msec
        public static Dictionary<VirtualKeyCode, ChannelDuration> KeyChannelCache = new Dictionary<VirtualKeyCode, ChannelDuration>(); 

        public static void PressKey(A_Enums.ActionBarSlot Hotkey, bool ForceMoveCancel = false)
        {
            if (DateTime.Now <= _lastKey.AddMilliseconds(_safeSleep))
                return;

            //-- log action
            //if (Properties.Settings.Default.Logger_extendedLog)
            //    lock (A_Handler.Log.Exception.HandlerLog)
            //        A_Handler.Log.Exception.HandlerLog.Add(new LogEntry(DateTime.Now,
            //            "Initiating KeyPress for " + Hotkey));

            

            if (Hotkey == A_Enums.ActionBarSlot.RMB)
            {
                IS_Mouse.RightCLick();

                if (ForceMoveCancel)
                    execute_ForceMove();

                _lastKey = DateTime.Now;
                return;

            }

            if (Hotkey == A_Enums.ActionBarSlot.LMB)
            {
                if (!ForceMoveCancel)
                    execute_StandStill_LeftClick();

                else
                    IS_Mouse.LeftClick();

                if (ForceMoveCancel)
                    execute_ForceMove();

                _lastKey = DateTime.Now;
                return;

            }


            switch (Hotkey)
            {
                case A_Enums.ActionBarSlot.Slot1:
                    if (A_Collection.Preferences.Hotkeys.ModKey1_ActionBarSkill1 != null)
                        PressModKey(A_Collection.Preferences.Hotkeys.ModKey1_ActionBarSkill1.Value);
                    else if (A_Collection.Preferences.Hotkeys.ModKey2_ActionBarSkill1 != null)
                        PressModKey(A_Collection.Preferences.Hotkeys.ModKey2_ActionBarSkill1.Value);

                    if (A_Collection.Preferences.Hotkeys.Key1_ActionBarSkill1 != Key.Undefined)
                        PressKey(convert_KeyToVirtualKeyCode(A_Collection.Preferences.Hotkeys.Key1_ActionBarSkill1));
                    else if (A_Collection.Preferences.Hotkeys.Key2_ActionBarSkill1 != Key.Undefined)
                        PressKey(convert_KeyToVirtualKeyCode(A_Collection.Preferences.Hotkeys.Key2_ActionBarSkill1));

                    if (A_Collection.Preferences.Hotkeys.ModKey1_ActionBarSkill1 != null)
                        ReleaseModKey(A_Collection.Preferences.Hotkeys.ModKey1_ActionBarSkill1.Value);
                    else if (A_Collection.Preferences.Hotkeys.ModKey2_ActionBarSkill1 != null)
                        ReleaseModKey(A_Collection.Preferences.Hotkeys.ModKey2_ActionBarSkill1.Value);

                    if (ForceMoveCancel)
                        execute_ForceMove();

                    _lastKey = DateTime.Now;
                    return;

                case A_Enums.ActionBarSlot.Slot2:
                    if (A_Collection.Preferences.Hotkeys.ModKey1_ActionBarSkill2 != null)
                        PressModKey(A_Collection.Preferences.Hotkeys.ModKey1_ActionBarSkill2.Value);
                    else if (A_Collection.Preferences.Hotkeys.ModKey2_ActionBarSkill2 != null)
                        PressModKey(A_Collection.Preferences.Hotkeys.ModKey2_ActionBarSkill2.Value);

                    if (A_Collection.Preferences.Hotkeys.Key1_ActionBarSkill2 != Key.Undefined)
                        PressKey(convert_KeyToVirtualKeyCode(A_Collection.Preferences.Hotkeys.Key1_ActionBarSkill2));
                    else if (A_Collection.Preferences.Hotkeys.Key2_ActionBarSkill2 != Key.Undefined)
                        PressKey(convert_KeyToVirtualKeyCode(A_Collection.Preferences.Hotkeys.Key2_ActionBarSkill2));

                    if (A_Collection.Preferences.Hotkeys.ModKey1_ActionBarSkill2 != null)
                        ReleaseModKey(A_Collection.Preferences.Hotkeys.ModKey1_ActionBarSkill2.Value);
                    else if (A_Collection.Preferences.Hotkeys.ModKey2_ActionBarSkill2 != null)
                        ReleaseModKey(A_Collection.Preferences.Hotkeys.ModKey2_ActionBarSkill2.Value);

                    if (ForceMoveCancel)
                        execute_ForceMove();

                    _lastKey = DateTime.Now;
                    return;

                case A_Enums.ActionBarSlot.Slot3:
                    if (A_Collection.Preferences.Hotkeys.ModKey1_ActionBarSkill3 != null)
                        PressModKey(A_Collection.Preferences.Hotkeys.ModKey1_ActionBarSkill3.Value);
                    else if (A_Collection.Preferences.Hotkeys.ModKey2_ActionBarSkill3 != null)
                        PressModKey(A_Collection.Preferences.Hotkeys.ModKey2_ActionBarSkill3.Value);

                    if (A_Collection.Preferences.Hotkeys.Key1_ActionBarSkill3 != Key.Undefined)
                        PressKey(convert_KeyToVirtualKeyCode(A_Collection.Preferences.Hotkeys.Key1_ActionBarSkill3));
                    else if (A_Collection.Preferences.Hotkeys.Key2_ActionBarSkill3 != Key.Undefined)
                        PressKey(convert_KeyToVirtualKeyCode(A_Collection.Preferences.Hotkeys.Key2_ActionBarSkill3));

                    if (A_Collection.Preferences.Hotkeys.ModKey1_ActionBarSkill3 != null)
                        ReleaseModKey(A_Collection.Preferences.Hotkeys.ModKey1_ActionBarSkill3.Value);
                    else if (A_Collection.Preferences.Hotkeys.ModKey2_ActionBarSkill3 != null)
                        ReleaseModKey(A_Collection.Preferences.Hotkeys.ModKey2_ActionBarSkill3.Value);

                    if (ForceMoveCancel)
                        execute_ForceMove();

                    _lastKey = DateTime.Now;
                    return;

                case A_Enums.ActionBarSlot.Slot4:
                    if (A_Collection.Preferences.Hotkeys.ModKey1_ActionBarSkill4 != null)
                        PressModKey(A_Collection.Preferences.Hotkeys.ModKey1_ActionBarSkill4.Value);
                    else if (A_Collection.Preferences.Hotkeys.ModKey2_ActionBarSkill4 != null)
                        PressModKey(A_Collection.Preferences.Hotkeys.ModKey2_ActionBarSkill4.Value);

                    if (A_Collection.Preferences.Hotkeys.Key1_ActionBarSkill4 != Key.Undefined)
                        PressKey(convert_KeyToVirtualKeyCode(A_Collection.Preferences.Hotkeys.Key1_ActionBarSkill4));
                    else if (A_Collection.Preferences.Hotkeys.Key2_ActionBarSkill4 != Key.Undefined)
                        PressKey(convert_KeyToVirtualKeyCode(A_Collection.Preferences.Hotkeys.Key2_ActionBarSkill4));

                    if (A_Collection.Preferences.Hotkeys.ModKey1_ActionBarSkill4 != null)
                        ReleaseModKey(A_Collection.Preferences.Hotkeys.ModKey1_ActionBarSkill4.Value);
                    else if (A_Collection.Preferences.Hotkeys.ModKey2_ActionBarSkill4 != null)
                        ReleaseModKey(A_Collection.Preferences.Hotkeys.ModKey2_ActionBarSkill4.Value);

                    if (ForceMoveCancel)
                        execute_ForceMove();

                    _lastKey = DateTime.Now;
                    return;

                case A_Enums.ActionBarSlot.Potion:
                    if (A_Collection.Preferences.Hotkeys.ModKey1_Potion != null)
                        PressModKey(A_Collection.Preferences.Hotkeys.ModKey1_Potion.Value);
                    else if (A_Collection.Preferences.Hotkeys.ModKey2_Potion != null)
                        PressModKey(A_Collection.Preferences.Hotkeys.ModKey2_Potion.Value);

                    if (A_Collection.Preferences.Hotkeys.Key1_Potion != Key.Undefined)
                        PressKey(convert_KeyToVirtualKeyCode(A_Collection.Preferences.Hotkeys.Key1_Potion));
                    else if (A_Collection.Preferences.Hotkeys.Key2_Potion != Key.Undefined)
                        PressKey(convert_KeyToVirtualKeyCode(A_Collection.Preferences.Hotkeys.Key2_Potion));

                    if (A_Collection.Preferences.Hotkeys.ModKey1_Potion != null)
                        ReleaseModKey(A_Collection.Preferences.Hotkeys.ModKey1_Potion.Value);
                    else if (A_Collection.Preferences.Hotkeys.ModKey2_Potion != null)
                        ReleaseModKey(A_Collection.Preferences.Hotkeys.ModKey2_Potion.Value);

                    _lastKey = DateTime.Now;
                    return;
            }

        }

        public static void ChannelKey_Maintain()
        {
            List<VirtualKeyCode> RemoveBuffer = new List<VirtualKeyCode>();

            foreach (var channelKey in KeyChannelCache)
            {
                if (channelKey.Value.Stop > DateTime.Now)
                {
                    if (channelKey.Key == VirtualKeyCode.LBUTTON)
                    {
                        IS_Mouse.LeftDown(0, 0);
                        continue;
                    }

                    if (channelKey.Key == VirtualKeyCode.RBUTTON)
                    {
                        IS_Mouse.RightDown(0, 0);
                        continue;
                    }

                    WindowsInput.InputSimulator.SimulateKeyDown(channelKey.Key);
                    continue;
                }

                
                if (channelKey.Value.Stop <= DateTime.Now)
                {
                    RemoveBuffer.Add(channelKey.Key);

                    if (channelKey.Key == VirtualKeyCode.LBUTTON)
                    {
                        IS_Mouse.LeftUp(0, 0);
                        continue;
                    }

                    if (channelKey.Key == VirtualKeyCode.RBUTTON)
                    {
                        IS_Mouse.RightUp(0, 0);
                        continue;
                    }

                    WindowsInput.InputSimulator.SimulateKeyUp(channelKey.Key);
                    continue;
                }
                
            }

            foreach (var remove in RemoveBuffer)
            {
                KeyChannelCache.Remove(remove);
            }
        }

        public static void ChannelKey_Start(A_Enums.ActionBarSlot Hotkey, int ChannelTicks)
        {
            var AllKeys = getKeys_ActionBarSlot(Hotkey);

            if (ChannelTicks > 0)
                ChannelTicks = (int)((1000/60)*ChannelTicks);

            foreach (var Key in AllKeys)
                {
                    if (KeyChannelCache.ContainsKey(Key))
                    {
                        if (ChannelTicks == -1)
                            KeyChannelCache[Key].Stop = DateTime.Now.AddMilliseconds(1);
                        else
                        {
                        KeyChannelCache[Key].Stop = DateTime.Now.AddMilliseconds(ChannelTicks);
                    }
                    }

                    else
                    {
                        if (ChannelTicks == -1)
                            KeyChannelCache.Add(Key, new ChannelDuration(DateTime.Now, DateTime.Now.AddMilliseconds(1), 1));
                        else
                        {
                            KeyChannelCache.Add(Key,
                                new ChannelDuration(DateTime.Now, DateTime.Now.AddMilliseconds(ChannelTicks), ChannelTicks));
                        }

                    }
                }

            

        }

        public static void ChannelKey_StopAll()
        {
            List<VirtualKeyCode> RemoveBuffer = new List<VirtualKeyCode>();

            foreach (var Key in KeyChannelCache)
            {
                RemoveBuffer.Add(Key.Key);

                if (Key.Key == VirtualKeyCode.LBUTTON)
                {
                    IS_Mouse.LeftUp(0, 0);
                    continue;
                }

                if (Key.Key == VirtualKeyCode.RBUTTON)
                {
                    IS_Mouse.RightUp(0, 0);
                    continue;
                }

                WindowsInput.InputSimulator.SimulateKeyUp(Key.Key);
            }

            foreach (var remove in RemoveBuffer)
            {
                KeyChannelCache.Remove(remove);
            }

        }

        private static List<VirtualKeyCode> getKeys_ActionBarSlot(ActionBarSlot actionbarslot)
        {
            List<VirtualKeyCode> Buffer = new List<VirtualKeyCode>();

            switch (actionbarslot)
            {
                    case ActionBarSlot.LMB:
                    Buffer.Add(VirtualKeyCode.LBUTTON);
                    break;

                    case ActionBarSlot.RMB:
                    Buffer.Add(VirtualKeyCode.RBUTTON);
                    break;

                    case ActionBarSlot.Potion:
                    if (A_Collection.Preferences.Hotkeys.ModKey1_Potion != null)
                        Buffer.AddRange(getVirtualKeyCodes_byModkey(A_Collection.Preferences.Hotkeys.ModKey1_Potion.Value));
                    else if (A_Collection.Preferences.Hotkeys.ModKey2_Potion != null)
                        Buffer.AddRange(getVirtualKeyCodes_byModkey(A_Collection.Preferences.Hotkeys.ModKey2_Potion.Value));

                    if (A_Collection.Preferences.Hotkeys.Key1_Potion != Key.Undefined)
                        Buffer.Add(convert_KeyToVirtualKeyCode(A_Collection.Preferences.Hotkeys.Key1_Potion));
                    else if (A_Collection.Preferences.Hotkeys.Key2_Potion != Key.Undefined)
                        Buffer.Add(convert_KeyToVirtualKeyCode(A_Collection.Preferences.Hotkeys.Key2_Potion));
                    break;

                case ActionBarSlot.Slot1:
                    if (A_Collection.Preferences.Hotkeys.ModKey1_ActionBarSkill1 != null)
                        Buffer.AddRange(getVirtualKeyCodes_byModkey(A_Collection.Preferences.Hotkeys.ModKey1_ActionBarSkill1.Value));
                    else if (A_Collection.Preferences.Hotkeys.ModKey2_ActionBarSkill1 != null)
                        Buffer.AddRange(getVirtualKeyCodes_byModkey(A_Collection.Preferences.Hotkeys.ModKey2_ActionBarSkill1.Value));

                    if (A_Collection.Preferences.Hotkeys.Key1_ActionBarSkill1 != Key.Undefined)
                        Buffer.Add(convert_KeyToVirtualKeyCode(A_Collection.Preferences.Hotkeys.Key1_ActionBarSkill1));
                    else if (A_Collection.Preferences.Hotkeys.Key2_ActionBarSkill1 != Key.Undefined)
                        Buffer.Add(convert_KeyToVirtualKeyCode(A_Collection.Preferences.Hotkeys.Key2_ActionBarSkill1));
                    break;

                case ActionBarSlot.Slot2:
                    if (A_Collection.Preferences.Hotkeys.ModKey1_ActionBarSkill2 != null)
                        Buffer.AddRange(getVirtualKeyCodes_byModkey(A_Collection.Preferences.Hotkeys.ModKey1_ActionBarSkill2.Value));
                    else if (A_Collection.Preferences.Hotkeys.ModKey2_ActionBarSkill2 != null)
                        Buffer.AddRange(getVirtualKeyCodes_byModkey(A_Collection.Preferences.Hotkeys.ModKey2_ActionBarSkill2.Value));

                    if (A_Collection.Preferences.Hotkeys.Key1_ActionBarSkill2 != Key.Undefined)
                        Buffer.Add(convert_KeyToVirtualKeyCode(A_Collection.Preferences.Hotkeys.Key1_ActionBarSkill2));
                    else if (A_Collection.Preferences.Hotkeys.Key2_ActionBarSkill2 != Key.Undefined)
                        Buffer.Add(convert_KeyToVirtualKeyCode(A_Collection.Preferences.Hotkeys.Key2_ActionBarSkill2));
                    break;

                case ActionBarSlot.Slot3:
                    if (A_Collection.Preferences.Hotkeys.ModKey1_ActionBarSkill3 != null)
                        Buffer.AddRange(getVirtualKeyCodes_byModkey(A_Collection.Preferences.Hotkeys.ModKey1_ActionBarSkill3.Value));
                    else if (A_Collection.Preferences.Hotkeys.ModKey2_ActionBarSkill3 != null)
                        Buffer.AddRange(getVirtualKeyCodes_byModkey(A_Collection.Preferences.Hotkeys.ModKey2_ActionBarSkill3.Value));

                    if (A_Collection.Preferences.Hotkeys.Key1_ActionBarSkill3 != Key.Undefined)
                        Buffer.Add(convert_KeyToVirtualKeyCode(A_Collection.Preferences.Hotkeys.Key1_ActionBarSkill3));
                    else if (A_Collection.Preferences.Hotkeys.Key2_ActionBarSkill3 != Key.Undefined)
                        Buffer.Add(convert_KeyToVirtualKeyCode(A_Collection.Preferences.Hotkeys.Key2_ActionBarSkill3));
                    break;

                case ActionBarSlot.Slot4:
                    if (A_Collection.Preferences.Hotkeys.ModKey1_ActionBarSkill4 != null)
                        Buffer.AddRange(getVirtualKeyCodes_byModkey(A_Collection.Preferences.Hotkeys.ModKey1_ActionBarSkill4.Value));
                    else if (A_Collection.Preferences.Hotkeys.ModKey2_ActionBarSkill4 != null)
                        Buffer.AddRange(getVirtualKeyCodes_byModkey(A_Collection.Preferences.Hotkeys.ModKey2_ActionBarSkill4.Value));

                    if (A_Collection.Preferences.Hotkeys.Key1_ActionBarSkill4 != Key.Undefined)
                        Buffer.Add(convert_KeyToVirtualKeyCode(A_Collection.Preferences.Hotkeys.Key1_ActionBarSkill4));
                    else if (A_Collection.Preferences.Hotkeys.Key2_ActionBarSkill4 != Key.Undefined)
                        Buffer.Add(convert_KeyToVirtualKeyCode(A_Collection.Preferences.Hotkeys.Key2_ActionBarSkill4));
                    break;
            }

            return Buffer;
        }

        private static List<VirtualKeyCode> getVirtualKeyCodes_byModkey(ModifierKeys modkey)
        {
            List<VirtualKeyCode> Buffer = new List<VirtualKeyCode>();

			if (modkey.HasFlag(ModifierKeys.Alt))
				Buffer.Add(VirtualKeyCode.LMENU);
			if (modkey.HasFlag(ModifierKeys.Shift))
				Buffer.Add(VirtualKeyCode.LSHIFT);
			if (modkey.HasFlag(ModifierKeys.Ctrl))
				Buffer.Add(VirtualKeyCode.LCONTROL);

            return Buffer;
        } 

        public static void ParagonWindow()
        {
            //-- log action
            A_Handler.Log.LogEntry.addLogEntry("Initiating KeyPress for open up ParagonWindow");
            //

            if (A_Collection.Preferences.Hotkeys.ModKey1_OpenParagon != null)
                PressModKey(A_Collection.Preferences.Hotkeys.ModKey1_OpenParagon.Value);
            else if (A_Collection.Preferences.Hotkeys.ModKey2_OpenParagon != null)
                PressModKey(A_Collection.Preferences.Hotkeys.ModKey2_OpenParagon.Value);

            if (A_Collection.Preferences.Hotkeys.Key1_OpenParagon != Key.Undefined)
                PressKey(convert_KeyToVirtualKeyCode(A_Collection.Preferences.Hotkeys.Key1_OpenParagon));
            else if (A_Collection.Preferences.Hotkeys.Key2_OpenParagon != Key.Undefined)
                PressKey(convert_KeyToVirtualKeyCode(A_Collection.Preferences.Hotkeys.Key2_OpenParagon));

            if (A_Collection.Preferences.Hotkeys.ModKey1_OpenParagon != null)
                ReleaseModKey(A_Collection.Preferences.Hotkeys.ModKey1_OpenParagon.Value);
            else if (A_Collection.Preferences.Hotkeys.ModKey2_OpenParagon != null)
                ReleaseModKey(A_Collection.Preferences.Hotkeys.ModKey2_OpenParagon.Value);
        }

        public static void Close_AllWindows()
        {
            //-- log action
            A_Handler.Log.LogEntry.addLogEntry("Initiating KeyPress for closing all windows");
            //

            if (A_Collection.Preferences.Hotkeys.ModKey1_CloseAllWindows != null)
                PressModKey(A_Collection.Preferences.Hotkeys.ModKey1_CloseAllWindows.Value);
            else if (A_Collection.Preferences.Hotkeys.ModKey2_CloseAllWindows != null)
                PressModKey(A_Collection.Preferences.Hotkeys.ModKey2_CloseAllWindows.Value);

            if (A_Collection.Preferences.Hotkeys.Key1_CloseAllWindows != Key.Undefined)
                PressKey(convert_KeyToVirtualKeyCode(A_Collection.Preferences.Hotkeys.Key1_CloseAllWindows));
            else if (A_Collection.Preferences.Hotkeys.Key2_CloseAllWindows != Key.Undefined)
                PressKey(convert_KeyToVirtualKeyCode(A_Collection.Preferences.Hotkeys.Key2_CloseAllWindows));

            if (A_Collection.Preferences.Hotkeys.ModKey1_CloseAllWindows != null)
                ReleaseModKey(A_Collection.Preferences.Hotkeys.ModKey1_CloseAllWindows.Value);
            else if (A_Collection.Preferences.Hotkeys.ModKey2_CloseAllWindows != null)
                ReleaseModKey(A_Collection.Preferences.Hotkeys.ModKey2_CloseAllWindows.Value);
        }

        public static void SkillsWindow()
        {
            //-- log action
            A_Handler.Log.LogEntry.addLogEntry("Initiating KeyPress for open up SkillsWindow");
            //

            if (A_Collection.Preferences.Hotkeys.ModKey1_SkillsWindow != null)
                PressModKey(A_Collection.Preferences.Hotkeys.ModKey1_SkillsWindow.Value);
            else if (A_Collection.Preferences.Hotkeys.ModKey2_SkillsWindow != null)
                PressModKey(A_Collection.Preferences.Hotkeys.ModKey2_SkillsWindow.Value);

            if (A_Collection.Preferences.Hotkeys.Key1_SkillsWindow != Key.Undefined)
                PressKey(convert_KeyToVirtualKeyCode(A_Collection.Preferences.Hotkeys.Key1_SkillsWindow));
            else if (A_Collection.Preferences.Hotkeys.Key2_SkillsWindow != Key.Undefined)
                PressKey(convert_KeyToVirtualKeyCode(A_Collection.Preferences.Hotkeys.Key2_SkillsWindow));

            if (A_Collection.Preferences.Hotkeys.ModKey1_SkillsWindow != null)
                ReleaseModKey(A_Collection.Preferences.Hotkeys.ModKey1_SkillsWindow.Value);
            else if (A_Collection.Preferences.Hotkeys.ModKey2_SkillsWindow != null)
                ReleaseModKey(A_Collection.Preferences.Hotkeys.ModKey2_SkillsWindow.Value);
        }

        public static void Inventory()
        {
            //-- log action
            A_Handler.Log.LogEntry.addLogEntry("Initiating KeyPress for open up Inventory");
            //

            if (A_Collection.Preferences.Hotkeys.ModKey1_OpenInventory != null)
                PressModKey(A_Collection.Preferences.Hotkeys.ModKey1_OpenInventory.Value);
            else if (A_Collection.Preferences.Hotkeys.ModKey2_OpenInventory != null)
                PressModKey(A_Collection.Preferences.Hotkeys.ModKey2_OpenInventory.Value);

            if (A_Collection.Preferences.Hotkeys.Key1_OpenInventory != Key.Undefined)
                PressKey(convert_KeyToVirtualKeyCode(A_Collection.Preferences.Hotkeys.Key1_OpenInventory));
            else if (A_Collection.Preferences.Hotkeys.Key2_OpenInventory != Key.Undefined)
                PressKey(convert_KeyToVirtualKeyCode(A_Collection.Preferences.Hotkeys.Key2_OpenInventory));

            if (A_Collection.Preferences.Hotkeys.ModKey1_OpenInventory != null)
                ReleaseModKey(A_Collection.Preferences.Hotkeys.ModKey1_OpenInventory.Value);
            else if (A_Collection.Preferences.Hotkeys.ModKey2_OpenInventory != null)
                ReleaseModKey(A_Collection.Preferences.Hotkeys.ModKey2_OpenInventory.Value);
        }

        private static void execute_ForceMove()
        {
            if (A_Collection.Preferences.Hotkeys.ModKey1_ForceMove != null)
                PressModKey(A_Collection.Preferences.Hotkeys.ModKey1_ForceMove.Value);
            else if (A_Collection.Preferences.Hotkeys.ModKey2_ForceMove != null)
                PressModKey(A_Collection.Preferences.Hotkeys.ModKey2_ForceMove.Value);

            if (A_Collection.Preferences.Hotkeys.Key1_ForceMove != Key.Undefined)
                PressKey(convert_KeyToVirtualKeyCode(A_Collection.Preferences.Hotkeys.Key1_ForceMove));
            else if (A_Collection.Preferences.Hotkeys.Key2_ForceMove != Key.Undefined)
                PressKey(convert_KeyToVirtualKeyCode(A_Collection.Preferences.Hotkeys.Key2_ForceMove));

            if (A_Collection.Preferences.Hotkeys.ModKey1_ForceMove != null)
                ReleaseModKey(A_Collection.Preferences.Hotkeys.ModKey1_ForceMove.Value);
            else if (A_Collection.Preferences.Hotkeys.ModKey2_ForceMove != null)
                ReleaseModKey(A_Collection.Preferences.Hotkeys.ModKey2_ForceMove.Value);
        }

        private static void execute_StandStill_LeftClick()
        {
            if (A_Collection.Preferences.Hotkeys.ModKey1_ForceStandStill != null)
                PressModKey(A_Collection.Preferences.Hotkeys.ModKey1_ForceStandStill.Value);
            else if (A_Collection.Preferences.Hotkeys.ModKey2_ForceStandStill != null)
                PressModKey(A_Collection.Preferences.Hotkeys.ModKey2_ForceStandStill.Value);

            if (A_Collection.Preferences.Hotkeys.Key1_ForceStandStill != Key.Undefined)
            {
                WindowsInput.InputSimulator.SimulateKeyDown(
                    convert_KeyToVirtualKeyCode(A_Collection.Preferences.Hotkeys.Key1_ForceStandStill));

                //-- log action
                //A_Handler.Log.LogEntry.addLogEntry("Pressed Key(" + A_Collection.Preferences.Hotkeys.Key1_ForceStandStill + ")");
                //
            }
            else if (A_Collection.Preferences.Hotkeys.Key2_ForceStandStill != Key.Undefined)
            {
                WindowsInput.InputSimulator.SimulateKeyDown(
                    convert_KeyToVirtualKeyCode(A_Collection.Preferences.Hotkeys.Key2_ForceStandStill));

                //-- log action
                //A_Handler.Log.LogEntry.addLogEntry("Pressed Key(" + A_Collection.Preferences.Hotkeys.Key2_ForceStandStill + ")");
                //
            }

            IS_Mouse.LeftClick();

            if (A_Collection.Preferences.Hotkeys.ModKey1_ForceStandStill != null)
                ReleaseModKey(A_Collection.Preferences.Hotkeys.ModKey1_ForceStandStill.Value);
            else if (A_Collection.Preferences.Hotkeys.ModKey2_ForceStandStill != null)
                ReleaseModKey(A_Collection.Preferences.Hotkeys.ModKey2_ForceStandStill.Value);

            if (A_Collection.Preferences.Hotkeys.Key1_ForceStandStill != Key.Undefined)
            {
                WindowsInput.InputSimulator.SimulateKeyUp(
                    convert_KeyToVirtualKeyCode(A_Collection.Preferences.Hotkeys.Key1_ForceStandStill));
                // release Key1 for StandStill

                //-- log action
                A_Handler.Log.LogEntry.addLogEntry("Released Key(" + A_Collection.Preferences.Hotkeys.Key1_ForceStandStill + ")");
                //
            }
            else if (A_Collection.Preferences.Hotkeys.Key2_ForceStandStill != Key.Undefined)
            {
                WindowsInput.InputSimulator.SimulateKeyUp(
                    convert_KeyToVirtualKeyCode(A_Collection.Preferences.Hotkeys.Key2_ForceStandStill));

                //-- log action
                A_Handler.Log.LogEntry.addLogEntry("Released Key(" + A_Collection.Preferences.Hotkeys.Key2_ForceStandStill + ")");
                //
            }
        }

        private static void PressKey(VirtualKeyCode Key)
        {
            try
            {
                if (Key == VirtualKeyCode.ESCAPE)
                    return;

                WindowsInput.InputSimulator.SimulateKeyPress(Key);

                //-- log actions
                //A_Handler.Log.LogEntry.addLogEntry("Pressed Key(" + Key + ")");
                //
            }
            catch (Exception)
            {
                
            }
        }

        private static void PressModKey(ModifierKeys ModKey)
        {
            if (ModKey == ModifierKeys.None)
                return;

            //-- log action
            A_Handler.Log.LogEntry.addLogEntry("Pressed ModKey(" + ModKey + ")");
            //

            if (ModKey.HasFlag(ModifierKeys.Alt))
				WindowsInput.InputSimulator.SimulateKeyDown(VirtualKeyCode.MENU);
			if (ModKey.HasFlag(ModifierKeys.Shift))
				WindowsInput.InputSimulator.SimulateKeyDown(VirtualKeyCode.SHIFT);
			if (ModKey.HasFlag(ModifierKeys.Ctrl))
				WindowsInput.InputSimulator.SimulateKeyDown(VirtualKeyCode.CONTROL);
        }
        private static void ReleaseModKey(ModifierKeys ModKey)
        {
            if (ModKey == ModifierKeys.None)
                return;

            //-- log action
            A_Handler.Log.LogEntry.addLogEntry("Released ModKey(" + ModKey + ")");
            //

            if (ModKey.HasFlag(ModifierKeys.Alt))
				WindowsInput.InputSimulator.SimulateKeyUp(VirtualKeyCode.MENU);
			if (ModKey.HasFlag(ModifierKeys.Shift))
				WindowsInput.InputSimulator.SimulateKeyUp(VirtualKeyCode.SHIFT);
			if (ModKey.HasFlag(ModifierKeys.Ctrl))
				WindowsInput.InputSimulator.SimulateKeyUp(VirtualKeyCode.CONTROL);
        }
        public static VirtualKeyCode convert_KeyToVirtualKeyCode(Key InternalKey)
        {
            switch(InternalKey)
            {
                case Key.A:
                    return VirtualKeyCode.VK_A;

                case Key.ADD:
                    return VirtualKeyCode.ADD;

                case Key.APOSTROPHE:
                    return VirtualKeyCode.OEM_7;

                case Key.APPS:
                    return VirtualKeyCode.APPS;

                case Key.AT:
                    return VirtualKeyCode.ATTN;

                case Key.B:
                    return VirtualKeyCode.VK_B;

                case Key.BACK:
                    return VirtualKeyCode.BACK;

                case Key.BACKSLASH:
                    return VirtualKeyCode.OEM_102;

                case Key.C:
                    return VirtualKeyCode.VK_C;

                case Key.CAPITAL:
                    return VirtualKeyCode.CAPITAL;

                case Key.COLON:
                    return VirtualKeyCode.OEM_1;

                case Key.COMMA:
                    return VirtualKeyCode.OEM_COMMA;

                case Key.CONVERT:
                    return VirtualKeyCode.CONVERT;

                case Key.D:
                    return VirtualKeyCode.VK_D;

                case Key.D0:
                    return VirtualKeyCode.VK_0;

                case Key.D1:
                    return VirtualKeyCode.VK_1;

                case Key.D2:
                    return VirtualKeyCode.VK_2;

                case Key.D3:
                    return VirtualKeyCode.VK_3;

                case Key.D4:
                    return VirtualKeyCode.VK_4;

                case Key.D5:
                    return VirtualKeyCode.VK_5;

                case Key.D6:
                    return VirtualKeyCode.VK_6;

                case Key.D7:
                    return VirtualKeyCode.VK_7;

                case Key.D8:
                    return VirtualKeyCode.VK_8;

                case Key.D9:
                    return VirtualKeyCode.VK_9;

                case Key.DECIMAL:
                    return VirtualKeyCode.DECIMAL;

                case Key.DELETE:
                    return VirtualKeyCode.DELETE;

                case Key.DIVIDE:
                    return VirtualKeyCode.DIVIDE;

                case Key.DOWN:
                    return VirtualKeyCode.DOWN;

                case Key.E:
                    return VirtualKeyCode.VK_E;

                case Key.END:
                    return VirtualKeyCode.END;

                case Key.EQUALS:
                    return VirtualKeyCode.OEM_PLUS;

                case Key.ESCAPE:
                    return VirtualKeyCode.ESCAPE;

                case Key.F:
                    return VirtualKeyCode.VK_F;

                case Key.F1:
                    return VirtualKeyCode.F1;

                case Key.F10:
                    return VirtualKeyCode.F10;

                case Key.F11:
                    return VirtualKeyCode.F11;

                case Key.F12:
                    return VirtualKeyCode.F12;

                case Key.F13:
                    return VirtualKeyCode.F13;

                case Key.F14:
                    return VirtualKeyCode.F14;

                case Key.F15:
                    return VirtualKeyCode.F15;

                case Key.F2:
                    return VirtualKeyCode.F2;

                case Key.F3:
                    return VirtualKeyCode.F3;

                case Key.F4:
                    return VirtualKeyCode.F4;

                case Key.F5:
                    return VirtualKeyCode.F5;

                case Key.F6:
                    return VirtualKeyCode.F6;

                case Key.F7:
                    return VirtualKeyCode.F7;

                case Key.F8:
                    return VirtualKeyCode.F8;

                case Key.F9:
                    return VirtualKeyCode.F9;

                case Key.G:
                    return VirtualKeyCode.VK_G;

                case Key.H:
                    return VirtualKeyCode.VK_N;

                case Key.HOME:
                    return VirtualKeyCode.HOME;

                case Key.I:
                    return VirtualKeyCode.VK_I;

                case Key.INSERT:
                    return VirtualKeyCode.INSERT;

                case Key.J:
                    return VirtualKeyCode.VK_J;

                case Key.K:
                    return VirtualKeyCode.VK_K;

                case Key.KANA:
                    return VirtualKeyCode.KANA;

                case Key.KANJI:
                    return VirtualKeyCode.KANJI;

                case Key.L:
                    return VirtualKeyCode.VK_L;

                case Key.LBRACKET:
                    return VirtualKeyCode.OEM_4;

                case Key.LCONTROL:
                    return VirtualKeyCode.LCONTROL;

                case Key.LEFT:
                    return VirtualKeyCode.LEFT;

                case Key.LMENU:
                    return VirtualKeyCode.LMENU;

                case Key.LSHIFT:
                    return VirtualKeyCode.LSHIFT;

                case Key.LWIN:
                    return VirtualKeyCode.LWIN;

                case Key.M:
                    return VirtualKeyCode.VK_M;

                case Key.MAIL:
                    return VirtualKeyCode.LAUNCH_MAIL;

                case Key.MEDIASELECT:
                    return VirtualKeyCode.LAUNCH_MEDIA_SELECT;

                case Key.MEDIASTOP:
                    return VirtualKeyCode.MEDIA_STOP;

                case Key.MINUS:
                    return VirtualKeyCode.OEM_MINUS;

                case Key.MULTIPLY:
                    return VirtualKeyCode.MULTIPLY;

                case Key.MUTE:
                    return VirtualKeyCode.VOLUME_MUTE;

                case Key.N:
                    return VirtualKeyCode.VK_N;

                case Key.NEXT:
                    return VirtualKeyCode.NEXT;

                case Key.NEXTTRACK:
                    return VirtualKeyCode.MEDIA_NEXT_TRACK;

                case Key.NOCONVERT:
                    return VirtualKeyCode.NONCONVERT;

                case Key.NUMLOCK:
                    return VirtualKeyCode.NUMLOCK;

                case Key.NUMPAD0:
                    return VirtualKeyCode.NUMPAD0;

                case Key.NUMPAD1:
                    return VirtualKeyCode.NUMPAD1;

                case Key.NUMPAD2:
                    return VirtualKeyCode.NUMPAD2;

                case Key.NUMPAD3:
                    return VirtualKeyCode.NUMPAD3;

                case Key.NUMPAD4:
                    return VirtualKeyCode.NUMPAD4;

                case Key.NUMPAD5:
                    return VirtualKeyCode.NUMPAD5;

                case Key.NUMPAD6:
                    return VirtualKeyCode.NUMPAD6;

                case Key.NUMPAD7:
                    return VirtualKeyCode.NUMPAD7;

                case Key.NUMPAD8:
                    return VirtualKeyCode.NUMPAD8;

                case Key.NUMPAD9:
                    return VirtualKeyCode.NUMPAD9;

                case Key.O:
                    return VirtualKeyCode.VK_O;

                case Key.OEM_102:
                    return VirtualKeyCode.OEM_102;

                case Key.OEM_8:
                    return VirtualKeyCode.OEM_8;

                case Key.P:
                    return VirtualKeyCode.VK_P;

                case Key.PAUSE:
                    return VirtualKeyCode.PAUSE;

                case Key.PERIOD:
                    return VirtualKeyCode.OEM_PERIOD;

                case Key.PLAYPAUSE:
                    return VirtualKeyCode.MEDIA_PLAY_PAUSE;

                case Key.PREVTRACK:
                    return VirtualKeyCode.MEDIA_PREV_TRACK;

                case Key.PRIOR:
                    return VirtualKeyCode.PRIOR;

                case Key.Q:
                    return VirtualKeyCode.VK_Q;

                case Key.R:
                    return VirtualKeyCode.VK_R;

                case Key.RBRACKET:
                    return VirtualKeyCode.OEM_6;

                case Key.RCONTROL:
                    return VirtualKeyCode.RCONTROL;

                case Key.RETURN:
                    return VirtualKeyCode.RETURN;

                case Key.RIGHT:
                    return VirtualKeyCode.RIGHT;

                case Key.RMENU:
                    return VirtualKeyCode.RMENU;

                case Key.RSHIFT:
                    return VirtualKeyCode.RSHIFT;

                case Key.RWIN:
                    return VirtualKeyCode.RWIN;

                case Key.S:
                    return VirtualKeyCode.VK_S;

                case Key.SCROLL:
                    return VirtualKeyCode.SCROLL;

                case Key.SEMICOLON:
                    return VirtualKeyCode.OEM_1;

                case Key.SLASH:
                    return VirtualKeyCode.OEM_2;

                case Key.SLEEP:
                    return VirtualKeyCode.SLEEP;

                case Key.SPACE:
                    return VirtualKeyCode.SPACE;

                case Key.STOP:
                    return VirtualKeyCode.BROWSER_STOP;

                case Key.SUBTRACT:
                    return VirtualKeyCode.SUBTRACT;

                case Key.T:
                    return VirtualKeyCode.VK_T;

                case Key.TAB:
                    return VirtualKeyCode.TAB;

                case Key.U:
                    return VirtualKeyCode.VK_U;

                case Key.UP:
                    return VirtualKeyCode.UP;

                case Key.V:
                    return VirtualKeyCode.VK_V;

                case Key.VOLUMEDOWN:
                    return VirtualKeyCode.VOLUME_DOWN;

                case Key.VOLUMEUP:
                    return VirtualKeyCode.VOLUME_UP;

                case Key.W:
                    return VirtualKeyCode.VK_W;

                case Key.WEBBACK:
                    return VirtualKeyCode.BROWSER_BACK;

                case Key.WEBFAVORITES:
                    return VirtualKeyCode.BROWSER_FAVORITES;

                case Key.WEBFORWARD:
                    return VirtualKeyCode.BROWSER_FORWARD;

                case Key.WEBHOME:
                    return VirtualKeyCode.BROWSER_HOME;

                case Key.WEBREFRESH:
                    return VirtualKeyCode.BROWSER_REFRESH;

                case Key.WEBSEARCH:
                    return VirtualKeyCode.BROWSER_SEARCH;

                case Key.WEBSTOP:
                    return VirtualKeyCode.BROWSER_STOP;

                case Key.X:
                    return VirtualKeyCode.VK_X;

                case Key.Y:
                    return VirtualKeyCode.VK_Y;

                case Key.Z:
                    return VirtualKeyCode.VK_Z;

                case Key.Mouse1:
                    return VirtualKeyCode.LBUTTON;

                case Key.Mouse2:
                    return VirtualKeyCode.RBUTTON;


                default:
                    return VirtualKeyCode.NONAME;
            }
        }
        public static SlimDX.DirectInput.Key convert_KeysToKey(System.Windows.Forms.Keys Key)
        {
            switch (Key)
            {
                case System.Windows.Forms.Keys.None:
                    return SlimDX.DirectInput.Key.Unknown;
                case System.Windows.Forms.Keys.LButton:
                    return SlimDX.DirectInput.Key.Unknown;
                case System.Windows.Forms.Keys.RButton:
                    return SlimDX.DirectInput.Key.Unknown;
                case System.Windows.Forms.Keys.Cancel:
                    return SlimDX.DirectInput.Key.Escape;
                case System.Windows.Forms.Keys.MButton:
                    return SlimDX.DirectInput.Key.Unknown;
                case System.Windows.Forms.Keys.XButton1:
                    return SlimDX.DirectInput.Key.Unknown;
                case System.Windows.Forms.Keys.XButton2:
                    return SlimDX.DirectInput.Key.Unknown;
                case System.Windows.Forms.Keys.Back:
                    return SlimDX.DirectInput.Key.Backspace;
                case System.Windows.Forms.Keys.Tab:
                    return SlimDX.DirectInput.Key.Tab;
                case System.Windows.Forms.Keys.LineFeed:
                    return SlimDX.DirectInput.Key.Unknown;
                case System.Windows.Forms.Keys.Clear:
                    return SlimDX.DirectInput.Key.Delete;
                case System.Windows.Forms.Keys.Return:
                    return SlimDX.DirectInput.Key.Return;
                case System.Windows.Forms.Keys.ShiftKey:
                    return SlimDX.DirectInput.Key.LeftShift;
                case System.Windows.Forms.Keys.ControlKey:
                    return SlimDX.DirectInput.Key.LeftControl;
                case System.Windows.Forms.Keys.Menu:
                    return SlimDX.DirectInput.Key.LeftAlt;
                case System.Windows.Forms.Keys.Pause:
                    return SlimDX.DirectInput.Key.Pause;
                case System.Windows.Forms.Keys.Capital:
                    return SlimDX.DirectInput.Key.CapsLock;
                case System.Windows.Forms.Keys.KanaMode:
                    return SlimDX.DirectInput.Key.Kana;
                case System.Windows.Forms.Keys.JunjaMode:
                    return SlimDX.DirectInput.Key.Unknown;
                case System.Windows.Forms.Keys.FinalMode:
                    return SlimDX.DirectInput.Key.Unknown;
                case System.Windows.Forms.Keys.HanjaMode:
                    return SlimDX.DirectInput.Key.Unknown;
                case System.Windows.Forms.Keys.Escape:
                    return SlimDX.DirectInput.Key.Escape;
                case System.Windows.Forms.Keys.IMEConvert:
                    return SlimDX.DirectInput.Key.Unknown;
                case System.Windows.Forms.Keys.IMENonconvert:
                    return SlimDX.DirectInput.Key.Unknown;
                case System.Windows.Forms.Keys.IMEAceept:
                    return SlimDX.DirectInput.Key.Unknown;
                case System.Windows.Forms.Keys.IMEModeChange:
                    return SlimDX.DirectInput.Key.Unknown;
                case System.Windows.Forms.Keys.Space:
                    return SlimDX.DirectInput.Key.Space;
                case System.Windows.Forms.Keys.PageUp:
                    return SlimDX.DirectInput.Key.PageUp;
               case System.Windows.Forms.Keys.Next:
                    return SlimDX.DirectInput.Key.NextTrack;
                case System.Windows.Forms.Keys.End:
                    return SlimDX.DirectInput.Key.End;
                case System.Windows.Forms.Keys.Home:
                    return SlimDX.DirectInput.Key.Home;
                case System.Windows.Forms.Keys.Left:
                    return SlimDX.DirectInput.Key.LeftArrow;
                case System.Windows.Forms.Keys.Up:
                    return SlimDX.DirectInput.Key.UpArrow;
                case System.Windows.Forms.Keys.Right:
                    return SlimDX.DirectInput.Key.RightArrow;
                case System.Windows.Forms.Keys.Down:
                    return SlimDX.DirectInput.Key.DownArrow;
                case System.Windows.Forms.Keys.Select:
                    return SlimDX.DirectInput.Key.MediaSelect;
                case System.Windows.Forms.Keys.Print:
                    return SlimDX.DirectInput.Key.PrintScreen;
                case System.Windows.Forms.Keys.Execute:
                    return SlimDX.DirectInput.Key.Return;
                case System.Windows.Forms.Keys.PrintScreen:
                    return SlimDX.DirectInput.Key.PrintScreen;
                case System.Windows.Forms.Keys.Insert:
                    return SlimDX.DirectInput.Key.Insert;
                case System.Windows.Forms.Keys.Delete:
                    return SlimDX.DirectInput.Key.Delete;
                case System.Windows.Forms.Keys.Help:
                    return SlimDX.DirectInput.Key.Unknown;
                case System.Windows.Forms.Keys.D0:
                    return SlimDX.DirectInput.Key.D0;
                case System.Windows.Forms.Keys.D1:
                    return SlimDX.DirectInput.Key.D1;
                case System.Windows.Forms.Keys.D2:
                    return SlimDX.DirectInput.Key.D2;
                case System.Windows.Forms.Keys.D3:
                    return SlimDX.DirectInput.Key.D3;
                case System.Windows.Forms.Keys.D4:
                    return SlimDX.DirectInput.Key.D4;
                case System.Windows.Forms.Keys.D5:
                    return SlimDX.DirectInput.Key.D5;
                case System.Windows.Forms.Keys.D6:
                    return SlimDX.DirectInput.Key.D6;
                case System.Windows.Forms.Keys.D7:
                    return SlimDX.DirectInput.Key.D7;
                case System.Windows.Forms.Keys.D8:
                    return SlimDX.DirectInput.Key.D8;
                case System.Windows.Forms.Keys.D9:
                    return SlimDX.DirectInput.Key.D9;
                case System.Windows.Forms.Keys.A:
                    return SlimDX.DirectInput.Key.A;
                case System.Windows.Forms.Keys.B:
                    return SlimDX.DirectInput.Key.B;
                case System.Windows.Forms.Keys.C:
                    return SlimDX.DirectInput.Key.C;
                case System.Windows.Forms.Keys.D:
                    return SlimDX.DirectInput.Key.D;
                case System.Windows.Forms.Keys.E:
                    return SlimDX.DirectInput.Key.E;
                case System.Windows.Forms.Keys.F:
                    return SlimDX.DirectInput.Key.F;
                case System.Windows.Forms.Keys.G:
                    return SlimDX.DirectInput.Key.G;
                case System.Windows.Forms.Keys.H:
                    return SlimDX.DirectInput.Key.H;
                case System.Windows.Forms.Keys.I:
                    return SlimDX.DirectInput.Key.I;
                case System.Windows.Forms.Keys.J:
                    return SlimDX.DirectInput.Key.J;
                case System.Windows.Forms.Keys.K:
                    return SlimDX.DirectInput.Key.K;
                case System.Windows.Forms.Keys.L:
                    return SlimDX.DirectInput.Key.L;
                case System.Windows.Forms.Keys.M:
                    return SlimDX.DirectInput.Key.M;
                case System.Windows.Forms.Keys.N:
                    return SlimDX.DirectInput.Key.N;
                case System.Windows.Forms.Keys.O:
                    return SlimDX.DirectInput.Key.O;
                case System.Windows.Forms.Keys.P:
                    return SlimDX.DirectInput.Key.P;
                case System.Windows.Forms.Keys.Q:
                    return SlimDX.DirectInput.Key.Q;
                case System.Windows.Forms.Keys.R:
                    return SlimDX.DirectInput.Key.R;
                case System.Windows.Forms.Keys.S:
                    return SlimDX.DirectInput.Key.S;
                case System.Windows.Forms.Keys.T:
                    return SlimDX.DirectInput.Key.T;
                case System.Windows.Forms.Keys.U:
                    return SlimDX.DirectInput.Key.U;
                case System.Windows.Forms.Keys.V:
                    return SlimDX.DirectInput.Key.V;
                case System.Windows.Forms.Keys.W:
                    return SlimDX.DirectInput.Key.W;
                case System.Windows.Forms.Keys.X:
                    return SlimDX.DirectInput.Key.X;
                case System.Windows.Forms.Keys.Y:
                    return SlimDX.DirectInput.Key.Y;
                case System.Windows.Forms.Keys.Z:
                    return SlimDX.DirectInput.Key.Z;
                case System.Windows.Forms.Keys.LWin:
                    return SlimDX.DirectInput.Key.LeftWindowsKey;
                case System.Windows.Forms.Keys.RWin:
                    return SlimDX.DirectInput.Key.RightWindowsKey;
                case System.Windows.Forms.Keys.Apps:
                    return SlimDX.DirectInput.Key.Applications;
                case System.Windows.Forms.Keys.Sleep:
                    return SlimDX.DirectInput.Key.Sleep;
                case System.Windows.Forms.Keys.NumPad0:
                    return SlimDX.DirectInput.Key.NumberPad0;
                case System.Windows.Forms.Keys.NumPad1:
                    return SlimDX.DirectInput.Key.NumberPad1;
                case System.Windows.Forms.Keys.NumPad2:
                    return SlimDX.DirectInput.Key.NumberPad2;
                case System.Windows.Forms.Keys.NumPad3:
                    return SlimDX.DirectInput.Key.NumberPad3;
                case System.Windows.Forms.Keys.NumPad4:
                    return SlimDX.DirectInput.Key.NumberPad4;
                case System.Windows.Forms.Keys.NumPad5:
                    return SlimDX.DirectInput.Key.NumberPad5;
                case System.Windows.Forms.Keys.NumPad6:
                    return SlimDX.DirectInput.Key.NumberPad6;
                case System.Windows.Forms.Keys.NumPad7:
                    return SlimDX.DirectInput.Key.NumberPad7;
                case System.Windows.Forms.Keys.NumPad8:
                    return SlimDX.DirectInput.Key.NumberPad8;
                case System.Windows.Forms.Keys.NumPad9:
                    return SlimDX.DirectInput.Key.NumberPad9;
                case System.Windows.Forms.Keys.Multiply:
                    return SlimDX.DirectInput.Key.NumberPadStar;
                case System.Windows.Forms.Keys.Add:
                    return SlimDX.DirectInput.Key.NumberPadPlus;
                case System.Windows.Forms.Keys.Separator:
                    return SlimDX.DirectInput.Key.Comma;
                case System.Windows.Forms.Keys.Subtract:
                    return SlimDX.DirectInput.Key.Minus;
                case System.Windows.Forms.Keys.Decimal:
                    return SlimDX.DirectInput.Key.Period;
                case System.Windows.Forms.Keys.Divide:
                    return SlimDX.DirectInput.Key.Slash;
                case System.Windows.Forms.Keys.F1:
                    return SlimDX.DirectInput.Key.F1;
                case System.Windows.Forms.Keys.F2:
                    return SlimDX.DirectInput.Key.F2;
                case System.Windows.Forms.Keys.F3:
                    return SlimDX.DirectInput.Key.F3;
                case System.Windows.Forms.Keys.F4:
                    return SlimDX.DirectInput.Key.F4;
                case System.Windows.Forms.Keys.F5:
                    return SlimDX.DirectInput.Key.F5;
                case System.Windows.Forms.Keys.F6:
                    return SlimDX.DirectInput.Key.F6;
                case System.Windows.Forms.Keys.F7:
                    return SlimDX.DirectInput.Key.F7;
                case System.Windows.Forms.Keys.F8:
                    return SlimDX.DirectInput.Key.F8;
                case System.Windows.Forms.Keys.F9:
                    return SlimDX.DirectInput.Key.F9;
                case System.Windows.Forms.Keys.F10:
                    return SlimDX.DirectInput.Key.F10;
                case System.Windows.Forms.Keys.F11:
                    return SlimDX.DirectInput.Key.F11;
                case System.Windows.Forms.Keys.F12:
                    return SlimDX.DirectInput.Key.F12;
                case System.Windows.Forms.Keys.F13:
                    return SlimDX.DirectInput.Key.F13;
                case System.Windows.Forms.Keys.F14:
                    return SlimDX.DirectInput.Key.F14;
                case System.Windows.Forms.Keys.F15:
                    return SlimDX.DirectInput.Key.F15;
                case System.Windows.Forms.Keys.F16:
                    return SlimDX.DirectInput.Key.Unknown;
                case System.Windows.Forms.Keys.F17:
                    return SlimDX.DirectInput.Key.Unknown;
                case System.Windows.Forms.Keys.F18:
                    return SlimDX.DirectInput.Key.Unknown;
                case System.Windows.Forms.Keys.F19:
                    return SlimDX.DirectInput.Key.Unknown;
                case System.Windows.Forms.Keys.F20:
                    return SlimDX.DirectInput.Key.Unknown;
                case System.Windows.Forms.Keys.F21:
                    return SlimDX.DirectInput.Key.Unknown;
                case System.Windows.Forms.Keys.F22:
                    return SlimDX.DirectInput.Key.Unknown;
                case System.Windows.Forms.Keys.F23:
                    return SlimDX.DirectInput.Key.Unknown;
                case System.Windows.Forms.Keys.F24:
                    return SlimDX.DirectInput.Key.Unknown;
                case System.Windows.Forms.Keys.NumLock:
                    return SlimDX.DirectInput.Key.NumberLock;
                case System.Windows.Forms.Keys.Scroll:
                    return SlimDX.DirectInput.Key.ScrollLock;
                case System.Windows.Forms.Keys.LShiftKey:
                    return SlimDX.DirectInput.Key.LeftShift;
                case System.Windows.Forms.Keys.RShiftKey:
                    return SlimDX.DirectInput.Key.RightShift;
                case System.Windows.Forms.Keys.LControlKey:
                    return SlimDX.DirectInput.Key.LeftControl;
                case System.Windows.Forms.Keys.RControlKey:
                    return SlimDX.DirectInput.Key.RightControl;
                case System.Windows.Forms.Keys.LMenu:
                    return SlimDX.DirectInput.Key.LeftAlt;
                case System.Windows.Forms.Keys.RMenu:
                    return SlimDX.DirectInput.Key.RightAlt;
                case System.Windows.Forms.Keys.BrowserBack:
                    return SlimDX.DirectInput.Key.WebBack;
                case System.Windows.Forms.Keys.BrowserForward:
                    return SlimDX.DirectInput.Key.WebForward;
                case System.Windows.Forms.Keys.BrowserRefresh:
                    return SlimDX.DirectInput.Key.WebRefresh;
                case System.Windows.Forms.Keys.BrowserStop:
                    return SlimDX.DirectInput.Key.WebStop;
                case System.Windows.Forms.Keys.BrowserSearch:
                    return SlimDX.DirectInput.Key.WebSearch;
                case System.Windows.Forms.Keys.BrowserFavorites:
                    return SlimDX.DirectInput.Key.WebFavorites;
                case System.Windows.Forms.Keys.BrowserHome:
                    return SlimDX.DirectInput.Key.WebHome;
                case System.Windows.Forms.Keys.VolumeMute:
                    return SlimDX.DirectInput.Key.Mute;
                case System.Windows.Forms.Keys.VolumeDown:
                    return SlimDX.DirectInput.Key.VolumeDown;
                case System.Windows.Forms.Keys.VolumeUp:
                    return SlimDX.DirectInput.Key.VolumeUp;
                case System.Windows.Forms.Keys.MediaNextTrack:
                    return SlimDX.DirectInput.Key.NextTrack;
                case System.Windows.Forms.Keys.MediaPreviousTrack:
                    return SlimDX.DirectInput.Key.PreviousTrack;
                case System.Windows.Forms.Keys.MediaStop:
                    return SlimDX.DirectInput.Key.MediaStop;
                case System.Windows.Forms.Keys.MediaPlayPause:
                    return SlimDX.DirectInput.Key.Pause;
                case System.Windows.Forms.Keys.LaunchMail:
                    return SlimDX.DirectInput.Key.Mail;
                case System.Windows.Forms.Keys.SelectMedia:
                    return SlimDX.DirectInput.Key.MediaSelect;
                case System.Windows.Forms.Keys.LaunchApplication1:
                    return SlimDX.DirectInput.Key.Unknown;
                case System.Windows.Forms.Keys.LaunchApplication2:
                    return SlimDX.DirectInput.Key.Unknown;
                case System.Windows.Forms.Keys.Oem1:
                    return SlimDX.DirectInput.Key.Oem102;
                case System.Windows.Forms.Keys.Oemplus:
                    return SlimDX.DirectInput.Key.NumberPadPlus;
                case System.Windows.Forms.Keys.Oemcomma:
                    return SlimDX.DirectInput.Key.Comma;
                case System.Windows.Forms.Keys.OemMinus:
                    return SlimDX.DirectInput.Key.Minus;
                case System.Windows.Forms.Keys.OemPeriod:
                    return SlimDX.DirectInput.Key.Period;
                case System.Windows.Forms.Keys.OemQuestion:
                    return SlimDX.DirectInput.Key.Unknown;
                case System.Windows.Forms.Keys.Oemtilde:
                    return SlimDX.DirectInput.Key.Unknown;
                case System.Windows.Forms.Keys.OemOpenBrackets:
                    return SlimDX.DirectInput.Key.LeftBracket;
                case System.Windows.Forms.Keys.Oem5:
                    return SlimDX.DirectInput.Key.Unknown;
                case System.Windows.Forms.Keys.Oem6:
                    return SlimDX.DirectInput.Key.Unknown;
                case System.Windows.Forms.Keys.Oem7:
                    return SlimDX.DirectInput.Key.Unknown;
                case System.Windows.Forms.Keys.Oem8:
                    return SlimDX.DirectInput.Key.Unknown;
                case System.Windows.Forms.Keys.OemBackslash:
                    return SlimDX.DirectInput.Key.Backslash;
                case System.Windows.Forms.Keys.ProcessKey:
                    return SlimDX.DirectInput.Key.Unknown;
                case System.Windows.Forms.Keys.Packet:
                    return SlimDX.DirectInput.Key.Unknown;
                case System.Windows.Forms.Keys.Attn:
                    return SlimDX.DirectInput.Key.AT;
                case System.Windows.Forms.Keys.Crsel:
                    return SlimDX.DirectInput.Key.Unknown;
                case System.Windows.Forms.Keys.Exsel:
                    return SlimDX.DirectInput.Key.Unknown;
                case System.Windows.Forms.Keys.EraseEof:
                    return SlimDX.DirectInput.Key.Unknown;
                case System.Windows.Forms.Keys.Play:
                    return SlimDX.DirectInput.Key.PlayPause;
                case System.Windows.Forms.Keys.Zoom:
                    return SlimDX.DirectInput.Key.Unknown;
                case System.Windows.Forms.Keys.NoName:
                    return SlimDX.DirectInput.Key.Unknown;
                case System.Windows.Forms.Keys.Pa1:
                    return SlimDX.DirectInput.Key.Unknown;
                case System.Windows.Forms.Keys.OemClear:
                    return SlimDX.DirectInput.Key.Unknown;
                case System.Windows.Forms.Keys.KeyCode:
                    return SlimDX.DirectInput.Key.Unknown;
                case System.Windows.Forms.Keys.Shift:
                    return SlimDX.DirectInput.Key.LeftShift;
                case System.Windows.Forms.Keys.Control:
                    return SlimDX.DirectInput.Key.LeftControl;
                case System.Windows.Forms.Keys.Alt:
                    return SlimDX.DirectInput.Key.LeftAlt;
                case System.Windows.Forms.Keys.Modifiers:
                    return SlimDX.DirectInput.Key.Unknown;


                default:
                    return SlimDX.DirectInput.Key.Unknown;
            }
        }

        public static SlimDX.DirectInput.MouseObject convert_KeyToMouseObject(Enigma.D3.Enums.Key Key)
        {
            switch (Key)
            {
                case Enigma.D3.Enums.Key.Mouse1:
                    return SlimDX.DirectInput.MouseObject.Button1;
                case Enigma.D3.Enums.Key.Mouse2:
                    return SlimDX.DirectInput.MouseObject.Button2;
                case Enigma.D3.Enums.Key.Mouse3:
                    return SlimDX.DirectInput.MouseObject.Button3;
                case Enigma.D3.Enums.Key.Mouse4:
                    return SlimDX.DirectInput.MouseObject.Button4;
                case Enigma.D3.Enums.Key.Mouse5:
                    return SlimDX.DirectInput.MouseObject.Button5;
                //case Enigma.D3.Enums.Key.MWheelUp:
                //    return SlimDX.DirectInput.MouseObject.
                //case Enigma.D3.Enums.Key.MWheelDown:
                //    return SlimDX.DirectInput.Mouse

                default:
                    return SlimDX.DirectInput.MouseObject.Button8;

            }
        }

        public static SlimDX.DirectInput.Key convert_KeyToSlimDxKey(Enigma.D3.Enums.Key Key)
        {
            switch (Key)
            {
                case Enigma.D3.Enums.Key.ESCAPE:
                    return SlimDX.DirectInput.Key.Escape;
                case Enigma.D3.Enums.Key.D1:
                    return SlimDX.DirectInput.Key.D1;
                case Enigma.D3.Enums.Key.D2:
                    return SlimDX.DirectInput.Key.D2;
                case Enigma.D3.Enums.Key.D3:
                    return SlimDX.DirectInput.Key.D3;
                case Enigma.D3.Enums.Key.D4:
                    return SlimDX.DirectInput.Key.D4;
                case Enigma.D3.Enums.Key.D5:
                    return SlimDX.DirectInput.Key.D5;
                case Enigma.D3.Enums.Key.D6:
                    return SlimDX.DirectInput.Key.D6;
                case Enigma.D3.Enums.Key.D7:
                    return SlimDX.DirectInput.Key.D7;
                case Enigma.D3.Enums.Key.D8:
                    return SlimDX.DirectInput.Key.D8;
                case Enigma.D3.Enums.Key.D9:
                    return SlimDX.DirectInput.Key.D9;
                case Enigma.D3.Enums.Key.D0:
                    return SlimDX.DirectInput.Key.D0;
                case Enigma.D3.Enums.Key.MINUS:
                    return SlimDX.DirectInput.Key.Minus;
                case Enigma.D3.Enums.Key.EQUALS:
                    return SlimDX.DirectInput.Key.Equals;
                case Enigma.D3.Enums.Key.BACK:
                    return SlimDX.DirectInput.Key.Backspace;
                case Enigma.D3.Enums.Key.TAB:
                    return SlimDX.DirectInput.Key.Tab;
                case Enigma.D3.Enums.Key.Q:
                    return SlimDX.DirectInput.Key.Q;
                case Enigma.D3.Enums.Key.W:
                    return SlimDX.DirectInput.Key.W;
                case Enigma.D3.Enums.Key.E:
                    return SlimDX.DirectInput.Key.E;
                case Enigma.D3.Enums.Key.R:
                    return SlimDX.DirectInput.Key.R;
                case Enigma.D3.Enums.Key.T:
                    return SlimDX.DirectInput.Key.T;
                case Enigma.D3.Enums.Key.Y:
                    return SlimDX.DirectInput.Key.Y;
                case Enigma.D3.Enums.Key.U:
                    return SlimDX.DirectInput.Key.U;
                case Enigma.D3.Enums.Key.I:
                    return SlimDX.DirectInput.Key.I;
                case Enigma.D3.Enums.Key.O:
                    return SlimDX.DirectInput.Key.O;
                case Enigma.D3.Enums.Key.P:
                    return SlimDX.DirectInput.Key.P;
                case Enigma.D3.Enums.Key.LBRACKET:
                    return SlimDX.DirectInput.Key.LeftBracket;
                case Enigma.D3.Enums.Key.RBRACKET:
                    return SlimDX.DirectInput.Key.RightBracket;
                case Enigma.D3.Enums.Key.RETURN:
                    return SlimDX.DirectInput.Key.Return;
                case Enigma.D3.Enums.Key.LCONTROL:
                    return SlimDX.DirectInput.Key.LeftControl;
                case Enigma.D3.Enums.Key.A:
                    return SlimDX.DirectInput.Key.A;
                case Enigma.D3.Enums.Key.S:
                    return SlimDX.DirectInput.Key.S;
                case Enigma.D3.Enums.Key.D:
                    return SlimDX.DirectInput.Key.D;
                case Enigma.D3.Enums.Key.F:
                    return SlimDX.DirectInput.Key.F;
                case Enigma.D3.Enums.Key.G:
                    return SlimDX.DirectInput.Key.G;
                case Enigma.D3.Enums.Key.H:
                    return SlimDX.DirectInput.Key.H;
                case Enigma.D3.Enums.Key.J:
                    return SlimDX.DirectInput.Key.J;
                case Enigma.D3.Enums.Key.K:
                    return SlimDX.DirectInput.Key.K;
                case Enigma.D3.Enums.Key.L:
                    return SlimDX.DirectInput.Key.L;
                case Enigma.D3.Enums.Key.SEMICOLON:
                    return SlimDX.DirectInput.Key.Semicolon;
                case Enigma.D3.Enums.Key.APOSTROPHE:
                    return SlimDX.DirectInput.Key.Apostrophe;
                case Enigma.D3.Enums.Key.GRAVE:
                    return SlimDX.DirectInput.Key.Grave;
                case Enigma.D3.Enums.Key.LSHIFT:
                    return SlimDX.DirectInput.Key.LeftShift;
                case Enigma.D3.Enums.Key.BACKSLASH:
                    return SlimDX.DirectInput.Key.Backslash;
                case Enigma.D3.Enums.Key.Z:
                    return SlimDX.DirectInput.Key.Z;
                case Enigma.D3.Enums.Key.X:
                    return SlimDX.DirectInput.Key.X;
                case Enigma.D3.Enums.Key.C:
                    return SlimDX.DirectInput.Key.C;
                case Enigma.D3.Enums.Key.V:
                    return SlimDX.DirectInput.Key.V;
                case Enigma.D3.Enums.Key.B:
                    return SlimDX.DirectInput.Key.B;
                case Enigma.D3.Enums.Key.N:
                    return SlimDX.DirectInput.Key.N;
                case Enigma.D3.Enums.Key.M:
                    return SlimDX.DirectInput.Key.M;
                case Enigma.D3.Enums.Key.COMMA:
                    return SlimDX.DirectInput.Key.Comma;
                case Enigma.D3.Enums.Key.PERIOD:
                    return SlimDX.DirectInput.Key.Period;
                case Enigma.D3.Enums.Key.SLASH:
                    return SlimDX.DirectInput.Key.Slash;
                case Enigma.D3.Enums.Key.RSHIFT:
                    return SlimDX.DirectInput.Key.RightShift;
                case Enigma.D3.Enums.Key.MULTIPLY:
                    return SlimDX.DirectInput.Key.NumberPadStar;
                case Enigma.D3.Enums.Key.LMENU:
                    return SlimDX.DirectInput.Key.LeftAlt;
                case Enigma.D3.Enums.Key.SPACE:
                    return SlimDX.DirectInput.Key.Space;
                case Enigma.D3.Enums.Key.CAPITAL:
                    return SlimDX.DirectInput.Key.Unknown;
                case Enigma.D3.Enums.Key.F1:
                    return SlimDX.DirectInput.Key.F1;
                case Enigma.D3.Enums.Key.F2:
                    return SlimDX.DirectInput.Key.F2;
                case Enigma.D3.Enums.Key.F3:
                    return SlimDX.DirectInput.Key.F3;
                case Enigma.D3.Enums.Key.F4:
                    return SlimDX.DirectInput.Key.F4;
                case Enigma.D3.Enums.Key.F5:
                    return SlimDX.DirectInput.Key.F5;
                case Enigma.D3.Enums.Key.F6:
                    return SlimDX.DirectInput.Key.F6;
                case Enigma.D3.Enums.Key.F7:
                    return SlimDX.DirectInput.Key.F7;
                case Enigma.D3.Enums.Key.F8:
                    return SlimDX.DirectInput.Key.F8;
                case Enigma.D3.Enums.Key.F9:
                    return SlimDX.DirectInput.Key.F9;
                case Enigma.D3.Enums.Key.F10:
                    return SlimDX.DirectInput.Key.F10;
                case Enigma.D3.Enums.Key.NUMLOCK:
                    return SlimDX.DirectInput.Key.NumberLock;
                case Enigma.D3.Enums.Key.SCROLL:
                    return SlimDX.DirectInput.Key.ScrollLock;
                case Enigma.D3.Enums.Key.NUMPAD7:
                    return SlimDX.DirectInput.Key.NumberPad7;
                case Enigma.D3.Enums.Key.NUMPAD8:
                    return SlimDX.DirectInput.Key.NumberPad8;
                case Enigma.D3.Enums.Key.NUMPAD9:
                    return SlimDX.DirectInput.Key.NumberPad9;
                case Enigma.D3.Enums.Key.SUBTRACT:
                    return SlimDX.DirectInput.Key.Minus;
                case Enigma.D3.Enums.Key.NUMPAD4:
                    return SlimDX.DirectInput.Key.NumberPad4;
                case Enigma.D3.Enums.Key.NUMPAD5:
                    return SlimDX.DirectInput.Key.NumberPad5;
                case Enigma.D3.Enums.Key.NUMPAD6:
                    return SlimDX.DirectInput.Key.NumberPad6;
                case Enigma.D3.Enums.Key.ADD:
                    return SlimDX.DirectInput.Key.NumberPadPlus;
                case Enigma.D3.Enums.Key.NUMPAD1:
                    return SlimDX.DirectInput.Key.NumberPad1;
                case Enigma.D3.Enums.Key.NUMPAD2:
                    return SlimDX.DirectInput.Key.NumberPad2;
                case Enigma.D3.Enums.Key.NUMPAD3:
                    return SlimDX.DirectInput.Key.NumberPad3;
                case Enigma.D3.Enums.Key.NUMPAD0:
                    return SlimDX.DirectInput.Key.NumberPad0;
                case Enigma.D3.Enums.Key.DECIMAL:
                    return SlimDX.DirectInput.Key.Slash;
                case Enigma.D3.Enums.Key.OEM_102:
                    return SlimDX.DirectInput.Key.Oem102;
                case Enigma.D3.Enums.Key.F11:
                    return SlimDX.DirectInput.Key.F11;
                case Enigma.D3.Enums.Key.F12:
                    return SlimDX.DirectInput.Key.F12;
                case Enigma.D3.Enums.Key.F13:
                    return SlimDX.DirectInput.Key.F13;
                case Enigma.D3.Enums.Key.F14:
                    return SlimDX.DirectInput.Key.F14;
                case Enigma.D3.Enums.Key.F15:
                    return SlimDX.DirectInput.Key.F15;
                case Enigma.D3.Enums.Key.KANA:
                    return SlimDX.DirectInput.Key.Kana;
                case Enigma.D3.Enums.Key.ABNT_C1:
                    return SlimDX.DirectInput.Key.AbntC1;
                case Enigma.D3.Enums.Key.CONVERT:
                    return SlimDX.DirectInput.Key.Convert;
                case Enigma.D3.Enums.Key.NOCONVERT:
                    return SlimDX.DirectInput.Key.NoConvert;
                case Enigma.D3.Enums.Key.YEN:
                    return SlimDX.DirectInput.Key.Yen;
                case Enigma.D3.Enums.Key.ABNT_C2:
                    return SlimDX.DirectInput.Key.AbntC2;
                case Enigma.D3.Enums.Key.NUMPADEQUALS:
                    return SlimDX.DirectInput.Key.NumberPadEquals;
                case Enigma.D3.Enums.Key.PREVTRACK:
                    return SlimDX.DirectInput.Key.PreviousTrack;
                case Enigma.D3.Enums.Key.AT:
                    return SlimDX.DirectInput.Key.AT;
                case Enigma.D3.Enums.Key.COLON:
                    return SlimDX.DirectInput.Key.Colon;
                case Enigma.D3.Enums.Key.UNDERLINE:
                    return SlimDX.DirectInput.Key.Underline;
                case Enigma.D3.Enums.Key.KANJI:
                    return SlimDX.DirectInput.Key.Kanji;
                case Enigma.D3.Enums.Key.STOP:
                    return SlimDX.DirectInput.Key.Stop;
                case Enigma.D3.Enums.Key.AX:
                    return SlimDX.DirectInput.Key.AX;
                case Enigma.D3.Enums.Key.UNLABELED:
                    return SlimDX.DirectInput.Key.Unlabeled;
                case Enigma.D3.Enums.Key.NEXTTRACK:
                    return SlimDX.DirectInput.Key.NextTrack;
                case Enigma.D3.Enums.Key.NUMPADENTER:
                    return SlimDX.DirectInput.Key.NumberPadEnter;
                case Enigma.D3.Enums.Key.RCONTROL:
                    return SlimDX.DirectInput.Key.RightControl;
                case Enigma.D3.Enums.Key.MUTE:
                    return SlimDX.DirectInput.Key.Mute;
                case Enigma.D3.Enums.Key.CALCULATOR:
                    return SlimDX.DirectInput.Key.Calculator;
                case Enigma.D3.Enums.Key.PLAYPAUSE:
                    return SlimDX.DirectInput.Key.PlayPause;
                case Enigma.D3.Enums.Key.MEDIASTOP:
                    return SlimDX.DirectInput.Key.MediaStop;
                case Enigma.D3.Enums.Key.VOLUMEDOWN:
                    return SlimDX.DirectInput.Key.VolumeDown;
                case Enigma.D3.Enums.Key.VOLUMEUP:
                    return SlimDX.DirectInput.Key.VolumeUp;
                case Enigma.D3.Enums.Key.WEBHOME:
                    return SlimDX.DirectInput.Key.WebHome;
                case Enigma.D3.Enums.Key.NUMPADCOMMA:
                    return SlimDX.DirectInput.Key.NumberPadComma;
                case Enigma.D3.Enums.Key.DIVIDE:
                    return SlimDX.DirectInput.Key.Slash;
                case Enigma.D3.Enums.Key.SYSRQ:
                    return SlimDX.DirectInput.Key.Unknown;
                case Enigma.D3.Enums.Key.RMENU:
                    return SlimDX.DirectInput.Key.RightAlt;
                case Enigma.D3.Enums.Key.PAUSE:
                    return SlimDX.DirectInput.Key.Pause;
                case Enigma.D3.Enums.Key.HOME:
                    return SlimDX.DirectInput.Key.Home;
                case Enigma.D3.Enums.Key.UP:
                    return SlimDX.DirectInput.Key.UpArrow;
                case Enigma.D3.Enums.Key.PRIOR:
                    return SlimDX.DirectInput.Key.Unknown;
                case Enigma.D3.Enums.Key.LEFT:
                    return SlimDX.DirectInput.Key.LeftArrow;
                case Enigma.D3.Enums.Key.RIGHT:
                    return SlimDX.DirectInput.Key.RightArrow;
                case Enigma.D3.Enums.Key.END:
                    return SlimDX.DirectInput.Key.End;
                case Enigma.D3.Enums.Key.DOWN:
                    return SlimDX.DirectInput.Key.DownArrow;
                case Enigma.D3.Enums.Key.NEXT:
                    return SlimDX.DirectInput.Key.NextTrack;
                case Enigma.D3.Enums.Key.INSERT:
                    return SlimDX.DirectInput.Key.Insert;
                case Enigma.D3.Enums.Key.DELETE:
                    return SlimDX.DirectInput.Key.Delete;
                case Enigma.D3.Enums.Key.LWIN:
                    return SlimDX.DirectInput.Key.LeftWindowsKey;
                case Enigma.D3.Enums.Key.RWIN:
                    return SlimDX.DirectInput.Key.RightWindowsKey;
                case Enigma.D3.Enums.Key.APPS:
                    return SlimDX.DirectInput.Key.Applications;
                case Enigma.D3.Enums.Key.Power:
                    return SlimDX.DirectInput.Key.Power;
                case Enigma.D3.Enums.Key.SLEEP:
                    return SlimDX.DirectInput.Key.Sleep;
                case Enigma.D3.Enums.Key.WAKE:
                    return SlimDX.DirectInput.Key.Wake;
                case Enigma.D3.Enums.Key.WEBSEARCH:
                    return SlimDX.DirectInput.Key.WebSearch;
                case Enigma.D3.Enums.Key.WEBFAVORITES:
                    return SlimDX.DirectInput.Key.WebFavorites;
                case Enigma.D3.Enums.Key.WEBREFRESH:
                    return SlimDX.DirectInput.Key.WebRefresh;
                case Enigma.D3.Enums.Key.WEBSTOP:
                    return SlimDX.DirectInput.Key.WebStop;
                case Enigma.D3.Enums.Key.WEBFORWARD:
                    return SlimDX.DirectInput.Key.WebForward;
                case Enigma.D3.Enums.Key.WEBBACK:
                    return SlimDX.DirectInput.Key.WebBack;
                case Enigma.D3.Enums.Key.MYCOMPUTER:
                    return SlimDX.DirectInput.Key.MyComputer;
                case Enigma.D3.Enums.Key.MAIL:
                    return SlimDX.DirectInput.Key.Mail;
                case Enigma.D3.Enums.Key.MEDIASELECT:
                    return SlimDX.DirectInput.Key.MediaSelect;
                case Enigma.D3.Enums.Key.Mouse1:
                    return SlimDX.DirectInput.Key.Unknown;
                case Enigma.D3.Enums.Key.Mouse2:
                    return SlimDX.DirectInput.Key.Unknown;
                case Enigma.D3.Enums.Key.Mouse3:
                    return SlimDX.DirectInput.Key.Unknown;
                case Enigma.D3.Enums.Key.Mouse4:
                    return SlimDX.DirectInput.Key.Unknown;
                case Enigma.D3.Enums.Key.Mouse5:
                    return SlimDX.DirectInput.Key.Unknown;
                case Enigma.D3.Enums.Key.MWheelUp:
                    return SlimDX.DirectInput.Key.Unknown;
                case Enigma.D3.Enums.Key.MWheelDown:
                    return SlimDX.DirectInput.Key.Unknown;
                case Enigma.D3.Enums.Key.OEM_8:
                    return SlimDX.DirectInput.Key.Unknown;
                case Enigma.D3.Enums.Key.Undefined:
                    return SlimDX.DirectInput.Key.Unknown;

                default:
                    return SlimDX.DirectInput.Key.Unknown;
            }
        }
    }
}
