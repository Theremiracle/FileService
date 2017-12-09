using Client.WpfApp.Events;
using Common.Infrastructure.Entities;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.WpfApp.ViewModels
{
    class WcfDataViewModel : ViewModelBase
    {
        public WcfDataViewModel(IEventAggregator eventAggregator) : base(eventAggregator)
        {
        }

        public ObservableCollection<Book> Books { get; private set; }
    }
}
