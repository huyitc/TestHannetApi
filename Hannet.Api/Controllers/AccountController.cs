using Hannet.Model.Models;
using Hannet.Service;
using Hannet.ViewModel.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Hannet.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IIdentityService _identityService;
        private readonly AppSettings _appSettings;

        public AccountController(UserManager<AppUser> userManager, IIdentityService identityService, IOptions<AppSettings> appSettings)
        {
            _userManager = userManager;
            _identityService = identityService;
            _appSettings = appSettings.Value;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route(nameof(Login))]
        public async Task<ActionResult<LoginResponseModel>> Login(LoginRequestModel model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);

            if (user == null)
            {
                return Unauthorized();
            }

            var passwordValid = await _userManager.CheckPasswordAsync(user, model.Password);
            List<string> roles = (List<string>)await _userManager.GetRolesAsync(user);
            if (!passwordValid)
            {
                return Unauthorized();
            }
            var token = this._identityService.GenerateJwtToken(
                user.Id,
                user.UserName,
                roles,
                this._appSettings.Secret);

            return new LoginResponseModel
            {
                UserId = user.Id,
                Token = "Bearer " + token,
                UserName = user.UserName,
                FullName = "Lee Huy",
                Email = user.Email,
                Image = user.Image,
            };
        }

        private void AddRolesToClaims(List<Claim> claims, IEnumerable<string> roles)
        {
            foreach (var role in roles)
            {
                var roleClaim = new Claim(ClaimTypes.Role, role);
                claims.Add(roleClaim);
            }
        }

    }
}
