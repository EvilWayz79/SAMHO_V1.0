using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace SAMHO.Models
{
    [Index(nameof(Identificacion), IsUnique = true)]
    public class ApplicationUser : IdentityUser
    {

        [Required(ErrorMessageResourceType = typeof(RESOURCE), ErrorMessageResourceName = "FormErrCampoRequerido")]
        public int IdEspecialidad { get; set; }

        [Required(ErrorMessageResourceType = typeof(RESOURCE), ErrorMessageResourceName = "FormErrCampoRequerido")]
        public int IdHorarioTrabajo { get; set; }

        [Required(ErrorMessageResourceType = typeof(RESOURCE), ErrorMessageResourceName = "FormErrCampoRequerido")]
        public int IdEstadoUsuario { get; set; }        

        [Required(ErrorMessageResourceType = typeof(RESOURCE), ErrorMessageResourceName = "FormErrCampoRequerido")]
        public int IdTipoUsuario { get; set; }

        [Required(ErrorMessageResourceType = typeof(RESOURCE), ErrorMessageResourceName = "FormErrCampoRequerido"), MaxLength(40)]
        [RegularExpression(@"^[a-zA-Z''-'\s]{1,40}$", ErrorMessageResourceType = typeof(RESOURCE), ErrorMessageResourceName = "FormErrRegEx")]
        public string PrimerNombre { get; set; }

        [Required(ErrorMessageResourceType = typeof(RESOURCE), ErrorMessageResourceName = "FormErrCampoRequerido"), MaxLength(40)]
        [RegularExpression(@"^[a-zA-Z''-'\s]{1,40}$", ErrorMessageResourceType = typeof(RESOURCE), ErrorMessageResourceName = "FormErrRegEx")]
        public string PrimerApellido { get; set; }

        [Required(ErrorMessageResourceType = typeof(RESOURCE), ErrorMessageResourceName = "FormErrCampoRequerido"), MaxLength(40)]
        [RegularExpression(@"^[a-zA-Z''-'\s]{1,40}$", ErrorMessageResourceType = typeof(RESOURCE), ErrorMessageResourceName = "FormErrRegEx")]
        public string SegundoNombre { get; set; }

        [Required(ErrorMessageResourceType = typeof(RESOURCE), ErrorMessageResourceName = "FormErrCampoRequerido"), MaxLength(40)]
        [RegularExpression(@"^[a-zA-Z''-'\s]{1,40}$", ErrorMessageResourceType = typeof(RESOURCE), ErrorMessageResourceName = "FormErrRegEx")]
        public string SegundoApellido { get; set; }

        [Required(ErrorMessageResourceType = typeof(RESOURCE), ErrorMessageResourceName = "FormErrCampoRequerido")]        
        public DateTime FechaNacimiento { get; set; }

        [Required(ErrorMessageResourceType = typeof(RESOURCE), ErrorMessageResourceName = "FormErrCampoRequerido"), MaxLength(20)]
        public string Identificacion { get; set; }

        [Required(ErrorMessageResourceType = typeof(RESOURCE), ErrorMessageResourceName = "FormErrCampoRequerido")]
        public int IdTipoIdentificacion { get; set; }
        //IdRol
        //Email
        //Password
        [Required(ErrorMessageResourceType = typeof(RESOURCE), ErrorMessageResourceName = "FormErrCampoRequerido")]
        public DateTime FechaCreacion { get; set; }

        [Required(ErrorMessageResourceType = typeof(RESOURCE), ErrorMessageResourceName = "FormErrCampoRequerido")]
        public int IdPaisNacimiento { get; set; }

        [Required(ErrorMessageResourceType = typeof(RESOURCE), ErrorMessageResourceName = "FormErrCampoRequerido"), MaxLength(200)]
        public string Direccion { get; set; }

        [Required(ErrorMessageResourceType = typeof(RESOURCE), ErrorMessageResourceName = "FormErrCampoRequerido"), MaxLength(10)]
        public string Sexo { get; set; }


        /// <summary>
        /// Retorna verdadero si la Identificacion (DOCUMENTO DE IDENTIDAD) es unica
        /// </summary>
        /// <param name="Id">Identificacion del usuario</param>
        /// <returns></returns>
        public static bool ValidarIdUnica(string Identificacion)
        {
            bool returnValue = true;

            try
            {
                string storedProcedure = "SP_USUARIOGETBYIDENTIFICACION";
                string parameterName = "@Identificacion";

                var connection = GlobalData.APLICACION["connectionString"];

                using (SqlConnection con = new SqlConnection(connection))
                {
                    using (SqlCommand cmd = new SqlCommand(storedProcedure, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(parameterName, SqlDbType.VarChar).Value = Identificacion;

                        con.Open();
                        cmd.ExecuteNonQuery();

                        int conteoId = Convert.ToInt32(cmd.ExecuteScalar());

                        if (conteoId > 0)
                            returnValue = false;
                    }
                }
            }
            catch (Exception e)
            { 
                //TODO ERROR CODE
            }

            return returnValue;
        
        }


        /*
        public ApplicationUser(string email, string username)
        { 
            this.Email = email;
            this.UserName = username;
            this.IdEspecialidad = string.Empty;
            this.IdEstadoEmpleado = string.Empty;
            this.IdEstadoUsuario = string.Empty;
            this.PrimerApellido = string.Empty;
            this.PrimerApellido = string.Empty;
            this.PrimerApellido = string.Empty;
            this.PrimerApellido = string.Empty;
            this.PrimerApellido = string.Empty;



        }*/
    }
}
