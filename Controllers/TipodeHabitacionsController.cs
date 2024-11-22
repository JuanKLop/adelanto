using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Glamping_Addventures.Models;

namespace Glamping_Addventures.Controllers
{
    public class TipodeHabitacionsController : Controller
    {
        private readonly GlampingAddventuresContext _context;

        public TipodeHabitacionsController(GlampingAddventuresContext context)
        {
            _context = context;
        }

        // GET: TipodeHabitacions
        public async Task<IActionResult> Index()
        {
            return View(await _context.TipodeHabitacions.ToListAsync());
        }

        // GET: TipodeHabitacions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipodeHabitacion = await _context.TipodeHabitacions
                .FirstOrDefaultAsync(m => m.IdtipodeHabitacion == id);
            if (tipodeHabitacion == null)
            {
                return NotFound();
            }

            return View(tipodeHabitacion);
        }

        // GET: TipodeHabitacions/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TipodeHabitacions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdtipodeHabitacion,NombreTipodeHabitacion,Descripcion,Estado")] TipodeHabitacion tipodeHabitacion)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tipodeHabitacion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tipodeHabitacion);
        }

        // GET: TipodeHabitacions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipodeHabitacion = await _context.TipodeHabitacions.FindAsync(id);
            if (tipodeHabitacion == null)
            {
                return NotFound();
            }
            return View(tipodeHabitacion);
        }

        // POST: TipodeHabitacions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdtipodeHabitacion,NombreTipodeHabitacion,Descripcion,Estado")] TipodeHabitacion tipodeHabitacion)
        {
            if (id != tipodeHabitacion.IdtipodeHabitacion)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tipodeHabitacion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TipodeHabitacionExists(tipodeHabitacion.IdtipodeHabitacion))
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
            return View(tipodeHabitacion);
        }

        // GET: TipodeHabitacions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoDeHabitacion = await _context.TipodeHabitacions
                .Include(t => t.Habitacions) // Incluimos las habitaciones asociadas
                .FirstOrDefaultAsync(m => m.IdtipodeHabitacion == id);
            if (tipoDeHabitacion == null)
            {
                return NotFound();
            }

            return View(tipoDeHabitacion);
        }

        // POST: TipodeHabitacions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tipoDeHabitacion = await _context.TipodeHabitacions
                .Include(t => t.Habitacions)
                .FirstOrDefaultAsync(m => m.IdtipodeHabitacion == id);

            if (tipoDeHabitacion == null)
            {
                return NotFound();
            }

            // Verifica si hay habitaciones asociadas antes de eliminar
            if (tipoDeHabitacion.Habitacions.Any())
            {
                TempData["ErrorMessage"] = "No se puede eliminar este tipo de habitación porque está asociado con una o más habitaciones.";
                return RedirectToAction(nameof(Index));
            }

            _context.TipodeHabitacions.Remove(tipoDeHabitacion);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool TipodeHabitacionExists(int id)
        {
            return _context.TipodeHabitacions.Any(e => e.IdtipodeHabitacion == id);
        }

        [HttpPost]
        public IActionResult CambiarEstado(int id)
        {
            var tipodehabitacion = _context.TipodeHabitacions.Find(id);
            if (tipodehabitacion != null)
            {
                tipodehabitacion.Estado = !tipodehabitacion.Estado;
                _context.Update(tipodehabitacion);
                _context.SaveChanges();
                return Json(new { success = true, estado = tipodehabitacion.Estado });
            }
            return Json(new { success = false });
        }
    }
}
