using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ProtoBuf;

namespace D3Helper.A_Enums
{
    [ProtoContract]
    public enum ConditionValueName
    {
        PowerSNO = 0,
        Distance = 1,
        Value = 2,
        Bool = 3,
        AttribID = 4,

    }
}
