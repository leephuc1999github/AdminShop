using admin_webapp.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;
using view_model.Common;
using view_model.System.Users;

namespace admin_webapp.Controllers
{
    [Authorize(Roles = "admin")]
    public class CustomerController : BaseController
    {
        private readonly IUserApiClient _userApiClient;
        private readonly IRoleApiClient _roleApiClient;
        private readonly IConfiguration _config;
        public CustomerController(IUserApiClient userApiClient, IConfiguration config, IRoleApiClient roleApiClient)
        {
            _userApiClient = userApiClient;
            _config = config;
            _roleApiClient = roleApiClient;
        }

        public async Task<IActionResult> Index(string keyword, int pageIndex = 1, int pageSize = 10)
        {
            var sessions = HttpContext.Session.GetString("Token");
            var request = new GetUserPagingRequest()
            {
                BearerToken = sessions,
                Keyword = keyword,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
            var data = await _userApiClient.GetUsersPagingCustomer(request);
            if (keyword != null)
            {
                if(!keyword.Equals("")) data = data.Where(c => c.Account.Username.ToLower().Contains(keyword.ToLower())).ToList();
            }
            var result = new ApiResult<PagedResult<UserResponse>>()
            {
                IsSuccessed = true,
                Message = "",
                ResultObj = new PagedResult<UserResponse>()
                {
                    Items = data,
                    PageIndex = pageIndex,
                    PageSize = pageSize,
                    TotalRecord = data.Count
                }
            };
            return View(result);
        }

    }
}
