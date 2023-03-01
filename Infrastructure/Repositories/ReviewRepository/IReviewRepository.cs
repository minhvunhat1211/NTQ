using Infrastructure.Common.BaseReponsitory;
using Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.ReviewRepository
{
    public interface IReviewRepository : IRepositoryBase<Review>
    {
        /*Task<IEnumerable<Review>> GetAllReview(int? pageSize, int? pageIndex);*/
    }
}
