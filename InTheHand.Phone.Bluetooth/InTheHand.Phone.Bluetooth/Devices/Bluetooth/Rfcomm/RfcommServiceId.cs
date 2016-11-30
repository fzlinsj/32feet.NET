// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RfcommServiceId.cs" company="In The Hand Ltd">
//   32feet.NET - Personal Area Networking for .NET
//   This source code is licensed under the In The Hand Community License - see License.txt
// </copyright>
// <summary>
//   Represents a RFCOMM service ID.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;

namespace InTheHand.Devices.Bluetooth.Rfcomm
{
    /// <summary>
    /// Represents a RFCOMM service ID.
    /// </summary>
    public sealed class RfcommServiceId
    {
        // Base UUID for all standard services
        private static readonly Guid baseUuid = new Guid(0x0, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        /// <summary>
        /// Creates a RfcommServiceId object corresponding to the service id for the standardized Generic File Transfer service (with short id 0x1202).
        /// </summary>
        public static RfcommServiceId GenericFileTransfer
        {
            get
            {
                return RfcommServiceId.FromShortId(0x1202);
            }
        }

        /// <summary>
        /// Creates a RfcommServiceId object corresponding to the service id for the standardized OBEX File Transfer service (with short id 0x1106).
        /// </summary>
        public static RfcommServiceId ObexFileTransfer
        {
            get
            {
                return RfcommServiceId.FromShortId(0x1106);
            }
        }

        /// <summary>
        /// Creates a RfcommServiceId object corresponding to the service id for the standardized OBEX Object Push service (with short id 0x1105).
        /// </summary>
        public static RfcommServiceId ObexObjectPush
        {
            get
            {
                return RfcommServiceId.FromShortId(0x1105);
            }
        }

        /// <summary>
        /// Creates a RfcommServiceId object corresponding to the service id for the standardized Phone Book Access (PCE) service (with short id 0x112E).
        /// </summary>
        public static RfcommServiceId PhoneBookAccessPce
        {
            get
            {
                return RfcommServiceId.FromShortId(0x112E);
            }
        }

        /// <summary>
        /// Creates a RfcommServiceId object corresponding to the service id for the standardized Phone Book Access (PSE) service (with short id 0x112F).
        /// </summary>
        public static RfcommServiceId PhoneBookAccessPse
        {
            get
            {
                return RfcommServiceId.FromShortId(0x112F);
            }
        }

        /// <summary>
        /// Creates a RfcommServiceId object corresponding to the service id for the standardized Serial Port service (with short id 0x1101).
        /// </summary>
        public static RfcommServiceId SerialPort
        {
            get
            {
                return RfcommServiceId.FromShortId(0x1101);
            }
        }

        private Guid uuid;

        /// <summary>
        /// Creates a RfcommServiceId object from a 32-bit service id.
        /// </summary>
        /// <param name="shortId">The 32-bit service id.</param>
        /// <returns>The RfcommServiceId object.</returns>
        public static RfcommServiceId FromShortId(uint shortId)
        {
            byte[] buffer = new byte[16];
            baseUuid.ToByteArray().CopyTo(buffer, 0);
            BitConverter.GetBytes(shortId).CopyTo(buffer, 0);
            return new RfcommServiceId(new Guid(buffer));
        }

        /// <summary>
        /// Creates a RfcommServiceId object from a 128-bit service id.
        /// </summary>
        /// <param name="uuid">The 128-bit service id.</param>
        /// <returns>The <see cref="RfcommServiceId"/> object.</returns>
        public static RfcommServiceId FromUuid(Guid uuid)
        {
            RfcommServiceId serviceId = new RfcommServiceId(uuid);
            return serviceId;
        }

        private RfcommServiceId(Guid uuid)
        {
            this.uuid = uuid;
        }

        /// <summary>
        /// Retrieves the 128-bit service id.
        /// </summary>
        public Guid Uuid
        {
            get
            {
                return uuid;
            }
        }

        /// <summary>
        /// Converts the <see cref="RfcommServiceId"/> to a 32-bit service id if possible.
        /// </summary>
        /// <returns>Returns the 32-bit service id if the <see cref="RfcommServiceId"/> represents a standardized service.</returns>
        public uint AsShortId()
        {
            byte[] rawUuid = uuid.ToByteArray();
            uint shortId = BitConverter.ToUInt32(rawUuid, 0);
            return shortId;
        }

        /// <summary>
        /// Converts the RfcommServiceId to a string.
        /// </summary>
        /// <returns>Returns the string representation of the 128-bit service id.</returns>
        public string AsString()
        {
            return this.uuid.ToString("B");
        }
    }
}
