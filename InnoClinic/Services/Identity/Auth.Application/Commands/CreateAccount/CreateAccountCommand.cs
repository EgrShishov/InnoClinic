using Microsoft.AspNetCore.Http;

public sealed record CreateAccountCommand(
    string Email, 
    string PhoneNumber,
    IFormFile Photo,
    int CreatedBy,
    string Role)
    : IRequest<ErrorOr<AuthorizationResponse>>
{
}
