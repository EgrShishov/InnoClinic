public class CreateSpecializationCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<CreateSpecializationCommand, ErrorOr<SpecializationResponse>>
{
    public async Task<ErrorOr<SpecializationResponse>> Handle(CreateSpecializationCommand request, CancellationToken cancellationToken)
    {
        var specialization = new Specialization
        {
            SpecializationName = request.SpecializationName,
            IsActive = request.IsActive
        };
        
        var newSpecialization = await unitOfWork.Specializations.AddSpecializationAsync(specialization, cancellationToken);
        
        if (newSpecialization is null)
        {
            return Errors.Specialization.AddingError;
        }

        await unitOfWork.SaveChangesAsync();

        return new SpecializationResponse
        {
            Id = specialization.Id,
            SpecializationName = specialization.SpecializationName,
            IsActive = specialization.IsActive
        };
    }
}