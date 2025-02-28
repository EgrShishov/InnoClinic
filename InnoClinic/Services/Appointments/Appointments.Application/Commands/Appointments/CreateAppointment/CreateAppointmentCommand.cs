public sealed record CreateAppointmentCommand(
    int PatientId,
    int SpecializationId,
    int DoctorId,
    int ServiceId,
    string OfficeId,
    DateTime Date,
    TimeSpan Time
    ) : IRequest<ErrorOr<Appointment>>
{
}
