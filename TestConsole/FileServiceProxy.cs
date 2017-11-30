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
    static class FileServiceProxy
    {
        public static string WebApiBaseAddress = @"http://localhost:54170/";

        public static string UploadFolderPath = @"K:\Code\EOG\FileService\TestData\Upload";
        public static string DownloadFolderPath = @"K:\Code\EOG\FileService\TestData\Download";
        public static string FileToDownload = @"K:\Code\EOG\FileService\TestData\FileToDownload.png";
        public static string FileToUpload = @"K:\Code\EOG\FileService\TestData\FileToUpload.jpg";

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

        public static async Task<bool> SaveFile(string uploadeFileFullName, string fileUploadFolder)
        {
            try
            {
                FileModel.CheckFileEixsts(uploadeFileFullName);
                using (var client = new HttpClient())
                {
                    using (var content = new MultipartFormDataContent())
                    {
                        var fileContent = new StreamContent(File.OpenRead(uploadeFileFullName));
                        fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                        {
                            FileName = uploadeFileFullName
                        };
                        content.Add(fileContent);

                        string fileFullNameToBeUploaded = $"{fileUploadFolder}" + @"\" + Path.GetFileName(uploadeFileFullName);
                        string address = WebApiBaseAddress + "/api/file/image";
                        var requestUri = BuildUri(address, fileFullNameToBeUploaded);
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

        public static async Task<bool> DeleteFile(string fileFullName)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    string address = WebApiBaseAddress + "/api/file/image";
                    var requestUri = BuildUri(address, fileFullName);
                    var message = await client.DeleteAsync(requestUri);

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
    }
}
