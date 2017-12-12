using Common.Contract.BookMgr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Infrastructure.Entities;
using Newtonsoft.Json;

namespace Client.ServiceProxy
{
    public class BookServiceProxy : ServiceProxyBase, IBookService
    {
        public async Task<IList<Book>> GetAllBooksAsync()
        {
            string address = WebApiBaseAddress + "/api/book";
            var requestUri = BuildUri(address);
            var response = await _client.GetAsync(requestUri);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var books = JsonConvert.DeserializeObject<List<Book>>(content);

                return books;
            }

            throw new Exception(response.ToString());
        }

        public async Task<IList<Book>> GetBooksAsync(IList<int> bookIds)
        {
            return await Task.Run(() => new List<Book>());
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
