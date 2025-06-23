namespace api.Models;

public class Class
{
    public int Id { get; set; }
    public string Description { get; set; }
    
    public ICollection<Brand> Brands { get; set; } 
}