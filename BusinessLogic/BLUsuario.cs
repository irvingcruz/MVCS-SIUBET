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
            catch (Exception)
            {
                throw;
            }
        }
    }
}
