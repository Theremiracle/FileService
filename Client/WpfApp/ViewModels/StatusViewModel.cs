using Client.WpfApp.Bases;
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

        }
    }
}
