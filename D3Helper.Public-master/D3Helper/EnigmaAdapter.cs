using Enigma.D3.UI.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D3Helper
{
	internal static class EnigmaAdapter
	{
		public static int GetPowerSnoId(this UXIcon icon)
		{
            //return icon.Read<int>(0x166C); // <= patch 2.5.x

            return icon.Read<int>(0x1670); // <= patch 2.6.x

            ////dirty trick to find offset
            //int offset = 5744;
            //while(134872 != icon.Read<int>(offset)) //archon sno id
            //{
            //    offset++;
            //}

            //return offset;

		}
	}
}
