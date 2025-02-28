public class RescheduleAppointmentHandlerTests
{
    private readonly Mock<IAppointmentsRepository> _repositroy;
    private readonly Mock<IUnitOfWork> _unitOfWork;
    private RescheduleAppointmentCommandHandler _handler;
    public RescheduleAppointmentHandlerTests()
    {
        _unitOfWork = new Mock<IUnitOfWork>();
        _repositroy = new Mock<IAppointmentsRepository>();

        _unitOfWork.Setup(u => u.AppointmentsRepository)
            .Returns(_repositroy.Object);

        _handler = new RescheduleAppointmentCommandHandler(_unitOfWork.Object);
    }

    [Fact]
    public async Task Handle_AppointmentNotFound_ReturnsNotFound()
    {
        var command = new RescheduleAppointmentCommand(1, 1, DateTime.Now, TimeSpan.FromHours(10));

        _unitOfWork.Setup(u => u.AppointmentsRepository.GetAppointmentByIdAsync(command.AppointmentId, CancellationToken.None))
            .ReturnsAsync((Appointment)null);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.Equal(Errors.Appointments.NotFound, result.FirstError);
    }

    [Fact]
    public async Task Handle_AppointmentIsApproved_ReturnsRescheduleIsImpossibleBecauseIsApproved()
    {
        var command = new RescheduleAppointmentCommand(1, 1, DateTime.Now, TimeSpan.FromHours(10));
        var appointment = new Appointment { Id = 1, IsApproved = true };

        _unitOfWork.Setup(u => u.AppointmentsRepository.GetAppointmentByIdAsync(command.AppointmentId, CancellationToken.None))
            .ReturnsAsync(appointment);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.Equal(Errors.Appointments.RescheduleIsImpossibleBecauseIsApproved, result.FirstError);
    }

    [Fact]
    public async Task Handle_SuccessfulReschedule_ReturnsAppointment()
    {
        var command = new RescheduleAppointmentCommand(1, 1, DateTime.Now, TimeSpan.FromHours(2));
        var appointment = new Appointment { Id = 1, IsApproved = false };

        _unitOfWork.Setup(u => u.AppointmentsRepository.GetAppointmentByIdAsync(command.AppointmentId, CancellationToken.None))
            .ReturnsAsync(appointment);

        _unitOfWork.Setup(u => u.AppointmentsRepository.UpdateAppointmentAsync(It.IsAny<Appointment>(), It.IsAny<CancellationToken>()));
        _unitOfWork.Setup(u => u.SaveChangesAsync(CancellationToken.None)).Returns(Task.CompletedTask);
        _unitOfWork.Setup(u => u.BeginTransactionAsync(It.IsAny<CancellationToken>()));
        _unitOfWork.Setup(u => u.CommitTransactionAsync(It.IsAny<CancellationToken>()));
        _unitOfWork.Setup(u => u.RollbackTransactionAsync(It.IsAny<CancellationToken>()));

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.False(result.IsError);
        Assert.Equal(appointment, result.Value);
    }
}