using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InTheHand.Devices.Bluetooth.Sdp
{
    public abstract class SdpAlternative : List<SdpDataElement>
    {
        public byte[] ToByteArray()
        {
            MemoryStream children = new MemoryStream();
            foreach (SdpDataElement element in this)
            {
                byte[] elementBytes = element.ToByteArray();
                children.Write(elementBytes, 0, elementBytes.Length);
            }

            byte[] childBytes = children.ToArray();


            MemoryStream s = new MemoryStream();

            if (childBytes.Length > ushort.MaxValue)
            {
                s.WriteByte((byte)SdpDataElementType.HugeAlternative);
                s.Write(BitConverter.GetBytes((uint)childBytes.Length), 0, 4);
            }
            else if (childBytes.Length > 256)
            {
                s.WriteByte((byte)SdpDataElementType.LongAlternative);
                s.Write(BitConverter.GetBytes((ushort)childBytes.Length), 0, 2);
            }
            else
            {
                s.WriteByte((byte)SdpDataElementType.Alternative);
                s.WriteByte((byte)childBytes.Length);
            }

            s.Write(childBytes, 0, childBytes.Length);

            return s.ToArray();
        }
    }
    public sealed class SdpSequence : List<SdpDataElement>
    {
        public byte[] ToByteArray()
        {
            MemoryStream children = new MemoryStream();
            foreach(SdpDataElement element in this)
            {
                byte[] elementBytes = element.ToByteArray();
                children.Write(elementBytes, 0, elementBytes.Length);
            }

            byte[] childBytes = children.ToArray();

            
            MemoryStream s = new MemoryStream();
            
            if (childBytes.Length > ushort.MaxValue)
            {
                s.WriteByte((byte)SdpDataElementType.HugeSequence);
                s.Write(BitConverter.GetBytes((uint)childBytes.Length), 0, 4);
            }
            else if (childBytes.Length > 256)
            {
                s.WriteByte((byte)SdpDataElementType.LongSequence);
                s.Write(BitConverter.GetBytes((ushort)childBytes.Length), 0, 2);
            }
            else
            {
                s.WriteByte((byte)SdpDataElementType.Sequence);
                s.WriteByte((byte)childBytes.Length);
            }

            s.Write(childBytes, 0, childBytes.Length);

            return s.ToArray();
        }
    }
}
