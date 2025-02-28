public record ViewByIdQuery(int DoctorId) : IRequest<ErrorOr<DoctorProfileResponse>>;
