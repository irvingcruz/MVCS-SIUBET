using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity
{
    public class BETrazaSITRAD
    {
        public int Nro { get; set; }
        public string Documento { get; set; }
        public string EnviaArea { get; set; }
        public string EnviaFecha { get; set; }
        public string RecibeArea { get; set; }
        public string RecibeFecha { get; set; }
        public string Estado { get; set; }
        public int Dias { get; set; }
        public string Observaciones { get; set; }
    }
}
