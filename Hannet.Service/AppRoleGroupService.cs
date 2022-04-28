using Hannet.Data.Repository;
using Hannet.Model.MappingModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hannet.Service
{
    public interface IAppRoleGroupService
    {
        IEnumerable<AppGroup_AppRole_Mapping> GetListByGroupId(int groupId);

        void DeleteMultipleAppRoleGroup(int groupId);

        void DeleteMultipleAppRoleGroupByRoleId(string roleId);
    }
    public class AppRoleGroupService : IAppRoleGroupService
    {
        private IAppRoleGroupRepository _appRoleGroupRepository;

        public AppRoleGroupService(IAppRoleGroupRepository appRoleGroupRepository)
        {
            _appRoleGroupRepository = appRoleGroupRepository;
        }

        public void DeleteMultipleAppRoleGroup(int groupId)
        {
            _appRoleGroupRepository.DeleteMulti(x => x.GroupId == groupId);
        }

        public void DeleteMultipleAppRoleGroupByRoleId(string roleId)
        {
            _appRoleGroupRepository.DeleteMulti(x => x.RoleId == roleId);
        }

        public IEnumerable<AppGroup_AppRole_Mapping> GetListByGroupId(int groupId)
        {
            return _appRoleGroupRepository.GetListByGroupId(groupId);
        }
    }
}
