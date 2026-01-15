using AutoMapper;
using Domain.Entities;
using Application.DTOs; 

namespace Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        
        CreateMap<Solicitud, SolicitudDTO>()
            .ForMember(dest => dest.NombreEstudiante, opt => opt.MapFrom(src => src.Estudiante.Nombre))
            .ForMember(dest => dest.Carrera, opt => opt.MapFrom(src => src.Estudiante.Carrera ?? "Sin Carrera"))
            .ForMember(dest => dest.Materia, opt => opt.MapFrom(src => src.Materia))
            .ForMember(dest => dest.FechaInicio, opt => opt.MapFrom(src => src.FechaInicio))
            .ForMember(dest => dest.FechaFin, opt => opt.MapFrom(src => src.FechaFin));


        CreateMap<SolicitudCreacionDTO, Solicitud>();

        CreateMap<Usuario, LoginDTO>();
    }
}