using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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

        [Display(ResourceType = typeof(RESOURCE), Name = "IdEspecialidad")]
        public int IdEspecialidad { get; set; }

        [Required(ErrorMessageResourceType = typeof(RESOURCE), ErrorMessageResourceName = "FormErrCampoRequerido")]

        [Display(ResourceType = typeof(RESOURCE), Name = "IdHorarioTrabajo")]
        public int IdHorarioTrabajo { get; set; }

        [Required(ErrorMessageResourceType = typeof(RESOURCE), ErrorMessageResourceName = "FormErrCampoRequerido")]
        [Display(ResourceType = typeof(RESOURCE), Name = "EstadoUsuario")]
        public int IdEstadoUsuario { get; set; }

        /// <summary>
        /// Solo Para vista NO ENTRA EN LA BASE DE DATOS!!!
        /// </summary>
        [Display(ResourceType = typeof(RESOURCE), Name = "RolUsuario")]
        public string RolUsuario { get; set; }

        [Required(ErrorMessageResourceType = typeof(RESOURCE), ErrorMessageResourceName = "FormErrCampoRequerido"), MaxLength(40)]
        [RegularExpression(@"^[a-zA-Z''-'\s]{1,40}$", ErrorMessageResourceType = typeof(RESOURCE), ErrorMessageResourceName = "FormErrRegEx")]
        [Display(ResourceType = typeof(RESOURCE), Name = "PrimerNombre")]
        public string PrimerNombre { get; set; }

        [Required(ErrorMessageResourceType = typeof(RESOURCE), ErrorMessageResourceName = "FormErrCampoRequerido"), MaxLength(40)]
        [RegularExpression(@"^[a-zA-Z''-'\s]{1,40}$", ErrorMessageResourceType = typeof(RESOURCE), ErrorMessageResourceName = "FormErrRegEx")]
        [Display(ResourceType = typeof(RESOURCE), Name = "PrimerApellido")]
        public string PrimerApellido { get; set; }

        [Required(ErrorMessageResourceType = typeof(RESOURCE), ErrorMessageResourceName = "FormErrCampoRequerido"), MaxLength(40)]
        [RegularExpression(@"^[a-zA-Z''-'\s]{1,40}$", ErrorMessageResourceType = typeof(RESOURCE), ErrorMessageResourceName = "FormErrRegEx")]
        [Display(ResourceType = typeof(RESOURCE), Name = "SegundoNombre")]
        public string SegundoNombre { get; set; }

        [Required(ErrorMessageResourceType = typeof(RESOURCE), ErrorMessageResourceName = "FormErrCampoRequerido"), MaxLength(40)]
        [RegularExpression(@"^[a-zA-Z''-'\s]{1,40}$", ErrorMessageResourceType = typeof(RESOURCE), ErrorMessageResourceName = "FormErrRegEx")]
        [Display(ResourceType = typeof(RESOURCE), Name = "SegundoApellido")]
        public string SegundoApellido { get; set; }

        [Required(ErrorMessageResourceType = typeof(RESOURCE), ErrorMessageResourceName = "FormErrCampoRequerido")]
        [Display(Name = "Fecha de Nacimiento")]
        [BindProperty, DataType(DataType.Date)]
        public DateTime FechaNacimiento { get; set; }

        [Required(ErrorMessageResourceType = typeof(RESOURCE), ErrorMessageResourceName = "FormErrCampoRequerido"), MaxLength(20)]
        public string Identificacion { get; set; }

        [Required(ErrorMessageResourceType = typeof(RESOURCE), ErrorMessageResourceName = "FormErrCampoRequerido")]
        [Display(Name = "Tipo de identificación")]
        public int IdTipoIdentificacion { get; set; }
        //IdRol
        //Email
        //Password
        [Required(ErrorMessageResourceType = typeof(RESOURCE), ErrorMessageResourceName = "FormErrCampoRequerido")]
        public DateTime FechaCreacion { get; set; }

        [Required(ErrorMessageResourceType = typeof(RESOURCE), ErrorMessageResourceName = "FormErrCampoRequerido")]
        [Display(Name = "IdPaisNacimiento")]
        public int IdPaisNacimiento { get; set; }

        [Required(ErrorMessageResourceType = typeof(RESOURCE), ErrorMessageResourceName = "FormErrCampoRequerido"), MaxLength(200)]
        [Display(Name = "Direccion")]
        public string Direccion { get; set; }

        [Required(ErrorMessageResourceType = typeof(RESOURCE), ErrorMessageResourceName = "FormErrCampoRequerido"), MaxLength(10)]
        [Display(Name = "Sexo")]
        public string Sexo { get; set; }


        /// <summary>
        /// Retorna un usuario localizado por el Id de la base de datos
        /// </summary>
        /// <param name="_userManager"></param>
        /// <param name="IdUsuario"></param>
        /// <returns></returns>
        public static ApplicationUser GetUsuarioporId(UserManager<ApplicationUser> _userManager, string IdUsuario)
        {
            
            ApplicationUser applicationUser = _userManager.FindByIdAsync(IdUsuario).Result;
            try
            {
                applicationUser.RolUsuario = _userManager.GetRolesAsync(applicationUser).Result.ToArray()[0];
            }
            catch(Exception e)
            {
                applicationUser.RolUsuario = RESOURCE.SeleccionaRol;
            }
            return applicationUser;
        }


        public static string GetUserRole(UserManager<ApplicationUser> _userManager, ApplicationUser loggedUser)
        {
            string userRole = string.Empty;

            try
            {
                userRole = _userManager.GetRolesAsync(loggedUser).Result.ToArray()[0];

            }
            catch (Exception e)
            { 
                //TODO Error code
            }

            return userRole;
        }


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
    }
}
