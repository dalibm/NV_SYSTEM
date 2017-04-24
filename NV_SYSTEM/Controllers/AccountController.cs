using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using WebMatrix.WebData;
using NV_SYSTEM.Models;

namespace NV_SYSTEM.Controllers {

    public class AccountController : Controller {

        public ActionResult Index()
        {
            if(WebSecurity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            } else
            {
                return RedirectToAction("Login", "Account");
            }
        }
        
        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login() {
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl) {
            if(ModelState.IsValid) {
                if(WebSecurity.Login(model.Login, model.Password)) {
                    return Redirect(returnUrl ?? "/");
                }
                ModelState.AddModelError("", "Le nom login ou mot de passe est incorrect");
            }
            return View(model);
        }

        //
        // GET: /Account/LogOff
        public ActionResult LogOff() {
            WebSecurity.Logout();
            return RedirectToAction("Index", "Home");
        }



    }
}