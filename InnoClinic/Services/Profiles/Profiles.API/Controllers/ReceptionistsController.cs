using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize(Roles = "Receptionist")]
public class ReceptionistsController : ApiController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public ReceptionistsController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateReceptionsitProfile(CreateReceptionistRequest request)
    {
        int CreatedBy = 0;

        var command = new CreateReceptionistCommand(
            request.FirstName, 
            request.LastName, 
            request.MiddleName, 
            request.Email,
            request.PhoneNumber,
            CreatedBy,
            request.OfficeId, 
            request.Photo);

        var result = await _mediator.Send(command);

        return result.Match(
            value => Ok(value),
            errors => Problem(errors));
    }

    [HttpPut("profile/edit/{id:int}")]
    public async Task<IActionResult> UpdateReceptionsitProfile(int id, UpdateReceptionistRequest request)
    {
        var command = new UpdateReceptionistCommand(
            id,
            request.FirstName,
            request.LastName,
            request.MiddleName,
            request.Email,
            request.Photo,
            request.OfficeId);

        var result = await _mediator.Send(command);

        return result.Match(
            value => Ok(value),
            errors => Problem(errors));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteReceptionsitProfile(int id)
    {
        var result = await _mediator.Send(new DeleteReceptionistCommand(id));

        return result.Match(
            response => Ok(response),
            errors => Problem(errors));
    }

    [HttpGet("profile/{id:int}")]
    public async Task<IActionResult> GetReceptionist(int id)
    {
        var result = await _mediator.Send(new ViewReceptionistProfileQuery(id));

        return result.Match(
            value => Ok(value),
            errors => Problem(errors));
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAllReceptionist(int pageNumber, int pageSize)
    {
        var result = await _mediator.Send(new ViewAllReceptionistsQuery(pageNumber, pageSize));

        return result.Match(
            value => Ok(value),
            errors => Problem(errors));
    }
}