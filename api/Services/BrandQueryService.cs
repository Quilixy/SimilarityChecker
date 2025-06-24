using System.Text.RegularExpressions;
using api.Data;

using api.DTOs;
using api.Helpers;
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
                .Where(bv => classIds.Contains(bv.Brand.ClassId))
                .ToList();

            var resultList = new List<BrandSimilarityResultDTO>();

            foreach (var brandGroup in brandVariations.GroupBy(bv => bv.BrandId))
            {
                var brand = brandGroup.First().Brand;
                double maxScore = 0.0;

                foreach (var brandVariation in brandGroup)
                {
                    foreach (var inputVariation in inputVariations)
                    {
                        double score = CalculateLevenshteinSimilarity(inputVariation, brandVariation.Variation);
                        if (score > maxScore)
                            maxScore = score;
                    }
                }

                resultList.Add(new BrandSimilarityResultDTO
                {
                    BrandName = brand.Name,
                    SimilarityScore = maxScore
                });
            }

            return resultList
                .OrderByDescending(x => x.SimilarityScore)
                .Take(10)
                .ToList();
        }

        private double CalculateLevenshteinSimilarity(string s1, string s2)
        {
            int distance = LevenshteinDistance(s1.ToLower(), s2.ToLower());
            return 1.0 - (double)distance / Math.Max(s1.Length, s2.Length);
        }

        private int LevenshteinDistance(string s, string t)
        {
            int[,] d = new int[s.Length + 1, t.Length + 1];

            for (int i = 0; i <= s.Length; i++) d[i, 0] = i;
            for (int j = 0; j <= t.Length; j++) d[0, j] = j;

            for (int i = 1; i <= s.Length; i++)
            {
                for (int j = 1; j <= t.Length; j++)
                {
                    int cost = (s[i - 1] == t[j - 1]) ? 0 : 1;
                    d[i, j] = Math.Min(
                        Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                        d[i - 1, j - 1] + cost
                    );
                }
            }
            return d[s.Length, t.Length];
        }
    }
}
