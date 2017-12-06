using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;
using Client.WpfApp.Bases;
using Prism.Commands;
using ServiceProxy;
using Common.Contract;
using Microsoft.Practices.Unity;

namespace Client.WpfApp.ViewModels
{
    class ShellViewModel : ViewModelBase
    {
        private readonly IFileService _fileServer;
        public ShellViewModel(IFileService fileService)
        {
            _fileServer = fileService;
            _webApiAddress = FileServiceProxy.DefaultWebApiBaseAddress;

            InitializeCommands();
        }

        [Dependency]
        public StatusViewModel StatusViewModel { get; set; }

        private void InitializeCommands()
        {
            UploadFileCommand = new DelegateCommand(OnUploadFile, CanUploadFile);
            DownloadFileCommand = new DelegateCommand(OnDownloadFile, CanDownloadFile);
            DeleteFileCommand = new DelegateCommand(OnDeleteFile, CanDeleteFile);
            UploadImageCommand = new DelegateCommand(OnUploadImge, CanUploadImage);
            DownloadImageCommand = new DelegateCommand(OnDownloadImge, CanDownloadImage);
        }

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

        public DelegateCommand UploadFileCommand { get; private set; }
        private void OnUploadFile()
        {
            IsConnecting = true;

            IsConnecting = false;
        }
        private bool CanUploadFile()
        {
            return CanStartService;
        }

        public DelegateCommand DownloadFileCommand { get; private set; }
        private void OnDownloadFile()
        {
            IsConnecting = true;

            IsConnecting = false;
        }
        private bool CanDownloadFile()
        {
            return CanStartService;
        }

        public DelegateCommand DeleteFileCommand { get; private set; }
        private void OnDeleteFile()
        {
            IsConnecting = true;

            IsConnecting = false;
        }
        private bool CanDeleteFile()
        {
            return CanStartService;
        }
        
        public DelegateCommand UploadImageCommand { get; private set; }
        private void OnUploadImge()
        {
            IsConnecting = true;

            IsConnecting = false;
        }
        private bool CanUploadImage()
        {
            return CanStartService;
        }

        public DelegateCommand DownloadImageCommand { get; private set; }
        private void OnDownloadImge()
        {
            IsConnecting = true;

            IsConnecting = false;
        }
        private bool CanDownloadImage()
        {
            return CanStartService;
        }

        private void RaiseCommandCanExecuteChanged()
        {
            UploadFileCommand.RaiseCanExecuteChanged();
            DownloadFileCommand.RaiseCanExecuteChanged();
            DeleteFileCommand.RaiseCanExecuteChanged();

            UploadImageCommand.RaiseCanExecuteChanged();
            DownloadImageCommand.RaiseCanExecuteChanged();
        }
    }
}
