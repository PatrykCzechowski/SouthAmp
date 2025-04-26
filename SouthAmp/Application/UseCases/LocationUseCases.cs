using System.Collections.Generic;
using System.Threading.Tasks;
using SouthAmp.Core.Entities;
using SouthAmp.Core.Interfaces;

namespace SouthAmp.Application.UseCases
{
    public class LocationUseCases
    {
        private readonly ILocationRepository _locationRepository;
        public LocationUseCases(ILocationRepository locationRepository)
        {
            _locationRepository = locationRepository;
        }
        public async Task<Location> AddLocationAsync(Location location)
        {
            await _locationRepository.AddAsync(location);
            return location;
        }
        public async Task<IEnumerable<Location>> GetAllLocationsAsync()
        {
            return await _locationRepository.GetAllAsync();
        }
        public async Task<Location> GetLocationByIdAsync(int id)
        {
            return await _locationRepository.GetByIdAsync(id);
        }
        public async Task UpdateLocationAsync(Location location)
        {
            await _locationRepository.UpdateAsync(location);
        }
        public async Task DeleteLocationAsync(int id)
        {
            await _locationRepository.DeleteAsync(id);
        }
    }
}