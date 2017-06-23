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
        public bool fnInsertarMovDD(BEMovimiento oDD, string vUsuario)
        {
            try
            {
                oCon = BLConexion.SIUBET();
                DAMovimientos obj = new DAMovimientos(oCon);
                return obj.fnInsertarMovDD(oDD,vUsuario);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public List<BEListado> fnListarCbo(string Tipo)
        {
            try
            {
                oCon = BLConexion.SIUBET();
                DAMovimientos obj = new DAMovimientos(oCon);
                List<BEListado> resultado = obj.fnListarCbo(Tipo);
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
                List<BEMovimiento> resultado = obj.fnListarMovimientos(Snip, IDTipoMov);
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
        public bool fnRetornaPre_RecepcionaDD(BEMovimiento oMov, string vUsuario)
        {
            try
            {
                oCon = BLConexion.SIUBET();
                DAMovimientos obj = new DAMovimientos(oCon);
                return obj.fnRetornaPre_RecepcionaDD(oMov,vUsuario);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public bool fnInsertarMovPrestamo(BEMovimiento oPrestamo, string vUsuario)
        {
            try
            {
                oCon = BLConexion.SIUBET();
                DAMovimientos obj = new DAMovimientos(oCon);
                return obj.fnInsertarMovPrestamo(oPrestamo,vUsuario);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public bool fnAnularMovimiento(BEMovimiento oMov, string vUsuario)
        {
            try
            {
                oCon = BLConexion.SIUBET();
                DAMovimientos obj = new DAMovimientos(oCon);
                return obj.fnAnularMovimiento(oMov,vUsuario);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public BEMovimiento fnObtenerMovimiento(int IDMovimiento)
        {
            try
            {
                oCon = BLConexion.SIUBET();
                DAMovimientos obj = new DAMovimientos(oCon);                
                BEMovimiento resultado = obj.fnObtenerMovimiento(IDMovimiento);
                return resultado;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public BEMovimiento fnReporteCargoPrestamos(int IDMovimiento) {            
            try
            {
                oCon = BLConexion.SIUBET();
                DAMovimientos obj = new DAMovimientos(oCon);
                BEMovimiento resultado = obj.fnObtenerMovimiento(IDMovimiento);
                if (resultado != null) {
                    resultado.ListadoETs = new DAMovimientos(oCon).fnListarDetalleMovimiento(IDMovimiento);
                }
                return resultado;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public bool fnInsertarMovTransferencia(BEMovimiento oTransf, string vUsuario)
        {
            try
            {
                oCon = BLConexion.SIUBET();
                DAMovimientos obj = new DAMovimientos(oCon);
                return obj.fnInsertarMovTransferencia(oTransf,vUsuario);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}