using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InTheHand.Devices.Bluetooth.Sdp
{
    internal enum SdpDataElementType : byte
    {
        Nil = 0,
        Byte = 8,
        UInt16 = 9,
        UInt32 = 0xA,
        UInt64 = 0xB,
        UInt128 = 0xC,

        SByte = 0x10,
        Int16 = 0x11,
        Int32 = 0x12,
        Int64 = 0x13,
        Int128 = 0x14,

        Uuid16 = 0x19,
        Uuid32 = 0x1A,
        Uuid128 = 0x1C,

        String = 0x25,
        LongString = 0x26,
        HugeString = 0x27,

        Boolean = 0x28,

        Sequence = 0x35,
        LongSequence = 0x36,
        HugeSequence = 0x37,

        Alternative = 0x3D,
        LongAlternative = 0x3E,
        HugeAlternative = 0x3F,

        Uri = 0x45,
        LongUri = 0x46,
        HugeUri = 0x47,
    }
}
