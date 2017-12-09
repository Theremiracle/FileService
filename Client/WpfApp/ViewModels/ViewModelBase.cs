using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;
using Prism.Events;

namespace Client.WpfApp.ViewModels
{
    abstract class ViewModelBase : BindableBase
    {
        public readonly IEventAggregator EventAggregator;
        public ViewModelBase(IEventAggregator eventAggregator)
        {
            EventAggregator = eventAggregator;
        }
    }
}
