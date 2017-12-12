using Server.AspWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Server.AspWebApi.Controllers
{
    public class BookMgrController : ApiController
    {
        public static readonly string AppDataFolder = AppDomain.CurrentDomain.GetData("DataDirectory").ToString();

        private readonly BookFactory _bookFactory = new BookFactory();



    }
}
