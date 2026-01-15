using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        _context.Solicitudes.Add(solicitud);
        await _context.SaveChangesAsync();
    }

    public async Task Actualizar(Solicitud solicitud)
    {
        _context.Solicitudes.Update(solicitud);
        await _context.SaveChangesAsync();
    }
    public async Task<Solicitud?> ObtenerPorId(int id)
    {
        return await _context.Solicitudes
            .Include(s => s.Estudiante)
            .Include(s => s.Estudiante.Carrera) 
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<List<Solicitud>> ObtenerTodas()
    {
        return await _context.Solicitudes
            .Include(s => s.Estudiante)
            .Include(s => s.Estudiante.Carrera)
            .OrderByDescending(s => s.FechaSolicitud)
            .ToListAsync();
    }

    public async Task<List<Solicitud>> ObtenerPorEstudiante(int estudianteId)
    {
        return await _context.Solicitudes
            .Where(s => s.EstudianteId == estudianteId)
            .OrderByDescending(s => s.FechaSolicitud)
            .ToListAsync();
    }

    public async Task<Usuario?> ObtenerUsuarioPorEmail(string email)
    {
        return await _context.Usuarios
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<Usuario?> ObtenerUsuarioPorId(int id)
    {
        return await _context.Usuarios
            .FindAsync(id);
    }
}