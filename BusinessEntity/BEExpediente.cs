using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity
{
    public class BEExpediente
    {
        public int Nro { get; set; }
        public int IDVersion { get; set; }
        public int IDExpTecnico { get; set; }
        [Required(ErrorMessage = "Favor de ingresar el código SNIP.")]
        public int Snip { get; set; }
        public string NombreProyecto { get; set; }
        [Required(ErrorMessage = "Favor de ingresar el numero de ingreso del ET.")]
        public string NVersion { get; set; }
        [Required(ErrorMessage = "Favor de ingresar la etapa del documento.")]
        public string Etapa { get; set; }
        public string DocumentoOficioSITRAD { get; set; }
        public string FechaOficio { get; set; }
        [Required(ErrorMessage = "Favor de ingresar el numero HT.")]
        public string NumeroHT { get; set; }
        public string FechaIngreso { get; set; }
        public string Documento { get; set; }
        public string UnidadConservacion { get; set; }
        public string Folios { get; set; }
        public string CDs { get; set; }
        public string Estado { get; set; }
        public bool Activo { get; set; }
        public string Seccion { get; set; }
        public string Serie { get; set; }
        public string SubSerie { get; set; }
        public string UbiTopografica { get; set; }
        public int IDTipoMov { get; set; }
        public int IDSede { get; set; }
        [Required(ErrorMessage = "Favor de ingresar la ubicación del ET")]
        public string UbicacionECB { get; set; }
        public string UbicacionPP { get; set; }

    }
}
