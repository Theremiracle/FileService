using Client.WpfApp.Events;
using Client.WpfApp.ViewModels;
using Prism.Events;
using System;
using System.Threading.Tasks;

namespace Client.WpfApp.Commands
{
    abstract class CommandsBase : ViewModelBase
    {
        public CommandsBase(IEventAggregator eventAggregator) : base(eventAggregator)
        {
        }        

        public abstract void RaiseCommandCanExecuteChanged();

        public abstract void Initialize();

        private bool _isRunning;
        public bool IsRunning
        {
            get { return _isRunning; }
            set
            {
                SetProperty(ref _isRunning, value);
                RaiseCommandCanExecuteChanged();
            }
        }

        public bool CanStartService => !IsRunning;

        protected async Task ExecuteCommand(Action commandAction)
        {
            await Task.Factory.StartNew(() => ExecuteCommandAsync(commandAction));
        }

        protected void ExecuteCommandAsync(Action commandAction)
        {
            try
            {
                IsRunning = true;
                commandAction.Invoke();
            }
            catch (Exception ex)
            {
                SendLogMessage(ex.Message);
            }
            finally
            {
                IsRunning = false;
            }
        }

        protected void SendLogMessage(string message)
        {
            EventAggregator.GetEvent<LogMessageSentOutEvent>().Publish(message);
        }

        protected void LogTaskResult(Task<bool> task)
        {
            if (task.IsCompleted && task.Result)
            {
                SendLogMessage($"Succeeds!!!");
            }
            else
            {
                SendLogMessage($"Failed...");
            }
        }
    }
}
