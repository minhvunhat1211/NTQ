using Infrastructure.Common.BaseReponsitory;
using Infrastructure.DbContext;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.ProductRepository
{
    public class ProductRepository : RepositoryBase<Product>, IProductRepository
    {
        private readonly NTQDbContext _NTQDbContext;
        public ProductRepository(NTQDbContext db) : base(db)
        {
            _NTQDbContext = db;
        }

        public async Task<IEnumerable<Product>> GetAllProduct(int? pageSize, int? pageIndex)
        {
            var query =  _NTQDbContext.Products.Include(x => x.ProductImgs).AsQueryable();
            var pageCount = query.Count();
            query = query.Skip((pageIndex.Value - 1) * pageSize.Value)
            .Take(pageSize.Value);
            return query.ToList();
        }
        public async Task<Product> GetByIdProduct(int? productId)
        {
            var query = _NTQDbContext.Products.Include(x => x.ProductImgs).Where(x => x.Id == productId).FirstOrDefault();
            return query;
        }
    }
}
