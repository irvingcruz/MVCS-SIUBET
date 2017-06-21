using BusinessEntity;
using BusinessLogic;
using SIUBET.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.IO;
using System.Web.Security;

namespace SIUBET.Controllers
{
    [Authorize]    
    public class ExpedientesController : Controller
    {
        private string vMsgSuccess = "El registró se guardó correctamente.";
        private string vMsgFail = "Error al tratar de guadar el registro.";
        private string vMsgThrow = "Error inesperado al procesar el registro.";
        private string vUsuario = "";

        // GET: Expedientes       
        public ActionResult Index()
        {
            vUsuario = User.Identity.Name;
            if (vUsuario == null || vUsuario.Length == 0) return RedirectToAction("Login", "Account");
            //if (oUsuario == null) return RedirectToAction("Logout","Account");
            //else
            return View();
        }

        [HttpPost]
        public JsonResult ListadoExpedientes(int snip, string numeroHT, string estado, string etapa, int pageNumber, int pageSize)
        {

            if (!Request.IsAjaxRequest()) return null;
            ObjetoJson result = new ObjetoJson();

            int totalRows = 0;
            int totalRowsFilter = 0;
            List<BEExpediente> datosResult = new BLExpedientes().fnListarExpedientes(snip, numeroHT, estado, etapa, pageNumber, pageSize, ref totalRows, ref totalRowsFilter);
            //Dim datosResult = New ReglaValidacionApp().ReglasVerificacionListarFiltradoPaginado(filtros, oUsuario.dni, pageIndex, take, TotalRows, TotalRowFilter)


            result.items = datosResult;
            result.totalRows = totalRows;
            result.totalRowsFilter = totalRowsFilter;
            result.success = true;
            result.message = totalRowsFilter + " registros encontrados";


            return new JsonResult { Data = result };
        }

        [HttpPost]
        public JsonResult ListadoExpedientesSelected(string IdsExpedientes, int pageNumber, int pageSize)
        {

            if (!Request.IsAjaxRequest()) return null;
            ObjetoJson result = new ObjetoJson();

            List<BEExpediente> datosResult = new BLExpedientes().fnListarExpedientesSelected(IdsExpedientes, pageNumber, pageSize);

            result.items = datosResult;
            result.totalRows = datosResult.Count();
            result.totalRowsFilter = result.totalRows;
            result.success = true;
            result.message = result.totalRowsFilter + " registros seleccionados";


            return new JsonResult { Data = result };
        }

        [HttpPost]
        public JsonResult ListadoExpedientesHistorial(int IDExpedienteVersion, int pageNumber, int pageSize)
        {

            if (!Request.IsAjaxRequest()) return null;
            ObjetoJson result = new ObjetoJson();

            List<BEMovimiento> datosResult = new BLExpedientes().fnListarExpedientesHistorial(IDExpedienteVersion, pageNumber, pageSize);

            result.items2 = datosResult;
            result.totalRows = datosResult.Count();
            result.totalRowsFilter = result.totalRows;
            result.success = true;
            result.message = result.totalRowsFilter + " registros seleccionados";


            return new JsonResult { Data = result };
        }

        [HttpPost]
        public JsonResult ListadoPersonas(int Tipo)
        {

            if (!Request.IsAjaxRequest()) return null;
            ObjetoJson result = new ObjetoJson();

            int totalRows = 0;
            int totalRowsFilter = 0;
            List<BEPersona> datosResult = new BLExpedientes().fnListarPersona(Tipo);

            result.items3 = datosResult;
            result.totalRows = totalRows;
            result.totalRowsFilter = totalRowsFilter;
            result.success = true;
            result.message = totalRowsFilter + " registros encontrados";


            return new JsonResult { Data = result };
        }

        [HttpPost]
        public JsonResult ListadoExpedientesEnRetorno(int IDMovimiento, int pageNumber, int pageSize)
        {

            if (!Request.IsAjaxRequest()) return null;
            ObjetoJson result = new ObjetoJson();

            List<BEExpediente> datosResult = new BLExpedientes().fnListarExpedientesEnRetorno(IDMovimiento, pageNumber, pageSize);

            result.items = datosResult;
            result.totalRows = datosResult.Count();
            result.totalRowsFilter = result.totalRows;
            result.success = true;
            result.message = result.totalRowsFilter + " registros para retorno";


            return new JsonResult { Data = result };
        }

        public ActionResult Crear() {
            vUsuario = User.Identity.Name;
            if (vUsuario == null || vUsuario.Length == 0) return RedirectToAction("Login", "Account");

            BEExpediente oEpx = new BEExpediente();
            ViewData["Sedes"] = new SelectList(new BLExpedientes().ListarSedes(), "IDSede", "Nombres");
            return View(oEpx);
        }
        [HttpPost]
        public ActionResult Crear(BEExpediente oExp) {
            bool rpta = new BLExpedientes().fnInsertarUpdateExpediente(oExp, vUsuario);
            ViewData["Sedes"] = new SelectList(new BLExpedientes().ListarSedes(), "IDSede", "Nombres");
            return View();
        }

        public ActionResult Etapa() {
            vUsuario = User.Identity.Name;
            if (vUsuario == null || vUsuario.Length == 0) return RedirectToAction("Login", "Account");
            return PartialView();
        }
        [HttpPost]
        public ActionResult Etapa(BEExpediente oExp) {
            if (!Request.IsAjaxRequest()) return null;

            ObjetoJson result = new ObjetoJson();
            bool rpta = false;           
            try
            {
                if (oExp.Documento == null || oExp.Documento.Length == 0)
                {
                    result.message = "No existen documentos para actualizar.";
                    goto Terminar;
                }

                rpta = new BLExpedientes().fnActualizarEtapaET(oExp.Etapa, oExp.Documento, User.Identity.Name);

                if (rpta) result.message = vMsgSuccess;
                else  result.message = vMsgFail;
            }
            catch (Exception)
            {
                result.message = vMsgThrow;
            }
            Terminar:
            result.items = null;
            result.success = rpta;
            //result.message = message;
            return new JsonResult { Data = result };
        }

    }
}
