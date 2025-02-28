public class AppointmentHistoryResponse
{
    public DateTime AppointmentDate {  get; init; }
    public TimeSpan AppointmentTime {  get; init; }
    public string DoctorFullName { get; init; }
    public string ServiceName { get; init; }
    public string LinkToMedicalResults { get; init; }
}