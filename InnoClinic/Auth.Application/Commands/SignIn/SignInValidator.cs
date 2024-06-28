
namespace Auth.Application.Commands.SignIn
{
    public sealed class SignInValidator :
        AbstractValidator<SignInCommand>
    {
        public SignInValidator() 
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
                .MustAsync(EmailExists).WithMessage("User with this email doesn’t exist");
        }

        private async Task<bool> EmailExists(string email, CancellationToken token)
        {
            return await Task.FromResult(true);
        }
    }
}
