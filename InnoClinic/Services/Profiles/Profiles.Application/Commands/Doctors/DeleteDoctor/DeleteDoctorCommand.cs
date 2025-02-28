public record DeleteDoctorCommand(int DoctorId) : IRequest<ErrorOr<Unit>>;
