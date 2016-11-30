using InTheHand.Devices.Bluetooth.Sdp;
using InTheHand.Devices.Enumeration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Bluetooth.Rfcomm;
using Windows.Devices.Enumeration;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace DevicePickerSample.win81
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
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
            DeviceInformation dev = await picker.PickSingleDeviceAsync(new Rect(200,20,50,50));
            if (dev != null)
            {
                // if a device is selected create a BluetoothDevice instance to get more information
                RfcommDeviceService service = await RfcommDeviceService.FromIdAsync(dev.Id);
                if (service != null)
                {
                    foreach (KeyValuePair<uint, IBuffer> attrib in await service.GetSdpRawAttributesAsync())
                    {
                        SdpDataElement de = SdpDataElement.FromByteArray(attrib.Value.ToArray());
                        System.Diagnostics.Debug.WriteLine(attrib.Key.ToString("X") + " " + de.ToString());
                    }
                    // set data-binding so that device properties are displayed
                    DeviceInformationPanel.DataContext = service;
                }
            }
        }
    }
}
