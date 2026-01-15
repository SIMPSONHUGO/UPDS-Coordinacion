using AutoMapper;
using Domain.Entities;
using Application.DTOs; // 👈 ¡Esta es la carpeta correcta donde creaste el archivo!

namespace Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // 1. Mapeo de Solicitud a SolicitudDTO
        CreateMap<Solicitud, SolicitudDTO>()
            .ForMember(dest => dest.NombreEstudiante, opt => opt.MapFrom(src => src.Estudiante.Nombre))
            // Corrección: Ahora Carrera es un texto simple, no un objeto
            .ForMember(dest => dest.Carrera, opt => opt.MapFrom(src => src.Estudiante.Carrera ?? "Sin Carrera"))
            .ForMember(dest => dest.Materia, opt => opt.MapFrom(src => src.Materia))
            .ForMember(dest => dest.FechaInicio, opt => opt.MapFrom(src => src.FechaInicio))
            .ForMember(dest => dest.FechaFin, opt => opt.MapFrom(src => src.FechaFin));

        // 2. Mapeo de Creación
        CreateMap<SolicitudCreacionDTO, Solicitud>();

        // 3. Otros mapeos
        CreateMap<Usuario, LoginDTO>();
    }
}