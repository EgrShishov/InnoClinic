namespace InnoClinic.Contracts.ServiceCreatedEvent;

public class ServiceCreatedEvent
{
    public int Id { get; set; }
    public string ServiceName { get; set; }
    public ServiceCategory ServiceCategory { get; set; }
    public bool IsActive { get; set; }
}

