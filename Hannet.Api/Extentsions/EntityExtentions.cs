using Hannet.Model.Models;
using Hannet.ViewModel.ViewModels;
using System;

namespace Hannet.Api.Extentsions
{
    public static class EntityExtentions
    {
        public static void UpdateGroup(this AppGroup group, AppGroupViewModel groupViewModel)
        {
            group.GroupId = groupViewModel.ID;
            group.Name = groupViewModel.NAME;
            group.Description = groupViewModel.DESCRIPTION;
        }
        public static void UpdateUser(this AppUser appUser, AppUserViewModel appUserViewModel, string action = "add")
        {
            if (action == "add")
                appUser.Id = Guid.NewGuid().ToString();
            else
            {
                appUser.Id = appUserViewModel.Id;
            }
            appUser.EmployeeId = appUserViewModel.EmployeeId;
            appUser.Email = appUserViewModel.Email;
            appUser.UserName = appUserViewModel.UserName;
            appUser.PhoneNumber = appUserViewModel.PhoneNumber;
            appUser.TimeLogin = appUserViewModel.TimeLogin;
            appUser.CountLogin = appUserViewModel.CountLogin;
            appUser.NormalizedUserName = appUserViewModel.UserName;
            appUser.CreatedDate = DateTime.Now;
        }
        public static void UpdateApplicationRole(this AppRole appRole, AppRoleViewModel appRoleViewModel, string action = "add")
        {
            if (action == "update")
                appRole.Id = appRoleViewModel.Id;
            else
                appRole.Id = Guid.NewGuid().ToString();
            appRole.Name = appRoleViewModel.Name;
            appRole.Description = appRoleViewModel.Description;
            appRole.CreatedDate = appRoleViewModel.CreatedDate = DateTime.Now;
            appRole.NormalizedName = appRoleViewModel.Name;
            appRole.ParentId = appRoleViewModel.ParentId;
        }
    }
}

