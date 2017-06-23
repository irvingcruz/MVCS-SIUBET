using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BusinessEntity
{
    public class BEMovimiento
    {
        public int Nro { get; set; }
        public int IDMovimiento { get; set; }
        public string FechaEmision { get; set; }
        public string FechaMov { get; set; }
        public int IDTipoMov { get; set; }
        public string TipoMov { get; set; }
        public int IDResponsable { get; set; }
        public string Responsable { get; set; }
        public string NumeroCargo { get; set; }        
        public string Ing_Evaluador { get; set; }
        public string UndEjec_CAC { get; set; }
        public string Observaciones { get; set; }
        public string FechaFinal { get; set; }
        public string ET_selected_D { get; set; }
        public string ET_selected_P { get; set; }
        public string ET_selected_T { get; set; }
        public string Numero { get; set; }
        public string Folios { get; set; }
        public string CDs { get; set; }
        public string Planos { get; set; }
        public HttpPostedFileBase FileEmision { get; set; }
        public HttpPostedFileBase FileCargo { get; set; }
        public HttpPostedFileBase FileFinal { get; set; }
        public string NombreFileCargo { get; set; }
        public string NombreFileEmision { get; set; }
        public string NombreFileFinal { get; set; }
        public string Estado { get; set; }
        public bool Activo { get; set; }        
        public int Plazo { get; set; }
        public string FechaRetornoEstimada { get; set; }
        public string Motivo { get; set; }
        public string Archivo { get; set; }
        public string ExtensionFile { get; set; }
        public string Correlativo { get; set; }
        public List<BEExpediente> ListadoETs { get; set; }
        public string Correo { get; set; }
        public string[] Modalidad { get; set; }
        public int PadreHome { get; set; }
        public int Print { get; set; }
        public string TipoDevolucion { get; set; }
    }
}
