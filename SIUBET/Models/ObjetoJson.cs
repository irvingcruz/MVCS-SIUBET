using BusinessEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIUBET.Models
{
    public class ObjetoJson
    {
        public List<BEExpediente> items { get; set; }
        public List<BEMovimiento> items2 { get; set; }
        public List<BEPersona> items3 { get; set; }
        public List<BETrazaSITRAD> items4 { get; set; }
        public BEMovimiento Movimiento { get; set; }
        public int totalRows { get; set; }
        public int totalRowsFilter { get; set; }
        public bool success { get; set; }
        public string message { get; set; }
    }
}