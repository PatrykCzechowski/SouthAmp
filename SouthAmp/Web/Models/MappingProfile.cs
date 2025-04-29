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
            CreateMap<Report, ReportDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ReverseMap()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => Enum.Parse<ReportStatus>(src.Status)));
            CreateMap<DiscountCode, DiscountCodeDto>().ReverseMap();
            CreateMap<Location, LocationDto>().ReverseMap();
            CreateMap<AppUserProfile, UserDto>().ReverseMap();
            CreateMap<AuditLog, AuditLogDto>().ReverseMap();
            // Dodaj mapowania dla innych encji/DTO
            // Przykład:
            // CreateMap<NazwaEncji, NazwaEncjiDto>().ReverseMap();
            // Dodaj kolejne mapowania poniżej według potrzeb
        }
    }
}