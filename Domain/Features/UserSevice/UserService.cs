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
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        public UserService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        public async Task<ApiResult<bool>> EditAsync(int id, UserEditResquest editRequest)
        {
            try
            {
                var findUserById = await _userRepository.GetById(id);
                findUserById.Email = editRequest.Email;
                findUserById.LastName = editRequest.LastName;
                findUserById.Firstname = editRequest.Firstname;
                findUserById.Role= editRequest.Role;    
                findUserById.UpdateAt = DateTime.Now;
                await _userRepository.UpdateAsync(findUserById);
                return new ApiSuccessResult<bool>();
            }
            catch (Exception)
            {

                throw;
            }
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
                var totalRow = await _userRepository.CountAsync();
                var query = await _userRepository.GetAll(pageSize, pageIndex);
                if (!string.IsNullOrEmpty(search))
                {
                    Expression< Func <Infrastructure.Entities.User, bool>> expression = x => x.Email.Contains(search);
                    query = await _userRepository.GetAll(pageSize, pageIndex, expression);
                    totalRow = await _userRepository.CountAsync(expression);
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
            var result = await _userRepository.GetByUserNameAsync(loginRequest.Email);
            string hashedPass = Domain.Common.HassPass.HashPass.Hash(loginRequest.PassWord);
            if (result == null)
            {
                return new ApiErrorResult<LoginResponse>("Sai tai khoan hoac mat khau");
            }
            if (result.PassWord != hashedPass)
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
            var authenKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
            var token = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
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

        public async Task<ApiResult<bool>> DeleteAsync(int id)
        {
            try
            {
                var findUserById = await _userRepository.GetById(id);
                findUserById.Status = 2;
                await _userRepository.UpdateAsync(findUserById);
                return new ApiSuccessResult<bool>(true);
            }
            catch (Exception)
            {

                throw;
            }
     
        }

        public async Task<ApiResult<SignUpResponse>> SignUpAsync(SignUpRequest signUpRequest)
        {
            var check = await _userRepository.GetByUserNameAsync(signUpRequest.Email);
            if (check != null) {
                return new ApiErrorResult<SignUpResponse>("Tai khoan da ton tai");
            }
            string hashedPass = Domain.Common.HassPass.HashPass.Hash(signUpRequest.PassWord);
            var newUser = new Infrastructure.Entities.User()
            {
                Email = signUpRequest.Email,
                Firstname = signUpRequest.Firstname,
                LastName = signUpRequest.LastName,
                CreateAt = DateTime.Now,
                Role = "USER",
                PassWord = hashedPass,
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
            await _userRepository.CreateAsync(newUser);
            return new ApiSuccessResult<SignUpResponse>(data);
        }

        public async Task<ApiResult<bool>> UnDeleteAsync(int id)
        {
            try
            {
                var findUserById = await _userRepository.GetById(id);
                findUserById.Status = 1;
                await _userRepository.UpdateAsync(findUserById);
                return new ApiSuccessResult<bool>(true);
            }
            catch (Exception)
            {

                throw;
            }
            
        }

        public async Task<ApiResult<UserDTO>> GetByIdAsync(int id)
        {
            try
            {
                var findById = await _userRepository.GetById(id);
                var user = new UserDTO()
                {
                    Id= findById.Id,
                    Email = findById.Email,
                    Firstname = findById.Firstname,
                    LastName = findById.LastName,
                    CreateAt = findById.CreateAt,
                    UpdateAt= findById.UpdateAt,
                    DeleteAt= findById.DeleteAt,
                    Role = findById.Role,
                    PassWord = findById.PassWord,
                    Status = findById.Status
                };
                return new ApiSuccessResult<UserDTO>(user);
            }
            catch (Exception)
            {

                throw;
            }
            throw new NotImplementedException();
        }
    }
}
