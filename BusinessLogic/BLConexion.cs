using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic
{
    public class BLConexion
    {
        public static SqlConnection SIUBET()
        {
            SqlConnection Cno = new SqlConnection(ConfigurationManager.AppSettings["SIUBET"]);
            return Cno;
        }
    }
}
