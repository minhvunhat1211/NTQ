using Domain.Common;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using UI_NTQ.Models;

namespace UI_NTQ.Controllers
{
    public class HomePageController : Controller
    {
        private readonly IConfiguration _configuration;
        public HomePageController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IActionResult MyProfile()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> MyProfile(string userId)
        {
            try
            {
                string token = HttpContext.Request.Cookies["token_user"].ToString();
                if (string.IsNullOrEmpty(token)) { return RedirectToAction("Login", "User"); };
                var handler = new JwtSecurityTokenHandler();
                var jwtSecurityToken = handler.ReadJwtToken(token);
                var Id = jwtSecurityToken.Claims.First(claim => claim.Type == "Id").Value;
                userId = Id;
                var client = new HttpClient();
                client.BaseAddress = new Uri(_configuration["BaseAddress"]);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = await client.GetAsync($"User/getbyid?id={userId}");
                if (!response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index", "Home");
                }
                var body = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<ApiResult<UserResponse>>(body);
                var user = new UserResponse()
                {
                    Id = data.ResultObj.Id,
                    Firstname = data.ResultObj.Firstname,
                    LastName = data.ResultObj.LastName,
                    Email = data.ResultObj.Email,
                    PassWord = data.ResultObj.PassWord,
                    Status = data.ResultObj.Status,
                    CreateAt = data.ResultObj.CreateAt,
                    UpdateAt = data.ResultObj.UpdateAt,
                    DeleteAt = data.ResultObj.DeleteAt,
                    Role = data.ResultObj.Role,
                };
                return View(user);
            }
            catch (Exception)
            {

                throw;
            }
            
        }
    }
}
