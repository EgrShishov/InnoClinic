public class ViewServicesQueryHandler(IUnitOfWork unitOfWork) 
    : IRequestHandler<ViewServicesQuery, ErrorOr<List<ServiceResponse>>>
{
    public async Task<ErrorOr<List<ServiceResponse>>> Handle(ViewServicesQuery request, CancellationToken cancellationToken)
    {
        var services = await unitOfWork.Services.GetAllAsync(cancellationToken);

        if (services is null || !services.Any())
        {
            return Errors.Service.EmptyServicesList;
        }

        var activeServices = await Task.WhenAll(services
            .Where(s => s.IsActive)
            .Select(async service => new
            {
                Service = service,
                Specialization = await unitOfWork.Specializations.GetSpecializationByIdAsync(service.SpecializationId)
            }));

        var validServices = activeServices
            .Where(s => s.Specialization!= null && s.Specialization.IsActive)
            .Select(s => new ServiceResponse
            {
                ServiceCategoryName = s.Service.ServiceCategory.ToString(),
                ServiceName = s.Service.ServiceName,
                ServicePrice = s.Service.ServicePrice,
                Specialization = s.Specialization.SpecializationName
            });

        return validServices.Any() ? validServices.ToList() : Errors.Service.NoActiveServices;
    }
}
