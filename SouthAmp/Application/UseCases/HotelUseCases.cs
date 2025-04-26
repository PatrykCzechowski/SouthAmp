using System.Collections.Generic;
using System.Threading.Tasks;
using SouthAmp.Core.Entities;
using SouthAmp.Core.Interfaces;

namespace SouthAmp.Application.UseCases
{
    public class HotelUseCases
    {
        private readonly IHotelRepository _hotelRepository;
        public HotelUseCases(IHotelRepository hotelRepository)
        {
            _hotelRepository = hotelRepository;
        }
        public async Task<Hotel> AddHotelAsync(Hotel hotel)
        {
            await _hotelRepository.AddAsync(hotel);
            return hotel;
        }
        public async Task UpdateHotelAsync(Hotel hotel)
        {
            await _hotelRepository.UpdateAsync(hotel);
        }
        public async Task DeleteHotelAsync(int id)
        {
            await _hotelRepository.DeleteAsync(id);
        }
        public async Task<Hotel> GetHotelByIdAsync(int id)
        {
            return await _hotelRepository.GetByIdAsync(id);
        }
        public async Task<IEnumerable<Hotel>> GetAllHotelsAsync()
        {
            return await _hotelRepository.GetAllAsync();
        }
    }
}