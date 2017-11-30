using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Runtime.Remoting.Activation;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;
using Contract;

namespace TestWebApi.Controllers
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
        public async Task<HttpResponseMessage> SaveFile([FromUri] FileModel fileModel)
        {
            try
            {
                var fileFullName = fileModel.FileFullName;
                var fileFolder = Path.GetDirectoryName(fileFullName);
                FileModel.CheckDirectoryEixsts(fileFolder);

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
        public HttpResponseMessage Get([FromUri] FileModel fileModel)
        {
            try
            {
                var filePath = fileModel.FileFullName;
                FileModel.CheckFileEixsts(filePath);

                using (var fileStream = new FileStream(filePath, FileMode.Open))
                {
                    var response = new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new StreamContent(fileStream)
                    };
                    response.Content.Headers.ContentType = new MediaTypeHeaderValue("image/png");
                    return response;
                }
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }

        [HttpDelete]
        [Route("api/file/image")]
        public async Task<HttpResponseMessage> Delete([FromUri] FileModel fileModel)
        {
            try
            {
                var filePath = fileModel.FileFullName;
                if (FileModel.DoesFileExist(filePath))
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
        public async Task<HttpResponseMessage> Update([FromUri] FileModel fileModel)
        {
            try
            {
                var filePath = fileModel.FileFullName;
                FileModel.CheckFileEixsts(filePath);

                if (FileModel.DoesFileExist(filePath))
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
