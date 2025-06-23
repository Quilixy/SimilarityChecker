using Microsoft.AspNetCore.Mvc;
using api.Data;
using api.Dtos;
using api.Models;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SectorController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SectorController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("add")]
        public IActionResult CreateSector([FromBody] CreateSectorDto dto)
        {
            var sector = new Sector { Name = dto.Name };
            _context.Sectors.Add(sector);
            _context.SaveChanges();
            return Ok("Sektör eklendi.");
        }
    }
}