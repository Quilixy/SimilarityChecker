namespace api.Models
{
    public class Brand
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<BrandClass> BrandClasses { get; set; }
        public ICollection<BrandSector> BrandSectors { get; set; }
    }
}