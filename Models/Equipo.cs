using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MantenimientoEquipos.Models
{
    public class Equipo
    {
        public int Id { get; set; }

        [Required]
        [StringLength(8)]
        public string Codigo { get; set; }

        [Required]
        public string Nombre { get; set; } // Nombre del equipo
        public string? Tipo { get; set; } // Neumática, Hidráulica, etc.
        [Required]
        public string Ubicacion { get; set; } // Ubicación física
        public string Estado { get; set; } // Activo, Inactivo, En Reparación
        public string? Fabricante { get; set; }
        public string? Modelo { get; set; }
        public DateTime FechaCompra { get; set; }
        public string? NumeroSerie { get; set; } // Nuevo campo
        public string? Capacidad { get; set; } // Nuevo campo
        public string FrecuenciaMantenimiento { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal? CostoAproximado { get; set; }
        public int? VidaUtil { get; set; }
        
        // Relación con Ordenes de Trabajo
        public ICollection<OrdenTrabajo>? OrdenesTrabajo { get; set; }
    }
}
