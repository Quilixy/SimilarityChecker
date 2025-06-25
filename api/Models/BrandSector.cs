namespace api.Models;

public class BrandSector
{
    public int BrandId { get; set; }
    public Brand Brand { get; set; }

    public int SectorId { get; set; }
    public Sector Sector { get; set; }
}