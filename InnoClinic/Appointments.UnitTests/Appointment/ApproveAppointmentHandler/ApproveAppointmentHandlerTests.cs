public class ApproveAppointmentHandlerTests
{
    private readonly Mock<IAppointmentsRepository> _repository;
    private readonly Mock<IUnitOfWork> _unitOfWork;
    private readonly ApproveAppointmentCommandHandler _handler;

    public ApproveAppointmentHandlerTests()
    {
        _unitOfWork = new Mock<IUnitOfWork>();
        _repository = new Mock<IAppointmentsRepository>();

        _unitOfWork.Setup(u => u.AppointmentsRepository)
            .Returns(_repository.Object);

        _handler = new ApproveAppointmentCommandHandler(_unitOfWork.Object);
    }

    [Fact]
    public async Task Handle_ValidRequest_ApprovesApointment()
    {
        //Arrange 
        var command = new ApproveAppointmentCommand(1);
        var appointment = new Appointment { Id = 1, IsApproved = false };

        _unitOfWork.Setup(u => u.AppointmentsRepository.GetAppointmentByIdAsync(command.AppointmentId, CancellationToken.None))
            .ReturnsAsync(appointment);
        _unitOfWork.Setup(u => u.SaveChangesAsync(CancellationToken.None)).Returns(Task.FromResult(1));

        //Act
        var result = await _handler.Handle(command, CancellationToken.None);

        //Assert
        Assert.False(result.IsError);
        Assert.Equal(true, appointment.IsApproved);
    }
}