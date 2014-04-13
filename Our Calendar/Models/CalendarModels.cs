using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Our_Calendar.Models
{
    public class CalendarModel
    {
    }

    public class CalendarEventsModel
    {
        [Required]
        public int userID { get; set; }

        [Required]
        public string EventName { get; set; }

        public string EventDate { get; set; }

        public string EventTime { get; set; }

        public string EventLocation { get; set; }

        public string ShortDesc { get; set; }

        public string FullDesc { get; set; }

        public string EventImageURL { get; set; }
    }
}