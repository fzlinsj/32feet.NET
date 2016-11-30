// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RfcommDeviceService.cs" company="In The Hand Ltd">
//   32feet.NET - Personal Area Networking for .NET
//   This source code is licensed under the In The Hand Community License - see License.txt
// </copyright>
// <summary>
//   Represents an instance of a service on a Bluetooth BR device.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using Windows.Networking;

namespace InTheHand.Devices.Bluetooth.Rfcomm
{
    /// <summary>
    /// Represents an instance of a service on a Bluetooth BR device.
    /// </summary>
    public sealed class RfcommDeviceService
    {
        /// <summary>
        /// Gets the HostName object of the RFCOMM service instance, which is used to connect to the remote device.
        /// </summary>
        public HostName ConnectionHostName { get; private set; }

        /// <summary>
        /// Gets the service name of the RFCOMM service instance, which is used to connect to the remote device.
        /// </summary>
        public string ConnectionServiceName
        {
            get
            {
                return ServiceId.AsString();
            }
        }

        /// <summary>
        /// Gets the RfcommServiceId of this RFCOMM service instance.
        /// </summary>
        public RfcommServiceId ServiceId { get; private set; }

    }
}
