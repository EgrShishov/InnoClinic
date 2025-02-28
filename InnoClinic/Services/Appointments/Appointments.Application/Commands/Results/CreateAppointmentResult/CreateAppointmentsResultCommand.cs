public sealed record CreateAppointmentsResultCommand(
    int AppointmentId,
    int PatientId,
    int DoctorId,
    int ServiceId,
    string Complaints,
    string Conclusion,
    string Recommendations) : IRequest<ErrorOr<Results>>
{
}
