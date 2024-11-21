using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Turnos.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace turnos.Controllers {

    public class MedicoController: Controller
    {

        private readonly TurnosContext _context;

        public MedicoController(TurnosContext context)
        {

            _context = context;

        }

        public async Task<IActionResult> Index()
        {

            if (_context.Medico == null)
            {

                return NotFound();

            }

            return View(await _context.Medico.ToListAsync());

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdMedico, Nombre, Apellido, Direccion, Telefono, Email, HorarioAtencionDesde, HorarioAtencionHasta")] Medico medico)
        {

            if (id != medico.IdMedico)
            {

                return NotFound();

            }

            if (ModelState.IsValid)
            {

                _context.Update(medico);
                await _context.SaveChangesAsync();
                RedirectToAction(nameof(Index));

            }

            return View(medico);

        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {

            if (id == null || _context.Medico == null) {

                return NotFound();

            }            

            var medico = await _context.Medico.FirstOrDefaultAsync(p => p.IdMedico == id);

            if (medico == null)
            {

                return NotFound();

            }

            _context.Medico.Remove(medico);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

        // GET: Medico/Details/5
        public async Task<IActionResult> Details(int? id)
        {

            if (_context.Medico == null || id == null)
            {
                return NotFound();
            }

            var medico = await _context.Medico
                .Where(m => m.IdMedico == id).Include(me => me.MedicoEspecialidad!)
                .ThenInclude(e => e.Especialidad!).FirstOrDefaultAsync();
                
            if (medico == null)
            {
                return NotFound();
            }

            return View(medico);
        }

        // GET: Medico/Create
        public IActionResult Create()
        {
            ViewData["ListaEspecialidades"] = new SelectList(_context.Especialidad,"IdEspecialidad","Descripcion");
            return View();
        }

        // POST: Medico/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdMedico,Nombre,Apellido,Direccion,Telefono,Email,HorarioAtencionDesde,HorarioAtencionHasta")] Medico medico, int IdEspecialidad)
        {
            ViewData["ListaEspecialidades"] = new SelectList(_context.Especialidad,"IdEspecialidad","Descripcion", IdEspecialidad);
            if (ModelState.IsValid)
            {
                _context.Add(medico);
                await _context.SaveChangesAsync();

                var medicoEspecialidad = new MedicoEspecialidad
                {
                    IdMedico = medico.IdMedico,
                    IdEspecialidad = IdEspecialidad,
                    Medico = medico,
                    Especialidad = await _context.Especialidad.FindAsync(IdEspecialidad)
                };
                
                _context.Add(medicoEspecialidad);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(medico);
        }

        // GET: Medico/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medico = await _context.Medico.Where(m => m.IdMedico == id)
            .Include(me => me.MedicoEspecialidad!).FirstOrDefaultAsync();

            if (medico == null)
            {
                return NotFound();
            }

            SelectList especialidades = new SelectList(
                _context.Especialidad, "IdEspecialidad", "Descripcion", medico.MedicoEspecialidad.Count > 0 ? medico.MedicoEspecialidad[0].IdEspecialidad: 0);            
            ViewData["ListaEspecialidades"] = especialidades;

            return View(medico);
        }

        // POST: Medico/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdMedico,Nombre,Apellido,Direccion,Telefono,Email,HorarioAtencionDesde,HorarioAtencionHasta")] Medico medico, int IdEspecialidad)
        {
            ViewData["ListaEspecialidades"] = new SelectList(_context.Especialidad,"IdEspecialidad","Descripcion", IdEspecialidad);

            if (id != medico.IdMedico)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(medico);
                    await _context.SaveChangesAsync();

                    var medicoEspecialidad = await _context.MedicoEspecialidad
                    .FirstOrDefaultAsync(me => me.IdMedico == id);

                    _context.Remove(medicoEspecialidad);
                    await _context.SaveChangesAsync();

                    medicoEspecialidad.IdEspecialidad = IdEspecialidad;

                    _context.Add(medicoEspecialidad);
                    await _context.SaveChangesAsync();

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MedicoExists(medico.IdMedico))
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
            return View(medico);
        }

        // GET: Medico/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medico = await _context.Medico
                .FirstOrDefaultAsync(m => m.IdMedico == id);
            if (medico == null)
            {
                return NotFound();
            }

            return View(medico);
        }

        // POST: Medico/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var medicoEspecialidad = await _context.MedicoEspecialidad
            .FirstOrDefaultAsync(me => me.IdMedico == id);

            _context.MedicoEspecialidad.Remove(medicoEspecialidad);
            await _context.SaveChangesAsync();

            var medico = await _context.Medico.FindAsync(id);
            
            _context.Medico.Remove(medico);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool MedicoExists(int id)
        {
            return _context.Medico.Any(e => e.IdMedico == id);
        }

        public string TraerHorarioAtencionDesde (int idMedico)
        {
            var HorarioAtencionDesde = _context.Medico.Where(m => m.IdMedico == idMedico).FirstOrDefault().HorarioAtencionDesde;
            return HorarioAtencionDesde.Hour + ":" + HorarioAtencionDesde.Minute;
        }

        public string TraerHorarioAtencionHasta (int idMedico)
        {
            var HorarioAtencionHasta = _context.Medico.Where(m => m.IdMedico == idMedico).FirstOrDefault().HorarioAtencionHasta;
            return HorarioAtencionHasta.Hour + ":" + HorarioAtencionHasta.Minute;
        }

    }

}

