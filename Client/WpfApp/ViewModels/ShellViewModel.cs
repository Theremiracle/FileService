using System;
using System.Threading.Tasks;
using Client.WpfApp.Bases;
using Prism.Commands;
using ServiceProxy;
using Common.Contract;
using Microsoft.Practices.Unity;
using System.IO;
using Client.WpfApp.Events;
using Prism.Events;

namespace Client.WpfApp.ViewModels
{
    class ShellViewModel : ViewModelBase
    {
        private readonly IFileService _fileServer;
        public ShellViewModel(IFileService fileService, IEventAggregator eventAggregator) : base(eventAggregator)
        {
            _fileServer = fileService;
            _webApiAddress = FileServiceProxy.DefaultWebApiBaseAddress;

            InitializeCommands();
        }

        private void InitializeCommands()
        {
            CancelCommand = new DelegateCommand(OnCancel, CanCancel);
            TestConnectionCommand = new DelegateCommand(OnTestConnection, CanTestConnection);

            UploadFileCommand = new DelegateCommand(OnUploadFile, CanUploadFile);
            DownloadFileCommand = new DelegateCommand(OnDownloadFile, CanDownloadFile);
            DeleteFileCommand = new DelegateCommand(OnDeleteFile, CanDeleteFile);
            UploadImageCommand = new DelegateCommand(OnUploadImge, CanUploadImage);
            DownloadImageCommand = new DelegateCommand(OnDownloadImge, CanDownloadImage);
            ResetImageCommand = new DelegateCommand(OnResetImage, CanResetImage);
        }

        [Dependency]
        public StatusViewModel StatusViewModel { get; set; }

        #region Properties
        private string _webApiAddress;
        public string WebApiAddress
        {
            get { return _webApiAddress; }
            set
            {
                SetProperty(ref _webApiAddress, value);
                RaiseCommandCanExecuteChanged();
            }
        }

        private string _logMessage;
        public string LogMessage
        {
            get { return _logMessage; }
            set
            {
                SetProperty(ref _logMessage, value);
                RaisePropertyChanged(nameof(HasLogMessage));
            }
        }

        public bool HasLogMessage => !string.IsNullOrEmpty(LogMessage);

        private bool _isConnecting;
        public bool IsConnecting
        {
            get { return _isConnecting; }
            set
            {
                SetProperty(ref _isConnecting, value);
                RaiseCommandCanExecuteChanged();
            }
        }

        public bool CanStartService => !IsConnecting && !string.IsNullOrEmpty(WebApiAddress);
        #endregion

        #region Commands
        public DelegateCommand TestConnectionCommand { get; private set; }
        private void OnTestConnection()
        {
            ExecuteCommand(() =>
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

        public DelegateCommand CancelCommand { get; private set; }
        private void OnCancel()
        {
            ExecuteCommand(() =>
            {

            });
        }
        private bool CanCancel()
        {
            return !CanStartService;
        }
        #endregion

        #region File Commands
        public DelegateCommand UploadFileCommand { get; private set; }
        private void OnUploadFile()
        {
            ExecuteCommand(UploadFile);
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
        private void OnDownloadFile()
        {
            ExecuteCommand(DownloadFile);
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
        private void OnDeleteFile()
        {
            ExecuteCommand(DeleteFile);
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

        #region Image Commands
        public DelegateCommand UploadImageCommand { get; private set; }
        private void OnUploadImge()
        {
            _eventAggregator.GetEvent<ImageUploadRequestedEvent>().Publish(DoUploadImage);
        }

        private void DoUploadImage(Byte[] bytes)
        {
            ExecuteCommand(() => UploadImage(bytes));
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
        private void OnDownloadImge()
        {
            ExecuteCommand(DownloadImage);
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
            _eventAggregator.GetEvent<ImageDownloadeEvent>().Publish(stream);

            return true;
        }
        private bool CanDownloadImage()
        {
            return CanStartService;
        }

        public DelegateCommand ResetImageCommand { get; private set; }
        private void OnResetImage()
        {
            _eventAggregator.GetEvent<ImageDownloadeEvent>().Publish(null);
        }        

        private bool CanResetImage()
        {
            return true;
        }
        #endregion

        #region Command Helpers
        private void ExecuteCommand(Action commandAction)
        {
            Task.Factory.StartNew(() => ExecuteCommandAsync(commandAction));
        }

        private void ExecuteCommandAsync(Action commandAction)
        {
            try
            {
                IsConnecting = true;
                commandAction.Invoke();
            }
            catch (Exception ex)
            {
                LogMessage = ex.Message;
            }
            finally
            {
                IsConnecting = false;
            }
        }

        private void SendLogMessage(string message)
        {
            LogMessage += $"\n{DateTime.Now} {message}";
        }

        private void LogTaskResult(Task<bool> task)
        {
            if (task.IsCompleted && task.Result)
            {
                LogMessage += $"\n{DateTime.Now} Succeeds!!!\n";
            }
            else
            {
                LogMessage += $"\n{DateTime.Now} Failed...\n";
            }
        }

        private void RaiseCommandCanExecuteChanged()
        {
            UploadFileCommand.RaiseCanExecuteChanged();
            DownloadFileCommand.RaiseCanExecuteChanged();
            DeleteFileCommand.RaiseCanExecuteChanged();

            UploadImageCommand.RaiseCanExecuteChanged();
            DownloadImageCommand.RaiseCanExecuteChanged();
            ResetImageCommand.RaiseCanExecuteChanged();
        }
        #endregion
    }
}
