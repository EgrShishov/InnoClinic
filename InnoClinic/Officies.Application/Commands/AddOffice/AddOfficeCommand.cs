namespace Officies.Application.Commands.AddOffice
{
    public sealed record AddOfficeCommand(Office office) : IRequest<ErrorOr<Unit>>
    {
    }

    public class AddOfficeCommandHandler(IOfficeRepository repository) : IRequestHandler<AddOfficeCommand, ErrorOr<Unit>>
    {
        public async Task<ErrorOr<Unit>> Handle(AddOfficeCommand request, CancellationToken cancellationToken)
        {
            await repository.AddOfficeAsync(request.office);
            return Unit.Value;
        }
    }
}
