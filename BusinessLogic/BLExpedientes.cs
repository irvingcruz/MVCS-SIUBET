using BusinessEntity;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic
{
   public class BLExpedientes
    {
        private SqlConnection oCon;

        public List<BEExpediente> fnListarExpedientes(int snip, string codigo, string proyecto, int pageNumber,int pageSize, ref int totalRows, ref int totalRowsFilter) {
            try {
                oCon = BLConexion.SIUBET();
                DAExpedientes obj = new DAExpedientes(oCon);
                BEExpediente _oExp = new BEExpediente();
                _oExp.Snip = snip;
                _oExp.NVersion = codigo;
                _oExp.NombreProyecto = proyecto;
                List<BEExpediente> resultado = obj.fnListarExpedientes(_oExp);
                totalRows = resultado.Count();
                totalRowsFilter = resultado.Count();
                resultado = resultado.OrderBy(e => e.Nro)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize).ToList();
                return resultado;
            }
            catch (Exception) {
                throw;
            }
        }

        public List<BEExpediente> fnListarExpedientesSelected(string IdsExpedientes, int pageNumber, int pageSize)
        {
            try
            {
                oCon = BLConexion.SIUBET();
                DAExpedientes obj = new DAExpedientes(oCon);
                List<BEExpediente> resultado = obj.fnListarExpedientesSelected(IdsExpedientes);
                resultado = resultado.OrderBy(e => e.Nro)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize).ToList();
                return resultado;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<BEMovimiento> fnListarExpedientesHistorial(int IDExpedienteVersion, int pageNumber, int pageSize)
        {
            try
            {
                oCon = BLConexion.SIUBET();
                DAExpedientes obj = new DAExpedientes(oCon);
                List<BEMovimiento> resultado = obj.fnListarExpedientesHistorial(IDExpedienteVersion);
                resultado = resultado.OrderBy(e => e.Nro)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize).ToList();
                return resultado;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
