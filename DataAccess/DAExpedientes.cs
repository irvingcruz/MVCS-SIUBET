﻿using BusinessEntity;
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
                cmd.Parameters.AddWithValue("@Numero", _oExp.NVersion);
                cmd.Parameters.AddWithValue("@NombreProyecto", _oExp.NombreProyecto);
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
                        oExp.Seccion = dr["Seccion"].ToString();
                        oExp.Serie = dr["Serie"].ToString();
                        oExp.SubSerie = dr["SubSerie"].ToString();
                        oExp.NVersion = dr["NVersion"].ToString();
                        oExp.EstadoActual = dr["EstadoActual"].ToString();
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
                        oExp.EntidadDestino = dr["EntidadDestino"].ToString();
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
    }
}
