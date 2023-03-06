using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Options;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Linq.Expressions;
using System.Net.Http.Headers;
using System.Text;
using UI_NTQ.Common;
using UI_NTQ.Models;

namespace UI_NTQ.Controllers
{
    public class ListUserController : BaseController
    {
        private readonly IConfiguration _configuration;
        public ListUserController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [HttpGet]
        public async Task<IActionResult> ListUser(int pageSize, int pageIndex, string? optionFilter)
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
                
                switch (optionFilter)
                {
                    case "ADMIN":
                        var userFilterByRole = new PagedResult<UserResponse>()
                        {
                            PageSize = data.ResultObj.PageSize,
                            PageIndex = data.ResultObj.PageIndex,
                            TotalRecord = data.ResultObj.TotalRecord,
                            Items = data.ResultObj.Items.Where(x => x.Role == "ADMIN").ToList(),
                        };
                        return View(userFilterByRole);
                    case "Delete":
                        var userFilterByDelete = new PagedResult<UserResponse>()
                        {
                            PageSize = data.ResultObj.PageSize,
                            PageIndex = data.ResultObj.PageIndex,
                            TotalRecord = data.ResultObj.TotalRecord,
                            Items = data.ResultObj.Items.Where(x => x.Status == 2).ToList(),
                        };
                        return View(userFilterByDelete);
                    case "Active":
                        var userFilterByActive = new PagedResult<UserResponse>()
                        {
                            PageSize = data.ResultObj.PageSize,
                            PageIndex = data.ResultObj.PageIndex,
                            TotalRecord = data.ResultObj.TotalRecord,
                            Items = data.ResultObj.Items.Where(x => x.Status == 1).ToList(),
                        };
                        return View(userFilterByActive);
                    default:
                        return View(data.ResultObj);
                }
                
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<IActionResult> Delete(int userId)
        {
            string token = HttpContext.Request.Cookies["token_user"].ToString();
            if (string.IsNullOrEmpty(token)) { return RedirectToAction("Login", "User"); };
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_configuration["BaseAddress"]);
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage Res = await client.DeleteAsync($"User/delete?id={userId}");
                    if (Res.IsSuccessStatusCode)
                    {
                        return RedirectToAction("ListUser");
                    }
                    
                }
            }
            return RedirectToAction("ListUser");
        }
        public async Task<IActionResult> UnDelete(int userId)
        {
            string token = HttpContext.Request.Cookies["token_user"].ToString();
            if (string.IsNullOrEmpty(token)) { return RedirectToAction("Login", "User"); };
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_configuration["BaseAddress"]);
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage Res = await client.DeleteAsync($"User/undelete?id={userId}");
                    if (Res.IsSuccessStatusCode)
                    {
                        return RedirectToAction("ListUser");
                    }

                }
            }
            return RedirectToAction("ListUser");
        }
    }
}
