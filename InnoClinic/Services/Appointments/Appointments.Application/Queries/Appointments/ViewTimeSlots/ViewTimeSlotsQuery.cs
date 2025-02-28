public sealed record ViewTimeSlotsQuery(
    int ServiceId,
    int? DoctorId,
    DateTime AppointmentDate) : IRequest<ErrorOr<List<TimeSlotResponse>>>
{
}
