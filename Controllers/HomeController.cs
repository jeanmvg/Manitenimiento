using System.Diagnostics;
using MantenimientoEquipos.Data;
using MantenimientoEquipos.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MantenimientoEquipos.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var totalEquipos = _context.Equipos.Count();
            var totalMantenimientos = _context.OrdenesTrabajo.Count();
            var ultimosMantenimientos = _context.OrdenesTrabajo
                .Include(o => o.Equipo)
                .OrderByDescending(o => o.FechaInicio)
                .Take(5)
                .ToList();

            ViewBag.TotalEquipos = totalEquipos;
            ViewBag.TotalMantenimientos = totalMantenimientos;
            ViewBag.UltimosMantenimientos = ultimosMantenimientos;

            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
