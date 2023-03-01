using Infrastructure.Common.BaseReponsitory;
using Infrastructure.DbContext;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.ReviewRepository
{
    public class ReviewRepository : RepositoryBase<Review>, IReviewRepository
    {
        private readonly NTQDbContext _NTQDbContext;
        public ReviewRepository(NTQDbContext db) : base(db)
        {
            _NTQDbContext = db;
        }
        /*public async Task<IEnumerable<Review>> GetAllReview(int? pageSize, int? pageIndex)
        {
            var query = _NTQDbContext.Reviews.Include(x => x.UserId).AsQueryable();
            var pageCount = query.Count();
            query = query.Skip((pageIndex.Value - 1) * pageSize.Value)
            .Take(pageSize.Value);
            return query.ToList();
        }*/
    }
}
