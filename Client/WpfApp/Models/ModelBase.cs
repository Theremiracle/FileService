using Client.WpfApp.ViewModels;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.WpfApp.Models
{
    abstract class ModelBase : ViewModelBase
    {
        public ModelBase(IEventAggregator eventAggregator) : base(eventAggregator)
        {
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public string UserName { get; set; }

    }
}
