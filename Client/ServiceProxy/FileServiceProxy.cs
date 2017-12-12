using Client.ServiceProxy;
using Common.Contract;
using Common.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;

namespace ServiceProxy
{
    public class FileServiceProxy : ServiceProxyBase, IFileService
    {
        public static readonly string UploadFolderPath = TestDataFolder + @"\Upload";
        public static readonly string DownloadFolderPath = TestDataFolder + @"\Download";
        public static readonly string FileToDownload = TestDataFolder + @"\FileToDownload.png";
        public static readonly string FileToUpload = TestDataFolder + @"\FileToUpload.jpg";      

        #region File
        public async Task<bool> GetFileAsync(string fileFullName)
        {
            FileUtil.CheckFileEixsts(fileFullName);
            string address = WebApiBaseAddress + "/api/file";
            var requestUri = BuildUri(address, fileFullName);
            var message = await _client.GetAsync(requestUri);

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

        public async Task<bool> SaveFileAsync(string uploadeFileFullName, string fileUploadFolder)
        {
            FileUtil.CheckFileEixsts(uploadeFileFullName);
            using (var content = new MultipartFormDataContent())
            {
                var fileContent = new StreamContent(File.OpenRead(uploadeFileFullName));
                fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = uploadeFileFullName
                };
                content.Add(fileContent);

                string fileFullNameToBeUploaded = $"{fileUploadFolder}" + @"\" + Path.GetFileName(uploadeFileFullName);
                string address = WebApiBaseAddress + "/api/file";
                var requestUri = BuildUri(address, fileFullNameToBeUploaded);
                var message = await _client.PostAsync(requestUri, content);

                if (message.IsSuccessStatusCode)
                {
                    return true;
                }

                throw new Exception(message.ToString());
            }
        }

        public async Task<bool> DeleteFileAsync(string fileFullName)
        {
            string address = WebApiBaseAddress + "/api/file";
            var requestUri = BuildUri(address, fileFullName);
            var message = await _client.DeleteAsync(requestUri);

            if (message.IsSuccessStatusCode)
            {
                return true;
            }

            throw new Exception(message.ToString());
        }
        #endregion

        #region Image
        public async Task<Stream> GetImageAsync(string fileFullName)
        {
            FileUtil.CheckFileEixsts(fileFullName);
            string address = WebApiBaseAddress + "/api/file/image";
            var requestUri = BuildUri(address, fileFullName);
            var message = await _client.GetAsync(requestUri);

            if (message.IsSuccessStatusCode)
            {
                string saveFileFullName = DownloadFolderPath + @"\" + Path.GetFileName(fileFullName);
                return await message.Content.ReadAsStreamAsync();
            }

            throw new Exception(message.ToString());
        }

        public async Task<bool> SaveImageAsync(Byte[] bytes, string filePathUploadedTo)
        {
            using (var content = new MultipartFormDataContent())
            {
                var fileContent = new StreamContent(new MemoryStream(bytes));
                fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = "Image"
                };
                content.Add(fileContent);
                
                string address = WebApiBaseAddress + "/api/file/image";
                var requestUri = BuildUri(address, filePathUploadedTo);
                var message = await _client.PostAsync(requestUri, content);

                if (message.IsSuccessStatusCode)
                {
                    return true;
                }

                throw new Exception(message.ToString());
            }
        }
        #endregion

        private static string BuildUri(string address, string fileFullName)
        {
            var dict = new Dictionary<string, string> { { "FileFullName", fileFullName } };
            return BuildUri(address, dict);
        }
    }
}
