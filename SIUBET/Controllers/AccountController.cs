using BusinessEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace SIUBET.Controllers
{
    public class AccountController : Controller
    {
        // GET: Login
        [AllowAnonymous]
        public ActionResult Login(string ReturnUrl)
        {
            BEUsuario oUsuario = new BEUsuario();
            return PartialView(oUsuario);
        }

        [HttpPost]
        //[AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(BEUsuario oUsuario, string ReturnUrl = "") {
            //Redirect('Expedientes/Index');
            if (oUsuario.UserName == "siubet" && oUsuario.Password == "siubet@2017") {
                oUsuario.Nombres = "Usuario MVCS";
                FormsAuthentication.SetAuthCookie(oUsuario.UserName, oUsuario.Recordarme);
                if (Url.IsLocalUrl(ReturnUrl))
                {
                    return Redirect(ReturnUrl);
                }
                else {
                    return RedirectToAction("Index", "Expedientes");
                }
            }
            ModelState.Remove("Password");
            return RedirectToAction("Index", "Expedientes");
        }
        [Authorize]
        public ActionResult Logout() {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login","Account");
        }
    }
}