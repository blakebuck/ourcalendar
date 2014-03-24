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
        public static Boolean CreateUser(RegisterModel registrationInfo)
        {
            // Encrypt the password, by hashing it. (Salt could be add later)
            byte[] password = System.Text.Encoding.Unicode.GetBytes(registrationInfo.Password);
            System.Security.Cryptography.HashAlgorithm hashAlgo = new System.Security.Cryptography.SHA256Managed();
            byte[] hashedPassword = hashAlgo.ComputeHash(password);
            
            string encryptedPassword = System.Text.Encoding.ASCII.GetString(hashedPassword);

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
                cmd.Parameters.AddWithValue("@password", encryptedPassword);
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception)
            {
                return false;
            }            
        }

        public static Boolean CheckPassword(string username, string password)
        {

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

    public class ChangePasswordModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

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

    public class LogOnModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterModel
    {
        [Required]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email address")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [System.Web.Mvc.Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
