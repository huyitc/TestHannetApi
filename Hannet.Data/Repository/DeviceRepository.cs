using Hannet.Data.Infrastructure;
using Hannet.Model.MappingModels;
using Hannet.Model.Models;
using Hannet.ViewModel.ViewModels;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Hannet.Data.Repository
{
    public interface IDeviceRepository : IRepository<Device>
    {
        public Task<IEnumerable<DeviceViewModels>> GetAll();

        public Task<IEnumerable<DeviceViewModels>> GetByPlaceID(int PlaceId);

        public Task<Device> Update(DeviceMapping models);

        public Task<Device> CreateDevice(DeviceMapping models);

        public Task<Device> Delete(int DeviceId);
    }
    public class DeviceRepository : RepositoryBase<Device>, IDeviceRepository
    {
        private readonly HannetDbContext DbContext;

        public DeviceRepository(HannetDbContext dbContext) : base(dbContext)
        {
            DbContext = dbContext;
        }
        public async Task<Device> CreateDevice(DeviceMapping models)
        {
            var device = new Device()
            {
                DeviceName = models.DeviceName,
                PlaceId = models.PlaceId,
            };
            DbContext.Devices.Add(device);
            await DbContext.SaveChangesAsync();
            return device;
        }

        public async Task<Device> Delete(int DeviceId)
        {
            var devide = await DbContext.Devices.FindAsync(DeviceId);
            DbContext.Devices.Remove(devide);
            await DbContext.SaveChangesAsync();
            return devide;
        }
        public async Task<IEnumerable<DeviceViewModels>> GetAll()
        {
            var query = from dv in DbContext.Devices
                        join pl in DbContext.Places
                        on dv.PlaceId equals pl.PlaceId
                        select new { dv, pl };
            var data = await query.Select(x => new DeviceViewModels()
            {
                Address = x.pl.Address,
                DeviceId = x.dv.DeviceId,
                DeviceName = x.dv.DeviceName,
                PlaceName = x.pl.PlaceName
            }).ToListAsync();

            return data;
        }

        public async Task<IEnumerable<DeviceViewModels>> GetByPlaceID(int PlaceId)
        {
            var query = from dv in DbContext.Devices
                        join pl in DbContext.Places
                        on dv.PlaceId equals pl.PlaceId
                        select new { pl, dv };
            if (PlaceId > 0)
            {
                query = query.Where(x => x.pl.PlaceId == PlaceId);
            }

            var data = await query.Select(x => new DeviceViewModels()
            {
                Address = x.pl.Address,
                DeviceId = x.dv.DeviceId,
                DeviceName = x.dv.DeviceName,
                PlaceName = x.pl.PlaceName
            }).ToListAsync();

            return data;
        }

        public async Task<Device> Update(DeviceMapping models)
        {
            var device = await DbContext.Devices.FindAsync(models.DeviceId);
            device.DeviceName = models.DeviceName;
            DbContext.Devices.Update(device);
            await DbContext.SaveChangesAsync();
            return device;
        }
    }
}
