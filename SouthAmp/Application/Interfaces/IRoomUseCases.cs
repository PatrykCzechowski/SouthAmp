using System.Collections.Generic;
using System.Threading.Tasks;
using SouthAmp.Core.Entities;

namespace SouthAmp.Application.Interfaces
{
    public interface IRoomUseCases
    {
        Task<Room> AddRoomAsync(Room room);
        Task UpdateRoomAsync(Room room);
        Task DeleteRoomAsync(int id);
        Task<Room> GetRoomByIdAsync(int id);
        Task<IEnumerable<Room>> GetRoomsByHotelIdAsync(int hotelId);
    }
}
