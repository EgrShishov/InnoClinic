using Auth.Application.Common;
using Auth.Domain.Interfaces;
using InnoClinic.Contracts.Authentication.Responses;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Auth.Application.Commands
{
    public sealed record SignInCommand(string email, string password, string role) : IRequest<ErrorOr<AuthorizationResponse>> { }

    public class SignInCommandHandler(
        IMediator mediator, 
        IAccountRepository _accountRepository, 
        UserManager<Account> manager, 
        IEmailSender emailSender,
        ITokenGenerator tokenService
        )
        : IRequestHandler<SignInCommand, ErrorOr<AuthorizationResponse>>
    {
        public async Task<ErrorOr<AuthorizationResponse>> Handle(SignInCommand request, CancellationToken cancellationToken)
        {
            if (!await _accountRepository.EmailExistsAsync(request.email))
            {
                return Errors.Authentication.InvalidCredentials;
            }

            var account = await _accountRepository.GetByEmailAsync(request.email);

            var isPasswordValid = await manager.CheckPasswordAsync(account, request.password);
            if (!isPasswordValid)
            {
                return Errors.Authentication.InvalidCredentials;
            }

            var accessToken = tokenService.GenerateAccessToken(account);
            var refreshToken = tokenService.GenerateRefreshToken(account);

            var roles = await manager.GetRolesAsync(account);
            var role = roles.Contains("Doctor") ? "Doctor" :
                    roles.Contains("Receptionist") ? "Receptionist" : "Patient";

            return new AuthorizationResponse(accessToken, refreshToken, role);
        }
    }
}
