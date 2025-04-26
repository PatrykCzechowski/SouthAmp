using System.Collections.Generic;
using System.Threading.Tasks;
using SouthAmp.Core.Entities;

namespace SouthAmp.Application.Interfaces
{
    public interface ILocationUseCases
    {
        Task<Location> AddLocationAsync(Location location);
        Task<IEnumerable<Location>> GetAllLocationsAsync();
        Task<Location> GetLocationByIdAsync(int id);
        Task UpdateLocationAsync(Location location);
        Task DeleteLocationAsync(int id);
    }
}
