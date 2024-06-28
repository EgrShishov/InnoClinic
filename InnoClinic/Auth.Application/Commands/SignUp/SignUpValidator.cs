
namespace Auth.Application.Commands.SignUp
{
    public sealed class SignUpValidator :
           AbstractValidator<SignUpCommand>
    {
        public SignUpValidator()
        {
            RuleFor(command => command.password)
                .NotEmpty().WithMessage("Please, enter the password")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters")
                .MaximumLength(15).WithMessage("Password must be no more than 15 characters")
                .When(x => x.password.Any(char.IsDigit), ApplyConditionTo.CurrentValidator)
                .When(x => x.password.Any(char.IsLetter), ApplyConditionTo.CurrentValidator);

            RuleFor(command => command.email)
                .NotEmpty().WithMessage("Please, enter the email")
                .EmailAddress().WithMessage("You've entered an invalid email")
                .MustAsync(EmailIsUnique).WithMessage("User with this email already exist");

            RuleFor(x => x.reentered_password)
                .NotEmpty().WithMessage("Please, reenter the password")
                .Equal(x => x.password).WithMessage("The passwords you’ve entered don’t coincide");
        }

        private async Task<bool> EmailIsUnique(string email, CancellationToken token)
        {
            return await Task.FromResult(true);
        }
    }
}
