public sealed record ViewAppointmentsListQuery(
    DateTime? AppointmentDate,
    int? DoctorId,
    int? ServiceId,
    bool AppointmentStatus) : IRequest<ErrorOr<List<AppointmentListResponse>>>
{
}
