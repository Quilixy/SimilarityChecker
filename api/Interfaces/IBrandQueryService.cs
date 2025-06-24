using api.DTOs;

namespace api.Interfaces
{
    public interface IBrandQueryService
    {
        List<BrandSimilarityResultDTO> GetSimilarBrands(string brandName, List<int> classIds);
    }
}