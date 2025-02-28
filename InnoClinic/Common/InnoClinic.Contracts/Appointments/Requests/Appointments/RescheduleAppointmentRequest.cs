public class RescheduleAppointmentRequest
{
    public int DoctorId { get; init; }
    public DateTime NewAppointmentDate {  get; init; }
    public TimeSpan NewAppointmentTime { get; init; }
}