using AutoMapper;
using Hannet.Api.Core;
using Hannet.Model.Models;
using Hannet.Service;
using Hannet.ViewModel.ViewModels;
using Microsoft.AspNetCore.Http;
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
    public class PlaceController : ApiBaseController<PlaceController>
    {
        private readonly IPlaceService _placeService;
        private readonly IMapper _mapper;


        public PlaceController(ILogger<PlaceController> logger, IMapper mapper, IPlaceService placeService) : base(logger)
        {
            _placeService = placeService;
            _mapper = mapper;
        }

        /// <summary>
        /// Get danh sách vị trí không truyền params
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route(nameof(GetAllNoParam))]
        public async Task<IActionResult> GetAllNoParam()
        {
            try
            {
                var model = await _placeService.GetAll();
                var map = _mapper.Map<IEnumerable<Place>, IEnumerable<PlaceViewModel>>(model);
                return Ok(map);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Get vị trí phân trang
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
                var model = await _placeService.GetAll(keyword);
                int totalRow = 0;
                var data = model.OrderByDescending(x => x.PlaceId).Skip(page * pageSize).Take(pageSize);
                var mapping = _mapper.Map<IEnumerable<Place>, IEnumerable<PlaceViewModel>>(data);

                totalRow = model.Count();

                var paging = new PaginationSet<PlaceViewModel>()
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
        /// Get thông tin vị trí theo id
        /// </summary>
        /// <param name="id">Id vị trí</param>
        /// <returns></returns>
        [HttpGet]
        [Route("getbyid/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var model = await _placeService.GetDetail(id);
                var mapping = _mapper.Map<Place, PlaceViewModel>(model);
                return Ok(mapping);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Thêm mới vị trí
        /// <returns></returns>

        [HttpPost]
        [Route(nameof(Create))]
        public async Task<string[]> Create(PlaceViewModel place)
        {
            string[] respon = new string[2];
            respon[0] = "Tạo thành công ";
            respon[1] = "Vị trí";
            if (ModelState.IsValid)
            {
                    var pla = new Place();
                pla.PlaceName = place.PlaceName ;
                pla.Address = place.Address;
                pla.CreatedBy = place.CreatedBy;
                pla.CreatedDate = DateTime.Now;
                pla.Status = place.Status;

                    if (await _placeService.CheckContainsAsync(pla) == false)
                    {
                        await _placeService.Add(pla);
                        
                    }
                    
                }
                return respon;
         }

        ///<summary></summary>
        ///Chỉnh sửa vị trí
        ///<returns></returns>
        ///
        [HttpPut]
        [Route(nameof(Update))]

        public async Task<IActionResult> Update(PlaceViewModel place)
        {
            if (ModelState.IsValid)
            {
                var mapping = _mapper.Map<PlaceViewModel, Place>(place);
                try
                {
                    await _placeService.Update(mapping);
                    return CreatedAtAction(nameof(Update), new { id = mapping.PlaceId }, mapping);
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
        ///Xóa vị trí
        ///<returns></returns>
        [HttpDelete]
        [Route(nameof(Delete))]
        public async Task<IActionResult> Delete(int PlaceId)
        {
            try
            {
                var result = await _placeService.Delete(PlaceId);
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
                            await _placeService.Delete(item);
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
