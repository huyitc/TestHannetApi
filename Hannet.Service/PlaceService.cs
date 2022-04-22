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
    public interface IPlaceService
    {
        Task<IEnumerable<Place>> GetAll(string keyword);
        Task<IEnumerable<Place>> GetAll();
        Task<Place> GetDetail(int DeviceId);
        Task<Place> Add(Place place);
        Task<Place> Update(Place place);
        Task<Place> Delete(int PlaceId);

        Task<bool> CheckContainsAsync(Place place);
    }
    public class PlaceService : IPlaceService
    {
        private readonly IPlaceRepository _placeRepository;

        public PlaceService(IPlaceRepository placeRepository)
        {
            _placeRepository = placeRepository;
        }

        public async Task<Place> Add(Place place)
        {
            return await _placeRepository.AddASync(place);
        }

        public  Task<bool> CheckContainsAsync(Place place)
        {
            return _placeRepository.CheckContainsAsync(x => x.PlaceName == place.PlaceName && x.PlaceId != place.PlaceId);
        }

        public async Task<Place> Delete(int PlaceId)
        {
            return await _placeRepository.DeleteAsync(PlaceId);
        }

        public async Task<IEnumerable<Place>> GetAll(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
                return await _placeRepository.GetAllAsync(x => x.PlaceName.ToUpper().Contains(keyword.ToUpper()));
            else
                return await _placeRepository.GetAllAsync();
        }

        public async Task<IEnumerable<Place>> GetAll()
        {
            return await _placeRepository.GetAllAsync();
        }

        public async Task<Place> GetDetail(int DeviceId)
        {
            return await _placeRepository.GetByIdAsync(DeviceId);
        }

        public async Task<Place> Update(Place place)
        {
            if (await _placeRepository.CheckContainsAsync(x => x.PlaceName == place.PlaceName && x.PlaceId != place.PlaceId))
                throw new Exception("Tên vị trí đã tồn tại!!!");
            else
            {
                var update = await _placeRepository.GetByIdAsync(place.PlaceId);
                update.PlaceName = place.PlaceName;
                update.Address = place.Address;
                update.UpdatedBy = place.UpdatedBy;
                update.UpdatedDate = DateTime.Now;
                update.CreatedBy = place.CreatedBy;
                update.CreatedDate = DateTime.Now;
                update.Status = place.Status;
                return await _placeRepository.UpdateASync(update);
            }
        }
    }
}
