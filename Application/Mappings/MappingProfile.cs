namespace Application.Mappings;

using AutoMapper;
using Application.DTOs;
using Domain.Entities;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Hizmet mappings
        CreateMap<CreateServiceRequest, Hizmet>();
        CreateMap<UpdateServiceRequest, Hizmet>();
        CreateMap<Hizmet, ServiceResponse>();
    }
}
