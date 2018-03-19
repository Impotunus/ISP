using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ISP.BLL.DTO.Domain;
using ISP.BLL.Interfaces;
using ISP.ViewModels;
using ISP.ViewModels.FeatureViewModels;

namespace ISP.Controllers
{
    [Authorize(Roles = "admin")]
    public class FeatureController : Controller
    {
        private IFeatureService FeatureService { get; }

        private IPlanService PlanService { get; }

        public FeatureController(IFeatureService featureService, IPlanService planService)
        {
            FeatureService = featureService;
            PlanService = planService;
        }

        public ActionResult Index(int planId)
        {
            var featureDTOs = FeatureService.GetFeatures(planId);
            var featureListViewModel = new FeatureListViewModel()
            {
                FeatureViewModels = AutoMapper.Mapper.Map<List<FeatureViewModel>>(featureDTOs),
                PlanId = planId
            }; 
            
            return View(featureListViewModel);
        }

        [HttpGet]
        public ActionResult Create(int planId)
        {
            var featureCreateViewModel = new FeatureCreateViewModel()
            {
                FeatureViewModel = new FeatureViewModel(),
                PlanId = planId
            };

            return View(featureCreateViewModel);
        }

        [HttpPost]
        public ActionResult Add(FeatureCreateViewModel featureCreateViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("Create", featureCreateViewModel);
            }

            var featureDTO = AutoMapper.Mapper.Map<FeatureDTO>(featureCreateViewModel.FeatureViewModel);
            var featureId = FeatureService.CreateFeature(featureDTO);
            var planDTO = PlanService.GetPlan(featureCreateViewModel.PlanId);
            PlanService.AddFeature(planDTO.Id, featureId);

            return RedirectToAction("Index", new {planId = featureCreateViewModel.PlanId});
        }

        public ActionResult Delete(int featureId)
        {
            var planId = GetFeaturePlanId(featureId);
            FeatureService.DeleteFeature(featureId);

            return RedirectToAction("Index", new { planId = planId });
        }

        private int GetFeaturePlanId(int featureId)
        {
            var plan = PlanService.GetPlans().First(t => t.Features.Any(x => x.Id == featureId));

            return plan.Id;
        }
    }
}