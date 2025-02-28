public sealed record ViewAppointmentsHistoryQuery(int PatientId) : IRequest<ErrorOr<List<AppointmentHistoryResponse>>>
{
}
