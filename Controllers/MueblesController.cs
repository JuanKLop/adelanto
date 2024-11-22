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
    public class MueblesController : Controller
    {
        private readonly GlampingAddventuresContext _context;

        public MueblesController(GlampingAddventuresContext context)
        {
            _context = context;
        }

        // GET: Muebles
        public async Task<IActionResult> Index()
        {
            var muebles = await _context.Muebles
            .ToListAsync();

            // Convertir imágenes a Base64
            foreach (var mueble in muebles)
            {
                if (mueble.ImagenMueble != null)
                {
                    string imageBase64Data = Convert.ToBase64String(mueble.ImagenMueble);
                    string imageDataURL = string.Format("data:image/png;base64,{0}", imageBase64Data);
                    mueble.ImagenDataURL = imageDataURL;
                }
            }

            return View(muebles);
        }

        // GET: Muebles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mueble = await _context.Muebles
                .FirstOrDefaultAsync(m => m.Idmueble == id);

            if (mueble == null)
            {
                return NotFound();
            }

            // Convertir imagen a Base64
            if (mueble.ImagenMueble != null)
            {
                string imageBase64Data = Convert.ToBase64String(mueble.ImagenMueble);
                string imageDataURL = string.Format("data:image/png;base64,{0}", imageBase64Data);

                // Almacenar la URL de la imagen en ViewData para usarla en la vista
                ViewData["ImageDataURL"] = imageDataURL;
            }

            return View(mueble);
        }

        // GET: Muebles/Create
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Idmueble,NombreMueble,ImagenMueble,CantidadDisponible,Valor,Estado")] Mueble mueble, IFormFile ImageFile)
        {
            if (mueble.Valor == null)
            {
                mueble.Valor = 0; // O toma un valor predeterminado si es necesario.
            }

            if (!ModelState.IsValid)
            {
                // Imprimir errores de validación
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine("Error de validación: " + error.ErrorMessage);
                }

                // Volver a cargar habitaciones activas
                var habitacionesActivas = _context.Habitacions
                                          .Where(h => h.Estado == true)
                                          .ToList();
                return View(mueble);
            }

            try
            {
                // Manejo de la imagen
                if (ImageFile != null && ImageFile.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await ImageFile.CopyToAsync(memoryStream);
                        mueble.ImagenMueble = memoryStream.ToArray();
                    }
                }

                // Verificar la cantidad disponible y actualizar el estado
                if (mueble.CantidadDisponible <= 0)
                {
                    // Si Estado es nullable, lo tratamos como false cuando la cantidad es 0
                    mueble.Estado = false;
                }

                // Agregar el nuevo mueble
                _context.Add(mueble);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Captura de cualquier excepción
                ModelState.AddModelError("", "Ocurrió un error al guardar el mueble. Detalles: " + ex.Message);
            }

            // Recargar habitaciones activas para la vista en caso de error
            var habitacionesActivasError = _context.Habitacions
                                          .Where(h => h.Estado == true)
                                          .ToList();
            return View(mueble);
        }

        // POST: Muebles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mueble = await _context.Muebles.FindAsync(id);
            if (mueble == null)
            {
                return NotFound();
            }

            return View(mueble);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Idmueble,NombreMueble,ImagenMueble,CantidadDisponible,Valor,Idhabitacion,Estado")] Mueble mueble, IFormFile ImageFile)
        {
            if (id != mueble.Idmueble)
            {
                return NotFound();
            }

            if (mueble.Valor < 0)
            {
                ModelState.AddModelError("Valor", "El valor no puede ser negativo.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Manejar la imagen
                    if (ImageFile != null && ImageFile.Length > 0)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            await ImageFile.CopyToAsync(memoryStream);
                            mueble.ImagenMueble = memoryStream.ToArray();
                        }
                    }
                    else
                    {
                        // Mantener la imagen existente si no se carga una nueva
                        var muebleExistente = await _context.Muebles.AsNoTracking().FirstOrDefaultAsync(h => h.Idmueble == id);
                        if (muebleExistente != null)
                        {
                            mueble.ImagenMueble = muebleExistente.ImagenMueble;
                        }
                    }

                    // Verificar la cantidad disponible y actualizar el estado
                    if (mueble.CantidadDisponible <= 0)
                    {
                        mueble.Estado = false; // Estado cambia automáticamente a false si la cantidad es 0
                    }

                    // Actualizar el mueble
                    _context.Update(mueble);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MuebleExists(mueble.Idmueble))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Ocurrió un error al guardar los cambios. Detalles: " + ex.Message);
                }
            }

            return View(mueble);
        }

        private bool MuebleExists(int id)
        {
            return _context.Muebles.Any(e => e.Idmueble == id);
        }


        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mueble = await _context.Muebles
                .FirstOrDefaultAsync(m => m.Idmueble == id);

            if (mueble == null)
            {
                return NotFound();
            }

            if (mueble.ImagenMueble != null)
            {
                ViewData["ImageDataURL"] = "data:image/png;base64," + Convert.ToBase64String(mueble.ImagenMueble);
            }

            return View(mueble);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var mueble = await _context.Muebles.FindAsync(id);
            if (mueble == null)
            {
                return NotFound();
            }

            try
            {
                _context.Muebles.Remove(mueble);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index)); // Asegúrate de que Index exista y sea la acción correcta
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Ocurrió un error al intentar eliminar el mueble: " + ex.Message);
                return View("Delete", mueble); // Muestra la vista Delete si ocurre un error
            }
        }
        [HttpPost]
        public IActionResult CambiarEstado(int id)
        {
            var mueble = _context.Muebles.Find(id);
            if (mueble != null)
            {
                mueble.Estado = !mueble.Estado;
                _context.Update(mueble);
                _context.SaveChanges();
                return Json(new { success = true, estado = mueble.Estado });
            }
            return Json(new { success = false });
        }
    }
}
