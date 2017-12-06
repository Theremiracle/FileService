using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Contract;

namespace Server.AspWebApi.Controllers
{
    public class FileServiceController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage Test()
        {
            return Request.CreateResponse(HttpStatusCode.OK, "Test");
        }

        [HttpPost]
        [Route("api/file/image")]
        public async Task<HttpResponseMessage> SaveFile([FromUri] FileRequest fileModel)
        {
            try
            {
                var fileFullName = fileModel.FileFullName;
                var fileFolder = Path.GetDirectoryName(fileFullName);
                FileUtil.CheckDirectoryEixsts(fileFolder);

                var httpRequest = HttpContext.Current.Request;
                if (httpRequest.Files.Count < 1)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest);
                }

                foreach (string file in httpRequest.Files)
                {
                    var postedFile = httpRequest.Files[file];
                    if (postedFile != null)
                    {
                        await Task.Factory.StartNew(() => postedFile.SaveAs(fileFullName));
                        break;
                    }
                }

                return Request.CreateResponse(HttpStatusCode.Created);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }

        [HttpGet]
        [Route("api/file/image")]
        public HttpResponseMessage Get([FromUri] FileRequest fileModel)
        {
            try
            {
                var filePath = fileModel.FileFullName;
                FileUtil.CheckFileEixsts(filePath);

                var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                var response = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StreamContent(fileStream)
                };
                response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("Attachment");
                response.Content.Headers.ContentDisposition.FileName = Path.GetFileName(filePath);
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                response.Content.Headers.Expires = new DateTimeOffset(DateTime.Now.AddDays(-1));

                return response;
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }

        [HttpDelete]
        [Route("api/file/image")]
        public async Task<HttpResponseMessage> Delete([FromUri] FileRequest fileModel)
        {
            try
            {
                var filePath = fileModel.FileFullName;
                if (FileUtil.DoesFileExist(filePath))
                {
                    await Task.Factory.StartNew(() => File.Delete(filePath));
                }
                
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }        

        [HttpPut]
        [Route("api/file/image")]
        public async Task<HttpResponseMessage> Update([FromUri] FileRequest fileModel)
        {
            try
            {
                var filePath = fileModel.FileFullName;
                FileUtil.CheckFileEixsts(filePath);

                if (FileUtil.DoesFileExist(filePath))
                {
                    await Task.Factory.StartNew(() => File.Delete(filePath));
                }

                var message = await SaveFile(fileModel);

                return message;
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }
    }
}
