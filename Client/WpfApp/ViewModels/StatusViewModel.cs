using Client.WpfApp.Events;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.WpfApp.ViewModels
{
    class StatusViewModel : ViewModelBase
    {
        public StatusViewModel(IEventAggregator eventAggregator) : base(eventAggregator)
        {
            EventAggregator.GetEvent<LogMessageSentOutEvent>().Subscribe(OnLogMessageSentOut);
            StatusMessage = "Ready!";
        }

        #region LogMessage
        private string _statusMessage;
        public string StatusMessage
        {
            get { return _statusMessage; }
            set
            {
                SetProperty(ref _statusMessage, value);
            }
        }

        private void OnLogMessageSentOut(string message)
        {
            StatusMessage = message;
        }
        #endregion
    }
}
