using Microsoft.AspNetCore.Http;

public class CreateDoctorRequest
{
    public IFormFile Photo { get; init; }
    public string FirstName {  get; init; }
    public string LastName {  get; init; }
    public string MiddleName {  get; init; }
    public DateTime DateOfBirth {  get; init; }
    public string Email { get; init; }
    public int SpecializationId {  get; init; }
    public string OfficeId {  get; init; }
    public int CareerStartYear { get; init; }
    public ProfileStatus Status {  get; init; }
}
