using System.Collections.Generic;
using System.Threading.Tasks;
using SouthAmp.Core.Entities;
using SouthAmp.Core.Interfaces;
using System.Linq;
using SouthAmp.Application.Interfaces;

namespace SouthAmp.Application.UseCases
{
    public class ReservationUseCases : IReservationUseCases
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly IRoomRepository _roomRepository;
        public ReservationUseCases(IReservationRepository reservationRepository, IRoomRepository roomRepository)
        {
            _reservationRepository = reservationRepository;
            _roomRepository = roomRepository;
        }
        public async Task<bool> CheckAvailabilityAsync(int roomId, System.DateTime start, System.DateTime end)
        {
            var room = await _roomRepository.GetByIdAsync(roomId);
            if (room == null || !room.IsAvailable) return false;
            var reservations = room.Reservations?.Where(r => r.Status == ReservationStatus.Confirmed).ToList() ?? new List<Reservation>();
            return !reservations.Any(r => r.StartDate < end && r.EndDate > start);
        }
        public async Task<Reservation> CreateReservationAsync(Reservation reservation)
        {
            if (!await CheckAvailabilityAsync(reservation.RoomId, reservation.StartDate, reservation.EndDate))
                throw new System.Exception("Room not available in selected dates");
            reservation.Status = ReservationStatus.Confirmed;
            reservation.CreatedAt = System.DateTime.UtcNow;
            await _reservationRepository.AddAsync(reservation);
            return reservation;
        }
        public async Task<IEnumerable<Reservation>> GetUserReservationsAsync(int userId)
        {
            return await _reservationRepository.GetByUserIdAsync(userId);
        }
        public async Task CancelReservationAsync(int reservationId)
        {
            var reservation = await _reservationRepository.GetByIdAsync(reservationId);
            if (reservation == null) throw new System.Exception("Reservation not found");
            reservation.Status = ReservationStatus.Cancelled;
            reservation.CancelledAt = System.DateTime.UtcNow;
            await _reservationRepository.UpdateAsync(reservation);
        }
        public async Task ChangeReservationDateAsync(int reservationId, System.DateTime newStart, System.DateTime newEnd)
        {
            var reservation = await _reservationRepository.GetByIdAsync(reservationId);
            if (reservation == null) throw new System.Exception("Reservation not found");
            if (!await CheckAvailabilityAsync(reservation.RoomId, newStart, newEnd))
                throw new System.Exception("Room not available in new dates");
            reservation.StartDate = newStart;
            reservation.EndDate = newEnd;
            await _reservationRepository.UpdateAsync(reservation);
        }
    }
}