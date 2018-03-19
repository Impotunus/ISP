using System.Collections.Generic;
using ISP.BLL.DTO.Domain;
using ISP.BLL.Interfaces;
using ISP.DAL.Interfaces;
using ISP.DAL.Models.Domain;

namespace ISP.BLL.Services
{
    public class ServiceService: IServiceService
    {
        private IUserServicesService UserServicesService { get; }

        private IUnitOfWork Database { get; set; }

        public ServiceService(IUnitOfWork unitOfWork, IUserServicesService userServicesService)
        {
            Database = unitOfWork;
            UserServicesService = userServicesService;
        }

        public ICollection<ServiceDTO> GetServices()
        {
            var services = Database.ServicesRepository.GetAll();
            var serviceDTOs = AutoMapper.Mapper.Map<List<ServiceDTO>>(services);

            return serviceDTOs;
        }

        public ServiceDTO GetService(int id)
        {
            var service = Database.ServicesRepository.Get(id);
            var serviceDTO = AutoMapper.Mapper.Map<ServiceDTO>(service);

            return serviceDTO;
        }

        public void CreateService(ServiceDTO serviceDTO)
        {
            var service = AutoMapper.Mapper.Map<Service>(serviceDTO);
            Database.ServicesRepository.Create(service);
            Database.SaveChanges();
        }

        public void UpdateService(ServiceDTO serviceDTO)
        {
            var service = Database.ServicesRepository.Get(serviceDTO.Id);
            service.Description = serviceDTO.Description;
            service.Title = serviceDTO.Title;

            Database.SaveChanges();
        }

        public void DeleteService(int id)
        {
            var userServices = Database.UserServicesRepository.Find(t => t.ServiceId == id);

            foreach (var userService in userServices)
            {
                UserServicesService.UnsubscribeUserFromService(userService.ApplicationUser.UserName, AutoMapper.Mapper.Map<ServiceDTO>(userService.Service));
            }

            Database.ServicesRepository.Delete(id);
            Database.SaveChanges();
        }
    }
}
