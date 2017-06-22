using BusinessEntity;
using BusinessLogic;
using Microsoft.Ajax.Utilities;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using SIUBET.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace SIUBET.Controllers
{
    [Authorize]
    public class MovimientoController : Controller
    {

        public ActionResult DDCrear(int id)
        {
            //if (oUsuario == null) return RedirectToAction("Logout", "Account");
            //else
            //{
                ViewData["Responsables"] = new SelectList(new BLMovimientos().fnListarResponsables(), "IDResponsable", "Descripcion");
                BEMovimiento oDD = new BEMovimiento();
                oDD.FechaMov = DateTime.Now.ToShortDateString();
                oDD.FechaEmision = oDD.FechaMov;
                oDD.IDTipoMov = id; //[2]Devolución [5]Derivación
                return PartialView(oDD);
            //}
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

                if (oDD.FileEmision.ContentLength > Global.iMaxSizeFile || oDD.FileCargo.ContentLength > Global.iMaxSizeFile)
                {
                    var _fileName = "";
                    if (oDD.FileEmision.ContentLength > Global.iMaxSizeFile) _fileName = oDD.FileEmision.FileName;
                    if (oDD.FileCargo.ContentLength > Global.iMaxSizeFile) _fileName = oDD.FileCargo.FileName;
                    result.message = Global.vMsgFileSizeFail + _fileName + ")";
                    goto Terminar;
                }

                var supportedTypes = new[] { Global.vPDF };
                var fileExtEmision = System.IO.Path.GetExtension(oDD.FileEmision.FileName).Substring(1);
                var fileExtCargo = System.IO.Path.GetExtension(oDD.FileCargo.FileName).Substring(1);
                if (!supportedTypes.Contains(fileExtEmision) || !supportedTypes.Contains(fileExtCargo))
                {
                    result.message = Global.vMsgFileTypefail;
                    goto Terminar;
                }

                oDD.ExtensionFile = Path.GetExtension(oDD.FileEmision.FileName);
                
                rpta = new BLMovimientos().fnInsertarMovDD(oDD,  User.Identity.Name);

                if (rpta)
                {
                    string adjuntoFileEmision = oDD.Archivo + "-E" + Path.GetExtension(oDD.FileEmision.FileName);
                    string adjuntoFileCargo = oDD.Archivo + "-S" + Path.GetExtension(oDD.FileCargo.FileName);
                    oDD.FileEmision.SaveAs(Server.MapPath("~/Uploads/" + adjuntoFileEmision));
                    oDD.FileCargo.SaveAs(Server.MapPath("~/Uploads/" + adjuntoFileCargo));
                    result.message = Global.vMsgSuccess;
                }
                else
                {
                    result.message = Global.vMsgFail;
                }

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
        public ActionResult DDIndex()
        {
            //if (oUsuario == null) return RedirectToAction("Logout", "Account");
            //else
                return View();
        }                
        public ActionResult DDRecepcionar(int id)
        {
            //if (oUsuario == null) return RedirectToAction("Logout", "Account");
            //else
            //{
                BEMovimiento oDD = new BEMovimiento();
                oDD.IDMovimiento = id;
                oDD.FechaFinal = DateTime.Now.ToShortDateString();

                return PartialView(oDD);
            //}
        }
        [HttpPost]
        public ActionResult DDRecepcionar(BEMovimiento oDD)
        {
            if (!Request.IsAjaxRequest()) return null;

            ObjetoJson result = new ObjetoJson();
            bool rpta = false;
            try
            {
                if (oDD.FileFinal == null) {
                    result.message = "Debe anexar un documento.";
                    goto Terminar;
                }

                oDD.ExtensionFile = Path.GetExtension(oDD.FileFinal.FileName);
                rpta = new BLMovimientos().fnRetornaPre_RecepcionaDD(oDD, User.Identity.Name);

                if (rpta)
                {
                    var file = Path.Combine(HttpContext.Server.MapPath("~/Uploads/"), oDD.Archivo + "-S" + oDD.ExtensionFile);
                    if (System.IO.File.Exists(file)) System.IO.File.Delete(file);

                    string adjuntoFileFinal = oDD.Archivo + "-R" + Path.GetExtension(oDD.FileFinal.FileName);
                    oDD.FileFinal.SaveAs(Server.MapPath("~/Uploads/" + adjuntoFileFinal));
                    result.message = Global.vMsgSuccess;
                }
                else
                {
                    result.message = Global.vMsgFail;
                }

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
            //if (oUsuario == null) return RedirectToAction("Logout", "Account");
            //else
                return View();
        }
        public ActionResult PreCrear(string ets) //ExpdientesTécnicos (1|2|..|n)
        {
            //if (oUsuario == null) return RedirectToAction("Logout", "Account");
            //else
            //{
                BEMovimiento oPrestamo = new BEMovimiento();
                oPrestamo.ET_selected_P = ets;
                oPrestamo.FechaMov = DateTime.Now.ToShortDateString();
                oPrestamo.FechaEmision = oPrestamo.FechaMov;
                oPrestamo.Plazo = 15;
                oPrestamo.IDTipoMov = 4; //Préstamos        
                return PartialView(oPrestamo);
            //}
        }
        public ActionResult PreEditar(int id) {
            //if (oUsuario == null) return RedirectToAction("Logout", "Account");
            //else
            //{
                BEMovimiento oPrestamo = new BEMovimiento();
                oPrestamo = new BLMovimientos().fnObtenerMovimiento(id);
                return PartialView("PreCrear", oPrestamo);
            //}
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

                    if (oPrestamo.FileCargo.ContentLength > Global.iMaxSizeFile)
                    {
                        result.message = Global.vMsgFileSizeFail + oPrestamo.FileCargo.FileName + ")";
                        goto Terminar;
                    }

                    var supportedTypes = new[] { Global.vPDF };
                    var fileExtCargo = System.IO.Path.GetExtension(oPrestamo.FileCargo.FileName).Substring(1);
                    if (!supportedTypes.Contains(fileExtCargo))
                    {
                        result.message = Global.vMsgFileTypefail;
                        goto Terminar;
                    }

                    oPrestamo.ExtensionFile = Path.GetExtension(oPrestamo.FileCargo.FileName);
                }
               
                rpta = new BLMovimientos().fnInsertarMovPrestamo(oPrestamo, User.Identity.Name);

                if (rpta)
                {
                    if (oPrestamo.FileCargo != null)
                    {
                        string adjuntoFileCargo = oPrestamo.Archivo + "-S" + Path.GetExtension(oPrestamo.FileCargo.FileName);
                        oPrestamo.FileCargo.SaveAs(Server.MapPath("~/Uploads/" + adjuntoFileCargo));
                    }
                    result.message = Global.vMsgSuccess;
                }
                else
                {
                    result.message = Global.vMsgFail;
                }

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
        public ActionResult PreRetornar(int id)
        {
            //if (oUsuario == null) return RedirectToAction("Logout", "Account");
            //else
            //{
                BEMovimiento oDevolucion = new BEMovimiento();
                oDevolucion.IDMovimiento = id;
                oDevolucion.FechaFinal = DateTime.Now.ToShortDateString();
                return PartialView(oDevolucion);
            //}
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

                if (oPrestamo.FileFinal.ContentLength > Global.iMaxSizeFile)
                {                    
                    result.message = Global.vMsgFileSizeFail + oPrestamo.FileFinal.FileName + ")";
                    goto Terminar;
                }

                var supportedTypes = new[] { Global.vPDF };
                var fileExtEmision = System.IO.Path.GetExtension(oPrestamo.FileFinal.FileName).Substring(1);                
                if (!supportedTypes.Contains(fileExtEmision))
                {
                    result.message = Global.vMsgFileTypefail;
                    goto Terminar;
                }

                oPrestamo.ExtensionFile = Path.GetExtension(oPrestamo.FileFinal.FileName);
                rpta = new BLMovimientos().fnRetornaPre_RecepcionaDD(oPrestamo, User.Identity.Name);

                if (rpta)
                {
                    var file = Path.Combine(HttpContext.Server.MapPath("~/Uploads/"), oPrestamo.Archivo + "-S" + oPrestamo.ExtensionFile);
                    if (System.IO.File.Exists(file)) System.IO.File.Delete(file);

                    string adjuntoFileFinal = oPrestamo.Archivo + "-R" + Path.GetExtension(oPrestamo.FileFinal.FileName); ;
                    oPrestamo.FileFinal.SaveAs(Server.MapPath("~/Uploads/" + adjuntoFileFinal));
                    result.message = Global.vMsgSuccess;
                }
                else
                {
                    result.message = Global.vMsgFail;
                }

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
        [HttpPost]
        public ActionResult AnularMovimiento(int IDMovimiento, string vMotivo, int IDTipoMov)
        {
            if (!Request.IsAjaxRequest()) return null;

            ObjetoJson result = new ObjetoJson();
            bool rpta = false;
            try
            {
                BEMovimiento oMov = new BEMovimiento() { IDMovimiento = IDMovimiento, Motivo = vMotivo };
                rpta = new BLMovimientos().fnAnularMovimiento(oMov, User.Identity.Name);
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
                result.message = Global.vMsgThrow;
            }


            result.items = null;
            result.success = rpta;
            //result.message = message;
            return new JsonResult { Data = result };
        }
        public ActionResult TIndex() {
            //if (oUsuario == null) return RedirectToAction("Logout", "Account");
            //else
                return View();
        }
        public ActionResult TCrear(string ets) {
            //if (oUsuario == null) return RedirectToAction("Logout", "Account");
            //else
            //{
                BEMovimiento oTransf = new BEMovimiento();
                oTransf.ET_selected_T = ets;
                oTransf.FechaEmision = DateTime.Now.ToShortDateString();
                oTransf.IDTipoMov = 1; //Transferencias
                return PartialView(oTransf);
            //}
        }
        public ActionResult TEditar(int id)
        {
            //if (oUsuario == null) return RedirectToAction("Logout", "Account");
            //else
            //{
                BEMovimiento oTransf = new BEMovimiento();
                oTransf = new BLMovimientos().fnObtenerMovimiento(id);
                oTransf.Responsable = oTransf.UndEjec_CAC;
                oTransf.ET_selected_T = oTransf.ET_selected_P;
                return PartialView("TCrear", oTransf);
            //}
        }
        [HttpPost]
        public ActionResult TCrear(BEMovimiento oTransf)
        {
            if (!Request.IsAjaxRequest()) return null;

            ObjetoJson result = new ObjetoJson();
            bool rpta = false;
            try
            {
                if (oTransf.Responsable == null || oTransf.Responsable.Trim().Length <= 0)
                {
                    result.message = "Ingrese a quien se transfiere.";
                    goto Terminar;
                }

                if (oTransf.IDMovimiento > 0)
                {
                    if (oTransf.FileCargo == null)
                    {
                        result.message = "Debe anexar un documento de cargo.";
                        goto Terminar;
                    }

                    if (oTransf.FileCargo.ContentLength > Global.iMaxSizeFile)
                    {
                        result.message = Global.vMsgFileSizeFail + oTransf.FileCargo.FileName + ")";
                        goto Terminar;
                    }

                    var supportedTypes = new[] { Global.vPDF };
                    var fileExtCargo = System.IO.Path.GetExtension(oTransf.FileCargo.FileName).Substring(1);
                    if (!supportedTypes.Contains(fileExtCargo))
                    {
                        result.message = Global.vMsgFileTypefail;
                        goto Terminar;
                    }

                    oTransf.ExtensionFile = Path.GetExtension(oTransf.FileCargo.FileName);
                }

                rpta = new BLMovimientos().fnInsertarMovTransferencia(oTransf, User.Identity.Name);

                if (rpta)
                {
                    if (oTransf.FileCargo != null)
                    {
                        string adjuntoFileCargo = oTransf.Archivo + "-S" + Path.GetExtension(oTransf.FileCargo.FileName);
                        oTransf.FileCargo.SaveAs(Server.MapPath("~/Uploads/" + adjuntoFileCargo));
                    }
                    result.message = Global.vMsgSuccess;
                }
                else
                {
                    result.message = Global.vMsgFail;
                }

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
        public void PrePrintCargo(int id) {            
            BEMovimiento oRpt = new BEMovimiento();
            oRpt = new BLMovimientos().fnReporteCargoPrestamos(id);

            ExcelPackage pck = new ExcelPackage();
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Cargo");

            //Construyendo el reporte      
            ws.Cells["E3:K3"].Merge = true;
            ws.Cells["E4:K4"].Merge = true;
            ws.Cells["E3"].Value = "AREA DE ADMINISTRACIÓN DOCUMENTARIA E INFORMÁTICA";
            ws.Cells["E4"].Value = "FORMULARIO DE SERVICIO ARCHIVÍSTICO N°" + oRpt.Correlativo;

            ws.Cells["I7"].Value = "FECHA";
            ws.Cells["J7"].Value = oRpt.FechaMov;

            ws.Cells["C8:C8"].Style.Font.Bold = true;
            ws.Cells["C8"].Value = "DATOS DEL SOLICITANTE";
            ws.Cells["C9"].Value = "NOMBRES Y APELLIDOS";
            ws.Cells["C10"].Value = "UNIDAD ÓRGANICA";
            ws.Cells["C11"].Value = "SEDE";
            ws.Cells["C12"].Value = "CORREO";

            ws.Cells["F9"].Value = oRpt.Ing_Evaluador;
            ws.Cells["F10"].Value = "";
            ws.Cells["F11"].Value = "OLAECHEA";
            ws.Cells["F12"].Value = oRpt.Correo;

            ws.Cells["C9:K12"].Style.Border.BorderAround(ExcelBorderStyle.Thin);

            ws.Cells["C14"].Value = "TIPO DEL SERVICIO";
            ws.Cells["F14"].Value = "";

            ws.Cells["C16"].Value = "ITEM";
            ws.Cells["D16"].Value = "SNIP";
            ws.Cells["E16"].Value = "DESCRIPCIÓN DEL PIP";
            ws.Cells["F16"].Value = "HT";
            ws.Cells["G16"].Value = "FECHA HT";
            ws.Cells["H16"].Value = "DOCUMENTO DE INGRESO";
            ws.Cells["I16"].Value = "UNIDAD DE CONSERVACIÓN";
            ws.Cells["J16"].Value = "FOLIOS";
            ws.Cells["K16"].Value = "CD";

            ws.Cells["C16:K16"].Style.Font.Bold = true;
            int item = 16;

            var headerTable = ws.Cells["C16:K16"];
            headerTable.Style.Border.Top.Style = ExcelBorderStyle.Thin;
            headerTable.Style.Border.Left.Style = ExcelBorderStyle.Thin;
            headerTable.Style.Border.Right.Style = ExcelBorderStyle.Thin;
            headerTable.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

            foreach (BEExpediente exp in oRpt.ListadoETs)
            {
                item++;
                ws.Cells["C" + item.ToString()].Value = exp.Nro;
                ws.Cells["D" + item.ToString()].Value = exp.Snip;
                ws.Cells["E" + item.ToString()].Value = exp.NombreProyecto;
                ws.Cells["F" + item.ToString()].Value = exp.NumeroHT;
                ws.Cells["G" + item.ToString()].Value = exp.FechaOficio;
                ws.Cells["H" + item.ToString()].Value = exp.NVersion;
                ws.Cells["I" + item.ToString()].Value = exp.UnidadConservacion;
                ws.Cells["J" + item.ToString()].Value = exp.Folios;
                ws.Cells["K" + item.ToString()].Value = exp.CDs;

                var bodyTable = ws.Cells["C" + item.ToString() + ":K" + item.ToString()];
                bodyTable.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                bodyTable.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                bodyTable.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                bodyTable.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            }

            ws.Cells["C16:K" + item.ToString()].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ws.Cells["C16:K" + item.ToString()].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            ws.Cells["E17:E" + item.ToString()].Style.WrapText = true;
            ws.Cells["H16:K" + item.ToString()].Style.WrapText = true;

            item++;
            ws.Cells["C" + item.ToString()].Value = "OBSERVACIONES";
            ws.Cells["E" + item.ToString() + ":K" + (item + 1).ToString()].Merge = true;
            ws.Cells["E" + item.ToString() + ":K" + (item + 1).ToString()].Style.VerticalAlignment = ExcelVerticalAlignment.Top;
            ws.Cells["E" + item.ToString()].Value = oRpt.Observaciones;

            item = item + 4;
            int itemFirmas = item;
            ws.Cells["H" + item.ToString() + ":J" + item.ToString()].Merge = true;
            ws.Cells["E" + item.ToString()].Value = "__________________________";
            ws.Cells["H" + item.ToString()].Value = "___________________________________________";

            item++;
            ws.Cells["H" + item.ToString() + ":J" + item.ToString()].Merge = true;
            ws.Cells["E" + item.ToString()].Value = "RESPONSABLE DE PRÉSTAMO";
            ws.Cells["H" + item.ToString()].Value = "CONFORMIDAD DE RECEPCIÓN DEL SOLICITANTE";

            item = item + 2;
            ws.Cells["F" + item.ToString() + ":G" + item.ToString()].Merge = true;
            ws.Cells["F" + item.ToString()].Value = "_____________________________";

            item++;
            ws.Cells["F" + item.ToString() + ":G" + item.ToString()].Merge = true;
            ws.Cells["F" + item.ToString()].Value = "CONFORMIDAD DE DEVOLUCIÓN";

            item++;

            ws.Cells["A1:K" + item.ToString()].Style.Font.Name = "Tahoma";
            ws.Cells["A1:K" + item.ToString()].Style.Font.Size = 10;
            ws.Cells["A3:K4"].Style.Font.Size = 14;
            ws.Cells["A17:K" + item.ToString()].Style.Font.Size = 9;
            ws.Cells["A3:K4"].Style.Font.Bold = true;
            ws.Cells["E3:K4"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ws.Cells["C" + itemFirmas.ToString() + ":K" + item.ToString()].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            ws.Cells["B2:L" + item.ToString()].Style.Border.BorderAround(ExcelBorderStyle.Thin);

            ws.Column(1).Width = 1;
            ws.Column(2).Width = 1;
            ws.Column(3).Width = 8;
            ws.Column(4).Width = 10;
            ws.Column(5).Width = 50;
            ws.Column(6).Width = 18;
            ws.Column(7).Width = 12;
            ws.Column(8).Width = 30;
            ws.Column(9).Width = 20;
            ws.Column(10).Width = 11;
            ws.Column(11).Width = 11;
            ws.Column(12).Width = 1;

            ws.PrinterSettings.FitToPage = true;
            ws.PrinterSettings.Orientation = eOrientation.Portrait;

            //fin

            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment;  filename=Sample1.xlsx");
            Response.BinaryWrite(pck.GetAsByteArray());
            Response.End();
        }
    }
}
