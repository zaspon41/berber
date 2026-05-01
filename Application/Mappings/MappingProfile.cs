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

        // OperatingHours mappings
        CreateMap<CreateOperatingHoursRequest, OperatingHours>();
        CreateMap<UpdateOperatingHoursRequest, OperatingHours>();
        CreateMap<OperatingHours, OperatingHoursResponse>();

        // BlockedDates mappings
        CreateMap<CreateBlockedDatesRequest, BlockedDates>();
        CreateMap<UpdateBlockedDatesRequest, BlockedDates>();
        CreateMap<BlockedDates, BlockedDatesResponse>();

        // Appointment mappings
        CreateMap<CreateAppointmentRequest, Appointment>();
        CreateMap<UpdateAppointmentRequest, Appointment>();
        CreateMap<Appointment, AppointmentResponse>();
    }
}
