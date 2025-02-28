public interface IProfilesHttpClient
{
    public Task<ErrorOr<PatientProfileResponse>> GetPatientAsync(int patientId);
    public Task<ErrorOr<DoctorProfileResponse>> GetDoctorAsync(int doctorId);
}
