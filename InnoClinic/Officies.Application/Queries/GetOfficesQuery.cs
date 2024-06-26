
namespace Officies.Application.Queries
{
    public sealed record GetOfficesQuery() : IRequest<ErrorOr<List<Office>>>
    {
    }

    public class GetOfficesQueryHandler(IOfficeRepository repository) : IRequestHandler<GetOfficesQuery, ErrorOr<List<Office>>>
    {
        public async Task<ErrorOr<List<Office>>> Handle(GetOfficesQuery request, CancellationToken cancellationToken)
        {
            return await repository.GetOfficesAsync();
        }
    }
}
