using Prism.Commands;
using ServiceProxy;
using Microsoft.Practices.Unity;
using Prism.Events;
using Client.WpfApp.Commands;
using Client.WpfApp.Events;
using System;
using Common.Contract;

namespace Client.WpfApp.ViewModels
{
    class ShellViewModel : ViewModelBase
    {
        private readonly IFileService _fileService;
        #region Constructor
        public ShellViewModel(IFileService fileService, IEventAggregator eventAggregator) : base(eventAggregator)
        {
            _fileService = fileService;
            EventAggregator.GetEvent<LogMessageSentOutEvent>().Subscribe(OnLogMessageSentOut);
            InitializeCommands();
        }

        private void InitializeCommands()
        {
            CancelCommand = new DelegateCommand(OnCancel, CanCancel);
        }

        [InjectionMethod]
        private void InjectionMethod()
        {
            _webApiAddress = _fileService.WebApiBaseAddress;
        }
        #endregion

        #region Properties
        [Dependency]
        public StatusViewModel StatusViewModel { get; set; }

        [Dependency]
        public WebApiDataViewModel WebApiDataViewModel { get; set; }

        [Dependency]
        public WcfDataViewModel WcfDataViewModel { get; set; }

        [Dependency]
        public ImageViewModel ImageViewModel { get; set; }

        [Dependency]
        public FileCommands FileCommands { get; set; }        

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
        #endregion

        #region LogMessage
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

        private void OnLogMessageSentOut(string message)
        {
            LogMessage += $"{DateTime.Now}: {message} {Environment.NewLine}";
        }
        #endregion

        #region Commands
        public DelegateCommand CancelCommand { get; private set; }
        private void OnCancel()
        {
        }
        private bool CanCancel()
        {
            return true;
            //return !CanStartService;
        }

        private void RaiseCommandCanExecuteChanged()
        {
            CancelCommand.RaiseCanExecuteChanged();

            FileCommands?.RaiseCommandCanExecuteChanged();
            ImageViewModel?.ImageCommands?.RaiseCommandCanExecuteChanged();
        }
        #endregion
    }
}
