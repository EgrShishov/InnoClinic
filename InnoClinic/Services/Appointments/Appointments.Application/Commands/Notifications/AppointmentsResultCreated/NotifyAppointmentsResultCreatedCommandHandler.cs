public class NotifyAppointmentsResultCreatedCommandHandler(
    IEmailSender emailSender, 
    IAccountsHttpClient accountHttpClient)
    : IRequestHandler<NotifyAppointmentsResultCreatedCommand, ErrorOr<Unit>>
{
    public async Task<ErrorOr<Unit>> Handle(NotifyAppointmentsResultCreatedCommand request, CancellationToken cancellationToken)
    {
        var patientAccountResponse = await accountHttpClient.GetAccountInfoAsync(request.AccountId);
        
        if (patientAccountResponse.IsError)
        {
            return patientAccountResponse.FirstError;
        }

        var patientAccount = patientAccountResponse.Value;

        var mailTemplate = new NewAppointmentResultsEmailTemplate
        {
            AppointmentDate = request.AppointmentDate
        };
       
        await emailSender.SendEmailWithAttachmentAsync(
            patientAccount.Email,
            NewAppointmentResultsEmailTemplate.TemplateName,
            mailTemplate.GetContent(), 
            request.AppointmentResult);

        return Unit.Value;
    }
}