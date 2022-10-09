using CafeErez.Shared.BusinessService.Validators;
using CafeErez.Shared.Model.Identity;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace BusinessService.Validators
{
    public class LoginRequestValidator : AbstractValidator<LoginRequest>, ILoginRequestValidator
    {
        private readonly IStringLocalizer<RegisterRequestValidator> _localizer;

        public LoginRequestValidator(IStringLocalizer<RegisterRequestValidator> localizer)
        {
            _localizer = localizer;
            DefineValidatorsForLoginRequest();
        }

        private void DefineValidatorsForLoginRequest()
        {
            RuleFor(request => request.Email)
                 .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => _localizer["Email is required"])
                 .EmailAddress().WithMessage(x => _localizer["Email is not correct"]).
                 Matches(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }
    }
}
