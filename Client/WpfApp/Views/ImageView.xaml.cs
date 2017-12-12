using System.Windows.Controls;
using Client.WpfApp.Events;
using Prism.Events;
using System.IO;
using System.Windows.Media.Imaging;
using Microsoft.Practices.Unity;
using System;
using Client.WpfApp.Helpers;
using Client.WpfApp.ViewModels;

namespace Client.WpfApp.Views
{
    public partial class ImageView : UserControl
    {
        public ImageView()
        {
            InitializeComponent();
            SetDownloadedImage(null);

            DataContextChanged += OnDataContextChanged;
        }

        private void OnDataContextChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            var viewmodel = DataContext as ImageViewModel;
            if (viewmodel != null)
            {
                var eventAggregator = viewmodel.EventAggregator;
                eventAggregator.GetEvent<ImageDownloadeEvent>().Subscribe(OnImageDownloade, ThreadOption.UIThread);
                eventAggregator.GetEvent<ImageUploadRequestedEvent>().Subscribe(OnImageUploadRequested, ThreadOption.UIThread);
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
                var uri = new Uri("pack://application:,,,/Client.WpfApp;component/Resources/Images/Maps/MapUS.jpg");
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
