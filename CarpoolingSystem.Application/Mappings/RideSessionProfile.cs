using AutoMapper;
using CarpoolingSystem.Application.DTOs;
using CarpoolingSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarpoolingSystem.Application.Mappings {
    public class RideSessionProfile: Profile {
        public RideSessionProfile() {
            CreateMap<RideSession, RideSessionDto>()

               .ForMember(
                   destination => destination.RideId,
                   options => options.MapFrom(source => source.Id))
               .ForMember(
                   destination => destination.DriverName,
                   options => options.MapFrom(
                       source => source.Driver.UserName))
               .ForMember(
                   destination => destination.VehicleName,
                   options => options.MapFrom(
                       source => source.Vehicle.VehicleName))
               .ForMember(
                   destination => destination.PassengerName,
                   options => options.MapFrom(
                       source => source.Passenger != null
                           ? source.Passenger.UserName
                           : null));

            CreateMap<RideSessionCreateDto, RideSession>();

            CreateMap<RideSessionUpdateDto, RideSession>()
                .ForAllMembers(mappingOptions =>
                    mappingOptions.Condition(
                        (sourceObject, destinationObject, sourceMemberValue) =>
                            sourceMemberValue != null));
        }
        }
}
