// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RfcommStreamSocketListener.cs" company="In The Hand Ltd">
//   32feet.NET - Personal Area Networking for .NET
//   This source code is licensed under the In The Hand Community License - see License.txt
// </copyright>
// <summary>
//   Supports listening for an incoming network connection using a Bluetooth stream socket.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using Windows.Networking.Proximity;
using Windows.Networking.Sockets;

namespace InTheHand.Networking.Sockets
{
    /// <summary>
    /// Supports listening for an incoming network connection using a Bluetooth stream socket.
    /// </summary>
    public sealed class RfcommStreamSocketListener
    {
        private StreamSocket socket;

        /// <summary>
        /// An event that indicates that a connection was received on the StreamSocketListener object.
        /// </summary>
        public event Windows.Foundation.TypedEventHandler<RfcommStreamSocketListener, RfcommStreamSocketListenerConnectionReceivedEventArgs> ConnectionReceived;

        /// <summary>
        /// Creates a new RfcommStreamSocketListener object.
        /// </summary>
        public RfcommStreamSocketListener()
        {
            PeerFinder.ConnectionRequested += PeerFinder_ConnectionRequested;
        }

        private async void PeerFinder_ConnectionRequested(object sender, ConnectionRequestedEventArgs args)
        {
            PeerInformation info = args.PeerInformation;
            socket = await PeerFinder.ConnectAsync(info);

            if (ConnectionReceived != null)
            {
                ConnectionReceived(this, new RfcommStreamSocketListenerConnectionReceivedEventArgs(socket));
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class RfcommStreamSocketListenerConnectionReceivedEventArgs
    {
        internal RfcommStreamSocketListenerConnectionReceivedEventArgs(StreamSocket socket)
        {
            this.Socket = socket;
        }

        /// <summary>
        /// The StreamSocket object created when a connection is received by the StreamSocketListener object.
        /// </summary>
        public Windows.Networking.Sockets.StreamSocket Socket { get; private set; }
    }
}
