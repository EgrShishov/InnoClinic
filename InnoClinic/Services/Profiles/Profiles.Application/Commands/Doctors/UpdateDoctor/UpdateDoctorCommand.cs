using Microsoft.AspNetCore.Http;

public record UpdateDoctorCommand(
    int DoctorId,
    string FirstName,
    string LastName,
    string MiddleName,
    DateTime DateOfBirth,
    int SpecializationId,
    string OfficeId,
    int CareerStartYear,
    IFormFile Photo,
    ProfileStatus Status,
    int UpdatedBy
) : IRequest<ErrorOr<Doctor>>;
