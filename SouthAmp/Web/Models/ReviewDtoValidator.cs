using FluentValidation;
using SouthAmp.Application.DTOs;

namespace SouthAmp.Web.Models
{
    public class ReviewDtoValidator : AbstractValidator<ReviewDto>
    {
        public ReviewDtoValidator()
        {
            RuleFor(x => x.HotelId).GreaterThan(0);
            RuleFor(x => x.Rating).InclusiveBetween(1, 5);
            RuleFor(x => x.Comment).NotEmpty().MaximumLength(1000);
        }
    }
}