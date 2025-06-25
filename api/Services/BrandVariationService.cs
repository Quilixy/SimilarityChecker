using System.Text.RegularExpressions;
using api.Data;
using api.Helpers;
using api.Interfaces;
using api.Models;

namespace api.Services
{
    public class BrandVariationService : IBrandVariationService
    {
        private readonly AppDbContext _context;

        public BrandVariationService(AppDbContext context)
        {
            _context = context;
        }

        public void AddBrandWithVariations(Brand brand)
        {
            _context.Brands.Add(brand);
            _context.SaveChanges();
            
            var sectorIds = brand.BrandSectors.Select(bs => bs.SectorId).ToList();

            var sectorKeywords = _context.SectorKeywords
                .Where(k => sectorIds.Contains(k.SectorId))
                .Select(k => k.Keyword)
                .ToList();


            string cleanedName = RemoveSectorKeywords(brand.Name, sectorKeywords);
            List<string> variations = GenerateVariations(cleanedName);

            foreach (var variation in variations.Distinct())
            {
                if (!string.IsNullOrWhiteSpace(variation))
                {
                    _context.BrandVariations.Add(new BrandVariation
                    {
                        BrandId = brand.Id,
                        Variation = variation.Trim()
                    });
                }
            }

            _context.SaveChanges();
        }

        public string RemoveSectorKeywords(string brandName, List<string> keywords)
        {
            foreach (var keyword in keywords)
            {
                brandName = Regex.Replace(brandName, keyword, "", RegexOptions.IgnoreCase);
            }
            return brandName.Trim();
        }

        private string ConvertNumbersToWords(string input)
        {
            return Regex.Replace(input, @"\d+", match =>
            {
                return NumberToWordsConverter.NumberToWords(int.Parse(match.Value));
            });
        }

        public List<string> GenerateVariations(string cleanedName)
        {
            List<string> variations = new List<string> { cleanedName };
            
            string numberConverted = ConvertNumbersToWords(cleanedName);
            if (!string.IsNullOrWhiteSpace(numberConverted) && numberConverted != cleanedName)
                variations.Add(numberConverted);
            
            var parts = Regex.Split(cleanedName, @"[\s&-]+");
            variations.AddRange(parts.Where(p => !string.IsNullOrWhiteSpace(p)));

            return variations;
        }
    }
}
