using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ProtoBuf;

namespace D3Helper.A_Enums
{
    [ProtoContract]
    public enum DamageType
    {
        none = -1,
        Physical = 0,
        Fire = 1,
        Lightning = 2,
        Cold = 3,
        Poison = 4,
        Arcane = 5,
        Holy = 6
    }
}
