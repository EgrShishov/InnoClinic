using Microsoft.AspNetCore.Identity;

namespace Auth.Application.Commands.VerifyEmail
{
    public record VerifyEmailCommand(string link) : IRequest<ErrorOr<Unit>> 
    {
    }

    public class VerifyEmailCommandHandler(IAccountRepository repository, UserManager<Account> manager) : IRequestHandler<VerifyEmailCommand, ErrorOr<Unit>>
    {
        public Task<ErrorOr<Unit>> Handle(VerifyEmailCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}   
