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
    public class DAMovimientos
    {
        SqlConnection oCon;
        public DAMovimientos(SqlConnection _oCon)
        {
            this.oCon = _oCon;
        }

        public bool fnInsertarMovDevolucion(BEMovimiento oDevolucion) {
            bool rpta = false;
            try
            {
                SqlCommand cmd = new SqlCommand("spiSUX_InsertarMovDevolucion", oCon);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@FechaEmision", oDevolucion.FechaEmision);
                cmd.Parameters.AddWithValue("@FechaMov", oDevolucion.FechaMov);
                cmd.Parameters.AddWithValue("@IDTipoMov", oDevolucion.IDTipoMov);
                cmd.Parameters.AddWithValue("@IDResponsable", oDevolucion.IDResponsable);
                cmd.Parameters.AddWithValue("@NumeroCargo", oDevolucion.NumeroCargo);                
                cmd.Parameters.AddWithValue("@EntidadDestino", oDevolucion.EntidadDestino);
                cmd.Parameters.AddWithValue("@Observaciones", oDevolucion.Observaciones);
                cmd.Parameters.AddWithValue("@IdsExpedientes", oDevolucion.ET_selected_D);
                cmd.Parameters.AddWithValue("@ExtensionFile", oDevolucion.ExtensionFile);
                cmd.Parameters.AddWithValue("@Usuario", "irving");
                cmd.Parameters.AddWithValue("@rpta", 0).Direction = ParameterDirection.InputOutput;
                cmd.Parameters.AddWithValue("@Archivo", "0000-0000").Direction = ParameterDirection.InputOutput;
                oCon.Open();
                
                cmd.ExecuteNonQuery();
                rpta = Convert.ToBoolean(cmd.Parameters["@rpta"].Value);
                oDevolucion.Archivo = cmd.Parameters["@Archivo"].Value.ToString();

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

        public List<BERespDev> fnListarResponsables()
        {
            List<BERespDev> listado = new List<BERespDev>();
            try
            {
                SqlCommand cmd = new SqlCommand("spSUX_ListarResponsables", oCon);
                cmd.CommandType = CommandType.StoredProcedure;
                oCon.Open();
                using (SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (dr.Read())
                    {
                        BERespDev oResp = new BERespDev();
                        oResp.IDResponsable = Convert.ToInt32(dr["IDResponsable"]);
                        oResp.Descripcion= dr["Descripcion"].ToString();                       
                        listado.Add(oResp);
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
            return listado;
        }

        public List<BEMovimiento> fnListarMovimientos(int Snip, int IDTipoMov)
        {
            List<BEMovimiento> listado = new List<BEMovimiento>();
            try
            {
                SqlCommand cmd = new SqlCommand("spSUX_ListarMovimientos", oCon);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Snip", Snip);
                cmd.Parameters.AddWithValue("@IDTipoMov", IDTipoMov);
                oCon.Open();
                using (SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (dr.Read())
                    {
                        BEMovimiento oMov = new BEMovimiento();
                        oMov.Nro = Convert.ToInt32(dr["Nro"]);
                        oMov.Correlativo = dr["Correlativo"].ToString();
                        oMov.IDMovimiento = Convert.ToInt32(dr["IDMovimiento"]);
                        oMov.IDTipoMov = Convert.ToInt32(dr["IDTipoMov"]);
                        oMov.TipoMov = dr["TipoMov"].ToString();
                        oMov.FechaEmision = dr["FechaEmision"].ToString();
                        oMov.NombreFileEmision = dr["NombreFileEmision"].ToString();
                        oMov.FechaMov = dr["FechaMov"].ToString();
                        oMov.Responsable = dr["Responsable"].ToString();
                        oMov.NumeroCargo = dr["NumeroCargo"].ToString();
                        oMov.NombreFileCargo = dr["NombreFileCargo"].ToString();
                        oMov.EntidadDestino = dr["EntidadDestino"].ToString();
                        oMov.FechaFinal = dr["FechaFinal"].ToString();
                        oMov.NombreFileFinal= dr["NombreFileFinal"].ToString();
                        oMov.Plazo = Convert.ToInt32(dr["Plazo"]);
                        oMov.FechaRetornoEstimada = dr["FechaRetornoEstimada"].ToString();
                        oMov.Estado = dr["Estado"].ToString();
                        oMov.Activo = Convert.ToBoolean(dr["Activo"]);
                        listado.Add(oMov);
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
            return listado;
        }

        public bool fnRetornaPre_RecepcionaDev(BEMovimiento oDevolucion)
        {
            bool rpta = false;
            try
            {
                SqlCommand cmd = new SqlCommand("spuSUX_RetornaPre_RecepcionaDev", oCon);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IDMovimiento", oDevolucion.IDMovimiento);
                cmd.Parameters.AddWithValue("@FechaFinal", oDevolucion.FechaFinal);
                cmd.Parameters.AddWithValue("@ExtensionFile", oDevolucion.ExtensionFile);
                cmd.Parameters.AddWithValue("@Usuario", "irving");
                cmd.Parameters.AddWithValue("@rpta", 0).Direction = ParameterDirection.InputOutput;
                cmd.Parameters.AddWithValue("@Archivo", "0000-0000").Direction = ParameterDirection.InputOutput;
                oCon.Open();

                cmd.ExecuteNonQuery();
                rpta = Convert.ToBoolean(cmd.Parameters["@rpta"].Value);
                oDevolucion.Archivo = cmd.Parameters["@Archivo"].Value.ToString();
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

        public bool fnInsertarMovPrestamo(BEMovimiento oPrestamo) {
            bool rpta = false;
            try
            {
                SqlCommand cmd = new SqlCommand("spiSUX_InsertarMovPrestamo", oCon);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@FechaMov", oPrestamo.FechaMov);
                cmd.Parameters.AddWithValue("@IDTipoMov", oPrestamo.IDTipoMov);
                cmd.Parameters.AddWithValue("@Plazo", oPrestamo.Plazo);                
                cmd.Parameters.AddWithValue("@EntidadDestino", oPrestamo.EntidadDestino);
                cmd.Parameters.AddWithValue("@Observaciones", oPrestamo.Observaciones);                
                cmd.Parameters.AddWithValue("@IdsExpedientes", oPrestamo.ET_selected_P);
                cmd.Parameters.AddWithValue("@ExtensionFile", oPrestamo.ExtensionFile);
                cmd.Parameters.AddWithValue("@Usuario", "irving");
                cmd.Parameters.AddWithValue("@rpta", 0).Direction = ParameterDirection.InputOutput;
                cmd.Parameters.AddWithValue("@Archivo", "0000-0000").Direction = ParameterDirection.InputOutput;
                oCon.Open();

                cmd.ExecuteNonQuery();
                rpta = Convert.ToBoolean(cmd.Parameters["@rpta"].Value);
                oPrestamo.Archivo = cmd.Parameters["@Archivo"].Value.ToString();

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

        public bool fnAnularMovimiento(BEMovimiento oDevolucion)
        {
            bool rpta = false;
            try
            {
                SqlCommand cmd = new SqlCommand("spuSUX_AnularMovimiento", oCon);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IDMovimiento", oDevolucion.IDMovimiento);
                cmd.Parameters.AddWithValue("@Motivo", oDevolucion.Motivo);
                cmd.Parameters.AddWithValue("@Usuario", "irving");
                cmd.Parameters.AddWithValue("@rpta", 0).Direction = ParameterDirection.InputOutput;
                oCon.Open();

                cmd.ExecuteNonQuery();
                rpta = Convert.ToBoolean(cmd.Parameters["@rpta"].Value);

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
