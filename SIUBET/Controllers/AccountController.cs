using BusinessEntity;
using BusinessLogic;
using SitradMovil.Models;
using SIUBET.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using static SitradMovil.Models.StringCrypto;

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
                if (ReturnUrl == null || ReturnUrl == "/") return RedirectToAction("Index", "Expedientes");
                else return RedirectToAction("AccessDenied", "Account");
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(BEUsuario oUsuario, string ReturnUrl = "")
        {
            if (oUsuario.UserName == null || oUsuario.UserName.Trim().Length == 0)
            {
                ViewBag.Mensaje = " Favor de ingresar el USUARIO.";
                goto Terminar;
            }

            if (oUsuario.Password == null || oUsuario.Password.Trim().Length == 0)
            {
                ViewBag.Mensaje = " Favor de ingresar el PASSWORD.";
                goto Terminar;
            }

            StringCrypto Clave = new StringCrypto(SymmProvEnum.DES);
            string PasswordEncriptado;
            PasswordEncriptado = Clave.Encrypting(oUsuario.Password, "keyLogin");
            oUsuario.Password = PasswordEncriptado;

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
            else { ViewBag.Mensaje = "(*) Las credenciales son incorrectas..!";  }

            Terminar:
            ModelState.Remove("Password");            
            return PartialView();
        }
        
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account");
        }

       
        public ActionResult AccessDenied()
        {
            ViewBag.Mensaje = "Lo sentimos, usted no tiene los permisos adecuados...!";
            return View();
        }
        [Authorize(Roles = "1")]
        public ActionResult Register()
        {
            BEUsuario oUsuario = new BEUsuario();
            oUsuario.IDPerfil = 2;
            ViewData["Roles"] = new SelectList(new BLUsuario().ListarPerfiles(), "IDCodigo", "Nombres");
            ViewData["Grupos"] = new SelectList(new BLUsuario().ListarGrupos(), "IDCodigo", "Nombres");
            return View(oUsuario);
        }
        [HttpPost]
        public ActionResult Register(BEUsuario oUsuario)
        {            
            if (!ModelState.IsValid){ goto Terminar; }

            int rpta = 0;
            ViewBag.Alerta = "danger";

            StringCrypto Clave = new StringCrypto(SymmProvEnum.DES);
            string PasswordEncriptado;
            PasswordEncriptado = Clave.Encrypting(oUsuario.Password, "keyLogin");
            oUsuario.Password = PasswordEncriptado;

            rpta = new BLUsuario().fnInsertarUpdateUsuario(oUsuario, User.Identity.Name);

            if (rpta == 1)
            {
                ViewBag.Mensaje = Global.vMsgSuccess;
                ViewBag.Alerta = "success";
            }
            else if (rpta == 2) ViewBag.Mensaje = "El usuario: [" + oUsuario.UserName + "] ya existe.";
            else ViewBag.Mensaje = Global.vMsgFail;

            Terminar:
            ViewData["Roles"] = new SelectList(new BLUsuario().ListarPerfiles(), "IDCodigo", "Nombres");
            ViewData["Grupos"] = new SelectList(new BLUsuario().ListarGrupos(), "IDCodigo", "Nombres");
            return View(oUsuario);
        }


    }
}