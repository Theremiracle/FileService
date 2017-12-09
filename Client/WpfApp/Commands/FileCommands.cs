using Common.Contract;
using Prism.Commands;
using Prism.Events;
using ServiceProxy;
using System.IO;
using System.Threading.Tasks;

namespace Client.WpfApp.Commands
{
    class FileCommands : CommandsBase
    {
        private readonly IFileService _fileServer;

        #region Constructor
        public FileCommands(IFileService fileService, IEventAggregator eventAggregator) : base(eventAggregator)
        {
            _fileServer = fileService;
            Initialize();
        }
        #endregion

        public override void Initialize()
        {
            TestConnectionCommand = new DelegateCommand(OnTestConnection, CanTestConnection);

            UploadFileCommand = new DelegateCommand(OnUploadFile, CanUploadFile);
            DownloadFileCommand = new DelegateCommand(OnDownloadFile, CanDownloadFile);
            DeleteFileCommand = new DelegateCommand(OnDeleteFile, CanDeleteFile);
        }

        public override void RaiseCommandCanExecuteChanged()
        {
            UploadFileCommand.RaiseCanExecuteChanged();
            DownloadFileCommand.RaiseCanExecuteChanged();
            DeleteFileCommand.RaiseCanExecuteChanged();
        }

        #region File Commands
        public DelegateCommand TestConnectionCommand { get; private set; }
        private async void OnTestConnection()
        {
            await ExecuteCommand(() =>
             {
                 SendLogMessage("Starts testing if service is ready");
                 var result = _fileServer.IsConnectionReadyAsync();

                 result.Wait();
                 LogTaskResult(result);
             });
        }
        private bool CanTestConnection()
        {
            return CanStartService;
        }

        public DelegateCommand UploadFileCommand { get; private set; }
        private async void OnUploadFile()
        {
            await ExecuteCommand(UploadFile);
        }

        private void UploadFile()
        {
            var result = UploadFileAsync();

            result.Wait();
            LogTaskResult(result);
        }

        private Task<bool> UploadFileAsync()
        {
            var fileFullName = FileServiceProxy.FileToUpload;
            var fileUploadFolder = FileServiceProxy.UploadFolderPath;
            SendLogMessage($"Starts upload file: From: {fileFullName} To: {fileUploadFolder}");
            return _fileServer.SaveFileAsync(fileFullName, fileUploadFolder);
        }

        private bool CanUploadFile()
        {
            return CanStartService;
        }

        public DelegateCommand DownloadFileCommand { get; private set; }
        private async void OnDownloadFile()
        {
            await ExecuteCommand(DownloadFile);
        }

        private void DownloadFile()
        {
            var result = DownloadFileAsync();

            result.Wait();
            LogTaskResult(result);
        }

        private Task<bool> DownloadFileAsync()
        {
            var fileFullName = FileServiceProxy.FileToDownload;
            SendLogMessage($"Starts getting file at: {fileFullName}");
            return _fileServer.GetFileAsync(fileFullName);
        }

        private bool CanDownloadFile()
        {
            return CanStartService;
        }

        public DelegateCommand DeleteFileCommand { get; private set; }
        private async void OnDeleteFile()
        {
            await ExecuteCommand(DeleteFile);
        }

        private void DeleteFile()
        {
            var result = DeleteFileAsync();

            result.Wait();
            LogTaskResult(result);
        }

        private Task<bool> DeleteFileAsync()
        {
            var fileFullName = FileServiceProxy.UploadFolderPath + @"\" + Path.GetFileName(FileServiceProxy.FileToUpload);
            SendLogMessage($"Starts deleting file at: {fileFullName}");
            return _fileServer.DeleteFileAsync(fileFullName);
        }

        private bool CanDeleteFile()
        {
            return CanStartService;
        }
        #endregion
    }
}
