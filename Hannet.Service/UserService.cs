using Hannet.Data.Repository;
using Hannet.ViewModel.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hannet.Service
{
    public interface IUserService
    {
        public Task<string> Authenticate(LoginRequestModel models);
        public Task<bool> Register(LoginRegisterModel models);
    }
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository deviceRepository)
        {
            _userRepository = deviceRepository;
        }
        public async Task<string> Authenticate(LoginRequestModel models)
        {
             
            return await _userRepository.Authenticate(models);
        }

        public async Task<bool> Register(LoginRegisterModel models)
        {
            if (await _userRepository.CheckContainsAsync(x=>x.UserName == models.UserName))
            {
                throw new Exception("Tài khoản đã tồn tại");
            }

            return await _userRepository.Register(models);
        }
    }
}
