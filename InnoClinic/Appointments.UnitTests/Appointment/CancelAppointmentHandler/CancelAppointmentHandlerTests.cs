public class CancelAppointmentHandlerTests
{
    private readonly Mock<IAppointmentsRepository> _repository;
    private readonly Mock<IUnitOfWork> _unitOfWork;
    private CancelAppointmentCommandHandler _handler;
    public CancelAppointmentHandlerTests()
    {
        _unitOfWork = new Mock<IUnitOfWork>();
        _repository = new Mock<IAppointmentsRepository>();

        _unitOfWork.Setup(u => u.AppointmentsRepository)
            .Returns(_repository.Object);

        _handler = new CancelAppointmentCommandHandler(_unitOfWork.Object);
    }

    [Fact]
    public async Task Handle_ValidRequest_CancelAppointment()
    {
        var command = new CancelAppointmentCommand(1);
        var appointment = new Appointment { Id = 1, IsApproved = false };

        _unitOfWork.Setup(u => u.AppointmentsRepository.GetAppointmentByIdAsync(command.AppointmentId, CancellationToken.None))
            .ReturnsAsync(appointment);
        _unitOfWork.Setup(u => u.SaveChangesAsync(CancellationToken.None))
            .Returns(Task.CompletedTask);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.False(result.IsError);
        Assert.Equal(false, appointment.IsApproved);
    }
}