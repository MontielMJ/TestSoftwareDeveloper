using DirectorioCore.Models;
using Microsoft.EntityFrameworkCore;

namespace DirectorioAPI.Data
{
    public class DirectorioDbContext : DbContext
    {
        public DirectorioDbContext(DbContextOptions<DirectorioDbContext> options) : base(options)
        {
        }
        public DbSet<Persona> Personas { get; set; }
        public DbSet<Factura> Facturas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Persona>().ToTable("Personas");
            modelBuilder.Entity<Factura>().ToTable("Facturas");
        }
    }
}
