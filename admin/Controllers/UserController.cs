using admin_webapp.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;
using view_model.Common;
using view_model.System.Users;

namespace admin_webapp.Controllers
{
    [Authorize(Roles = "admin")]
    public class UserController : BaseController
    {
        private readonly IUserApiClient _userApiClient;
        private readonly IRoleApiClient _roleApiClient;
        private readonly IConfiguration _config;
        public UserController(IUserApiClient userApiClient , IConfiguration config , IRoleApiClient roleApiClient)
        {
            _userApiClient = userApiClient;
            _config = config;
            _roleApiClient = roleApiClient;
        }

        public async Task<IActionResult> Index(string keyword,int pageIndex=1,int pageSize = 10)
        {
            try
            {
                var sessions = HttpContext.Session.GetString("Token");
                var request = new GetUserPagingRequest()
                {
                    BearerToken = sessions,
                    Keyword = keyword,
                    PageIndex = pageIndex,
                    PageSize = pageSize
                };
                var data = await _userApiClient.GetUsersPaging(request);
                return View(data);
            }
            catch(Exception e)
            {
                return RedirectToAction("", "error");
            }
            
        }


        
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("", "login");
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(RegisterRequest request)
        {
            if (!ModelState.IsValid)
                return View();

            var result = await _userApiClient.RegisterUser(request);
            
            if (result.IsSuccessed)
            {
                TempData["Success"] = "Create Success";
                return RedirectToAction("Index");
            }
            TempData["Failure"] = result.Message;
            ModelState.AddModelError("", result.Message);
            return View(request);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var result = await _userApiClient.GetById(id);
            if (result.IsSuccessed)
            {
                var user = result.ResultObj;
                var updateRequest = new UserUpdateRequest()
                {
                    Dob = user.Dob,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    PhoneNumber = user.PhoneNumber,
                    Id = id
                };
                return View(updateRequest);
            }
            TempData["Failure"] = "User's Information may be wrong";
            return RedirectToAction("", "user");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UserUpdateRequest request)
        {
            if (!ModelState.IsValid)
                return View();

            var result = await _userApiClient.UpdateUser(request.Id, request);
            if (result.IsSuccessed)
            {
                TempData["Success"] = "Update Success";
                return RedirectToAction("","user");
            }
            TempData["Failure"] = result.Message;
            ModelState.AddModelError("", result.Message);
            return View(request);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _userApiClient.Delete(id);
            if (!result.IsSuccessed)
            {
                TempData["Failure"] = result.Message;
                ModelState.AddModelError("", result.Message);
            }
            TempData["Success"] = "Delete success";
            return RedirectToAction("","user");
        }

        [HttpGet]
        public async Task<IActionResult> RoleAssign(Guid id)
        {
            var roleAssignRequest = await GetRoleAssignRequest(id);
            if(roleAssignRequest == null)
            {
                TempData["Failure"] = "User's Infomation may be wrong";
                return RedirectToAction("", "user");
            }
            return View(roleAssignRequest);
        }

        [HttpPost]
        public async Task<IActionResult> RoleAssign(RoleAssignRequest request)
        {
            if (!ModelState.IsValid)
                return View();

            var result = await _userApiClient.RoleAssign(request.Id, request);

            if (result.IsSuccessed)
            {
                TempData["Success"] = "Update Success";
                return RedirectToAction("","user");
            }
            TempData["Failure"] = result.Message;
            ModelState.AddModelError("", result.Message);
            var roleAssignRequest = await GetRoleAssignRequest(request.Id);
            if(roleAssignRequest == null)
            {
                TempData["Failure"] = "User's Infomation may be wrong";
                return RedirectToAction("","user");
            }
            return View(roleAssignRequest);
        }


        private async Task<RoleAssignRequest> GetRoleAssignRequest(Guid id)
        {
            var userObj = await _userApiClient.GetById(id);
            if (!userObj.IsSuccessed) return null;
            var roleObj = await _roleApiClient.GetAll();
            var roleAssignRequest = new RoleAssignRequest() { Id = id };
            foreach (var role in roleObj.ResultObj)
            {
                roleAssignRequest.Roles.Add(new SelectItem()
                {
                    Id = role.Id.ToString(),
                    Name = role.Name,
                    Selected = userObj.ResultObj.Roles.Contains(role.Name)
                });
            }
            return roleAssignRequest;
        }
    }
}
