using Common.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Contract.BookMgr
{
    public class SaveBooksRequest
    {
        public List<Book> Books { get; set; }
    }
}
