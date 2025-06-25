namespace api.Models;

public class BrandClass
{
    public int BrandId { get; set; }
    public Brand Brand { get; set; }

    public int ClassId { get; set; }
    public Class Class { get; set; }
}