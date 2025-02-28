public class FilterBySpecializationQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<FilterBySpecializationQuery, ErrorOr<List<Doctor>>>
{
    public async Task<ErrorOr<List<Doctor>>> Handle(FilterBySpecializationQuery request, CancellationToken cancellationToken)
    {
        var doctors = await unitOfWork
            .DoctorsRepository
            .ListDoctorsAsync(d => d.SpecializationId == request.SpecializationId, request.PageNumber, request.PageSize);

        if (doctors is null || !doctors.Any())
        {
            return Errors.Doctors.EmptyList;
        }

        return doctors;
    }
}
