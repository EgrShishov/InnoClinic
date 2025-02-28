public class AppointmentsScheduleResponse
{
    public TimeSpan Time { get; init; }
    public string PatientFullName { get; init; }
    public string PatientProfileLink { get; init; }
    public string ServiceName { get; init; }
    public string ApprovalStatus { get; init; }
    public string MedicalResultsLink { get; init; }
}
