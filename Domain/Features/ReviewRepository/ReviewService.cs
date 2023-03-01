using Domain.Common;
using Domain.Common.FileStorage;
using Domain.Models.DTO.ProductDTO;
using Domain.Models.DTO.ReviewDTO;
using Infrastructure.Entities;
using Infrastructure.Repositories.ProductRepository;
using Infrastructure.Repositories.ReviewRepository;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Features.ReviewRepository
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _reviewRepository;

        public ReviewService(IReviewRepository reviewRepository)
        {
            _reviewRepository = reviewRepository;
        }

        public async Task<ApiResult<bool>> CreateAsync(int userId, ReviewCreateRequest request)
        {
            try
            {
                var newReview = new Infrastructure.Entities.Review()
                {
                    Title= request.Title,
                    Comment= request.Comment,
                    CreateAt= DateTime.Now,
                    UserId= userId,
                    Status= 1,
                };
                await _reviewRepository.CreateAsync(newReview);
                return new ApiSuccessResult<bool>(true);
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
                var findReviewById = await _reviewRepository.GetById(id);
                findReviewById.Status = 2;
                await _reviewRepository.UpdateAsync(findReviewById);
                return new ApiSuccessResult<bool>(true);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<ApiResult<PagedResult<ReviewDTO>>> GetAllAsync(int? pageSize, int? pageIndex, string? search)
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
                var totalRow = await _reviewRepository.CountAsync();
                var query = await _reviewRepository.GetAll(pageSize, pageIndex);
                if (!string.IsNullOrEmpty(search))
                {
                    Expression<Func<Review, bool>> expression = x => x.Title.Contains(search);
                    query = await _reviewRepository.GetAll(pageSize, pageIndex, expression);
                    totalRow = await _reviewRepository.CountAsync(expression);
                }
                var data = query
                    .Select(review => new ReviewDTO()
                    {
                        Title= review.Title,
                        Comment = review.Comment,
                        Status= review.Status,
                        UserID = review.UserId,
                        CreateAt = review.CreateAt,
                        DeleteAt = review.DeleteAt,
                        UpdateAt = review.UpdateAt,
                    }).ToList();
                var pagedResult = new PagedResult<ReviewDTO>()
                {
                    TotalRecord = totalRow,
                    PageSize = pageSize.Value,
                    PageIndex = pageIndex.Value,
                    Items = data
                };
                if (pagedResult == null)
                {
                    return new ApiErrorResult<PagedResult<ReviewDTO>>("Khong co gi ca");
                }
                return new ApiSuccessResult<PagedResult<ReviewDTO>>(pagedResult);
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
                var findReviewById = await _reviewRepository.GetById(id);
                findReviewById.Status = 1;
                await _reviewRepository.UpdateAsync(findReviewById);
                return new ApiSuccessResult<bool>(true);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
