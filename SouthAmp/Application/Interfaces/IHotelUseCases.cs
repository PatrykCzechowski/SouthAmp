using System.Collections.Generic;
using System.Threading.Tasks;
using SouthAmp.Core.Entities;

namespace SouthAmp.Application.Interfaces
{
    public interface IHotelUseCases
    {
        Task<Hotel> AddHotelAsync(Hotel hotel);
        Task UpdateHotelAsync(Hotel hotel);
        Task DeleteHotelAsync(int id);
        Task<Hotel> GetHotelByIdAsync(int id);
        Task<IEnumerable<Hotel>> GetAllHotelsAsync();
    }
}
