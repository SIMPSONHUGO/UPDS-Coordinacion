using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Solicitud> Solicitudes { get; set; }
    
    // CAMBIO CLAVE: Quitamos Areas y ponemos Carreras
    public DbSet<Carrera> Carreras { get; set; } 

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Usuario>()
            .Property(u => u.Id)
            .ValueGeneratedOnAdd();
            
        modelBuilder.Entity<Carrera>()
            .Property(c => c.Id)
            .ValueGeneratedOnAdd();
    }
}