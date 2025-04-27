using FluentValidation;
using SouthAmp.Application.DTOs;

namespace SouthAmp.Web.Models
{
    public class ReservationDtoValidator : AbstractValidator<ReservationDto>
    {
        public ReservationDtoValidator()
        {
            RuleFor(x => x.RoomId).GreaterThan(0);
            RuleFor(x => x.StartDate).NotEmpty();
            RuleFor(x => x.EndDate).NotEmpty().GreaterThan(x => x.StartDate);
        }
    }
}