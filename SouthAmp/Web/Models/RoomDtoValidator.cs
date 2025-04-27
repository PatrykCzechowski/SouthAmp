using FluentValidation;
using SouthAmp.Application.DTOs;

namespace SouthAmp.Web.Models
{
    public class RoomDtoValidator : AbstractValidator<RoomDto>
    {
        public RoomDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Description).NotEmpty().MaximumLength(1000);
            RuleFor(x => x.Price).GreaterThan(0);
            RuleFor(x => x.Capacity).GreaterThan(0);
            RuleFor(x => x.HotelId).GreaterThan(0);
        }
    }
}