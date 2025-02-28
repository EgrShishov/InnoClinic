public class Results : Entity
{
    public int AppointmentId { get; set; }
    public DateTime Date { get; set; }
    public string Complaints { get; set; }
    public string Conclusion { get; set; }
    public string Recommendations { get; set; }
    public Appointment Appointment { get; set; }
}
