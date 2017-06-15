using BusinessEntity;
using BusinessLogic;
using Microsoft.Ajax.Utilities;
using SIUBET.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Excel = Microsoft.Office.Interop.Excel;


namespace SIUBET.Controllers
{
    public class MovimientoController : Controller
    {
        private string vMsgSuccess = "El registró se guardó correctamente.";
        private string vMsgFail = "Error al tratar de guadar el registro.";
        private string vMsgThrow = "Error inesperado al procesar el registro.";
        private int iMaxSizeFile = 512000; //500KB - 0.5MB
        private string vPDF = "pdf";

        public ActionResult DDCrear(int id)
        {
            ViewData["Responsables"] = new SelectList(new BLMovimientos().fnListarResponsables(), "IDResponsable", "Descripcion");
            BEMovimiento oDD = new BEMovimiento();
            oDD.FechaMov = DateTime.Now.ToShortDateString();
            oDD.FechaEmision = oDD.FechaMov;
            oDD.IDTipoMov = id; //[2]Devolución [5]Derivación
            return PartialView(oDD);
        }
        [HttpPost]
        public ActionResult DDCrear(BEMovimiento oDD)
        {

            if (!Request.IsAjaxRequest()) return null;

            ObjetoJson result = new ObjetoJson();
            bool rpta = false;
            try
            {
                if (oDD.NumeroCargo == null || oDD.NumeroCargo.Trim().Length <= 0)
                {
                    result.message = "Ingrese el número de cargo";
                    goto Terminar;
                }
                if (oDD.UndEjec_CAC == null || oDD.UndEjec_CAC.Trim().Length <= 0)
                {
                    result.message = oDD.IDTipoMov == 2 ? "Seleccione ó Ingrese la Unidad Ejecutora" : "Seleccione un CAC";
                    goto Terminar;
                }
                if (oDD.FileEmision == null)
                {
                    result.message = "Debe anexar un documento para la Emisión";
                    goto Terminar;
                }

                if (oDD.FileCargo == null)
                {
                    result.message = "Debe anexar un documento para el cargo";
                    goto Terminar;
                }

                if (oDD.FileEmision.ContentLength > iMaxSizeFile || oDD.FileCargo.ContentLength > iMaxSizeFile)
                {
                    var _fileName = "";
                    if (oDD.FileEmision.ContentLength > iMaxSizeFile) _fileName = oDD.FileEmision.FileName;
                    if (oDD.FileCargo.ContentLength > iMaxSizeFile) _fileName = oDD.FileCargo.FileName;
                    result.message = "Tamaño máximo del archivo 500 KB. (Archivo: " + _fileName + ")";
                    goto Terminar;
                }

                var supportedTypes = new[] { vPDF };
                var fileExtEmision = System.IO.Path.GetExtension(oDD.FileEmision.FileName).Substring(1);
                var fileExtCargo = System.IO.Path.GetExtension(oDD.FileCargo.FileName).Substring(1);
                if (!supportedTypes.Contains(fileExtEmision) || !supportedTypes.Contains(fileExtCargo))
                {
                    result.message = "Solo esta permitido documentos PDF.";
                    goto Terminar;
                }

                oDD.ExtensionFile = Path.GetExtension(oDD.FileEmision.FileName);
                rpta = new BLMovimientos().fnInsertarMovDD(oDD);

                if (rpta)
                {
                    string adjuntoFileEmision = oDD.Archivo + "-E" + Path.GetExtension(oDD.FileEmision.FileName);
                    string adjuntoFileCargo = oDD.Archivo + "-S" + Path.GetExtension(oDD.FileCargo.FileName);
                    oDD.FileEmision.SaveAs(Server.MapPath("~/Uploads/" + adjuntoFileEmision));
                    oDD.FileCargo.SaveAs(Server.MapPath("~/Uploads/" + adjuntoFileCargo));
                    result.message = vMsgSuccess;
                }
                else
                {
                    result.message = vMsgFail;
                }

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
        public ActionResult DDIndex()
        {
            return View();
        }                
        public ActionResult DDRecepcionar(int id)
        {
            BEMovimiento oDD = new BEMovimiento();
            oDD.IDMovimiento = id;
            oDD.FechaFinal = DateTime.Now.ToShortDateString();

            return PartialView(oDD);
        }
        [HttpPost]
        public ActionResult DDRecepcionar(BEMovimiento oDD)
        {
            if (!Request.IsAjaxRequest()) return null;

            ObjetoJson result = new ObjetoJson();
            bool rpta = false;
            //string message = "";
            try
            {
                if (oDD.FileFinal == null) {
                    result.message = "Debe anexar un documento.";
                    goto Terminar;
                }

                oDD.ExtensionFile = Path.GetExtension(oDD.FileFinal.FileName);
                rpta = new BLMovimientos().fnRetornaPre_RecepcionaDD(oDD);

                if (rpta)
                {
                    var file = Path.Combine(HttpContext.Server.MapPath("~/Uploads/"), oDD.Archivo + "-S" + oDD.ExtensionFile);
                    if (System.IO.File.Exists(file)) System.IO.File.Delete(file);

                    string adjuntoFileFinal = oDD.Archivo + "-R" + Path.GetExtension(oDD.FileFinal.FileName);
                    oDD.FileFinal.SaveAs(Server.MapPath("~/Uploads/" + adjuntoFileFinal));
                    result.message = vMsgSuccess;
                }
                else
                {
                    result.message = vMsgFail;
                }

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
        [HttpPost]
        public JsonResult ListadoMovimientos(int Snip,int IDTipoMov, int pageNumber, int pageSize)
        {

            if (!Request.IsAjaxRequest()) return null;
            ObjetoJson result = new ObjetoJson();

            int totalRows = 0;
            int totalRowsFilter = 0;
            //int IDTipoMov = 4;
            List<BEMovimiento> datosResult = new BLMovimientos().fnListarMovimientos(Snip, IDTipoMov, pageNumber, pageSize, ref totalRows, ref totalRowsFilter);

            result.items2 = datosResult;
            result.totalRows = totalRows;
            result.totalRowsFilter = totalRowsFilter;
            result.success = true;
            result.message = totalRowsFilter + " registros encontrados";


            return new JsonResult { Data = result };
        }
        public ActionResult PreIndex()
        {
            return View();
        }
        public ActionResult PreCrear(string ets) //ExpdientesTécnicos (1|2|..|n)
        {            
            BEMovimiento oPrestamo = new BEMovimiento();
            oPrestamo.ET_selected_P = ets;
            oPrestamo.FechaMov = DateTime.Now.ToShortDateString();
            oPrestamo.FechaEmision = oPrestamo.FechaMov;
            oPrestamo.Plazo = 15;
            oPrestamo.IDTipoMov = 4; //Préstamos        
            return PartialView(oPrestamo);
        }
        public ActionResult PreEditar(int id) {
            BEMovimiento oPrestamo = new BEMovimiento();
            oPrestamo = new BLMovimientos().fnObtenerMovimiento(id);
            return PartialView("PreCrear", oPrestamo);
        }
        [HttpPost]
        public ActionResult PreCrear(BEMovimiento oPrestamo)
        {

            if (!Request.IsAjaxRequest()) return null;

            ObjetoJson result = new ObjetoJson();
            bool rpta = false;
            try
            {
                if (oPrestamo.Ing_Evaluador == null || oPrestamo.Ing_Evaluador.Trim().Length <= 0)
                {
                    result.message = "Ingrese el Ingeniero/Otros";
                    goto Terminar;
                }

                if(oPrestamo.IDMovimiento > 0)
                {
                    if (oPrestamo.FileCargo == null)
                    {
                        result.message = "Debe anexar un documento de cargo.";
                        goto Terminar;
                    }

                    if (oPrestamo.FileCargo.ContentLength > iMaxSizeFile)
                    {
                        result.message = "Tamaño máximo del archivo 500 KB. (Archivo: " + oPrestamo.FileCargo.FileName + ")";
                        goto Terminar;
                    }

                    var supportedTypes = new[] { vPDF };
                    var fileExtCargo = System.IO.Path.GetExtension(oPrestamo.FileCargo.FileName).Substring(1);
                    if (!supportedTypes.Contains(fileExtCargo))
                    {
                        result.message = "Solo esta permitido documentos PDF.";
                        goto Terminar;
                    }

                    oPrestamo.ExtensionFile = Path.GetExtension(oPrestamo.FileCargo.FileName);
                }
               
                rpta = new BLMovimientos().fnInsertarMovPrestamo(oPrestamo);

                if (rpta)
                {
                    if (oPrestamo.FileCargo != null)
                    {
                        string adjuntoFileCargo = oPrestamo.Archivo + "-S" + Path.GetExtension(oPrestamo.FileCargo.FileName);
                        oPrestamo.FileCargo.SaveAs(Server.MapPath("~/Uploads/" + adjuntoFileCargo));
                    }
                    result.message = vMsgSuccess;
                }
                else
                {
                    result.message = vMsgFail;
                }

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
        public ActionResult PreRetornar(int id)
        {

            BEMovimiento oDevolucion = new BEMovimiento();
            oDevolucion.IDMovimiento = id;
            oDevolucion.FechaFinal = DateTime.Now.ToShortDateString();

            return PartialView(oDevolucion);
        }
        [HttpPost]
        public ActionResult PreRetornar(BEMovimiento oPrestamo)
        {
            if (!Request.IsAjaxRequest()) return null;

            ObjetoJson result = new ObjetoJson();
            bool rpta = false;
            try
            {
                if (oPrestamo.FileFinal == null)
                {
                    result.message = "Debe anexar un documento de cargo";
                    goto Terminar;
                }

                if (oPrestamo.ET_selected_P == null || oPrestamo.ET_selected_P.Length == 0) {
                    result.message = "Debe seleccionar uno o más registros.";
                    goto Terminar;
                }

                if (oPrestamo.FileFinal.ContentLength > iMaxSizeFile)
                {                    
                    result.message = "Tamaño máximo del archivo 500 KB. (Archivo: " + oPrestamo.FileFinal.FileName + ")";
                    goto Terminar;
                }

                var supportedTypes = new[] { vPDF };
                var fileExtEmision = System.IO.Path.GetExtension(oPrestamo.FileFinal.FileName).Substring(1);                
                if (!supportedTypes.Contains(fileExtEmision))
                {
                    result.message = "Solo esta permitido documentos PDF.";
                    goto Terminar;
                }

                oPrestamo.ExtensionFile = Path.GetExtension(oPrestamo.FileFinal.FileName);
                rpta = new BLMovimientos().fnRetornaPre_RecepcionaDD(oPrestamo);

                if (rpta)
                {
                    var file = Path.Combine(HttpContext.Server.MapPath("~/Uploads/"), oPrestamo.Archivo + "-S" + oPrestamo.ExtensionFile);
                    if (System.IO.File.Exists(file)) System.IO.File.Delete(file);

                    string adjuntoFileFinal = oPrestamo.Archivo + "-R" + Path.GetExtension(oPrestamo.FileFinal.FileName); ;
                    oPrestamo.FileFinal.SaveAs(Server.MapPath("~/Uploads/" + adjuntoFileFinal));
                    result.message = vMsgSuccess;
                }
                else
                {
                    result.message = vMsgFail;
                }

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
        [HttpPost]
        public ActionResult AnularMovimiento(int IDMovimiento, string vMotivo, int IDTipoMov) {
            if (!Request.IsAjaxRequest()) return null;

            ObjetoJson result = new ObjetoJson();
            bool rpta = false;
            try
            {
                BEMovimiento oMov = new BEMovimiento(){ IDMovimiento = IDMovimiento, Motivo = vMotivo };
                rpta = new BLMovimientos().fnAnularMovimiento(oMov);
                switch (IDTipoMov)
                {
                    case 2:
                        result.message = rpta ? "La Devolución se anuló correctamente" : "No se puede anular la devolución.";
                        break;
                    case 4:
                        result.message = rpta ? "El Préstamo se anuló correctamente" : "No se puede anular el préstamo.";
                        break;
                }                                       
                
            }
            catch (Exception)
            {
                result.message = vMsgThrow;
            }
            result.items = null;
            result.success = rpta;
            //result.message = message;
            return new JsonResult { Data = result };
        }

        public ActionResult PrePrintCargo(int IDMovimiento) {
            ObjetoJson result = new ObjetoJson();
            bool rpta = false;
            BEMovimiento oRpt = new BEMovimiento();
            oRpt = new BLMovimientos().fnReporteCargoPrestamos(IDMovimiento);

            if (oRpt == null) {
                result.message = "No se encontraron datos para el cargo.";
                goto Terminar;
            }

            Excel.Application oXL;
            Excel.Workbook oWB;
            Excel.Worksheet oSheet;
            
            oXL = new Excel.Application();
            //oXL.Visible = true;

            oWB = oXL.Workbooks.Add();
            oSheet = (Excel.Worksheet)oWB.Worksheets.get_Item(1);
            oSheet.Name = "PrestamoCargo";

            oSheet.Range["E3", "K3"].MergeCells = true;
            oSheet.Range["E4", "K4"].MergeCells = true;
            oSheet.Cells[3, 5] = "AREA DE ADMINISTRACIÓN DOCUMENTARIA E INFORMÁTICA";
            oSheet.Cells[4, 5] = "FORMULARIO DE SERVICIO ARCHIVÍSTICO N°" + oRpt.Correlativo;
            
            oSheet.Cells[7, 9] = "FECHA";
            oSheet.Cells[7, 10] = oRpt.FechaMov;

            oSheet.Range["C8", "C8"].Font.Bold = true;
            oSheet.Cells[8, 3] = "DATOS DEL SOLICITANTE";
            oSheet.Cells[9, 3] = "NOMBRES Y APELLIDOS";
            oSheet.Cells[10, 3] = "UNIDAD ÓRGANICA";
            oSheet.Cells[11, 3] = "SEDE";
            oSheet.Cells[12, 3] = "CORREO";            

            oSheet.Cells[9, 6] = oRpt.Ing_Evaluador;
            oSheet.Cells[10, 6] = "";
            oSheet.Cells[11, 6] = "OLAECHEA";
            oSheet.Cells[12, 6] = oRpt.Correo;

            oSheet.Range["C9", "K12"].BorderAround(Excel.XlLineStyle.xlContinuous);

            oSheet.Cells[14, 3] = "TIPO DEL SERVICIO";
            oSheet.Cells[14, 6] = "";

            oSheet.Cells[16, 3] = "ITEM";
            oSheet.Cells[16, 4] = "SNIP";
            oSheet.Cells[16, 5] = "DESCRIPCIÓN DEL PIP";
            oSheet.Cells[16, 6] = "HT";
            oSheet.Cells[16, 7] = "FECHA HT";
            oSheet.Cells[16, 8] = "DOCUMENTO DE INGRESO";
            oSheet.Cells[16, 9] = "UNIDAD DE CONSERVACIÓN";
            oSheet.Cells[16, 10] = "FOLIOS";
            oSheet.Cells[16, 11] = "CD";

            oSheet.Range["C16", "K16"].Font.Bold = true;            
            int item = 16;            
            foreach (BEExpediente exp in oRpt.ListadoETs)
            {
                item++;
                oSheet.Cells[item, 3] = exp.Nro;
                oSheet.Cells[item, 4] = exp.Snip;
                oSheet.Cells[item, 5] = exp.NombreProyecto;
                oSheet.Cells[item, 6] = exp.NumeroHT;
                oSheet.Cells[item, 7] = exp.FechaOficio;
                oSheet.Cells[item, 8] = exp.NVersion;
                oSheet.Cells[item, 9] = exp.UnidadConservacion;
                oSheet.Cells[item, 10] = exp.Folios;
                oSheet.Cells[item, 11] = exp.CDs;
            }
            oSheet.Range["C16", "K" + item.ToString()].Borders.Weight = Excel.XlBorderWeight.xlThin;

            oSheet.Range["C16", "K" + item.ToString()].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            oSheet.Range["C16", "K" + item.ToString()].VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
            //oSheet.Range["E17", "E" + item.ToString()].WrapText = true;            
            oSheet.Range["H16", "K" + item.ToString()].WrapText = true;
            

            item++;
            oSheet.Cells[item, 3] = "OBSERVACIONES";
            oSheet.Range["E" + item.ToString(), "K" + (item+1).ToString()].MergeCells = true;
            oSheet.Range["E" + item.ToString(), "K" + (item + 1).ToString()].VerticalAlignment = Excel.XlVAlign.xlVAlignTop;
            oSheet.Cells[item, 5] = oRpt.Observaciones;
            
            item = item + 4;
            int itemFirmas = item;
            oSheet.Range["H" + item.ToString(), "J" + item.ToString()].MergeCells = true;
            oSheet.Cells[item, 5] = "__________________________";
            oSheet.Cells[item, 8] = "___________________________________________";

            item++;
            oSheet.Range["H" + item.ToString(), "J" + item.ToString()].MergeCells = true;
            oSheet.Cells[item, 5] = "RESPONSABLE DE PRÉSTAMO";
            oSheet.Cells[item, 8] = "CONFORMIDAD DE RECEPCIÓN DEL SOLICITANTE";

            item = item + 2;
            oSheet.Range["F" + item.ToString(), "G" + item.ToString()].MergeCells = true;
            oSheet.Cells[item, 6] = "_____________________________";

            item++;
            oSheet.Range["F" + item.ToString(), "G" + item.ToString()].MergeCells = true;
            oSheet.Cells[item, 6] = "CONFORMIDAD DE DEVOLUCIÓN";

            item++;           

            oSheet.Range["A1", "K" + item.ToString()].Font.Name = "Tahoma";
            oSheet.Range["A1", "K" + item.ToString()].Font.Size = 10;
            oSheet.Range["A3", "K4"].Font.Size = 14;
            oSheet.Range["A17", "K" + item.ToString()].Font.Size = 9;
            oSheet.Range["A3", "K4"].Font.Bold = true;
            oSheet.Range["E3","K4"].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            oSheet.Range["C" + itemFirmas.ToString(), "K" + item.ToString()].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

            oSheet.Range["B2", "L" + item.ToString()].BorderAround(Excel.XlLineStyle.xlContinuous);

            oSheet.Cells[1, 1].ColumnWidth = 1;
            oSheet.Cells[1, 2].ColumnWidth = 1;
            oSheet.Cells[1, 3].ColumnWidth = 8;
            oSheet.Cells[1, 4].ColumnWidth = 10;
            oSheet.Cells[1, 5].ColumnWidth = 50;
            oSheet.Cells[1, 6].ColumnWidth = 18;
            oSheet.Cells[1, 7].ColumnWidth = 12;
            oSheet.Cells[1, 8].ColumnWidth = 30;
            oSheet.Cells[1, 9].ColumnWidth = 20;
            oSheet.Cells[1, 10].ColumnWidth = 11;
            oSheet.Cells[1, 11].ColumnWidth = 11;
            oSheet.Cells[1, 12].ColumnWidth = 1;

            var _print = oSheet.PageSetup;
            _print.Zoom = false;
            _print.FitToPagesWide = 1;
            _print.Orientation = Excel.XlPageOrientation.xlLandscape;

            //oSheet.Range["B2:L" + item.ToString()].PrintOut();

            oXL.Visible = true;
            oXL.UserControl = true;

            rpta = true;
            result.message = "Documento de cargo generado correctamente.";

            Terminar:
            result.success = rpta;
            //result.message = "Exportando a Excel";
            return new JsonResult { Data = result };
        }

    }
}
