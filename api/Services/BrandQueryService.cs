using System.Text.RegularExpressions;
using api.Data;
using api.Helpers;
using api.DTOs;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Services
{
    public class BrandQueryService : IBrandQueryService
    {
        private readonly AppDbContext _context;
        private readonly IBrandVariationService _variationService;

        public BrandQueryService(AppDbContext context, IBrandVariationService variationService)
        {
            _context = context;
            _variationService = variationService;
        }

        public List<BrandSimilarityResultDTO> GetSimilarBrands(string brandName, List<int> classIds)
        {
            
            var allSectorKeywords = _context.SectorKeywords
                .Select(k => k.Keyword)
                .ToList();
            
            string cleanedInput = _variationService.RemoveSectorKeywords(brandName, allSectorKeywords);
            var inputVariations = _variationService.GenerateVariations(cleanedInput);
            
            var brandVariations = _context.BrandVariations
                            .Include(bv => bv.Brand)
                            .ThenInclude(b => b.BrandClasses)
                            .Where(bv => bv.Brand.BrandClasses.Any(bc => classIds.Contains(bc.ClassId)))
                            .ToList();
            var resultList = new List<BrandSimilarityResultDTO>();
            
            foreach (var brandGroup in brandVariations.GroupBy(bv => bv.BrandId))
            {
                var groupedBrand = brandGroup.First().Brand;
                double totalCombinedScore = 0.0;
                double totalLevenshtein = 0.0;
                double totalPhonetic = 0.0;
                int variationCount = 0;
                
                foreach (var inputVariation in inputVariations)
                {
                    double bestCombined = 0.0;
                    double bestLevenshtein = 0.0;
                    double bestPhonetic = 0.0;

                    foreach (var brandVariation in brandGroup)
                    {
                        double levScore = CalculateLevenshteinSimilarity(inputVariation, brandVariation.Variation);
                        double phoneticScore = CalculatePhoneticSimilarity(inputVariation, brandVariation.Variation);
                        
                        if (IsNumeric(inputVariation) && IsWord(brandVariation.Variation) &&
                            NumberToWordsConverter.NumberToWords(int.Parse(inputVariation)) == brandVariation.Variation)
                        {
                            bestCombined = bestLevenshtein = bestPhonetic = 1.0;
                            break;
                        }
                        else if (IsNumeric(brandVariation.Variation) && IsWord(inputVariation) &&
                                 NumberToWordsConverter.NumberToWords(int.Parse(brandVariation.Variation)) == inputVariation)
                        {
                            bestCombined = bestLevenshtein = bestPhonetic = 1.0;
                            break;
                        }
                        else
                        {
                            double combined = 0.6 * levScore + 0.4 * phoneticScore;

                            if (combined > bestCombined)
                            {
                                bestCombined = combined;
                                bestLevenshtein = levScore;
                                bestPhonetic = phoneticScore;
                            }
                        }
                    }

                    totalCombinedScore += bestCombined;
                    totalLevenshtein += bestLevenshtein;
                    totalPhonetic += bestPhonetic;
                    variationCount++;
                }

               

                resultList.Add(new BrandSimilarityResultDTO
                {
                    BrandName = groupedBrand.Name,
                    SimilarityScore = totalCombinedScore / variationCount,
                    LevenshteinScore = totalLevenshtein / variationCount,
                    PhoneticScore = totalPhonetic / variationCount
                });
            }

            return resultList
                .OrderByDescending(x => x.SimilarityScore)
                .Take(10)
                .ToList();
        }

        private double CalculateLevenshteinSimilarity(string s1, string s2)
        {
            
            string norm1 = Normalize(s1);
            string norm2 = Normalize(s2);
            int distance = LevenshteinDistanceHelper.Calculate(norm1, norm2);
            return 1.0 - (double)distance / Math.Max(norm1.Length, norm2.Length);
        }
        private double CalculatePhoneticSimilarity(string s1, string s2)
        {
            string p1 = HybridPhoneticHelper.NormalizePhonetic(s1);
            string p2 = HybridPhoneticHelper.NormalizePhonetic(s2);
            int dist = LevenshteinDistanceHelper.Calculate(p1, p2);
            return 1.0 - (double)dist / Math.Max(p1.Length, p2.Length);
        }
        
        private string Normalize(string input)
        {
            return Regex.Replace(input.ToLower().Trim(), @"\s+", ""); 
        }
        private bool IsNumeric(string input)
        {
            return int.TryParse(input, out _);
        }
        private bool IsWord(string input)
        {
            return Regex.IsMatch(input, @"^[a-zA-ZçğıöşüÇĞİÖŞÜ]+$");
        }

    }
}
