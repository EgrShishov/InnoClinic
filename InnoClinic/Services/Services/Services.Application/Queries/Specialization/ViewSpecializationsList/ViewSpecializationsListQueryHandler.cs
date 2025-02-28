public class ViewSpecializationsListQueryHandler(IUnitOfWork unitOfWork) 
    : IRequestHandler<ViewSpecializationsListQuery, ErrorOr<List<SpecializationResponse>>>
{
    public async Task<ErrorOr<List<SpecializationResponse>>> Handle(ViewSpecializationsListQuery request, CancellationToken cancellationToken)
    {
        var specializations = await unitOfWork.Specializations.GetAllAsync(cancellationToken);
        
        if (specializations is null)
        {
            return Errors.Specialization.EmptySpecializationsList;
        }

        var specializationsList = new List<SpecializationResponse>();

        foreach (var specialization in specializations)
        {
            specializationsList.Add(new SpecializationResponse
            {
                Id = specialization.Id,
                SpecializationName = specialization.SpecializationName,
                IsActive = specialization.IsActive,
            });
        }

        return specializationsList;
    }
}
