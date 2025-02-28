public class UpdateServiceInfoRequest
{
    public ServiceCategory ServiceCategory { get; init; }
    public string ServiceName {  get; init; }
    public Decimal ServicePrice {  get; init; }
    public bool IsActive {  get; init; }
}
