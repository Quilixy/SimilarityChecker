using api.Models;

namespace api.Interfaces;

public interface IBrandService
{
    void AddBrandWithVariations(Brand brand);
}