using api.DTOs;
using Microsoft.AspNetCore.Mvc;
using api.Models;
using api.Interfaces;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BrandController : ControllerBase
    {
        private readonly IBrandVariationService _brandVariationService;
        private readonly IBrandRepository _brandRepository;
        private readonly IBrandQueryService _brandQueryService;

        public BrandController(IBrandVariationService brandVariationService, IBrandRepository brandRepository, IBrandQueryService brandQueryService)
        {
            _brandVariationService = brandVariationService;
            _brandRepository = brandRepository;
            _brandQueryService = brandQueryService;

        }
        
        [HttpGet]
        public IActionResult GetAll()
        {
            var brands = _brandRepository.GetAll(); 
            return Ok(brands);
        }
       
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var brand = _brandRepository.GetById(id);
            if (brand == null)
                return NotFound();
            return Ok(brand);
        }
        
        [HttpPost]
        public IActionResult Create([FromBody] CreateBrandDTO.CreateBrandDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var brand = new Brand
            {
                Name = dto.Name,
                ClassId = dto.ClassId,
                SectorId = dto.SectorId
            };

            _brandVariationService.AddBrandWithVariations(brand);

            return CreatedAtAction(nameof(GetById), new { id = brand.Id }, brand);
        }

       
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Brand brand)
        {
            if (id != brand.Id)
                return BadRequest();

            var updated = _brandRepository.Update(brand);
            if (!updated)
                return NotFound();

            return NoContent();
        }

        
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var deleted = _brandRepository.Delete(id);
            if (!deleted)
                return NotFound();

            return NoContent();
        }
        
        [HttpPost("similar")]
        public IActionResult GetSimilarBrands([FromBody] BrandQueryDTO query)
        {
            var result = _brandQueryService.GetSimilarBrands(query.BrandName, query.ClassIds);
            return Ok(result);
        }
    }
}
