using Microsoft.AspNetCore.Http;

public sealed record EditAccountCommand(int AccountId, int UpdatedBy, string PhoneNumber, IFormFile Photo) : IRequest<ErrorOr<Unit>>
{
}
