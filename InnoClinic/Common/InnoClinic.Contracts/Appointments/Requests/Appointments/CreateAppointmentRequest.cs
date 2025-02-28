public class CreateAppointmentRequest
{
    public int SpecializationId { get; init; }
    public int DoctorId { get; init;}
    public int PatientId { get; init; }
    public int ServiceId { get; init;}
    public string OfficeId { get; init;}
    public DateTime AppointmentDate { get; init;}
    public TimeSpan TimeSlot { get; init;}
}