using System;
using System.Collections.Generic;
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

        public ActionResult Forgot(ForgotPasswordVModel model)
        {
            if (ModelState.IsValid)
            {
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
                    Session["UserID"] = UserManagementModel.UserExist(model.Email);
                    
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

            // Redirects the user to the homepage.
            return RedirectToAction("Index", "Home");
        }

        // User Account Management
        // GET: /Account/Manage

        public ActionResult Manage()
        {
            return View();
        }

        // User Account Management (Handler)
        // POST: /Account/Manage
        [HttpPost]
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
                if (UserManagementModel.UserExist(model.Email.ToLower()) > 0)
                {
                    // Try to create account
                    if (UserManagementModel.CreateUser(model))
                    {
                        // Message for user.
                        ViewBag.alertMessage = "Account successfully created, check your e-mail for account confirmation e-mail!";

                        // Redirect user to the home page.
                        return RedirectToAction("Index", "Home");                        
                    }
                    else
                    {
                        // Account creation failed, pass message to user.
                        ViewBag.alertMessage = "There was an error creating your account.";                        
                    }
                }
                else
                {
                    // Message for user.
                    ViewBag.alertMessage = "There is already an account associated with that e-mail address.";
                }
            }
            // If they get here, account creation failed.
            return View(model);
        }

        // Set/Change Password
        // GET: /Account/SetPassword

        [Authorize]
        public ActionResult SetPassword(string passcode)
        {
            // Sets ViewBag.passcode equal to the value of passcode 
            // if it isn't null otherwise equal to empty string.
            ViewBag.passcode = passcode ?? "";
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
                    // Try to set password
                    if (UserManagementModel.SetPassword(model.NewPassword, model.Passcode, Convert.ToInt32(Session["UserID"])))
                    {
                        // Success setting password.
                        // Redirect user to the manage calendar page.
                        return RedirectToAction("Manage", "Calendar");     
                    }
                    else
                    {
                        ModelState.AddModelError("", "A problem occured while trying to set your password.");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "The typed passwords do not match.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }
    }
}
