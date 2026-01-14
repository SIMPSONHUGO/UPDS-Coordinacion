using AutoMapper;
using Application.DTOs;
using Domain.Entities;

namespace Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // 🚨 CAMBIO: De SolicitudDTO a Solicitud
        CreateMap<SolicitudDTO, Solicitud>();

        // Mapeos para ver datos
        CreateMap<Solicitud, SolicitudEstudianteDTO>()
            .ForMember(dest => dest.ObservacionJefe, opt => opt.MapFrom(src => src.ObservacionJefe ?? "Sin observaciones"));

        CreateMap<Solicitud, SolicitudCoordinadorDTO>()
            .ForMember(dest => dest.NombreEstudiante, opt => opt.MapFrom(src => src.Estudiante.Nombre))
            .ForMember(dest => dest.Carrera, opt => opt.MapFrom(src => src.Estudiante.Carrera.Nombre))
            .ForMember(dest => dest.ObservacionJefe, opt => opt.MapFrom(src => src.ObservacionJefe ?? "Sin observaciones"));
    }
}