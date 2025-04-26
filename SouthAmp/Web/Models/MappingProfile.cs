using AutoMapper;
using SouthAmp.Core.Entities;
using SouthAmp.Application.DTOs;

namespace SouthAmp.Web.Models
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Hotel, HotelDto>().ReverseMap();
            CreateMap<Room, RoomDto>().ReverseMap();
            CreateMap<Reservation, ReservationDto>().ReverseMap();
            CreateMap<Payment, PaymentDto>().ReverseMap();
            CreateMap<Review, ReviewDto>().ReverseMap();
            CreateMap<Notification, NotificationDto>().ReverseMap();
            CreateMap<Report, ReportDto>().ReverseMap();
            CreateMap<DiscountCode, DiscountCodeDto>().ReverseMap();
            CreateMap<Location, LocationDto>().ReverseMap();
            // Dodaj mapowania dla innych encji/DTO
        }
    }
}