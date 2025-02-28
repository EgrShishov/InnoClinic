using ErrorOr;
public class CreateAppointmentResultHandlerTests
{
    private readonly Mock<IAppointmentsResultRepository> _repository;
    private readonly Mock<IUnitOfWork> _unitOfWork;
    private readonly Mock<IProfilesHttpClient> _profilesHttpClient;
    private CreateAppointmentCommandHandler _handler;
    public CreateAppointmentResultHandlerTests()
    {
        _unitOfWork = new Mock<IUnitOfWork>();
        _repository = new Mock<IAppointmentsResultRepository>();
        _profilesHttpClient = new Mock<IProfilesHttpClient>();
        
        _unitOfWork.Setup(u => u.ResultsRepository)
            .Returns(_repository.Object);

        _handler = new CreateAppointmentCommandHandler(_unitOfWork.Object, _profilesHttpClient.Object);
    }

    [Fact]
    public async Task Handle_DoctorDoesNotExist_ReturnsNotFound()
    {
        var command = new CreateAppointmentCommand(1, 1, 1, 1, 1, DateTime.Now, TimeSpan.FromHours(10));

        _profilesHttpClient.Setup(client => client.DoctorExistsAsync(command.DoctorId))
            .ReturnsAsync(false);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.True(result.IsError);
        Assert.Equal(Error.NotFound(), result.FirstError);
    }

    [Theory]
    [InlineData(1, 2, 1, true, false, "Patients.NotFound")]
    [InlineData(2, 1, 1, false, true, "Doctors.NotFound")]
    public async Task Handle_InvalidProfiles_ReturnsNotFound(int doctorId, int patientId, int serviceId, bool doctorExists, bool patientExists, string expectedError)
    {
        var command = new CreateAppointmentCommand(patientId, 1, doctorId, serviceId, 1, DateTime.Now, TimeSpan.FromHours(10));

        _profilesHttpClient.Setup(client => client.DoctorExistsAsync(command.DoctorId))
            .ReturnsAsync(doctorExists);
        _profilesHttpClient.Setup(client => client.PatientExistsAsync(command.PatientId))
            .ReturnsAsync(patientExists);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.True(result.IsError);
        Assert.Equal(expectedError, result.FirstError.Code);
    }
}