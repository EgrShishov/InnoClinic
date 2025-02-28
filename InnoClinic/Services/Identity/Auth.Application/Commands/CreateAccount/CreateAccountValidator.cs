public sealed class CreateAccountValidator :
        AbstractValidator<CreateAccountCommand>
{
    public CreateAccountValidator()
    {
        RuleFor(command => command.Email)
            .NotEmpty().WithMessage("Please, enter the email")
            .EmailAddress().WithMessage("You've entered an invalid email");
    }
}
