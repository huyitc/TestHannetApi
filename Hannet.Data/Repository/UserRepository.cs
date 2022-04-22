using Hannet.Data.Infrastructure;
using Hannet.Model.Models;
using Hannet.ViewModel.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Hannet.Data.Repository
{
    public interface IUserRepository : IRepository<AppUser>
    {
        public Task<string> Authenticate(LoginRequestModel models);
        public Task<bool> Register(LoginRegisterModel models);
    }
    public class UserRepository : RepositoryBase<AppUser>, IUserRepository
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IConfiguration _config;
        private readonly HannetDbContext DbContext;
        public UserRepository(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,
          RoleManager<AppRole> roleManager, IConfiguration config, HannetDbContext dbContext) : base(dbContext)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _config = config;
            DbContext = dbContext;
        }

        public async Task<string> Authenticate(LoginRequestModel models)
        {
            var user = await _userManager.FindByNameAsync(models.UserName);
            if (user == null)
            {
                throw new Exception("Tài khoản không tồn tại");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, models.Password, true);
            if (!result.Succeeded)
            {
                throw new Exception("Đăng nhập thất bại");
            }
            var roles = await _userManager.GetRolesAsync(user);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.GivenName,user.FirstName),
                new Claim(ClaimTypes.Role, string.Join(";",roles)),
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Tokens:Issuer"],
                _config["Tokens:Issuer"],
                claims,
                expires: DateTime.Now.AddHours(3),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<bool> Register(LoginRegisterModel models)
        {
            var user = await _userManager.FindByNameAsync(models.UserName);

            if (await _userManager.FindByEmailAsync(models.Email) != null)
            {
                throw new Exception("Email đã tồn tại");
            }
            user = new AppUser()
            {
                FirstName = models.FirstName,
                LastName = models.LastName,
                PlaceId = models.PlaceId,
                UserName = models.UserName,
                Email = models.Email,
                PhoneNumber = models.PhoneNumber,
                DateOfBirth = models.DateOfBirth,
            };
            var result = await _userManager.CreateAsync(user, models.Password);
            if (result.Succeeded)
            {
                return true;
            }
            return false;
        }
    }
}
