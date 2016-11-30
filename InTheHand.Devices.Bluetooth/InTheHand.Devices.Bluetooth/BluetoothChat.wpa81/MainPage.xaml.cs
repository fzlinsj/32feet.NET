//-----------------------------------------------------------------------
// <copyright file="MainPage.xaml.cs" company="In The Hand Ltd">
//   32feet.NET - Personal Area Networking for .NET
//   This source code is licensed under the Microsoft Public License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using InTheHand.Devices.Enumeration;
using System;
using System.IO;
using Windows.ApplicationModel.Background;
using Windows.Devices.Bluetooth.Rfcomm;
using Windows.Devices.Enumeration;
using Windows.Foundation;
using Windows.Networking.Sockets;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

namespace BluetoothChat.wpa81
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private const string TaskName = "BluetoothChatBackgroundTask";

        private RfcommDeviceService remoteService;

        public MainPage()
        {
            this.InitializeComponent();

            this.Loaded += MainPage_Loaded;
            this.NavigationCacheMode = NavigationCacheMode.Required;
        }

        async void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            var promise = await BackgroundExecutionManager.RequestAccessAsync();

            if(promise == BackgroundAccessStatus.Denied)
            {
                var warning = new MessageDialog("Background execution is disabled. Please re-enable to allow this sample to receive messages", "Background Tasks");
                await warning.ShowAsync();
            }
            else
            {
                bool taskRegistered = false;

                foreach(var task in BackgroundTaskRegistration.AllTasks)
                {
                    if(task.Value.Name == TaskName)
                    {
                        taskRegistered = true;
                        break;
                    }
                }

                if(!taskRegistered)
                {
                    var trigger = new RfcommConnectionTrigger();
                    trigger.InboundConnection.LocalServiceId = RfcommServiceId.FromUuid(App.ChatServiceID);
                    // add a default service name (not localised)
                    //SdpDataElement rec = new SdpDataElement(new List<SdpDataElement> { new SdpDataElement((ushort)0x100), new SdpDataElement("BluetoothChat Sample") });
                    trigger.AllowMultipleConnections = true;

                    var builder = new BackgroundTaskBuilder();
                    builder.Name = "BluetoothChat";
                    builder.TaskEntryPoint = "BluetoothChat.BackgroundTask.BluetoothChatBackgroundTask";
                    builder.SetTrigger(trigger);


                    try
                    {
                        var reg = builder.Register();
                    }
                    catch(Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine(ex);
                    }
                }
            }
        }

        private async void TextBox_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if(e.Key == Windows.System.VirtualKey.Enter)
            {
                e.Handled = true;
                string message = InputText.Text;
                if(!string.IsNullOrEmpty(message))
                {
                    var socket = new StreamSocket();
                    try
                    {
                        await socket.ConnectAsync(remoteService.ConnectionHostName, remoteService.ConnectionServiceName);
                        byte[] buffer = System.Text.Encoding.Unicode.GetBytes(message);
                        Stream s = socket.OutputStream.AsStreamForWrite();
                        await s.WriteAsync(buffer, 0, buffer.Length);
                        await s.FlushAsync();

                    }
                    finally
                    {
                        socket.Dispose();
                    }
                }
            }
        }

        private async void Connect_Click(object sender, RoutedEventArgs e)
        {
            

                string aqs = RfcommDeviceService.GetDeviceSelector(RfcommServiceId.FromUuid(App.ChatServiceID));

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
                    remoteService = await RfcommDeviceService.FromIdAsync(dev.Id);

                    if (remoteService != null)
                    {
                        InputText.IsEnabled = true;
                    }
                }
        }
    }
}
