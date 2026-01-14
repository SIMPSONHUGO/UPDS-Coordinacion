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

    // 🚨 AQUÍ ESTÁ EL ARREGLO:
    // Agregamos 'string rutaArchivo' para que coincida con el Controlador
    public async Task Ejecutar(SolicitudDTO dto, string rutaArchivo)
    {
        // 1. Convertimos los datos del formulario
        var solicitud = _mapper.Map<Solicitud>(dto);

        // 2. Guardamos la ruta de la foto que nos pasó el Controlador
        solicitud.RutaRespaldo = rutaArchivo;
        
        // 3. Llenamos los datos automáticos
        solicitud.Estado = "Pendiente";
        solicitud.FechaSolicitud = DateTime.Now;
        solicitud.ObservacionJefe = "Sin observaciones";

        // 4. Guardamos en la Base de Datos
        await _repository.Crear(solicitud);
    }
}