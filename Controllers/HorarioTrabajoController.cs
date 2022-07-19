using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SAMHO.Data;
using SAMHO.Models;

namespace SAMHO.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class HorarioTrabajoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HorarioTrabajoController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: HorarioTrabajo
        public async Task<IActionResult> ListarHorario()
        {
              return _context.HorarioTrabajo != null ? 
                          View(await _context.HorarioTrabajo.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.HorarioTrabajo'  is null.");
        }

        // GET: HorarioTrabajo/Details/5
        public async Task<IActionResult> BuscarHorario(int? id)
        {
            if (id == null || _context.HorarioTrabajo == null)
            {
                return NotFound();
            }

            var horarioTrabajo = await _context.HorarioTrabajo
                .FirstOrDefaultAsync(m => m.IdHorarioTrabajo == id);
            if (horarioTrabajo == null)
            {
                return NotFound();
            }

            return View(horarioTrabajo);
        }

        // GET: HorarioTrabajo/Create
        public IActionResult CrearHorario()
        {
            return View();
        }

        // POST: HorarioTrabajo/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CrearHorario([Bind("IdHorarioTrabajo,DiasSemana,HInicio,MInicio,HFin,MFin,IdEstado,NombreHorario")] HorarioTrabajo horarioTrabajo)
        {   
            if (horarioTrabajo.DiasSemana != null)
            {
                string diasSemana = horarioTrabajo.DiasSemana.Replace("true", "").Replace("false", "").Replace(",", " ");

                if (horarioTrabajo.DiasSemana.Equals(""))
                {
                    string diasError = "Seleccionar dias";
                    ModelState.AddModelError("DiasSemana", diasError);
                }
            }

            if (horarioTrabajo.HInicio > horarioTrabajo.HFin || horarioTrabajo.HInicio == horarioTrabajo.HFin)
            {
                string errorHoraIvsF = "Error en Horas";
                ModelState.AddModelError("HFin", errorHoraIvsF);
            }

            if (ModelState.IsValid)
            {
                
                _context.Add(horarioTrabajo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ListarHorario));
            }
            return View(horarioTrabajo);
        }

        // GET: HorarioTrabajo/Edit/5
        public async Task<IActionResult> EditarHorario(int? id)
        {
            if (id == null || _context.HorarioTrabajo == null)
            {
                return NotFound();
            }

            var horarioTrabajo = await _context.HorarioTrabajo.FindAsync(id);

            if (horarioTrabajo == null)
            {
                return NotFound();
            }
            return View(horarioTrabajo);
        }

        // POST: HorarioTrabajo/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditarHorario(int id, [Bind("IdHorarioTrabajo,DiasSemana,HInicio,MInicio,HFin,MFin,IdEstado,NombreHorario")] HorarioTrabajo horarioTrabajo)
        {
            if (id != horarioTrabajo.IdHorarioTrabajo)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(horarioTrabajo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HorarioTrabajoExists(horarioTrabajo.IdHorarioTrabajo))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(ListarHorario));
            }
            return View(horarioTrabajo);
        }

        // GET: HorarioTrabajo/Delete/5
        public async Task<IActionResult> EliminarHorario(int? id)
        {
            if (id == null || _context.HorarioTrabajo == null)
            {
                return NotFound();
            }

            var horarioTrabajo = await _context.HorarioTrabajo
                .FirstOrDefaultAsync(m => m.IdHorarioTrabajo == id);
            if (horarioTrabajo == null)
            {
                return NotFound();
            }

            return View(horarioTrabajo);
        }

        // POST: HorarioTrabajo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarConfirmado(int id)
        {
            if (_context.HorarioTrabajo == null)
            {
                return Problem("Entity set 'ApplicationDbContext.HorarioTrabajo'  is null.");
            }
            var horarioTrabajo = await _context.HorarioTrabajo.FindAsync(id);
            if (horarioTrabajo != null)
            {
                _context.HorarioTrabajo.Remove(horarioTrabajo);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HorarioTrabajoExists(int id)
        {
          return (_context.HorarioTrabajo?.Any(e => e.IdHorarioTrabajo == id)).GetValueOrDefault();
        }
    }
}
