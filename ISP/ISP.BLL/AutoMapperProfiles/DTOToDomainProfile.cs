using AutoMapper;
using ISP.BLL.DTO;
using ISP.BLL.DTO.Domain;
using ISP.BLL.DTO.Identity;
using ISP.DAL.Enums;
using ISP.DAL.Models.Domain;
using ISP.DAL.Models.Identity;

namespace ISP.BLL.AutoMapperProfiles
{
    public class DTOToDomainProfile : Profile
    {
        public DTOToDomainProfile()
        {
            CreateMap<ServiceDTO, Service>();
            CreateMap<PlanDTO, Plan>();
            CreateMap<FeatureDTO, Feature>();
            CreateMap<UserPlanDTO, UserPlan>();
            CreateMap<UserServiceDTO, UserService>();
            CreateMap<ServiceStatusDTO, ServiceStatus>();
            CreateMap<UserDTO, ApplicationUser>();
        }
    }
}
