using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Drawing.Printing;
using System.Net.Http.Headers;
using System.Text;
using UI_NTQ.Common;
using UI_NTQ.Models;
using UI_NTQ.Models.ProductModel;

namespace UI_NTQ.Controllers
{
    public class ProductController : BaseController
    {
        private readonly IConfiguration _configuration;
        public ProductController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [HttpGet]
        public async Task<IActionResult> ListProduct(int pageSize, int pageIndex, string? optionFilter)
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
                var response = await client.GetAsync($"Product/getall?pageSize={pageSize}&pageIndex={pageIndex}");
                if (!response.IsSuccessStatusCode)
                {

                }
                var body = await response.Content.ReadAsStringAsync();

                var data = JsonConvert.DeserializeObject<ApiSuccessResult<PagedResult<ProductResponse<ProductImg>>>>(body);

                return View(data.ResultObj);

            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<IActionResult> Delete(int productId)
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
                    HttpResponseMessage Res = await client.DeleteAsync($"Product/delete?id={productId}");
                    if (Res.IsSuccessStatusCode)
                    {
                        return RedirectToAction("ListProduct");
                    }

                }
            }
            return RedirectToAction("ListProduct");
        }
        public async Task<IActionResult> UnDelete(int productId)
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
                    HttpResponseMessage Res = await client.DeleteAsync($"Product/undelete?id={productId}");
                    if (Res.IsSuccessStatusCode)
                    {
                        return RedirectToAction("ListProduct");
                    }
                }
            }
            return RedirectToAction("ListProduct");
        }
        [HttpGet]
        public async Task<IActionResult> ProductDetail(int productId, ProductResponse<ProductImg> product)
        {
            string token = HttpContext.Request.Cookies["token_user"].ToString();
            if (string.IsNullOrEmpty(token)) { return RedirectToAction("Login", "User"); };
            var client = new HttpClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response = await client.GetAsync($"Product/getbyid?productId={productId}");
            if (!response.IsSuccessStatusCode)
            {

            }
            var body = await response.Content.ReadAsStringAsync();

            var data = JsonConvert.DeserializeObject<ApiSuccessResult<ProductResponse<ProductImg>>>(body);

            return View(data.ResultObj);
        }

        public ActionResult AddProduct()
        {
            return View();
        }
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> AddProduct(AddProductRequest request)
        {
            try
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
                       /* var json = JsonConvert.SerializeObject(request);
                        var httpContent = new StringContent(json, Encoding.UTF8, "application/json");*/
                        var requestContent = new MultipartFormDataContent();

                        if (request.ProductImgs != null)
                        {
                            foreach (var file in request.ProductImgs)
                            {
                                var fileStream = file.OpenReadStream();
                                requestContent.Add(new StreamContent(fileStream), "ProductImgs", file.FileName.ToString());
                            }

                        }
                        requestContent.Add(new StringContent(request.Name.ToString()), "name");
                        requestContent.Add(new StringContent(request.Price.ToString()), "price");
                        requestContent.Add(new StringContent(request.ProductDetail.ToString()), "productDetail");
                        requestContent.Add(new StringContent(request.Slug.ToString()), "slug");
                        requestContent.Add(new StringContent(request.Trending.ToString()), "trending");
                        HttpResponseMessage Res = await client.PostAsync("Product/create", requestContent);
                        if (Res.IsSuccessStatusCode)
                        {

                            return View();
                        }
                        else
                        {
                            var Response = Res.Content.ReadAsStringAsync().Result;
                            var addFail = JsonConvert.DeserializeObject<ApiErrorResult<FailResponseModel>>(Response);
                            string errorMessage = addFail.Message.ToString();
                            ModelState.AddModelError("", errorMessage);
                            return View();
                        }
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            return View();
        }
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> EditProduct(int productId,PoductEditRequest request)
        {
            try
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
                        /* var json = JsonConvert.SerializeObject(request);
                         var httpContent = new StringContent(json, Encoding.UTF8, "application/json");*/
                        var requestContent = new MultipartFormDataContent();

                        if (request.ProductImgs != null)
                        {
                            foreach (var file in request.ProductImgs)
                            {
                                var fileStream = file.OpenReadStream();
                                requestContent.Add(new StreamContent(fileStream), "ProductImgs", file.FileName.ToString());
                            }

                        }
                        requestContent.Add(new StringContent(request.Name.ToString()), "name");
                        requestContent.Add(new StringContent(request.Price.ToString()), "price");
                        requestContent.Add(new StringContent(request.ProductDetail.ToString()), "productDetail");
                        requestContent.Add(new StringContent(request.Slug.ToString()), "slug");
                        requestContent.Add(new StringContent(request.Trending.ToString()), "trending");
                        HttpResponseMessage Res = await client.PutAsync("Product/edit?productId=" + request.Id, requestContent);
                        if (Res.IsSuccessStatusCode)
                        {
                            return RedirectToAction("ProductDetail", new {productId = request.Id});
                        }
                        else
                        {
                            var Response = Res.Content.ReadAsStringAsync().Result;
                            var addFail = JsonConvert.DeserializeObject<ApiErrorResult<FailResponseModel>>(Response);
                            string errorMessage = addFail.Message.ToString();
                            ModelState.AddModelError("", errorMessage);
                            return View();
                        }
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            return View();
        }
    }
}
