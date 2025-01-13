using MantenimientoEquipos.Models;
using Microsoft.EntityFrameworkCore;

namespace MantenimientoEquipos.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Equipo> Equipos { get; set; }
        public DbSet<OrdenTrabajo> OrdenesTrabajo { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrdenTrabajo>()
                .HasOne(o => o.Equipo)
                .WithMany(e => e.OrdenesTrabajo)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
