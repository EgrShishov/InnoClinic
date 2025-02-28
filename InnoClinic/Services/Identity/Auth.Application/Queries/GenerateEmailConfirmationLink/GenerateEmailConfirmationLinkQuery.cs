public sealed record GenerateEmailConfirmationLinkQuery(int AccountId) : IRequest<ErrorOr<string>>
{
}
