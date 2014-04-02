using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Security;
using MySql.Data.MySqlClient;

namespace Our_Calendar.Models
{
    public class UserManagementModel
    {
        // Returns a boolean (true = success adding user, false = user not added)
        public static Boolean CreateUser(RegisterVModel registrationInfo)
        {
            /*string APIKey = APPSETTING_MANDRILL_API_KEY;

            MandrillApi.MandrillApi _mapi = new MandrillApi.MandrillApi(APIKey, "json");

            ViewBag.Mandrill = _mapi.Ping();

            object message = new
            {
                html = "test html",
                text = "text",
                subject = "test subject",
                from_email = "blake@blakebuckit.com",
                from_name = "Our Calendar",
                to = new List<Recipient>{new Recipient {email = "blake@bdev.dreamhosters.com", name = "Blake"}}
            };

            List<MandrillApi.Model.RecipientReturn> returnValue = _mapi.send(message);

            ViewBag.MandrillStatus = returnValue.Count.ToString(CultureInfo.InvariantCulture);*/

            // Encrypt the password, by hashing it. (Salt could be add later)
            /*byte[] password = System.Text.Encoding.Unicode.GetBytes(registrationInfo.Password);
            System.Security.Cryptography.HashAlgorithm hashAlgo = new System.Security.Cryptography.SHA256Managed();
            byte[] hashedPassword = hashAlgo.ComputeHash(password);

            string encryptedPassword = Convert.ToBase64String(hashedPassword);*/

            // Create database connection
            MySqlConnection connection = new MySqlConnection(Environment.GetEnvironmentVariable("APPSETTING_MYSQL_CONNECTION_STRING"));
            MySqlCommand cmd;
            connection.Open();

            try
            {
                // Try to add user to the Users table in the database
                cmd = connection.CreateCommand();
                cmd.CommandText = "INSERT INTO Users(fullName, email, password) VALUES (@fullName, @email, @password)";
                cmd.Parameters.AddWithValue("@fullName", registrationInfo.FullName);
                cmd.Parameters.AddWithValue("@email", registrationInfo.Email);
                //cmd.Parameters.AddWithValue("@password", encryptedPassword);
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception)
            {
                return false;
            }            
        }

        public static Boolean UserExist(string userEmail)
        {
            MySqlConnection connection = new MySqlConnection(Environment.GetEnvironmentVariable("APPSETTING_MYSQL_CONNECTION_STRING"));
            MySqlCommand cmd;
            connection.Open();
            try
            {
                cmd = connection.CreateCommand();
                cmd.CommandText = "SELECT Count(userID) FROM Users WHERE email = @email";
                cmd.Parameters.AddWithValue("@email", userEmail);
                MySqlDataAdapter a = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                a.Fill(dt);

                if (Convert.ToInt32(dt.Rows[0][0]) == 0)
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return true;
            }
            return true;
        }

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
                cmd.Parameters.AddWithValue("@email", logOnInfo.Email);
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
    }

    public class UserModel
    {
        [Required]
        public int UserID { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

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
