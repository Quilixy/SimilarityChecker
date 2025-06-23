namespace api.Models
{
    public class Brand
    {
        public int    Id   { get; set; }
        public string Name { get; set; }
        
        public int   ClassId { get; set; }
        public Class Class   { get; set; }  
        
        public int    SectorId { get; set; }
        public Sector Sector   { get; set; } 
    }
}