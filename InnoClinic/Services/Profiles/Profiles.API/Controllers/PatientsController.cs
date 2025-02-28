using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

public class PatientsController : ApiController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public PatientsController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpPost("profile/link")]
    //[Authorize(Roles = "Patient")]
    public async Task<IActionResult> LinkProfile(int accountId, int profileId)
    {
        var result = await _mediator.Send(new LinkPatientToExistingAccountCommand(accountId, profileId));

        return result.Match(
            _ => Ok("Profile linked successfully"),
            errors => Problem(errors));
    }

    [HttpPost("create")]
    //[Authorize(Roles = "Patient, Receptionist")]
    public async Task<IActionResult> CreatePatientProfile(CreatePatientRequest request)
    {
        var command = new CreatePatientCommand(
            request.FirstName,
            request.LastName,
            request.MiddleName,
            request.PhoneNumber,
            request.Email,
            request.DateOfBirth,
            request.Photo);

        var result = await _mediator.Send(command);

        return result.Match(
            patient => Ok(patient),
            errors => Problem(errors));
    }

    [HttpPut("profile/edit/{id:int}")]
    //[Authorize(Roles = "Patient, Receptionist")]
    public async Task<IActionResult> EditProfile(int id, UpdatePatientRequest request)
    {
        var command = new UpdatePatientCommand(
            id, 
            request.FirstName,
            request.LastName, 
            request.MiddleName, 
            request.PhoneNumber, 
            request.DateOfBirth);

        var result = await _mediator.Send(command);

        return result.Match(
            patient => Ok(patient),
            errors => Problem(errors));
    }

    [HttpDelete("{id}")]
    //[Authorize(Roles = "Receptionist")]
    public async Task<IActionResult> DeletePatientProfile(int id)
    {
        var result = await _mediator.Send(new DeletePatientCommand(id));

        return result.Match(
            _ => NoContent(),
            errors => Problem(errors));
    }

    [HttpGet("profile/{id:int}")]
    //[Authorize(Roles = "Doctor, Receptionist, Patient")] //difference between roles -> receptionist doesnt see appoitments
    public async Task<IActionResult> GetPatientProfile(int id)
    {
        var result = await _mediator.Send(new ViewPatientProfileQuery(id));

        return result.Match(
            patient => Ok(patient),
            errors => Problem(errors));
    }

    [HttpGet("search/by-name")]
    //[Authorize(Roles = "Receptionist")]
    public async Task<IActionResult> GetPatientByName(string FirstName, string LastName, string MiddleName)
    {
        var result = await _mediator.Send(new SearchPatientByNameQuery(FirstName, LastName, MiddleName));

        return result.Match(
            patient => Ok(patient),
            errors => Problem(errors));
    }

    [HttpGet("all")]
    //[Authorize(Roles = "Receptionist")]
    public async Task<IActionResult> GetAllPatients(int pageNumber, int pageSize)
    {
        var result = await _mediator.Send(new ViewAllPatientsQuery(pageNumber, pageSize));

        return result.Match(
            patients => Ok(patients),
            errors => Problem(errors));
    }
}
