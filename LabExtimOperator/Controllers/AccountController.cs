using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using DevExpress.DataAccess.Native.DB;
//using LabExtimOperator.Filters;
using LabExtimOperator.Models;
using WebMatrix.WebData;
using DLLabExtim;

namespace LabExtimOperator.Controllers
{
    [Authorize]
    //[InitializeSimpleMembership]
    public class AccountController : Controller
    {

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        //[AllowAnonymous]
        //public ActionResult SessionKeeper()
        //{
        //    return View();
        //}

        //
        // POST: /Account/Login


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                //if (WebSecurity.Login(model.UserName, model.Password, persistCookie: model.RememberMe.Value))
                if (Membership.ValidateUser(model.UserName, model.Password))
                {
                    Session["LabextimUser"] = new LabextimUser(Membership.GetUser(model.UserName));

                    if (((LabextimUser)Session["LabextimUser"]).Member == null)
                    {
                        Session.Clear();
                        Session.Abandon();
                        FormsAuthentication.SignOut();
                        Session["LabextimUser"] = null;
                        return RedirectToAction("Login", "Account");
                    }

                    if (!string.IsNullOrEmpty(returnUrl))
                    {
                        //return Redirect(returnUrl ?? "/");
                        FormsAuthentication.SetAuthCookie(model.UserName, true);
                        return Redirect(returnUrl);

                    }
                    else
                    {
                        FormsAuthentication.SetAuthCookie(model.UserName, true);
                        return RedirectToAction("Index", "TempProductionOrderDetail");
                    }
                }
                ModelState.AddModelError("", "The user name or password provided is incorrect.");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/LogOff

        public ActionResult LogOff()
        {
            //WebSecurity.Logout();
            //Session.Abandon();
            //return RedirectToAction("Index", "TempProductionOrderDetail");


            Session.Clear();
            Session.Abandon();
            FormsAuthentication.SignOut();
            Session["LabextimUser"] = null;
            //FormsAuthentication.RedirectToLoginPage();
            return RedirectToAction("Login", "Account");
            //return RedirectToAction("Index", "TempProductionOrderDetail");

        }

        //
        // GET: /Account/Register

        //[AllowAnonymous]
        //public ActionResult Register()
        //{
        //    return View();
        //}

        //
        // POST: /Account/Register

        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public ActionResult Register(RegisterModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var ctxOperator = new LabExtimOperatorEntities();
        //        var ctxLabextim = new LabExtimEntities();
        //        // Attempt to register the user
        //        try
        //        {
        //            WebSecurity.CreateUserAndAccount(model.UserName, model.Password);
        //            //WebSecurity.Login(model.UserName, model.Password);
        //            if (model.LabExtimUser != "Administrator (Vede tutti)")
        //            {
        //                ctxOperator.UserProfile.Single(x => x.UserName == model.UserName).UserLabeId =
        //                    ctxLabextim.Employees.Single(x => x.UniqueName == model.LabExtimUser).ID;
        //                ctxOperator.SaveChanges();
        //            }
        //            return RedirectToAction("Index", "TempProductionOrderDetail");
        //        }
        //        catch (MembershipCreateUserException e)
        //        {
        //            ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
        //        }
        //    }

        //    // If we got this far, something failed, redisplay form
        //    return View(model);
        //}

        //
        // GET: /Account/ChangePassword

        //public ActionResult ChangePassword()
        //{
        //    return View();
        //}

        //
        // POST: /Account/ChangePassword

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult ChangePassword(ChangePasswordModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        bool changePasswordSucceeded;
        //        try
        //        {
        //            changePasswordSucceeded = WebSecurity.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword);
        //        }
        //        catch (Exception)
        //        {
        //            changePasswordSucceeded = false;
        //        }
        //        if (changePasswordSucceeded)
        //        {
        //            return RedirectToAction("ChangePasswordSuccess");
        //        }
        //        else
        //        {
        //            ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
        //        }

        //    }

        //    // If we got this far, something failed, redisplay form
        //    return View(model);
        //}

        //
        // GET: /Account/ChangePasswordSuccess

        //public ActionResult ChangePasswordSuccess()
        //{
        //    return View();
        //}

        //#region Status Codes
        //private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        //{
        //    // See http://go.microsoft.com/fwlink/?LinkID=177550 for
        //    // a full list of status codes.
        //    switch (createStatus)
        //    {
        //        case MembershipCreateStatus.DuplicateUserName:
        //            return "User name already exists. Please enter a different user name.";

        //        case MembershipCreateStatus.DuplicateEmail:
        //            return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

        //        case MembershipCreateStatus.InvalidPassword:
        //            return "The password provided is invalid. Please enter a valid password value.";

        //        case MembershipCreateStatus.InvalidEmail:
        //            return "The e-mail address provided is invalid. Please check the value and try again.";

        //        case MembershipCreateStatus.InvalidAnswer:
        //            return "The password retrieval answer provided is invalid. Please check the value and try again.";

        //        case MembershipCreateStatus.InvalidQuestion:
        //            return "The password retrieval question provided is invalid. Please check the value and try again.";

        //        case MembershipCreateStatus.InvalidUserName:
        //            return "The user name provided is invalid. Please check the value and try again.";

        //        case MembershipCreateStatus.ProviderError:
        //            return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

        //        case MembershipCreateStatus.UserRejected:
        //            return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

        //        default:
        //            return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
        //    }
        //}
        //#endregion
    }
}