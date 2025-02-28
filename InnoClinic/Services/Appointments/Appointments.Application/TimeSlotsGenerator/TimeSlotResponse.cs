public class TimeSlotResponse
{
    public DateTime AppointmentDate { get; init; }
    public TimeSpan StartTime { get; init; }
    public TimeSpan EndTime { get; init; }
    public bool IsAvaibale { get; init; }
}