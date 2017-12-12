using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Client.ServiceProxy
{
    public abstract class ServiceProxyBase
    {
        protected static readonly string TestDataFolder = GetTestDataFolder();
        private static string GetTestDataFolder()
        {
            var dir = AppDomain.CurrentDomain.BaseDirectory;
            var path = Directory.GetParent(dir).Parent.Parent.Parent.Parent.FullName + @"\TestData";
            return path;
        }

        protected readonly HttpClient _client = new HttpClient();

        public virtual string WebApiBaseAddress
        {
            get
            {
                //return @"http://localhost:54170";
                return @"http://localhost/AspWebApi";
            }
        }

        protected static string BuildUri(string address, Dictionary<string, string> dictionary = null)
        {
            var builder = new UriBuilder(address);
            if (dictionary != null)
            {
                var query = HttpUtility.ParseQueryString(builder.Query);
                foreach (var item in dictionary)
                {
                    query[item.Key] = item.Value;
                }
                
                builder.Query = query.ToString();
            }
            string url = builder.ToString();

            return url;
        }

        public async Task<bool> IsConnectionReadyAsync()
        {
            string address = WebApiBaseAddress + "/api/test";
            var requestUri = BuildUri(address);
            var message = await _client.GetAsync(requestUri);
            if (message.IsSuccessStatusCode)
            {
                return true;
            }

            throw new Exception(message.ToString());
        }
    }
}
