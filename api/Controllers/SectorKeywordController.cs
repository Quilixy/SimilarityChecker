using Microsoft.AspNetCore.Mvc;
using api.Data;
using api.Dtos;
using api.Models;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SectorKeywordController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SectorKeywordController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("add")]
        public IActionResult CreateSectorKeyword([FromBody] CreateSectorKeywordDto dto)
        {
            var keyword = new SectorKeyword
            {
                Keyword = dto.Keyword,
                SectorId = dto.SectorId
            };

            _context.SectorKeywords.Add(keyword);
            _context.SaveChanges();
            return Ok("Sektör anahtar kelimesi eklendi.");
        }
    }
}