// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SdpUniversalAttributes.cs" company="In The Hand Ltd">
//   32feet.NET - Personal Area Networking for .NET
//   This source code is licensed under the Microsoft Public License - see License.txt
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace InTheHand.Devices.Bluetooth.Sdp
{
    public static class SdpUniversalAttributes
    {
        /// <summary>
        /// Service Record Handle (Allocated by the host system).
        /// </summary>
        /// <remarks>
        /// A service record handle is a 32-bit number that uniquely identifies each service record within an SDP server.
        /// It is important to note that, in general, each handle is unique only within each SDP server.</remarks>
        public static ushort ServiceRecordHandle = 0x0;

        /// <summary>
        /// The ServiceClassIDList attribute consists of a data element sequence in which each data element is a UUID representing the service classes that a given service record conforms to.
        /// </summary>
        public static ushort ServiceClassIDList = 0x1;

        /// <summary>
        /// The ServiceRecordState is a 32-bit integer that is used to facilitate caching of Service Attributes.
        /// </summary>
        /// <remarks> If this attribute is contained in a service record, its value shall be changed when any other attribute value is added to, deleted from or changed within the service record.
        /// This permits a client to check the value of this single attribute.
        /// If its value has not changed since it was last checked, the client knows that no other attribute values within the service record have changed.</remarks>
        public static ushort ServiceRecordState = 0x2;

        /// <summary>
        /// The ServiceID is a UUID that universally and uniquely identifies the service instance described by the service record.
        /// </summary>
        public static ushort ServiceID = 0x3;

        /// <summary>
        /// The ProtocolDescriptorList attribute describes one or more protocol stacks that can be used to gain access to the service described by the service record.
        /// </summary>
        public static ushort ProtocolDescriptorList = 0x4;


        public static ushort AdditionalProtocolDescriptorList = 0xD;
        public static ushort BrowseGroupList = 0x5;
        public static ushort LanguageBaseAttributeIDList = 0x6;
        public static ushort ServiceInfoTimeToLive = 0x7;
        public static ushort ServiceAvailability = 0x0008;
        public static ushort BluetoothProfileDescriptorList = 0x0009;
        public static ushort DocumentationURL = 0x000A;
        public static ushort ClientExecutableURL = 0x000B;
        public static ushort IconURL = 0x000C;
    }
}
