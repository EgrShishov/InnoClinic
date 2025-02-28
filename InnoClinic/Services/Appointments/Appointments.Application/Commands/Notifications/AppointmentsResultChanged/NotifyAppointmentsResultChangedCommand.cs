public sealed record NotifyAppointmentsResultChangedCommand(int ResultsId) : IRequest<ErrorOr<Unit>>
{
}
