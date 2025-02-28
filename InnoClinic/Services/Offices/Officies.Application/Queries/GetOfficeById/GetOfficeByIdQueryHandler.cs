public class GetOfficeByIdQueryHandler(IUnitOfWork unitOfWork) 
    : IRequestHandler<GetOfficeByIdQuery, ErrorOr<OfficeResponse>>
{
    public async Task<ErrorOr<OfficeResponse>> Handle(GetOfficeByIdQuery request, CancellationToken cancellationToken)
    {
        var office = await unitOfWork.OfficeRepository.GetOfficeByIdAsync(request.Id, cancellationToken);
        
        if (office is null)
        {
            return Errors.Offices.NotFound;
        }

        return new OfficeResponse
        {
            Id = office.Id.ToString(),
            Address = office.Address,
            PhotoUrl = office.PhotoId,
            RegistryPhoneNumber = office.RegistryPhoneNumber,
            IsActive = office.IsActive
        };
    }
}
