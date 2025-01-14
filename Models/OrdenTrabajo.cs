using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MantenimientoEquipos.Models
{
    public class OrdenTrabajo
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Debe seleccionar un equipo.")]
        public int EquipoId { get; set; }

        [ForeignKey("EquipoId")]
        public virtual Equipo Equipo { get; set; }

        [Required]
        [StringLength(15)]
        public string NumeroOrden { get; set; } = string.Empty; // Ejemplo: OT-2025-001

        [Required(ErrorMessage = "La descripción es obligatoria.")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "El tipo de mantenimiento es obligatorio.")]
        public string TipoMantenimiento { get; set; }

        [Required(ErrorMessage = "El estado es obligatorio.")]
        public string Estado { get; set; } // Pendiente, En Proceso, Completado

        [Required(ErrorMessage = "Debe ingresar una fecha de inicio válida.")]
        [DataType(DataType.Date)]
        public DateTime FechaInicio { get; set; } = DateTime.Today;

        [DataType(DataType.Date)]
        public DateTime? FechaFin { get; set; } // Opcional

        
    }
}
