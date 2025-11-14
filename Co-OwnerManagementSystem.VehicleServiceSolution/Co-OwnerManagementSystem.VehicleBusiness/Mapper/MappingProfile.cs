using AutoMapper;
using Co_OwnerManagementSystem.VehicleApplication.Models;
using Co_OwnerManagementSystem.VehicleInfrastructure.Entities;

namespace Co_OwnerManagementSystem.VehicleApplication.Mapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Vehicle, VehicleOverallModel>();
        CreateMap<Vehicle, VehicleModel>();
        CreateMap<VehicleCreateModel, Vehicle>();
        CreateMap<VehicleUpdateModel, Vehicle>()
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
    }
}