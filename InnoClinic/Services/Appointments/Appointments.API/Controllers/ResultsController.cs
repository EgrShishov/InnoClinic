public class ResultsController : ApiController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public ResultsController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpGet("{id:int}")]
    [Authorize(Roles = "Patient, Doctor")]
    public async Task<IActionResult> GetAppointmentResult(int id)
    {
        var result = await _mediator.Send(new ViewAppointmentResultQuery(id));

        return result.Match(
            appointmentResult => Ok(appointmentResult),
            errors => Problem(errors));
    }

    [HttpGet("download/{id:int}")]
    [Authorize(Roles = "Patient")]
    public async Task<IActionResult> DownloadAppointmentsResults(int id)
    {
        var result = await _mediator.Send(new DownloadAppointmentResultsQuery(id));

        return result.Match(
             formFile =>
             {
                 using var memoryStream = new MemoryStream();
                 using var inputStream = formFile.OpenReadStream();

                 inputStream.CopyTo(memoryStream);
                 memoryStream.Position = 0;

                 return File(memoryStream, formFile.ContentType, formFile.FileName);
             },
            errors => Problem(errors));
    }

    [HttpPost("create")]
    [Authorize(Roles = "Doctor")]
    public async Task<IActionResult> CreateAppointmentResult(CreateAppointmentResultRequest request)
    {
        //var command = _mapper.Map<CreateAppointmentsResultCommand>(request);
        var command = new CreateAppointmentsResultCommand(
            request.AppointmentId, 
            request.PatientId, 
            request.DoctorId, 
            request.ServiceId, 
            request.Complaints, 
            request.Conclusion, 
            request.Recommendations);

        var result = await _mediator.Send(command);

        return result.Match(
            result => Ok(result),
            errors => Problem(errors));
    }

    [HttpPut("edit/{id:int}")]
    [Authorize(Roles = "Doctor")]
    public async Task<IActionResult> UpdateAppointmentResultInfo(int id, UpdateAppointmentResultRequest request)
    {
        var command = new EditAppointmentsResultCommand(id, request.Complaints, request.Conclusion, request.Recommendations);

        var result = await _mediator.Send(command);

        return result.Match(
            appointmentResult => Ok(appointmentResult),
            errors => Problem(errors));
    }
}
