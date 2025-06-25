namespace api.Models;

public class Sector
{
    public int Id { get; set; }
    public string Name { get; set; }

    public ICollection<BrandSector> BrandSectors { get; set; }
    public ICollection<SectorKeyword> SectorKeywords { get; set; }
}