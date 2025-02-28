public class CreateReceptionistCommandValidator : AbstractValidator<CreateReceptionistCommand>
{
    public CreateReceptionistCommandValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("Please, enter the first name")
            .MaximumLength(100).WithMessage("First name cannot exceed 100 characters");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Please, enter the last name")
            .MaximumLength(100).WithMessage("Last name cannot exceed 100 characters");

        RuleFor(x => x.MiddleName)
            .MaximumLength(100).WithMessage("Middle name cannot exceed 100 characters");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Please, enter the email")
            .EmailAddress().WithMessage("You've entered an invalid email");

        RuleFor(x => x.OfficeId)
            .NotEmpty().WithMessage("Please, choose the office");
    }
}
