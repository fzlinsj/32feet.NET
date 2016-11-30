// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RfcommServiceProvider.cs" company="In The Hand Ltd">
//   32feet.NET - Personal Area Networking for .NET
//   This source code is licensed under the In The Hand Community License - see License.txt
// </copyright>
// <summary>
//   Represents an instance of a local RFCOMM service.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using InTheHand.Networking.Sockets;
using System;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Networking.Proximity;

namespace InTheHand.Devices.Bluetooth.Rfcomm
{
    /// <summary>
    /// Represents an instance of a local RFCOMM service.
    /// </summary>
    public sealed class RfcommServiceProvider
    {
        private RfcommServiceId serviceId;
        private RfcommStreamSocketListener listener;

        /// <summary>
        /// Gets a RfcommServiceProvider object from a DeviceInformation Id for a RFCOMM service instance.
        /// </summary>
        /// <param name="serviceId">The RfcommServiceId to be hosted locally.</param>
        /// <returns>The RfcommServiceProvider object that represents the local RFCOMM service instance.</returns>
        public static IAsyncOperation<RfcommServiceProvider> CreateAsync(RfcommServiceId serviceId)
        {
            return CreateProviderAsync(serviceId).AsAsyncOperation<RfcommServiceProvider>();
        }

        private static async Task<RfcommServiceProvider> CreateProviderAsync(RfcommServiceId serviceId)
        {
            RfcommServiceProvider provider = new RfcommServiceProvider();
            provider.serviceId = serviceId;
            return provider;
        }

        /// <summary>
        /// Begins advertising the SDP attributes.
        /// </summary>
        /// <param name="listener">The <see cref="RfcommStreamSocketListener"/> that is listening for incoming connections.</param>
        public void StartAdvertising(RfcommStreamSocketListener listener)
        {
            PeerFinder.AlternateIdentities["Bluetooth:SDP"] = serviceId.AsString();
            //PeerFinder.Start();
            this.listener = listener;
        }

        /// <summary>
        /// Stops advertising the SDP attributes.
        /// </summary>
        public void StopAdvertising()
        {
            PeerFinder.AlternateIdentities.Remove("Bluetooth:SDP");
            //PeerFinder.Stop();
        }


        private RfcommServiceProvider()
        {
            this.listener = null;
        }

        /// <summary>
        /// 
        /// </summary>
        public RfcommServiceId ServiceId
        {
            get
            {
                return serviceId;
            }
        }


    }
}
