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
        [Authorize]
        public ActionResult Manage()
        {
            if (Session["UserID"] != null)
            {
                List<EventModel> events = EventManageModel.GetAllEvents(Session["UserID"].ToString());
                ViewBag.Events = events;
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }            
            return View();
        }

        public ActionResult View(int userId, int month = 0, int year = 0)
        {
            if (month < 1 || month > 12) month = DateTime.Now.Month;
            if (year < 1 || year > 9999) year = DateTime.Now.Year;

            DateTime firstDayOfMonth = DateTime.Parse(month + "/1/" + year);

            ViewBag.startDay = (int) firstDayOfMonth.DayOfWeek;
            ViewBag.DaysInMonth = DateTime.DaysInMonth(year, month);

            List<EventModel> events = EventManageModel.GetAllEvents(userId.ToString(CultureInfo.InvariantCulture), month.ToString(CultureInfo.InvariantCulture), year.ToString(CultureInfo.InvariantCulture));

            ViewBag.Events = events;
            ViewBag.userID = userId;
            ViewBag.monthName = firstDayOfMonth.ToString("MMMM yyyy");
            if (month == 12)
            {
                ViewBag.nextMonth = 1;
                ViewBag.prevMonth = 11;
                ViewBag.nextYear = year + 1;
                ViewBag.prevYear = year;
            }
            else if (month == 1)
            {
                ViewBag.nextMonth = 2;
                ViewBag.prevMonth = 12;
                ViewBag.nextYear = year;
                ViewBag.prevYear = year - 1;
            }
            else
            {
                ViewBag.nextMonth = month + 1;
                ViewBag.nextYear = year;
                ViewBag.prevMonth = month - 1;
                ViewBag.prevYear = year;
            }
            return View("ViewCalendar", "_CalendarLayout");
        }
    }
}
