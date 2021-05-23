using Microsoft.AspNetCore.Authentication;
using System;
using System.Threading.Tasks;
using view_model.Common;
using view_model.System.Users;

namespace admin_webapp.Services
{
    public interface IUserApiClient
    {
        Task<string> Authenticate(LoginRequest request);
        Task<ApiResult<PagedResult<UserVm>>> GetUsersPaging(GetUserPagingRequest request);
        Task<ApiResult<bool>> RegisterUser(RegisterRequest registerRequest);
        Task<ApiResult<UserVm>> GetById(Guid id);
        Task<ApiResult<bool>> UpdateUser(Guid id , UserUpdateRequest request);
        Task<ApiResult<bool>> Delete(Guid id);
        Task<ApiResult<bool>> RoleAssign(Guid id , RoleAssignRequest request);
    }
}
