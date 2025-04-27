using FluentValidation;
using SouthAmp.Application.DTOs;

namespace SouthAmp.Web.Models
{
    public class HotelDtoValidator : AbstractValidator<HotelDto>
    {
        public HotelDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Description).NotEmpty().MaximumLength(1000);
            RuleFor(x => x.Address).NotEmpty().MaximumLength(200);
            RuleFor(x => x.City).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Country).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Latitude).InclusiveBetween(-90, 90);
            RuleFor(x => x.Longitude).InclusiveBetween(-180, 180);
        }
    }
}