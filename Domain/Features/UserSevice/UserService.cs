using Domain.Common;
using Domain.Models.DTO.UserDTO;
using Infrastructure.Entities;
using Infrastructure.Repositories.UserRepository;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
#nullable disable
namespace Domain.Features.UserSevice
{
    public class UserService : IUserService
    {
        private readonly IUserRepository USERREPOSITORY;
        private readonly IConfiguration CONFIGURATION;
        public UserService(IUserRepository userRepository, IConfiguration configuration)
        {
            USERREPOSITORY = userRepository;
            CONFIGURATION = configuration;
        }

        public async Task<ApiResult<PagedResult<UserDTO>>> GetAll(int? pageSize, int? pageIndex, string search)
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
                var totalRow = await USERREPOSITORY.CountAsync();
                var query = await USERREPOSITORY.GetAll(pageSize, pageIndex);
                if (!string.IsNullOrEmpty(search))
                {
                    Expression< Func <Infrastructure.Entities.User, bool>> expression = x => x.Email.Contains(search);
                    query = await USERREPOSITORY.GetAll(pageSize, pageIndex, expression);
                    totalRow = await USERREPOSITORY.CountAsync(expression);
                }
                //Paging
                var data = query.Skip((pageIndex.Value - 1) * pageSize.Value)
                    .Take(pageSize.Value)
                    .Select(x => new UserDTO()
                    {
                        Email = x.Email,
                        CreateAt = x.CreateAt.Value,
                        UpdateAt = x.UpdateAt.Value,
                        DeleteAt = x.DeleteAt.Value,
                        Firstname = x.Firstname,
                        Id = x.Id,
                        LastName = x.LastName,
                        Status = x.Status,
                    }).ToList();
                var pagedResult = new PagedResult<UserDTO>()
                {
                    TotalRecord = totalRow,
                    PageSize = pageSize.Value,
                    PageIndex = pageIndex.Value,
                    Items = data
                };
                if (pagedResult == null)
                {
                    return new ApiErrorResult<PagedResult<UserDTO>>("Khong co gi ca");
                }
                return new ApiSuccessResult<PagedResult<UserDTO>>(pagedResult);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<ApiResult<LoginResponse>> LoginAsync(LoginRequest loginRequest)
        {
            var result = await USERREPOSITORY.GetByUserNameAsync(loginRequest.Email);
            if (result == null)
            {
                return new ApiErrorResult<LoginResponse>("Sai tai khoan hoac mat khau");
            }
            var authClaim = new List<Claim>
            {
                new Claim("Email", result.Email),
                new Claim("Status", result.Status.ToString()),
                new Claim("Role", result.Role.ToString()),
                new Claim(JwtRegisteredClaimNames.Exp, "20"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };
            var authenKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(CONFIGURATION["JWT:Secret"]));
            var token = new JwtSecurityToken(
                    issuer: CONFIGURATION["JWT:ValidIssuer"],
                    audience: CONFIGURATION["JWT:ValidAudience"],
                    expires: DateTime.Now.AddMinutes(20),
                    claims: authClaim,
                    signingCredentials: new SigningCredentials(authenKey, SecurityAlgorithms.HmacSha256Signature)
                );
            string accessToken = new JwtSecurityTokenHandler().WriteToken(token);
            var TokenResult = new LoginResponse()
            {
                AccessToken = accessToken,
            };
            return new ApiSuccessResult<LoginResponse>(TokenResult);
        }
        public async Task<ApiResult<SignUpResponse>> SignUpAsync(SignUpRequest signUpRequest)
        {
            var check = await USERREPOSITORY.GetByUserNameAsync(signUpRequest.Email);
            if (check != null) {
                return new ApiErrorResult<SignUpResponse>("Tai khoan da ton tai");
            }
            var newUser = new Infrastructure.Entities.User()
            {
                Email = signUpRequest.Email,
                Firstname = signUpRequest.Firstname,
                LastName = signUpRequest.LastName,
                CreateAt = DateTime.Now,
                Role = "USER",
                PassWord = signUpRequest.PassWord,
                Status = 1

            };
            var data = new SignUpResponse()
            {
                Email = newUser.Email,
                Firstname = newUser.Firstname,
                LastName = newUser.LastName,
                CreateAt = newUser.CreateAt.Value,
                Role = newUser.Role,
                PassWord = newUser.PassWord,
                Status = newUser.Status
            };
            await USERREPOSITORY.CreateAsync(newUser);
            return new ApiSuccessResult<SignUpResponse>(data);
        }
    }
}
