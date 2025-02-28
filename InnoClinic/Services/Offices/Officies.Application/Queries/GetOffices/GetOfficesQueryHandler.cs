public class GetOfficesQueryHandler(IOfficeRepository repository) : IRequestHandler<GetOfficesQuery, ErrorOr<List<OfficeListResponse>>>
{
    public async Task<ErrorOr<List<OfficeListResponse>>> Handle(GetOfficesQuery request, CancellationToken cancellationToken)
    {
        var offices = await repository.GetOfficesAsync();
        
        if (offices is null || !offices.Any())
        {
            return Errors.Offices.EmptyOfficesList;
        }

        var officesList = new List<OfficeListResponse>();

        foreach(var office in offices)
        {
            officesList.Add(new OfficeListResponse
            {
                Id = office.Id.ToString(),
                Address = office.Address,
                RegistryPhoneNumber = office.RegistryPhoneNumber,
                IsActive = office.IsActive
            });
        }

        return officesList;
    }
}
