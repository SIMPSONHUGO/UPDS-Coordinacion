using Microsoft.AspNetCore.Mvc;
using Application.UseCases;
using Application.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SolicitudController : ControllerBase
{
    private readonly CrearSolicitudUseCase _crearUseCase;
    private readonly VerMisSolicitudesUseCase _verMisSolicitudesUseCase;
    private readonly RevisarSolicitudUseCase _revisarUseCase;
    private readonly VerReporteCoordinadorUseCase _verReporteUseCase;
    private readonly LoginUseCase _loginUseCase;

    public SolicitudController(
        CrearSolicitudUseCase crearUseCase,
        VerMisSolicitudesUseCase verMisSolicitudesUseCase,
        RevisarSolicitudUseCase revisarUseCase,
        VerReporteCoordinadorUseCase verReporteUseCase,
        LoginUseCase loginUseCase)
    {
        _crearUseCase = crearUseCase;
        _verMisSolicitudesUseCase = verMisSolicitudesUseCase;
        _revisarUseCase = revisarUseCase;
        _verReporteUseCase = verReporteUseCase;
        _loginUseCase = loginUseCase;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDTO dto)
    {
        try
        {
            var token = await _loginUseCase.Ejecutar(dto);
            return Ok(new { token });
        }
        catch (Exception ex)
        {
            return Unauthorized(ex.Message);
        }
    }

    // 🚨 AQUÍ ESTÁ LA CORRECCIÓN IMPORTANTE 🚨
    [HttpPost("crear")]
    [Authorize] 
    // Fíjate aquí: Recibimos 'dto' Y APARTE recibimos 'archivo'
    public async Task<IActionResult> Crear([FromForm] SolicitudDTO dto, IFormFile? archivo) 
    {
        try
        {
            // Manejo del archivo (foto)
            string rutaArchivo = "Sin respaldo";
            
            // Verificamos la variable 'archivo', no 'dto.Archivo'
            if (archivo != null && archivo.Length > 0)
            {
                // Crear carpeta si no existe
                var carpeta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "respaldos");
                if (!Directory.Exists(carpeta)) Directory.CreateDirectory(carpeta);

                // Nombre único para el archivo
                var nombreArchivo = Guid.NewGuid().ToString() + Path.GetExtension(archivo.FileName);
                var rutaCompleta = Path.Combine(carpeta, nombreArchivo);

                // Guardar en disco
                using (var stream = new FileStream(rutaCompleta, FileMode.Create))
                {
                    await archivo.CopyToAsync(stream);
                }

                rutaArchivo = "/respaldos/" + nombreArchivo;
            }

            // Enviamos al caso de uso el DTO limpio y la ruta aparte
            await _crearUseCase.Ejecutar(dto, rutaArchivo);
            return Ok(new { mensaje = "Solicitud creada exitosamente" });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("mis-solicitudes/{estudianteId}")]
    [Authorize]
    public async Task<IActionResult> MisSolicitudes(int estudianteId)
    {
        var lista = await _verMisSolicitudesUseCase.Ejecutar(estudianteId);
        return Ok(lista);
    }

    [HttpGet("pendientes")] 
    [Authorize(Roles = "Jefe,Decano")] 
    public async Task<IActionResult> ObtenerPendientes()
    {
        var lista = await _verReporteUseCase.Ejecutar();
        return Ok(lista);
    }
    
    [HttpGet("coordinador")]
    public async Task<IActionResult> ObtenerCoordinador()
    {
        var lista = await _verReporteUseCase.Ejecutar();
        return Ok(lista);
    }

    [HttpPost("revisar")]
    [Authorize]
    public async Task<IActionResult> Revisar([FromBody] RevisarSolicitudDTO dto)
    {
        try
        {
            await _revisarUseCase.Ejecutar(dto);
            return Ok(new { mensaje = "Solicitud revisada correctamente" });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}