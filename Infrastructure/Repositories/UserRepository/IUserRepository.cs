using Infrastructure.Common.BaseReponsitory;
using Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.UserRepository
{
    public interface IUserRepository : IRepositoryBase<User>
    {
        Task<User> GetByUserNameAsync(string email);
    }
}
