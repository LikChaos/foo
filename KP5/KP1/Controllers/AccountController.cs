using KP1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace KP1.Controllers
{
    public class AccountController : Controller
    {
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        public bool UserContainRole(String role)
        {
            bool answer = false;
            String username = User.Identity.Name;
            using (ShopDBEntities entities = new ShopDBEntities())
            {
                User user = entities.User.Single(u => u.Login == username);
                foreach (var item in entities.User_Role.Where(u => u.Id_User == user.Id))
                {
                    if (item.Role.Role1==role)
                    {
                        answer = true;
                        break;
                    }
                }
            }
            return answer;
        }

        [HttpPost]
        public ActionResult Login(User model, string returnUrl)
        {
            // Lets first check if the Model is valid or not
            if (ModelState.IsValid)
            {
                using (ShopDBEntities entities = new ShopDBEntities())
                {
                    string username = model.Login;
                    string password = model.Password;

                    bool userValid = entities.User.Any(user => user.Login == username && user.Password == password);

                    // User found in the database
                    if (userValid)
                    {
                        FormsAuthentication.SetAuthCookie(username, false);
                        //    return RedirectToAction("Users", "Home");
                        if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                            && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                        {
                            return Redirect(returnUrl);
                        }
                        else
                        {
                            return RedirectToAction("Index", "WorkerHome");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Пароль или имя пользователя не верны.");
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }
        [Authorize]
        //[Authorize (Roles = "admin")]
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");
        }
    }
}