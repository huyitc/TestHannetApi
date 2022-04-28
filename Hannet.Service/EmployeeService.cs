using Hannet.Data;
using Hannet.Data.Repository;
using Hannet.Model.MappingModels;
using Hannet.Model.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hannet.Service
{
    public interface IEmployeeService
    {
     /*   IEnumerable<Employee> GetListEmployeeByUserId(string UserId);
        Task<IEnumerable<EmployeeMapping>> GetAllWithUser();*/
        Task<IEnumerable<Employee>> GetAll(string keyword);
        Task<IEnumerable<Employee>> GetAll();
        Task<Employee> GetDetail(int EmployeeId);
        Task<Employee> Add(Employee employee);
        Task<Employee> Update(Employee employee);
        Task<Employee> Delete(int EmployeeId);
        Task<bool> CheckContainsAsync(Employee em);
    }
    public class EmployeeService : IEmployeeService
    {
        private IEmployeeRepository _employeeRepository;
        HannetDbContext _dbContext;

        public EmployeeService(IEmployeeRepository employeeRepository, HannetDbContext dbContext)
        {
            _dbContext = dbContext;
            _employeeRepository=employeeRepository;
        }

        public async Task<Employee> Add(Employee employee)
        {
            return await _employeeRepository.AddASync(employee);
        }

        public Task<bool> CheckContainsAsync(Employee em)
        {
            return _employeeRepository.CheckContainsAsync(x => x.EmployeeName == em.EmployeeName && x.EmployeeId != em.EmployeeId);
        }

        public async Task<Employee> Delete(int EmployeeId)
        {
           return await _employeeRepository.DeleteAsync(EmployeeId);
        }

        public async Task<IEnumerable<Employee>> GetAll(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
                return await _employeeRepository.GetAllAsync(x => x.EmployeeName.ToUpper().Contains(keyword.ToUpper()));
            else
                return await _employeeRepository.GetAllAsync();
        }

        public async Task<IEnumerable<Employee>> GetAll()
        {
            return await _employeeRepository.GetAllAsync();
        }

        /*  public async Task<IEnumerable<EmployeeMapping>> GetAllWithUser()
          {
              var query = from e in _dbContext.Employees
                          join u in _dbContext.AppUsers
                          on e.UserId equals u.UserId
                          select new {e,u};
              var data = await query.Select(x => new EmployeeMapping()
              {
                  EmployeeName= x.e.EmployeeName,
                  EmployeeAge = x.e.EmployeeAge,
                  Sex= x.e.Sex,
                  FirstName=x.u.FirstName,
                  LastName=x.u.LastName,
                  DateOfBirth=x.u.DateOfBirth,
                  UserId = x.u.UserId,
              }).ToListAsync();

              return data;
          }*/

        public async Task<Employee> GetDetail(int EmployeeId)
        {
            return await _employeeRepository.GetByIdAsync(EmployeeId);
        }

       /* public IEnumerable<Employee> GetListEmployeeByUserId(string UserId)
        {
            var query = from e in _dbContext.Employees
                        join us in _dbContext.AppUsers
                        on e.UserId equals us.UserId
                        where us.UserId == UserId
                        select e;
            return query;
        }
*/
        public async Task<Employee> Update(Employee employee)
        {
            if (await _employeeRepository.CheckContainsAsync(x => x.EmployeeName == employee.EmployeeName && x.EmployeeId != employee.EmployeeId))
                throw new Exception("Tên nhân viên!!!!");
            else
            {
                var update = await _employeeRepository.GetByIdAsync(employee.EmployeeId);
                update.EmployeeName = employee.EmployeeName;
                update.EmployeeAge = employee.EmployeeAge;
                update.Sex = employee.Sex;
                return await _employeeRepository.UpdateASync(update);
            }
        }
    }
}
