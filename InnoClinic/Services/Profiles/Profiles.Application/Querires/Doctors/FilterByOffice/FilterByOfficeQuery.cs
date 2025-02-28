public record FilterByOfficeQuery(string OfficeId, int PageNumber, int PageSize) : IRequest<ErrorOr<List<Doctor>>>;

