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
    public class AppGroupController : ApiBaseController<AppGroupController>
    {
        private readonly IAppGroupService _appGroupService;
        private readonly IAppRoleService _appRoleService;
        private readonly IMapper _mapper;

        public AppGroupController(ILogger<AppGroupController> logger, IMapper mapper, IAppGroupService appGroupService, IAppRoleService appRoleService) : base(logger)
        {
            _appGroupService = appGroupService;
            _mapper = mapper;
            _appRoleService = appRoleService;
        }

        /// <summary>
        /// Get danh sách phân nhóm không truyền params
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "ViewAppGroup")]
        [Route(nameof(GetAll))]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var model = await _appGroupService.GetAll();
                var mapping = _mapper.Map<IEnumerable<AppGroup>, IEnumerable<AppGroupViewModel>>(model.OrderByDescending(x => x.GroupId));
                return Ok(mapping);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Get phân nhóm phân trang
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "ViewAppGroup")]
        [Route(nameof(GetAllByPaging))]
        public async Task<IActionResult> GetAllByPaging(int page = 0, int pageSize = 100, string keyword = null)
        {
            try
            {
                var model = await _appGroupService.GetAll(keyword);
                int totalRow = 0;
                var data = model.OrderByDescending(x => x.GroupId).Skip(page * pageSize).Take(pageSize);
                var mapping = _mapper.Map<IEnumerable<AppGroup>, IEnumerable<AppGroupViewModel>>(data);

                totalRow = model.Count();

                var paging = new PaginationSet<AppGroupViewModel>()
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
        /// Get thông tin phân nhóm theo id
        /// </summary>
        /// <param name="id">Id nhóm</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "ViewAppGroup")]
        [Route("getbyid/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var model = await _appGroupService.GetDetail(id);
                var mapping = _mapper.Map<AppGroup, AppGroupViewModel>(model);
                return Ok(mapping);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Thêm mới phân nhóm
        /// </summary>
        /// <param name="appGroupViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "CreateAppGroup")]
        [Route(nameof(Create))]
        //[Authorize(Roles = "CreateAppGroup")]
        public async Task<IActionResult> Create(AppGroupViewModel appGroupViewModel)
        {
            if (ModelState.IsValid)
            {
                var newAppGroup = new AppGroup();
                newAppGroup.Name = appGroupViewModel.NAME;
                newAppGroup.Description = appGroupViewModel.DESCRIPTION;
                try
                {
                    var appGroup = await _appGroupService.Add(newAppGroup);

                    //save group
                    var listRoleGroup = new List<AppRole_Group>();
                    if (appGroupViewModel.Roles != null)
                    {
                        foreach (var role in appGroupViewModel.Roles)
                        {
                            listRoleGroup.Add(new AppRole_Group()
                            {
                                GroupId = appGroup.GroupId,
                                RoleId = role.Id
                            });
                        }
                        await _appRoleService.AddRolesToGroup(listRoleGroup, appGroup.GroupId);
                    }


                    return CreatedAtAction(nameof(Create), new { id = newAppGroup.GroupId }, newAppGroup);
                }
                catch (HannetExeptions dex)
                {
                    return BadRequest(dex.Message);
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
        /// Chỉnh sửa phân nhóm
        /// </summary>
        /// <param name="appGroupViewModel"></param>
        /// <returns></returns>
        [HttpPut]
        [Authorize(Roles = "EditAppGroup")]
        [Route(nameof(Update))]
        //[Authorize(Roles = "CreateAppGroup")]
        public async Task<IActionResult> Update(AppGroupViewModel appGroupViewModel)
        {
            if (ModelState.IsValid)
            {
                var newAppGroup = new AppGroup();
                newAppGroup.UpdateGroup(appGroupViewModel);
                try
                {
                    var appGroup = await _appGroupService.Update(newAppGroup);

                    //save group
                    var listRoleGroup = new List<AppRole_Group>();
                    if (appGroupViewModel.Roles != null)
                    {
                        foreach (var role in appGroupViewModel.Roles)
                        {
                            listRoleGroup.Add(new AppRole_Group()
                            {
                                GroupId = appGroup.GroupId,
                                RoleId = role.Id
                            });
                        }
                    }

                    await _appRoleService.AddRolesToGroup(listRoleGroup, appGroup.GroupId);

                    return CreatedAtAction(nameof(Update), new { id = appGroupViewModel.ID }, appGroupViewModel);
                }
                catch (HannetExeptions dex)
                {
                    return BadRequest(dex.Message);
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
        /// Xóa 1 bản ghi
        /// </summary>
        /// <param name="id">id nhóm</param>
        /// <returns></returns>
        [HttpDelete]
        [Authorize(Roles = "DeleteAppGroup")]
        [Route(nameof(Delete))]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var appGroup = await _appGroupService.Delete(id);
                return Ok(appGroup);
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
        [Authorize(Roles = "DeleteAppGroup")]
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
                            await _appGroupService.Delete(item);
                            countSuccess++;
                        }
                        catch (Exception)
                        {
                            countError++;
                        }
                    }
                    result.Add("Xóa thành công: " + countSuccess);
                    result.Add("Lỗi" + countError);

                    return Ok(result);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }

        /// <summary>
        /// Lấy danh sách nhóm theo id User
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "ViewAppGroup")]
        [Route(nameof(GetListGroupByUser))]
        public IActionResult GetListGroupByUser(string userId)
        {
            try
            {
                var result = _appGroupService.GetListGroupByUserId(userId);
                var mapping = _mapper.Map<IEnumerable<AppGroup>, IEnumerable<AppGroupViewModel>>(result);
                return Ok(mapping);
            }
            catch (Exception ex)
            {

                return BadRequest(ex);
            }
        }
    }
}
