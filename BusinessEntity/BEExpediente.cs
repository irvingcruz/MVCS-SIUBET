using System;
using System.Collections.Generic;
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
        public int Snip { get; set; }
        public string NombreProyecto { get; set; }
        public string NVersion { get; set; }
        public string DocumentoOficioSITRAD { get; set; }
        public string FechaOficio { get; set; }
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

    }
}
