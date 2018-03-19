using AutoMapper;
using ISP.BLL.DTO;
using ISP.BLL.DTO.Domain;
using ISP.BLL.DTO.Identity;
using ISP.ViewModels;
using ISP.ViewModels.FeatureViewModels;
using ISP.ViewModels.PlanViewModels;
using ISP.ViewModels.ProfileViewModels;
using ISP.ViewModels.ServiceViewModels;

namespace ISP.AutoMapperProfiles
{
    public  class DTOToViewModelProfile : Profile
    {
        public DTOToViewModelProfile()
        {
            CreateMap<UserDTO, UserViewModel>();
            CreateMap<FeatureDTO, FeatureViewModel>();
            CreateMap<PlanDTO, PlanViewModel>();
            CreateMap<ServiceDTO, ServiceViewModel>()
                .ForMember(t => t.UserServices, x => x.Ignore());
            CreateMap<UserPlanDTO, UserPlanViewModel>();
            CreateMap<UserServiceDTO, UserServiceViewModel>();
            CreateMap<ServiceStatusDTO, ServiceStatusViewModel>();
        }
    }
}