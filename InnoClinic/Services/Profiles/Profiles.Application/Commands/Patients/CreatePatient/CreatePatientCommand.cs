using Microsoft.AspNetCore.Http;

public sealed record CreatePatientCommand(
    string FirstName,
    string LastName,
    string MiddleName,
    string PhoneNumber,
    string Email,
    DateTime DateOfBirth,
    IFormFile Photo) : IRequest<ErrorOr<CreatePatientResponse>>
{ }
