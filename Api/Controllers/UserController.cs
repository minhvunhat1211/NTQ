
using Domain.Features.UserSevice;
using Domain.Models.DTO.UserDTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService ;

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
                var result = await _userService.LoginAsync(request);
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
                var result = await _userService.SignUpAsync(request);
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
                var result = await _userService.GetAll(pageSize,  pageIndex,  search);
                return Ok(result);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
        [HttpPut("edit")]
        public async Task<IActionResult> Edit(int id, UserEditResquest resquest)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                var result = await _userService.EditAsync(id, resquest);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
                throw;
            }
            throw new NotImplementedException();
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
                var result = await _userService.DeleteAsync(id);
                return Ok(result);
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
                var result = await _userService.UnDeleteAsync(id);
                return Ok(result);
            }
            catch (Exception)
            {

                return BadRequest($"Could not un delete {id}");
            }
            throw new NotImplementedException();
        }
        [HttpGet("getbyid")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var result = await _userService.GetByIdAsync(id);
                return Ok(result);
            }
            catch (Exception)
            {
                return BadRequest();
                throw;
            }
        }
    }
}
