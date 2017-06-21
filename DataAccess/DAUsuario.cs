using BusinessEntity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class DAUsuario
    {
        SqlConnection oCon;
        public DAUsuario(SqlConnection _oCon)
        {
            this.oCon = _oCon;
        }

        public bool fnAutenticacion(BEUsuario oUsuario)
        {
            bool rpta = false;
            try
            {
                SqlCommand cmd = new SqlCommand("spSUX_Autenticacion", oCon);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Usuario", oUsuario.UserName);
                cmd.Parameters.AddWithValue("@Password", oUsuario.Password);
                oCon.Open();
                using (SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (dr.Read())
                    {
                        //oMov = new BEMovimiento();
                        oUsuario.IDUsuario = Convert.ToInt32(dr["IDUsuario"]);
                        oUsuario.IDPerfil = Convert.ToInt32(dr["IDPerfil"]);
                        oUsuario.Nombres= dr["Nombres"].ToString();
                        rpta = true;                        
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                oCon.Close();
            }
            return rpta;                    
        }
    }
}
