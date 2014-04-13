using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Our_Calendar.Models;

namespace Our_Calendar.Controllers
{
    public class EventController : Controller
    {
        // Add Event 
        // GET: /Event/Add
        public ActionResult Add()
        {
            return View();
        }

        // Add Event (Handler)
        // POST: /Event/Add
        [HttpPost]
        public ActionResult Add(EventVModel model)
        {
            // Try to save results
            if (EventManageModel.SaveEvent(model))
            {
                // Success, redirect to calendar management page
                return RedirectToAction("Manage", "Calendar");
            }
            return View();
        }

        // Delete Event (Handler)
        // POST: /Event/Delete
        [HttpPost]
        public ActionResult Delete(string eventID, string userID)
        {
            // Try to delete event
            EventManageModel.DeleteEvent(eventID, userID);

            // Redirect to calendar management page.
            return RedirectToAction("Manage", "Calendar");           
        }

        // Edit Event 
        // GET: /Event/Edit
        public ActionResult Edit(string eventID, string userID)
        {
            EventVModel model = EventManageModel.GetEventDetails(eventID, userID);
            return View(model);
        }

        // Edit Event (Handler)
        // POST: /Event/Edit
        [HttpPost]
        public ActionResult Edit(EventVModel model)
        {
            // Try to save results
            if (EventManageModel.SaveEvent(model))
            {
                // Success, redirect to calendar management page
                return RedirectToAction("Manage", "Calendar");
            }
            return View();
        }

        // View Event 
        // GET: /Event/View
        public ActionResult View(string eventID, string userID)
        {
            EventVModel model = EventManageModel.GetEventDetails(eventID, userID);
            return View(model);
        }
    }
}
