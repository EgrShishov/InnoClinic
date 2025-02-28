public class ViewTimeSlotsQueryHandler(
    IUnitOfWork unitOfWork, 
    ITimeSlotsGenerator timeSlotGenerator) 
    : IRequestHandler<ViewTimeSlotsQuery, ErrorOr<List<TimeSlotResponse>>>
{
    public async Task<ErrorOr<List<TimeSlotResponse>>> Handle(ViewTimeSlotsQuery request, CancellationToken cancellationToken)
    {
        var service = await unitOfWork.ServiceRepository.GetServiceByIdAsync(request.ServiceId);

        if (service is null)
        {
            return Errors.Service.NotFound(request.ServiceId);
        }

        var avaibaleSlots = await timeSlotGenerator
            .GenerateSlots(request.AppointmentDate, TimeSpan.FromHours(8), TimeSpan.FromHours(17), service.ServiceCategory);

        if (!avaibaleSlots.Any())
        {
            return Errors.Appointments.ThereAreNoAvaibaleTimeSlots;
        }

        return avaibaleSlots;
    }
}
