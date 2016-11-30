using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace InTheHand.Devices.Bluetooth.Sdp
{
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

        public static SdpDataElement CreateUuid16(ushort value)
        {
            SdpDataElement e = new SdpDataElement(value);
            e._type = SdpDataElementType.Uuid16;
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
                else if(value is SdpAlternative)
                {
                    _type = SdpDataElementType.Alternative;
                }
                else if(value is SdpSequence)
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
                    s.Write(BitConverter.GetBytes((uint)stringBytes.Length), 0, 4);
                }
                else if (stringBytes.Length > 256)
                {
                    s.WriteByte((byte)SdpDataElementType.LongString);
                    s.Write(BitConverter.GetBytes((ushort)stringBytes.Length), 0, 2);
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
                    s.Write(BitConverter.GetBytes((uint)stringBytes.Length), 0, 4);
                }
                else if (stringBytes.Length > 256)
                {
                    s.WriteByte((byte)SdpDataElementType.LongUri);
                    s.Write(BitConverter.GetBytes((ushort)stringBytes.Length), 0, 2);
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
                SdpSequence sequence = (SdpSequence)_value;
                byte[] sequenceBytes = sequence.ToByteArray();
                if (sequenceBytes.Length > ushort.MaxValue)
                {
                    s.WriteByte((byte)SdpDataElementType.HugeSequence);
                    s.Write(BitConverter.GetBytes((uint)sequenceBytes.Length), 0, 4);
                }
                else if (sequenceBytes.Length > 256)
                {
                    s.WriteByte((byte)SdpDataElementType.LongSequence);
                    s.Write(BitConverter.GetBytes((ushort)sequenceBytes.Length), 0, 2);
                }
                else
                {
                    s.WriteByte((byte)SdpDataElementType.Sequence);
                    s.WriteByte((byte)sequenceBytes.Length);
                }
                s.Write(sequenceBytes, 0, sequenceBytes.Length);
            }
            else if (_type == SdpDataElementType.Alternative)
            {
                SdpAlternative alternative = (SdpAlternative)_value;
                byte[] alternativeBytes = alternative.ToByteArray();
                if (alternativeBytes.Length > ushort.MaxValue)
                {
                    s.WriteByte((byte)SdpDataElementType.HugeAlternative);
                    s.Write(BitConverter.GetBytes((uint)alternativeBytes.Length), 0, 4);
                }
                else if (alternativeBytes.Length > 256)
                {
                    s.WriteByte((byte)SdpDataElementType.LongAlternative);
                    s.Write(BitConverter.GetBytes((ushort)alternativeBytes.Length), 0, 2);
                }
                else
                {
                    s.WriteByte((byte)SdpDataElementType.Alternative);
                    s.WriteByte((byte)alternativeBytes.Length);
                }
                s.Write(alternativeBytes, 0, alternativeBytes.Length);
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
                        s.Write(BitConverter.GetBytes((ushort)_value), 0, 2);
                        break;
                    case SdpDataElementType.UInt32:
                        s.Write(BitConverter.GetBytes((uint)_value), 0, 4);
                        break;
                    case SdpDataElementType.UInt64:
                        s.Write(BitConverter.GetBytes((ulong)_value), 0, 8);
                        break;
                    case SdpDataElementType.UInt128:
                        s.Write(((Guid)_value).ToByteArray(), 0, 16);
                        break;

                    case SdpDataElementType.SByte:
                        s.Write(BitConverter.GetBytes((sbyte)_value), 0, 1);
                        break;
                    case SdpDataElementType.Int16:
                        s.Write(BitConverter.GetBytes((short)_value), 0, 2);
                        break;
                    case SdpDataElementType.Int32:
                        s.Write(BitConverter.GetBytes((int)_value), 0, 4);
                        break;
                    case SdpDataElementType.Int64:
                        s.Write(BitConverter.GetBytes((long)_value), 0, 8);
                        break;
                    case SdpDataElementType.Int128:
                        s.Write(((Guid)_value).ToByteArray(), 0, 16);
                        break;

                    case SdpDataElementType.Uuid16:
                        s.Write(BitConverter.GetBytes((ushort)_value), 0, 2);
                        break;
                    case SdpDataElementType.Uuid32:
                        s.Write(BitConverter.GetBytes((uint)_value), 0, 4);
                        break;
                    case SdpDataElementType.Uuid128:
                        s.Write(((Guid)_value).ToByteArray(), 0, 16);
                        break;

                        
                }
            }
            return s.ToArray();
        }

        public object Value
        {
            get
            {
                return _value;
            }
        }
    }
}
