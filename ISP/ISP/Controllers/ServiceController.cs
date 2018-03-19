using System.Collections.Generic;
using System.Web.Mvc;
using ISP.Attributes;
using ISP.BLL.DTO.Domain;
using ISP.BLL.Interfaces;
using ISP.ViewModels;
using ISP.ViewModels.ServiceViewModels;

namespace ISP.Controllers
{
    [RedirectBanned()]
    public class ServiceController : Controller
    {
        private IServiceService ServiceService { get; }

        public ServiceController(IServiceService serviceService)
        {
            ServiceService = serviceService;
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public ActionResult GetServicesForAdmin()
        {
            var serviceDTOs = ServiceService.GetServices();
            var serviceViewModels = AutoMapper.Mapper.Map<List<ServiceViewModel>>(serviceDTOs);

            return View(serviceViewModels);
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public ActionResult CreateService()
        {
            return View(new ServiceCreateViewModel() { ServiceViewModel = new ServiceViewModel()});
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public ActionResult AddService(ServiceViewModel serviceViewModel)
        {
            if (ModelState.IsValid)
            {
                ServiceService.CreateService(AutoMapper.Mapper.Map<ServiceDTO>(serviceViewModel));
                return RedirectToAction("GetServicesForAdmin");
            }
            var createViewModel = new ServiceCreateViewModel {ServiceViewModel = serviceViewModel};

            return View("CreateService", createViewModel);
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public ActionResult EditService(int serviceId)
        {
            var serviceDTO = ServiceService.GetService(serviceId);
            var serviceViewModel = AutoMapper.Mapper.Map<ServiceViewModel>(serviceDTO);
            var editViewModel = new ServiceCreateViewModel {ServiceViewModel = serviceViewModel};

            return View(editViewModel);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public ActionResult UpdateService(ServiceViewModel serviceViewModel)
        {
            if (ModelState.IsValid)
            {
                ServiceService.UpdateService(AutoMapper.Mapper.Map<ServiceDTO>(serviceViewModel));
                return RedirectToAction("GetServicesForAdmin");
            }
            var editViewModel = new ServiceCreateViewModel {ServiceViewModel = serviceViewModel};

            return View("EditService", editViewModel);
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public ActionResult DeleteService(int serviceId)
        {
            ServiceService.DeleteService(serviceId);

            return RedirectToAction("GetServicesForAdmin");
        }
    }
}