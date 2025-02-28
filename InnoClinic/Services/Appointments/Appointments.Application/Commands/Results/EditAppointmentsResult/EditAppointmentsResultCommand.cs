public sealed record EditAppointmentsResultCommand(
    int ResultsId,
    string Complaints,
    string Conclusion,
    string Recommendations) : IRequest<ErrorOr<Results>>
{
}
