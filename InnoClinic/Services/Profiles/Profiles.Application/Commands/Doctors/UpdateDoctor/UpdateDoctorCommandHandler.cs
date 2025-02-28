public class UpdateDoctorCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<UpdateDoctorCommand, ErrorOr<Doctor>>
{
    public async Task<ErrorOr<Doctor>> Handle(UpdateDoctorCommand request, CancellationToken cancellationToken)
    {
        var doctor = await unitOfWork.DoctorsRepository.GetDoctorByIdAsync(request.DoctorId);
        
        if (doctor is null)
        {
            return Errors.Doctors.NotFound(request.DoctorId);
        }

        doctor.FirstName = request.FirstName;
        doctor.LastName = request.LastName;
        doctor.MiddleName = request.MiddleName;
        doctor.DateOfBirth = request.DateOfBirth;
        doctor.SpecializationId = request.SpecializationId;
        doctor.OfficeId = request.OfficeId;
        doctor.CareerStartYear = request.CareerStartYear;
        doctor.Status = request.Status;

        await unitOfWork.DoctorsRepository.UpdateDoctorAsync(doctor);

        await unitOfWork.CompleteAsync(cancellationToken);

        return doctor;
    }
}
