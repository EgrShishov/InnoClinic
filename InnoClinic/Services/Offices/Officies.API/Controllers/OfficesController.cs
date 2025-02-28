using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

public class OfficesController : ApiController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public OfficesController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetOffices()
    {
        var response = await _mediator.Send(new GetOfficesQuery());

        return response.Match(
            value => Ok(value),
            errors => Problem(errors));
    }
        
    [HttpGet("{id}")]
    public async Task<IActionResult> GetOffice(string id)
    {
        var response = await _mediator.Send(new GetOfficeByIdQuery(id));

        return response.Match(
            value => Ok(value),
            errors => Problem(errors));
    }

    [HttpPost("create")]
    [Authorize(Roles = "Receptionist")]
    public async Task<IActionResult> CreateOffice(CreateOfficeRequest request)
    {
        var response = await _mediator.Send(
            new CreateOfficeCommand(
                request.City, 
                request.Street, 
                request.HouseNumber,
                request.OfficeNumber, 
                request.Photo, 
                request.RegistryPhoneNumber, 
                request.IsActive));

        return response.Match(
            value => Ok(value),
            errors => Problem(errors));
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Receptionist")]
    public async Task<IActionResult> UpdateOffice(string id, UpdateOfficeRequest request)
    {
        var response = await _mediator.Send(
           new UpdateOfficeCommand(
               id,
               request.City,
               request.Street,
               request.HouseNumber,
               request.OfficeNumber,
               request.Photo,
               request.RegistryPhoneNumber,
               request.IsActive));

        return response.Match(
            value => Ok(value),
            errors => Problem(errors));
    }

    [HttpPut("change-status/{id}")]
    [Authorize(Roles = "Receptionist")]
    public async Task<IActionResult> UpdateOfficesStatus(string id, bool IsActive)
    {
        var result = await _mediator.Send(new ChangeOfficesStatusCommand(id, IsActive));

        return result.Match(
            _ => Ok(),
            errors => Problem(errors));
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Receptionist")]
    public async Task<IActionResult> DeleteOffice(string id)
    {
        var result = await _mediator.Send(new DeleteOfficeCommand(id));

        return result.Match(
            _ => NoContent(),
            errors => Problem(errors));
    }
}
