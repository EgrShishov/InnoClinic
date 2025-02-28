public class DownloadResultsHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWork;
    private readonly Mock<IAppointmentsResultRepository> _repository;
    private readonly Mock<IPDFDocumentGenerator> _documentGenerator;
    private DownloadAppointmentResultsQueryHandler _handler;
    public DownloadResultsHandlerTests()
    {
        _unitOfWork = new Mock<IUnitOfWork>();
        _repository = new Mock<IAppointmentsResultRepository>();
        _documentGenerator = new Mock<IPDFDocumentGenerator>();

        _unitOfWork.Setup(u => u.ResultsRepository)
            .Returns(_repository.Object);

        _handler = new DownloadAppointmentResultsQueryHandler(_unitOfWork.Object, _documentGenerator.Object);
    }

    [Fact]
    public async Task Handle_PDFGenerationFailed_ReturnsGenerationError()
    {
        var query = new DownloadAppointmentResultsQuery(1);
        var appointmentResult = new Results
        {
            Id = 1
        };

        _unitOfWork.Setup(u => u.ResultsRepository.GetResultsByIdAsync(query.ResultsId, CancellationToken.None))
            .ReturnsAsync(appointmentResult);
        _documentGenerator.Setup(d => d.GenerateAppointmentResults(new GeneratePDFResultsRequest
        {
            ServiceName = "Test"
        }))
            .Returns((byte[])null);

        var result = await _handler.Handle(query, CancellationToken.None);

        Assert.Equal(Errors.Results.CannotGeneratePDF.Code, result.FirstError.Code);
    }

    [Fact]
    public async Task Handle_ResultsDoesNotExist_ReturnsResultsNotFoundError()
    {
        var query = new DownloadAppointmentResultsQuery(1);

        _unitOfWork.Setup(u => u.ResultsRepository.GetResultsByIdAsync(query.ResultsId, CancellationToken.None))
            .ReturnsAsync((Results)null);

        var result = await _handler.Handle(query, CancellationToken.None);

        Assert.Equal(Errors.Results.NotFound.Code, result.FirstError.Code);
    }
}