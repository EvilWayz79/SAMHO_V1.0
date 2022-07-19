using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SAMHO.Data;
using SAMHO.Models;

namespace SAMHO.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
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
