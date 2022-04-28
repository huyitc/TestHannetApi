using Hannet.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hannet.Service
{
    public interface IAppUserRoleService
    {
        Task<List<string>> GetAllUserRole(string userId);

        void DeleteMultipleAppUserRoleByRoleId(string roleId);

        List<string> GetListRoleCheck(string userName);
    }
    public class AppUserRoleService : IAppUserRoleService
    {
        private readonly IAppUserRoleRepository _appUserRoleRepository;

        public AppUserRoleService(IAppUserRoleRepository appUserRoleRepository)
        {
            _appUserRoleRepository = appUserRoleRepository;
        }

        public void DeleteMultipleAppUserRoleByRoleId(string roleId)
        {
            _appUserRoleRepository.DeleteMulti(x => x.RoleId == roleId);
        }

        public async Task<List<string>> GetAllUserRole(string userId)
        {
            return await _appUserRoleRepository.GetListRole(userId);
        }

        public List<string> GetListRoleCheck(string userName)
        {
            return _appUserRoleRepository.GetListRoleCheck(userName);
        }
    }
}
