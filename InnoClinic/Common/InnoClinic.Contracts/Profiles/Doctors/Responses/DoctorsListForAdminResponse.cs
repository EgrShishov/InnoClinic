public record DoctorsListForAdminResponse(
    int Id,
    string PhotoUrl,
    string LastName,
    string FirstName,
    string MiddleName,
    string Specialization,
    DateTime DateOfBirth,
    int Experience,
    string City,
    string Street,
    string HouseNumber,
    string OfficeNumber);
