using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Our_Calendar.Controllers
{
    public class EventController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Edit()
        {
            return View();
        }

        public ActionResult Add()
        {
            return View();
        }

        public ActionResult ViewEvent(int eventID)
        {

            return View();
        }
    }
}
