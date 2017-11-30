using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Contract;
using System.Web;

namespace TestConsole
{
    static class FileManager
    {
        public static string WebApiBaseAddress = @"http://localhost:54170/";

        public static string FolderPath = @"C:\Users\jim\Downloads\tech\code\TestData\Images";
        public static string SavePath = @"C:\Users\jim\Downloads\tech\code\TestData\Saved";

        private static string BuildUri(string address, string fileFullName)
        {
            var builder = new UriBuilder(address);
            var query = HttpUtility.ParseQueryString(builder.Query);
            query["FileFullName"] = fileFullName;
            builder.Query = query.ToString();
            string url = builder.ToString();

            return url;
        }

        public static async Task<bool> GetFile(string fileFullName)
        {
            try
            {
                FileModel.CheckFileEixsts(fileFullName);
                using (var client = new HttpClient())
                {
                    string address = WebApiBaseAddress + "/api/file/image";
                    var requestUri = BuildUri(address, fileFullName);
                    var message = await client.GetAsync(requestUri);

                    if (message.IsSuccessStatusCode)
                    {
                        return true;
                    }

                    throw new Exception(message.ToString());
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                Console.WriteLine(e);
                return false;
            }
        }

        public static async Task<bool> SaveFile(string fileFullName, string fileSavePath)
        {
            try
            {
                FileModel.CheckFileEixsts(fileFullName);
                using (var client = new HttpClient())
                {
                    using (var content = new MultipartFormDataContent())
                    {
                        var fileContent = new StreamContent(System.IO.File.OpenRead(fileFullName));
                        fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                        {
                            FileName = fileFullName
                        };
                        content.Add(fileContent);

                        string address = WebApiBaseAddress + "/api/file/image";
                        var requestUri = BuildUri(address, fileFullName);
                        var message = await client.PostAsync(requestUri, content);

                        if (message.IsSuccessStatusCode)
                        {
                            return true;
                        }

                        throw new Exception(message.ToString());
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                Console.WriteLine(e);
                return false;
            }
        }
    }
}
