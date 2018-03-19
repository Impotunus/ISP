using System.Collections.Generic;
using System.Linq;
using ISP.BLL.DTO.Domain;
using ISP.BLL.Interfaces;
using ISP.DAL.Interfaces;
using ISP.DAL.Models.Domain;

namespace ISP.BLL.Services
{
    public class FeatureService : IFeatureService
    {
        private IUnitOfWork Database { get; }

        public FeatureService(IUnitOfWork database)
        {
            Database = database;
        }

        public ICollection<FeatureDTO> GetFeatures(int planId)
        {
            var features = Database.PlansRepository.Get(planId).Features.Where(t => t.IsDeleted == false);
            var featureDTOs = AutoMapper.Mapper.Map<List<FeatureDTO>>(features);

            return featureDTOs;
        }

        public FeatureDTO GetFeature(int id)
        {
            var feature = Database.FeaturesRepository.Get(id);
            var featureDTO = AutoMapper.Mapper.Map<FeatureDTO>(feature);

            return featureDTO;
        }

        public void UpdateFeature(FeatureDTO featureDTO)
        {
            var feature = Database.FeaturesRepository.Get(featureDTO.Id);
            AutoMapper.Mapper.Map(featureDTO, feature);
            
            Database.SaveChanges();
        }

        public int CreateFeature(FeatureDTO featureDTO)
        {
            var feature = AutoMapper.Mapper.Map<Feature>(featureDTO);
            Database.FeaturesRepository.Create(feature);

            Database.SaveChanges();

            return feature.Id;
        }

        public void DeleteFeature(int id)
        {
            Database.FeaturesRepository.Delete(id);
            Database.SaveChanges();
        }
    }
}
