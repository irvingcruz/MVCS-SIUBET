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
    public class BLUsuario
    {
        private SqlConnection oCon;
        public bool fnAutenticacion(BEUsuario oUsuario)
        {
            try
            {
                oCon = BLConexion.SIUBET();
                DAUsuario obj = new DAUsuario(oCon);
                return obj.fnAutenticacion(oUsuario);
            }
            catch (Exception e) { throw e; }
        }

        public BEUsuario fnObtenerUsuario(string vUsuario) {
            try
            {
                oCon = BLConexion.SIUBET();
                DAUsuario obj = new DAUsuario(oCon);
                return obj.fnObtenerUsuario(vUsuario);
            }
            catch (Exception e) { throw e; }
        }

        public int fnInsertarUpdateUsuario(BEUsuario oUsuario, string vUsuario)
        {
            try
            {
                oCon = BLConexion.SIUBET();
                DAUsuario obj = new DAUsuario(oCon);
                return obj.fnInsertarUpdateUsuario(oUsuario, vUsuario);
            }
            catch (Exception e) { throw e; }
        }
        public List<BEPersona> ListarPerfiles()
        {
            List<BEPersona> listado = new List<BEPersona>();
            listado.Add(new BEPersona { IDCodigo = 1, Nombres = "Supervisor" });
            listado.Add(new BEPersona { IDCodigo = 2, Nombres = "Normal" });
            return listado;
        }
        public List<BEPersona> ListarGrupos()
        {
            List<BEPersona> listado = new List<BEPersona>();
            listado.Add(new BEPersona { IDCodigo = 1, Nombres = "PNSU" });
            listado.Add(new BEPersona { IDCodigo = 2, Nombres = "PNSR" });
            return listado;
        }
    }
}
