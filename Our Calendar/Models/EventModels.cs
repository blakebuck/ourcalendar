using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using MySql.Data.MySqlClient;

namespace Our_Calendar.Models
{
    public class EventManageModel
    {
        public static Boolean CreateEvent(AddEventVModel eventInfo)
        {
            // Create database connection
            MySqlConnection connection = new MySqlConnection(Environment.GetEnvironmentVariable("APPSETTING_MYSQL_CONNECTION_STRING"));
            MySqlCommand cmd;
            connection.Open();

            int unixDate = ConvertToTimestamp(Convert.ToDateTime(eventInfo.EventDate));

            try
            {
                // Try to add user to the Users table in the database
                cmd = connection.CreateCommand();
                cmd.CommandText = "INSERT INTO CalendarEvents (userID, eventName, eventDate, eventTime, eventLocation, shortDesc, fullDesc, eventImageURL) VALUES (@userID, @eventName, @eventDate, @eventTime, @eventLocation, @shortDesc, @fullDesc, @eventImageURL)";
                cmd.Parameters.AddWithValue("@userID", eventInfo.UserID);
                cmd.Parameters.AddWithValue("@eventName", eventInfo.EventName);
                cmd.Parameters.AddWithValue("@eventDate", unixDate);
                cmd.Parameters.AddWithValue("@eventTime", eventInfo.EventTime);
                cmd.Parameters.AddWithValue("@eventLocation", eventInfo.EventLocation);
                cmd.Parameters.AddWithValue("@shortDesc", eventInfo.ShortDesc);
                cmd.Parameters.AddWithValue("@fullDesc", eventInfo.FullDesc);
                cmd.Parameters.AddWithValue("@eventImageURL", eventInfo.EventImage);
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception)
            {
                return false;
            }            
        }

        public static DataTable GetEventDetails(int eventID)
        {
            DataTable dt = new DataTable();

            MySqlConnection connection = new MySqlConnection(Environment.GetEnvironmentVariable("APPSETTING_MYSQL_CONNECTION_STRING"));
            MySqlCommand cmd;
            connection.Open();
            try
            {
                cmd = connection.CreateCommand();
                cmd.CommandText = "SELECT * FROM Users WHERE eventID = @eventID LIMIT 1";
                cmd.Parameters.AddWithValue("@eventID", eventID);
                MySqlDataAdapter a = new MySqlDataAdapter(cmd);                
                a.Fill(dt);
            }
            catch (Exception)
            {
                return dt;
            }
            return dt;
        }

        public static DataTable GetAllEvents(int userID)
        {
            DataTable dt = new DataTable();

            MySqlConnection connection = new MySqlConnection(Environment.GetEnvironmentVariable("APPSETTING_MYSQL_CONNECTION_STRING"));
            MySqlCommand cmd;
            connection.Open();
            try
            {
                cmd = connection.CreateCommand();
                cmd.CommandText = "SELECT * FROM Users WHERE userID = @userID";
                cmd.Parameters.AddWithValue("@userID", userID);
                MySqlDataAdapter a = new MySqlDataAdapter(cmd);
                a.Fill(dt);
            }
            catch (Exception)
            {
                return dt;
            }
            return dt;
        }

        // The int parameters should be Unix timestamps, use ConvertToTimestamp to get the time stamps.
        public static List<EventModel> GetAllEvents(int userID, int month, int year)
        {
            DataTable dt = new DataTable();
            List<EventModel> events = new List<EventModel> {};
            EventModel eventModel = new EventModel();

            MySqlConnection connection = new MySqlConnection(Environment.GetEnvironmentVariable("APPSETTING_MYSQL_CONNECTION_STRING"));
            MySqlCommand cmd;
            connection.Open();
            try
            {
                cmd = connection.CreateCommand();
                cmd.CommandText = "SELECT * FROM Events WHERE userID = @userID AND month = @month AND year = @year";
                cmd.Parameters.AddWithValue("@userID", userID);
                cmd.Parameters.AddWithValue("@month", month);
                cmd.Parameters.AddWithValue("@year", year);  
                MySqlDataAdapter a = new MySqlDataAdapter(cmd);
                a.Fill(dt);
            }
            catch (Exception)
            {
                return events;
            }

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                events.Add(new EventModel(){DayOfMonth = Convert.ToInt32(dt.Rows[i]["day"]), EventName = dt.Rows[i]["eventName"].ToString()});
            }
            return events;
        }

        public static int[] GetMonthTimeStamps(DateTime date)
        {
            // Create integer array to hold timestamps
            int[] timestamps = {};
            
            // Get timestamp for start of the month
            timestamps[0] = ConvertToTimestamp(date);

            // Get timestamp for end of the month
            DateTime nextMonth = date.AddMonths(1);
            timestamps[1] = ConvertToTimestamp(nextMonth);

            return timestamps;
        }

        public static int ConvertToTimestamp(DateTime value)
        {
            //create Timespan by subtracting the value provided from
            //the Unix Epoch
            TimeSpan span = (value - new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime());

            //return the total seconds (which is a UNIX timestamp)
            return (int)span.TotalSeconds;
        }
    }

    public class EventModel
    {
        public string EventName { get; set; }
        public int DayOfMonth { get; set; }
    }

    public class AddEventVModel
    {
        [HiddenInput(DisplayValue = false)]
        public int? UserID { get; set; }

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

        [Display(Name = "Upload Event Image")]
        public HttpPostedFileBase EventImage { get; set; }
    }

    public class EditEventVModel
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

        [Display(Name = "Upload Event Image")]
        public HttpPostedFileBase EventImage { get; set; }
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