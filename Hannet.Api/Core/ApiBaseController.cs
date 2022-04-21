using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Hannet.Api.Core
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiBaseController<T> : ControllerBase where T : class
    {
        private readonly ILogger<T> _logger;

        public ApiBaseController(ILogger<T> logger)
        {
            _logger = logger;
        }

        protected async Task<ActionResult> CreateAction(Func<Task<ActionResult>> function)
        {
            try
            {
                var res = await function.Invoke();
                //_logger.LogInformation(res);
                return Ok(res);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BadRequest(ex);
            }
        }
    }
}
