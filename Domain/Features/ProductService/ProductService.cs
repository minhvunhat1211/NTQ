using Domain.Common;
using Domain.Common.FileStorage;
using Domain.Common.HassPass;
using Domain.Models.DTO.ProductDTO;
using Domain.Models.DTO.UserDTO;
using Infrastructure.Entities;
using Infrastructure.Repositories.ProductRepository;
using Infrastructure.Repositories.UserRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Features.ProductService
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IConfiguration _configuration;
        private readonly IFileStorageService _storageService;

        public ProductService(IProductRepository productRepository, IFileStorageService fileStorageService)
        {
            _productRepository = productRepository;
            _storageService = fileStorageService;
        }
        private async Task<string> SaveFile(IFormFile file)
        {
            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
            await _storageService.SaveFileAsync(file.OpenReadStream(), fileName);
            return fileName;
        }
        public async Task<ApiResult<bool>> CreateAsync(ProductCreateRequest request)
        {
            try
            {
                var newProduct = new Infrastructure.Entities.Product()
                {
                    Name = request.Name,
                    Slug = request.Slug,
                    ProductDetail= request.ProductDetail,
                    Price = request.Price,
                    Trending = request.Trending,
                    CreateAt = DateTime.Now,
                    Status = 1
                };
                var tempImg = new List<ProductImg>();
                if (request.ProductImgs != null)
                {
                    foreach (var img in request.ProductImgs)
                    {
                        var productImage = new ProductImg()
                        {
                            Caption = newProduct.Name,
                            FileSize = img.Length,
                            ImagePath = await this.SaveFile(img),
                            IsDefault = true,
                        };
                        tempImg.Add(productImage);
                    }
                    newProduct.ProductImgs = tempImg;
                }
                await _productRepository.CreateAsync(newProduct);
                return new ApiSuccessResult<bool>(true);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<ApiResult<PagedResult<ProductDTO>>> GetAllAsync(int? pageSize, int? pageIndex, string? search)
        {
            try
            {
                if (pageSize != null)
                {
                    pageSize = pageSize.Value;
                }
                if (pageIndex != null)
                {
                    pageIndex = pageIndex.Value;
                }
                var totalRow = await _productRepository.CountAsync();
                var query = await _productRepository.GetAllProduct(pageSize, pageIndex);
                if (!string.IsNullOrEmpty(search))
                {
                    Expression<Func<Product, bool>> expression = x => x.Name.Contains(search);
                    query = await _productRepository.GetAll(pageSize, pageIndex, expression);
                    totalRow = await _productRepository.CountAsync(expression);
                }
                var data = query
                    .Select(product => new ProductDTO()
                    {
                        Name = product.Name,
                        Price = product.Price,
                        ProductDetail = product.ProductDetail,
                        Slug = product.Slug,
                        Trending = product.Trending,
                        Id = product.Id,
                        ProductImgs = product.ProductImgs,
                        CreateAt = product.CreateAt,
                        DeleteAt = product.DeleteAt,
                        UpdateAt = product.UpdateAt,
                        Status= product.Status,
                    }).ToList();
                var pagedResult = new PagedResult<ProductDTO>()
                {
                    TotalRecord = totalRow,
                    PageSize = pageSize.Value,
                    PageIndex = pageIndex.Value,
                    Items = data
                };
                if (pagedResult == null)
                {
                    return new ApiErrorResult<PagedResult<ProductDTO>>("Khong co gi ca");
                }
                return new ApiSuccessResult<PagedResult<ProductDTO>>(pagedResult);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<ApiResult<bool>> DeleteAsync(int id)
        {
            try
            {
                var findProductById = await _productRepository.GetById(id);
                findProductById.Status = 2;
                await _productRepository.UpdateAsync(findProductById);
                return new ApiSuccessResult<bool>(true);
            }
            catch (Exception)
            {

                throw;
            }

        }

        public async Task<ApiResult<bool>> UnDeleteAsync(int id)
        {
            try
            {
                var findProductById = await _productRepository.GetById(id);
                findProductById.Status = 1;
                await _productRepository.UpdateAsync(findProductById);
                return new ApiSuccessResult<bool>(true);
            }
            catch (Exception)
            {

                throw;
            }
            throw new NotImplementedException();
        }

        public async Task<ApiResult<ProductDTO>> GetByIdAsync(int id)
        {
            try
            {
                var findById = await _productRepository.GetByIdProduct(id);
                var product = new ProductDTO()
                {
                    Id = findById.Id,
                    Name = findById.Name,
                    Slug = findById.Slug,
                    ProductDetail = findById.ProductDetail,
                    CreateAt = findById.CreateAt,
                    UpdateAt = findById.UpdateAt,
                    DeleteAt = findById.DeleteAt,
                    Price = findById.Price,
                    Trending = findById.Trending,
                    Status = findById.Status,
                    ProductImgs= findById.ProductImgs,
                };
                return new ApiSuccessResult<ProductDTO>(product);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
