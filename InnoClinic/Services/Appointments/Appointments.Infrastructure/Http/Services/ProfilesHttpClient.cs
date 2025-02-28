using System.Net.Http.Json;
using ErrorOr;

public class ProfilesHttpClient : IProfilesHttpClient
{
    private readonly IHttpClientFactory _factory;
    private readonly HttpClient _httpClient;

    public ProfilesHttpClient(IHttpClientFactory factory)
    {
        _factory = factory;
        _httpClient = _factory.CreateClient("profiles");
    }

    public async Task<ErrorOr<DoctorProfileResponse>> GetDoctorAsync(int doctorId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}/Doctors/profile/{doctorId}");

            if (!response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();

                return Error.Failure(description: responseBody);
            }

            return await response.Content.ReadFromJsonAsync<DoctorProfileResponse>();
        }
        catch (Exception ex)
        {
            return Error.Failure(code: "An error occurred while getting info about doctors profile", description: ex.Message);
        }
    }
    public async Task<ErrorOr<PatientProfileResponse>> GetPatientAsync(int patientId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}/Patients/profile/{patientId}");

            if (!response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();

                return Error.Failure(description: responseBody);
            }

            return await response.Content.ReadFromJsonAsync<PatientProfileResponse>();
        }
        catch (Exception ex)
        {
            return Error.Failure(code: "An error occurred while getting info about patient profile", description: ex.Message);

        }
    }
}
