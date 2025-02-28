using Microsoft.AspNetCore.Authorization;
public class SpecializationController : ApiController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public SpecializationController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpGet("all")]
    [Authorize(Roles = "Patient")]
    public async Task<IActionResult> GetAllSpecializations()
    {
        var result = await _mediator.Send(new ViewSpecializationsListQuery());

        return result.Match(
            specializations => Ok(specializations),
            errors => Problem(errors));
    }

    [HttpGet("{id:int}")]
    [Authorize(Roles = "Receptionist")]
    public async Task<IActionResult> GetSpecializationInfo(int id)
    {
        var result = await _mediator.Send(new ViewSpecializationsInfoQuery(id));

        return result.Match(
            specialization => Ok(specialization),
            errors => Problem(errors));
    }

    [HttpPost("create")]
    [Authorize(Roles = "Receptionist")]
    public async Task<IActionResult> CreateSpecialization(CreateSpecializationRequest request)
    {
        var command = _mapper.Map<CreateSpecializationCommand>(request);

        var result = await _mediator.Send(command);

        return result.Match(
            specialization => Ok(specialization),
            errors => Problem(errors));
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Receptionist")]
    public async Task<IActionResult> UpdateSpecializationInfo(int id, UpdateSpecializationRequest request)
    {
        var command = _mapper.Map<UpdateSpecializationCommand>((id, request));

        var result = await _mediator.Send(new UpdateSpecializationCommand(id, request.SpecializationName, request.IsActive));

        return result.Match(
            specialization => Ok(specialization),
            errors => Problem(errors));
    }

    [HttpPost("change-status")]
    [Authorize(Roles = "Receptionist")]
    public async Task<IActionResult> ChangeSpecializationStatus(int id, bool status)
    {
        var result = await _mediator.Send(new ChangeSpecializationStatusCommand(id, status));

        return result.Match(
            specialization => Ok(specialization),
            errors => Problem(errors));
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Receptionsit")]
    public async Task<IActionResult> DeleteSpecialization(int id)
    {
        var result = await _mediator.Send(new DeleteSpecializationCommand(id));

        return result.Match(
            _ => Ok(),
            errors => Problem(errors));
    }
}
