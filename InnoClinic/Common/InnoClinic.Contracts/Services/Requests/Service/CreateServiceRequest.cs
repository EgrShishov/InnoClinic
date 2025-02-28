public class CreateServiceRequest
{
    public ServiceCategory ServiceCategory { get; init; }
    public string ServiceName {  get; init; }
    public Decimal ServicePrice {  get; init; }
    public bool IsActive { get; init; } = false;
    public int SpecializationId { get; init; }
}
