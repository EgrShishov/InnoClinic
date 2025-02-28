public class CreateAppointmentHandlerTests
{
    private readonly Mock<IAppointmentsRepository> _repository;
    private readonly Mock<IUnitOfWork> _unitOfWork;
    private readonly Mock<IProfilesHttpClient> _profilesHttpClient;
    private CreateAppointmentCommandHandler _handler;
    public CreateAppointmentHandlerTests()
    {
        _unitOfWork = new Mock<IUnitOfWork>();
        _repository = new Mock<IAppointmentsRepository>();
        _profilesHttpClient = new Mock<IProfilesHttpClient>();

        _unitOfWork.Setup(u => u.AppointmentsRepository)
            .Returns(_repository.Object);

        _handler = new CreateAppointmentCommandHandler(_unitOfWork.Object, _profilesHttpClient.Object);
    }

    [Fact]
    public async Task Handle_ValidRequest_CreatesAppointment()
    {
        var command = new CreateAppointmentCommand(1, 1, 1, 1, 1, DateTime.Now, TimeSpan.FromHours(10));
        var service = new Service { Id = 1, ServiceName = "Test Service" };
        var appointment = new Appointment { Id = 1, Date = command.Date, PatientId = command.PatientId };

        _unitOfWork.Setup(u => u.AppointmentsRepository.AddAppointmentAsync(It.IsAny<Appointment>(), CancellationToken.None))
            .ReturnsAsync(appointment);
        _unitOfWork.Setup(u => u.ServiceRepository.GetServiceByIdAsync(command.ServiceId, CancellationToken.None))
            .ReturnsAsync(service);
        _unitOfWork.Setup(u => u.SaveChangesAsync(CancellationToken.None))
            .Returns(Task.CompletedTask);

        _profilesHttpClient.Setup(client => client.DoctorExistsAsync(command.DoctorId))
            .ReturnsAsync(true);
        _profilesHttpClient.Setup(client => client.PatientExistsAsync(command.PatientId))
            .ReturnsAsync(true);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.False(result.IsError);
        Assert.NotNull(result.Value);
        Assert.Equal(appointment.Date, result.Value.Date);
    }
}