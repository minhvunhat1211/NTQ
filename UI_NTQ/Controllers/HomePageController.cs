
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Text;
using UI_NTQ.Common;
using UI_NTQ.Models;
using UI_NTQ.Models.UserModel;

namespace UI_NTQ.Controllers
{
    public class HomePageController : BaseController
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
                    Lastname = data.ResultObj.Lastname,
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
        [HttpPost]
        public async Task<IActionResult> Edit(UserEditRequest request)
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    string token = HttpContext.Request.Cookies["token_user"].ToString();
                    client.BaseAddress = new Uri(_configuration["BaseAddress"]);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var json = JsonConvert.SerializeObject(request);
                    var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                    HttpResponseMessage Res = await client.PutAsync("User/edit?id=" + request.Id.ToString(), httpContent);
                    if (Res.IsSuccessStatusCode)
                    {
                        return RedirectToAction("MyProfile", "HomePage");
                    }
                    else
                    {
                        var Response = Res.Content.ReadAsStringAsync().Result;
                        var fail = JsonConvert.DeserializeObject<ApiErrorResult<FailResponseModel>>(Response);
                        string errorMessage = fail.Message.ToString();
                        ModelState.AddModelError("", errorMessage);
                        return RedirectToAction("MyProfile", "HomePage");
                    }
                }
            }
            return RedirectToAction("MyProfile", "HomePage");
        }
    }
}
