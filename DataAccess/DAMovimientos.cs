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

        public bool fnInsertarMovDD(BEMovimiento oDD) {
            bool rpta = false;
            try
            {
                SqlCommand cmd = new SqlCommand("spiSUX_InsertarMovDD", oCon);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@FechaEmision", oDD.FechaEmision);
                cmd.Parameters.AddWithValue("@FechaMov", oDD.FechaMov);
                cmd.Parameters.AddWithValue("@IDTipoMov", oDD.IDTipoMov);
                cmd.Parameters.AddWithValue("@IDResponsable", oDD.IDResponsable);
                cmd.Parameters.AddWithValue("@NumeroCargo", oDD.NumeroCargo);                
                cmd.Parameters.AddWithValue("@UndEjec_CAC", oDD.UndEjec_CAC);
                cmd.Parameters.AddWithValue("@Observaciones", oDD.Observaciones);
                cmd.Parameters.AddWithValue("@IdsExpedientes", oDD.ET_selected_D);
                cmd.Parameters.AddWithValue("@ExtensionFile", oDD.ExtensionFile);
                cmd.Parameters.AddWithValue("@Usuario", "irving");
                cmd.Parameters.AddWithValue("@rpta", 0).Direction = ParameterDirection.InputOutput;
                cmd.Parameters.AddWithValue("@Archivo", "0-0000-0000").Direction = ParameterDirection.InputOutput;
                oCon.Open();
                
                cmd.ExecuteNonQuery();
                rpta = Convert.ToBoolean(cmd.Parameters["@rpta"].Value);
                oDD.Archivo = cmd.Parameters["@Archivo"].Value.ToString();

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
                        if (oMov.IDTipoMov == 4)
                        {
                            oMov.Ing_Evaluador = dr["EntidadDestino"].ToString();
                        }
                        else {
                            oMov.UndEjec_CAC = dr["EntidadDestino"].ToString();
                        }
                        oMov.FechaFinal = dr["FechaFinal"].ToString();
                        oMov.NombreFileFinal= dr["NombreFileFinal"].ToString();
                        oMov.Plazo = Convert.ToInt32(dr["Plazo"]);
                        oMov.FechaRetornoEstimada = dr["FechaRetornoEstimada"].ToString();
                        oMov.Estado = dr["Estado"].ToString();
                        oMov.Motivo = dr["Motivo"].ToString();
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

        public bool fnRetornaPre_RecepcionaDD(BEMovimiento oMov)
        {
            bool rpta = false;
            try
            {
                SqlCommand cmd = new SqlCommand("spuSUX_RetornaPre_RecepcionaDD", oCon);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IDMovimiento", oMov.IDMovimiento);
                cmd.Parameters.AddWithValue("@FechaFinal", oMov.FechaFinal);
                cmd.Parameters.AddWithValue("@ExtensionFile", oMov.ExtensionFile);
                cmd.Parameters.AddWithValue("@IdsExpedientes", oMov.ET_selected_P);
                cmd.Parameters.AddWithValue("@Usuario", "irving");
                cmd.Parameters.AddWithValue("@rpta", 0).Direction = ParameterDirection.InputOutput;
                cmd.Parameters.AddWithValue("@Archivo", "0-0000-0000").Direction = ParameterDirection.InputOutput;
                oCon.Open();

                cmd.ExecuteNonQuery();
                rpta = Convert.ToBoolean(cmd.Parameters["@rpta"].Value);
                oMov.Archivo = cmd.Parameters["@Archivo"].Value.ToString();
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
                cmd.Parameters.AddWithValue("@Ing_Evaluador", oPrestamo.Ing_Evaluador);
                cmd.Parameters.AddWithValue("@Observaciones", oPrestamo.Observaciones);                
                cmd.Parameters.AddWithValue("@IdsExpedientes", oPrestamo.ET_selected_P);
                cmd.Parameters.AddWithValue("@ExtensionFile", oPrestamo.ExtensionFile);
                cmd.Parameters.AddWithValue("@Usuario", "irving");
                cmd.Parameters.AddWithValue("@rpta", 0).Direction = ParameterDirection.InputOutput;
                cmd.Parameters.AddWithValue("@Archivo", "0-0000-0000").Direction = ParameterDirection.InputOutput;
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

        public bool fnAnularMovimiento(BEMovimiento oMov)
        {
            bool rpta = false;
            try
            {
                SqlCommand cmd = new SqlCommand("spuSUX_AnularMovimiento", oCon);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IDMovimiento", oMov.IDMovimiento);
                cmd.Parameters.AddWithValue("@Motivo", oMov.Motivo);
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
