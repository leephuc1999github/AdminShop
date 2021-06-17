using admin_webapp.Services;
using library.Models.ESHOP;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using view_model.System.Users;

namespace admin_webapp.Controllers
{
    public class LoginController : Controller
    {
        private readonly IConfiguration _config;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        public LoginController( IConfiguration config, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _config = config;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                await _signInManager.SignOutAsync();
                return View();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.StackTrace);
                return RedirectToAction("", "error");
            }
            
        }


        [HttpPost]
        public async Task<IActionResult> Index(LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(ModelState);
            }
            try
            {
                var user = await _userManager.FindByNameAsync(request.UserName);
                if (user == null)
                {
                    TempData["Failure"] = "Username Or Password Is Incorrect";
                    return RedirectToAction("", "login");
                }

                var result = await _signInManager.PasswordSignInAsync(user, request.Password, request.RememberMe, true);
                if (!result.Succeeded)
                {
                    TempData["Failure"] = "Infor Incorrect";
                    return RedirectToAction("","login");
                }
                var roles = await _userManager.GetRolesAsync(user);
                var claims = new[]
                {
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.GivenName,user.FirstName),
                new Claim(ClaimTypes.Role, string.Join(";",roles)),
                new Claim(ClaimTypes.Name, request.UserName)
            };
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["AuthSettings:Key"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(_config["AuthSettings:Issuer"],
                    _config["AuthSettings:Audience"],
                    claims,
                    expires: DateTime.Now.AddDays(1),
                    signingCredentials: creds);

                string tokenJWT = new JwtSecurityTokenHandler().WriteToken(token);
                HttpContext.Session.SetString("Token", tokenJWT);
                HttpContext.Session.SetString("DefaultLanguageId", "en-US");
                var authProperties = new AuthenticationProperties
                {
                    ExpiresUtc = DateTimeOffset.UtcNow.AddDays(3),
                    IsPersistent = true
                };
                await _signInManager.SignInAsync(user, authProperties);
                return RedirectToAction("", "home");
            }
            catch(Exception e)
            {
                Console.WriteLine(e.StackTrace);
                return RedirectToAction("", "error");
            }
            
        }
    }
}
