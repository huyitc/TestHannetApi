using AutoMapper;
using Hannet.Api.Core;
using Hannet.Model.Models;
using Hannet.Service;
using Hannet.ViewModel.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hannet.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EmployeeController : ApiBaseController<EmployeeController>
    {
        private readonly IEmployeeService _employeeService;
        private readonly IMapper _mapper;


        public EmployeeController(ILogger<EmployeeController> logger, IMapper mapper, IEmployeeService employeeService) : base(logger)
        {
            _employeeService = employeeService;
            _mapper = mapper;
        }
        /// <summary>
        /// Get danh sách nhân viên với UserId
        /// </summary>
        /// <returns></returns>
    /*    [HttpGet]
        [Route(nameof(GetAllEmployeeWithUserID))]
        public IActionResult GetAllEmployeeWithUserID(string UserId)
        {
            try
            {
                var result = _employeeService.GetListEmployeeByUserId(UserId);
                var mapping = _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeViewModels>>(result);
                return Ok(mapping);
            }
            catch (Exception ex)
            {

                return BadRequest(ex);
            }
        }*/
        /// <summary>
        /// Get danh sách nhân viên với thông tin user
        /// </summary>
        /// <returns></returns>
     /*   [HttpGet]
        [Route(nameof(GetAllEmployeeWithUser))]
        public async Task<IActionResult> GetAllEmployeeWithUser()
        {
            try
            {
                var devices = await _employeeService.GetAllWithUser();
                return Ok(devices);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }*/

        /// <summary>
        /// Get danh sách nhân viên ko truyền params
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "ViewEmPloyee")]
        [Route(nameof(GetAllNoParam))]
        public async Task<IActionResult> GetAllNoParam()
        {
            try
            {
                var model = await _employeeService.GetAll();
                var map = _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeViewModels>>(model);
                return Ok(map);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Get thiết nhân viên phân trang
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "ViewAllEmpByPaging")]
        [Route(nameof(GetAllByPaging))]
        public async Task<IActionResult> GetAllByPaging(int page = 0, int pageSize = 100, string keyword = null)
        {
            try
            {
                var model = await _employeeService.GetAll(keyword);
                int totalRow = 0;
                var data = model.OrderByDescending(x => x.EmployeeId).Skip(page * pageSize).Take(pageSize);
                var mapping = _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeViewModels>>(data);

                totalRow = model.Count();

                var paging = new PaginationSet<EmployeeViewModels>()
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
        /// Get thông tin nhân viên theo id
        /// </summary>
        /// <param name="id">Id nhân viên</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "ViewEmPById")]
        [Route("getbyid/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var model = await _employeeService.GetDetail(id);
                var mapping = _mapper.Map<Employee, EmployeeViewModels>(model);
                return Ok(mapping);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Thêm mới nhân viên
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "CreateEmployee")]
        [Route(nameof(Create))]
        public async Task<string[]> Create(EmployeeViewModels employee)
        {
            string[] respon = new string[2];
            respon[0] = "Tạo tành công ";
            respon[1] = "Nhân viên";
            if (ModelState.IsValid)
            {

                var em = new Employee();
                em.EmployeeName = employee.EmployeeName;
                em.EmployeeAge = employee.EmployeeAge;
                em.Sex = employee.Sex;

                if (await _employeeService.CheckContainsAsync(em) == false)
                {
                    await _employeeService.Add(em);
                }

            }

            return respon;
        }

        ///<summary></summary>
        ///Chỉnh sửa nhân viên
        ///<returns></returns>
        ///
        [HttpPut]
        [Authorize(Roles = "UpdateEmployee")]
        [Route(nameof(Update))]
        public async Task<IActionResult> Update(EmployeeViewModels employee)
        {
            if (ModelState.IsValid)
            {
                var mapping = _mapper.Map<EmployeeViewModels, Employee>(employee);
                try
                {
                    await _employeeService.Update(mapping);
                    return CreatedAtAction(nameof(Update), new { id = mapping.EmployeeId }, mapping);
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
        ///Xóa nhân viên
        ///<returns></returns>
        [HttpDelete]
        [Authorize(Roles = "DeleteEmployee")]
        [Route(nameof(Delete))]
        public async Task<IActionResult> Delete(int EmployeeId)
        {
            try
            {
                var result = await _employeeService.Delete(EmployeeId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }

    
}

