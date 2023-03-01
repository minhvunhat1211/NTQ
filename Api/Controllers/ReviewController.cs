using Domain.Features.ProductService;
using Domain.Features.ReviewRepository;
using Domain.Models.DTO.ReviewDTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewService;
        public ReviewController(IReviewService reviewService)
        {
            _reviewService= reviewService;
        }
        [HttpPost("create")]
        public async Task<IActionResult> Create(int userId, ReviewCreateRequest request)
        {
            try
            {
                var result = await _reviewService.CreateAsync(userId, request);
                if (result.IsSuccessed)
                    return Ok(result);
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
                throw;
            }
        }
        [HttpGet("getall")]
        public async Task<IActionResult> GetAll(int? pageSize, int? pageIndex, string? search)
        {
            try
            {
                var result = await _reviewService.GetAllAsync(pageSize, pageIndex, search);
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
                var result = await _reviewService.DeleteAsync(id);
                if (result.IsSuccessed)
                    return Ok(result);
                return BadRequest();
            }
            catch (Exception)
            {

                return BadRequest($"Could not delete {id}");
            }
        }
        [HttpPut("undelete")]
        public async Task<IActionResult> UnDelete(int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                var result = await _reviewService.UnDeleteAsync(id);
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
