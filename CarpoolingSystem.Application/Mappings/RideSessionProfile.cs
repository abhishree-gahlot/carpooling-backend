using AutoMapper;
using CarpoolingSystem.Application.DTOs;
using CarpoolingSystem.Domain.Entities;

namespace CarpoolingSystem.Application.Mappings;

public class RideSessionProfile : Profile
{
    public RideSessionProfile()
    {
        CreateMap<RideSession, RideSessionDto>()
            .ForMember(
                dest => dest.RideId,
                opt => opt.MapFrom(src => src.Id))
            .ForMember(
                dest => dest.DriverName,
                opt => opt.MapFrom(src => src.Driver.UserName))
            .ForMember(
                dest => dest.VehicleName,
                opt => opt.MapFrom(src => src.Vehicle.VehicleName));

        CreateMap<RideSessionCreateDto, RideSession>();

        CreateMap<RideSessionUpdateDto, RideSession>()
            .ForAllMembers(opt =>
                opt.Condition((src, dest, srcMember) => srcMember != null));
    }
}