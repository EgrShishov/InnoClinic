namespace Officies.Application.Commands.DeleteOffice
{
    public sealed record DeleteOfficeCommand(string Id) : IRequest<ErrorOr<Unit>>
    {
    }

    public class DeleteOfficeCommandHandler(IOfficeRepository repository) : IRequestHandler<DeleteOfficeCommand, ErrorOr<Unit>>
    {
        public async Task<ErrorOr<Unit>> Handle(DeleteOfficeCommand request, CancellationToken cancellationToken)
        {
            await repository.DeleteOfficeAsync(request.Id);
            return Unit.Value;
        }
    }
}
