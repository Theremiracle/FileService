using Common.Contract.BookMgr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Infrastructure.Entities;

namespace Client.ServiceProxy
{
    public class BookServiceProxy : ServiceProxyBase, IBookService
    {
        public async Task<bool> GetAllBooksAsync()
        {
            return await Task.Run(() => true);
        }

        public async Task<bool> GetBooksAsync(IList<int> bookIds)
        {
            return await Task.Run(() => true);
        }

        public async Task<bool> CreateBooksAsync(IList<Book> books)
        {
            return await Task.Run(() => true);
        }

        public async Task<bool> UpdateBooksAsync(IList<Book> books)
        {
            return await Task.Run(() => true);
        }

        public async Task<bool> DeleteBooksAsync(IList<int> bookIds)
        {
            return await Task.Run(() => true);
        }
    }
}
