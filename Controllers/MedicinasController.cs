using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SAMHO.Data;
using SAMHO.Models;

namespace SAMHO.Controllers
{
    public class MedicinasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MedicinasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Medicinas
        public async Task<IActionResult> ListarMedicinas()
        {
              return _context.Medicina != null ? 
                          View(await _context.Medicina.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Medicina'  is null.");
        }

        // GET: Medicinas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Medicina == null)
            {
                return NotFound();
            }

            var medicina = await _context.Medicina
                .FirstOrDefaultAsync(m => m.IdMedicina == id);
            if (medicina == null)
            {
                return NotFound();
            }

            return View(medicina);
        }

        // GET: Medicinas/Create
        public IActionResult CrearMedicina()
        {
            return View();
        }

        // POST: Medicinas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CrearMedicina([Bind("IdMedicina,PrecioMedicina,IdEstadoMedicina,CantidadMedicina,FechaCaducidad,FechaRegistro")] Medicina medicina)
        {
            if (ModelState.IsValid)
            {
                _context.Add(medicina);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(medicina);
        }

        // GET: Medicinas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Medicina == null)
            {
                return NotFound();
            }

            var medicina = await _context.Medicina.FindAsync(id);
            if (medicina == null)
            {
                return NotFound();
            }
            return View(medicina);
        }

        // POST: Medicinas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdMedicina,PrecioMedicina,IdEstadoMedicina,CantidadMedicina,FechaCaducidad,FechaRegistro")] Medicina medicina)
        {
            if (id != medicina.IdMedicina)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(medicina);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MedicinaExists(medicina.IdMedicina))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(medicina);
        }

        // GET: Medicinas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Medicina == null)
            {
                return NotFound();
            }

            var medicina = await _context.Medicina
                .FirstOrDefaultAsync(m => m.IdMedicina == id);
            if (medicina == null)
            {
                return NotFound();
            }

            return View(medicina);
        }

        // POST: Medicinas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Medicina == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Medicina'  is null.");
            }
            var medicina = await _context.Medicina.FindAsync(id);
            if (medicina != null)
            {
                _context.Medicina.Remove(medicina);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MedicinaExists(int id)
        {
          return (_context.Medicina?.Any(e => e.IdMedicina == id)).GetValueOrDefault();
        }
    }
}
