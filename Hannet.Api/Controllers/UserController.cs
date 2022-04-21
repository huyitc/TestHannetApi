using AutoMapper;
using Hannet.Api.Core;
using Hannet.Service;
using Hannet.ViewModel.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Hannet.Api.Controllers
{
    [Route("hannet.api.ai/[controller]")]
    [ApiController]
    public class UserController : ApiBaseController<UserController>
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;


        public UserController(ILogger<UserController> logger, IMapper mapper, IUserService userService) : base(logger)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [HttpPost("Authenticate")]
        [AllowAnonymous]
        public async Task<IActionResult> Authenticate([FromForm] LoginRequestModel models)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var resultToken = await _userService.Authenticate(models);
            if (string.IsNullOrEmpty(resultToken))
            {
                return BadRequest("Username or password is incorrect");

            }
            return Ok(resultToken);
        }

        [HttpPost("Register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromForm] LoginRegisterModel models)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _userService.Register(models);
            if (!result)
            {
                return BadRequest("Register is unsuccessful.");

            }
            return Ok();
        }
    }
}
