public sealed record DoctorProfileForDoctorResponse(
    string PhotoUrl,
    string FirstName,
    string LastName,
    string MiddleName,
    DateTime DateOfBirth,
    DateTime CareetStartTime,
    int SpecializationId,
    string City,
    string Street,
    string HouseNumber,
    string OfficeNumber);