public class AppointmentsController : ApiController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public AppointmentsController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpGet("schedule/{doctorId:int}")]
    [Authorize(Roles = "Doctor")]
    public async Task<IActionResult> GetAppointmentSchedule(int doctorId, DateTime appointmentDate)
    {
        var result = await _mediator.Send(new ViewAppointmentScheduleQuery(doctorId, appointmentDate));

        return result.Match(
            schedule => Ok(schedule),
            errors => Problem(errors));
    }

    [HttpGet("all")]
    [Authorize(Roles = "Receptionist")]
    public async Task<IActionResult> GetAppointmentList(
        DateTime? appointmentDate, 
        int? doctorId, 
        int? serviceId, 
        bool isActive)
    {
        var command = new ViewAppointmentsListQuery(appointmentDate, doctorId, serviceId, isActive);
        
        var result = await _mediator.Send(command);

        return result.Match(
            list => Ok(list),
            errors => Problem(errors));
    }

    [HttpGet("history")]
    [Authorize(Roles = "Doctor, Patient")]
    public async Task<IActionResult> GetAppointmentHistory(int id)
    {
        var result = await _mediator.Send(new ViewAppointmentsHistoryQuery(id));

        return result.Match(
            history => Ok(history),
            errors => Problem(errors));
    }

    [HttpPost("create")]
    [Authorize(Roles = "Patient, Receptionist")]
    public async Task<IActionResult> CreateAppointment(CreateAppointmentRequest request)
    {
        var command = new CreateAppointmentCommand(
            request.PatientId,
            request.SpecializationId,
            request.DoctorId,
            request.ServiceId,
            request.OfficeId,
            request.AppointmentDate,
            request.TimeSlot);

        var result = await _mediator.Send(command);

        return result.Match(
            appointment => Ok(appointment),
            errors => Problem(errors));
    }

    [HttpPost("approve/{id:int}")]
    [Authorize(Roles = "Receptionist")]
    public async Task<IActionResult> ApproveAppointment(int id)
    {
        var result = await _mediator.Send(new ApproveAppointmentCommand(id));
        
        return result.Match(
            value => Ok(value),
            errors => Problem(errors));
    }

    [HttpPost("cancel/{id:int}")]
    [Authorize(Roles = "Receptionist")]
    public async Task<IActionResult> CancelAppointment(int id)
    {
        var result = await _mediator.Send(new CancelAppointmentCommand(id));

        return result.Match(
            value => Ok(value),
            errors => Problem(errors));
    }

    [HttpGet("time-slots")]
    [Authorize(Roles = "Receptionist, Patient")]
    public async Task<IActionResult> ViewTimeSlots(int ServiceId, int DoctorId, DateTime AppointmentDate)
    {
        var command = new ViewTimeSlotsQuery(
            ServiceId,
            DoctorId, 
            AppointmentDate);

        var result = await _mediator.Send(command);

        return result.Match(
            slots => Ok(slots),
            errors => Problem(errors));
    }

    [HttpPut("reschedule/{id:int}")]
    [Authorize(Roles = "Receptionist, Patient")]
    public async Task<IActionResult> RescheduleAppointment(int appointmentId, RescheduleAppointmentRequest request)
    {
        var command = new RescheduleAppointmentCommand(
            appointmentId, 
            request.DoctorId, 
            request.NewAppointmentDate, 
            request.NewAppointmentTime);

        var result = await _mediator.Send(command);

        return result.Match(
            rescheduledAppointment => Ok(rescheduledAppointment),
            errors => Problem(errors));
    }
}
