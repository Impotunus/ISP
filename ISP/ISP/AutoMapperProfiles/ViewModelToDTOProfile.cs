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
    public  class ViewModelToDTOProfile : Profile
    {
        public ViewModelToDTOProfile()
        {
            CreateMap<UserViewModel, UserDTO>();
            CreateMap<FeatureViewModel, FeatureDTO>();
            CreateMap<PlanViewModel, PlanDTO>();
            CreateMap<ServiceViewModel, ServiceDTO>();
            CreateMap<UserPlanViewModel, UserPlanDTO>();
            CreateMap<UserServiceViewModel, UserServiceDTO>();
            CreateMap<ServiceStatusViewModel, ServiceStatusDTO>();
        }
    }
}