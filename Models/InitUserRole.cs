using Microsoft.AspNetCore.Identity;

namespace SAMHO.Models
{
    /// <summary>
    /// Clase para configurar los roles iniciales y admin
    /// </summary>
    public class InitUserRole
    {
        public static bool AdminAndRoles(IServiceProvider serviceProvider, string[] args)
        {
            bool resultado = false;

            List<string> lRoles = ListaRoles(args);
            AdminUser adminUserData = AdminUserData(args);
            string rolAdministrador = RolAdministrador(args);

            InitUserRole IUrole = new();
            var userResult = IUrole.CreateRoles(serviceProvider, lRoles, adminUserData, rolAdministrador);

            return resultado;        
        }
        /// <summary>
        /// Retorna la lista de roles del archivo de configuracion
        /// </summary>
        /// <returns></returns>
        public static List<string> ListaRoles(string[] args)
        {
            List<string> roleNames = new();

            try
            {
                var builder = WebApplication.CreateBuilder(args);
                //Sacar lista de roles de appsettings
                var lRoles = builder.Configuration.GetSection("Roles").Get<List<string>>();

                foreach (var rol in lRoles)
                {
                    roleNames.Add(rol);
                }
            }
            catch (Exception e)
            { 
                //TODO error code            
            }

            //grabar en globales la lista de roles
            GlobalData.ROLELIST = roleNames;

            return roleNames;
        }

        /// <summary>
        /// Recoge los datos del super usuario desde el archivo de configuracion
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static AdminUser AdminUserData(string[] args)
        {
            AdminUser adminUserData = new();

            try
            {
                var builder = WebApplication.CreateBuilder(args);

                adminUserData.UserName = builder.Configuration.GetSection("AdminUserData")["AdminName"];
                adminUserData.Email = builder.Configuration.GetSection("AdminUserData")["AdminEmail"];
                adminUserData.Password = builder.Configuration.GetSection("AdminUserData")["AdminPWD"];
            }
            catch (Exception e)
            { 
                //TODO Error code            
            }

            return adminUserData;
        }

        /// <summary>
        /// Retorna El nombre del rol del administrador del archivo de configuracion
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static string RolAdministrador(string[] args)
        {
            string rolAdministrador = String.Empty;

            try
            {
                var builder = WebApplication.CreateBuilder(args);

                rolAdministrador = builder.Configuration.GetSection("Roles")["Administrador"];
            }
            catch (Exception e)
            {
                //TODO Error code            
            }

            return rolAdministrador;
        }


        /// <summary>
        /// Valida o crea los roles en la app
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        private async Task CreateRoles(IServiceProvider serviceProvider, List<string> lRoles, AdminUser adminUserData, string rolAdministrador)
        {

            try
            {
                using var scope = serviceProvider.CreateScope();

                //Inicializar roles
                UserManager<ApplicationUser>? _userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                RoleManager<IdentityRole>? _roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                

                //Crear Roles
                List<string> roleNames = lRoles;

                IdentityResult roleResult;

                foreach (var roleName in roleNames)
                {
                    var roleExist = await _roleManager.RoleExistsAsync(roleName);
                    if (!roleExist)
                {
                        //Crear roles y almacenar en base de datos
                        roleResult = await _roleManager.CreateAsync(new IdentityRole(roleName));
                    }
                }

                

                //Crear Admin ORIGINAL para la aplicacion
                //verificar si existe
                var _user = await _userManager.FindByEmailAsync(adminUserData.Email);
                if (_user == null)
                {
                    ApplicationUser ApplicationUser = new();

                    ApplicationUser.Email = adminUserData.Email;
                    ApplicationUser.UserName = adminUserData.Email;

                    ApplicationUser.IdEspecialidad = 0;
                    ApplicationUser.IdHorarioTrabajo = 0;
                    ApplicationUser.IdEstadoUsuario = 0;
                    ApplicationUser.IdTipoUsuario = 0;
                    ApplicationUser.PrimerNombre = "Admin";
                    ApplicationUser.PrimerApellido = "Admin";
                    ApplicationUser.SegundoNombre = "Admin";
                    ApplicationUser.SegundoApellido = "Admin";
                    ApplicationUser.FechaNacimiento = DateTime.Now;
                    ApplicationUser.Identificacion = "0000";
                    ApplicationUser.IdTipoIdentificacion = 0;
                    ApplicationUser.FechaCreacion = DateTime.Now;
                    ApplicationUser.IdPaisNacimiento = 61;
                    ApplicationUser.Direccion = "0000";
                    ApplicationUser.Sexo = GlobalData.SEXO.MASCULINO.ToString();

                    
                    var createAdmin = await _userManager.CreateAsync(ApplicationUser, adminUserData.Password);
                    if (createAdmin.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(ApplicationUser, rolAdministrador);
                    }
                }
            }
            catch (Exception e)
            { 
                //TODO error code
            }
        }

    }
}
