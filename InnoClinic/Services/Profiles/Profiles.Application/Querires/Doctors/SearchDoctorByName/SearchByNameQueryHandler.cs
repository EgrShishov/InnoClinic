public class SearchByNameQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<SearchByNameQuery, ErrorOr<List<Doctor>>>
{
    public async Task<ErrorOr<List<Doctor>>> Handle(SearchByNameQuery request, CancellationToken cancellationToken)
    {
        var doctors = await unitOfWork
            .DoctorsRepository
            .ListDoctorsAsync(d => d.FirstName == request.FirstName
                                && d.LastName == request.LastName 
                                && d.MiddleName == request.MiddleName);
        
        if (doctors is null || !doctors.Any())
        {
            return Errors.Doctors.NotFoundByFullName;
        }
        
        return doctors;
    }
}
