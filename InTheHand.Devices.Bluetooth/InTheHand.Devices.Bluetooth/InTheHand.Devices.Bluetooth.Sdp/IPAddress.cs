// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IPAddress.cs" company="In The Hand Ltd">
//   32feet.NET - Personal Area Networking for .NET
//   This source code is licensed under the Microsoft Public License - see License.txt
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace InTheHand.Net
{
    /// <summary>
    /// Helper network-order conversion functions for the Universal Windows Platform.
    /// </summary>
    public static class IPAddress
    {
        /// <summary>
        /// Converts a short value from network byte order to host byte order.
        /// </summary>
        /// <param name="network">The number to convert, expressed in network byte order.</param>
        /// <returns>A short value, expressed in host byte order.</returns>
        public static short NetworkToHostOrder(short network)
        {
            return HostToNetworkOrder(network);
        } 

        /// <summary>
        /// Converts an integer value from network byte order to host byte order.
        /// </summary>
        /// <param name="network">The number to convert, expressed in network byte order. </param>
        /// <returns>An integer value, expressed in host byte order.</returns>
        public static int NetworkToHostOrder(int network)
        {
            return HostToNetworkOrder(network);
        }

        /// <summary>
        /// Converts a long value from network byte order to host byte order.
        /// </summary>
        /// <param name="network">The number to convert, expressed in network byte order.</param>
        /// <returns>A long value, expressed in host byte order.</returns>
        public static long NetworkToHostOrder(long network)
        {
            return HostToNetworkOrder(network);
        }
        

        /// <summary>
        /// Converts a short value from host byte order to network byte order.
        /// </summary>
        /// <param name="host">The number to convert, expressed in host byte order. </param>
        /// <returns>A short value, expressed in network byte order.</returns>
        public static short HostToNetworkOrder(short host)
        {
            return (short)((((int)host & 0xFF) << 8) | (int)((host >> 8) & 0xFF));
        }

        /// <summary>
        /// Converts an integer value from host byte order to network byte order.
        /// </summary>
        /// <param name="host">The number to convert, expressed in host byte order. </param>
        /// <returns>An integer value, expressed in network byte order.</returns>
        public static int HostToNetworkOrder(int host)
        {
            return (((int)HostToNetworkOrder((short)host) & 0xFFFF) << 16)
                    | ((int)HostToNetworkOrder((short)(host >> 16)) & 0xFFFF);
        }

        /// <summary>
        /// Converts a long value from host byte order to network byte order.
        /// </summary>
        /// <param name="host">The number to convert, expressed in host byte order. </param>
        /// <returns>A long value, expressed in network byte order.</returns>
        public static long HostToNetworkOrder(long host)
        {
            return (((long)HostToNetworkOrder((int)host) & 0xFFFFFFFF) << 32)
                    | ((long)HostToNetworkOrder((int)(host >> 32)) & 0xFFFFFFFF);
        }
    }
}
