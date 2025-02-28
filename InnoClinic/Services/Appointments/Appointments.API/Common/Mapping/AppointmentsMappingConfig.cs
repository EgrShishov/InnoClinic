public class AppointmentsMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CreateAppointmentRequest, CreateAppointmentCommand>()
            .Map(dest => dest.Date, src => src.AppointmentDate)
            .Map(dest => dest.DoctorId, src => src.DoctorId)
            .Map(dest => dest.OfficeId, src => src.OfficeId)
            .Map(dest => dest.PatientId, src => src.PatientId)
            .Map(dest => dest.ServiceId, src => src.ServiceId)
            .Map(dest => dest.SpecializationId, src => src.SpecializationId)
            .Map(dest => dest.Time, src => src.TimeSlot);

        config.NewConfig<CreateAppointmentResultRequest, CreateAppointmentsResultCommand>()
            .Map(dest => dest.PatientId, src => src.PatientId)
            .Map(dest => dest.DoctorId, src => src.DoctorId)
            .Map(dest => dest.ServiceId, src => src.ServiceId)
            .Map(dest => dest.Conclusion, src => src.Conclusion)
            .Map(dest => dest.Recommendations, src => src.Recommendations)
            .Map(dest => dest.Complaints, src => src.Complaints);

        config.NewConfig<(UpdateAppointmentResultRequest request, int ResultId), EditAppointmentsResultCommand>()
            .Map(dest => dest.ResultsId, src => src.ResultId)
            .Map(dest => dest.Conclusion, src => src.request.Conclusion)
            .Map(dest => dest.Recommendations, src => src.request.Recommendations)
            .Map(dest => dest.Complaints, src => src.request.Complaints);
    }
}
