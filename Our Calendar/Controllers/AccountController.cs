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
                    // Redirects to the home page.
                    return RedirectToAction("Index", "Home");
                }
            }
            return View();
        }

        // User Login 
        // GET: /Account/Login

        public ActionResult Login()
        {
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
                    ModelState.AddModelError("", "The user name or password provided is incorrect.");
                }
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
            // Get user info from database
            ManageAcctVModel model = UserManagementModel.ManageAcct(Session["UserID"].ToString());
            return View(model);
        }

        // User Account Management (Handler)
        // POST: /Account/Manage
        [HttpPost]
        [Authorize]
        public ActionResult Manage(ManageAcctVModel model)
        {
            return View();
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
                        ViewBag.alertMessage = "Account successfully created, check your e-mail for account confirmation e-mail!";

                        // Redirect user to the home page.
                        return RedirectToAction("Index", "Home");                        
                    }
                    // Account creation failed, pass message to user.
                    ViewBag.AlertHeading = "Sorry.";
                    ViewBag.Alert = "There was an error creating your account.";
                }
                else
                {
                    // Message for user.
                    ViewBag.AlertHeading = "Whoa!";
                    ViewBag.Alert = "There is already an account associated with that e-mail address.";
                }
            }
            else
            {
                ViewBag.AlertHeading = "Oops!";
                ViewBag.Alert = "There were errors on the form you submitted.";
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
                        return RedirectToAction("Login", "Account");     
                    }
                    else
                    {
                        ViewBag.AlertHeading = "Sorry.";
                        ViewBag.Alert = "A problem occured while trying to set your password.";
                    }
                }
                else
                {
                    ViewBag.AlertHeading = "Oops!";
                    ViewBag.Alert = "The typed passwords do not match.";
                }
            }
            else
            {
                ViewBag.AlertHeading = "Oops!";
                ViewBag.Alert = "There were errors on the form you submitted.";
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }
    }
}
