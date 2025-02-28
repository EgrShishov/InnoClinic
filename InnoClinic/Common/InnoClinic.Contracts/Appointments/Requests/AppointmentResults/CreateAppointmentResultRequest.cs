public class CreateAppointmentResultRequest
{
    public int AppointmentId { get; init; }
    public int PatientId { get; init; }
    public int DoctorId { get; init; }
    public int ServiceId { get; init; }
    public string Complaints { get; init; }
    public string Conclusion { get; init; }
    public string Recommendations { get; init; }
}
