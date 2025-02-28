public sealed class SignInValidator :
    AbstractValidator<SignInCommand>
{
    public SignInValidator() 
    {
        RuleFor(command => command.Password)
            .NotEmpty().WithMessage("Please, enter the password")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters")
            .MaximumLength(15).WithMessage("Password must be no more than 15 characters")
            .When(x => x.Password.Any(char.IsDigit), ApplyConditionTo.CurrentValidator)
            .When(x => x.Password.Any(char.IsLetter), ApplyConditionTo.CurrentValidator);

        RuleFor(command => command.Email)
            .NotEmpty().WithMessage("Please, enter the email")
            .EmailAddress().WithMessage("You've entered an invalid email");
    }
}
