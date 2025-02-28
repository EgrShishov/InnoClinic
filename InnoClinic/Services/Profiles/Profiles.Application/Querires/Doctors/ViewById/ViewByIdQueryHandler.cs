public class ViewByIdQueryHandler(
    IUnitOfWork unitOfWork, 
    IAccountHttpClient accountHttpClient) : IRequestHandler<ViewByIdQuery, ErrorOr<DoctorProfileResponse>>
{
    public async Task<ErrorOr<DoctorProfileResponse>> Handle(ViewByIdQuery request, CancellationToken cancellationToken)
    {
        var doctor = await unitOfWork.DoctorsRepository.GetDoctorByIdAsync(request.DoctorId);
        
        if (doctor is null)
        {
            return Errors.Doctors.NotFound(request.DoctorId);
        }

        var accountInfoResponse = await accountHttpClient.GetAccountInfo(doctor.AccountId);
        
        if (accountInfoResponse.IsError)
        {
            return accountInfoResponse.FirstError;
        }

        var account = accountInfoResponse.Value;

        var office = await unitOfWork.OfficeRepository.GetOfficeByIdAsync(doctor.OfficeId.ToString());
        
        if (office is null)
        {
            return Errors.Office.NotFound;
        }

        string specilization = string.Empty;

        return new DoctorProfileResponse 
        {
            DateOfBirth = doctor.DateOfBirth,
            CareerStartYear = doctor.CareerStartYear,
            FirstName = doctor.FirstName,
            LastName = doctor.LastName,
            MiddleName = doctor.MiddleName,
            PhotoUrl = account.PhotoUrl,
            City = office.City,
            HouseNumber = office.HouseNumber,
            OfficeNumber = office.OfficeNumber, 
            Street = office.Street,
            SpecializationName = specilization,
        };
    }
}
