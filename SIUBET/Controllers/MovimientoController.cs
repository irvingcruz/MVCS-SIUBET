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
             
        public ActionResult DevCrear()
        {
            ViewData["Responsables"] = new SelectList(new BLMovimientos().fnListarResponsables(), "IDResponsable", "Descripcion");
            BEMovimiento oDevolucion = new BEMovimiento();
            oDevolucion.FechaMov = DateTime.Now.ToShortDateString();
            oDevolucion.FechaEmision = oDevolucion.FechaMov;
            oDevolucion.IDTipoMov = 2; //Devolución        
            return PartialView(oDevolucion);
        }
        [HttpPost]
        public ActionResult DevCrear(BEMovimiento oDevolucion)
        {

            if (!Request.IsAjaxRequest()) return null;

            ObjetoJson result = new ObjetoJson();
            bool rpta = false;
            try
            {
                if (oDevolucion.NumeroCargo == null || oDevolucion.NumeroCargo.Trim().Length <= 0)
                {
                    result.message = "Ingrese el número de cargo";
                    goto Terminar;
                }
                if (oDevolucion.EntidadDestino == null || oDevolucion.EntidadDestino.Trim().Length <= 0)
                {
                    result.message = "Seleccione ó Ingrese la Unidad Ejecutora";
                    goto Terminar;
                }
                if (oDevolucion.FileEmision == null)
                {
                    result.message = "Debe anexar un documento para la Emisión";
                    goto Terminar;
                }
                if (oDevolucion.FileCargo == null)
                {
                    result.message = "Debe anexar un documento para el cargo";
                    goto Terminar;
                }

                oDevolucion.ExtensionFile = Path.GetExtension(oDevolucion.FileEmision.FileName);
                rpta = new BLMovimientos().fnInsertarMovDevolucion(oDevolucion);

                if (rpta)
                {
                    string adjuntoFileEmision = oDevolucion.Archivo + "-E" + Path.GetExtension(oDevolucion.FileEmision.FileName);
                    string adjuntoFileCargo = oDevolucion.Archivo + "-S" + Path.GetExtension(oDevolucion.FileCargo.FileName);
                    oDevolucion.FileEmision.SaveAs(Server.MapPath("~/Uploads/D/" + adjuntoFileEmision));
                    oDevolucion.FileCargo.SaveAs(Server.MapPath("~/Uploads/D/" + adjuntoFileCargo));
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
        public ActionResult DevIndex()
        {
            return View();
        }                
        public ActionResult DevRecepcionar(int id)
        {
            BEMovimiento oDevolucion = new BEMovimiento();
            oDevolucion.IDMovimiento = id;
            oDevolucion.FechaFinal = DateTime.Now.ToShortDateString();

            return PartialView(oDevolucion);
        }
        [HttpPost]
        public ActionResult DevRecepcionar(BEMovimiento oDevolucion)
        {
            if (!Request.IsAjaxRequest()) return null;

            ObjetoJson result = new ObjetoJson();
            bool rpta = false;
            //string message = "";
            try
            {
                if (oDevolucion.FileFinal == null) {
                    result.message = "Debe anexar un documento.";
                    goto Terminar;
                }

                oDevolucion.ExtensionFile = Path.GetExtension(oDevolucion.FileFinal.FileName);
                rpta = new BLMovimientos().fnRetornaPre_RecepcionaDev(oDevolucion);

                if (rpta)
                {
                    var file = Path.Combine(HttpContext.Server.MapPath("~/Uploads/D/"), oDevolucion.Archivo + "-S" + oDevolucion.ExtensionFile);
                    if (System.IO.File.Exists(file)) System.IO.File.Delete(file);

                    string adjuntoFileFinal = oDevolucion.Archivo + "-R" + Path.GetExtension(oDevolucion.FileFinal.FileName);                                        
                    oDevolucion.FileFinal.SaveAs(Server.MapPath("~/Uploads/D/" + adjuntoFileFinal));
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
                if (oPrestamo.EntidadDestino == null || oPrestamo.EntidadDestino.Trim().Length <= 0)
                {
                    result.message = "Seleccione ó Ingrese el Ingeniero/Otros";
                    goto Terminar;
                }
                if (oPrestamo.FileCargo == null)
                {
                    result.message = "Debe anexar un documento de cargo.";
                    goto Terminar;
                }

                oPrestamo.ExtensionFile = Path.GetExtension(oPrestamo.FileCargo.FileName);
                rpta = new BLMovimientos().fnInsertarMovPrestamo(oPrestamo);

                if (rpta)
                {
                    string adjuntoFileCargo = oPrestamo.Archivo + "-S" + Path.GetExtension(oPrestamo.FileCargo.FileName);
                    oPrestamo.FileCargo.SaveAs(Server.MapPath("~/Uploads/P/" + adjuntoFileCargo));
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
            string message = "";
            try
            {
                if (oPrestamo.FileFinal == null)
                {
                    result.message = "Debe anexar un documento de cargo";
                    goto Terminar;
                }                                

                oPrestamo.ExtensionFile = Path.GetExtension(oPrestamo.FileFinal.FileName);
                rpta = new BLMovimientos().fnRetornaPre_RecepcionaDev(oPrestamo);

                if (rpta)
                {
                    var file = Path.Combine(HttpContext.Server.MapPath("~/Uploads/P/"), oPrestamo.Archivo + "-S" + oPrestamo.ExtensionFile);
                    if (System.IO.File.Exists(file)) System.IO.File.Delete(file);

                    string adjuntoFileFinal = oPrestamo.Archivo + "-R" + Path.GetExtension(oPrestamo.FileFinal.FileName); ;
                    oPrestamo.FileFinal.SaveAs(Server.MapPath("~/Uploads/P/" + adjuntoFileFinal));
                    message = vMsgSuccess;
                }
                else
                {
                    message = vMsgFail;
                }

            }
            catch (Exception)
            {
                message = vMsgThrow;
            }
            Terminar:
            result.items = null;
            result.success = rpta;
            result.message = message;
            return new JsonResult { Data = result };
        }
        [HttpPost]
        public ActionResult AnularMovimiento(int IDMovimiento, string vMotivo, int IDTipoMov) {
            if (!Request.IsAjaxRequest()) return null;

            ObjetoJson result = new ObjetoJson();
            bool rpta = false;
            string message = "";
            try
            {
                BEMovimiento oMov = new BEMovimiento(){ IDMovimiento = IDMovimiento, Motivo = vMotivo };
                rpta = new BLMovimientos().fnAnularMovimiento(oMov);
                switch (IDTipoMov)
                {
                    case 2:
                        message = rpta ? "La Devolución se anuló correctamente" : "No se puede anular la devolución."; ;
                        break;
                    case 4:
                        message = rpta ? "El Préstamo se anuló correctamente" : "No se puede anular el préstamo.";
                        break;
                }                                       
                
            }
            catch (Exception)
            {
                message = vMsgThrow;
            }
            result.items = null;
            result.success = rpta;
            result.message = message;
            return new JsonResult { Data = result };
        }

    }
}
