using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    // Esto crea la tabla "Solicitudes" en tu base de datos
    public DbSet<Solicitud> Solicitudes { get; set; }
}