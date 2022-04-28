using Hannet.Data.Repository;
using Hannet.Model.MappingModels;
using Hannet.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hannet.Service
{
    public interface IAppUserService
    {
        AppUser_Mapping GetUserName(string userName);

        IEnumerable<AppUser_Employee_Accout_Mapping> GetAllByMapping(string keyword);
        Task<IEnumerable<AppUser>> GetAll();
        Task<IEnumerable<AppUser>> GetAll(string keyword);
    }
    public class AppUserService : IAppUserService
    {
        private readonly IAppUserRepository _appUserRepository;

        public AppUserService(IAppUserRepository appUserRepository)
        {
            _appUserRepository = appUserRepository;
        }

        public async Task<IEnumerable<AppUser>> GetAll()
        {
            return await _appUserRepository.GetAllAsync();
        }

        public async Task<IEnumerable<AppUser>> GetAll(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
            {
                return await _appUserRepository.GetAllAsync(x => x.UserName.ToUpper().Contains(keyword));
            }
            else
                return await _appUserRepository.GetAllAsync();
        }

        public IEnumerable<AppUser_Employee_Accout_Mapping> GetAllByMapping(string keyword)
        {
            return _appUserRepository.GetAllByMapping(keyword);
        }

        public AppUser_Mapping GetUserName(string userName)
        {
            return _appUserRepository.GetUserName(userName);
        }
    }
}
