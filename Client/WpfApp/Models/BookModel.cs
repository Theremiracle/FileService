using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.WpfApp.Models
{
    class BookModel : ModelBase
    {
        public BookModel(IEventAggregator eventAggregator) : base(eventAggregator)
        {
        }
    }
}
