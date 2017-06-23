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

        public List<BEExpediente> fnListarExpedientes(int snip, string numeroHT, string docIngreso, string estado, string etapa, int pageNumber,int pageSize, ref int totalRows, ref int totalRowsFilter) {
            try {
                oCon = BLConexion.SIUBET();
                DAExpedientes obj = new DAExpedientes(oCon);
                BEExpediente _oExp = new BEExpediente();
                _oExp.Snip = snip;
                _oExp.NumeroHT = numeroHT;
                _oExp.NVersion = docIngreso;
                _oExp.Estado = estado;
                _oExp.Etapa = etapa;
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

        public List<BEPersona> fnListarPersona(int Tipo)
        {
            try
            {
                oCon = BLConexion.SIUBET();
                DAExpedientes obj = new DAExpedientes(oCon);
                List<BEPersona> resultado = obj.fnListarPersona(Tipo);
                return resultado;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<BEExpediente> fnListarExpedientesEnRetorno(int IDMovimiento, int pageNumber, int pageSize)
        {
            try
            {
                oCon = BLConexion.SIUBET();
                DAExpedientes obj = new DAExpedientes(oCon);
                List<BEExpediente> resultado = obj.fnListarExpedientesEnRetorno(IDMovimiento);
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
        public bool fnInsertarUpdateExpediente(BEExpediente oExp, string vUsuario) {
            try
            {
                oCon = BLConexion.SIUBET();
                DAExpedientes obj = new DAExpedientes(oCon);
                return obj.fnInsertarUpdateExpediente(oExp,vUsuario);                                
            }
            catch (Exception)
            {
                throw;
            }
        }
        public List<BEPersona> ListarSedes()
        {
            List<BEPersona> listado = new List<BEPersona>();
            listado.Add(new BEPersona { IDSede = 1, Nombres = "Olaechea" });
            listado.Add(new BEPersona { IDSede = 2, Nombres = "Callao" });
            return listado;
        }

        public bool fnActualizarEtapaET(string vEtapa, string IdsExpedientes, string vUsuario)
        {
            try
            {
                oCon = BLConexion.SIUBET();
                DAExpedientes obj = new DAExpedientes(oCon);
                return obj.fnActualizarEtapaET(vEtapa, IdsExpedientes, vUsuario);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public BEExpediente fnObtenerExpediente(int IDVersion)
        {
            try
            {
                oCon = BLConexion.SIUBET();
                DAExpedientes obj = new DAExpedientes(oCon);
                BEExpediente resultado = obj.fnObtenerExpediente(IDVersion);
                return resultado;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
