using Microsoft.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace SAMHO.Models
{
    public class Estado
    {
        [Key]
        public int IdEstado { get; set; }
        public string GrupoEstado { get; set; }

        public string TipoEstado { get; set; }


        /// <summary>
        /// Retorna el código de Estado para la entidad seleccionada
        /// </summary>
        /// <param name="GrupoEstado">Tipo de entidad</param>
        /// <param name="TipoEstado">Tipo de estado</param>
        /// <returns></returns>
        public static int GetCodigoEstadoUsuario(string GrupoEstado, string TipoEstado)
        {
            int returnValue = -1;

            try
            {
                string storedProcedure = "SP_ESTADOCODIGOPORGRUPOYTIPO";
                string parameterNameA = "@GrupoEstado";
                string parameterNameB = "@TipoEstado";

                var connection = GlobalData.APLICACION["connectionString"];

                using (SqlConnection con = new SqlConnection(connection))
                {
                    using (SqlCommand cmd = new SqlCommand(storedProcedure, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(parameterNameA, SqlDbType.VarChar).Value = GrupoEstado;
                        cmd.Parameters.Add(parameterNameB, SqlDbType.VarChar).Value = TipoEstado;

                        con.Open();
                        cmd.ExecuteNonQuery();

                        returnValue = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }
            }
            catch (Exception e)
            {
                //TODO ERROR CODE
            }

            return returnValue;

        }


        /// <summary>
        /// Retorna la lista de estados para el tipo de 
        /// </summary>
        /// <param name="isDDl"></param>
        /// <param name="TipoEstado"></param>
        /// <returns></returns>
        public static List<Estado> lEstados(bool isDDl, string GrupoEstado)
        { 
            List<Estado> lEstados = new List<Estado>();

            if (isDDl)
            {
                Estado estado = new Estado() { IdEstado = 0, GrupoEstado = String.Empty, TipoEstado = "Selecciona Estado" };
                lEstados.Add(estado);
            }

            try
            {
                var connection = GlobalData.APLICACION["connectionString"];

                using (SqlConnection con = new SqlConnection(connection))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_EstadosListar", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@GrupoEstado", SqlDbType.VarChar).Value = GrupoEstado;

                        con.Open();
                        cmd.ExecuteNonQuery();

                        SqlDataReader idr = cmd.ExecuteReader();

                        if (idr.HasRows)
                        {
                            while (idr.Read())
                            {
                                lEstados.Add(new Estado
                                {
                                    IdEstado = Convert.ToInt32(idr["IdEstado"]),
                                    GrupoEstado = idr["GrupoEstado"].ToString(),
                                    TipoEstado = idr["TipoEstado"].ToString()
                                });
                            }
                        }
                    }


                }
            }
            catch (Exception e)
            {
                //TODO error code            
            }

            return lEstados;
        
        }

    }
}
