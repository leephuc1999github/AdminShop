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
        private readonly IUserApiClient _userApiClient;
        private readonly IConfiguration _config;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        public LoginController(IUserApiClient userApiClient, IConfiguration config, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _userApiClient = userApiClient;
            _config = config;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            await _signInManager.SignOutAsync();
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Index(LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(ModelState);
            }
            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user == null) return null;

            var result = await _signInManager.PasswordSignInAsync(user, request.Password, request.RememberMe, true);
            if (!result.Succeeded)
            {
                return null;
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
        [HttpGet]
        [Route("gg")]
        public async Task<IActionResult> Google()
        {
            string url = Url.Action("GoogleResponse");
            var properties = _signInManager.ConfigureExternalAuthenticationProperties("Google", url);
            return new ChallengeResult("Google", properties);
        }


        [AllowAnonymous]
        public async Task<IActionResult> GoogleResponse()
        {
            HttpContext.Session.SetString("DefaultLanguageId", "en-US");
            ExternalLoginInfo info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
                return RedirectToAction("", "login");
            //var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider , info.ProviderKey , true);
            string[] userInfo =
            {
                info.Principal.FindFirst(ClaimTypes.Name).Value,
                info.Principal.FindFirst(ClaimTypes.Email).Value
            };
            var checker = await _userManager.FindByEmailAsync(userInfo[1]);

            AppUser user = new AppUser()
            {
                Email = userInfo[1],
                EmailConfirmed = true,
                UserName = userInfo[1],
                LastName = userInfo[0],
                FirstName = userInfo[0],
                Dob = DateTime.Now
            };

            if (checker != null)
            {
                user = checker;
            }
            else
            {
                var identityResult = await _userManager.CreateAsync(user);
                if (!identityResult.Succeeded)
                {
                    TempData["Failure"] = "Account Not Found";
                    return RedirectToAction("", "login");
                }
            }
            var roles = await _userManager.GetRolesAsync(user);
            var claims = new[]
            {
                        new Claim(ClaimTypes.Email,user.Email),
                        new Claim(ClaimTypes.GivenName,user.FirstName),
                        new Claim(ClaimTypes.Role, string.Join(";",roles)),
                        new Claim(ClaimTypes.Name, user.UserName)
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
            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddDays(5),
                IsPersistent = true
            };
            await _signInManager.SignInAsync(user, authProperties);
            return RedirectToAction("", "home");
        }
        private ClaimsPrincipal ValidationToken(string jwtToken)
        {
            IdentityModelEventSource.ShowPII = true;
            SecurityToken validationToken;
            TokenValidationParameters validationParameters = new TokenValidationParameters();
            validationParameters.ValidateLifetime = true;
            validationParameters.ValidAudience = _config["AuthSettings:Audience"];
            validationParameters.ValidIssuer = _config["AuthSettings:Issuer"];
            validationParameters.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["AuthSettings:Key"]));
            ClaimsPrincipal principal = new JwtSecurityTokenHandler().ValidateToken(jwtToken, validationParameters, out validationToken);
            return principal;
        }
    }
}
