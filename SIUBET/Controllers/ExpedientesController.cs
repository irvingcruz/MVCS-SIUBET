using BusinessEntity;
using BusinessLogic;
using SIUBET.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIUBET.Controllers
{
    public class ExpedientesController : Controller
    {
        // GET: Expedientes
        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public JsonResult ListadoExpedientes(int snip, string codigo, string proyecto, int pageNumber, int pageSize) {
            
            if (!Request.IsAjaxRequest()) return null;
            ObjetoJson result = new ObjetoJson();
            
            int totalRows = 0;
            int totalRowsFilter = 0;
            List<BEExpediente> datosResult = new BLExpedientes().fnListarExpedientes(snip, codigo, proyecto, pageNumber, pageSize, ref totalRows, ref totalRowsFilter);
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
    }
}
