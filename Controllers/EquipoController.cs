using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using MantenimientoEquipos.Data;
using MantenimientoEquipos.Models;

namespace MantenimientoEquipos.Controllers
{
    public class EquipoController : Controller
    {
        private readonly AppDbContext _context;

        public EquipoController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _context.Equipos.ToListAsync());
        }
                // Mostrar el formulario de creación
        public IActionResult Create()
        {
            return View();
        }
                // Guardar los datos en la base de datos
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Codigo,Nombre,Tipo,Ubicacion,Estado,Fabricante,Modelo,NumeroSerie,Capacidad,FrecuenciaMantenimiento,CostoAproximado,VidaUtil,FechaCompra")] Equipo equipo)
        {
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine($"Error en validación: {error.ErrorMessage}");
                }
                return View(equipo);
            }

            _context.Add(equipo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        // Detalles, Editar, Eliminar
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var equipo = await _context.Equipos
                .FirstOrDefaultAsync(m => m.Id == id);

            if (equipo == null)
            {
                return NotFound();
            }

            return View(equipo);
        }
        // Editar
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var equipo = await _context.Equipos.FindAsync(id);
            if (equipo == null)
            {
                return NotFound();
            }

            return View(equipo);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Codigo,Nombre,Tipo,Ubicacion,Estado,Fabricante,Modelo,NumeroSerie,Capacidad,FrecuenciaMantenimiento,FechaCompra")] Equipo equipo)
        {
            if (id != equipo.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                Console.WriteLine("❌ Error en ModelState. No se guardaron los cambios.");
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine($"- {error.ErrorMessage}");
                }
                return View(equipo);
            }

            try
            {
                _context.Update(equipo);
                await _context.SaveChangesAsync();
                Console.WriteLine("✅ Equipo actualizado correctamente.");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Equipos.Any(e => e.Id == equipo.Id))
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



        // Eliminar
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var equipo = await _context.Equipos
                .FirstOrDefaultAsync(m => m.Id == id);

            if (equipo == null)
            {
                return NotFound();
            }

            return View(equipo); // 🔹 Retorna la vista Delete.cshtml con el equipo cargado
        }
        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var equipo = await _context.Equipos.FindAsync(id);
            if (equipo == null)
            {
                return NotFound();
            }

            _context.Equipos.Remove(equipo);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index)); // Redirigir a la lista después de eliminar
        }


        // Importar desde Excel
        public IActionResult Importar()
        {
            return View();
        }

        [HttpPost]
        
        public async Task<IActionResult> ImportarDesdeExcel(IFormFile archivo)
        {
            if (archivo == null || archivo.Length == 0)
            {
                ViewBag.Message = "❌ Por favor, selecciona un archivo válido.";
                return View("Importar");
            }

            using (var stream = new MemoryStream())
            {
                archivo.CopyTo(stream);
                using (var package = new ExcelPackage(stream))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0]; // Primera hoja del Excel
                    if (worksheet == null)
                    {
                        ViewBag.Message = "❌ No se encontró una hoja válida en el archivo.";
                        return View("Importar");
                    }

                    int rowCount = worksheet.Dimension.Rows;
                    List<Equipo> nuevosEquipos = new List<Equipo>();
                    List<string> codigosDuplicados = new List<string>();

                    for (int row = 2; row <= rowCount; row++) // Empezamos en la fila 2 (Saltamos encabezado)
                    {
                        string codigo = worksheet.Cells[row, 1].Text.Trim();
                        string nombre = worksheet.Cells[row, 2].Text.Trim();
                        string ubicacion = worksheet.Cells[row, 3].Text.Trim();
                        string estado = worksheet.Cells[row, 4].Text.Trim();
                        string frecuenciaMantenimiento = worksheet.Cells[row, 5].Text.Trim();
                        string fechaCompraTexto = worksheet.Cells[row, 6].Text.Trim();

                        DateTime fechaCompra = DateTime.Now; // Valor por defecto si la fecha está vacía
                        if (DateTime.TryParse(fechaCompraTexto, out DateTime fechaValida))
                        {
                            fechaCompra = fechaValida;
                        }

                        // 📌 Verificar si el código ya existe en la base de datos
                        bool existeEquipo = _context.Equipos.Any(e => e.Codigo == codigo);
                        if (!existeEquipo)
                        {
                            var equipo = new Equipo
                            {
                                Codigo = codigo,
                                Nombre = nombre,
                                Ubicacion = ubicacion,
                                Estado = estado,
                                FrecuenciaMantenimiento = frecuenciaMantenimiento,
                                FechaCompra = fechaCompra
                            };
                            nuevosEquipos.Add(equipo);
                        }
                        else
                        {
                            codigosDuplicados.Add(codigo); // Guardar los códigos duplicados
                        }
                    }

                    if (nuevosEquipos.Count > 0)
                    {
                        _context.Equipos.AddRange(nuevosEquipos);
                        await _context.SaveChangesAsync();
                    }

                    if (codigosDuplicados.Count > 0)
                    {
                        ViewBag.Message = $"⚠️ Se ignoraron {codigosDuplicados.Count} equipos porque ya existen en la base de datos: {string.Join(", ", codigosDuplicados)}";
                    }
                    else
                    {
                        ViewBag.Message = "✅ Equipos importados exitosamente.";
                    }
                }
            }

            return View("Importar");
        }
    }
}

