namespace api.DTOs;

public class CreateBrandDTO
{
    public class CreateBrandDto
    {
        public string Name  { get; set; }
        public int ClassId  { get; set; }
        public int SectorId { get; set; }
    }
}