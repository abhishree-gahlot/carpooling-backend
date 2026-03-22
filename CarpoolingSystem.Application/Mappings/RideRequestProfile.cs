using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using CarpoolingSystem.Domain.Entities;
using CarpoolingSystem.Application.DTOs;

namespace CarpoolingSystem.Application.Mappings {
    public class RideRequestProfile:Profile {
        public RideRequestProfile() {

            CreateMap<RideRequests, RideRequestDto>()
            .ForMember(dest => dest.RequestId,
                opt => opt.MapFrom(src => src.Id))

            .ForMember(dest => dest.PassengerName,
                opt => opt.MapFrom(src => src.Passenger != null
                    ? src.Passenger.UserName
                    : string.Empty))

            .ForMember(dest => dest.Pickup,
                opt => opt.MapFrom(src => new LocationDto
                {
                    Latitude = src.PickupLatitude,
                    Longitude = src.PickupLongitude,
                    Name = src.PickupName
                }))

            .ForMember(dest => dest.Destination,
                opt => opt.MapFrom(src => new LocationDto
                {
                    Latitude = src.DestinationLatitude,
                    Longitude = src.DestinationLongitude,
                    Name = src.DestinationName
                }));

            CreateMap<RideRequestUpdateDto, RideRequests>()
                .ForAllMembers(mappingOptions =>
                    mappingOptions.Condition(
                        (sourceObject, destinationObject, sourceMemberValue) =>
                            sourceMemberValue != null));
        }
    }
}
