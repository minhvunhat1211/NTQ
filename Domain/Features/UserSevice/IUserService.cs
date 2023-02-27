﻿using Domain.Common;
using Domain.Models.DTO.UserDTO;
using Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Features.UserSevice
{
    public interface IUserService
    {
        public Task<ApiResult<LoginResponse>> LoginAsync(LoginRequest loginRequest);
        public Task<ApiResult<SignUpResponse>> SignUpAsync(SignUpRequest loginRequest);
        public Task<ApiResult<PagedResult<UserDTO>>> GetAll(int? pageSize, int? pageIndex, string? search);
    }
    /*public Task<ApiResult<UserEditRequest>> Edit(int id, UserEditRequest request);
    public Task<ApiResult<bool>> Remove(int id);
    public Task<ApiResult<bool>> UnRemove(int id);
    */
}
