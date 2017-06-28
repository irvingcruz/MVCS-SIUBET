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
                        oUsuario.Nombres = dr["Nombres"].ToString();
                        rpta = true;
                    }
                }
            }
            catch (Exception e) { throw e; }
            finally { oCon.Close(); }
            return rpta;
        }

        public BEUsuario fnObtenerUsuario(string vUsuario) {
            BEUsuario oUsuario = new BEUsuario();
            try
            {
                SqlCommand cmd = new SqlCommand("spSUX_ObtenerUsuario", oCon);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Usuario", vUsuario);
                oCon.Open();
                using (SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (dr.Read())
                    {
                        oUsuario.IDUsuario = Convert.ToInt32(dr["IDUsuario"]);
                        oUsuario.IDPerfil = Convert.ToInt32(dr["IDPerfil"]);
                    }
                }
            }
            catch (Exception e) { throw e; }
            finally { oCon.Close(); }
            return oUsuario;
        }

        public int fnInsertarUpdateUsuario(BEUsuario oUsuario, string vUsuario)
        {
            int rpta = 0;                        

            try
            {
                SqlCommand cmd = new SqlCommand("spiuSUX_InsertarUpdateUsuario", oCon);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IDUsuario", oUsuario.IDUsuario);
                cmd.Parameters.AddWithValue("@UserName", oUsuario.UserName);
                cmd.Parameters.AddWithValue("@Password", oUsuario.Password);
                cmd.Parameters.AddWithValue("@Nombres", oUsuario.Nombres);
                cmd.Parameters.AddWithValue("@IDPerfil", oUsuario.IDPerfil);
                cmd.Parameters.AddWithValue("@Grupo", oUsuario.Grupo);            
                cmd.Parameters.AddWithValue("@Usuario", vUsuario);
                cmd.Parameters.AddWithValue("@rpta", 0).Direction = ParameterDirection.InputOutput;
                oCon.Open();

                cmd.ExecuteNonQuery();
                rpta = Convert.ToInt32(cmd.Parameters["@rpta"].Value);

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
