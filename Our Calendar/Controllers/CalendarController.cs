using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Our_Calendar.Models;

namespace Our_Calendar.Controllers
{
    public class CalendarController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Manage()
        {
            return View();
        }

        public ActionResult ViewCalendar(int userId, int month = 0, int year = 0)
        {
            if (month < 1 || month > 12) month = DateTime.Now.Month;
            if (year < 1 || year > 9999) year = DateTime.Now.Year;

            DateTime firstDayOfMonth = DateTime.Parse(month + "/1/" + year);

            ViewBag.startDay = (int) firstDayOfMonth.DayOfWeek;

            ViewBag.DaysInMonth = DateTime.DaysInMonth(year, month);

            List<EventModel> events = EventManageModel.GetAllEvents(userId, month, year);

            ViewBag.Events = events;

            return View("ViewCalendar", "_CalendarLayout");
        }
    }
}
