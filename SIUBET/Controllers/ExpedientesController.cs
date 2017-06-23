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

        // GET: Expedientes       
        public ActionResult Index()
        {
            //if (oUsuario == null) return RedirectToAction("Logout","Account");
            //else
            return View();
        }

        [HttpPost]
        public JsonResult ListadoExpedientes(int snip, string numeroHT,string docIngreso, string estado, string etapa, int pageNumber, int pageSize)
        {

            if (!Request.IsAjaxRequest()) return null;
            ObjetoJson result = new ObjetoJson();

            int totalRows = 0;
            int totalRowsFilter = 0;
            List<BEExpediente> datosResult = new BLExpedientes().fnListarExpedientes(snip, numeroHT, docIngreso, estado, etapa, pageNumber, pageSize, ref totalRows, ref totalRowsFilter);
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

        [Authorize(Roles = "1")]
        public ActionResult Crear() {
            BEExpediente oEpx = new BEExpediente();
            ViewData["Sedes"] = new SelectList(new BLExpedientes().ListarSedes(), "IDSede", "Nombres");
            return View(oEpx);
        }
        [HttpPost]
        public ActionResult Crear(BEExpediente oExp) {
            if (!ModelState.IsValid) {
                goto Terminar;
            }

            ViewBag.Alerta = "danger";
            
            string[] ECB = oExp.UbicacionECB.Split(Convert.ToChar(":"));

            if (ECB.Length != 3) {
                ViewBag.Mensaje = "Debe ingresar una ubicación (E:C:B) válida.";
                goto Terminar;
            }

            if (oExp.UbicacionPP != null && oExp.UbicacionPP.Trim().Length > 0) {
                string[] PP = oExp.UbicacionPP.Split(Convert.ToChar(":"));
                if (PP.Length != 2)
                {
                    ViewBag.Mensaje = "Debe ingresar una ubicación (PQ:PO) válida.";
                    goto Terminar;
                }
            }

            bool rpta = new BLExpedientes().fnInsertarUpdateExpediente(oExp, User.Identity.Name);
            if (rpta)
            {
                ViewBag.Mensaje = Global.vMsgSuccess;
                ViewBag.Alerta = "success";
            }
            else ViewBag.Mensaje = Global.vMsgFail;

            Terminar:
            ViewData["Sedes"] = new SelectList(new BLExpedientes().ListarSedes(), "IDSede", "Nombres");
            return View();
        }

        [Authorize(Roles = "1")]
        public ActionResult Editar(int id)
        {
            BEExpediente oEpx = new BEExpediente();
            oEpx = new BLExpedientes().fnObtenerExpediente(id);
            ViewData["Sedes"] = new SelectList(new BLExpedientes().ListarSedes(), "IDSede", "Nombres");
            return View("Crear",oEpx);
        }

        public ActionResult Etapa() {
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

                if (rpta) result.message = Global.vMsgSuccess;
                else  result.message = Global.vMsgFail;
            }
            catch (Exception)
            {
                result.message = Global.vMsgThrow;
            }
            Terminar:
            result.items = null;
            result.success = rpta;
            //result.message = message;
            return new JsonResult { Data = result };
        }

    }
}
