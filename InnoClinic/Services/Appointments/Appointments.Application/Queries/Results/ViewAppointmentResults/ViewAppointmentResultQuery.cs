public sealed record ViewAppointmentResultQuery(int ResultsId) : IRequest<ErrorOr<ResultResponse>>
{
}
