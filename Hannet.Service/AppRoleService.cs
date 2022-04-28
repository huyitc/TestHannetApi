using Hannet.Common.Exeptions;
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
    public interface IAppRoleService
    {
        Task<AppRole> GetDetail(string id);

        Task<IEnumerable<AppRole>> GetAll(string keyword);

        Task<IEnumerable<AppRole>> GetAll();

        Task<AppRole> Add(AppRole appRole);

        Task<AppRole> Update(AppRole appRole);

        Task<AppRole> Delete(string id);

        Task<bool> AddRolesToGroup(IEnumerable<AppRole_Group> roleGroups, int groupId);

        IEnumerable<AppRole> GetListRoleByGroupId(int groupId);
    }
    public class AppRoleService : IAppRoleService
    {
        private IAppRoleRepository _appRoleRepository;
        private IAppRoleGroupRepository _appRoleGroupRepository;

        public AppRoleService(IAppRoleRepository appRoleRepository, IAppRoleGroupRepository appRoleGroupRepository)
        {
            _appRoleRepository = appRoleRepository;
            _appRoleGroupRepository = appRoleGroupRepository;
        }

        public async Task<AppRole> Add(AppRole appRole)
        {
            if (await _appRoleRepository.CheckContainsAsync(r => r.Description == appRole.Description || r.Name == appRole.Name))
                throw new HannetExeptions("Tên không được trùng.");
            return await _appRoleRepository.AddASync(appRole);
        }

        public async Task<bool> AddRolesToGroup(IEnumerable<AppRole_Group> roleGroups, int groupId)
        {
            await _appRoleGroupRepository.DeleteMulti(x => x.GroupId == groupId);
            foreach (var roleGroup in roleGroups)
            {
                await _appRoleGroupRepository.AddASync(roleGroup);
            }
            return true;
        }

        public async Task<AppRole> Delete(string id)
        {
            return await _appRoleRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<AppRole>> GetAll(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
                return await _appRoleRepository.GetAllAsync(x => x.Name.ToUpper().Contains(keyword.ToUpper()) || x.Description.Contains(keyword.ToUpper()));
            else
                return await _appRoleRepository.GetAllAsync();
        }

        public async Task<IEnumerable<AppRole>> GetAll()
        {
            return await _appRoleRepository.GetAllAsync();
        }

        public async Task<AppRole> GetDetail(string id)
        {
            return await _appRoleRepository.GetSingleByConditionAsync(s => s.Id == id);
        }

        public IEnumerable<AppRole> GetListRoleByGroupId(int groupId)
        {
            return _appRoleRepository.GetListRoleByGroupId(groupId);
        }

        public async Task<AppRole> Update(AppRole appRole)
        {
            if (await _appRoleRepository.CheckContainsAsync(x => x.Description == appRole.Description && x.Name == appRole.Name && x.Id != appRole.Id))
                throw new HannetExeptions("Tên không được trùng.");
            return await _appRoleRepository.UpdateASync(appRole);
        }
    }
}
