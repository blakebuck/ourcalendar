using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
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
            EventVModel model = new EventVModel();
            model.EventID = "0";
            model.UserID = Session["UserID"].ToString();
            return View(model);
        }

        // Add Event (Handler)
        // POST: /Event/Add
        [HttpPost]
        public ActionResult Add(EventVModel model, HttpPostedFileBase file)
        {
            try
            {
                if (file != null)
                {
                    if (file.ContentType == "image/jpeg" || file.ContentType == "image/png")
                    {
                        if (file.ContentLength < 1048576)
                        {
                            string filename = Path.GetFileName(file.FileName);

                            Random random = new Random();
                            filename += random.Next().ToString(CultureInfo.InvariantCulture);
                            string hashedname = UserManagementModel.HashIT(filename, true);

                            filename = file.ContentType == "image/jpeg" ? hashedname + ".jpg" : hashedname + ".png";

                            file.SaveAs(Server.MapPath("~/uploads/") + filename);
                            model.EventImage = filename;

                            // Try to save results
                            if (EventManageModel.SaveEvent(model))
                            {
                                SessionAlert.SessionAlert.CreateAlert("Your event is going to be awesome!!",
                                    "Woot Woot! ", "success");
                                // Success, redirect to calendar management page
                                return RedirectToAction("Manage", "Calendar");
                            }
                        }
                        else
                        {
                            SessionAlert.SessionAlert.CreateAlert("I can't fit your big file (it has to be less than 1 MB).", "The box is too small!");
                        }                           
                    }
                    else
                    {
                        SessionAlert.SessionAlert.CreateAlert("We can only accept jpeg or png, sorry.", "Oh that kind of image!?!");
                    }                        
                }
                else
                {
                    // Try to save results
                    if (EventManageModel.SaveEvent(model))
                    {
                        SessionAlert.SessionAlert.CreateAlert("Your event is going to be awesome!!", "Woot Woot! ", "success");
                        // Success, redirect to calendar management page
                        return RedirectToAction("Manage", "Calendar");
                    }
                }
            }
            catch (Exception ex)
            {
                SessionAlert.SessionAlert.CreateAlert("There was a problem saving your event.", "Oh No!");
            }
            return View();
        }

        // Delete Event (Handler)
        public ActionResult Delete(string eventID)
        {
            // 
            string userID = Session["UserID"].ToString();

            // Try to delete event
            EventManageModel.DeleteEvent(eventID, userID);

            // Redirect to calendar management page.
            return RedirectToAction("Manage", "Calendar");           
        }

        // Edit Event 
        // GET: /Event/Edit
        public ActionResult Edit(string eventID)
        {
            string userID = Session["UserID"].ToString();
            EventVModel model = EventManageModel.GetEventDetails(eventID, userID);
            return View(model);
        }

        // View Event 
        // GET: /Event/View
        public ActionResult Details(string eventID, string userID)
        {
            EventVModel model = EventManageModel.GetEventDetails(eventID, userID);
            return View(model);
        }
    }
}
