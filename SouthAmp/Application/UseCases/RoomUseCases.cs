using System.Collections.Generic;
using System.Threading.Tasks;
using SouthAmp.Core.Entities;
using SouthAmp.Core.Interfaces;

namespace SouthAmp.Application.UseCases
{
    public class RoomUseCases
    {
        private readonly IRoomRepository _roomRepository;
        public RoomUseCases(IRoomRepository roomRepository)
        {
            _roomRepository = roomRepository;
        }
        public async Task<Room> AddRoomAsync(Room room)
        {
            await _roomRepository.AddAsync(room);
            return room;
        }
        public async Task UpdateRoomAsync(Room room)
        {
            await _roomRepository.UpdateAsync(room);
        }
        public async Task DeleteRoomAsync(int id)
        {
            await _roomRepository.DeleteAsync(id);
        }
        public async Task<Room> GetRoomByIdAsync(int id)
        {
            return await _roomRepository.GetByIdAsync(id);
        }
        public async Task<IEnumerable<Room>> GetRoomsByHotelIdAsync(int hotelId)
        {
            return await _roomRepository.GetByHotelIdAsync(hotelId);
        }
    }
}