﻿using Hannet.Data.Infrastructure;
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
       
    }
    public class DeviceRepository : RepositoryBase<Device>, IDeviceRepository
    {
        private readonly HannetDbContext DbContext;

        public DeviceRepository(HannetDbContext dbContext) : base(dbContext)
        {
            DbContext = dbContext;
        }
    }
}
