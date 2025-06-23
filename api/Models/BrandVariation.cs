namespace api.Models;

public class BrandVariation
{
    public int    Id        { get; set; }
    public string Variation { get; set; }

    public int   BrandId { get; set; }
    public Brand Brand   { get; set; }
}