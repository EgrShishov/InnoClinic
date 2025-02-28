namespace InnoClinic.Contracts.ServiceStatusChangedEvent;

public class ServiceStatusChangedEvent
{
    public int Id { get; set; }
    public string ServiceName { get; set; }
    public bool IsActive { get; set; }
}