using FluentValidation;
using SouthAmp.Application.DTOs;

namespace SouthAmp.Web.Models
{
    public class PaymentDtoValidator : AbstractValidator<PaymentDto>
    {
        public PaymentDtoValidator()
        {
            RuleFor(x => x.ReservationId).GreaterThan(0);
            RuleFor(x => x.Amount).GreaterThan(0);
        }
    }
}