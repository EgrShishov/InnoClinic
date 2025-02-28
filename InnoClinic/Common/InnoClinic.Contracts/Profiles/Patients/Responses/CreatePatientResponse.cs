public class CreatePatientResponse
{
    public int Id { get; init; }
    public bool Success { get; init; }
    public bool IsMatchFound {  get; init; }
    public string Message {  get; init; }
    public PatientProfileResponse MatchedProfile {  get; init; }
}