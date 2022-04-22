using Hannet.Data.Infrastructure;
using Hannet.Model.MappingModels;
using Hannet.Model.Models;
using Hannet.ViewModel.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hannet.Data.Repository
{
    public interface IPlaceRepository : IRepository<Place>
    {
    }
    public class PlaceRepository : RepositoryBase<Place>, IPlaceRepository
    {
        private readonly HannetDbContext DbContext;

        public PlaceRepository(HannetDbContext dbContext) : base(dbContext)
        {
            DbContext = dbContext;
        }

    }

 }
