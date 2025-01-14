using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MantenimientoEquipos.Data;
using MantenimientoEquipos.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MantenimientoEquipos.Controllers
{
    public class OrdenTrabajoController : Controller
    {
        private readonly AppDbContext _context;

        public OrdenTrabajoController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var ordenes = _context.OrdenesTrabajo.Include(o => o.Equipo);
            return View(await ordenes.ToListAsync());
        }

        public IActionResult Create()
        {
            var nuevaOrden = new OrdenTrabajo
            {
                NumeroOrden = GenerarNumeroOrden(),
                FechaInicio = DateTime.Today
            };

            var equipos = _context.Equipos.Select(e => new { e.Id, e.Codigo }).ToList();

            if (equipos == null || equipos.Count == 0)
            {
                Console.WriteLine("❌ ERROR: No hay equipos en la base de datos.");
            }

            ViewBag.Equipos = equipos;
            return View(nuevaOrden);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NumeroOrden,Descripcion,Estado,FechaInicio,FechaFin,EquipoId,TipoMantenimiento")] OrdenTrabajo orden)
        {
            Console.WriteLine("🚀 DEPURACIÓN: Datos Recibidos en el Controlador");
            Console.WriteLine($"📌 NumeroOrden: {orden.NumeroOrden}");
            Console.WriteLine($"📌 EquipoId: {orden.EquipoId}");
            Console.WriteLine($"📌 TipoMantenimiento: {orden.TipoMantenimiento}");
            Console.WriteLine($"📌 FechaInicio: {orden.FechaInicio}");

            // 🔹 Verificar si el EquipoId realmente existe en la base de datos antes de guardarlo
            var equipoExistente = await _context.Equipos.FindAsync(orden.EquipoId);
            if (equipoExistente == null)
            {
                Console.WriteLine("❌ ERROR: El EquipoId no existe en la base de datos.");
                ModelState.AddModelError("EquipoId", "Debe seleccionar un equipo válido.");
                ViewBag.Equipos = _context.Equipos.ToList();
                return View(orden);
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Equipos = _context.Equipos.ToList();
                return View(orden);
            }

            orden.NumeroOrden = GenerarNumeroOrden();
            _context.Add(orden);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private string GenerarNumeroOrden()
        {
            int añoActual = DateTime.Now.Year;
            var ultimaOrden = _context.OrdenesTrabajo
                .Where(o => o.NumeroOrden.StartsWith($"OT-{añoActual}-"))
                .OrderByDescending(o => o.NumeroOrden)
                .FirstOrDefault();

            int siguienteNumero = ultimaOrden == null ? 1 : int.Parse(ultimaOrden.NumeroOrden.Split('-').Last()) + 1;
            return $"OT-{añoActual}-{siguienteNumero:D3}";
        }
    }
}
