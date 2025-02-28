public class ViewServicesInfoQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<ViewServicesInfoQuery, ErrorOr<ServiceInfoResponse>>
{
    public async Task<ErrorOr<ServiceInfoResponse>> Handle(ViewServicesInfoQuery request, CancellationToken cancellationToken)
    {
        var service = await unitOfWork.Services.GetServiceByIdAsync(request.Id);

        if (service is null)
        {
            return Errors.Service.NotFound;
        }

        return new ServiceInfoResponse
        {
            Id = service.Id,
            ServiceCategoryName = service.ServiceCategory.ToString(),
            ServiceName = service.ServiceName,
            ServicePrice = service.ServicePrice,
            IsActive = service.IsActive,
        };
    }
}
