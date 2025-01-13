using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MantenimientoEquipos.Models
{
    public class OrdenTrabajo
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(15)]
        public string NumeroOrden { get; set; } = string.Empty;

        public string Descripcion { get; set; }

        [Required(ErrorMessage = "El Estado es obligatorio.")]
        public string Estado { get; set; } // Pendiente, En Proceso, Completado

        [Required(ErrorMessage = "La Fecha de Inicio es obligatoria.")]
        [DataType(DataType.Date)]
        public DateTime FechaInicio { get; set; }

        [DataType(DataType.Date)]
        public DateTime? FechaFin { get; set; } // Opcional

        // Relación con Equipo
        [Required(ErrorMessage = "El Equipo es obligatorio.")]
        public virtual Equipo Equipo { get; set; } // Relación con la tabla Equipos
        [Required(ErrorMessage = "El Tipo de Mantenimiento es obligatorio.")]
        public string TipoMantenimiento { get; set; } // 🔹 Nueva propiedad
    }
}
