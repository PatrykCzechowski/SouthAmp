using FluentValidation;
using SouthAmp.Application.DTOs;
using SouthAmp.Web.Controllers;

namespace SouthAmp.Web.Models
{
    public class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Password).NotEmpty();
        }
    }
}