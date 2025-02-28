public class CreateSpecializationCommandValidator : AbstractValidator<CreateSpecializationCommand>
{
    public CreateSpecializationCommandValidator()
    {
        RuleFor(v => v.SpecializationName)
            .NotEmpty().WithMessage("Specialization name is required.");
    }
}
