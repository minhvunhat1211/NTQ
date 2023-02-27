
using Domain.Features.UserSevice;
using Domain.Models.DTO.UserDTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService USERSERVICE;
        public UserController(IUserService userService)
        {
             USERSERVICE = userService ;

        }
        [HttpPost("login")]
        public async Task<IActionResult> Login( LoginRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                var result = await USERSERVICE.LoginAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody]SignUpRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                var result = await USERSERVICE.SignUpAsync(request);
                return Ok(result);
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
                var result = await USERSERVICE.GetAll(pageSize,  pageIndex,  search);
                return Ok(result);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

    }
}
