using CafeErez.Shared.BusinessService.Validators;
using CafeErez.Shared.Model.Identity;
using FluentValidation;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessService.Validators
{
    public class RegisterRequestValidator : AbstractValidator<RegisterRequest>, IRegisterRequestValidator
    {
        private readonly IStringLocalizer<RegisterRequestValidator> _localizer;

        public RegisterRequestValidator(IStringLocalizer<RegisterRequestValidator> localizer)
        {
            _localizer = localizer;
            DefineValidatorsForRegisterRequest();
        }

        public void DefineValidatorsForRegisterRequest()
        {
            RuleFor(request => request.FirstName)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => _localizer["First Name is required"]);
            RuleFor(request => request.LastName)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => _localizer["Last Name is required"]);
           RuleFor(request => request.Email)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => _localizer["Email is required"])
                .EmailAddress().WithMessage(x => _localizer["Email is not correct"]);
            RuleFor(request => request.UserName)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => _localizer["UserName is required"])
                .MinimumLength(6).WithMessage(_localizer["UserName must be at least of length 6"]);
            RuleFor(request => request.Password)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => _localizer["Password is required!"])
                .MinimumLength(8).WithMessage(_localizer["Password must be at least of length 8"])
                .Matches(@"[A-Z]").WithMessage(_localizer["Password must contain at least one capital letter"])
                .Matches(@"[a-z]").WithMessage(_localizer["Password must contain at least one lowercase letter"])
                .Matches(@"[0-9]").WithMessage(_localizer["Password must contain at least one digit"]);
            RuleFor(request => request.ConfirmPassword)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => _localizer["Password Confirmation is required!"])
                .Equal(request => request.Password).WithMessage(x => _localizer["Passwords don't match"]);
            RuleFor(request => request.PhoneNumber)
                .Must(x => !string.IsNullOrWhiteSpace(x) && x.Length >= 10).Matches(@"[0-9]").WithMessage(x => _localizer["Phone Number is required!"]);
        }
    }
}
