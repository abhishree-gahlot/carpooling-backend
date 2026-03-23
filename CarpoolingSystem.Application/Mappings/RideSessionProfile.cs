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
                opt => opt.MapFrom(src => src.Vehicle.VehicleName))
            .ForMember(
                    dest => dest.Pickup,
                    opt => opt.MapFrom(src => new LocationDto {
                        Latitude = src.PickupLatitude,
                        Longitude = src.PickupLongitude,
                        Name = src.PickupName
                    }))
                .ForMember(
                    dest => dest.Destination,
                    opt => opt.MapFrom(src => new LocationDto {
                        Latitude = src.DestinationLatitude,
                        Longitude = src.DestinationLongitude,
                        Name = src.DestinationName
                    }));

        CreateMap<RideSessionCreateDto, RideSession>()
            .ForMember(dest => dest.PickupLatitude,
                    opt => opt.MapFrom(src => src.Pickup.Latitude))
                .ForMember(dest => dest.PickupLongitude,
                    opt => opt.MapFrom(src => src.Pickup.Longitude))
                .ForMember(dest => dest.PickupName,
                    opt => opt.MapFrom(src => src.Pickup.Name))
                .ForMember(dest => dest.DestinationLatitude,
                    opt => opt.MapFrom(src => src.Destination.Latitude))
                .ForMember(dest => dest.DestinationLongitude,
                    opt => opt.MapFrom(src => src.Destination.Longitude))
                .ForMember(dest => dest.DestinationName,
                    opt => opt.MapFrom(src => src.Destination.Name)); ;

        CreateMap<RideSessionUpdateDto, RideSession>()
            .ForAllMembers(opt =>
                opt.Condition((src, dest, srcMember) => srcMember != null));
    }
}