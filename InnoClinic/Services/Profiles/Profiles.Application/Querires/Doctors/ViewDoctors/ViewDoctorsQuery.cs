public record ViewDoctorsQuery(int PageNumber, int PageSize) : IRequest<ErrorOr<List<DoctorListResponse>>>;
