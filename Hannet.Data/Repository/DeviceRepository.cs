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
        Task<IEnumerable<DeviceMapping>> GetAllWithPlace();

        Task<IEnumerable<DeviceMapping>> GetAllDeviceByPlaceId(int PlaceId);
    }
    public class DeviceRepository : RepositoryBase<Device>, IDeviceRepository
    {
        private readonly HannetDbContext DbContext;

        public DeviceRepository(HannetDbContext dbContext) : base(dbContext)
        {
            DbContext = dbContext;
        }

        public async Task<IEnumerable<DeviceMapping>> GetAllDeviceByPlaceId(int PlaceId)
        {
            var query = from dv in DbContext.Devices
                        join pl in DbContext.Places
                        on dv.PlaceId equals pl.PlaceId
                        select new { dv, pl };
            if (PlaceId > 0)
            {
                query = query.Where(x => x.dv.PlaceId == PlaceId);
            }
            var data = await query.Select(x => new DeviceMapping()
            {
                Address = x.pl.Address,
                DeviceId = x.dv.DeviceId,
                DeviceName = x.dv.DeviceName,
                PlaceName = x.pl.PlaceName
            }).ToListAsync();

            return data;
        }

        public async Task<IEnumerable<DeviceMapping>> GetAllWithPlace()
        {
            var query = from dv in DbContext.Devices
                        join pl in DbContext.Places
                        on dv.PlaceId equals pl.PlaceId
                        select new { dv, pl };
            var data = await query.Select(x => new DeviceMapping()
            {
                Address = x.pl.Address,
                DeviceId = x.dv.DeviceId,
                DeviceName = x.dv.DeviceName,
                PlaceName = x.pl.PlaceName
            }).ToListAsync();
            return data;
        }
    }
}
