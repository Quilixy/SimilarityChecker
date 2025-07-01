namespace api.DTOs;

public class BrandSimilarityResultDTO
{
    public string BrandName        { get; set; }           
    public double SimilarityScore  { get; set; }
    public double LevenshteinScore { get; set; }
    public double PhoneticScore    { get; set; } 
    public double JaroWinklerScore { get; set; }
}