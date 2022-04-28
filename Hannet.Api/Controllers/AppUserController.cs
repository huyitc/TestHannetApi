using AutoMapper;
using Hannet.Api.Core;
using Hannet.Api.Extentsions;
using Hannet.Model.Models;
using Hannet.Service;
using Hannet.ViewModel.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
    public class AppUserController :  ApiBaseController<AppUserController>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IAppUserService _appUserService;
        private readonly IMapper _mapper;
        private readonly IAppGroupService _appGroupService;
        private readonly IAppRoleService _appRoleService;

        public AppUserController(ILogger<AppUserController> logger, UserManager<AppUser> userManager,
            IAppUserService appUserService, IMapper mapper, IAppGroupService appGroupService, IAppRoleService appRoleService) : base(logger)
        {
            _userManager = userManager;
            _appUserService = appUserService;
            _mapper = mapper;
            _appGroupService = appGroupService;
            _appRoleService = appRoleService;
        }

        [HttpGet]
        [Route(nameof(GetAll))]
        [Authorize(Roles = "ViewAccount")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var model = await _appUserService.GetAll();
                var mapping = _mapper.Map<IEnumerable<AppUser>, IEnumerable<AppUserViewModel>>(model.OrderByDescending(x => x.CreatedDate));
                return Ok(mapping);
            }
            catch (Exception dex)
            {
                return BadRequest(dex);
            }
        }

        [HttpGet]
        [Route(nameof(GetListPaging))]
        [Authorize(Roles = "ViewAccount")]
        public async Task<IActionResult> GetListPaging(int page = 0, int pageSize = 10, string keyword = null)
        {
            try
            {
                var model = await _appUserService.GetAll(keyword);
                int totalRow = 0;
                totalRow = model.Count();
                var paging = model.OrderByDescending(x => x.CreatedDate).Skip(page * pageSize).Take(pageSize);
                var mapping = _mapper.Map<IEnumerable<AppUser>, IEnumerable<AppUserViewModel>>(paging);
                var paginationSet = new PaginationSet<AppUserViewModel>()
                {
                    Items = mapping,
                    Page = page,
                    TotalCount = totalRow,
                    TotalPages = (int)Math.Ceiling((decimal)totalRow / pageSize)
                };

                return Ok(paginationSet);
            }
            catch (Exception dex)
            {
                return BadRequest(dex);
            }
        }


        [HttpGet]
        [Authorize(Roles = "ViewAccount")]
        [Route(nameof(GetById) + "/{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            try
            {
                var model = await _userManager.FindByIdAsync(id);
                var mapping = _mapper.Map<AppUser, AppUserViewModel>(model);
                return Ok(mapping);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPut]
        [Route(nameof(Update))]
        [Authorize(Roles = "UpdateAccount")]
        [AllowAnonymous]
        public async Task<IActionResult> Update([FromForm] AppUserViewModel applicationUserViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    AppUser appUser = await _userManager.FindByIdAsync(applicationUserViewModel.Id);
                    appUser.UpdateUser(applicationUserViewModel, "update");
                    if (!string.IsNullOrEmpty(applicationUserViewModel.Password))
                    {
                        appUser.PasswordHash = _userManager.PasswordHasher.HashPassword(appUser, applicationUserViewModel.Password);
                    }
                    var result = await _userManager.UpdateAsync(appUser);
                    if (result.Succeeded)
                    {
                        var listAppUserGroup = new List<AppUser_Group>();
                        var listRole = await _appRoleService.GetAll();
                        foreach (var role in listRole)
                        {
                            await _userManager.RemoveFromRoleAsync(appUser, role.Name);
                        }
                        if (applicationUserViewModel.Groups != null)
                        {
                            foreach (var group in applicationUserViewModel.Groups)
                            {
                                listAppUserGroup.Add(new AppUser_Group()
                                {
                                    GroupId = group.ID,
                                    UserId = applicationUserViewModel.Id
                                });

                                var listRole1 = _appRoleService.GetListRoleByGroupId(group.ID).ToList();

                                foreach (var role1 in listRole1)
                                {
                                    await _userManager.AddToRoleAsync(appUser, role1.Name);
                                }
                            }
                        }
                        await _appGroupService.AddUserToGroups(listAppUserGroup, applicationUserViewModel.Id);

                        return CreatedAtAction(nameof(Create), appUser);

                    }
                    else
                        return BadRequest(result.Errors);

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

        [HttpPost]
        [Route(nameof(Create))]
        [Authorize(Roles = "CreateAccount")]
        [AllowAnonymous]
        public async Task<IActionResult> Create(AppUserViewModel applicationUserViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    AppUser appUser = new AppUser();
                    appUser.UpdateUser(applicationUserViewModel, "add");
                    appUser.CreatedDate = DateTime.Now;
                    appUser.TimeLogin = DateTime.Now;
                    appUser.CountLogin = 0;
                    var result = await _userManager.CreateAsync(appUser, applicationUserViewModel.Password);
                    if (result.Succeeded)
                    {
                        var listAppUserGroup = new List<AppUser_Group>();
                        if (applicationUserViewModel.Groups != null)
                        {
                            foreach (var group in applicationUserViewModel.Groups)
                            {
                                listAppUserGroup.Add(new AppUser_Group()
                                {
                                    GroupId = group.ID,
                                    UserId = appUser.Id
                                });
                                //add role to user
                                var listRole = _appRoleService.GetListRoleByGroupId(group.ID).ToList();
                                foreach (var role in listRole)
                                {
                                    await _userManager.RemoveFromRoleAsync(appUser, role.Name);
                                    await _userManager.AddToRoleAsync(appUser, role.Name);
                                }
                            }
                            await _appGroupService.AddUserToGroups(listAppUserGroup, appUser.Id);
                        }

                        return CreatedAtAction(nameof(Create), appUser);
                    }
                    else
                    {
                        return BadRequest(result.Errors);
                    }
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
        /// Xóa tài khoản
        /// </summary>
        /// <param name="request"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route(nameof(Delete) + "/{id}")]
        [Authorize(Roles = "DeleteAccount")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var appUser = await _userManager.FindByIdAsync(id);
                var result = await _userManager.DeleteAsync(appUser);
                if (result.Succeeded)
                    return Ok(appUser);
                else
                    return BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

        }

        /// <summary>
        /// Xóa nhiều tài khoản
        /// </summary>
        /// <param name="checkedList"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route(nameof(DeleteMulti))]
        [Authorize(Roles = "DeleteAccount")]
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
                        var appUser = await _userManager.FindByIdAsync(item);
                        var res = await _userManager.DeleteAsync(appUser);
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
                return BadRequest(ex);
            }

        }
    }
}
