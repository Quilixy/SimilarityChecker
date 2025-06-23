using Microsoft.AspNetCore.Mvc;
using api.Data;
using api.Dtos;
using api.Models;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClassController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ClassController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("add")]
        public IActionResult CreateClass([FromBody] CreateClassDto dto)
        {
            var classEntity = new Class
            {
                Id = dto.Id,
                Description = dto.Description
            };

            _context.Classes.Add(classEntity);
            _context.SaveChanges();
            return Ok("Sınıf eklendi.");
        }
    }
}