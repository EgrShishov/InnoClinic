using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

public class DoctorsController : ApiController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public DoctorsController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpPost("create")]
    //[Authorize(Roles = "Receptionist")]
    public async Task<IActionResult> CreateDoctorProfile(CreateDoctorRequest request)
    {
        int CreatedBy = 0;
        var command = new CreateDoctorCommand(
            request.FirstName,
            request.LastName,
            request.MiddleName,
            request.Email,
            request.DateOfBirth,
            request.Photo,
            request.SpecializationId,
            request.OfficeId,
            request.CareerStartYear,
            CreatedBy,
            request.Status);
        
        var result = await _mediator.Send(command);
        
        if (result.IsError)
        {
            return BadRequest(result.FirstError);
        
        }
        return Ok(result.Value);
    }

    [HttpDelete("{id}")]
    //[Authorize(Roles = "Receptionist")]
    public async Task<IActionResult> DeleteDoctor(int id)
    {
        var command = new DeleteDoctorCommand(id);
        
        var result = await _mediator.Send(command);
        
        if (result.IsError)
        {
            return BadRequest(result.FirstError);
        }
        
        return NoContent();
    }

    [HttpPut("profile/edit/{id}")]
    //[Authorize(Roles = "Receptionist, Doctor")]
    public async Task<IActionResult> EditProfile(int id, UpdateDoctorRequest request)
    {
        int UpdatedBy = 0;

        var command = new UpdateDoctorCommand(
            id,
            request.FirstName,
            request.LastName,
            request.MiddleName,
            request.DateOfBirth,
            request.SpecializationId,
            request.OfficeId,
            request.CareerStartYear,
            request.Photo,
            request.Status,
            UpdatedBy);

        var result = await _mediator.Send(command);
        
        if (result.IsError)
        {
            return BadRequest(result.FirstError);
        }
        
        return Ok(result.Value);
    }

    [HttpGet("by-office")]
    //[Authorize(Roles = "Patient")]
    public async Task<IActionResult> GetDoctorsByOffice(string officeId, int pageNumber = 1, int pageSize = 10)
    {
        var query = new FilterByOfficeQuery(officeId, pageNumber, pageSize);
        
        var result = await _mediator.Send(query);
        
        if (result.IsError)
        {
            return BadRequest(result.FirstError);
        }
        
        return Ok(result.Value);
    }

    [HttpGet("by-specialization")]
    //[Authorize(Roles = "Patient")]
    public async Task<IActionResult> GetDoctorsBySpecialization(int specializationId, int pageNumber = 1, int pageSize = 10)
    {
        var query = new FilterBySpecializationQuery(specializationId, pageNumber, pageSize);
        
        var result = await _mediator.Send(query);
        
        if (result.IsError)
        {
            return BadRequest(result.FirstError);
        }
        
        return Ok(result.Value);
    }

    [HttpGet("search/by-name")]
    //[Authorize(Roles = "Receptionist")]
    public async Task<IActionResult> SearchDoctorsByName(string FirstName, string LastName, string MiddleName)
    {
        var query = new SearchByNameQuery(FirstName, LastName, MiddleName);
        
        var result = await _mediator.Send(query);
        
        if (result.IsError)
        {
            return BadRequest(result.FirstError);
        }
        
        return Ok(result.Value);
    }

    [HttpGet("profile/{id}")]
    //[Authorize(Roles = "Patient")]
    public async Task<IActionResult> GetDoctorById(int id)
    {
        var query = new ViewByIdQuery(id);
        
        var result = await _mediator.Send(query);
        
        if (result.IsError)
        {
            return BadRequest(result.FirstError);
        }
        
        return Ok(result.Value);
    }

    [HttpGet("all")]
    //[Authorize(Roles = "Patient")]
    public async Task<IActionResult> GetAllDoctors(int pageNumber = 1, int pageSize = 10)
    {
        var query = new ViewDoctorsQuery(pageNumber, pageSize);
        
        var result = await _mediator.Send(query);
        
        if (result.IsError)
        {
            return BadRequest(result.FirstError);
        }
        
        return Ok(result.Value);
    }

}
