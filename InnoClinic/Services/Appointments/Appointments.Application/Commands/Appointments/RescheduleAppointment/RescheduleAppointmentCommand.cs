public sealed record RescheduleAppointmentCommand(
    int AppointmentId,
    int DoctorId,
    DateTime Date,
    TimeSpan Time
    ) : IRequest<ErrorOr<Appointment>>
{
}
