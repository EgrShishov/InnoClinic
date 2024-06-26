
namespace Officies.Application.Commands
{
    public sealed record UpdateOfficeCommand(Office office) : IRequest<ErrorOr<Unit>>
    {
    }

    public class UpdateOfficeCommandHandler(IOfficeRepository repository) : IRequestHandler<UpdateOfficeCommand, ErrorOr<Unit>>
    {
        public async Task<ErrorOr<Unit>> Handle(UpdateOfficeCommand request, CancellationToken cancellationToken)
        {
            await repository.UpdateOfficeAsync(request.office);
            return Unit.Value;
        }
    }
}
