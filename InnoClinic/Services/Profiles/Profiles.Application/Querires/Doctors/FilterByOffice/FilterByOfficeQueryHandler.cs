public class FilterByOfficeQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<FilterByOfficeQuery, ErrorOr<List<Doctor>>>
{
    public async Task<ErrorOr<List<Doctor>>> Handle(FilterByOfficeQuery request, CancellationToken cancellationToken)
    {
        var office = await unitOfWork.OfficeRepository.GetOfficeByIdAsync(request.OfficeId);

        if (office is null)
        {
            return Errors.Office.NotFound;
        }

        var doctors = await unitOfWork
            .DoctorsRepository
            .ListDoctorsAsync(d => d.OfficeId == request.OfficeId, request.PageNumber, request.PageSize, cancellationToken);
        
        if (doctors is null || !doctors.Any())
        {
            return Errors.Doctors.EmptyList;
        }

        return doctors;
    }
}
