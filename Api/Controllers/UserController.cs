
using Domain.Features.UserSevice;
using Domain.Models.DTO.UserDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService ;

        }
        [AllowAnonymous]
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
                if (result.IsSuccessed)
                    return Ok(result);
                return BadRequest(result);
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
                if (result.IsSuccessed)
                    return Ok(result);
                return BadRequest(result);
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
                if (result.IsSuccessed)
                    return Ok(result);
                return BadRequest(result);
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
                if (result.IsSuccessed)
                    return Ok(result);
                return BadRequest(result);
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
                if (result.IsSuccessed)
                    return Ok(result);
                return BadRequest(result);
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
                var result = await _userService.UnDeleteAsync(id);
                if (result.IsSuccessed)
                    return Ok(result);
                return BadRequest(result);
            }
            catch (Exception)
            {

                return BadRequest($"Could not un delete {id}");
            }
        }
        [HttpGet("getbyid")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var result = await _userService.GetByIdAsync(id);
                if(result.IsSuccessed)
                    return Ok(result);
                return BadRequest(result);
            }
            catch (Exception)
            {
                return BadRequest();
                throw;
            }
        }
    }
}
