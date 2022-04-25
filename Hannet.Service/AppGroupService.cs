using Hannet.Common.Exeptions;
using Hannet.Data.Repository;
using Hannet.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hannet.Service
{
    public interface IAppGroupService
    {
        Task<AppGroup> GetDetail(int id);

        Task<AppGroup> GetByName(string name);

        Task<IEnumerable<AppGroup>> GetAll(string keyword);

        Task<IEnumerable<AppGroup>> GetAll();

        Task<AppGroup> Add(AppGroup appGroup);

        Task<AppGroup> Update(AppGroup appGroup);

        Task<AppGroup> Delete(int id);

        Task<bool> AddUserToGroups(IEnumerable<AppUser_Group> userGroups, string userId);

        IEnumerable<AppGroup> GetListGroupByUserId(string userId);

        IEnumerable<AppUser> GetListUserByGroupId(int groupId);
    }
    public class AppGroupService: IAppGroupService
    {
        private IAppGroupRepository _appGroupRepository;
        private IAppUserGroupRepository _appUserGroupRepository;

        public AppGroupService(IAppGroupRepository appGroupRepository,
           IAppUserGroupRepository appUserGroupRepository)
        {
            _appGroupRepository = appGroupRepository;
            _appUserGroupRepository = appUserGroupRepository;
        }

        public async Task<AppGroup> Add(AppGroup appGroup)
        {
            if (await _appGroupRepository.CheckContainsAsync(x => x.Name == appGroup.Name))
                throw new HannetExeptions("Tên không được trùng");
            return await _appGroupRepository.AddASync(appGroup);
        }

        public async Task<bool> AddUserToGroups(IEnumerable<AppUser_Group> userGroups, string userId)
        {
            await _appUserGroupRepository.DeleteMulti(x => x.UserId == userId);
            foreach (var userGroup in userGroups)
            {
                await _appUserGroupRepository.AddASync(userGroup);
            }
            return true;
        }

        public async Task<AppGroup> Delete(int id)
        {
            return await _appGroupRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<AppGroup>> GetAll(string keyword)
        {
            return await _appGroupRepository.GetAllAsync();
        }

        public async Task<IEnumerable<AppGroup>> GetAll()
        {
            return await _appGroupRepository.GetAllAsync();
        }

        public async Task<AppGroup> GetByName(string name)
        {
            return await _appGroupRepository.GetSingleByConditionAsync(x => x.Name == name);
        }

        public async Task<AppGroup> GetDetail(int id)
        {
            return await _appGroupRepository.GetByIdAsync(id);
        }

        public IEnumerable<AppGroup> GetListGroupByUserId(string userId)
        {
            return _appGroupRepository.GetListGroupByUserId(userId);
        }

        public IEnumerable<AppUser> GetListUserByGroupId(int groupId)
        {
            return _appGroupRepository.GetListUserByGroupId(groupId);
        }

        public async Task<AppGroup> Update(AppGroup appGroup)
        {
            if (await _appGroupRepository.CheckContainsAsync(x => x.Name == appGroup.Name && x.GroupId != appGroup.GroupId))
                throw new HannetExeptions("Tên không được trùng");
            return await _appGroupRepository.UpdateASync(appGroup);
        }
    }
}
