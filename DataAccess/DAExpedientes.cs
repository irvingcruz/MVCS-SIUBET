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
    public class DAExpedientes
    {
        SqlConnection oCon;
        public DAExpedientes(SqlConnection _oCon) {
            this.oCon = _oCon;
        }

        public List<BEExpediente> fnListarExpedientes(BEExpediente _oExp)
        {
            List<BEExpediente> listado = new List<BEExpediente>();
            try
            {
                SqlCommand cmd = new SqlCommand("spSUX_ListarExpedientes", oCon);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Snip", _oExp.Snip);
                cmd.Parameters.AddWithValue("@NumeroHT", _oExp.NumeroHT);
                cmd.Parameters.AddWithValue("@Estado", _oExp.Estado);
                cmd.Parameters.AddWithValue("@Etapa", _oExp.Etapa);                
                oCon.Open();
                using (SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (dr.Read())
                    {
                        BEExpediente oExp = new BEExpediente();
                        oExp.Nro = Convert.ToInt32(dr["Nro"]);
                        oExp.IDVersion = Convert.ToInt32(dr["ID"]);
                        oExp.IDExpTecnico = Convert.ToInt32(dr["IDExpTecnico"]);
                        oExp.Snip = Convert.ToInt32(dr["Snip"]);
                        oExp.NombreProyecto = dr["NombreProyecto"].ToString();
                        oExp.NumeroHT = dr["NumeroHT"].ToString();
                        oExp.NVersion = dr["NVersion"].ToString();                        
                        oExp.Estado = dr["Estado"].ToString();
                        oExp.Etapa = dr["Etapa"].ToString();
                        oExp.IDTipoMov = Convert.ToInt32(dr["IDTipoMov"]);
                        oExp.UbiTopografica = dr["UbiTopografica"].ToString();
                        oExp.Activo = Convert.ToBoolean(dr["Activo"]);
                        listado.Add(oExp);
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

        public List<BEExpediente> fnListarExpedientesSelected(string IdsExpedientes) {
            List<BEExpediente> listado = new List<BEExpediente>();
            try
            {
                SqlCommand cmd = new SqlCommand("spSUX_ListarExpedientesSelected", oCon);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdsExpedientes", IdsExpedientes);
                oCon.Open();
                using (SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (dr.Read())
                    {
                        BEExpediente oExp = new BEExpediente();
                        oExp.Nro = Convert.ToInt32(dr["Nro"]);
                        oExp.IDVersion = Convert.ToInt32(dr["ID"]);
                        oExp.Snip = Convert.ToInt32(dr["Snip"]);
                        oExp.NVersion = dr["Numero"].ToString();
                        oExp.DocumentoOficioSITRAD = dr["DocumentoOficioSITRAD"].ToString();
                        oExp.FechaOficio = dr["FechaOficio"].ToString();
                        oExp.NumeroHT = dr["NumeroHT"].ToString();
                        oExp.FechaIngreso = dr["FechaIngreso"].ToString();
                        oExp.Documento = dr["Documento"].ToString();
                        oExp.UnidadConservacion = dr["UnidadConservacion"].ToString();
                        oExp.Folios = dr["Folios"].ToString();
                        oExp.CDs = dr["CDs"].ToString();
                        listado.Add(oExp);
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

        public List<BEMovimiento> fnListarExpedientesHistorial(int IDExpedienteVersion)
        {
            List<BEMovimiento> listado = new List<BEMovimiento>();
            try
            {
                SqlCommand cmd = new SqlCommand("spSUX_ListarExpedientesHistorial", oCon);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IDVersion", IDExpedienteVersion);
                oCon.Open();
                using (SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (dr.Read())
                    {
                        BEMovimiento oExp = new BEMovimiento();
                        oExp.Nro = Convert.ToInt32(dr["Nro"]);
                        oExp.FechaMov = dr["FechaMov"].ToString();
                        oExp.TipoMov = dr["TipoMov"].ToString();           
                        oExp.Responsable = dr["Responsable"].ToString();
                        oExp.NumeroCargo = dr["NumeroCargo"].ToString();
                        oExp.NombreFileCargo = dr["NombreFileCargo"].ToString();
                        if (Convert.ToInt32(dr["IDTipoMov"]) == 4) { oExp.Ing_Evaluador = dr["EntidadDestino"].ToString(); }
                        else { oExp.UndEjec_CAC = dr["EntidadDestino"].ToString(); }
                        oExp.Observaciones = dr["Observaciones"].ToString();
                        oExp.FechaFinal = dr["FechaFinal"].ToString();
                        oExp.Estado = dr["Estado"].ToString();
                        oExp.Numero = dr["Numero"].ToString();
                        oExp.Folios = dr["Folios"].ToString();
                        oExp.CDs = dr["CDs"].ToString();
                        oExp.Planos = dr["Planos"].ToString();
                        listado.Add(oExp);
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

        public List<BEPersona> fnListarPersona(int Tipo)
        {
            List<BEPersona> listado = new List<BEPersona>();
            try
            {
                SqlCommand cmd = new SqlCommand("spSUX_ListarPersonas", oCon);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Tipo", Tipo);
                oCon.Open();
                using (SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (dr.Read())
                    {
                        BEPersona oUE = new BEPersona();
                        oUE.IDPersona = Convert.ToInt32(dr["ID"]);
                        oUE.Nombres = dr["Nombres"].ToString().Trim();                       
                        listado.Add(oUE);
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

        public List<BEExpediente> fnListarExpedientesEnRetorno(int IDMovimiento)
        {
            List<BEExpediente> listado = new List<BEExpediente>();
            try
            {
                SqlCommand cmd = new SqlCommand("spSUX_ListarExpedientesEnRetorno", oCon);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IDMovimiento", IDMovimiento);
                oCon.Open();
                using (SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (dr.Read())
                    {
                        BEExpediente oExp = new BEExpediente();
                        oExp.Nro = Convert.ToInt32(dr["Nro"]);
                        oExp.IDVersion = Convert.ToInt32(dr["IDVersion"]);
                        oExp.Snip = Convert.ToInt32(dr["Snip"]);
                        oExp.NumeroHT = dr["NumeroHT"].ToString();
                        oExp.NVersion = dr["NVersion"].ToString();                        
                        listado.Add(oExp);
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
        public bool fnInsertarUpdateExpediente(BEExpediente oExp, string vUsuario)
        {
            bool rpta = false;
            try
            {
                SqlCommand cmd = new SqlCommand("spiuSUX_InsertarUpdateExpediente", oCon);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IDVersion", oExp.IDVersion);
                cmd.Parameters.AddWithValue("@Snip", oExp.Snip);
                cmd.Parameters.AddWithValue("@Seccion", oExp.Seccion);
                cmd.Parameters.AddWithValue("@Serie", oExp.Serie);
                cmd.Parameters.AddWithValue("@SubSerie", oExp.SubSerie);
                cmd.Parameters.AddWithValue("@NumeroHT", oExp.NumeroHT);
                cmd.Parameters.AddWithValue("@DocIngreso", oExp.NVersion);
                cmd.Parameters.AddWithValue("@Ubicacion", oExp.UbiTopografica);
                cmd.Parameters.AddWithValue("@Usuario", vUsuario);
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

        public bool fnActualizarEtapaET(string vEtapa, string IdsExpedientes, string vUsuario)
        {
            bool rpta = false;
            try
            {
                SqlCommand cmd = new SqlCommand("spuSUX_ActualizarEtapaET", oCon);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Etapa", vEtapa);
                cmd.Parameters.AddWithValue("@IdsExpedientes", IdsExpedientes);               
                cmd.Parameters.AddWithValue("@Usuario", vUsuario);
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
