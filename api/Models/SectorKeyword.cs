namespace api.Models;

public class SectorKeyword
{
    public int Id { get; set; }
    public string Keyword { get; set; }
    
    public int SectorId { get; set; }
    public Sector Sector { get; set; }
}