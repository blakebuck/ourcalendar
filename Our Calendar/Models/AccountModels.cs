using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Security;
using MandrillApi.Model;
using MySql.Data.MySqlClient;

namespace Our_Calendar.Models
{
    public class UserManagementModel
    {
        public static Boolean CheckPassword(LoginVModel logOnInfo)
        {
            // Encrypt the password, by hashing it. (Salt could be add later)
            byte[] password = System.Text.Encoding.Unicode.GetBytes(logOnInfo.Password);
            System.Security.Cryptography.HashAlgorithm hashAlgo = new System.Security.Cryptography.SHA256Managed();
            byte[] hashedPassword = hashAlgo.ComputeHash(password);

            string encryptedPassword = Convert.ToBase64String(hashedPassword); //System.Text.Encoding.UTF8.GetString(hashedPassword);

            MySqlConnection connection = new MySqlConnection(Environment.GetEnvironmentVariable("APPSETTING_MYSQL_CONNECTION_STRING"));
            MySqlCommand cmd;
            connection.Open();
            try
            {
                cmd = connection.CreateCommand();
                cmd.CommandText = "SELECT password FROM Users WHERE email = @email LIMIT 1";
                cmd.Parameters.AddWithValue("@email", logOnInfo.Email.ToLower());
                MySqlDataAdapter a = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                a.Fill(dt);

                if ((string)dt.Rows[0][0] == encryptedPassword)
                {
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
            return false;
        } 

        // Returns a boolean (true = success adding user, false = user not added)
        public static Boolean CreateUser(RegisterVModel registrationInfo)
        {
            // Create a unique code to be used for setting their password after
            // they confirm their e-mail address.
            Random random = new Random();
            System.Security.Cryptography.HashAlgorithm hashAlgo = new System.Security.Cryptography.SHA256Managed();
            string passcode = Convert.ToBase64String(hashAlgo.ComputeHash(System.Text.Encoding.Unicode.GetBytes(random.Next().ToString(CultureInfo.InvariantCulture))));

            // Create database connection APPSETTING_MYSQL_CONNECTION_STRING is from Azure server config 
            // you can replace it with your connection string if deploying to a different server.
            MySqlConnection connection = new MySqlConnection(Environment.GetEnvironmentVariable("APPSETTING_MYSQL_CONNECTION_STRING"));
            
            try
            {
                // Open connection to Database
                connection.Open();

                // Try to add user to the Users table in the database
                MySqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "INSERT INTO Users(fullName, email, passcode) VALUES (@fullName, @email, @passcode)";
                cmd.Parameters.AddWithValue("@fullName", registrationInfo.FullName);
                cmd.Parameters.AddWithValue("@email", registrationInfo.Email.ToLower());
                cmd.Parameters.AddWithValue("@passcode", passcode);
                cmd.ExecuteNonQuery();

                // Close connection
                connection.Close();
            }
            catch (Exception)
            {
                // Something failed while trying to add user
                return false;
            }

            // APPSETTING_MANDRILL_API_KEY is from Azure server config, replace with your API key if you deploy to another server
            string APIKey = Environment.GetEnvironmentVariable("APPSETTING_MANDRILL_API_KEY");

            // Create a MandrillApi instance.
            MandrillApi.MandrillApi _mapi = new MandrillApi.MandrillApi(APIKey, "json");

            // Create e-mail message.
            object message = new
            {
                html = "Thank you for registering for an Our Calander account. Please verify your e-mail address by going to http://ourcal.azurewebsites.net/account/setpassword/" + passcode,
                text = "Thank you for registering for an Our Calander account. Please verify your e-mail address by going to http://ourcal.azurewebsites.net/account/setpassword/" + passcode,
                subject = "Account Setup",
                from_email = "OurCalendar@blakebuckit.com",
                from_name = "Our Calendar App",
                to = new List<Recipient>{new Recipient {email = registrationInfo.Email, name = registrationInfo.FullName}}
            };

            // Attempt to send e-mail
            List<MandrillApi.Model.RecipientReturn> returnValue = _mapi.send(message);

            // Returns false if confirmation e-mail was not sent
            return returnValue[0].status == "sent";
        }

        public static Boolean SendPasswordReset(ForgotPasswordVModel AccountInfo)
        {
            // Create a unique code to be used for setting their password            
            Random random = new Random();
            System.Security.Cryptography.HashAlgorithm hashAlgo = new System.Security.Cryptography.SHA256Managed();
            string passcode = Convert.ToBase64String(hashAlgo.ComputeHash(System.Text.Encoding.Unicode.GetBytes(random.Next().ToString(CultureInfo.InvariantCulture))));

            // Create database connection APPSETTING_MYSQL_CONNECTION_STRING is from Azure server config 
            // you can replace it with your connection string if deploying to a different server.
            MySqlConnection connection = new MySqlConnection(Environment.GetEnvironmentVariable("APPSETTING_MYSQL_CONNECTION_STRING"));

            try
            {
                // Open connection to Database
                connection.Open();

                // Set the passcode in the database
                MySqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "UPDATE Users SET passcode = @passcode WHERE email = @email";
                cmd.Parameters.AddWithValue("@passcode", passcode);
                cmd.Parameters.AddWithValue("@email", AccountInfo.Email.ToLower());
                cmd.ExecuteNonQuery();

                // Close connection
                connection.Close();
            }
            catch (Exception)
            {
                // Something failed while trying to add passcode
                return false;
            }

            // APPSETTING_MANDRILL_API_KEY is from Azure server config, replace with your API key if you deploy to another server
            string APIKey = Environment.GetEnvironmentVariable("APPSETTING_MANDRILL_API_KEY");

            // Create a MandrillApi instance.
            MandrillApi.MandrillApi _mapi = new MandrillApi.MandrillApi(APIKey, "json");

            // Create e-mail message.
            object message = new
            {
                html = "Someone has requested a password reset on your Our Calendar account. If this was you please follow this link: http://ourcal.azurewebsites.net/account/setpassword/" + passcode + " otherwise please delete this e-mail.",
                text = "Someone has requested a password reset on your Our Calendar account. If this was you please follow this link: http://ourcal.azurewebsites.net/account/setpassword/" + passcode + " otherwise please delete this e-mail.",
                subject = "Our Calendar App - Password Reset",
                from_email = "OurCalendar@blakebuckit.com",
                from_name = "Our Calendar App",
                to = new List<Recipient> { new Recipient { email = AccountInfo.Email } }
            };

            // Attempt to send e-mail
            List<MandrillApi.Model.RecipientReturn> returnValue = _mapi.send(message);

            // Returns false if confirmation e-mail was not sent
            return returnValue[0].status == "sent";
        }

        public static Boolean SetPassword(string password, string passcode = null, int userID = 0)
        {
            // Make sure that either the passcode or UserID is set.
            if (passcode == null && userID == 0) return false;

            // Hash the selected password to protect it in the database
            System.Security.Cryptography.HashAlgorithm hashAlgo = new System.Security.Cryptography.SHA256Managed();
            string encryptedPassword = Convert.ToBase64String(hashAlgo.ComputeHash(System.Text.Encoding.Unicode.GetBytes(password)));

            // Create database connection APPSETTING_MYSQL_CONNECTION_STRING is from Azure server config 
            // you can replace it with your connection string if deploying to a different server.
            MySqlConnection connection = new MySqlConnection(Environment.GetEnvironmentVariable("APPSETTING_MYSQL_CONNECTION_STRING"));

            try
            {
                // Open connection to Database
                connection.Open();

                // Try to save the user's password in the database
                MySqlCommand cmd = connection.CreateCommand();

                // If a passcode was given use it
                if (passcode != null)
                {
                    cmd.CommandText = "UPDATE Users SET passcode = '', password = @password WHERE passcode = @passcode";
                    cmd.Parameters.AddWithValue("@passcode", passcode);
                }
                else
                {
                    // If no passcode was given try userID
                    cmd.CommandText = "UPDATE Users SET passcode = '', password = @password WHERE UserID = @userID";
                    cmd.Parameters.AddWithValue("@userID", userID);
                }
                // Bind the encrypted password to the password parameter in the query.
                cmd.Parameters.AddWithValue("@password", encryptedPassword);
                // Execute the query.                
                if (cmd.ExecuteNonQuery() <= 0)
                {
                    // If the number of rows affected by 
                    // the query is 0 or less then return false
                    return false;
                }

                // Close connection
                connection.Close();
            }
            catch (Exception)
            {
                // Something failed while trying to set the password
                return false;
            }
            // Returns true meaning the password was successfully set.
            return true;
        }

        // Returns a integer representing the UserID of the user 
        // or 0 indicating the user doesn't exist.
        public static int UserExist(string userEmail)
        {
            // Create database connection APPSETTING_MYSQL_CONNECTION_STRING is from Azure server config 
            // you can replace it with your connection string if deploying to a different server.
            MySqlConnection connection = new MySqlConnection(Environment.GetEnvironmentVariable("APPSETTING_MYSQL_CONNECTION_STRING"));            
            
            try
            {
                // Open connection to Database
                connection.Open();

                // Queries database for the UserID of a user who has the given e-mail address                
                MySqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "SELECT userID FROM Users WHERE email = @email LIMIT 1";
                cmd.Parameters.AddWithValue("@email", userEmail.ToLower());
                
                // Puts query results into Datatable
                MySqlDataAdapter a = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                a.Fill(dt);
                               
                // If count equals 0 then return false ("User doesn't exist")
                if (dt.Rows.Count <= 0)
                {
                    return 0;
                }
                else
                {
                    return Convert.ToInt32(dt.Rows[0]["userID"]);
                }
            }
            catch (Exception)
            {
                // On error return 0, that the user does exist 
                // (this will prevent duplicate account creation in the case of an error)
                return 0;
            }
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
