using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using D3Helper.A_Collection;
using SlimDX.DirectInput;
using SlimDX.DirectWrite;

namespace D3Helper.A_Collector
{
    class H_Keyboard
    {
        private static DateTime lastHotkeyProcess = new DateTime();

        public static void Collect()
        {
            try
            {
                UpdateHotkeyList();
                processPressedKeys();
                ResetLastProcessedHotkey();
                get_PressedIngameKeys();
            }
            catch (Exception e)
            {
                A_Handler.Log.Exception.addExceptionLogEntry(e, A_Enums.ExceptionThread.ECollector);
            }
        }


        private static void UpdateHotkeyList()
        {
            try
            {
                Hotkey slot1 = get_HotkeyFromSettingsString(Properties.Settings.Default.HotkeySlot1);
                Hotkey slot2 = get_HotkeyFromSettingsString(Properties.Settings.Default.HotkeySlot2);
                Hotkey slot3 = get_HotkeyFromSettingsString(Properties.Settings.Default.HotkeySlot3);
                Hotkey slot4 = get_HotkeyFromSettingsString(Properties.Settings.Default.HotkeySlot4);
                Hotkey slotrmb = get_HotkeyFromSettingsString(Properties.Settings.Default.HotkeySlotRmb);
                Hotkey slotlmb = get_HotkeyFromSettingsString(Properties.Settings.Default.HotkeySlotLmb);
                Hotkey paragonpoints1 = get_HotkeyFromSettingsString(Properties.Settings.Default.HotkeyParagonPoints1);
                Hotkey paragonpoints2 = get_HotkeyFromSettingsString(Properties.Settings.Default.HotkeyParagonPoints2);
                Hotkey paragonpoints3 = get_HotkeyFromSettingsString(Properties.Settings.Default.HotkeyParagonPoints3);
                Hotkey paragonpoints4 = get_HotkeyFromSettingsString(Properties.Settings.Default.HotkeyParagonPoints4);
                Hotkey autogamble = get_HotkeyFromSettingsString(Properties.Settings.Default.HotkeyAutoGamble);

                Hotkey autopick = get_HotkeyFromSettingsString(Properties.Settings.Default.HotkeyAutoPick);
                Hotkey autocube_upgraderare = get_HotkeyFromSettingsString(Properties.Settings.Default.HotkeyAutoCube_UpgradeRare);
                Hotkey autocube_convertmaterial = get_HotkeyFromSettingsString(Properties.Settings.Default.HotkeyAutoCube_ConvertMaterial);

                Dictionary<Hotkey, string> hotkeys = new Dictionary<Hotkey, string>();


                hotkeys.Add(slot1, "slot1");
                hotkeys.Add(slot2, "slot2");
                hotkeys.Add(slot3, "slot3");
                hotkeys.Add(slot4, "slot4");
                hotkeys.Add(slotrmb, "slotrmb");
                hotkeys.Add(slotlmb, "slotlmb");
                hotkeys.Add(paragonpoints1, "paragonpoints1");
                hotkeys.Add(paragonpoints2, "paragonpoints2");
                hotkeys.Add(paragonpoints3, "paragonpoints3");
                hotkeys.Add(paragonpoints4, "paragonpoints4");
                hotkeys.Add(autogamble, "autogamble");
                hotkeys.Add(autopick, "autopick");
                hotkeys.Add(autocube_upgraderare, "autocube_upgraderare");
                hotkeys.Add(autocube_convertmaterial, "autocube_convertmaterial");

                A_Collection.Hotkeys.D3Helper_Hotkeys = hotkeys;
            }
            catch (Exception e)
            {
                A_Handler.Log.Exception.addExceptionLogEntry(e, A_Enums.ExceptionThread.ECollector);
            }
        }


        public static Hotkey get_HotkeyFromSettingsString(string HotkeyString)
        {
            try
            {
                if(HotkeyString == String.Empty)
                    return new Hotkey(Key.Unknown, new List<Key>());

                var Split = HotkeyString.Split('+');

                Key key = Key.Unknown;
                List<Key> modifiers = new List<Key>();

                for (int i = 0; i < Split.Length; i++)
                {
                    
                    if (i == Split.Length -1)
                        if(!Enum.TryParse<SlimDX.DirectInput.Key>(Split[i], out key))
                            break;
                    
                    if(Split[i].Contains("CTRL"))
                        modifiers.Add(Key.LeftControl);
                    if(Split[i].Contains("ALT"))
                        modifiers.Add(Key.LeftAlt);
                    if(Split[i].Contains("SHIFT"))
                        modifiers.Add(Key.LeftShift);
                }

                return new Hotkey(key, modifiers);
            }
            catch (Exception)
            {
                return new Hotkey(SlimDX.DirectInput.Key.Unknown, new List<Key>());
                throw;
            }
        }


        private static void processPressedKeys()
        {
            try
            {
                if (A_Collection.D3Client.Window.isForeground && !A_Collection.D3UI.isChatting)
                {
                    if (Window_Main.keyboard == null)
                        return;

                    KeyboardState deviceState = Window_Main.keyboard.GetCurrentState();

                    lock (A_Collection.Hotkeys._PressedKeys)
                        A_Collection.Hotkeys._PressedKeys = deviceState.PressedKeys.ToList();



                    //collect pressed mouse buttons
                    lock (A_Collection.Hotkeys._pressedMouseButtons)
                    {
                        MouseState mouseState = Window_Main.mouse.GetCurrentState();

                        List<MouseObject> _tempList = new List<MouseObject>();
                        foreach (int mouseKeyCode in Enum.GetValues(typeof(MouseObject)))
                        {
                            if(mouseKeyCode < (int)MouseObject.XAxis)
                            {
                                if (mouseState.IsPressed(mouseKeyCode))
                                {
                                    _tempList.Add((MouseObject)mouseKeyCode);
                                }
                            }
                        }

                        A_Collection.Hotkeys._pressedMouseButtons = _tempList;
                    }


                    foreach (var hotkey in A_Collection.Hotkeys.D3Helper_Hotkeys)
                    {
                        if (hotkey.Key.Key == Key.Unknown)
                        {
                            continue;
                        }

                        if (A_Collection.Hotkeys.lastprocessedHotkey != hotkey.Key.Key)
                        {
                            bool isPressed = true;

                            foreach (var modifier in hotkey.Key.Modifiers)
                            {
                                if (!deviceState.IsPressed(modifier))
                                    isPressed = false;
                            }

                            if (!deviceState.IsPressed(hotkey.Key.Key))
                                isPressed = false;

                            if(!isPressed)
                                continue;

                            switch (hotkey.Value)
                            {

                                case "slot1":
                                    A_Tools.T_ExternalFile.AutoCastOverrides.ChangeOverrides(0);
                                    break;
                                case "slot2":
                                    A_Tools.T_ExternalFile.AutoCastOverrides.ChangeOverrides(1);
                                    break;
                                case "slot3":
                                    A_Tools.T_ExternalFile.AutoCastOverrides.ChangeOverrides(2);
                                    break;
                                case "slot4":
                                    A_Tools.T_ExternalFile.AutoCastOverrides.ChangeOverrides(3);
                                    break;
                                case "slotrmb":
                                    A_Tools.T_ExternalFile.AutoCastOverrides.ChangeOverrides(4);
                                    break;
                                case "slotlmb":
                                    A_Tools.T_ExternalFile.AutoCastOverrides.ChangeOverrides(5);
                                    break;
                            
                                case "paragonpoints1":
                                    if (!A_Collection.Me.ParagonPointSpender.Is_SpendingPoints)
                                    {
                                        A_Collection.Me.ParagonPointSpender.SelectedParagonPoints_Setup = 1;

                                        A_Handler.ParagonPointSpender.ParagonPointSpender.Set_ParagonPoints();
                                    }
                                    break;
                                case "paragonpoints2":
                                    if (!A_Collection.Me.ParagonPointSpender.Is_SpendingPoints)
                                    {
                                        A_Collection.Me.ParagonPointSpender.SelectedParagonPoints_Setup = 2;

                                        A_Handler.ParagonPointSpender.ParagonPointSpender.Set_ParagonPoints();
                                    }
                                    break;
                                case "paragonpoints3":
                                    if (!A_Collection.Me.ParagonPointSpender.Is_SpendingPoints)
                                    {
                                        A_Collection.Me.ParagonPointSpender.SelectedParagonPoints_Setup = 3;

                                        A_Handler.ParagonPointSpender.ParagonPointSpender.Set_ParagonPoints();
                                    }
                                    break;
                                case "paragonpoints4":
                                    if (!A_Collection.Me.ParagonPointSpender.Is_SpendingPoints)
                                    {
                                        A_Collection.Me.ParagonPointSpender.SelectedParagonPoints_Setup = 4;

                                        A_Handler.ParagonPointSpender.ParagonPointSpender.Set_ParagonPoints();
                                    }
                                    break;
                                case "autogamble":
                                    Properties.Settings.Default.AutoGambleBool =
                                        !Properties.Settings.Default.AutoGambleBool;
                                    Properties.Settings.Default.Save();
                                    break;

                                case "autopick":
                                    if (!A_Handler.AutoPick.AutoPick.IsPicking)
                                    {
                                        A_Handler.AutoPick.AutoPick.DoPickup();
                                    }
                                    break;

                                case "autocube_upgraderare":
                                    if (!A_Handler.AutoCube.UpgradeRare.IsUpgrading_Rare)
                                    {
                                        A_Handler.AutoCube.UpgradeRare.DoUpgrade();
                                    }
                                    break;

                                case "autocube_convertmaterial":
                                    if (!A_Handler.AutoCube.ConvertMaterial.IsConvertingMaterial)
                                    {
                                        // normal, magic, rare
                                        A_Handler.AutoCube.ConvertMaterial.DoConvert(Properties.Settings.Default.ConvertMaterialFrom, Properties.Settings.Default.ConvertMaterialTo);
                                    }
                                    break;

                            }

                            A_Collection.Hotkeys.lastprocessedHotkey = hotkey.Key.Key;
                            lastHotkeyProcess = DateTime.Now;
                        }


                    }

                }
            }
            catch (Exception e)
            {
                A_Handler.Log.Exception.addExceptionLogEntry(e, A_Enums.ExceptionThread.ECollector);
            }
        }


        private static void ResetLastProcessedHotkey()
        {
            try
            {
                if(lastHotkeyProcess.AddMilliseconds(300) <= DateTime.Now)
                {
                    A_Collection.Hotkeys.lastprocessedHotkey = new Key();
                }
            }
            catch (Exception e)
            {
                A_Handler.Log.Exception.addExceptionLogEntry(e, A_Enums.ExceptionThread.ECollector);
            }
        }


        private static void get_PressedIngameKeys()
        {
            try
            {

                A_Collection.Hotkeys.IngameKeys.IsForceStandStill = 
                    A_Collection.Hotkeys._PressedKeys.Contains(A_Tools.InputSimulator.IS_Keyboard.convert_KeyToSlimDxKey(A_Collection.Preferences.Hotkeys.Key1_ForceStandStill)) ||
                    A_Collection.Hotkeys._PressedKeys.Contains(A_Tools.InputSimulator.IS_Keyboard.convert_KeyToSlimDxKey(A_Collection.Preferences.Hotkeys.Key2_ForceStandStill));

                Key key_1 = A_Tools.InputSimulator.IS_Keyboard.convert_KeyToSlimDxKey(A_Collection.Preferences.Hotkeys.Key1_ForceMove);
                Key key_2 = A_Tools.InputSimulator.IS_Keyboard.convert_KeyToSlimDxKey(A_Collection.Preferences.Hotkeys.Key2_ForceMove);
                if(key_1 == Key.Unknown && key_2 == Key.Unknown)
                {

                    if(A_Collection.Hotkeys._pressedMouseButtons.Count > 0)
                    {

                        //try mouse read
                        MouseObject m_1 =  A_Tools.InputSimulator.IS_Keyboard.convert_KeyToMouseObject(A_Collection.Preferences.Hotkeys.Key1_ForceMove);
                        MouseObject m_2 = A_Tools.InputSimulator.IS_Keyboard.convert_KeyToMouseObject(A_Collection.Preferences.Hotkeys.Key1_ForceMove);

                        A_Collection.Hotkeys.IngameKeys.isForceMove = A_Collection.Hotkeys._pressedMouseButtons.Contains(m_1) || A_Collection.Hotkeys._pressedMouseButtons.Contains(m_2);

                        //if (A_Collection.Hotkeys.IngameKeys.isForceMove)
                        //    System.Windows.MessageBox.Show("Force Move pressed!");

                    }

                }
                else
                {
                    A_Collection.Hotkeys.IngameKeys.isForceMove =
                        A_Collection.Hotkeys._PressedKeys.Contains(key_1) ||
                        A_Collection.Hotkeys._PressedKeys.Contains(key_2);

                }

                


                A_Collection.Hotkeys.IngameKeys.IsTownPortal =
                    A_Collection.Hotkeys._PressedKeys.Contains(A_Tools.InputSimulator.IS_Keyboard.convert_KeyToSlimDxKey(A_Collection.Preferences.Hotkeys.Key1_Townportal)) ||
                    A_Collection.Hotkeys._PressedKeys.Contains(A_Tools.InputSimulator.IS_Keyboard.convert_KeyToSlimDxKey(A_Collection.Preferences.Hotkeys.Key2_Townportal));
            }
            catch (Exception e)
            {
                A_Handler.Log.Exception.addExceptionLogEntry(e, A_Enums.ExceptionThread.ECollector);
            }
        }
    }
}
