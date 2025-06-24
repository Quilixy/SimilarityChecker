using System.Collections.Generic;
using System.Linq;
using api.Data;
using api.DTOs;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories
{
    public class BrandRepository : IBrandRepository
    {
        private readonly AppDbContext _context;

        public BrandRepository(AppDbContext context)
        {
            _context = context;
        }

        public List<BrandDTO> GetAll()
        {
            return _context.Brands
                .Include(b => b.Class)
                .Select(b => new BrandDTO
                {
                    Id = b.Id,
                    Name = b.Name,
                    ClassId = b.ClassId
                })
                .ToList();
        }
        
        public Brand GetById(int id)
        {
            return _context.Brands
                .Include(b => b.Class)
                .Include(b => b.Sector)
                .FirstOrDefault(b => b.Id == id);
        }

        public void Add(Brand brand)
        {
            _context.Brands.Add(brand);
            _context.SaveChanges();
        }

        public bool Update(Brand brand)
        {
            var existingBrand = _context.Brands.Find(brand.Id);
            if (existingBrand == null)
                return false;

            existingBrand.Name = brand.Name;
            existingBrand.ClassId = brand.ClassId;
            existingBrand.SectorId = brand.SectorId;

            _context.SaveChanges();
            return true;
        }

        public bool Delete(int id)
        {
            var brand = _context.Brands.Find(id);
            if (brand == null)
                return false;

            _context.Brands.Remove(brand);
            _context.SaveChanges();
            return true;
        }
    }
}