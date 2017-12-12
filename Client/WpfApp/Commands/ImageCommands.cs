using Client.WpfApp.Events;
using Common.Contract;
using Prism.Commands;
using Prism.Events;
using ServiceProxy;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace Client.WpfApp.Commands
{
    class ImageCommands : CommandsBase
    {
        private readonly IFileService _fileServer;

        public ImageCommands(IFileService fileService, IEventAggregator eventAggregator) : base(eventAggregator)
        {
            _fileServer = fileService;
            Initialize();
        }

        public override void Initialize()
        {
            UploadImageCommand = new DelegateCommand(OnUploadImge, CanUploadImage);
            DownloadImageCommand = new DelegateCommand(OnDownloadImge, CanDownloadImage);
            ResetImageCommand = new DelegateCommand(OnResetImage, CanResetImage);
            ChangeImageCommand = new DelegateCommand(OnChangeImage, CanChangeImage);
            OpenExplorerCommand = new DelegateCommand<string>(OnOpenExplorer, CanOpenExplorer);
        }

        public override void RaiseCommandCanExecuteChanged()
        {
            UploadImageCommand.RaiseCanExecuteChanged();
            DownloadImageCommand.RaiseCanExecuteChanged();
            ResetImageCommand.RaiseCanExecuteChanged();
        }

        #region Image Commands
        public DelegateCommand UploadImageCommand { get; private set; }
        private void OnUploadImge()
        {
            EventAggregator.GetEvent<ImageUploadRequestedEvent>().Publish(DoUploadImage);
        }

        private async void DoUploadImage(Byte[] bytes)
        {
            await ExecuteCommand(() => UploadImage(bytes));
        }

        private void UploadImage(Byte[] bytes)
        {
            var result = UploadImageAsync(bytes);

            result.Wait();
            LogTaskResult(result);
        }

        private Task<bool> UploadImageAsync(Byte[] bytes)
        {
            var fileFullName = FileServiceProxy.FileToUpload;
            var fileUploadFolder = FileServiceProxy.UploadFolderPath;
            var filePathUploadedTo = fileUploadFolder + @"\" + Path.GetFileName(fileFullName);
            SendLogMessage($"Starts upload image: From: {fileFullName} To: {fileUploadFolder}");
            return _fileServer.SaveImageAsync(bytes, filePathUploadedTo);
        }
        private bool CanUploadImage()
        {
            return CanStartService;
        }

        public DelegateCommand DownloadImageCommand { get; private set; }
        private async void OnDownloadImge()
        {
            await ExecuteCommand(DownloadImage);
        }
        private void DownloadImage()
        {
            var result = DownloadImageAsync();

            result.Wait();
            LogTaskResult(result);
        }

        private async Task<bool> DownloadImageAsync()
        {
            var fileFullName = FileServiceProxy.FileToDownload;
            SendLogMessage($"Starts getting image at: {fileFullName}");
            var stream = await _fileServer.GetImageAsync(fileFullName);
            EventAggregator.GetEvent<ImageDownloadeEvent>().Publish(stream);

            return true;
        }
        private bool CanDownloadImage()
        {
            return CanStartService;
        }

        public DelegateCommand ResetImageCommand { get; private set; }
        private void OnResetImage()
        {
            EventAggregator.GetEvent<ImageDownloadeEvent>().Publish(null);
        }

        private bool CanResetImage()
        {
            return true;
        }

        public DelegateCommand ChangeImageCommand { get; private set; }
        private void OnChangeImage()
        {
            EventAggregator.GetEvent<ImageChangeRequestedEvent>().Publish(null);
        }

        private bool CanChangeImage()
        {
            return true;
        }

        public DelegateCommand<string> OpenExplorerCommand { get; private set; }
        private void OnOpenExplorer(string upload)
        {
            string directory = IsUpload(upload) ? FileServiceProxy.UploadFolderPath : FileServiceProxy.DownloadFolderPath;
            Process.Start(directory);
        }

        private bool CanOpenExplorer(string upload)
        {
            return true;
        }

        private static bool IsUpload(string upload)
        {
            if (string.IsNullOrEmpty(upload)) return false;
            return upload.ToLower() == "true";
        }
        #endregion
    }
}
