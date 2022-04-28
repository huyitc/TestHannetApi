using AutoMapper;
using Hannet.Api.Core;
using Hannet.Api.Extentsions;
using Hannet.Common.Exeptions;
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
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AppRoleController :  ApiBaseController<AppRoleController>
    {
        private readonly IAppRoleService _appRoleService;
        private readonly IMapper _mapper;

        public AppRoleController(ILogger<AppRoleController> logger, IAppRoleService appRoleService, IMapper mapper) : base(logger)
        {
            _appRoleService = appRoleService;
            _mapper = mapper;
        }

        /// <summary>
        /// Get danh sách quyền phân trang
        /// </summary>
        /// <param name="request"></param>
        /// <param name="page">Trang thứ</param>
        /// <param name="pageSize">Số bản ghi hiển thị trong 1 trang</param>
        /// <param name="filter">Từ khóa tìm kiếm</param>
        /// <returns></returns>
        [Route(nameof(GetListPaging))]
        // [Authorize(Roles = "ViewAppRole")]
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetListPaging(int page = 0, int pageSize = 100, string keyword = null)
        {
            try
            {
                int totalRow = 0;
                var model = await _appRoleService.GetAll(keyword);
                totalRow = model.Count();
                var paging = model.OrderByDescending(x => x.CreatedDate).Skip(page * pageSize).Take(pageSize);
                IEnumerable<AppRoleViewModel> modelVm = _mapper.Map<IEnumerable<AppRole>, IEnumerable<AppRoleViewModel>>(paging);

                PaginationSet<AppRoleViewModel> pagedSet = new PaginationSet<AppRoleViewModel>()
                {
                    Page = page,
                    TotalCount = totalRow,
                    TotalPages = (int)Math.Ceiling((decimal)totalRow / pageSize),
                    Items = modelVm
                };

                return Ok(pagedSet);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Get id role theo group id
        /// </summary>
        /// <param name="groupId">id group</param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]

        [Route(nameof(GetListByGroupId))]
        //     [Authorize(Roles = "ViewAppRole")]
        [AllowAnonymous]
        public IActionResult GetListByGroupId(int groupId)
        {
            try
            {
                var model = _appRoleService.GetListRoleByGroupId(groupId);
                IEnumerable<AppRoleViewModel> modelVm = _mapper.Map<IEnumerable<AppRole>, IEnumerable<AppRoleViewModel>>(model);
                return Ok(modelVm);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Get danh sách quyền
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Route(nameof(GetAll))]
        [HttpGet]
        //  [Authorize(Roles = "ViewAppRole")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var model = await _appRoleService.GetAll();
                IEnumerable<AppRoleViewModel> modelVm = _mapper.Map<IEnumerable<AppRole>, IEnumerable<AppRoleViewModel>>(model.OrderByDescending(x => x.CreatedDate));

                return Ok(modelVm);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Xem thông tin chi tiết quyền
        /// </summary>
        /// <param name="request"></param>
        /// <param name="id">id quyền cần xem</param>
        /// <returns></returns>
        [Route("detail/{id}")]
        [HttpGet]
        //    [Authorize(Roles = "ViewAppRole")]
        [AllowAnonymous]
        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest(nameof(id) + " không có giá trị.");
            }
            try
            {
                AppRole appRole = await _appRoleService.GetDetail(id);
                var modelVm = _mapper.Map<AppRole, AppRoleViewModel>(appRole);
                return Ok(appRole);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Thêm mới quyền
        /// </summary>
        /// <param name="request"></param>
        /// <param name="AppRoleViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route(nameof(Create))]
        //     [Authorize(Roles = "CreateAppRole")]
        [AllowAnonymous]
        public async Task<IActionResult> Create(AppRoleViewModel AppRoleViewModel)
        {
            if (ModelState.IsValid)
            {
                var newAppRole = new AppRole();
                newAppRole.UpdateApplicationRole(AppRoleViewModel, "add");
                newAppRole.CreatedDate = DateTime.Now;
                //newAppRole.Id = Guid.NewGuid().ToString();
                try
                {
                    var role = await _appRoleService.Add(newAppRole);
                    var result = _mapper.Map<AppRole, AppRoleViewModel>(role);
                    return CreatedAtAction(nameof(Create), new { id = result.Id }, result);
                }
                catch (HannetExeptions dex)
                {
                    return BadRequest(dex);
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

        /// <summary>
        /// Chỉnh sửa quyền
        /// </summary>
        /// <param name="AppRoleViewModel"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut]
        [Route(nameof(Update))]
        //    [Authorize(Roles = "UpdateAppRole")]
        [AllowAnonymous]
        public async Task<IActionResult> Update(AppRoleViewModel AppRoleViewModel)
        {
            if (ModelState.IsValid)
            {
                var appRole = await _appRoleService.GetDetail(AppRoleViewModel.Id);
                try
                {
                    appRole.UpdateApplicationRole(AppRoleViewModel, "update");
                    var result = await _appRoleService.Update(appRole);
                    return CreatedAtAction(nameof(Update), result);
                }
                catch (HannetExeptions dex)
                {
                    return BadRequest(dex);
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

        /// <summary>
        /// Xóa quyền
        /// </summary>
        /// <param name="request"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route(nameof(Delete))]
        //  [Authorize(Roles = "DeleteAppRole")]
        [AllowAnonymous]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var result = await _appRoleService.Delete(id);
                return CreatedAtAction(nameof(Delete), result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Xóa nhiều bản ghi
        /// </summary>
        /// <param name="request"></param>
        /// <param name="checkedList">List id cần xóa</param>
        /// <returns></returns>
        [Route("deletemulti")]
        [HttpDelete]
        // [Authorize(Roles = "DeleteAppRole")]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteMulti(string checkedList)
        {
            try
            {
                int countSuccess = 0;
                int countError = 0;
                List<string> result = new List<string>();
                var listItem = JsonConvert.DeserializeObject<List<string>>(checkedList);
                foreach (var item in listItem)
                {
                    try
                    {
                        await _appRoleService.Delete(item);
                        countSuccess++;
                    }
                    catch (Exception)
                    {
                        countError++;
                    }
                }
                result.Add("Xoá thành công: " + countSuccess);
                result.Add("Lỗi: " + countError);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

    }
}
