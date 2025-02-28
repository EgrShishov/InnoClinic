public class ProfilesMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        // Doctor

        config.NewConfig<(CreateDoctorRequest request, int AccountId, int CreatedBy), CreateDoctorCommand>()
            .Map(dest => dest.OfficeId, src => src.request.OfficeId)
            .Map(dest => dest.CareerStartYear, src => src.request.CareerStartYear)
            .Map(dest => dest.DateOfBirth, src => src.request.DateOfBirth)
            .Map(dest => dest.FirstName, src => src.request.FirstName)
            .Map(dest => dest.LastName, src => src.request.LastName)
            .Map(dest => dest.MiddleName, src => src.request.MiddleName)
            .Map(dest => dest.CreatedBy, src => src.CreatedBy)
            .Map(dest => dest, src => src.request.Email)
            .Map(dest => dest.SpecializationId, src => src.request.SpecializationId)
            .Map(dest => dest.Status, src => src.request.Status);

        config.NewConfig<SearchByNameRequest, SearchByNameQuery>()
            .Map(dest => dest.FirstName, src => src.FirstName)
            .Map(dest => dest.LastName, src => src.LastName)
            .Map(dest => dest.MiddleName, src => src.MiddleName);

        config.NewConfig<(UpdateDoctorRequest request, int DoctorId), UpdateDoctorCommand>()
            .Map(dest => dest.OfficeId, src => src.request.OfficeId)
            .Map(dest => dest.Photo, src => src.request.Photo)
            .Map(dest => dest.CareerStartYear, src => src.request.CareerStartYear)
            .Map(dest => dest.DateOfBirth, src => src.request.DateOfBirth)
            .Map(dest => dest.DateOfBirth, src => src.request.DateOfBirth)
            .Map(dest => dest.FirstName, src => src.request.FirstName)
            .Map(dest => dest.LastName, src => src.request.LastName)
            .Map(dest => dest.MiddleName, src => src.request.MiddleName)
            .Map(dest => dest.SpecializationId, src => src.request.SpecializationId)
            .Map(dest => dest.Status, src => src.request.Status);
         
        // Patient

        config.NewConfig<CreatePatientRequest, CreatePatientCommand>()
            .Map(dest => dest.DateOfBirth, src => src.DateOfBirth)
            .Map(dest => dest.FirstName, src => src.FirstName)
            .Map(dest => dest.LastName, src => src.LastName)
            .Map(dest => dest.MiddleName, src => src.MiddleName)
            .Map(dest => dest.PhoneNumber, src => src.PhoneNumber)
            .Map(dest => dest.Photo, src => src.Photo);

        config.NewConfig<(UpdatePatientRequest request, int PatientId), UpdatePatientCommand>()
            .Map(dest => dest.PatientId, src => src.PatientId)
            .Map(dest => dest.DateOfBirth, src => src.request.DateOfBirth)
            .Map(dest => dest.FirstName, src => src.request.FirstName)
            .Map(dest => dest.LastName, src => src.request.LastName)
            .Map(dest => dest.MiddleName, src => src.request.MiddleName)
            .Map(dest => dest.PhoneNumber, src => src.request.PhoneNumber);

        // Receptionist

        config.NewConfig<CreateReceptionistRequest, CreateReceptionistCommand>()
            .Map(dest => dest.MiddleName, src => src.MiddleName)
            .Map(dest => dest.LastName, src => src.LastName)
            .Map(dest => dest.FirstName, src => src.FirstName)
            .Map(dest => dest.OfficeId, src => src.OfficeId)
            .Map(dest => dest.Email, src => src.Email)
            .Map(dest => dest.Photo, src => src.Photo)
            .Ignore(dest => dest.Photo.Headers);

        config.NewConfig<(UpdateReceptionistRequest, int), UpdateReceptionistCommand>()
            .Map(dest => dest.ReceptionistId, src => src.Item2)
            .Map(dest => dest.MiddleName, src => src.Item1.MiddleName)
            .Map(dest => dest.LastName, src => src.Item1.LastName)
            .Map(dest => dest.FirstName, src => src.Item1.FirstName)
            .Map(dest => dest.OfficeId, src => src.Item1.OfficeId)
            .Map(dest => dest.Photo, src => src.Item1.Photo)
            .Map(dest => dest.Email, src => src.Item1.Email);
    }
}
