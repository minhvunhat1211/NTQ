using Infrastructure.Common.BaseReponsitory;
using Infrastructure.DbContext;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
#nullable disable
namespace Infrastructure.Repositories.UserRepository
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        private readonly NTQDbContext  _NTQDbContext;
        public UserRepository(NTQDbContext db) : base(db)
        {
            _NTQDbContext= db;
        }

        public async Task<User> GetByUserNameAsync(string email)
        {
            var result = await _NTQDbContext.Users.Where(x => x.Email == email).FirstOrDefaultAsync();
            return result;
        }
        
    }
}
