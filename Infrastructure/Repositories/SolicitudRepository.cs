using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class SolicitudRepository : ISolicitudRepository
{
    private readonly AppDbContext _context;

    public SolicitudRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task Crear(Solicitud solicitud)
    {
        await _context.Solicitudes.AddAsync(solicitud);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Solicitud>> ObtenerTodas()
    {
        return await _context.Solicitudes.ToListAsync();
    }
}