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

namespace SIUBET.Controllers
{
    public class ExpedientesController : Controller
    {
        // GET: Expedientes
        public ActionResult Index()
        {
            //ExcelPackage pck = new ExcelPackage();
            //var ws = pck.Workbook.Worksheets.Add("Sample1");

            //ws.Cells["A1"].Value = "Sample 1";
            //ws.Cells["A1"].Style.Font.Bold = true;
            //var shape = ws.Drawings.AddShape("Shape1", eShapeStyle.Rect);
            //shape.SetPosition(50, 200);
            //shape.SetSize(200, 100);
            //shape.Text = "Sample 1 saves to the Response.OutputStream";

            //pck.SaveAs(Response.OutputStream);
            //Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            //Response.AddHeader("content-disposition", "attachment;  filename=Sample1.xlsx");

            //============

            //string filePath = Server.MapPath("~/Content/ExcelReport.xlsx");
            //FileInfo Files = new FileInfo(filePath);
            //ExcelPackage excel = new ExcelPackage();
            //var sheetCreate = excel.Workbook.Worksheets.Add("Cargo");
            //sheetCreate.Cells[1, 1].Value = "Hola Mundo";
            //excel.SaveAs(Response.OutputStream);

            return View();
        }


        [HttpPost]
        public JsonResult ListadoExpedientes(int snip, string numeroHT, string estado, int pageNumber, int pageSize) {
            
            if (!Request.IsAjaxRequest()) return null;
            ObjetoJson result = new ObjetoJson();
            
            int totalRows = 0;
            int totalRowsFilter = 0;
            List<BEExpediente> datosResult = new BLExpedientes().fnListarExpedientes(snip, numeroHT, estado, pageNumber, pageSize, ref totalRows, ref totalRowsFilter);
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
    }
}
