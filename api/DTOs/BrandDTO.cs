using api.Models;

namespace api.DTOs;

public class BrandDTO
{
        public int Id { get; set; }
        public string Name { get; set; }

        public List<Class> Classes { get; set; }
        public List<Sector> Sectors { get; set; }
}