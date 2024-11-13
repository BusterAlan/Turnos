using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Turnos.Models;

namespace Turnos.Controllers
{
    public class EspecialidadController : Controller
    {

        private readonly TurnosContext _context;

        public EspecialidadController(TurnosContext context)
        {

            _context = context;

        }

        public async Task<IActionResult> Index()
        {
            if (_context.Especialidad == null)
            {

                return NotFound();

            }

            return View(await _context.Especialidad.ToListAsync());
            
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || _context.Especialidad == null)
            {

                return NotFound();

            }

            var especialidad = _context.Especialidad.Find(id);

            if (especialidad == null)
            {

                return NotFound();

            }

            return View(especialidad);

        }

        [HttpPost]
        public IActionResult Edit(
            
            int id, 
            [Bind("IdEspecialidad, Descripcion")] Especialidad especialidad
            
        ) {

            if (id != especialidad.IdEspecialidad)
            {

                return NotFound();

            }

            if (ModelState.IsValid)
            {

                try
                {

                    _context.Update(especialidad);
                    _context.SaveChanges();

                }
                catch (DbUpdateConcurrencyException)
                {

                    if (_context.Especialidad == null) {

                        return NotFound();

                    }

                    if (!_context.Especialidad.Any(e => e.IdEspecialidad == especialidad.IdEspecialidad))
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

            return View(especialidad);

        }

         public async Task<IActionResult> Delete(int? id) {

            if (id == null) {

                return NotFound();

            }

            var especialidad = await _context.Especialidad.FirstOrDefaultAsync(e => e.IdEspecialidad == id);

            if (especialidad == null) {

                return NotFound();

            }

            return View(especialidad);

        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {

            var especialidad = await _context.Especialidad.FindAsync(id);
            _context.Especialidad.Remove(especialidad);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
            
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("IdEspecialidad, Descripcion")] Especialidad especialidad)
        {
            if(ModelState.IsValid)
            {
                _context.Add(especialidad);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(especialidad);
        }

    }

}

