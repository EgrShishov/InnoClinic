public class CreatePatientProfileCommandValidator : AbstractValidator<CreatePatientCommand>
{
    public CreatePatientProfileCommandValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("Please, enter the first name");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Please, enter the last name");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Please, enter the phone number")
            .Matches(@"^\+\d+$").WithMessage("You've entered an invalid phone number");

        RuleFor(x => x.DateOfBirth)
            .NotEmpty().WithMessage("Please, select the date")
            .LessThanOrEqualTo(DateTime.Now).WithMessage("Date of birth must be less or equal to the current date");
    }
}
