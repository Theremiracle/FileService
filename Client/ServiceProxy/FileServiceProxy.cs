using Common.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ServiceProxy
{
    public static class FileServiceProxy
    {
        public const string WebApiBaseAddress = @"http://localhost:54170/";

        public static readonly string TestDataFolder = GetTestDataFolder();
        public static readonly string UploadFolderPath = TestDataFolder + @"\Upload";
        public static readonly string DownloadFolderPath = TestDataFolder + @"\Download";
        public static readonly string FileToDownload = TestDataFolder + @"\FileToDownload.png";
        public static readonly string FileToUpload = TestDataFolder + @"\FileToUpload.jpg";

        private static string GetTestDataFolder()
        {
            var dir = AppDomain.CurrentDomain.BaseDirectory;
            var path = Directory.GetParent(dir).Parent.Parent.Parent.Parent.FullName + @"\TestData";
            return path;
        }

        private static string BuildUri(string address, string fileFullName)
        {
            var builder = new UriBuilder(address);
            if (!string.IsNullOrEmpty(fileFullName))
            {
                var query = HttpUtility.ParseQueryString(builder.Query);
                query["FileFullName"] = fileFullName;
                builder.Query = query.ToString();
            }

            string url = builder.ToString();

            return url;
        }

        public static async Task<bool> Test()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    string address = WebApiBaseAddress + "/api/fileservice/test";
                    var requestUri = BuildUri(address, string.Empty);
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

        public static async Task<bool> GetFile(string fileFullName)
        {
            try
            {
                FileUtil.CheckFileEixsts(fileFullName);
                using (var client = new HttpClient())
                {
                    string address = WebApiBaseAddress + "/api/file/image";
                    var requestUri = BuildUri(address, fileFullName);
                    var message = await client.GetAsync(requestUri);

                    if (message.IsSuccessStatusCode)
                    {
                        string saveFileFullName = DownloadFolderPath + @"\" + Path.GetFileName(fileFullName);
                        using (Stream contentStream = await message.Content.ReadAsStreamAsync(),
                            stream = new FileStream(saveFileFullName, FileMode.Create, FileAccess.Write, FileShare.None))
                        {
                            await contentStream.CopyToAsync(stream);
                        }
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
                FileUtil.CheckFileEixsts(uploadeFileFullName);
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
