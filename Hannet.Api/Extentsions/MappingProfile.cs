using AutoMapper;
using Hannet.Model.Models;
using Hannet.ViewModel.ViewModels;

namespace Hannet.Api.Extentsions
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Device, DeviceViewModels>().ReverseMap();
            CreateMap<Place, PlaceViewModel>().ReverseMap();
            CreateMap<Employee, EmployeeViewModels>().ReverseMap();
            CreateMap<AppGroup, AppGroupViewModel>().ReverseMap();
            CreateMap<AppUser, AppUserViewModel>().ReverseMap();
            CreateMap<AppRole, AppRoleViewModel>().ReverseMap();
        }
    }
}
