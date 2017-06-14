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
    public class BLMovimientos
    {
        private SqlConnection oCon;
        public bool fnInsertarMovDD(BEMovimiento oDD)
        {
            try
            {
                oCon = BLConexion.SIUBET();
                DAMovimientos obj = new DAMovimientos(oCon);
                return obj.fnInsertarMovDD(oDD);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public List<BERespDev> fnListarResponsables()
        {
            try
            {
                oCon = BLConexion.SIUBET();
                DAMovimientos obj = new DAMovimientos(oCon);
                List<BERespDev> resultado = obj.fnListarResponsables();
                return resultado;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public List<BEMovimiento> fnListarMovimientos(int Snip, int IDTipoMov, int pageNumber, int pageSize, ref int totalRows, ref int totalRowsFilter)
        {
            try
            {
                oCon = BLConexion.SIUBET();
                DAMovimientos obj = new DAMovimientos(oCon);
                List<BEMovimiento> resultado = obj.fnListarMovimientos(Snip,IDTipoMov);
                totalRows = resultado.Count();
                totalRowsFilter = resultado.Count();
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
        public bool fnRetornaPre_RecepcionaDD(BEMovimiento oMov)
        {
            try
            {
                oCon = BLConexion.SIUBET();
                DAMovimientos obj = new DAMovimientos(oCon);
                return obj.fnRetornaPre_RecepcionaDD(oMov);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool fnInsertarMovPrestamo(BEMovimiento oPrestamo)
        {
            try
            {
                oCon = BLConexion.SIUBET();
                DAMovimientos obj = new DAMovimientos(oCon);
                return obj.fnInsertarMovPrestamo(oPrestamo);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool fnAnularMovimiento(BEMovimiento oMov) {
            try
            {
                oCon = BLConexion.SIUBET();
                DAMovimientos obj = new DAMovimientos(oCon);
                return obj.fnAnularMovimiento(oMov);
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
