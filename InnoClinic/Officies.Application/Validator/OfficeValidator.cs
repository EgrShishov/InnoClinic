
namespace Officies.Application.Validator
{
    public class OfficeValidator : AbstractValidator<Office>
    {
        public OfficeValidator()
        {
            RuleFor(office => office.City).NotEmpty().WithMessage("Please, enter the office’s city");
            RuleFor(office => office.Street).NotEmpty().WithMessage("Please, enter the office’s street");
            RuleFor(office => office.HouseNumber).NotEmpty().WithMessage("Please, enter the office’s house number");
            RuleFor(office => office.RegistryPhoneNumber).NotEmpty().WithMessage("Please, enter the phone number")
                                                         .Matches(@"^\+\d+$").WithMessage("You've entered an invalid phone number");
        }
    }
}
