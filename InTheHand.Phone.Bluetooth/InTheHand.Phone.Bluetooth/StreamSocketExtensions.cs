// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StreamSocketExtensions.cs" company="In The Hand Ltd">
//   32feet.NET - Personal Area Networking for .NET
//   This source code is licensed under the In The Hand Community License - see License.txt
// </copyright>
// <summary>
//   Extension methods to support Bluetooth connections.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace InTheHand.Phone.Bluetooth
{
    using System;
    using System.Threading.Tasks;
    using Windows.Networking;
    using Windows.Networking.Sockets;

    /// <summary>
    /// Extension methods to support Bluetooth connections.
    /// </summary>
    public static class StreamSocketExtensions
    {
        /// <summary>
        /// Starts an asynchronous operation on a StreamSocket object to connect to a remote network destination specified by a remote hostname and a remote Bluetooth service ID.
        /// </summary>
        /// <param name="s">The Socket.</param>
        /// <param name="remoteHostName">The Bluetooth address of the remote device.</param>
        /// <param name="serviceId">The Bluetooth Service.</param>
        /// <returns>An asynchronous connect operation on a StreamSocket object.</returns>
        public static Windows.Foundation.IAsyncAction ConnectAsync(this StreamSocket s, HostName remoteHostName, InTheHand.Devices.Bluetooth.Rfcomm.RfcommServiceId serviceId)
        {
            return s.ConnectAsync(remoteHostName, serviceId.AsString());
        }

        /// <summary>
        /// Starts an asynchronous operation on a StreamSocket object to connect to a remote network destination specified by a remote hostname and a remote Bluetooth service UUID.
        /// </summary>
        /// <param name="s">The Socket.</param>
        /// <param name="remoteHostName">The Bluetooth address of the remote device.</param>
        /// <param name="serviceGuid">The Bluetooth Service.</param>
        /// <returns>An asynchronous connect operation on a StreamSocket object.</returns>
        public static Windows.Foundation.IAsyncAction ConnectAsync(this StreamSocket s, HostName remoteHostName, Guid serviceGuid)
        {
            return s.ConnectAsync(remoteHostName, serviceGuid.ToString("B"));
        }

        /// <summary>
        /// Starts an asynchronous operation on a StreamSocket object to connect to a remote network destination specified by a remote hostname and a remote Bluetooth port.
        /// </summary>
        /// <param name="s">The Socket.</param>
        /// <param name="remoteHostName">The Bluetooth address of the remote device.</param>
        /// <param name="port">The Bluetooth Port.</param>
        /// <returns>An asynchronous connect operation on a StreamSocket object.</returns>
        public static Windows.Foundation.IAsyncAction ConnectAsync(this StreamSocket s, HostName remoteHostName, int port)
        {
            return s.ConnectAsync(remoteHostName, port.ToString(System.Globalization.CultureInfo.InvariantCulture));
        }
    }
}
