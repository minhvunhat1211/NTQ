using Domain.Common;
using Domain.Models.DTO.ProductDTO;
using Domain.Models.DTO.ReviewDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Features.ReviewRepository
{
    public interface IReviewService
    {
        public Task<ApiResult<bool>> CreateAsync(int userId,ReviewCreateRequest request);
        public Task<ApiResult<PagedResult<ReviewDTO>>> GetAllAsync(int? pageSize, int? pageIndex, string? search);
        public Task<ApiResult<bool>> DeleteAsync(int id);
        public Task<ApiResult<bool>> UnDeleteAsync(int id);
    }
}
