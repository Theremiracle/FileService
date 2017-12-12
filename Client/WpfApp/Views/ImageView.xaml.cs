using System.Windows.Controls;
using Client.WpfApp.Events;
using Prism.Events;
using System.IO;
using System.Windows.Media.Imaging;
using Microsoft.Practices.Unity;
using System;
using Client.WpfApp.Helpers;
using Client.WpfApp.ViewModels;
using System.Collections.Generic;

namespace Client.WpfApp.Views
{
    public partial class ImageView : UserControl
    {
        public ImageView()
        {
            InitializeComponent();

            SetDownloadedImage();
            SetUploadedImage();

            DataContextChanged += OnDataContextChanged;
        }

        private readonly IList<Uri> _imageUris = new List<Uri>
        {
            new Uri("pack://application:,,,/Client.WpfApp;component/Resources/Images/Maps/MapUS.jpg"),            
            new Uri("pack://application:,,,/Client.WpfApp;component/Resources/Images/Maps/MapTexas.png"),
            new Uri("pack://application:,,,/Client.WpfApp;component/Resources/Images/Maps/MapHouston.png"),
            new Uri("pack://application:,,,/Client.WpfApp;component/Resources/Images/Maps/MapWorld.jpg")
        };
        private int _uploadImageUriIndex = 0;

        private void OnDataContextChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            var viewmodel = DataContext as ImageViewModel;
            if (viewmodel != null)
            {
                var eventAggregator = viewmodel.EventAggregator;
                eventAggregator.GetEvent<ImageDownloadeEvent>().Subscribe(OnImageDownloade, ThreadOption.UIThread);
                eventAggregator.GetEvent<ImageUploadRequestedEvent>().Subscribe(OnImageUploadRequested, ThreadOption.UIThread);
                eventAggregator.GetEvent<ImageChangeRequestedEvent>().Subscribe(OnImageChangeRequested, ThreadOption.UIThread);
            }
        }

        private void OnImageDownloade(Stream stream)
        {
            SetDownloadedImage(stream);
        }

        private void SetDownloadedImage(Stream stream = null)
        {
            if (stream == null)
            {
                var uri = _imageUris[GetNextUploadImageUriIndex()];
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

        private void OnImageChangeRequested(string path)
        {
            _uploadImageUriIndex = GetNextUploadImageUriIndex();
            SetUploadedImage();
        }

        private void SetUploadedImage()
        {
            UploadedImage.Source = new BitmapImage(_imageUris[_uploadImageUriIndex]);
        }

        private int GetNextUploadImageUriIndex()
        {
            return (_uploadImageUriIndex + 1) % _imageUris.Count;
        }
    }
}
