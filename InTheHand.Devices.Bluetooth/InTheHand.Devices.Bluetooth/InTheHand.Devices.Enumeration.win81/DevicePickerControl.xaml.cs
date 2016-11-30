using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System.Threading;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace InTheHand.Devices.Enumeration
{
    internal sealed partial class DevicePickerControl : UserControl
    {
        private DevicePicker _owner;
        private Popup _popup;
        private System.Threading.EventWaitHandle _handle;
        internal DevicePickerControl(DevicePicker picker, Popup popup)
        {
            _owner = picker;
            _popup = popup;
            _popup.Closed += _popup_Closed;
            _handle = new System.Threading.EventWaitHandle(false, System.Threading.EventResetMode.AutoReset);

            this.Resources.Add("AccentBrush", new SolidColorBrush(this._owner.Appearance.AccentColor));
            this.Resources.Add("ForegroundBrush", new SolidColorBrush(this._owner.Appearance.ForegroundColor));
            this.Resources.Add("SelectedAccentBrush", new SolidColorBrush(this._owner.Appearance.SelectedAccentColor));
            this.Resources.Add("SelectedBackgroundBrush", new SolidColorBrush(this._owner.Appearance.SelectedBackgroundColor));
            this.Resources.Add("SelectedForegroundBrush", new SolidColorBrush(this._owner.Appearance.SelectedForegroundColor));

            this.InitializeComponent();

            RootVisual.Background = new SolidColorBrush(this._owner.Appearance.BackgroundColor);
            this.Foreground = new SolidColorBrush(this._owner.Appearance.ForegroundColor);

            this.Loaded += DevicePickerControl_Loaded;

            
        }

        void _popup_Closed(object sender, object e)
        {
            _handle.Set();
        }

        public DeviceInformation WaitForSelection()
        {
            _handle.WaitOne();
            return SelectedDevice;
        }
        async void DevicePickerControl_Loaded(object sender, RoutedEventArgs e)
        {
            List<DeviceViewModel> devices = new List<DeviceViewModel>();

            if (_owner.Filter.SupportedDeviceSelectors.Count > 0)
            {
                foreach (string selector in _owner.Filter.SupportedDeviceSelectors)
                {
                    DeviceInformationCollection filteredDevices = await DeviceInformation.FindAllAsync(selector);
                    foreach (DeviceInformation info in filteredDevices)
                    {
                        devices.Add(new DeviceViewModel(info));
                    }
                }
            }
            else
            {
                foreach (DeviceInformation info in await DeviceInformation.FindAllAsync())
                {
                    devices.Add(new DeviceViewModel(info));
                }
            }

            DeviceList.ItemsSource = devices;

            if (devices.Count == 0)
            {
                EmptyMessage.Visibility = Visibility.Visible;
            }
        }

        private DeviceInformation _selectedDevice;
        public DeviceInformation SelectedDevice
        {
            get
            {
                return _selectedDevice;
            }
        }

        private void DeviceList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DeviceList.SelectedIndex > -1)
            {
                _selectedDevice = (DeviceList.SelectedItem as DeviceViewModel).Information;
                _owner.OnDeviceSelected(_selectedDevice);
                _popup.IsOpen = false;
            }
        }
    }
}
