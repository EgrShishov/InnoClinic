public sealed record ApproveAppointmentCommand(int AppointmentId) : IRequest<ErrorOr<Unit>>
{
}
