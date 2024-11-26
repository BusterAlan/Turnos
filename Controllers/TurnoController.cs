using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Turnos.Models;

namespace Turnos.Controllers
{
    public class TurnoController : Controller
    {
        private readonly TurnosContext _context;
        private IConfiguration _configuration;

        public TurnoController(TurnosContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            if (_context.Medico == null || _context.Paciente == null)
            {
                return NotFound();
            }

            Console.WriteLine("Medico: " + _context.Medico.ToList().Count);
            ViewData["IdMedico"] = new SelectList(from medico in _context.Medico.ToList() select new { IdMedico = medico.IdMedico, NombreCompleto = medico.Nombre + " " + medico.Apellido},"IdMedico","NombreCompleto");
            ViewData["IdPaciente"] = new SelectList(from paciente in _context.Paciente.ToList() select new { IdPaciente = paciente.IdPaciente, NombreCompleto = paciente.Nombre + " " + paciente.Apellido},"IdPaciente","NombreCompleto");
            return View();
        }

        public JsonResult ObtenerTurnos(int idMedico)
        {
            Console.WriteLine("idMedico ObtenerTurnos: " + idMedico);
            if (_context.Turno == null)
            {
                return Json(null);
            }

            var turnos = _context.Turno.Where(t => t.IdMedico == idMedico)
            .Select(t => new {
                t.IdTurno,
                t.IdMedico,
                t.IdPaciente,
                t.FechaHoraInicio,
                t.FechaHoraFin,
                paciente = t.Paciente.Nombre + ", " + t.Paciente.Apellido
            })
            .ToList();

            return Json(turnos);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult GrabarTurno(Turno turno)
        {
            Console.WriteLine("GrabarTurno: " + turno.IdTurno);
            var ok = false;
            try
            {
                if (_context.Turno == null)
                {
                    return Json(null);
                }

               _context.Turno.Add(turno);
               _context.SaveChanges();
               ok = true; 
            }
            catch (Exception e)
            {
                Console.WriteLine("{0} Excepcion encontrada", e);                
            }
            var jsonResult = new { ok = ok };
            return Json(jsonResult);
        }

        [HttpPost]
        public JsonResult EliminarTurno(int idTurno)
        {
            Console.WriteLine("idTurno EliminarTurno: " + idTurno);
            var ok = false;
            try
            {
                if (_context.Turno == null)
                {
                    return Json(null);
                }

                var turnoAEliminar = _context.Turno.Where(t => t.IdTurno == idTurno).FirstOrDefault();
                if (turnoAEliminar != null)
                {
                    _context.Turno.Remove(turnoAEliminar);
                    _context.SaveChanges();
                    ok = true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("{0} Excepcion encontrada", e);
            }

            var jsonResult = new {ok = ok};
            return Json(jsonResult);
        }

        // GET: Turno/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Turno/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Paciente,Medico,Fecha,Hora")] Turno turno)
        {

            if (ModelState.IsValid)
            {
                _context.Add(turno);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(turno);
            
        }
    }
}