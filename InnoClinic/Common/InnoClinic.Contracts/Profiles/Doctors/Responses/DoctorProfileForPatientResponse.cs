public sealed record DoctorProfileForPatientResponse(
    string FirstName,
    string LastName,
    string MiddleName,
    string PhotoUrl,
    string Specialization,
    int Experience,
    string City,
    string Street,
    string HouseNumber,
    string OfficeNumber,
    List<SpecializationInfoResponse> specializations);
