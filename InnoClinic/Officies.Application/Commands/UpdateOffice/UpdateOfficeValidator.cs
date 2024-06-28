
namespace Officies.Application.Commands.UpdateOffice
{
    public sealed class UpdateOfficeValidator
        : AbstractValidator<UpdateOfficeCommand>
    {
        public UpdateOfficeValidator()
        {
            RuleFor(x => x.office.City)
               .NotEmpty().WithMessage("Please, enter the office’s city");

            RuleFor(x => x.office.Street)
                .NotEmpty().WithMessage("Please, enter the office’s street");

            RuleFor(x => x.office.HouseNumber)
                .NotEmpty().WithMessage("Please, enter the office’s house number");

            RuleFor(x => x.office.RegistryPhoneNumber)
                .NotEmpty().WithMessage("Please, enter the phone number")
                .Matches(@"^\+?\d+$").WithMessage("You've entered an invalid phone number");
        }
    }
}
