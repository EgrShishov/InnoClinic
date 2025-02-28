public sealed class DeleteSpecializationCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteSpecializationCommand, ErrorOr<Unit>>
{
    public async Task<ErrorOr<Unit>> Handle(DeleteSpecializationCommand request, CancellationToken cancellationToken)
    {
        var specialization = await unitOfWork.Specializations.GetSpecializationByIdAsync(request.Id);

        if (specialization is null)
        {
            return Errors.Specialization.NotFound;
        }

        await unitOfWork.Specializations.DeleteSpecializationAsync(request.Id, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
