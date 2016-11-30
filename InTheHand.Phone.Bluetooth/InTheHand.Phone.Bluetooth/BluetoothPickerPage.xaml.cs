// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BluetoothPickerPage.cs" company="In The Hand Ltd">
//   32feet.NET - Personal Area Networking for .NET
//   This source code is licensed under the In The Hand Community License - see License.txt
// </copyright>
// <summary>
//   XAML Page which implements BluetoothDevicePicker.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace InTheHand.Devices.Bluetooth
{
    using System.Windows.Controls;
    using System.Windows.Navigation;

    using Microsoft.Phone.Controls;

    using Windows.Networking.Proximity;

    /// <summary>
    /// XAML Page which implements BluetoothDevicePicker.
    /// </summary>
    internal partial class BluetoothPickerPage : PhoneApplicationPage
    {
        public BluetoothPickerPage()
        {
            this.InitializeComponent();

            HeaderText.Text = InTheHand.Devices.Bluetooth.Resources.Strings.ChooseBluetoothDevice;
            NoteText.Text = InTheHand.Devices.Bluetooth.Resources.Strings.ChooseBluetoothDeviceNote;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            DeviceList.ItemsSource = BluetoothDevicePicker.Items;

            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            BluetoothDevicePicker.WaitHandle.Set();

            base.OnNavigatedFrom(e);
        }

        private void DeviceList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                BluetoothDevicePicker.SelectedItem = (PeerInformation)e.AddedItems[0];
                NavigationService.GoBack();
            }
        }
    }
}