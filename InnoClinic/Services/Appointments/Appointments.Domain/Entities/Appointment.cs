public class Appointment : Entity
{
    public int PatientId { get; set; }
    public int DoctorId { get; set; }
    public int ServiceId { get; set; }
    public DateTime Date { get; set; }
    public TimeSpan Time { get; set; }
    public bool IsApproved { get; set; }
}
