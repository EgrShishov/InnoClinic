public class RememberAboutUpcomingAppointmentCommandHandler(
    IUnitOfWork unitOfWork, 
    IEmailSender emailSender,
    IProfilesHttpClient profilesHttpClient,
    IAccountsHttpClient accountsHttpClient)
    : IRequestHandler<RememberAboutUpcomingAppointmentCommand, ErrorOr<Unit>>
{
    public async Task<ErrorOr<Unit>> Handle(RememberAboutUpcomingAppointmentCommand request, CancellationToken cancellationToken)
    {
        var currentDate = DateTime.UtcNow;

        var appointments = await unitOfWork.AppointmentsRepository
            .ListAsync(a => a.Date == currentDate.Date.AddDays(TimeSpan.FromDays(1).TotalMilliseconds));
        
        if (appointments is null || !appointments.Any())
        {
            return Errors.Appointments.EmptyList;
        }

        foreach (var appointment in appointments)
        {
            var doctorProfileResponse = await profilesHttpClient.GetDoctorAsync(appointment.DoctorId);
            if (doctorProfileResponse.IsError)
            {
                return doctorProfileResponse.FirstError;
            }

            var doctorProfile = doctorProfileResponse.Value;

            var patientProfileResponse = await profilesHttpClient.GetPatientAsync(appointment.PatientId);
           
            if (patientProfileResponse.IsError)
            {
                return patientProfileResponse.FirstError;
            }

            var patientProfile = patientProfileResponse.Value;
            
            var patientAccountResponse = await accountsHttpClient.GetAccountInfoAsync(patientProfile.UserId);
           
            if (patientAccountResponse.IsError)
            {
                return patientAccountResponse.FirstError;
            }

            var patientAccount = patientAccountResponse.Value;

            var service = await unitOfWork.ServiceRepository.GetServiceByIdAsync(appointment.ServiceId);
         
            if (service == null)
            {
                return Errors.Service.NotFound(appointment.ServiceId);
            }

            string DoctorsFullName = $"{doctorProfile.LastName} {doctorProfile.FirstName} {doctorProfile.MiddleName}";
            string PatientsFullName = $"{patientProfile.LastName} {patientProfile.FirstName} {patientProfile.MiddleName}";

            var emailNotificationTemplate = new EmailTemplates.AppointmentNotificationEmailTemplate
            {
                AppointmentDate = appointment.Date,
                AppointmentTime = appointment.Time,
                DoctorsFullName = DoctorsFullName,
                PatientsFullName = PatientsFullName,
                ServiceName = service.ServiceName
            };

            await emailSender.SendEmailAsync(patientAccount.Email, "Upcoming appointment", emailNotificationTemplate.GetContent());
        }

        return Unit.Value;
    }
}
