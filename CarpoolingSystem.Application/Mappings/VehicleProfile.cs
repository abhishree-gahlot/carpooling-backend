using AutoMapper;
using CarpoolingSystem.Application.DTOs;
using CarpoolingSystem.Domain.Entities;

namespace CarpoolingSystem.Application.Mappings
{
    public class VehicleProfile : Profile
    {
        public VehicleProfile()
        {
            CreateMap<Vehicle, VehicleDTO>()
                .ForMember(
                    destination => destination.DriverName,
                    options => options.MapFrom(source => source.Driver.UserName)
                );

            CreateMap<VehicleCreateDTO, Vehicle>();

            CreateMap<VehicleUpdateDTO, Vehicle>()
                .ForAllMembers(options => options.Condition(
                    (source, destination, sourceMember) => sourceMember != null
                ));
        }
    }
}