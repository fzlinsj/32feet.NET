// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BluetoothService.cs" company="In The Hand Ltd">
//   32feet.NET - Personal Area Networking for .NET
//   This source code is licensed under the In The Hand Community License - see License.txt
// </copyright>
// <summary>
//   Standard Bluetooth profile identifiers.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace InTheHand.Phone.Bluetooth
{
    using System;

    /// <summary>
    /// Standard Bluetooth profile identifiers.
    /// </summary>
    public static class BluetoothService
    {
        /// <summary>
        /// Provides a basic Serial emulation connect over Bluetooth. [0x1101]
        /// </summary>
        public static readonly Guid SerialPort = new Guid(0x00001101, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        /// <summary>
        /// Used for sending binary objects between devices.[0x1105]
        /// </summary>
        public static readonly Guid ObexObjectPush = new Guid(0x00001105, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        /// <summary>
        /// OBEX version of an FTP server [0x1106]
        /// </summary>
        public static readonly Guid ObexFileTransfer = new Guid(0x00001106, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);    
    }
}
