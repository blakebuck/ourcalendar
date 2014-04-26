using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using Our_Calendar.Models;

namespace Our_Calendar.Controllers
{
    public class AccountController : Controller
    {
        // Forgot Password 
        // GET: /Account/Forgot

        public ActionResult Forgot()
        {
            return View();
        }

        // Forgot Password (Handler)
        // POST: /Account/Forgot
        [HttpPost]
        public ActionResult Forgot(ForgotPasswordVModel model)
        {
            // Check for errors on the form
            if (ModelState.IsValid)
            {
                // Send password reset e-mail
                if (UserManagementModel.SendPasswordReset(model))
                {
                    SessionAlert.SessionAlert.CreateAlert("Please check your e-mail for the password reset e-mail.", "E-mail Sent.", "success");
                    // Redirects to the home page.
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    SessionAlert.SessionAlert.CreateAlert("There was a problem sending your password reset e-mail.", "Oh No!");
                }
            }
            else
            {
                SessionAlert.SessionAlert.CreateAlert("There were errors on the form you submitted.", "Oops!");
            }
            return View();
        }

        // User Login 
        // GET: /Account/Login

        public ActionResult Login()
        {
            if (Session["UserID"] != null)
            {
                // Redirects to Manage in the Calendar controller
                return RedirectToAction("Manage", "Calendar");
            }
            return View();
        }

        // User Login (Handler)
        // POST: /Account/Login

        [HttpPost]
        // model holds all the user login information, 
        // returnUrl is the URL they were trying to access but were redirect to login
        public ActionResult Login(LoginVModel model, string returnUrl)
        {
            // Checks to make sure there were no
            // errors on on the login page (i.e. all fields were filled out)
            if (ModelState.IsValid)
            {
                // Checks to see if the password given is valid
                if (UserManagementModel.CheckPassword(model))
                {
                    // Sets a logged in cookie
                    FormsAuthentication.SetAuthCookie(model.Email, true);

                    // Saves Email Address in Session Variable
                    Session["UserID"] = UserManagementModel.UserExist(model.Email).ToString(CultureInfo.InvariantCulture);
                    
                    // Checks to see if the returnUrl is valid 
                    if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/") && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                    {
                        // Redirects to returnUrl
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        // Redirects to Manage in the Calendar controller
                        return RedirectToAction("Manage", "Calendar");
                    }
                }
                else
                {
                    // Send the user an error message
                    SessionAlert.SessionAlert.CreateAlert("The e-mail address or password provided is incorrect.", "Whoops!");
                }
            }
            else
            {
                SessionAlert.SessionAlert.CreateAlert("There were errors on the form you submitted.", "Oops!");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        // User Logout
        // GET: /Account/Logout

        public ActionResult Logout()
        {
            // Destroys Logged In Cookie/Session
            FormsAuthentication.SignOut();
            // Clear session variables
            Session.Clear();

            // Redirects the user to the homepage.
            return RedirectToAction("Index", "Home");
        }

        // User Account Management
        // GET: /Account/Manage
        [Authorize]
        public ActionResult Manage()
        {
            if (Session["UserID"] != null)
            {
                // Get user info from database
                ManageAcctVModel model = UserManagementModel.ManageAcct(Session["UserID"].ToString());
                return View(model);
            }
            return RedirectToAction("Login", "Account");
        }

        // User Account Management (Handler)
        // POST: /Account/Manage
        [HttpPost]
        [Authorize]
        public ActionResult Manage(ManageAcctVModel model)
        {
            if (Session["UserID"] != null)
            {
                if (ModelState.IsValid)
                {
                    // Checks to see if the password given is valid
                    if (UserManagementModel.ManageAcct(model, Session["UserID"].ToString()))
                    {
                        SessionAlert.SessionAlert.CreateAlert("Your info has been updated.", "Success!", "success");
                        
                        // Get user updated info from database
                        ManageAcctVModel updatedModel = UserManagementModel.ManageAcct(Session["UserID"].ToString());
                        return View(updatedModel);
                    }
                    else
                    {
                        SessionAlert.SessionAlert.CreateAlert("Something happened while trying to update your info.",
                            "IDK!");
                    }
                }
                else
                {
                    SessionAlert.SessionAlert.CreateAlert("There were errors on the form you submitted.", "Oops!");
                }
                return View();
            }
            return RedirectToAction("Login", "Account");
        }


        // User Account Registration
        // GET: /Account/Register

        public ActionResult Register()
        {
            return View();
        }

        // User Account Registration (Handler)
        // POST: /Account/Register

        [HttpPost]
        public ActionResult Register(RegisterVModel model)
        {
            // Checks to make sure there were no
            // errors on on the login page (i.e. all fields were filled out)
            if (ModelState.IsValid)
            {
                // Check to see if an account already exists for the given e-mail address
                if (UserManagementModel.UserExist(model.Email.ToLower()) <= 0)
                {
                    // Try to create account
                    if (UserManagementModel.CreateUser(model))
                    {
                        // Message for user.
                        SessionAlert.SessionAlert.CreateAlert("Your account was successfully created, check your e-mail for account confirmation e-mail!", "Success!!", "success");

                        // Redirect user to the home page.
                        return RedirectToAction("Index", "Home");                        
                    }
                    // Account creation failed, pass message to user.
                    SessionAlert.SessionAlert.CreateAlert("There was an error creating your account.", "Sorry...");
                }
                else
                {
                    // Message for user.
                    SessionAlert.SessionAlert.CreateAlert("There is already an account associated with that e-mail address.", "Whoa!");
                }
            }
            else
            {
                SessionAlert.SessionAlert.CreateAlert("There were errors on the form you submitted.", "Oops!");
            }
            // If they get here, account creation failed.
            return View(model);
        }

        // Set/Change Password
        // GET: /Account/SetPassword
        public ActionResult SetPassword(string passcode)
        {
            return View();
        }

        // Set/Change Password (Handler)
        // POST: /Account/SetPassword
        [HttpPost]
        public ActionResult SetPassword(SetPasswordVModel model)
        {
            // Make sure that there were not errors on the form
            if (ModelState.IsValid)
            {
                // Check to see if they typed in their password correctly
                if (model.NewPassword == model.ConfirmPassword)
                {
                    string userID = Session["UserID"] != null ? Session["UserID"].ToString() : null;

                    if (UserManagementModel.SetPassword(model.NewPassword, model.Passcode, userID))
                    {
                        // Success setting password.
                        // Redirect user to the manage calendar page.
                        SessionAlert.SessionAlert.CreateAlert("Your password has been set.", "What a Password!", "success");
                        return RedirectToAction("Login", "Account");     
                    }
                    else
                    {
                        SessionAlert.SessionAlert.CreateAlert("A problem occured while trying to set your password.", "Sorry.");
                    }
                }
                else
                {
                    SessionAlert.SessionAlert.CreateAlert("The typed passwords do not match.", "Oops!");
                }
            }
            else
            {
                SessionAlert.SessionAlert.CreateAlert("There were errors on the form you submitted.", "Oops!");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }
    }
}
