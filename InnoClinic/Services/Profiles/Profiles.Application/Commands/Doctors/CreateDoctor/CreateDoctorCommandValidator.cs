public class CreateDoctorCommandValidator : AbstractValidator<CreateDoctorCommand>
{
    public CreateDoctorCommandValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty().WithMessage("Please, enter the first name");
        RuleFor(x => x.LastName).NotEmpty().WithMessage("Please, enter the last name");
        RuleFor(x => x.MiddleName).NotEmpty().WithMessage("Please, enter the middle name");
        RuleFor(x => x.DateOfBirth)
            .NotEmpty().WithMessage("Please, select the date")
            .LessThanOrEqualTo(DateTime.Now).WithMessage("Date of birth cannot be in the future");
        RuleFor(x => x.SpecializationId).NotEmpty().WithMessage("Please, choose the specialization");
        RuleFor(x => x.OfficeId).NotEmpty().WithMessage("Please, choose the office");
        RuleFor(x => x.CareerStartYear)
            .NotEmpty().WithMessage("Please, select the year")
            .LessThanOrEqualTo(DateTime.Now.Year).WithMessage("Career start year cannot be in the future");
        RuleFor(x => x.Status).NotEmpty().WithMessage("Please, select the status");
    }
}
