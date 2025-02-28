public interface ITimeSlotsGenerator
{
    public Task<List<TimeSlotResponse>> GenerateSlots(
        DateTime appointmentDate, 
        TimeSpan startWorkingHours, 
        TimeSpan endWorkingHours, 
        ServiceCategory serviceCategory);
}
