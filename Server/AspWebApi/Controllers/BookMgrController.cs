using Common.Contract.BookMgr;
using Common.Infrastructure.Entities;
using Newtonsoft.Json;
using Server.AspWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Server.AspWebApi.Controllers
{
    public class BookMgrController : ApiController
    {
        public static readonly string AppDataFolder = AppDomain.CurrentDomain.GetData("DataDirectory").ToString();

        private readonly BookFactory _bookFactory = new BookFactory();

        #region Book API
        [HttpGet]
        [Route("api/book")]
        public HttpResponseMessage GetAllBooks()
        {
            try
            {
                var books = _bookFactory.GetAllBooks();
                var response = Request.CreateResponse(HttpStatusCode.OK);
                response.Content = new StringContent(JsonConvert.SerializeObject(books), System.Text.Encoding.UTF8, "application/json");

                return response;
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }

        [HttpPost]
        [Route("api/book/get")]
        public HttpResponseMessage GetBooks([FromUri] GetBooksRequest bookReq)
        {
            try
            {
                var books = _bookFactory.GetAllBooks();
                var response = Request.CreateResponse(HttpStatusCode.OK);
                response.Content = new StringContent(JsonConvert.SerializeObject(books), System.Text.Encoding.UTF8, "application/json");

                return response;
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }

        [HttpPost]
        [Route("api/book")]
        public HttpResponseMessage AddBooks([FromUri] AddBooksRequest bookReq)
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.Created);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }        

        [HttpDelete]
        [Route("api/book")]
        public HttpResponseMessage DeleteBooks([FromUri] DeleteBookRequest bookReq)
        {
            try
            {
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }

        [HttpPut]
        [Route("api/book")]
        public HttpResponseMessage UpdateBooks([FromUri] UpdateBookRequest bookReq)
        {
            try
            {
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }
        #endregion

    }
}
