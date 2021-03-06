﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using MySql.Data.MySqlClient;

namespace Our_Calendar.Models
{
    public class EventManageModel
    {
        public static Boolean DeleteEvent(string eventID, string userID)
        {
            // Set condition of DELETE to match given eventID
            Dictionary<string, string> conditions = new Dictionary<string, string>()
            {
                { "eventID", eventID },
                { "userID", userID}
            };
            return DatabaseHelper.DatabaseHelper.DbDelete("Events", conditions);
        }

        public static Boolean SaveEvent(EventVModel eventInfo)
        {
            // Prepare values for INSERT into database
            Dictionary<string, string> values = new Dictionary<string, string>()
            {
                {"userID", eventInfo.UserID},
                {"eventName", eventInfo.EventName},
                {"eventDate", eventInfo.EventDate.ToString(CultureInfo.InvariantCulture)},
                {"month", eventInfo.EventDate.Month.ToString(CultureInfo.InvariantCulture)},
                {"day", eventInfo.EventDate.Day.ToString(CultureInfo.InvariantCulture)},
                {"year", eventInfo.EventDate.Year.ToString(CultureInfo.InvariantCulture)},
                {"time", eventInfo.EventTime},
                {"location", eventInfo.EventLocation},
                {"fullDesc", eventInfo.FullDesc},
                {"imageURL", eventInfo.EventImage}
            };

            // If an eventID is given then this is an update
            if (Convert.ToInt32(eventInfo.EventID) > 0)
            {
                // Set condition of UPDATE to match given eventID
                Dictionary<string, string> conditions = new Dictionary<string, string>() { { "eventID", eventInfo.EventID } };

                // Try UPDATE.
                return DatabaseHelper.DatabaseHelper.DbUpdate("Events", conditions, values);  
            }
            return DatabaseHelper.DatabaseHelper.DbInsert("Events", values);                      
        }

        public static EventVModel GetEventDetails(string eventID, string userID)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>()
            {
                { "eventID", eventID },
                { "userID", userID}
            };
            DataTable resultsTable = DatabaseHelper.DatabaseHelper.DbSelect("SELECT * FROM Events WHERE eventID = @eventID AND userID = @userID LIMIT 1", parameters);           

            EventVModel eventInfo = new EventVModel()
            {
                EventID = resultsTable.Rows[0]["eventID"].ToString(),
                UserID = resultsTable.Rows[0]["userID"].ToString(),
                EventName = resultsTable.Rows[0]["eventName"].ToString(),
                EventDate = Convert.ToDateTime(resultsTable.Rows[0]["eventDate"].ToString()),
                EventTime = resultsTable.Rows[0]["time"].ToString(),
                EventLocation = resultsTable.Rows[0]["location"].ToString(),
                FullDesc = resultsTable.Rows[0]["fullDesc"].ToString(),
                EventImage = resultsTable.Rows[0]["imageURL"].ToString()
            };

            return eventInfo;
        }

        public static List<EventModel> GetAllEvents(string userID)
        {
            List<EventModel> events = new List<EventModel> { };
            EventModel eventModel = new EventModel();

            Dictionary<string, string> parameters = new Dictionary<string, string>()
            {
                { "userID", userID }
            };
            DataTable resultsTable = DatabaseHelper.DatabaseHelper.DbSelect("SELECT day, eventName, eventID, eventDate FROM Events WHERE userID = @userID", parameters);

            for (int i = 0; i < resultsTable.Rows.Count; i++)
            {
                events.Add(new EventModel() { DayOfMonth = Convert.ToInt32(resultsTable.Rows[i]["day"]), EventName = resultsTable.Rows[i]["eventName"].ToString(), EventID = resultsTable.Rows[i]["eventID"].ToString(), Date = Convert.ToDateTime(resultsTable.Rows[i]["eventDate"].ToString()) });
            }
            return events;
        }

        // The int parameters should be Unix timestamps, use ConvertToTimestamp to get the time stamps.
        public static List<EventModel> GetAllEvents(string userID, string month, string year)
        {
            List<EventModel> events = new List<EventModel> {};
            EventModel eventModel = new EventModel();

            Dictionary<string, string> parameters = new Dictionary<string, string>()
            {
                { "userID", userID },
                { "month", month},
                { "year", year}
            };
            DataTable resultsTable = DatabaseHelper.DatabaseHelper.DbSelect("SELECT day, eventName, eventID, eventDate, time FROM Events WHERE userID = @userID AND month = @month AND year = @year", parameters);           

            for (int i = 0; i < resultsTable.Rows.Count; i++)
            {
                events.Add(new EventModel() { DayOfMonth = Convert.ToInt32(resultsTable.Rows[i]["day"]), EventName = resultsTable.Rows[i]["eventName"].ToString(), EventID = resultsTable.Rows[i]["eventID"].ToString(), Date = Convert.ToDateTime(resultsTable.Rows[i]["eventDate"].ToString()), EventTime = resultsTable.Rows[i]["time"].ToString()});
            }
            return events;
        }
    }

    public class EventModel
    {
        public string EventName { get; set; }
        public string EventID { get; set; }
        public int DayOfMonth { get; set; }
        public DateTime Date { get; set; }
        public string EventTime { get; set; }
    }

    public class EventVModel
    {
        [HiddenInput(DisplayValue = false)]
        public string UserID { get; set; }

        [HiddenInput(DisplayValue = false)]
        public string EventID { get; set; }

        [Required]
        [Display(Name = "Event Name")]
        public string EventName { get; set; }

        [Required]
        [Display(Name = "Date of Event")]
        public DateTime EventDate { get; set; }

        [Display(Name = "Time of Event")]
        public string EventTime { get; set; }

        [Display(Name = "Location of Event")]
        public string EventLocation { get; set; }

        [Display(Name = "Description of Event")]
        [DataType(DataType.MultilineText)]
        public string FullDesc { get; set; }

        [Display(Name = "Upload Event Image")]
        public string EventImage { get; set; }
    }

    public class ViewEventVModel
    {
        [Required]
        [HiddenInput(DisplayValue = false)]
        public int? EventID { get; set; }

        [Required]
        [Display(Name = "Event Name")]
        public string EventName { get; set; }

        [Required]
        [Display(Name = "Date of Event")]
        public string EventDate { get; set; }

        [Display(Name = "Time of Event")]
        public string EventTime { get; set; }

        [Display(Name = "Location of Event")]
        public string EventLocation { get; set; }

        [Display(Name = "Brief Description of Event")]
        public string ShortDesc { get; set; }

        [Display(Name = "Description of Event")]
        public string FullDesc { get; set; }

        public string EventImageURL { get; set; }
    }
}