using Application.DTOs;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Threading.Tasks;

namespace Application.UseCases;

public class CrearSolicitudUseCase
{
    private readonly ISolicitudRepository _repository;
    private readonly IMapper _mapper;

    public CrearSolicitudUseCase(ISolicitudRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task Ejecutar(SolicitudDTO dto, string rutaArchivo)
    {

        var solicitud = _mapper.Map<Solicitud>(dto);

        solicitud.RutaRespaldo = rutaArchivo;
        solicitud.Estado = "Pendiente";
        solicitud.FechaSolicitud = DateTime.Now;
        solicitud.ObservacionJefe = "Sin observaciones";

        await _repository.Crear(solicitud);
    }
}