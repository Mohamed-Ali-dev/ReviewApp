using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ReviewApp.Dtos;
using ReviewApp.Models;
using ReviewApp.Services;
using ReviewApp.Utility;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly JwtOptions _jwtOptions;
        private readonly IAuthService _authService;

        public AccountController(UserManager<ApplicationUser> userManager, JwtOptions jwtOptions, IAuthService authService)
        {
            this._userManager = userManager;
            this._jwtOptions = jwtOptions;
            _authService = authService;
        }
        [HttpPost("register")]
        public async Task<IActionResult> RegisterNewUser(RegisterDto userDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.RegisterAsync(userDto);

            if (!result.IsAuthenticated) 
            return BadRequest(result.Message);

            SetRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiration);

            return Ok(result);
        }
        [HttpPost("login")]
        public async Task<IActionResult> LogIn(LogInDto logInDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.GetTokenAsync(logInDto);

            if (!result.IsAuthenticated)
                return BadRequest(result.Message);

            if (!string.IsNullOrEmpty(result.RefreshToken))
                SetRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiration);
            return Ok(result);
            }
        [HttpPost("addRole")]
        [Authorize(Roles = SD.Role_Admin)]

        public async Task<IActionResult> AddRoleAsync(AddRoleDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result =await _authService.AddRoleAsync(dto);

            if (!string.IsNullOrEmpty(result))
                return BadRequest(result);

            return Ok(dto);
        }
        [HttpGet("refreshToken")]
        //get the refreshtoken from the cookie
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            var result = await _authService.RefreshTokenAsync(refreshToken);
            if(!result.IsAuthenticated)
                return BadRequest(result);
            SetRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiration);

            return Ok(result);
        }
        [HttpPost("revokeToken")]
        public async Task<IActionResult> RevokeToken(RevokeTokenDto dto)
        {
            var token = dto.Token ?? Request.Cookies["refreshToken"];
            if(string.IsNullOrEmpty(token))
                return BadRequest("Token is required");
            var result =  await _authService.RevokeTokenAsync(token);

            if (!result)
                return BadRequest("Token is invalid");
            return Ok();
          
        }
        private void SetRefreshTokenInCookie(string refreshToken, DateTime expires)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = expires.ToLocalTime()
            };

            Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
        }
    }
}
