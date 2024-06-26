using InnoClinic.Contracts.Offices.Requests;
using InnoClinic.Contracts.Offices.Responses;
using Mapster;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Officies.Application.Commands;
using Officies.Application.Queries;
using Officies.Domain.Entities;

namespace Officies.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OfficesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public OfficesController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<OfficeResponse>>> GetOffices()
        {
            var offices = await _mediator.Send(new GetOfficesQuery());
            var response = offices.Adapt<IEnumerable<OfficeResponse>>();
            return Ok(response);
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<OfficeResponse>> GetOffice(string id)
        {
            var office = await _mediator.Send(new GetOfficeByIdQuery(id));
            if (office == null)
            {
                return NotFound();
            }
            var response = office.Adapt<OfficeResponse>();
            return Ok(response);
        }

        [HttpPost("create")]
        public async Task<ActionResult<OfficeResponse>> CreateOffice(CreateOfficeRequest request)
        {
            var office = request.Adapt<Office>();
            await _mediator.Send(new AddOfficeCommand(office));
            var response = office.Adapt<OfficeResponse>();
            return CreatedAtAction(nameof(GetOffice), new { id = office.Id }, response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOffice(string id, UpdateOfficeRequest request)
        {
            var office = request.Adapt<Office>();
            office.Id = id;

            await _mediator.Send(new UpdateOfficeCommand(office));
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOffice(string id)
        {
            await _mediator.Send(new DeleteOfficeCommand(id));
            return NoContent();
        }
    }
}
