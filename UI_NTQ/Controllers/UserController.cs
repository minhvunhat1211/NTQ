
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Text;
using UI_NTQ.Common;
using UI_NTQ.Models;
using UI_NTQ.Models.UserModel;

namespace UI_NTQ.Controllers
{
    public class UserController : Controller
    {
        private readonly IConfiguration _configuration;
        public UserController(IConfiguration configuration)
        {
            _configuration= configuration;
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_configuration["BaseAddress"]);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var json = JsonConvert.SerializeObject(loginModel);
                    var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                    HttpResponseMessage Res = await client.PostAsync("User/login", httpContent);
                    if (Res.IsSuccessStatusCode)
                    {
                        var loginsucess = JsonConvert.DeserializeObject<ApiSuccessResult<LoginSuccessModel>>(Res.Content.ReadAsStringAsync().Result);
                        string token = loginsucess.ResultObj.AccessToken.ToString();
                        var handler = new JwtSecurityTokenHandler();
                        var jwtSecurityToken = handler.ReadJwtToken(token);
                        var Id = jwtSecurityToken.Claims.First(claim => claim.Type == "Id").Value;
                        var Email = jwtSecurityToken.Claims.First(claim => claim.Type == "Email").Value;
                        var Status = jwtSecurityToken.Claims.First(claim => claim.Type == "Status").Value;
                        var Role = jwtSecurityToken.Claims.First(claim => claim.Type == "Role").Value;
                        var ExpriseIn = jwtSecurityToken.Claims.First(claim => claim.Type == "ExpriseIn").Value;
                        HttpContext.Session.SetString("Role", Role);
                        CookieOptions cookieOptions = new CookieOptions();
                        cookieOptions.Expires = new DateTimeOffset(DateTime.Now.AddMinutes(Convert.ToInt32(ExpriseIn)));
                        HttpContext.Response.Cookies.Append("token_user", token, cookieOptions);
                        return RedirectToAction("MyProfile","HomePage");
                    }
                    else
                    {
                        var Response = Res.Content.ReadAsStringAsync().Result;
                        
                        var loginFail = JsonConvert.DeserializeObject<ApiErrorResult<FailResponseModel>>(Response);
                        string errorMessage = loginFail.Message.ToString();
                        ModelState.AddModelError("", errorMessage);
                        return View();
                    }
                }
            }
            return View();
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterRequestModel request)
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_configuration["BaseAddress"]);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var json = JsonConvert.SerializeObject(request);
                    var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                    HttpResponseMessage Res = await client.PostAsync("User/signup", httpContent);
                    if (Res.IsSuccessStatusCode)
                    {
                        
                        ModelState.AddModelError("", "Dang ki thanh cong");
                        return View();
                    }
                    else
                    {
                        var Response = Res.Content.ReadAsStringAsync().Result;
                        var registerFail = JsonConvert.DeserializeObject<ApiErrorResult<FailResponseModel>>(Response);
                        string errorMessage = registerFail.Message.ToString();
                        ModelState.AddModelError("", errorMessage);
                        return View();
                    }
                }
            }
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> ListUser(int pageSize, int pageIndex)
        {
            try
            {
                if (pageSize == 0)
                    pageSize = 10;
                if (pageIndex == 0)
                    pageIndex = 1;
                string token = HttpContext.Request.Cookies["token_user"].ToString();
                if (string.IsNullOrEmpty(token)) { return RedirectToAction("Login", "User"); };
                var client = new HttpClient();
                client.BaseAddress = new Uri(_configuration["BaseAddress"]);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = await client.GetAsync($"User/getall?pageSize={pageSize}&pageIndex={pageIndex}");
                if (!response.IsSuccessStatusCode)
                {
                    
                }
                var body = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<ApiSuccessResult<PagedResult<UserResponse>>>(body);
                return View(data.ResultObj);
            }
            catch (Exception)
            {

                throw;
            }
            return View();
        }

        public IActionResult Logout()
        {

            Response.Cookies.Delete("token_user");
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "User");
        }
    }
}
