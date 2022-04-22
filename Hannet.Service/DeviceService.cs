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
        Task<IEnumerable<Device>> GetAll(string keyword);
        Task<IEnumerable<Device>> GetAll();
        Task<Device> GetDetail(int DeviceId);
        Task<Device> Add(Device device);
        Task<Device> Update(Device device);
        Task<Device> Delete(int DeviceId);

        Task<bool> CheckContainsAsync(Device device);
    }
    public class DeviceService:IDeviceService
    {
        private readonly IDeviceRepository _deviceRepository;

        public DeviceService(IDeviceRepository deviceRepository)
        {
            _deviceRepository = deviceRepository;
        }

        public async Task<Device> Add(Device device)
        {
            return await _deviceRepository.AddASync(device);
        }

        public Task<bool> CheckContainsAsync(Device device)
        {
            return _deviceRepository.CheckContainsAsync(x=>x.DeviceName == device.DeviceName && x.DeviceId != device.DeviceId);
        }

        public async Task<Device> Delete(int DeviceId)
        {
            return await _deviceRepository.DeleteAsync(DeviceId);
        }

        public async Task<IEnumerable<Device>> GetAll(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
                return await _deviceRepository.GetAllAsync(x => x.DeviceName.ToUpper().Contains(keyword.ToUpper()));
            else
                return await _deviceRepository.GetAllAsync();
        }

        public async Task<IEnumerable<Device>> GetAll()
        {
            return await _deviceRepository.GetAllAsync();
        }

        public async Task<Device> GetDetail(int DeviceId)
        {
            return await _deviceRepository.GetByIdAsync(DeviceId);
        }

        public async Task<Device> Update(Device device)
        {
            if (await _deviceRepository.CheckContainsAsync(x => x.DeviceName == device.DeviceName && x.DeviceId != device.DeviceId))
                throw new Exception("Tên thiết bị đã tồn tại!!!!");
            else
            {
                var update = await _deviceRepository.GetByIdAsync(device.DeviceId);
                update.DeviceName = device.DeviceName;
                update.PlaceId = device.PlaceId;
                update.UpdatedDate = DateTime.Now;
                update.UpdatedBy = device.UpdatedBy;
                update.Status = device.Status;
                return await _deviceRepository.UpdateASync(update);
            }    

        }
    }
}
