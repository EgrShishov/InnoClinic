public class AppointmentListResponse
{
    public TimeSpan AppointmentTime { get; init; }
    public string DoctorFullName {  get; init; }
    public string PatientFullName { get; init; }
    public string PatientPhoneNumber { get; init; }
    public string ServiceName { get; init; }
}