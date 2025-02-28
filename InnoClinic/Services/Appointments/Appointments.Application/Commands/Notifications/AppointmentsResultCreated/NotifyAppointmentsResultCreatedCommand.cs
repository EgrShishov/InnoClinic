using Microsoft.AspNetCore.Http;

public sealed record NotifyAppointmentsResultCreatedCommand(
    IFormFile AppointmentResult, 
    DateTime AppointmentDate,
    int AccountId) : IRequest<ErrorOr<Unit>>
{
}