using Domain.Common;
using Domain.Models.DTO.ProductDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Features.ProductService
{
    public interface IProductService
    {
        public Task<ApiResult<bool>> CreateAsync(ProductCreateRequest request);
        public Task<ApiResult<PagedResult<ProductDTO>>> GetAllAsync(int? pageSize, int? pageIndex, string? search);
        public Task<ApiResult<bool>> DeleteAsync(int id);
        public Task<ApiResult<bool>> UnDeleteAsync(int id);
    }
}
