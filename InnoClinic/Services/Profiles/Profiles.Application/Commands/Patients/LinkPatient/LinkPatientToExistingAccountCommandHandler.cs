public sealed class LinkPatientToExistingAccountCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<LinkPatientToExistingAccountCommand, ErrorOr<Unit>>
{
    public async Task<ErrorOr<Unit>> Handle(LinkPatientToExistingAccountCommand request, CancellationToken cancellationToken)
    {
        var matchedProfile = await unitOfWork.PatientsRepository.GetPatientByIdAsync(request.PatientProfileId, cancellationToken);
        
        if (matchedProfile is null)
        {
            return Errors.Patients.NotFound(request.PatientProfileId);
        }

        if (matchedProfile.IsLinkedToAccount)
        {
            return Errors.Patients.AlreadyLinked;
        }

        matchedProfile.IsLinkedToAccount = true;
        matchedProfile.AccountId = request.AccountId;

        await unitOfWork.PatientsRepository.UpdatePatientAsync(matchedProfile, cancellationToken);

        await unitOfWork.CompleteAsync(cancellationToken);

        return Unit.Value;
    }
}
