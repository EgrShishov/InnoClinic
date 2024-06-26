
namespace Officies.Application.Queries
{
    public sealed record GetOfficeByIdQuery(string Id) : IRequest<Office>
    {
    }

    public class GetOfficeByIdQueryHandler(IOfficeRepository repository) : IRequestHandler<GetOfficeByIdQuery, Office>
    {
        public async Task<Office> Handle(GetOfficeByIdQuery request, CancellationToken cancellationToken)
        {
            return await repository.GetOfficeByIdAsync(request.Id);
        }
    }
}
