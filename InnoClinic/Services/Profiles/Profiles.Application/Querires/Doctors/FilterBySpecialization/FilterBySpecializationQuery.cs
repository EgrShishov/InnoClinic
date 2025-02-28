public record FilterBySpecializationQuery(int SpecializationId, int PageNumber, int PageSize) : IRequest<ErrorOr<List<Doctor>>>;

