using AutoMapper;
using Hannet.Api.Core;
using Hannet.Model.Models;
using Hannet.Service;
using Hannet.ViewModel.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hannet.Api.Controllers
{
    [Route("partner.hannet.api/[controller]")]
    [ApiController]
    public class DeviceController : ApiBaseController<DeviceController>
    {
        private readonly IDeviceService _deviceService;
        private readonly IMapper _mapper;


        public DeviceController(ILogger<DeviceController> logger, IMapper mapper, IDeviceService deviceService) : base(logger)
        {
            _deviceService = deviceService;
            _mapper = mapper;
        }
        /// <summary>
        /// Get danh sách thiết bị với thông tin vị trí
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route(nameof(GetAllDeviceWithPlace))]
        public async Task<IActionResult> GetAllDeviceWithPlace()
        {
            try
            {
                var devices = await _deviceService.GetAllWithPlace();
                return Ok(devices);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        /// <summary>
        /// Get danh sách thiết bị theo ID của vị trí
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAllDeviceByPlaceID/{PlaceId}")]
        public async Task<IActionResult> GetAllDeviceByPlaceID(int PlaceId)
        {
            var devices = await _deviceService.GetAllDeviceByPlaceId(PlaceId);
            if (devices == null)
            {
                return BadRequest();
            }
            return Ok(devices);
        }
        /// <summary>
        /// Get danh sách thiết bị không truyền params
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route(nameof(GetAllNoParam))]
        public async Task<IActionResult> GetAllNoParam()
        {
            try
            {
                var model = await _deviceService.GetAll();
                var map = _mapper.Map<IEnumerable<Device>, IEnumerable<DeviceViewModels>>(model);
                return Ok(map);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }


        /// <summary>
        /// Get thiết bị phân trang
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        [HttpGet]
        [Route(nameof(GetAllByPaging))]
        public async Task<IActionResult> GetAllByPaging(int page = 0, int pageSize = 100, string keyword = null)
        {
            try
            {
                var model = await _deviceService.GetAll(keyword);
                int totalRow = 0;
                var data = model.OrderByDescending(x => x.DeviceId).Skip(page * pageSize).Take(pageSize);
                var mapping = _mapper.Map<IEnumerable<Device>, IEnumerable<DeviceViewModels>>(data);

                totalRow = model.Count();

                var paging = new PaginationSet<DeviceViewModels>()
                {
                    Items = mapping,
                    Page = page,
                    TotalCount = totalRow,
                    TotalPages = (int)Math.Ceiling((decimal)totalRow / pageSize)
                };
                return Ok(paging);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        /// <summary>
        /// Get thông tin thiết bị theo id
        /// </summary>
        /// <param name="id">Id thiết bị</param>
        /// <returns></returns>
        [HttpGet]
        [Route("getbyid/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var model = await _deviceService.GetDetail(id);
                var mapping = _mapper.Map<Device, DeviceViewModels>(model);
                return Ok(mapping);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Thêm mới thiết bị
        /// <returns></returns>

        [HttpPost]
        [Route(nameof(Create))]
        public async Task<string[]> Create(DeviceViewModels device)
        {
            string[] respon = new string[2];
            respon[0] = "Tạo tành công ";
            respon[1] = "Thiết bị";
            if (ModelState.IsValid)
            {
               
                    var devi = new Device();
                    devi.DeviceName = device.DeviceName;
                    devi.PlaceId = device.PlaceId;
                    devi.CreatedBy = device.CreatedBy;
                    devi.CreatedDate = DateTime.Now;
                    devi.Status = device.Status;

                    if (await _deviceService.CheckContainsAsync(devi) == false)
                    {
                        await _deviceService.Add(devi);
                    }
                
                }
               
                return respon;
        }

        ///<summary></summary>
        ///Chỉnh sửa thiết bị
        ///<returns></returns>
        ///
        [HttpPut]
        [Route(nameof(Update))]

        public async Task<IActionResult> Update(DeviceViewModels device)
        {
            if (ModelState.IsValid)
            {
                var mapping = _mapper.Map<DeviceViewModels, Device>(device);
                try
                {
                    await _deviceService.Update(mapping);
                    return CreatedAtAction(nameof(Update), new { id = mapping.DeviceId }, mapping);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex);
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        ///<summary></summary>
        ///Xóa thiết bị
        ///<returns></returns>
        [HttpDelete]
        [Route(nameof(Delete))]
        public async Task<IActionResult> Delete(int DeviceId)
        {
            try
            {
                var result = await _deviceService.Delete(DeviceId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Xóa nhiều bản ghi
        /// </summary>
        /// <param name="checkedList">list id nhóm cần xóa</param>
        /// <returns></returns>
        [Route(nameof(DeleteMulti))]
        [HttpDelete]
        //[Authorize(Roles = "DeleteAppGroup")]
        public async Task<IActionResult> DeleteMulti(string checkedList)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                try
                {
                    int countSuccess = 0;
                    int countError = 0;
                    List<string> result = new List<string>();
                    var listItem = JsonConvert.DeserializeObject<List<int>>(checkedList);
                    //  var listItem = new JavaScriptSerializer().Deserialize<List<int>>(checkedList);
                    foreach (var item in listItem)
                    {
                        try
                        {
                            await _deviceService.Delete(item);
                            countSuccess++;
                        }
                        catch (Exception)
                        {
                            countError++;
                        }
                    }
                    result.Add("Xóa thành công: " + countSuccess + " bản ghi");
                    result.Add("Lỗi" + countError + " bản ghi");

                    return Ok(result);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }
    }
}
