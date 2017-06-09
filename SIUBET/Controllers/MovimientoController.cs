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
            string message = "";
            try
            {
                if (oDevolucion.NumeroCargo == null || oDevolucion.NumeroCargo.Trim().Length <= 0) message = "Ingrese el número de cargo";
                if (message.Trim().Length == 0 && (oDevolucion.EntidadDestino == null || oDevolucion.EntidadDestino.Trim().Length <= 0)) message = "Ingrese la Unidad Ejecutora";

                if (message.Trim().Length == 0)
                {
                    string _file = DateTime.Now.ToString("yyyyMMddHHmmss");
                    string adjuntoFileEmision = "";
                    string adjuntoFileCargo = "";
                    if (oDevolucion.FileEmision != null)
                    {
                        adjuntoFileEmision = _file + Path.GetExtension(oDevolucion.FileEmision.FileName);
                        oDevolucion.NombreFileEmision = adjuntoFileEmision;
                    }
                    if (oDevolucion.FileCargo != null)
                    {
                        adjuntoFileCargo = _file + "_" + oDevolucion.NumeroCargo + Path.GetExtension(oDevolucion.FileCargo.FileName);
                        oDevolucion.NombreFileCargo = adjuntoFileCargo;
                    }

                    rpta = new BLMovimientos().fnInsertarMovDevolucion(oDevolucion);

                    if (rpta)
                    {
                        if (adjuntoFileEmision.Length > 0) oDevolucion.FileEmision.SaveAs(Server.MapPath("~/Uploads/D/" + adjuntoFileEmision));
                        if (adjuntoFileCargo.Length > 0) oDevolucion.FileCargo.SaveAs(Server.MapPath("~/Uploads/D/" + adjuntoFileCargo));
                        message = "El registro se guardó correctamente";
                    }
                    else
                    {
                        message = "Error al insertar el registro.";
                    }

                }

            }
            catch (Exception)
            {
                message = "Error inesperado al procesar el movimiento";
            }
            result.items = null;
            result.success = rpta;
            result.message = message;
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
            string message = "";
            try
            {
                string _file = DateTime.Now.ToString("yyyyMMddHHmmss");
                string adjuntoFileFinal = "";
                if (oDevolucion.FileFinal != null)
                {
                    adjuntoFileFinal = _file + Path.GetExtension(oDevolucion.FileFinal.FileName);
                    oDevolucion.NombreFileFinal = adjuntoFileFinal;
                }

                rpta = new BLMovimientos().fnRetornaPre_RecepcionaDev(oDevolucion);

                if (rpta)
                {
                    if (adjuntoFileFinal.Length > 0) oDevolucion.FileFinal.SaveAs(Server.MapPath("~/Uploads/D/" + adjuntoFileFinal));
                    message = "La recepción se guardó correctamente";
                }
                else
                {
                    message = "Error al recepcionar la devolución.";
                }

            }
            catch (Exception)
            {
                message = "Error inesperado al procesar el movimiento";
            }
            result.items = null;
            result.success = rpta;
            result.message = message;
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
            string message = "";
            try
            {
                if (message.Trim().Length == 0 && (oPrestamo.EntidadDestino == null || oPrestamo.EntidadDestino.Trim().Length <= 0)) message = "Ingrese el Ingeniero/Otros";

                if (message.Trim().Length == 0)
                {
                    string _file = DateTime.Now.ToString("yyyyMMddHHmmss");
                    string adjuntoFileCargo = "";
                    if (oPrestamo.FileCargo != null)
                    {
                        adjuntoFileCargo = _file + Path.GetExtension(oPrestamo.FileCargo.FileName);
                        oPrestamo.NombreFileCargo = adjuntoFileCargo;
                    }

                    rpta = new BLMovimientos().fnInsertarMovPrestamo(oPrestamo);

                    if (rpta)
                    {
                        if (adjuntoFileCargo.Length > 0) oPrestamo.FileCargo.SaveAs(Server.MapPath("~/Uploads/P/" + adjuntoFileCargo));
                        message = "El registro se guardó correctamente";
                    }
                    else
                    {
                        message = "Error al insertar el registro.";
                    }

                }

            }
            catch (Exception)
            {
                message = "Error inesperado al procesar el movimiento";
            }
            result.items = null;
            result.success = rpta;
            result.message = message;
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
        public ActionResult PreRetornar(BEMovimiento oDevolucion)
        {
            if (!Request.IsAjaxRequest()) return null;

            ObjetoJson result = new ObjetoJson();
            bool rpta = false;
            string message = "";
            try
            {
                string _file = DateTime.Now.ToString("yyyyMMddHHmmss");
                string adjuntoFileFinal = "";
                if (oDevolucion.FileFinal != null)
                {
                    adjuntoFileFinal = _file + Path.GetExtension(oDevolucion.FileFinal.FileName);
                    oDevolucion.NombreFileFinal = adjuntoFileFinal;
                }

                rpta = new BLMovimientos().fnRetornaPre_RecepcionaDev(oDevolucion);

                if (rpta)
                {
                    if (adjuntoFileFinal.Length > 0) oDevolucion.FileFinal.SaveAs(Server.MapPath("~/Uploads/P/" + adjuntoFileFinal));
                    message = "El retorno del documento se guardó correctamente";
                }
                else
                {
                    message = "Error al retonar el préstamo.";
                }

            }
            catch (Exception)
            {
                message = "Error inesperado al procesar el movimiento";
            }
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
                message = "Error inesperado al procesar la anulación";
            }
            result.items = null;
            result.success = rpta;
            result.message = message;
            return new JsonResult { Data = result };
        }

    }
}
