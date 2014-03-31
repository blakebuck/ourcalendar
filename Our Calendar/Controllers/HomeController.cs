using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Our_Calendar.Models;

namespace Our_Calendar.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            string fullName = CallDatabase.ReturnName();

            ViewBag.Message = "Welcome to ASP.NET MVC!";

            return View();
        }

        public ActionResult About()
        {
            string APIKey = "MSZero_69BqI7L6N2p6jow";

            MandrillApi.MandrillApi _mapi = new MandrillApi.MandrillApi(APIKey, "json");

            ViewBag.Mandrill = _mapi.Ping();

            return View();
        }
    }
}
