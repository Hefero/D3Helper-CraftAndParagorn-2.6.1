using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Enigma.D3;
using Enigma.D3.UI;
using Enigma.D3.Helpers;
using Enigma.D3.UI.Controls;

namespace D3Helper.A_Collection
{
    class Skills
    {
        

        public class SkillInfos
        {
            public static SkillPower _HotBar1Skill;
            public static SkillPower _HotBar2Skill;
            public static SkillPower _HotBar3Skill;
            public static SkillPower _HotBar4Skill;
            public static SkillPower _HotBarRightClickSkill;
            public static SkillPower _HotBarLeftClickSkill;
            public static SkillPower _EmptySkill = null;
        }
        
        public class UI_Controls
        {
            public static List<UXIcon> SkillControls = new List<UXIcon>();
        }
        
    }
}
