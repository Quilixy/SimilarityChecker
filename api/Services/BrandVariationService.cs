using System.Text.RegularExpressions;
using api.Data;
using api.Helpers;
using api.Interfaces;
using api.Models;

namespace api.Services;
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

        var sectorKeywords = _context.SectorKeywords
            .Where(k => k.SectorId == brand.SectorId)
            .Select(k => k.Keyword)
            .ToList();

        string cleanedName = RemoveSectorKeywords(brand.Name, sectorKeywords);
        List<string> variations = GenerateVariations(cleanedName);

        foreach (var variation in variations.Distinct())
        {
            _context.BrandVariations.Add(new BrandVariation
            {
                BrandId = brand.Id,
                Variation = variation
            });
        }
        _context.SaveChanges();
    }
    private string RemoveSectorKeywords(string brandName, List<string> keywords)
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
    
     private List<string> GenerateVariations(string cleanedName)
       {
           List<string> variations = new List<string> { cleanedName };
           
           variations.Add(ConvertNumbersToWords(cleanedName));
           
           var parts = Regex.Split(cleanedName, @"[\s&-]+");
           variations.AddRange(parts.Where(p => p.Length > 0));
   
           return variations;
       }
}