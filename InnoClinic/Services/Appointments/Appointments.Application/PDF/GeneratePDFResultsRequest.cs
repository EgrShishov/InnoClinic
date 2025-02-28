public class GeneratePDFResultsRequest
{
    public DateTime Date { get; init; }
    public string PatientName { get; init; }
    public DateTime DateOfBirth { get; init; }
    public string DoctorName { get; init; }
    public string Specialization { get; init; }
    public string ServiceName { get; init; }
    public string Complaints { get; init; }
    public string Conclusion { get; init; }
    public string Recommendations { get; init; }
}
