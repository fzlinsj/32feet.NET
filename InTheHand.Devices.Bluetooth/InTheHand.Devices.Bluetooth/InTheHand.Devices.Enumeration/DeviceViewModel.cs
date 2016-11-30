﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Media;

namespace InTheHand.Devices.Enumeration
{
    public sealed class DeviceViewModel : INotifyPropertyChanged
    {
        private DeviceInformation _information;

        public DeviceViewModel(DeviceInformation information)
        {
            _information = information;
        }
        public string Name
        {
            get
            {
                return _information.Name;
            }
        }

        private ImageSource _thumbnail;
        public ImageSource Thumbnail
        {
            get
            {
                if(_thumbnail == null)
                {
                    GetThumbnail();
                }

                return _thumbnail;
            }
        }

        private async void GetThumbnail()
        {
            DeviceThumbnail thumb = await _information.GetGlyphThumbnailAsync();
            Stream s = thumb.AsStreamForRead();
            MemoryStream ms = new MemoryStream();
            await s.CopyToAsync(ms);
            ms.Seek(0, SeekOrigin.Begin);

            WriteableBitmap wb = new WriteableBitmap(1, 1);
            await wb.SetSourceAsync(ms.AsRandomAccessStream());
            _thumbnail = wb;
            OnPropertyChanged("Thumbnail");
        }

        public DeviceInformation Information
        {
            get
            {
                return _information;
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            if(this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
