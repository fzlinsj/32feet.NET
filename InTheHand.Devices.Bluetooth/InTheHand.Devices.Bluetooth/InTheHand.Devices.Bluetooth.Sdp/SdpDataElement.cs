// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SdpDataElement.cs" company="In The Hand Ltd">
//   32feet.NET - Personal Area Networking for .NET
//   This source code is licensed under the Microsoft Public License - see License.txt
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using InTheHand.Net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace InTheHand.Devices.Bluetooth.Sdp
{
    /// <summary>
    /// Represents an individual item of data within a service record.
    /// </summary>
    public class SdpDataElement
    {
        private SdpDataElementType _type;
        private object _value;

        private SdpDataElement(SdpDataElementType type, object value)
        {
            this._type = type;
            this._value = value;
        }

        private SdpDataElement()
        {
            _type = SdpDataElementType.Nil;
        }

        public static SdpDataElement FromByteArray(byte[] rawData)
        {
            if(rawData == null)
            {
                throw new ArgumentNullException("rawData");
            }
            if(rawData.Length == 0)
            {
                throw new ArgumentException("rawData");
            }

            int offset = 0;
            return FromByteArray(rawData, ref offset, rawData.Length);
        }

        private static SdpDataElement FromByteArray(byte[] rawData, ref int offset, int len)
        {
            SdpDataElement element = new SdpDataElement();
            element._type =(SdpDataElementType)rawData[offset];

            switch (element._type)
            {
                case SdpDataElementType.Boolean:
                    element._value = false;
                    offset += 1;
                    break;
                case SdpDataElementType.BooleanTrue:
                    element._value = true;
                    element._type = SdpDataElementType.Boolean;
                    offset += 1;
                    break;
                case SdpDataElementType.Byte:
                    element._value = rawData[offset + 1];
                    offset += 2;
                    break;
                case SdpDataElementType.Uuid16:
                case SdpDataElementType.UInt16:                   
                    element._value = BitConverter.ToUInt16(rawData, offset + 1);
                    offset += 3;
                    break;
                case SdpDataElementType.UInt32:
                case SdpDataElementType.Uuid32:
                    element._value = BitConverter.ToUInt32(rawData, offset + 1);
                    offset += 5;
                    break;
                case SdpDataElementType.UInt64:
                    element._value = BitConverter.ToUInt64(rawData, offset + 1);
                    offset += 9;
                    break;
                case SdpDataElementType.UInt128:
                case SdpDataElementType.Int128:
                case SdpDataElementType.Uuid128:
                    byte[] guidBytes = new byte[16];
                    Buffer.BlockCopy(rawData, offset + 1, guidBytes, 0, 16);
                    element._value = new Guid(guidBytes);
                    offset += 17;
                    break;
                case SdpDataElementType.SByte:
                    element._value = Convert.ToSByte(rawData[offset + 1]);
                    offset += 2;
                    break;
                case SdpDataElementType.Int16:
                    element._value = BitConverter.ToInt16(rawData, offset + 1);
                    offset += 3;
                    break;
                case SdpDataElementType.Int32:
                    element._value = BitConverter.ToInt32(rawData, offset + 1);
                    offset += 5;
                    break;
                case SdpDataElementType.Int64:
                    element._value = BitConverter.ToInt64(rawData, offset + 1);
                    offset += 9;
                    break;

                case SdpDataElementType.Alternative:
                case SdpDataElementType.Sequence:
                    byte qlen = rawData[offset + 1];
                    int soffset = offset + 2;
                    int end = soffset + qlen;
                    List<SdpDataElement> elements = new List<SdpDataElement> { SdpDataElement.FromByteArray(rawData, ref soffset, end) };
                    while(soffset < end)
                    {
                        elements.Add(SdpDataElement.FromByteArray(rawData, ref soffset, end));
                    }
                    element._value = elements;
                    offset += (2 + qlen);
                    break;

                case SdpDataElementType.String:
                    byte slen = rawData[offset + 1];
                    string sval = System.Text.Encoding.UTF8.GetString(rawData, offset + 2, slen);
                    element._value = sval;
                    offset += (2 + slen);
                    break;

                case SdpDataElementType.Uri:
                    byte ulen = rawData[offset + 1];
                    string uval = System.Text.Encoding.UTF8.GetString(rawData, offset + 2, ulen);
                    element._value = new Uri(uval);
                    offset += (2 + ulen);
                    break;

                case SdpDataElementType.LongAlternative:
                case SdpDataElementType.LongSequence:
                    ushort lqlen = BitConverter.ToUInt16(rawData, offset + 1);
                    int lsoffset = offset + 3;
                    int lend = lsoffset + lqlen;
                    List<SdpDataElement> lelements = new List<SdpDataElement> { SdpDataElement.FromByteArray(rawData, ref lsoffset, lend) };
                    while(lsoffset < lend)
                    {
                        lelements.Add(SdpDataElement.FromByteArray(rawData, ref lsoffset, lend));
                    }
                    element._value = lelements;
                    offset += (3 + lqlen);
                    break;

                case SdpDataElementType.LongString:
                    ushort lslen = BitConverter.ToUInt16(rawData, offset + 1);
                    string lsval = System.Text.Encoding.UTF8.GetString(rawData, offset + 3, lslen);
                    element._value = lsval;
                    offset += (3 + lslen);
                    break;

                case SdpDataElementType.LongUri:
                    ushort lulen = BitConverter.ToUInt16(rawData, offset + 1);
                    string luval = System.Text.Encoding.UTF8.GetString(rawData, offset + 3, lulen);
                    element._value = new Uri(luval);
                    offset += (3 + lulen);
                    break;

                case SdpDataElementType.HugeAlternative:
                case SdpDataElementType.HugeSequence:
                    int hqlen = BitConverter.ToInt32(rawData, offset + 1);
                    int hsoffset = offset + 5;
                    int hend = hsoffset + hqlen;
                    List<SdpDataElement> helements = new List<SdpDataElement> { SdpDataElement.FromByteArray(rawData, ref hsoffset, hend) };
                    while(hsoffset < hend)
                    {
                        helements.Add(SdpDataElement.FromByteArray(rawData, ref hsoffset, hend));
                    }
                    element._value = helements;
                    offset += (5 + hqlen);
                    break;

                case SdpDataElementType.HugeString:
                    int hslen = BitConverter.ToInt32(rawData, offset + 1);
                    string hsval = System.Text.Encoding.UTF8.GetString(rawData, offset + 5, hslen);
                    element._value = hsval;
                    offset += (5 + hslen);
                    break;

                case SdpDataElementType.HugeUri:
                    int hulen = BitConverter.ToInt32(rawData, offset + 1);
                    string huval = System.Text.Encoding.UTF8.GetString(rawData, offset + 5, hulen);
                    element._value = new Uri(huval);
                    offset += (5 + hulen);
                    break;
            }

            return element;
        }

        public static SdpDataElement CreateUuid16(ushort value)
        {
            SdpDataElement e = new SdpDataElement(value);
            e._type = SdpDataElementType.Uuid16;
            return e;
        }

        public static SdpDataElement CreateAlternative(IEnumerable<SdpDataElement> value)
        {
            SdpDataElement e = new SdpDataElement(value);
            e._type = SdpDataElementType.Alternative;
            return e;
        }

        public SdpDataElement(object value)
        {
            if(value == null)
            {
                _type = SdpDataElementType.Nil;
            }
            else
            {
                _value = value;

                if(value is bool)
                {
                    _type = SdpDataElementType.Boolean;
                }
                else if(value is byte)
                {
                    _type = SdpDataElementType.Byte;
                }
                else if(value is ushort)
                {
                    _type = SdpDataElementType.UInt16;
                }
                else if(value is uint)
                {
                    _type = SdpDataElementType.UInt32;
                }
                else if(value is ulong)
                {
                    _type = SdpDataElementType.UInt64;
                }
                else if(value is sbyte)
                {
                    _type = SdpDataElementType.SByte;
                }
                else if (value is short)
                {
                    _type = SdpDataElementType.Int16;
                }
                else if (value is int)
                {
                    _type = SdpDataElementType.Int32;
                }
                else if (value is long)
                {
                    _type = SdpDataElementType.Int64;
                }
                else if(value is Guid)
                {
                    _type = SdpDataElementType.Uuid128;
                }
                else if(value is string)
                {
                    _type = SdpDataElementType.String;
                }
                else if(value is IEnumerable<SdpDataElement>)
                {
                    _type = SdpDataElementType.Sequence;
                }
                else if (value is Uri)
                {
                    _type = SdpDataElementType.Uri;
                }
                else
                {
                    throw new ArgumentException("value is not an expected type");
                }
            }
        }

        public byte[] ToByteArray()
        {
            MemoryStream s = new MemoryStream();
            if (_type == SdpDataElementType.Boolean)
            {
                s.WriteByte((byte)((byte)_type & ((bool)_value ? 0x1 : 0)));
            }
            else if(_type == SdpDataElementType.String)
            {
                byte[] stringBytes = System.Text.Encoding.UTF8.GetBytes(_value.ToString());

                if (stringBytes.Length > ushort.MaxValue)
                {
                    s.WriteByte((byte)SdpDataElementType.HugeString);
                    byte[] sl32 = BitConverter.GetBytes((uint)stringBytes.Length);
                    Array.Reverse(sl32);
                    s.Write(sl32, 0, 4);
                }
                else if (stringBytes.Length > 256)
                {
                    s.WriteByte((byte)SdpDataElementType.LongString);
                    byte[] sl16 = BitConverter.GetBytes((ushort)stringBytes.Length);
                    Array.Reverse(sl16);
                    s.Write(sl16, 0, 2);
                }
                else
                {
                    s.WriteByte((byte)SdpDataElementType.String);
                    s.WriteByte((byte)stringBytes.Length);
                }
                s.Write(stringBytes, 0, stringBytes.Length);
            }
            else if (_type == SdpDataElementType.Uri)
            {
                byte[] stringBytes = System.Text.Encoding.UTF8.GetBytes(((Uri)_value).ToString());

                if (stringBytes.Length > ushort.MaxValue)
                {
                    s.WriteByte((byte)SdpDataElementType.HugeUri);
                    byte[] ul32 = BitConverter.GetBytes((uint)stringBytes.Length);
                    Array.Reverse(ul32);
                    s.Write(ul32, 0, 4);
                }
                else if (stringBytes.Length > 256)
                {
                    s.WriteByte((byte)SdpDataElementType.LongUri);
                    byte[] ul16 = BitConverter.GetBytes((ushort)stringBytes.Length);
                    Array.Reverse(ul16);
                    s.Write(ul16, 0, 2);
                }
                else
                {
                    s.WriteByte((byte)SdpDataElementType.Uri);
                    s.WriteByte((byte)stringBytes.Length);
                }
                s.Write(stringBytes, 0, stringBytes.Length);
            }
            else if(_type == SdpDataElementType.Sequence)
            {
                byte[] childBytes = GetChildBytes();


                if (childBytes.Length > ushort.MaxValue)
                {
                    s.WriteByte((byte)SdpDataElementType.HugeSequence);
                    byte[] sql32 = BitConverter.GetBytes((uint)childBytes.Length);
                    Array.Reverse(sql32);
                    s.Write(sql32, 0, 4);
                }
                else if (childBytes.Length > 256)
                {
                    s.WriteByte((byte)SdpDataElementType.LongSequence);
                    byte[] sql16 = BitConverter.GetBytes((ushort)childBytes.Length);
                    Array.Reverse(sql16);
                    s.Write(sql16, 0, 2);
                }
                else
                {
                    s.WriteByte((byte)SdpDataElementType.Sequence);
                    s.WriteByte((byte)childBytes.Length);
                }

                s.Write(childBytes, 0, childBytes.Length);
            }
            else if (_type == SdpDataElementType.Alternative)
            {
                byte[] childBytes = GetChildBytes();


                if (childBytes.Length > ushort.MaxValue)
                {
                    s.WriteByte((byte)SdpDataElementType.HugeAlternative);
                    byte[] al32 = BitConverter.GetBytes((uint)childBytes.Length);
                    Array.Reverse(al32);
                    s.Write(al32, 0, 4);
                }
                else if (childBytes.Length > 256)
                {
                    s.WriteByte((byte)SdpDataElementType.LongAlternative);
                    byte[] al16 = BitConverter.GetBytes((ushort)childBytes.Length);
                    Array.Reverse(al16);
                    s.Write(al16, 0, 2);
                }
                else
                {
                    s.WriteByte((byte)SdpDataElementType.Alternative);
                    s.WriteByte((byte)childBytes.Length);
                }

                s.Write(childBytes, 0, childBytes.Length);
            }
            else
            {
                s.WriteByte((byte)_type);

                switch (_type)
                {
                    case SdpDataElementType.Byte:
                        s.WriteByte((byte)_value);
                        break;
                    case SdpDataElementType.UInt16:
                        byte[] u16 = BitConverter.GetBytes((ushort)_value);
                        Array.Reverse(u16);
                        s.Write(u16, 0, 2);
                        break;
                    case SdpDataElementType.UInt32:
                        byte[] u32 = BitConverter.GetBytes((uint)_value);
                        Array.Reverse(u32);
                        s.Write(u32, 0, 4);
                        break;
                    case SdpDataElementType.UInt64:
                        byte[] u64 = BitConverter.GetBytes((ulong)_value);
                        Array.Reverse(u64);
                        s.Write(u64, 0, 8);
                        break;
                    case SdpDataElementType.UInt128:
                        byte[] u128 = ((Guid)_value).ToByteArray();
                        Array.Reverse(u128);
                        s.Write(u128, 0, 16);
                        break;

                    case SdpDataElementType.SByte:
                        s.Write(BitConverter.GetBytes((sbyte)_value), 0, 1);
                        break;
                    case SdpDataElementType.Int16:
                        byte[] i16 = BitConverter.GetBytes((short)_value);
                        Array.Reverse(i16);
                        s.Write(i16, 0, 2);
                        break;
                    case SdpDataElementType.Int32:
                        byte[] i32 = BitConverter.GetBytes((int)_value);
                        Array.Reverse(i32);
                        s.Write(i32, 0, 4);
                        break;
                    case SdpDataElementType.Int64:
                        byte[] i64 = BitConverter.GetBytes((long)_value);
                        Array.Reverse(i64);
                        s.Write(i64, 0, 8);
                        break;
                    case SdpDataElementType.Int128:
                        byte[] i128 = ((Guid)_value).ToByteArray();
                        Array.Reverse(i128);
                        s.Write(i128, 0, 16);
                        break;

                    case SdpDataElementType.Uuid16:
                        byte[] uu16 = BitConverter.GetBytes((ushort)_value);
                        Array.Reverse(uu16);
                        s.Write(uu16, 0, 2);
                        break;
                    case SdpDataElementType.Uuid32:
                        byte[] uu32 = BitConverter.GetBytes((uint)_value);
                        Array.Reverse(uu32);
                        s.Write(uu32, 0, 4);
                        break;
                    case SdpDataElementType.Uuid128:
                        byte[] uu128 = ((Guid)_value).ToByteArray();
                        Array.Reverse(uu128);
                        s.Write(uu128, 0, 16);
                        break;

                        
                }
            }
            return s.ToArray();
        }

        private byte[] GetChildBytes()
        {
            MemoryStream children = new MemoryStream();
            foreach (SdpDataElement element in (IEnumerable<SdpDataElement>)_value)
            {
                byte[] elementBytes = element.ToByteArray();
                children.Write(elementBytes, 0, elementBytes.Length);
            }

            return children.ToArray();
        }

        public object Value
        {
            get
            {
                return _value;
            }
        }

        public override string ToString()
        {
            switch(_type)
            {
                case SdpDataElementType.Nil:
                    return "{}";

                case SdpDataElementType.Sequence:
                case SdpDataElementType.LongSequence:
                case SdpDataElementType.HugeSequence:
                    return "{\"type\"=\"Sequence\",\"value\"=" + OutputAllElements(Value as IEnumerable<SdpDataElement>) + "}";

                case SdpDataElementType.Alternative:
                case SdpDataElementType.LongAlternative:
                case SdpDataElementType.HugeAlternative:
                    return "{\"type\"=\"Sequence\",\"value\"=" + OutputAllElements(Value as IEnumerable<SdpDataElement>) + "}";

                case SdpDataElementType.String:
                case SdpDataElementType.LongString:
                case SdpDataElementType.HugeString:
                    return "{\"type\"=\"String\",\"value\"=" + Value.ToString() + "\"}";

                case SdpDataElementType.Uri:
                case SdpDataElementType.LongUri:
                case SdpDataElementType.HugeUri:
                    return "{\"type\"=\"Uri\",\"value\"=" + Value.ToString() + "\"}";

                default:
                    return "{\"type\"=\"" + _type.ToString() + "\",\"value\"=" + Value.ToString() + "}";
            }
        }

        private static string OutputAllElements(IEnumerable<SdpDataElement> elements)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            foreach (SdpDataElement element in elements)
            {
                sb.Append(element.ToString() + ",");
            }

            if (sb.Length > 1)
            {
                sb.Length -= 1;
            }
            sb.Append("]");

            return sb.ToString();
        }
    }
}
