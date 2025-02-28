public class TimeSlotsGenerator : ITimeSlotsGenerator
{
    private readonly IUnitOfWork _unitOfWork;
    public TimeSlotsGenerator(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<List<TimeSlotResponse>> GenerateSlots(
        DateTime appointmentDate, 
        TimeSpan startWorkingHours, 
        TimeSpan endWorkingHours, 
        ServiceCategory serviceCategory)
    {
        List<TimeSlotResponse> slots = new List<TimeSlotResponse>();
        var bookedSlots = await GetBookedSlotsForDate(appointmentDate);
        int requiredMinutes = GetRequiredMinutes(serviceCategory);
        int incrementMinutes = GetIncrementMinutes(serviceCategory);

        for (var time = startWorkingHours; time < endWorkingHours;)
        {
            var endTime = time.Add(TimeSpan.FromMinutes(requiredMinutes));

            if (endTime > endWorkingHours)
            {
                break;
            }

            if (IsTimeSlotAvaibale(time, endTime, bookedSlots))
            {
                slots.Add(new TimeSlotResponse
                {
                    AppointmentDate = appointmentDate,
                    StartTime = time,
                    EndTime = endTime,
                    IsAvaibale = true
                });
            }

            time = time.Add(TimeSpan.FromMinutes(incrementMinutes));
        }

        return slots;
    }

    public async Task<List<TimeSlotResponse>> GetBookedSlotsForDate(DateTime AppointmentDate)
    {
        var avaibaleSlots = new List<TimeSlotResponse>();

        var appointmentsByDate = await _unitOfWork.AppointmentsRepository.ListAsync(a => a.Date.Date == AppointmentDate.Date.Date);

        foreach (var appointment in appointmentsByDate)
        {
            var service = await _unitOfWork.ServiceRepository.GetServiceByIdAsync(appointment.ServiceId);
            
            if (service is null)
            {
                return null;
            }

            int requiredMinutes = GetRequiredMinutes(service.ServiceCategory);
            
            avaibaleSlots.Add(new TimeSlotResponse
            {
                IsAvaibale = false,
                AppointmentDate = AppointmentDate.Date,
                StartTime = appointment.Time,
                EndTime = appointment.Time.Add(TimeSpan.FromMinutes(requiredMinutes))
            });
        }

        return avaibaleSlots;
    }

    private bool IsTimeSlotAvaibale(TimeSpan StartTime, TimeSpan EndTime, List<TimeSlotResponse> BookedSlots)
    {
        foreach (var slot in BookedSlots)
        {
            if ((StartTime >= slot.StartTime && StartTime <= slot.EndTime) || 
                (EndTime >= slot.StartTime && EndTime <= slot.EndTime))
            {
                return false;
            }
        }
        return true;
    }

    private int GetRequiredMinutes(ServiceCategory serviceCategory)
    {
        return serviceCategory switch
        {
            ServiceCategory.Consultation => 20,
            ServiceCategory.Diagnostics => 30,
            ServiceCategory.Analyses => 10,
            _ => 10
        };
    }

    private int GetIncrementMinutes(ServiceCategory serviceCategory)
    {
        return serviceCategory switch
        {
            ServiceCategory.Consultation => 10,
            ServiceCategory.Diagnostics => 20,
            ServiceCategory.Analyses => 10,
            _ => 0
        };
    }
}
