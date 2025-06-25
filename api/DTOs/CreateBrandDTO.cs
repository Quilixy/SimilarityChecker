using api.Models;

namespace api.DTOs;

public class CreateBrandDTO
{
    public class CreateBrandDto
    {
        public string Name { get; set; }
        public List<int> ClassIds  { get; set; }  
        public List<int> SectorIds { get; set; }   
    }
}