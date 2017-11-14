using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SlimDX.DirectInput;

namespace D3Helper.A_Collection
{
    public class Hotkey
    {
        public Hotkey(Key key, List<Key> modifiers)
        {
            this.Key = key;
            this.Modifiers = modifiers;
        }
        public Key Key { get; set; }
        public List<Key> Modifiers { get; set; }
    }
    public class Hotkeys
    {
        public static SlimDX.DirectInput.Key lastprocessedHotkey = new SlimDX.DirectInput.Key();

        public static List<Key> _PressedKeys = new List<Key>();

        public static List<MouseObject> _pressedMouseButtons = new List<MouseObject>();

        public static Dictionary<Hotkey, string> D3Helper_Hotkeys = new Dictionary<Hotkey, string>();

        public class IngameKeys
        {
            public static bool IsForceStandStill;
            public static bool IsTownPortal;
            public static bool isForceMove;
        }
    }
}
