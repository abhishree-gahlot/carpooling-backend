using AutoMapper;
using CarpoolingSystem.Application.DTOs;
using CarpoolingSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarpoolingSystem.Application.Mappings {
    public class BookingProfile : Profile {

        public BookingProfile() {
            CreateMap<Booking, BookingDto>()
                .ForMember(
                    destination => destination.DriverName,
                    options => options.MapFrom(
                        source => source.RideSession.Driver.UserName))
                .ForMember(
                    destination => destination.VehicleName,
                    options => options.MapFrom(
                        source => source.RideSession.Vehicle.VehicleName))
                .ForMember(
                    destination => destination.PassengerName,
                    options => options.MapFrom(
                        source => source.RideRequest.Passenger.UserName));
        }
    }
}
