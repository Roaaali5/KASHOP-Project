using KASHOP.BLL.Service;
using KASHOP.DAL.DTO.Request;
using KASHOP.PL.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace KASHOP.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandsController : ControllerBase
    {
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly IBrandService _brandService;
        public BrandsController(IStringLocalizer<SharedResources> localizer, IBrandService brandService)
        {
            _localizer = localizer;
            _brandService = brandService;

        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var brands = await _brandService.GetAllBrandsAsynce();
            return Ok(new
            {
                data = brands
            });

        }



        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var brand = await _brandService.GetBrand(p => p.Id == id);
            if (brand == null) return NotFound();
            return Ok(new
            {
                data = brand
            });

        }
      

        [HttpPost("")]
        [Authorize]
        public async Task<IActionResult> Create([FromForm] BrandRequest request)
        {
            await _brandService.CreateBrand(request);
            return Ok();

        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _brandService.DeleteBrand(id);
            if (!deleted) return BadRequest();

            return Ok();

        }



    }
}
