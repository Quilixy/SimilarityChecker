using api.Models;

namespace api.Interfaces;

public interface IBrandVariationService
{
    void AddBrandWithVariations(Brand brand);
    string RemoveSectorKeywords(string brandName, List<string> keywords);
    List<string> GenerateVariations(string cleanedName);
}