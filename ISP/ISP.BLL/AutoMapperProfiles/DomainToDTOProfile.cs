using AutoMapper;
using ISP.BLL.DTO;
using ISP.BLL.DTO.Domain;
using ISP.BLL.DTO.Identity;
using ISP.DAL.Enums;
using ISP.DAL.Models.Domain;
using ISP.DAL.Models.Identity;

namespace ISP.BLL.AutoMapperProfiles
{
    public class DomainToDTOProfile : Profile
    {
        public DomainToDTOProfile()
        {
            CreateMap<Service, ServiceDTO>();
            CreateMap<Plan, PlanDTO>();
            CreateMap<Feature, FeatureDTO>();
            CreateMap<UserPlan, UserPlanDTO>();
            CreateMap<UserService, UserServiceDTO>();
            CreateMap<ServiceStatus, ServiceStatusDTO>();
            CreateMap<ApplicationUser, UserDTO>();
        }
    }
}
