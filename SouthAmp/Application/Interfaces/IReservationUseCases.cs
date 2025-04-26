using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SouthAmp.Core.Entities;

namespace SouthAmp.Application.Interfaces
{
    public interface IReservationUseCases
    {
        Task<bool> CheckAvailabilityAsync(int roomId, DateTime start, DateTime end);
        Task<Reservation> CreateReservationAsync(Reservation reservation);
        Task<IEnumerable<Reservation>> GetUserReservationsAsync(int userId);
        Task CancelReservationAsync(int reservationId);
        Task ChangeReservationDateAsync(int reservationId, DateTime newStart, DateTime newEnd);
    }
}
