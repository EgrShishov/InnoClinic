public class EditAppointmentResultHandlerTests
{
    private readonly Mock<IAppointmentsResultRepository> _repository;
    private readonly Mock<IUnitOfWork> _unitOfWork;
    private EditAppointmentResultsCommandHandler _handler;
    public EditAppointmentResultHandlerTests()
    {
        _unitOfWork = new Mock<IUnitOfWork>();
        _repository = new Mock<IAppointmentsResultRepository>();

        _unitOfWork.Setup(u => u.ResultsRepository)
            .Returns(_repository.Object);

        _handler = new EditAppointmentResultsCommandHandler(_unitOfWork.Object);
    }

    [Theory]
    [MemberData(nameof(ResultsIdExpectedResult))]
    public async Task Handle_ResultsDoesNotExist_ReturnsNotFound(int resultsId, string expectedResult)
    {
        var command = new EditAppointmentsResultCommand(resultsId, "", "", "");

        _unitOfWork.Setup(u => u.ResultsRepository.GetResultsByIdAsync(command.ResultsId, CancellationToken.None))
            .ReturnsAsync((Results)null);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.Equal(expectedResult, result.FirstError.Code);
    }

    public static IEnumerable<object[]> ResultsIdExpectedResult()
    {
        yield return new object[] { 1, Errors.Results.NotFound.Code };
        yield return new object[] { 2, Errors.Results.NotFound.Code };
    }
}