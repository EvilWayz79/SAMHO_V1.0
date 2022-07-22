using Microsoft.AspNetCore.Identity;
using SAMHO.Data;
using System.ComponentModel.DataAnnotations;

namespace SAMHO.Models
{
    public class AUViewModel
    {
        #region Propiedades
        //Para vista simple
        [Key]
        public string Id { get; set; }
        [Display(Name = "Nombre de Usuario")]
        public string UserName { get; set; }
        public string PrimerNombre { get; set; }
        public string SegundoNombre { get; set; }
        public string PrimerApellido { get; set; }
        public string SegundoApellido { get; set; }

        [Display(Name = "Nombre Completo")]
        public string NombreCompleto { get; set; }

        public int IdEstadoUsuario { get; set; }

        [Display(Name = "Estado de Usuario")]
        public string EstadoUsuario { get; set; }

        /// <summary>
        /// Se usará lista si se implementan varios roles por usuario, al momento no
        /// </summary>
        [Display(Name = "Tipo de Usuario")]
        public string ListaRoles { get; set; }

        // Para vista detallada
        public int IdEspecialidad { get; set; }
        public int IdHorarioTrabajo { get; set; }
        public DateTime FechaNacimiento { get; set; }

        [Display(Name = "Tipo de Identificación")]
        public string TipoIdentificacion { get; set; }
        public string Identificacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int IdPaisNacimiento { get; set; }

        [Display(Name = "País de Nacimiento")]
        public string PaisNacimiento { get; set; }
        public string Direccion { get; set; }
        public string Sexo { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }


        #endregion Propiedades




        public static AUViewModel GetUsuarioporId(UserManager<ApplicationUser> _userManager, string IdUsuario)
        {
            string espacio = " ";
            AUViewModel viewUser = new AUViewModel();

            ApplicationUser applicationUser = _userManager.FindByIdAsync(IdUsuario).Result;
            applicationUser.RolUsuario = _userManager.GetRolesAsync(applicationUser).Result.ToArray()[0];

            viewUser.PrimerNombre = applicationUser.PrimerNombre;
            viewUser.SegundoNombre = applicationUser.SegundoNombre;
            viewUser.PrimerApellido = applicationUser.PrimerApellido;
            viewUser.SegundoApellido = applicationUser.SegundoApellido;
            viewUser.NombreCompleto = string.Concat(applicationUser.PrimerNombre, espacio, applicationUser.SegundoNombre, espacio,
                applicationUser.PrimerApellido, espacio, applicationUser.SegundoApellido);
            viewUser.UserName = applicationUser.UserName;
            viewUser.Direccion = applicationUser.Direccion;
            viewUser.Email = applicationUser.Email;
            viewUser.FechaCreacion = applicationUser.FechaCreacion;
            viewUser.FechaNacimiento = applicationUser.FechaNacimiento;
            viewUser.Id = applicationUser.Id;
            viewUser.TipoIdentificacion = GlobalData.ParseEnum<GlobalData.TIPOIDENTIFICACION>(applicationUser.IdTipoIdentificacion.ToString()).ToString();
            viewUser.Identificacion = applicationUser.Identificacion;
            viewUser.IdEspecialidad = applicationUser.IdEspecialidad;
            viewUser.IdEstadoUsuario = applicationUser.IdEstadoUsuario;
            viewUser.EstadoUsuario = Estado.GetCodigoEstadoUsuario(Convert.ToInt32(applicationUser.IdEstadoUsuario)).TipoEstado;
            viewUser.IdHorarioTrabajo = applicationUser.IdHorarioTrabajo;
            viewUser.PaisNacimiento = Pais.PaisPorId(applicationUser.IdPaisNacimiento).NombrePais;
            viewUser.ListaRoles = applicationUser.RolUsuario;
            viewUser.Sexo = GlobalData.ParseEnum<GlobalData.SEXO>(applicationUser.Sexo.ToString()).ToString();
            viewUser.Telefono = applicationUser.PhoneNumber;
            viewUser.UserName = applicationUser.UserName;


            return viewUser;
        }

        /// <summary>
        /// Recupera todos los usuarios
        /// </summary>
        /// <param name="_userManager"></param>
        /// <param name="_context"></param>
        /// <returns></returns>
        public static List<AUViewModel> ListarUsuarios(UserManager<ApplicationUser> _userManager, ApplicationDbContext _context)
        {
            List<AUViewModel> lUsuarios = new();

            foreach (ApplicationUser appUser in _context.Users.ToList())
            {
                //datos de prueba dan error artilugio de control
                string rol = "EditarRol";
                try
                {
                    rol = _userManager.GetRolesAsync(appUser).Result.ToArray()[0];
                }
                catch (Exception e)
                { 
                    //NADA!!!
                
                }
                AUViewModel auVIew = new()
                {
                    Id = appUser.Id,
                    UserName = appUser.UserName,
                    PrimerNombre = appUser.PrimerNombre,
                    SegundoNombre = appUser.SegundoNombre,
                    PrimerApellido = appUser.PrimerApellido,
                    SegundoApellido = appUser.SegundoApellido,
                    IdEstadoUsuario = appUser.IdEstadoUsuario,
                    ListaRoles = rol
                };

                lUsuarios.Add(auVIew);
            }

            return lUsuarios;
        }
    }
}
