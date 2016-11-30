using InTheHand.Phone.Bluetooth;
using Microsoft.Phone.Controls;
using System;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows;
using System.Windows.Navigation;
using Windows.Networking.Proximity;

namespace BluetoothExample
{
    using System.Text;
    using System.Threading.Tasks;

    using Windows.Storage.Streams;

    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            PeerFinder.Start();
            
            // Sample code to localize the ApplicationBar
            // BuildLocalizedApplicationBar();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            reading = false;
            PeerFinder.Stop();

            base.OnNavigatedFrom(e);
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            InTheHand.Devices.Bluetooth.BluetoothDevicePicker bdp = new InTheHand.Devices.Bluetooth.BluetoothDevicePicker();
            //bdp.ServiceFilter = InTheHand.Devices.Bluetooth.Rfcomm.RfcommServiceId.ObexObjectPush.Uuid;
            PeerInformation pi = await bdp.PickDeviceAsync();
            if (pi != null)
            {
                Windows.Networking.HostName hn = new Windows.Networking.HostName(pi.HostName.RawName.ToString());
                
                // do something with the device
                System.Threading.ThreadPool.QueueUserWorkItem(ReadThread, hn);
            }
        }

        private bool reading;

        private async void ReadThread(object host)
        {
            reading = true;
            Windows.Networking.Sockets.StreamSocket socket = new Windows.Networking.Sockets.StreamSocket();
            
            await socket.ConnectAsync((Windows.Networking.HostName)host, InTheHand.Devices.Bluetooth.Rfcomm.RfcommServiceId.SerialPort.AsString());

            byte[] buffer = new byte[32];
            System.Text.StringBuilder sb = new StringBuilder();
            
            while (reading)
            {
                IBuffer returnedBuffer = await socket.InputStream.ReadAsync(buffer.AsBuffer(), (uint)buffer.Length, Windows.Storage.Streams.InputStreamOptions.Partial);

                // break loop if response is empty (connection fail etc)
                if (returnedBuffer.Length == 0) break;

                string s = System.Text.Encoding.UTF8.GetString(buffer, 0, (int)returnedBuffer.Length);

                if (!string.IsNullOrEmpty(s))
                {
                    if (s.IndexOf('\0') > -1)
                    {
                        s = s.Substring(0, s.IndexOf('\0'));
                    }

                    sb.Append(s);

                    // Only process when we have a complete line
                    if (sb.ToString().EndsWith("\r"))
                    {
                        Dispatcher.BeginInvoke(new Action<string>(this.InsertMessage), sb.ToString());
                        sb.Clear();
                    }
                }
            }

            socket.Dispose();
        }

        /// <summary>
        /// Adds a scanned item to the list (called on UI thread).
        /// </summary>
        /// <param name="message"></param>
        private void InsertMessage(string message)
        {
            TheList.Items.Add(message);
        }


        // Sample code for building a localized ApplicationBar
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Set the page's ApplicationBar to a new instance of ApplicationBar.
        //    ApplicationBar = new ApplicationBar();

        //    // Create a new button and set the text value to the localized string from AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Create a new menu item with the localized string from AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}
    }
}