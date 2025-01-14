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

            ViewBag.Equipos = _context.Equipos
                .Select(e => new { e.Id, e.Nombre })
                .ToList();
            return View(nuevaOrden);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NumeroOrden,Descripcion,Estado,FechaInicio,FechaFin,EquipoId,TipoMantenimiento")] OrdenTrabajo orden)
        {
            // Debug: Imprimir valores en consola
            Console.WriteLine("🚀 Recibido en el controlador:");
            Console.WriteLine($"NumeroOrden: {orden.NumeroOrden}");
            Console.WriteLine($"EquipoId: {orden.EquipoId}");
            Console.WriteLine($"TipoMantenimiento: {orden.TipoMantenimiento}");
            Console.WriteLine($"FechaInicio: {orden.FechaInicio}");
            
            if (orden.EquipoId == 0)
            {
                Console.WriteLine("❌ ERROR: EquipoId está llegando como 0 o NULL");
                ModelState.AddModelError("EquipoId", "Debe seleccionar un equipo válido.");
            }

            // Validaciones manuales para depuración
            if (string.IsNullOrEmpty(orden.TipoMantenimiento))
            {
                ModelState.AddModelError("TipoMantenimiento", "El tipo de mantenimiento es obligatorio.");
            }

            if (!_context.Equipos.Any(e => e.Id == orden.EquipoId))
            {
                ModelState.AddModelError("EquipoId", "Debe seleccionar un equipo válido.");
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
