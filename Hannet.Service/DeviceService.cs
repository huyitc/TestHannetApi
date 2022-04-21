using Hannet.Data.Repository;
using Hannet.Model.MappingModels;
using Hannet.Model.Models;
using Hannet.ViewModel.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hannet.Service
{
    public interface IDeviceService
    {
        public Task<IEnumerable<DeviceViewModels>> GetAll();

        public Task<IEnumerable<DeviceViewModels>> GetByPlaceID(int PlaceId);

        public Task<Device> Update(DeviceMapping models);

        public Task<Device> CreateDevice(DeviceMapping models);

        public Task<Device> Delete(int DeviceId);
    }
    public class DeviceService:IDeviceService
    {
        private readonly IDeviceRepository _deviceRepository;

        public DeviceService(IDeviceRepository deviceRepository)
        {
            _deviceRepository = deviceRepository;
        }

        public async Task<Device> CreateDevice(DeviceMapping models)
        {
            return await _deviceRepository.CreateDevice(models);
        }

        public async Task<Device> Delete(int DeviceId)
        {
            if (await _deviceRepository.CheckContainsAsync(x => x.DeviceId != DeviceId))
            throw new Exception($"Không tìm thấy DeviceId để xóa: {DeviceId}");
            return await _deviceRepository.Delete(DeviceId);
        }

        public async Task<IEnumerable<DeviceViewModels>> GetAll()
        {
            return (List<DeviceViewModels>) await _deviceRepository.GetAll();
        }

        public async Task<IEnumerable<DeviceViewModels>> GetByPlaceID(int PlaceId)
        {
            return await _deviceRepository.GetByPlaceID(PlaceId);
        }

        public async Task<Device> Update(DeviceMapping models)
        {
            if ( await _deviceRepository.CheckContainsAsync(x=>x.DeviceId != models.DeviceId))
            {
                throw new Exception("Cannot find device");

            }
            return await _deviceRepository.Update(models);
        }
    }
}
