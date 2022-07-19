using Microsoft.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace SAMHO.Models
{
    public class Especialidad
    {
        const string _ESPECIALIDAD = "ESPECIALIDAD";

        [Key]
        public int IdEspecialidad { get; set; }
        [Required(ErrorMessageResourceType = typeof(RESOURCE), ErrorMessageResourceName = "FormErrCampoRequerido"), MaxLength(20)]
        public string NombreEspecialidad { get; set; }

        [RegularExpression(@"^[1-9\D]{1,3}$", ErrorMessageResourceType = typeof(RESOURCE), ErrorMessageResourceName = "SeleccionarEstado")]
        public string IdEstadoEspecialidad { get; set; }
        
        public string? EstadoEspecialidad { get; set; }

        public static List<Dictionary<string, string>> lEstadosEspecialidad()
        { 
            List<Dictionary<string, string>> lEstadosEspecialidad = new List<Dictionary<string, string>>();

            try
            {   
                string appSettingsPath = Path.Combine("\\..\\Models\\","modelData.json");
                var _config = new ConfigurationBuilder().AddJsonFile(appSettingsPath).Build();

                var config = _config.GetSection("Estados");
            }
            catch (Exception e)
            { 
                //TODO Error
            
            }

            return lEstadosEspecialidad;
        }

        /// <summary>
        /// Retorna especialidad por id o por nombre
        /// </summary>
        /// <param name="idEspecialidad"></param>
        /// <param name="nombreEspecialidad"></param>
        /// <returns></returns>
        public static Especialidad BuscarEspecialidad(int? idEspecialidad, string? nombreEspecialidad)
        { 
            Especialidad especialidad = new Especialidad();

            string storedProcedure = string.Empty;
            string parameterName = string.Empty;

            try
            {
                if (idEspecialidad != null)
                {
                    storedProcedure = "SP_ESPECIALIDADBUSCARID";
                    parameterName = "@IDESPECIALIDAD";
                }
                else
                {
                    if (nombreEspecialidad != null)
                    {
                        storedProcedure = "SP_ESPECIALIDADBUSCARNOMBRE";
                        parameterName = "@NOMBREESPECIALIDAD";
                    }
                }
                var connection = GlobalData.APLICACION["connectionString"];

                using (SqlConnection con = new SqlConnection(connection))
                {
                    using (SqlCommand cmd = new SqlCommand(storedProcedure, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        /*
                        parameter = Convert.ToInt32(idEstado);
                        parameter = nombreEspecialidad.ToString();
                        */
                        if (idEspecialidad != null)
                            cmd.Parameters.Add(parameterName, SqlDbType.VarChar).Value = Convert.ToInt32(idEspecialidad);
                        else
                            cmd.Parameters.Add(parameterName, SqlDbType.VarChar).Value = nombreEspecialidad.ToString();

                        con.Open();
                        cmd.ExecuteNonQuery();

                        SqlDataReader idr = cmd.ExecuteReader();

                        if (idr.HasRows)
                        {
                            while (idr.Read())
                            {
                                especialidad.IdEspecialidad = Convert.ToInt32(idr["IdEspecialidad"]);
                                especialidad.IdEstadoEspecialidad = idr["IdEstadoEspecialidad"].ToString();
                                foreach (Estado est in Estado.lEstados(false, _ESPECIALIDAD))
                                {
                                    if (est.IdEstado == Convert.ToInt32(idr["IdEstadoEspecialidad"]))
                                        especialidad.EstadoEspecialidad = est.TipoEstado;
                                    else
                                        especialidad.EstadoEspecialidad = String.Empty;
                                    //especialidad.EstadoEspecialidad = idr["GrupoEstado"].ToString();
                                    especialidad.NombreEspecialidad = idr["NombreEspecialidad"].ToString();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                //TODO Error code

            }
            return especialidad;
        }

        /// <summary>
        /// Metodo para editar una especialidad
        /// </summary>
        /// <param name="editarEspecialidad"></param>
        /// <returns></returns>
        public static bool EditarEspecialidad(Especialidad editarEspecialidad)
        {
            bool returnValue = false;

            try
            {
                //validar si existe el nombre de especialidad
                editarEspecialidad.NombreEspecialidad = editarEspecialidad.NombreEspecialidad.ToUpper();

                var connection = GlobalData.APLICACION["connectionString"];

                using (SqlConnection con = new SqlConnection(connection))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_ESPECIALIDADEDITAR", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@IDESPECIALIDAD", SqlDbType.Int).Value = editarEspecialidad.IdEspecialidad;
                        cmd.Parameters.Add("@NOMBREESPECIALIDAD", SqlDbType.VarChar).Value = editarEspecialidad.NombreEspecialidad;
                        cmd.Parameters.Add("@IDESTADOESPECIALIDAD", SqlDbType.Int).Value = editarEspecialidad.IdEstadoEspecialidad;

                        con.Open();
                        int result = cmd.ExecuteNonQuery();

                        if (result == 1)
                            returnValue = true;
                    }
                }
            }
            catch (Exception e)
            {
                //TODO Error code

            }
            return returnValue;
        }

        /// <summary>
        /// Crea Una nueva Especialidad en la base de datos validando que no este ya creada
        /// </summary>
        /// <param name="nuevaEspecialidad"></param>
        /// <returns></returns>
        public static bool CrearEspecialidad(Especialidad nuevaEspecialidad)
        {
            bool returnValue = false;

            try
            {
                //validar si existe el nombre de especialidad
                nuevaEspecialidad.NombreEspecialidad = nuevaEspecialidad.NombreEspecialidad.ToUpper();

                var connection = GlobalData.APLICACION["connectionString"];

                using (SqlConnection con = new SqlConnection(connection))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_EspecialidadCrear", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@NOMBREESPECIALIDAD", SqlDbType.VarChar).Value = nuevaEspecialidad.NombreEspecialidad;
                        cmd.Parameters.Add("@IDESTADOESPECIALIDAD", SqlDbType.Int).Value = nuevaEspecialidad.IdEstadoEspecialidad;

                        con.Open();
                        int result = cmd.ExecuteNonQuery();

                        if (result == 1)
                            returnValue = true;
                    }
                }
            }
            catch (Exception e)
            { 
                //TODO Error code
            
            }
            return returnValue;
        }


        /// <summary>
        /// Retorna La lista de Especialidades y sus estados de la base de datos
        /// </summary>
        /// <param name="esDdl">Discrimina si vas a usarla en DDL o solo para despliegue</param>
        /// <returns></returns>
        public static List<Especialidad> ListaEspecialidades(bool esDdl)
        {
            List<Especialidad> lEspecialidad = new();
            try
            {
                if (esDdl)
                {
                    Especialidad especialidad = new Especialidad() { IdEspecialidad = 0, NombreEspecialidad = "SELECCIONA ESPECIALIDAD", EstadoEspecialidad = "ACTIVO" };
                    lEspecialidad.Add(especialidad);
                }

                var connection = GlobalData.APLICACION["connectionString"];

                using (SqlConnection con = new SqlConnection(connection))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_ESPECIALIDADESLISTAR", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        con.Open();
                        cmd.ExecuteNonQuery();

                        SqlDataReader idr = cmd.ExecuteReader();

                        if (idr.HasRows)
                        {
                            while (idr.Read())
                            {
                                lEspecialidad.Add(new Especialidad
                                {
                                    IdEspecialidad = Convert.ToInt32(idr["IdEspecialidad"]),
                                    NombreEspecialidad = idr["NombreEspecialidad"].ToString(),
                                    IdEstadoEspecialidad= idr["IDESTADOESPECIALIDAD"].ToString(),
                                    EstadoEspecialidad = idr["EstadoEspecialidad"].ToString(),                                    
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                //TODO Error code        
            }

            return lEspecialidad;
        }
    }    
}
