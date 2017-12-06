using Client.WpfApp.Events;
using Prism.Events;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using Microsoft.Practices.Unity;
using System;
using System.Runtime.InteropServices;
using System.Drawing;
using Client.WpfApp.Helpers;

namespace Client.WpfApp.Views
{
    /// <summary>
    /// Interaction logic for ShellView.xaml
    /// </summary>
    public partial class ShellView : Window
    {
        public ShellView()
        {
            InitializeComponent();
            SetDownloadedImage(null);
        }

        private IEventAggregator _eventAggregator;

        [Dependency]
        public IEventAggregator EventAggregator
        {
            get { return _eventAggregator; }
            set
            {
                _eventAggregator = value;
                _eventAggregator.GetEvent<ImageDownloadeEvent>().Subscribe(OnImageDownloade, ThreadOption.UIThread);
                _eventAggregator.GetEvent<ImageUploadRequestedEvent>().Subscribe(OnImageUploadRequested, ThreadOption.UIThread);
            }
        }

        private void OnImageDownloade(Stream stream)
        {
            SetDownloadedImage(stream);
        }

        private void SetDownloadedImage(Stream stream)
        {
            if (stream == null)
            {
                var uri = new Uri("pack://application:,,,/Client.WpfApp;component/Resources/Images/MapUS.jpg");
                DownloadedImage.Source = new BitmapImage(uri);
                return;
            }

            DownloadedImage.Source = BitmapFrame.Create(stream,
                                                  BitmapCreateOptions.None,
                                                  BitmapCacheOption.OnLoad);
        }

        private void OnImageUploadRequested(Action<Byte[]> action)
        {
            if (action == null) return;

            var bytes = ImageHelper.ConvertBitmapSourceToByteArray(UploadedImage.Source);

            action.Invoke(bytes);
        }        
    }
}
