using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Our_Calendar.SessionAlert
{
    public class SessionAlert
    {
        public static void CreateAlert(string alertMessage, string alertHeading = "", string alertType = "danger")
        {
            HttpContext httpContext = HttpContext.Current;
            httpContext.ApplicationInstance.Session["Alert"] = alertMessage;
            httpContext.ApplicationInstance.Session["AlertHeading"] = alertHeading;
            httpContext.ApplicationInstance.Session["AlertType"] = alertType;
        }
    }
}