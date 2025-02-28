using Microsoft.AspNetCore.Http;

public sealed record CreateDoctorCommand(
    string FirstName, 
    string LastName, 
    string MiddleName,
    string Email,
    DateTime DateOfBirth,
    IFormFile Photo,
    int SpecializationId,
    string OfficeId,
    int CareerStartYear,
    int CreatedBy,
    ProfileStatus Status) : IRequest<ErrorOr<Doctor>>
{ }
