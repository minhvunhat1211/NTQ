using Domain.Features.ProductService;
using Domain.Features.UserSevice;
using Domain.Models.DTO.ProductDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;

        }
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromForm]ProductCreateRequest request)
        {
            try
            {
                var result = await _productService.CreateAsync(request);
                if (result.IsSuccessed)
                {
                    return Ok(result);
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("getall")]
        public async Task<IActionResult> GetAll(int? pageSize, int? pageIndex, string? search)
        {
            try
            {
                var result = await _productService.GetAllAsync(pageSize, pageIndex, search);
                if (result.IsSuccessed)
                    return Ok(result);
                return BadRequest();
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
           
        }
        [HttpGet("getbyid")]
        public async Task<IActionResult> GetById(int productId)
        {
            try
            {
                var result = await _productService.GetByIdAsync(productId);
                if (result.IsSuccessed)
                    return Ok(result);
                return BadRequest();
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }

        }
        [HttpDelete("delete")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                var result = await _productService.DeleteAsync(id);
                if (result.IsSuccessed)
                    return Ok(result);
                return BadRequest();
            }
            catch (Exception)
            {

                return BadRequest($"Could not delete {id}");
            }
        }
        [HttpDelete("undelete")]
        public async Task<IActionResult> UnDelete(int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                var result = await _productService.UnDeleteAsync(id);
                if (result.IsSuccessed)
                    return Ok(result);
                return BadRequest();
            }
            catch (Exception)
            {

                return BadRequest($"Could not un delete {id}");
            }
        }
    }
}
