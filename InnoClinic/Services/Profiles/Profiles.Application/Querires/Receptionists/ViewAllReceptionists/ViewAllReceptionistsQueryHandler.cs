public class ViewAllReceptionistsQueryHandler(IUnitOfWork unitOfWork, IAccountHttpClient accountHttpClient)
    : IRequestHandler<ViewAllReceptionistsQuery, ErrorOr<List<ReceptionistListResponse>>>
{
    public async Task<ErrorOr<List<ReceptionistListResponse>>> Handle(ViewAllReceptionistsQuery request, CancellationToken cancellationToken)
    {
        var receptionists = await unitOfWork
            .ReceptionistsRepository
            .GetListReceptionistAsync(request.PageNumber, request.PageSize, cancellationToken);
            
        if (receptionists is null || !receptionists.Any())
        {
            return Errors.Receptionists.EmptyList;
        }

        var receptionistList = new List<ReceptionistListResponse>();

        foreach (var receptionist in receptionists)
        {
            var accountResponse = await accountHttpClient.GetAccountInfo(receptionist.AccountId);

            if (accountResponse.IsError)
            {
                return accountResponse.FirstError;
            }

            var account = accountResponse.Value;

            var office = await unitOfWork.OfficeRepository.GetOfficeByIdAsync(receptionist.OfficeId, cancellationToken);

            if (office is null)
            {
                return Errors.Office.NotFound;
            }

            receptionistList.Add(new ReceptionistListResponse
            {
                Id = receptionist.Id,
                FirstName = receptionist.FirstName,
                LastName = receptionist.LastName,
                MiddleName = receptionist.MiddleName,
                Email = account.Email,
                City = office.City,
                Street = office.Street,
                HouseNumber = office.HouseNumber,
                OfficeNumber = office.OfficeNumber
            });
        }

        return receptionistList;
    }
}
