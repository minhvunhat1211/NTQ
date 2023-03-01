using Domain.Common;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Text;
using UI_NTQ.Models;

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
                        var Email = jwtSecurityToken.Claims.First(claim => claim.Type == "Email").Value;
                        var Status = jwtSecurityToken.Claims.First(claim => claim.Type == "Status").Value;
                        var Role = jwtSecurityToken.Claims.First(claim => claim.Type == "Role").Value;
                        var ExpriseIn = jwtSecurityToken.Claims.First(claim => claim.Type == "ExpriseIn").Value;
                        CookieOptions cookieOptions = new CookieOptions();
                        cookieOptions.Expires = new DateTimeOffset(DateTime.Now.AddMinutes(Convert.ToInt32(ExpriseIn)));
                        HttpContext.Response.Cookies.Append("token_user", token, cookieOptions);
                        if (Role == "ADMIN")
                        {
                            return RedirectToAction("Index","Home");
                        }
                        return RedirectToAction("", "");
                    }
                    else
                    {
                        var Response = Res.Content.ReadAsStringAsync().Result;
                        var loginFail = new FailResponseModel();
                        loginFail = JsonConvert.DeserializeObject<FailResponseModel>(Response);
                        string errorMessage = loginFail.Error.ToString();
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
    }
}
