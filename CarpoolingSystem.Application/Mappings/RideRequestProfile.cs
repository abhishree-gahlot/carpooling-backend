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
                 .ForMember(
                     destination => destination.RequestId,
                     options => options.MapFrom(
                         source => source.Id))
                 .ForMember(
                     destination => destination.PassengerName,
                     options => options.MapFrom(
                         source => source.Passenger.UserName))

                 .ForMember(
                     destination => destination.Pickup,
                     options => options.MapFrom(source => new LocationDto {
                         Latitude = source.PickupLatitude,
                         Longitude = source.PickupLongitude,
                         Name = source.PickupName
                     }))

                 .ForMember(
                     destination => destination.Destination,
                     options => options.MapFrom(source => new LocationDto {
                         Latitude = source.DestinationLatitude,
                         Longitude = source.DestinationLongitude,
                         Name = source.DestinationName
                     }));

            CreateMap<RideRequestUpdateDto, RideRequests>()
                .ForAllMembers(mappingOptions =>
                    mappingOptions.Condition(
                        (sourceObject, destinationObject, sourceMemberValue) =>
                            sourceMemberValue != null));
        }
    }
}
