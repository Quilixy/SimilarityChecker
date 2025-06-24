using System.Collections.Generic;
using api.DTOs;
using api.Models;

namespace api.Interfaces
{
    public interface IBrandRepository
    {
        List<BrandDTO> GetAll();
        Brand GetById(int id);
        void Add(Brand brand);
        bool Update(Brand brand);
        bool Delete(int id);
    }
}