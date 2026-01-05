using Application.DTOs;
using Application.UseCases;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SolicitudController : ControllerBase
{
    private readonly CrearSolicitudUseCase _useCase;
    private readonly IMapper _mapper;

    public SolicitudController(CrearSolicitudUseCase useCase, IMapper mapper)
    {
        _useCase = useCase;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<IActionResult> Crear([FromBody] SolicitudDTO dto)
    {
        try
        {
            var solicitud = _mapper.Map<Solicitud>(dto);
            solicitud.Id = Guid.NewGuid();
            await _useCase.Ejecutar(solicitud);
            return Ok("Solicitud Registrada Exitosamente");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}