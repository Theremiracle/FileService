using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Infrastructure.Entities
{
    public class Book : EntityBase
    {
        public string Author { get; set; }
        public DateTime PublishDate { get; set; }
    }
}
