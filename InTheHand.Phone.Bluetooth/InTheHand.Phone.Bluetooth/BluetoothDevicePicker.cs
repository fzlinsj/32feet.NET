// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BluetoothDevicePicker.cs" company="In The Hand Ltd">
//   32feet.NET - Personal Area Networking for .NET
//   This source code is licensed under the In The Hand Community License - see License.txt
// </copyright>
// <summary>
//   Represents a UI element that lets the user choose a Bluetooth device.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace InTheHand.Devices.Bluetooth
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using Windows.Devices.Enumeration;
    using Windows.Networking.Proximity;

    /// <summary>
    /// Represents a UI element that lets the user choose a Bluetooth device.
    /// </summary>
    public sealed class BluetoothDevicePicker
    {
        private Guid serviceFilter = Guid.Empty;

        /// <summary>
        /// Gets or sets a Guid representing an individual Bluetooth service to filter on.
        /// </summary>
        public Guid ServiceFilter
        {
            get
            {
                return serviceFilter;
            }

            set
            {
                serviceFilter = value;
            }
        }

        /// <summary>
        /// Shows the Bluetooth device picker so that the user can pick one device.
        /// </summary>
        /// <returns>When the call to this method completes successfully, it returns a PeerInformation object that represents the device that the user picked.</returns>
        public async Task<PeerInformation> PickDeviceAsync()
        {
            // replace waithandle - fixes bug with multiple calls (Thanks Phil Allison)
            WaitHandle = new System.Threading.EventWaitHandle(false, System.Threading.EventResetMode.AutoReset);

            SelectedItem = null;

#if V81
            DeviceInformationCollection devices = await DeviceInformation.FindAllAsync(Windows.Devices.Bluetooth.BluetoothDevice.GetDeviceSelector());
            foreach(DeviceInformation info in devices)
            {
                info.
            }
#else
            PeerFinder.AlternateIdentities["Bluetooth:Paired"] = string.Empty;

            // get all service types
            if (this.serviceFilter == Guid.Empty)
            {
                PeerFinder.AlternateIdentities["Bluetooth:SDP"] = string.Empty;
            }
            else
            {
                PeerFinder.AlternateIdentities["Bluetooth:SDP"] = this.serviceFilter.ToString("D");
                PeerFinder.AlternateIdentities.Remove("Bluetooth:Paired");
            }
#endif

            IReadOnlyCollection<PeerInformation> devices = null;

            while (devices == null)
            {
                try
                {
                    devices = await PeerFinder.FindAllPeersAsync();
                }
                catch (Exception ex)
                {
                    if ((uint)ex.HResult == 0x8007048F)
                    {
                        if (MessageBox.Show(Resources.Strings.OpenSettings, Resources.Strings.Bluetooth, MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                        {
                            Microsoft.Phone.Tasks.ConnectionSettingsTask cst = new Microsoft.Phone.Tasks.ConnectionSettingsTask();
                            cst.ConnectionSettingsType = Microsoft.Phone.Tasks.ConnectionSettingsType.Bluetooth;
                            cst.Show();
                        }
                        else
                        {
                            // don't prompt again - return with no device selected
                            return null;
                        }
                    }
                }

            }

            Items = devices;

            Frame f = (Frame)Application.Current.RootVisual;

            f.Navigate(new Uri("/InTheHand.Devices.Bluetooth;component/BluetoothPickerPage.xaml", UriKind.Relative));

            var tcs = new TaskCompletionSource<bool>();
            var rwh = System.Threading.ThreadPool.RegisterWaitForSingleObject(WaitHandle, delegate { tcs.TrySetResult(true); }, null, -1, true);
            var t = tcs.Task;
            await t.ContinueWith(_ => rwh.Unregister(WaitHandle));

            return SelectedItem;
        }

        internal static System.Threading.EventWaitHandle WaitHandle = null;

        internal static IReadOnlyCollection<PeerInformation> Items
        {
            get;
            set;
        }

        internal static PeerInformation SelectedItem
        {
            get;
            set;
        }
    }
}
