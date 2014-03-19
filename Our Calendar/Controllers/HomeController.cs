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

            ViewBag.Message = "Welcome " + fullName + "to ASP.NET MVC!";

            return View();
        }

        public ActionResult About()
        {
            return View();
        }
    }
}
