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
            if (!string.IsNullOrWhiteSpace(numberConverted) && !string.Equals(numberConverted, cleanedName, StringComparison.OrdinalIgnoreCase))
                variations.Add(numberConverted.ToLowerInvariant());
            
            var parts = Regex.Split(cleanedName, @"[\s&-]+")
                             .Where(p => !string.IsNullOrWhiteSpace(p))
                             .Select(p => p.ToLowerInvariant())
                             .ToList();

            variations.AddRange(parts);
            
            string joined = string.Concat(parts);
            if (!string.IsNullOrWhiteSpace(joined) && !string.Equals(joined, cleanedName, StringComparison.OrdinalIgnoreCase))
                variations.Add(joined);
            
            var numberConvertedParts = Regex.Split(numberConverted, @"[\s&-]+")
                                            .Where(p => !string.IsNullOrWhiteSpace(p))
                                            .Select(p => p.ToLowerInvariant())
                                            .ToList();

            string joinedNumberWords = string.Concat(numberConvertedParts);
            if (!string.IsNullOrWhiteSpace(joinedNumberWords) && !string.Equals(joinedNumberWords, numberConverted, StringComparison.OrdinalIgnoreCase))
                variations.Add(joinedNumberWords);
            
            return variations
                .Where(v => !string.IsNullOrWhiteSpace(v))
                .Select(v => v.Trim().ToLowerInvariant())
                .Distinct()
                .ToList();
        }
    }
}
