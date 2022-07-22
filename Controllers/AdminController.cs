using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SAMHO.Data;
using SAMHO.Models;

namespace SAMHO.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class AdminController : Controller
    {   
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }




        // GET: Usuario/Crear
        public IActionResult CrearUsuario()
        {
            //*****requerimientos para el formulario*****\\
            //Lista roles => validar tipo de rol para pasar siguientes parámetros
            //Lista especialidades
            //lista horarios
            //lista Paises
            //Lista estados
            //Lista tipo ids
            //Lista Sexo
            //*******************************************\\

            List<string> roles = _context.Roles.Select(r => r.ToString()).ToList();
            
            List<SelectListItem> lRoles = new List<SelectListItem>();

            for (int i = 0; i < roles.Count; i++)
            {
                SelectListItem listItem = new SelectListItem
                {
                    Value = roles[i],
                    Text = roles[i],
                    Selected = false
                };
                lRoles.Add(listItem);
            }

            List<Especialidad> especialidades = Especialidad.ListaEspecialidades(true);
            List<SelectListItem> lEspecialidades = new();
            foreach (Especialidad item in especialidades)
            {
                SelectListItem listItem = new SelectListItem
                {
                    Value = item.IdEspecialidad.ToString(),
                    Text = item.NombreEspecialidad,
                    Selected = false
                };
                lEspecialidades.Add(listItem);
            }


            List<HorarioTrabajo> lHtrabajo = new();

            if (_context.HorarioTrabajo != null)
            {
                foreach (var horario in _context.HorarioTrabajo.ToList())
                {
                    HorarioTrabajo ht = new HorarioTrabajo
                    {
                        HFin = horario.HFin,
                        HInicio = horario.HInicio,
                        IdEstado = horario.IdEstado,
                        IdHorarioTrabajo = horario.IdHorarioTrabajo,
                        MFin = horario.MFin,
                        MInicio = horario.MInicio,
                        DiasSemana = horario.DiasSemana,
                        NombreHorario = horario.NombreHorario
                    };
                    lHtrabajo.Add(ht);
                }
            }
            else 
            {
                //validar que hacer en caso de no creado horario de trabajo
            }
            
            List<SelectListItem> lPais = new();
            foreach (var item in Pais.ListaPaises())
            {
                SelectListItem itemPais = new SelectListItem { Value = item.IdPais.ToString().Trim(), Text = item.NombrePais};
                lPais.Add(itemPais);
            }

            List<SelectListItem> lEstados = new List<SelectListItem>();
            foreach (var item in Estado.lEstados(true, "USUARIO"))
            {
                SelectListItem itemEstado = new SelectListItem { Value = item.IdEstado.ToString().Trim(), Text = item.TipoEstado };
                lEstados.Add(itemEstado);
            }

            List<SelectListItem> lTipoId = new List<SelectListItem>();

            lTipoId = Enum.GetValues(typeof(GlobalData.TIPOIDENTIFICACION)).Cast<GlobalData.TIPOIDENTIFICACION>().Select(v => new SelectListItem
            {
                Text = v.ToString(),
                Value = ((int)v).ToString()
            }).ToList();

            List<SelectListItem> lSexo = new List<SelectListItem>();

            lSexo = Enum.GetValues(typeof(GlobalData.SEXO)).Cast<GlobalData.SEXO>().Select(v => new SelectListItem
            {
                Text = v.ToString(),
                Value = ((int)v).ToString()
            }).ToList();

            ViewBag.lRoles = lRoles;
            ViewBag.lEspecialidades = lEspecialidades;
            ViewBag.lHtrabajo = lHtrabajo;
            ViewBag.lPais = lPais;
            ViewBag.lEstados = lEstados;
            ViewBag.lTipoId = lTipoId;
            ViewBag.lSexo = lSexo;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CrearUsuario([Bind(
                "Direccion", "Email", "FechaCreacion", "FechaNacimiento",
                "Id", "IdEspecialidad", "IdEstadoUsuario", "IdHorarioTrabajo", "IdPaisNacimiento",
                "IdTipoIdentificacion", "Identificacion",
                "PhoneNumber", "PrimerApellido",
                "PrimerNombre", "RolUsuario", "SegundoApellido", "SegundoNombre",
                "Sexo", "UserName")] ApplicationUser usuarioModelo)
        {
            if (usuarioModelo.Id != null)
            {



            }


            if (ModelState.IsValid)
            {
                //password generico para todo usuario creado por el administrador, configurado en appsettings
                string appSettingsPath = Path.Combine("appsettings.json");
                var _config = new ConfigurationBuilder().AddJsonFile(appSettingsPath).Build();
                var configUpwd = _config.GetSection("GeneralUserData")["UserPWD"];

                usuarioModelo.UserName = usuarioModelo.Email;
                usuarioModelo.NormalizedUserName = usuarioModelo.Email.ToUpper();

                usuarioModelo.FechaCreacion = DateTime.Now;

                /*validar que no se tomen valores de empleado para pacientes
                 */

                if (usuarioModelo.RolUsuario == "Paciente")
                {
                    usuarioModelo.IdEspecialidad = 0;
                    usuarioModelo.IdHorarioTrabajo = 0;
                }

                var result = await _userManager.CreateAsync(usuarioModelo, configUpwd);
                await _context.SaveChangesAsync();

                if (result.Succeeded)
                {
                    /* Logear transaccion
                    _logger.LogInformation(RESOURCE.PacienteNuevoRegistrado);

                    var userId = await _userManager.GetUserIdAsync(user);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);*/

                    //Adicionar el rol de paciente al paciente nuevo

                    await _userManager.AddToRoleAsync(usuarioModelo, usuarioModelo.RolUsuario);


                    /* Enviar correo de confirmacion
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }*/
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }



                return RedirectToAction(nameof(ListarUsuarios));
            }

            return View(usuarioModelo);
        }




        /// <summary>
        /// Edita un usuario existente
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult EditarUsuario(string id)
        {
            ApplicationUser usuarioEditar = ApplicationUser.GetUsuarioporId(_userManager, id);

            string rolUsuario = RESOURCE.SeleccionaRol;

            try
            {
                rolUsuario = _userManager.GetRolesAsync(usuarioEditar).Result.ToArray()[0];
            }
            catch (Exception e)
            { 
                //VALIDAR SI EXISTE OTRO TIPO DE ERROR SI LLEGA A SER NECESARIO
            }

            usuarioEditar.RolUsuario = RESOURCE.SeleccionaRol;

            List<string> lRoles = _context.Roles.Select(r => r.ToString()).ToList();

            ViewData["ListaRoles"] = lRoles;

            if (usuarioEditar.Id != null)
            {
                return PartialView(usuarioEditar);
            }

            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditarUsuario(string id, [Bind(
                "Direccion", "Email", "EmailConfirmed", "FechaCreacion", "FechaNacimiento",
                "Id", "IdEspecialidad", "IdEstadoUsuario", "IdHorarioTrabajo", "IdPaisNacimiento",
                "IdTipoIdentificacion", "Identificacion", "LockoutEnabled", "LockoutEnd", "NormalizedEmail",
                "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "PrimerApellido",
                "PrimerNombre", "RolUsuario", "SecurityStamp", "SegundoApellido", "SegundoNombre",
                "Sexo", "TwoFactorEnabled", "UserName", "ConcurrencyStamp")] ApplicationUser usuarioModelo)
        {   
            if (ModelState.IsValid)
            {
                try
                {
                    usuarioModelo.UserName = usuarioModelo.Email;

                    _context.Update(usuarioModelo);

                    //caso 1 no tiene rol
                    //caso 2 no cambia rol
                    //caso 3 cambia rol




                    //encontrar rol actual del usuario
                    string[] rolesActuales = _userManager.GetRolesAsync(usuarioModelo).Result.ToArray();

                    if (rolesActuales.Count() != 0)
                        for (int i = 0; i < rolesActuales.Count(); i++)
                        {
                            if (string.Compare(rolesActuales[i], usuarioModelo.RolUsuario) != 0)
                            {
                                //caso 3
                                //eliminar rol(es) actual(es) del usuario
                                await _userManager.RemoveFromRoleAsync(usuarioModelo, rolesActuales[i]);
                                await _userManager.AddToRoleAsync(usuarioModelo, usuarioModelo.RolUsuario);
                            }
                            //caso 2
                        }
                    else
                    {
                        //caso 1
                        //adicionar nuevo rol del usuario
                        await _userManager.AddToRoleAsync(usuarioModelo, usuarioModelo.RolUsuario);
                    }

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuarioExists(usuarioModelo.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(ListarUsuarios));
            }
            return View(usuarioModelo);
        }

        private bool UsuarioExists(string id)
        {
            return (_context.AUViewModel?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        public async Task<IActionResult> DetallesUsuario(string id)
        {
            AUViewModel usuarioVisualizar = AUViewModel.GetUsuarioporId(_userManager, id);
            return View(usuarioVisualizar);
        }

        public IActionResult ListarUsuarios()
        {
            List<AUViewModel> lUsuarios = AUViewModel.ListarUsuarios(_userManager, _context);
            return View(lUsuarios);
        }

        public IActionResult PantallaAdministracion()
        {
            if (RouteData.Values["restultadoCreaEspecialidad"] != null)
            {
                GlobalData.TRANSACTRESULTS resultado;

                if (Enum.TryParse(Convert.ToString(RouteData.Values["restultadoCreaEspecialidad"]), out resultado))
                    switch (resultado)
                    {
                        case GlobalData.TRANSACTRESULTS.FALLO:
                            break;

                        case GlobalData.TRANSACTRESULTS.NOUNICO:
                            break;
                    }
            }

            ViewData["ListaEspecialidades"] = Especialidad.ListaEspecialidades(false);            
            return View();
        }


        // GET: RegistroUsuario/Edit/5
        public IActionResult EditarEspecialidad(int? idEspecialidad)
        {   /*
            if (idEspecialidad == null || _context.RegistroUsuarioModelo == null)
            {
                return NotFound();
            }
            */
            Especialidad especialidadEncontrada = Especialidad.BuscarEspecialidad(idEspecialidad, null);

            if (especialidadEncontrada.IdEstadoEspecialidad != null)
            {
                return PartialView(especialidadEncontrada);
            }

            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditarEspecialidad([Bind("IdEspecialidad, NombreEspecialidad, IdEstadoEspecialidad, EstadoEspecialidad")] Especialidad EspecialidadModelo)
        {
            GlobalData.TRANSACTRESULTS resultado = new();

            if (EspecialidadModelo.IdEspecialidad != null)
            {
                if (ModelState.IsValid)
                {
                    if (EspecialidadModelo.IdEstadoEspecialidad != null)
                    {
                        bool resultadoEdicion = Especialidad.EditarEspecialidad(EspecialidadModelo);
                        if (resultadoEdicion)
                            ViewBag.resultadoCrearEspecialidad = GlobalData.TRANSACTRESULTS.EXITO.ToString();
                        else
                            ViewBag.resultadoCrearEspecialidad = GlobalData.TRANSACTRESULTS.FALLO.ToString();
                    }
                    else
                    {
                        ViewBag.resultadoCrearEspecialidad = GlobalData.TRANSACTRESULTS.NOUNICO.ToString();
                    }
                }

                if (Enum.TryParse(ViewBag.resultadoCrearEspecialidad, out resultado))
                    if (resultado == GlobalData.TRANSACTRESULTS.EXITO)
                    {
                        return RedirectToAction(nameof(PantallaAdministracion), new { restultadoCreaEspecialidad = resultado });
                    }
            }

            return RedirectToAction(nameof(PantallaAdministracion), new { restultadoCreaEspecialidad = resultado });

        }

        public IActionResult CrearEspecialidad()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CrearEspecialidad([Bind("IdEspecialidad, NombreEspecialidad, IdEstadoEspecialidad, EstadoEspecialidad")] Especialidad EspecialidadModelo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(EspecialidadModelo);
                //await _context.SaveChangesAsync();

                Especialidad especialidadEncontrada = Especialidad.BuscarEspecialidad(null, EspecialidadModelo.NombreEspecialidad);

                if ( especialidadEncontrada.IdEstadoEspecialidad == null)
                {
                    bool resultadoCreacion = Especialidad.CrearEspecialidad(EspecialidadModelo);
                    if (resultadoCreacion)
                        ViewBag.resultadoCrearEspecialidad = GlobalData.TRANSACTRESULTS.EXITO.ToString();
                    else
                        ViewBag.resultadoCrearEspecialidad = GlobalData.TRANSACTRESULTS.FALLO.ToString();
                }
                else 
                {
                    ViewBag.resultadoCrearEspecialidad = GlobalData.TRANSACTRESULTS.NOUNICO.ToString();                    
                }
            }

            GlobalData.TRANSACTRESULTS resultado;

            if (Enum.TryParse(ViewBag.resultadoCrearEspecialidad, out resultado))
                if (resultado == GlobalData.TRANSACTRESULTS.EXITO)
                {
                    return RedirectToAction(nameof(PantallaAdministracion), new { restultadoCreaEspecialidad = resultado });
                }
                /*else
                {
                    RedirectToAction(nameof(PantallaAdministracion), new { restultadoCreacion = resultado });
                    //return PartialView(EspecialidadModelo);
                }*/

            return View(EspecialidadModelo);
        }
    }
}
