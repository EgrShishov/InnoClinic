namespace InnoClinic.Contracts.ServiceUpdatedEvent;
public class ServiceUpdatedEvent
{
    public int Id { get; set; }
    public string ServiceName { get; set; }
    public ServiceCategory ServiceCategory { get; set; }
    public bool IsActive { get; set; }
}
