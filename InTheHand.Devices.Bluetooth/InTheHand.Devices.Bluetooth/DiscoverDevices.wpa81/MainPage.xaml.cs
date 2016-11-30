//-----------------------------------------------------------------------
// <copyright file="MainPage.xaml.cs" company="In The Hand Ltd">
//   32feet.NET - Personal Area Networking for .NET
//   This source code is licensed under the Microsoft Public License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using InTheHand.Devices.Enumeration;
using System;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.Rfcomm;
using Windows.Devices.Enumeration;
using Windows.Foundation;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using InTheHand.Devices.Bluetooth.Sdp;

namespace SelectDevice.wpa81
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            string aqs = RfcommDeviceService.GetDeviceSelector(RfcommServiceId.ObexObjectPush);

            DevicePicker picker = new DevicePicker();
            picker.Appearance.BackgroundColor = Color.FromArgb(0xff, 0, 0x33, 0x33);
            picker.Appearance.ForegroundColor = Colors.White;
            picker.Appearance.AccentColor = Colors.Goldenrod;

            // add our query string
            picker.Filter.SupportedDeviceSelectors.Add(aqs);

            // prompt user to select a single device
            DeviceInformation dev = await picker.PickSingleDeviceAsync(new Rect());
            if (dev != null)
            {
                // if a device is selected create a BluetoothDevice instance to get more information
                BluetoothDevice bd = await BluetoothDevice.FromIdAsync(dev.Id);
                foreach (IBuffer r in bd.SdpRecords)
                {
                    if (r.Length > 0)
                    {
                        SdpDataElement record = SdpDataElement.FromByteArray(r.ToArray());
                        if (record.Value != null)
                        {
                            string s = record.ToString();
                            System.Diagnostics.Debug.WriteLine(s);
                        }
                    }
                }
                // set data-binding so that device properties are displayed
                DeviceInformationPanel.DataContext = bd;
            }
        }
    }
}
