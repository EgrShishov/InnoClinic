public sealed record CancelAppointmentCommand(int AppointmentId) : IRequest<ErrorOr<Unit>>
{
}
