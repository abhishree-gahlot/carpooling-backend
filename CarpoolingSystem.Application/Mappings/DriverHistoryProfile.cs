using AutoMapper;
using CarpoolingSystem.Application.DTOs;
using CarpoolingSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarpoolingSystem.Application.Mappings {
    public class DriverHistoryProfile : Profile{

        public DriverHistoryProfile() {
            CreateMap<DriverHistory, DriverHistoryDto>()
                .ForMember(
                    dest => dest.DriverName,
                    opt => opt.MapFrom(src => src.Driver.UserName))
                .ForMember(
                    dest => dest.Passengers,
                    opt => opt.MapFrom(src => src.Passengers));

            CreateMap<DriverHistoryPassenger, DriverHistoryPassengerDto>();
        }
    }
}
