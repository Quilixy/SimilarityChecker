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
                .Include(b => b.BrandClasses)
                .ThenInclude(bc => bc.Class)
                .Include(b => b.BrandSectors)
                .ThenInclude(bs => bs.Sector)
                .Select(b => new BrandDTO
                {
                    Id = b.Id,
                    Name = b.Name,
                    Classes = b.BrandClasses.Select(bc => bc.Class).ToList(),
                    Sectors = b.BrandSectors.Select(bs => bs.Sector).ToList()
                })
                .ToList();
        }
        
        public Brand GetById(int id)
        {
            return _context.Brands
                       .Include(b => b.BrandClasses)
                       .ThenInclude(bc => bc.Class)
                       .Include(b => b.BrandSectors)
                       .ThenInclude(bs => bs.Sector)
                       .FirstOrDefault(b => b.Id == id)
                   ?? throw new InvalidOperationException("Brand not found.");
        }

        public void Add(Brand brand)
        {
            _context.Brands.Add(brand);
            _context.SaveChanges();
        }

        public bool Update(Brand brand)
        {
            var existingBrand = _context.Brands
                .Include(b => b.BrandClasses)
                .Include(b => b.BrandSectors)
                .FirstOrDefault(b => b.Id == brand.Id);

            if (existingBrand == null)
                return false;

            existingBrand.Name = brand.Name;
            
            existingBrand.BrandClasses.Clear();
            foreach (var bc in brand.BrandClasses)
            {
                existingBrand.BrandClasses.Add(new BrandClass
                {
                    BrandId = brand.Id,
                    ClassId = bc.ClassId
                });
            }
            
            existingBrand.BrandSectors.Clear();
            foreach (var bs in brand.BrandSectors)
            {
                existingBrand.BrandSectors.Add(new BrandSector
                {
                    BrandId = brand.Id,
                    SectorId = bs.SectorId
                });
            }

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