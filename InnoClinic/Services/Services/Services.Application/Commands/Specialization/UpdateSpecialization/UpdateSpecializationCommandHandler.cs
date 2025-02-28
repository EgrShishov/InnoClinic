public class UpdateSpecializationCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateSpecializationCommand, ErrorOr<SpecializationInfoResponse>>
{
    public async Task<ErrorOr<SpecializationInfoResponse>> Handle(UpdateSpecializationCommand request, CancellationToken cancellationToken)
    {
        var specialization = await unitOfWork.Specializations.GetSpecializationByIdAsync(request.Id);
        
        if (specialization is null) 
        {
            return Errors.Specialization.NotFound;
        }

        specialization.SpecializationName = request.SpecializationName;
        specialization.IsActive = request.IsActive;

        await unitOfWork.Specializations.UpdateSpecializationAsync(specialization);

        var allServices = await unitOfWork.Services.GetAllAsync(cancellationToken);
        
        var relatedServices = new List<ServiceInfoResponse>();

        foreach (var service in allServices)
        {
            if (service.SpecializationId != specialization.Id)
            {
                continue;
            }

            relatedServices.Add(new ServiceInfoResponse
            {
                Id = service.Id,
                IsActive = service.IsActive,
                ServiceCategoryName = service.ServiceCategory.ToString(),
                ServiceName = service.ServiceName,
                ServicePrice = service.ServicePrice
            });

        }

        return new SpecializationInfoResponse
        {
            SpecializationName = specialization.SpecializationName,
            SpecializationStatus = specialization.IsActive ? "Active" : "Inactive",
            RelatedServices = relatedServices
        };
    }
}
