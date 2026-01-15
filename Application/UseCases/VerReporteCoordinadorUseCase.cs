using Application.DTOs;
using AutoMapper;
using Domain.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.UseCases;

public class VerReporteCoordinadorUseCase
{
    private readonly ISolicitudRepository _repository;
    private readonly IMapper _mapper;

    public VerReporteCoordinadorUseCase(ISolicitudRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<List<SolicitudCoordinadorDTO>> Ejecutar()
    {
        var listaEntidades = await _repository.ObtenerTodas();
        
        return _mapper.Map<List<SolicitudCoordinadorDTO>>(listaEntidades);
    }
}