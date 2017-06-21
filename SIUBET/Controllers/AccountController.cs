using BusinessEntity;
using BusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace SIUBET.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        // GET: Login
        [AllowAnonymous]
        public ActionResult Login(string ReturnUrl)
        {
            string Usuario = User.Identity.Name;
            if (Usuario == null || Usuario.Length == 0)
            {
                BEUsuario oUsuario = new BEUsuario();
                ViewBag.ReturnUrl = ReturnUrl;
                return PartialView(oUsuario);
            }
            else
            {
                return RedirectToAction("Index", "Expedientes");
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(BEUsuario oUsuario, string ReturnUrl = "") {
            if (new BLUsuario().fnAutenticacion(oUsuario))
            {
                FormsAuthentication.SetAuthCookie(oUsuario.UserName, oUsuario.Recordarme);
                System.Web.HttpContext.Current.Session["Usuario"] = oUsuario;
                if (Url.IsLocalUrl(ReturnUrl))
                {
                    return Redirect(ReturnUrl);
                }
                else
                {
                    return RedirectToAction("Index", "Expedientes");
                }
            }
            ModelState.Remove("Password");
            return PartialView();
        }
        //[Authorize]
        public ActionResult Logout() {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login","Account");
        }
    }
}