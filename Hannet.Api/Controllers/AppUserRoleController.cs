using AutoMapper;
using Hannet.Api.Core;
using Hannet.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Hannet.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppUserRoleController : ApiBaseController<AppUserRoleController>
    {
        private readonly IAppUserRoleService _appUserRoleService;
        private readonly IMapper _mapper;

        public AppUserRoleController(ILogger<AppUserRoleController> logger, IAppUserRoleService appUserRoleService, IMapper mapper) : base(logger)
        {
            _appUserRoleService = appUserRoleService;
            _mapper = mapper;
        }

        /// <summary>
        /// Get danh sách roles
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route(nameof(GetUserRoleId))]
        [AllowAnonymous]
        public async Task<IActionResult> GetUserRoleId(string userId)
        {
            try
            {
                var model = await _appUserRoleService.GetAllUserRole(userId);

                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
