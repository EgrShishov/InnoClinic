
namespace Officies.Application.Commands.AddOffice
{
    public sealed class AddOfficeValidator
        : AbstractValidator<AddOfficeCommand>
    {
        public AddOfficeValidator() 
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
