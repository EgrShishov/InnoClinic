public sealed record DoctorProfileForAdminResponse(
    string PhotoUrl,
    string FirstName,
    string LastName,
    string MiddleName,
    DateTime DateOfBirth,
    DateTime CareetStartTime,
    string Specialization,
    string City,
    string Street,
    string HouseNumber,
    string OfficeNumber,
    string Status);
