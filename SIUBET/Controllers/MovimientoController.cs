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
        public ActionResult PreCrear()
        {            
            BEMovimiento oDevolucion = new BEMovimiento();
            oDevolucion.FechaMov = DateTime.Now.ToShortDateString();
            oDevolucion.Plazo = 15;
            oDevolucion.IDTipoMov = 4; //Préstamos        
            return PartialView(oDevolucion);
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
                    result.message = "Seleccione ó Ingrese el Ingeniero/Otros";
                    goto Terminar;
                }
                //if (oPrestamo.FileCargo == null)
                //{
                //    result.message = "Debe anexar un documento de cargo.";
                //    goto Terminar;
                //}
                if (oPrestamo.FileCargo != null) oPrestamo.ExtensionFile = Path.GetExtension(oPrestamo.FileCargo.FileName);
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

    }
}
