public class ViewTimeSlotsQueryValidator : AbstractValidator<ViewTimeSlotsQuery>
{
    public ViewTimeSlotsQueryValidator()
    {
        RuleFor(x => x.ServiceId).GreaterThan(0);
        RuleFor(x => x.AppointmentDate).NotEmpty().WithMessage("Please, select the date");
    }
}
