using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MantenimientoEquipos.Data;
using MantenimientoEquipos.Models;
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
                NumeroOrden = GenerarNumeroOrden() // 🔹 Generamos el número de orden antes de mostrar la vista
            };

            ViewBag.Equipos = _context.Equipos.ToList();
            return View(nuevaOrden);
        }
        private string GenerarNumeroOrden()
        {
            int añoActual = DateTime.Now.Year;

            // Obtener la última orden registrada en el año actual
            var ultimaOrden = _context.OrdenesTrabajo
                .Where(o => o.NumeroOrden.StartsWith($"OT-{añoActual}-"))
                .OrderByDescending(o => o.NumeroOrden)
                .FirstOrDefault();

            int siguienteNumero = 1;
            if (ultimaOrden != null)
            {
                string ultimoNumeroStr = ultimaOrden.NumeroOrden.Split('-').Last();
                if (int.TryParse(ultimoNumeroStr, out int ultimoNumero))
                {
                    siguienteNumero = ultimoNumero + 1;
                }
            }

            return $"OT-{añoActual}-{siguienteNumero:D3}";
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Descripcion,Estado,FechaInicio,FechaFin,Equipo,NumeroOrden")] OrdenTrabajo orden, string CodigoHidden)
        {
            if (string.IsNullOrEmpty(CodigoHidden))
            {
                ModelState.AddModelError("Equipo", "Debe seleccionar un equipo válido.");
            }

            // Buscar el equipo por el código ingresado
            var equipoSeleccionado = _context.Equipos.FirstOrDefault(e => e.Codigo == CodigoHidden);

            if (equipoSeleccionado == null)
            {
                ModelState.AddModelError("Equipo", "El equipo seleccionado no existe.");
            }
            else
            {
                orden.Equipo = equipoSeleccionado; // Asignar el equipo seleccionado
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
    }
}
