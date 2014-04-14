using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Globalization;
using System.Security.Policy;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.Services.Description;
using MandrillApi.Model;
using MySql.Data.MySqlClient;


namespace Our_Calendar.Models
{
    public class UserManagementModel
    {        
        // Returns true if the password given matches the on in the database, else false.
        public static Boolean CheckPassword(LoginVModel logOnInfo)
        {
            // Encrypt the password, by hashing it. (Salt could be added later)
            string encryptedPassword = HashIT(logOnInfo.Password);

            // Prepare parameters for query and try query.
            Dictionary<string, string> conditions = new Dictionary<string, string>() { { "email", logOnInfo.Email } };
            DataTable resultsTable = DatabaseHelper.DatabaseHelper.DbSelect("SELECT password FROM Users WHERE email = @email LIMIT 1", conditions);

            // If query returns results and the password give matches 
            // the one in the database then return true.
            if (resultsTable.Rows.Count > 0)
            {
                if ((string)resultsTable.Rows[0]["password"] == encryptedPassword)
                {
                    return true;
                }
            }            
            return false;
        } 

        // Returns a boolean (true = success adding user, false = user not added)
        public static Boolean CreateUser(RegisterVModel registrationInfo)
        {
            // Create a unique code to be used for setting their password after
            // they confirm their e-mail address.
            Random random = new Random();
            string passcode = HashIT(random.Next().ToString(CultureInfo.InvariantCulture));            

            // Prepare values for INSERT into database
            Dictionary<string, string> values = new Dictionary<string, string>()
            {
                {"fullName", registrationInfo.FullName},
                {"email", registrationInfo.Email.ToLower()},
                {"passcode", passcode}
            };

            // Try to insert new user into the database
            if (DatabaseHelper.DatabaseHelper.DbInsert("Users", values))
            {
                // Success adding user to database, create e-mail message
                string message = "Thank you for registering for an Our Calander account. Please verify your e-mail address by going to http://ourcal.azurewebsites.net/account/setpassword?passcode=" + HttpUtility.UrlEncode(passcode);

                // Try to send account setup e-mail
                if (SendEmail(message, "Our Calendar App - Account Setup", registrationInfo.Email, registrationInfo.FullName))
                {
                    // Success, return true.
                    return true;
                }
            }
            // Something failed, return false.
            return false;
        }

        // Creates SHA256 hashes of the given string.
        public static string HashIT(string what2Hash)
        {            
            // Next 3 lines create hash.
            byte[] raw = System.Text.Encoding.Unicode.GetBytes(what2Hash);
            System.Security.Cryptography.HashAlgorithm hashAlgo = new System.Security.Cryptography.SHA256Managed();
            byte[] hashed = hashAlgo.ComputeHash(raw);

            // Convert byte hash to string
            string hashedString = Convert.ToBase64String(hashed); 
            
            return hashedString;
        }

        // Returns user info from database used on the manage user account page.
        public static ManageAcctVModel ManageAcct(string userID)
        {
            Dictionary<string, string> conditions = new Dictionary<string, string>() { { "userID", userID } };
            DataTable resultsTable = DatabaseHelper.DatabaseHelper.DbSelect("SELECT fullName, email FROM Users WHERE userID = @userID LIMIT 1", conditions);

            ManageAcctVModel model = new ManageAcctVModel();

            if (resultsTable.Rows.Count > 0)
            {
                model.FullName = resultsTable.Rows[0]["fullName"].ToString();
                model.Email = resultsTable.Rows[0]["email"].ToString();
            }
            return model;
        }

        // Updates user info in the database (returns true on success or false on failure.)
        public static Boolean ManageAcct(ManageAcctVModel model, string userID)
        {
            // Set up values and conditions for database update.
            Dictionary<string, string> values = new Dictionary<string, string>() { { "fullName", model.FullName } };
            Dictionary<string, string> conditions = new Dictionary<string, string>() { { "userID", userID } };

            // Try to update user in the database.
            return DatabaseHelper.DatabaseHelper.DbUpdate("Users", conditions, values);
        }

        // Used to send e-mails to the user (returns true when e-mail is successfully sent, false on failure.)
        public static Boolean SendEmail(string emailMessage, string emailSubject, string toEmail, string toName)
        {
            // APPSETTING_MANDRILL_API_KEY is from Azure server config, replace with your API key if you deploy to another server
            string APIKey = Environment.GetEnvironmentVariable("APPSETTING_MANDRILL_API_KEY");

            // Create a MandrillApi instance.
            MandrillApi.MandrillApi _mapi = new MandrillApi.MandrillApi(APIKey, "json");

            // Create e-mail message.
            object message = new
            {
                html = emailMessage,
                text = emailMessage,
                subject = emailSubject,
                from_email = "OurCalendar@blakebuckit.com",
                from_name = "Our Calendar App",
                to = new List<Recipient> { new Recipient { email = toEmail, name = toName } }
            };

            // Attempt to send e-mail
            List<MandrillApi.Model.RecipientReturn> returnValue = _mapi.send(message);

            // Returns false if confirmation e-mail was not sent
            return returnValue[0].status == "sent";
        }

        // Creates a new passcode and send user a password reset e-mail.
        public static Boolean SendPasswordReset(ForgotPasswordVModel AccountInfo)
        {
            // Create a unique code to be used for setting their password            
            Random random = new Random();
            string passcode = HashIT(random.Next().ToString(CultureInfo.InvariantCulture));

            // Prepare database values and conditions for setting the passcode in the database.
            Dictionary<string, string> values = new Dictionary<string, string>() { { "passcode", passcode } };
            Dictionary<string, string> conditions = new Dictionary<string, string>() { { "email", AccountInfo.Email.ToLower() } };

            // Try to set passcode in the database.
            if (!DatabaseHelper.DatabaseHelper.DbUpdate("Users", conditions, values)) return false;

            // Create e-mail message
            string message = "Someone has requested a password reset on your Our Calendar account. If this was you please follow this link: http://ourcal.azurewebsites.net/account/setpassword?passcode=" + HttpUtility.UrlEncode(passcode) + " otherwise please delete this e-mail.";

            // Attempt to send e-mail message.
            return SendEmail(message, "Our Calendar App - Password Reset", AccountInfo.Email, AccountInfo.Email);
        }

        // Actually updates the user's password (returns true on success, false on failure).
        public static Boolean SetPassword(string password, string passcode = null, string userID = "")
        {
            // Make sure that either the passcode or UserID is set.
            if (passcode == null && userID == "") return false;

            // Hash the selected password to protect it in the database
            string encryptedPassword = HashIT(password);
            
            // Prepare password for update in database
            Dictionary<string, string> values = new Dictionary<string, string>() { { "password", encryptedPassword }, { "passcode", "" } };
            
            // If passcode is given (use it)
            if (passcode != null)
            {
                // Prepare database update condition
                Dictionary<string, string> conditions = new Dictionary<string, string>() { { "passcode", passcode } };

                DataTable resultsTable = DatabaseHelper.DatabaseHelper.DbSelect("SELECT userID FROM Users WHERE passcode = @passcode", conditions);
                userID = resultsTable.Rows[0]["userID"] == null ? "0" : resultsTable.Rows[0]["userID"].ToString();
            }

            // Prepare database update condition
            Dictionary<string, string> conditions4Update = new Dictionary<string, string>() { { "userID", userID } };
            // Try to update database.
            return DatabaseHelper.DatabaseHelper.DbUpdate("Users", conditions4Update, values);           
        }

        // Returns a integer representing the UserID of the user 
        // or 0 indicating the user doesn't exist.
        public static int UserExist(string userEmail)
        {
            // Prepare parameters for query and try query.
            Dictionary<string, string> conditions = new Dictionary<string, string>() { { "email", userEmail } };            
            DataTable resultsTable = DatabaseHelper.DatabaseHelper.DbSelect("SELECT userID FROM Users WHERE email = @email LIMIT 1", conditions);

            // Returns the UserID of the user or 0 if user was not found.
            return resultsTable.Rows.Count <= 0 ? 0 : Convert.ToInt32(resultsTable.Rows[0]["userID"]);
        }                
    }


    public class ForgotPasswordVModel
    {
        [Required]
        [Display(Name = "Email Address")]
        public string Email { get; set; }
    }

    public class LoginVModel
    {
        [Required]
        [Display(Name = "Email Address")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }        
    }

    public class ManageAcctVModel
    {
        [Required]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email Address")]
        public string Email { get; set; }
    }

    public class RegisterVModel
    {
        [Required]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email address")]
        public string Email { get; set; }
    }

    public class SetPasswordVModel
    {
        [HiddenInput]
        public string Passcode { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [System.Web.Mvc.Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
