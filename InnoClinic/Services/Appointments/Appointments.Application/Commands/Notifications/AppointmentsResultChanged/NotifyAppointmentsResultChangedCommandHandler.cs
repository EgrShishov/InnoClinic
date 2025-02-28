public class NotifyAppointmentsResultChangedCommandHandler(
    IUnitOfWork unitOfWork,
    IEmailSender emailSender,
    IFilesHttpClient filesHttpClient,
    IProfilesHttpClient profilesHttpClient,
    IAccountsHttpClient accountHttpClient)
    : IRequestHandler<NotifyAppointmentsResultChangedCommand, ErrorOr<Unit>>
{
    public async Task<ErrorOr<Unit>> Handle(NotifyAppointmentsResultChangedCommand request, CancellationToken cancellationToken)
    {
        var appointmentResults = await unitOfWork.AppointmentsRepository.GetAppointmentByIdAsync(request.ResultsId);
        
        if (appointmentResults is null)
        {
            return Errors.Results.NotFound;
        }

        var documentResponse = await filesHttpClient.GetDocumentForResultAsync(request.ResultsId);
        
        if (documentResponse.IsError)
        {
            return documentResponse.FirstError;
        }

        var document = documentResponse.Value;

        var profileResponse = await profilesHttpClient.GetPatientAsync(appointmentResults.PatientId);
        
        if (profileResponse.IsError)
        {
            return profileResponse.FirstError;
        }

        var profile = profileResponse.Value;

        var patientAccountResponse = await accountHttpClient.GetAccountInfoAsync(profile.UserId);
        
        if (patientAccountResponse.IsError)
        {
            return patientAccountResponse.FirstError;
        }

        var patientAccount = patientAccountResponse.Value;

        var mailTemplate = new NewAppointmentResultsEmailTemplate
        {
            AppointmentDate = appointmentResults.Date
        };

        await emailSender.SendEmailWithAttachmentAsync(
            patientAccount.Email, 
            "Appointments result changed", 
            mailTemplate.GetContent(), 
            document);

        return Unit.Value;
    }
}
