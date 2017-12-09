using Prism.Commands;
using ServiceProxy;
using Microsoft.Practices.Unity;
using Prism.Events;
using Client.WpfApp.Commands;
using Client.WpfApp.Events;
using System;

namespace Client.WpfApp.ViewModels
{
    class ImageViewModel : ViewModelBase
    {
        public ImageViewModel(IEventAggregator eventAggregator) : base(eventAggregator)
        {
        }

        [Dependency]
        public ImageCommands ImageCommands { get; set; }
    }
}
