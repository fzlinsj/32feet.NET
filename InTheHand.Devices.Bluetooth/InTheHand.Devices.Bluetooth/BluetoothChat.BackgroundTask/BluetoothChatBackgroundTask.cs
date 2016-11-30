using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Devices.Bluetooth.Background;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.IO;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
using Windows.Storage.Streams;

namespace BluetoothChat.BackgroundTask
{
    public sealed class BluetoothChatBackgroundTask : IBackgroundTask
    {
        //internal static readonly Guid ServiceID = new Guid("39A2D110-7CB1-4C9F-B5CD-954B636E73EA");
        private const int MAX_MESSAGE_SIZE = 128;
        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            var deferral = taskInstance.GetDeferral();

            var details = taskInstance.TriggerDetails as RfcommConnectionTriggerDetails;
            var socket = details.Socket;
            var sender = details.RemoteDevice.Name;
            var s = socket.InputStream;
            var buffer = new Windows.Storage.Streams.Buffer(MAX_MESSAGE_SIZE * 2);

            try
            {
                IBuffer b = await s.ReadAsync(buffer, buffer.Capacity, Windows.Storage.Streams.InputStreamOptions.Partial);
                if (b.Length > 0)
                {
                    string r = System.Text.Encoding.Unicode.GetString(b.ToArray(), 0, (int)b.Length);
                    RaiseAToast(sender, r);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
            finally
            {
                if (socket != null)
                {
                    try
                    {
                        socket.Dispose();
                    }
                    catch { }
                }
                deferral.Complete();
            }
        }

        private void RaiseAToast(string title, string message)
        {
            ToastNotifier notifier = ToastNotificationManager.CreateToastNotifier();
            XmlDocument xd = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText02);

            var elements = xd.GetElementsByTagName("text");
            if(!string.IsNullOrEmpty(title))
            {
                elements[0].AppendChild(xd.CreateTextNode(title));
            }

            if(!string.IsNullOrEmpty(message))
            {
                elements[1].AppendChild(xd.CreateTextNode(message));
            }

            ToastNotification tn = new ToastNotification(xd);

            try
            {
                notifier.Show(tn);
            }
            catch { }
        }
    }
}
