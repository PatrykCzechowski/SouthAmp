using FluentValidation;
using SouthAmp.Application.DTOs;
using SouthAmp.Web.Controllers;

namespace SouthAmp.Web.Models
{
    public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
    {
        public RegisterRequestValidator()
        {
            RuleFor(x => x.UserName).NotEmpty().MinimumLength(3);
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Password).NotEmpty().MinimumLength(6);
            RuleFor(x => x.Role).Must(r => r == null || r == "guest" || r == "provider" || r == "admin");
        }
    }
}