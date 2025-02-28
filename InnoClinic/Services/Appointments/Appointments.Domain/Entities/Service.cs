public class Service : Entity
{
    public ServiceCategory ServiceCategory { get; set; }
    public string ServiceName { get; set; }
    public bool IsActive {  get; set; }
}