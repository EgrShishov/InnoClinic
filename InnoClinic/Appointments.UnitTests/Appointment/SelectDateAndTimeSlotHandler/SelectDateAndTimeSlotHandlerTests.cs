using ErrorOr;

public class SelectDateAndTimeSlotHandlerTests
{
    private readonly Mock<IAppointmentsRepository> _repository;
    private readonly Mock<IUnitOfWork> _unitOfWork;
    private readonly Mock<ITimeSlotsGenerator> _timeSlotsGenerator;
    private ViewTimeSlotsQueryHandler _handler;
    public SelectDateAndTimeSlotHandlerTests()
    {
        _unitOfWork = new Mock<IUnitOfWork>();
        _repository = new Mock<IAppointmentsRepository>();
        _timeSlotsGenerator = new Mock<ITimeSlotsGenerator>();

        _unitOfWork.Setup(u => u.AppointmentsRepository)
            .Returns(_repository.Object);

        _handler = new ViewTimeSlotsQueryHandler(_unitOfWork.Object, _timeSlotsGenerator.Object);
    }

    [Fact]
    public async Task Handle_ServiceNotFound_ReturnsNotFound()
    {
        var command = new ViewTimeSlotsQuery(1, 1, 1, DateTime.Now, TimeSpan.FromHours(10));

        _unitOfWork.Setup(u => u.ServiceRepository.GetServiceByIdAsync(command.ServiceId, CancellationToken.None))
            .ReturnsAsync((Service)null);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.Equal(Error.NotFound(), result.FirstError);
    }

    [Fact]
    public async Task Handle_NoAvailableTimeSlots_ReturnsNotFound()
    {
        var command = new ViewTimeSlotsQuery(1, 1, 1, DateTime.Now, TimeSpan.FromHours(10));
        var service = new Service { Id = 1, ServiceCategory = ServiceCategory.Analyses, ServiceName = "TestService" };

        _unitOfWork.Setup(u => u.ServiceRepository.GetServiceByIdAsync(command.ServiceId, CancellationToken.None))
            .ReturnsAsync(service);

        _timeSlotsGenerator.Setup(g => g.GenerateSlots(command.AppointmentDate, It.IsAny<TimeSpan>(), It.IsAny<TimeSpan>(), service.ServiceCategory))
            .ReturnsAsync(new List<TimeSlotResponse>());

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.Equal(Error.NotFound(), result.FirstError);
    }
}