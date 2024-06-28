using Officies.Domain.Common.Errors;

namespace Officies.Application.Commands.ChangeOfficesStatus
{
    public record ChangeOfficesStatusCommand(string Id, bool isActive) : IRequest<ErrorOr<Unit>>
    {
    }

    public class ChangeOfficesStatusCommandHandler(IOfficeRepository repository) : IRequestHandler<ChangeOfficesStatusCommand, ErrorOr<Unit>>
    {
        public async Task<ErrorOr<Unit>> Handle(ChangeOfficesStatusCommand request, CancellationToken cancellationToken)
        {
            var office = await repository.GetOfficeByIdAsync(request.Id);

            if (office == null)
            {
                return Errors.Offices.NotFound;
            }

            office.IsActive = request.isActive;

            await repository.UpdateOfficeAsync(office);
        }
    }
}
