using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using ReviewApp.Dtos;
using ReviewApp.Models;
using ReviewApp.Utility;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ReviewApp.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly JwtOptions jwtOptions;

        public AuthService(UserManager<ApplicationUser> userManager, JwtOptions jwtOptions, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            this.jwtOptions = jwtOptions;
            this._roleManager = roleManager;
        }


        public async Task<AuthModel> RegisterAsync(RegisterDto dto)
        {
            if (await _userManager.FindByEmailAsync(dto.Email) is not null || await _userManager.FindByEmailAsync(dto.UserName) is not null)
                return new AuthModel { Message = "Email is already registered!" };

            var user = new ApplicationUser
            {
                UserName = dto.UserName,
                Email = dto.Email,
                FirstName = dto.FirstName,
                lastName = dto.LastName,
                PhoneNumber = dto.PhoneNumber
            };

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
            {
                var errors = string.Empty;
                foreach (var error in result.Errors)
                {
                    errors += $"{error.Description}, ";

                }
                return new AuthModel { Message = errors };
            }
            await _userManager.AddToRoleAsync(user, SD.Role_User);
            var jwtSecurityToken = await CreateJwtToken(user);

            return new AuthModel
            {
                Email = user.Email,
                ExpiresOn = jwtSecurityToken.ValidTo,
                IsAuthenticated = true,
                Roles = new List<string> { SD.Role_User },
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                Username = user.UserName,

            };

        }
        public async Task<AuthModel> GetTokenAsync(LogInDto dto)
        {
            var authModel = new AuthModel();

            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, dto.Password))
            {
                authModel.Message = "Email Or Password is incorrect";
                return authModel;

            }

            var jwtSecurityToken = await CreateJwtToken(user);
            var rolesList = await _userManager.GetRolesAsync(user);

            authModel.IsAuthenticated = true;
            authModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            authModel.Email = user.Email;
            authModel.Username = user.UserName;
            authModel.ExpiresOn = jwtSecurityToken.ValidTo;
            authModel.Roles = rolesList.ToList();
            return authModel;
        }
        public async Task<string> AddRoleAsync(AddRoleDto dto)
        {
            var user = await _userManager.FindByIdAsync(dto.UserId);
            if (user is null || !await _roleManager.RoleExistsAsync(dto.Role))
                return "Invalid user Id Or Role";

            if (await _userManager.IsInRoleAsync(user, dto.Role))
                return "User is already assign to this role";

            var result = await _userManager.AddToRoleAsync(user, dto.Role);
            return result.Succeeded ?  String.Empty : "Something went wrong";
        
        } 

        public async Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();

            foreach (var role in roles)
            {
                roleClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            var claims = new[] {
                
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            }
            .Union(userClaims).Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SigningKey));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: jwtOptions.Issuer,
                audience:  jwtOptions.Audience,
                claims:claims,
                expires: DateTime.Now.AddDays(jwtOptions.LifeTime),
                signingCredentials:signingCredentials);
            return jwtSecurityToken;
        }

    
    }
}
