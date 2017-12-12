using Common.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Contract.BookMgr
{
    public interface IBookService : IWebApiService
    {
        Task<bool> IsConnectionReadyAsync();

        Task<IList<Book>> GetAllBooksAsync();

        Task<IList<Book>> GetBooksAsync(IList<int> bookIds);

        Task<bool> CreateBooksAsync(IList<Book> books);

        Task<bool> UpdateBooksAsync(IList<Book> books);

        Task<bool> DeleteBooksAsync(IList<int> bookIds);
    }
}