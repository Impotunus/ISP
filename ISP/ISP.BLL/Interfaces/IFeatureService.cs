using System.Collections.Generic;
using ISP.BLL.DTO.Domain;

namespace ISP.BLL.Interfaces
{
    public interface IFeatureService
    {
        ICollection<FeatureDTO> GetFeatures(int planId);

        FeatureDTO GetFeature(int id);

        void UpdateFeature(FeatureDTO featureDTO);

        int CreateFeature(FeatureDTO featureDTO);

        void DeleteFeature(int id);
    }
}
