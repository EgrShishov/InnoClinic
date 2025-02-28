public class SpecializationInfoResponse
{
    public string SpecializationName { get; init; }
    public string SpecializationStatus {  get; init; }
    public List<ServiceInfoResponse> RelatedServices {  get; init; }
}