using AutoMapper;
using Hannet.Api.Core;
using Hannet.Model.MappingModels;
using Hannet.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Hannet.Api.Controllers
{
    [Route("hannet.api.ai/[controller]")]
    [ApiController]
    public class DeviceController : ApiBaseController<DeviceController>
    {
        private readonly IDeviceService _deviceService;
        private readonly IMapper _mapper;


        public DeviceController(ILogger<DeviceController> logger, IMapper mapper, IDeviceService apartmentService) : base(logger)
        {
            _deviceService = apartmentService;
            _mapper = mapper;
        }

        [HttpGet("getListDevice")]
        public async Task<IActionResult> getListDevice()
        {
            var devices = await _deviceService.GetAll();
            return Ok(devices);
        }

        [HttpGet("get-list-device-by-placeId/{PlaceId}")]
        public async Task<IActionResult> getListDeviceByPlaceId(int PlaceId)
        {
            var devices = await _deviceService.GetByPlaceID(PlaceId);
            if (devices == null)
            {
                return BadRequest("Device not found");
            }
            return Ok(devices);
        }

        [HttpPut("updateDevice")]
        public async Task<IActionResult> updateDevice([FromBody] DeviceMapping models)
        {
            var affectedResult = await _deviceService.Update(models);
            if (affectedResult == null)
            {
                return BadRequest();
            }
            return Ok();
        }

        [HttpPost("addDevice")]
        public async Task<IActionResult> addDevice([FromBody] DeviceMapping models)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var createdPlace = await _deviceService.CreateDevice(models);
                return CreatedAtAction(nameof(addDevice), new { id = createdPlace.DeviceId }, createdPlace);
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("deleteDevice")]
        public async Task<IActionResult> deleteDevice(int DevideId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _deviceService.Delete(DevideId);
            if (result == null)
                return BadRequest();
            return Ok();
        }
    }
}
