using Application.DTOs;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SolicitudController : ControllerBase
{
    private readonly AppDbContext _context;
    
    public SolicitudController(AppDbContext context)
    {
        _context = context;
    }

    // 1. CREAR SOLICITUD (Estudiante)
    [HttpPost]
    [Authorize(Roles = "Estudiante")]
    public async Task<IActionResult> Crear([FromForm] SolicitudCreacionDTO dto)
    {
        try
        {
            // Extraer ID de forma robusta soportando esquemas XML y estándar
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? 
                              User.Claims.FirstOrDefault(c => c.Type == "Id" || c.Type == "nameid")?.Value;

            if (string.IsNullOrEmpty(userIdClaim)) 
                return Unauthorized(new { error = "Sesión inválida. No se encontró el ID del usuario." });

            var userId = int.Parse(userIdClaim);
            string nombreArchivoFinal = "Sin respaldo";

            // Procesamiento de archivos
            if (dto.Archivo != null && dto.Archivo.Length > 0)
            {
                var extension = Path.GetExtension(dto.Archivo.FileName);
                var nombreUnico = $"{Guid.NewGuid()}{extension}";
                var rutaCarpeta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "respaldos");
                
                if (!Directory.Exists(rutaCarpeta)) Directory.CreateDirectory(rutaCarpeta);
                
                var rutaCompleta = Path.Combine(rutaCarpeta, nombreUnico);
                using (var stream = new FileStream(rutaCompleta, FileMode.Create))
                {
                    await dto.Archivo.CopyToAsync(stream);
                }
                nombreArchivoFinal = nombreUnico;
            }

            var nuevaSolicitud = new Solicitud
            {
                EstudianteId = userId, 
                Motivo = dto.Motivo,
                Materia = dto.Materia,
                Docente = dto.Docente,
                FechaInicio = dto.FechaInicio,
                FechaFin = dto.FechaFin,
                RutaRespaldo = nombreArchivoFinal,
                FechaSolicitud = DateTime.Now,
                Estado = "Pendiente",
                ObservacionJefe = "Sin observaciones"
            };

            _context.Solicitudes.Add(nuevaSolicitud);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Solicitud enviada correctamente" });
        }
        catch (Exception ex)
        {
            var msg = ex.InnerException?.Message ?? ex.Message;
            return BadRequest(new { error = $"Error al guardar: {msg}" });
        }
    }

    // 2. VER MIS SOLICITUDES (Estudiante)
    [HttpGet("mis-solicitudes")]
    [Authorize(Roles = "Estudiante")]
    public async Task<ActionResult<IEnumerable<SolicitudVisualDTO>>> ObtenerMisSolicitudes()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? 
                          User.Claims.FirstOrDefault(c => c.Type == "Id" || c.Type == "nameid")?.Value;

        if (userIdClaim == null) return Unauthorized(new { error = "Usuario no identificado." });
        int userId = int.Parse(userIdClaim);

        return await _context.Solicitudes
            .Where(s => s.EstudianteId == userId)
            .OrderByDescending(s => s.FechaSolicitud)
            .Select(s => new SolicitudVisualDTO
            {
                Id = s.Id,
                NombreEstudiante = s.Estudiante.Nombre,
                Carrera = s.Estudiante.Carrera ?? "Sin Carrera",
                Motivo = s.Motivo,
                Materia = s.Materia,
                Estado = s.Estado,
                ObservacionJefe = s.ObservacionJefe,
                RutaRespaldo = s.RutaRespaldo,
                FechaSolicitud = s.FechaSolicitud,
                FechaInicio = s.FechaInicio,
                FechaFin = s.FechaFin
            })
            .ToListAsync();
    }

    // 3. VER PENDIENTES (Jefes de Carrera)
    [HttpGet("pendientes")]
    [Authorize(Roles = "Jefe")]
    public async Task<ActionResult<IEnumerable<SolicitudVisualDTO>>> ObtenerPendientes()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? 
                          User.Claims.FirstOrDefault(c => c.Type == "Id" || c.Type == "nameid")?.Value;

        if (userIdClaim == null) return Unauthorized();
        int jefeId = int.Parse(userIdClaim);

        var jefe = await _context.Usuarios.FindAsync(jefeId);
        if (jefe == null) return Unauthorized();

        var query = _context.Solicitudes.Include(s => s.Estudiante).Where(s => s.Estado == "Pendiente");

        // Filtrado por Carrera usando nombres parciales para mayor estabilidad
        if (jefe.Nombre.Contains("Vanessa")) 
            query = query.Where(s => s.Estudiante.Carrera.Contains("Psicología") || s.Estudiante.Carrera.Contains("Social"));
        else if (jefe.Nombre.Contains("Javier"))
            query = query.Where(s => s.Estudiante.Carrera.Contains("Contaduría") || s.Estudiante.Carrera.Contains("Administración") || s.Estudiante.Carrera.Contains("Marketing"));
        else if (jefe.Nombre.Contains("Ana"))
            query = query.Where(s => s.Estudiante.Carrera.Contains("Derecho"));
        else if (jefe.Nombre.Contains("Eiver"))
            query = query.Where(s => s.Estudiante.Carrera.Contains("Sistemas") || s.Estudiante.Carrera.Contains("Redes") || s.Estudiante.Carrera.Contains("Telecomunicaciones"));
        else if (jefe.Nombre.Contains("Martin"))
            query = query.Where(s => s.Estudiante.Carrera.Contains("Civil") || s.Estudiante.Carrera.Contains("Industrial"));

        return await query.Select(s => new SolicitudVisualDTO
        {
            Id = s.Id,
            NombreEstudiante = s.Estudiante.Nombre,
            Carrera = s.Estudiante.Carrera ?? "Sin Carrera",
            Motivo = s.Motivo,
            Materia = s.Materia,
            FechaInicio = s.FechaInicio,
            FechaFin = s.FechaFin,
            FechaSolicitud = s.FechaSolicitud,
            Estado = s.Estado,
            RutaRespaldo = s.RutaRespaldo
        }).ToListAsync();
    }

    // 4. REVISAR (Aprobar o Rechazar)
    [HttpPost("revisar/{id}")]
    [Authorize(Roles = "Jefe")]
    public async Task<IActionResult> Revisar(int id, [FromBody] RevisarSolicitudAccionDTO dto)
    {
        var solicitud = await _context.Solicitudes.FindAsync(id);
        if (solicitud == null) return NotFound(new { error = "Solicitud no encontrada" });
        
        solicitud.Estado = dto.Estado;
        solicitud.ObservacionJefe = dto.Observacion;
        await _context.SaveChangesAsync();
        
        return Ok(new { message = $"Solicitud {dto.Estado} correctamente" });
    }

    // 5. OBTENER PROCESADAS (Para Historial del Jefe)
    [HttpGet("procesadas")]
    [Authorize(Roles = "Jefe")]
    public async Task<ActionResult<IEnumerable<SolicitudVisualDTO>>> ObtenerProcesadas()
    {
        return await _context.Solicitudes
            .Where(s => s.Estado != "Pendiente")
            .OrderByDescending(s => s.FechaSolicitud)
            .Select(s => new SolicitudVisualDTO { Id = s.Id, Estado = s.Estado })
            .ToListAsync();
    }
}

// DTOs auxiliares sincronizados con el Frontend
public class RevisarSolicitudAccionDTO { public string Estado { get; set; } = string.Empty; public string Observacion { get; set; } = string.Empty; }

public class SolicitudVisualDTO
{
    public int Id { get; set; }
    public string NombreEstudiante { get; set; } = string.Empty;
    public string Carrera { get; set; } = string.Empty;
    public DateTime FechaSolicitud { get; set; }
    public string Motivo { get; set; } = string.Empty;
    public string Materia { get; set; } = string.Empty;
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }
    public string Estado { get; set; } = string.Empty;
    public string RutaRespaldo { get; set; } = string.Empty;
    public string? ObservacionJefe { get; set; }
}