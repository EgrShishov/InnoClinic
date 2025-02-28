using Microsoft.AspNetCore.Authorization;

public class ServiceController : ApiController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public ServiceController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpGet("all")]
    [Authorize(Roles = "Patient")]
    public async Task<IActionResult> GetAllServices()
    {
        var result = await _mediator.Send(new ViewServicesQuery());

        return result.Match(
            services => Ok(services),
            errors => Problem(errors));
    }

    [HttpGet("{id:int}")]
    [Authorize(Roles = "Receptionist")]
    public async Task<IActionResult> GetServiceInfo(int id)
    {
        var result = await _mediator.Send(new ViewServicesInfoQuery(id));

        return result.Match(
            services => Ok(services),
            errors => Problem(errors));
    }

    [HttpPost("create")]
    [Authorize(Roles = "Receptionist")]
    public async Task<IActionResult> CreateService(CreateServiceRequest request)
    {
        var command = _mapper.Map<CreateServiceCommand>(request);
        
        var result = await _mediator.Send(command);

        return result.Match(
            services => Ok(services),
            errors => Problem(errors));
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Receptionist")]
    public async Task<IActionResult> UpdateServiceInfo(int id, UpdateServiceInfoRequest request)
    {
        var command = _mapper.Map<UpdateServiceCommand>((id, request));
        
        var result = await _mediator.Send(
            new UpdateServiceCommand(id, request.ServiceCategory, request.ServiceName, request.ServicePrice, request.IsActive));

        return result.Match(
            service => Ok(service),
            errors => Problem(errors));
    }

    [HttpPost("change-status")]
    [Authorize(Roles = "Receptionist")]
    public async Task<IActionResult> ChangeServiceStatus(int id, bool status)
    {
        var result = await _mediator.Send(new ChangeServiceStatusCommand(id, status));

        return result.Match(
            service => Ok(service),
            errors => Problem(errors));
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Receptionsit")]
    public async Task<IActionResult> DeleteService(int id)
    {
        var result = await _mediator.Send(new DeleteServiceCommand(id));

        return result.Match(
            _ => Ok(),
            errors => Problem(errors));
    }
}
