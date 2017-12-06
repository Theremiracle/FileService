﻿using Client.WpfApp.Events;
using Prism.Events;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using Microsoft.Practices.Unity;

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
                var uri = new System.Uri("pack://application:,,,/Client.WpfApp;component/Resources/Images/MapUS.jpg");
                DownloadedImage.Source = new BitmapImage(uri);
                return;
            }

            DownloadedImage.Source = BitmapFrame.Create(stream,
                                                  BitmapCreateOptions.None,
                                                  BitmapCacheOption.OnLoad);
        }
    }
}
