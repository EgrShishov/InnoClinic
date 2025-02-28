using System.Linq.Expressions;

public interface IAppointmentsRepository
{
    public Task<Appointment> GetAppointmentByIdAsync(int id, CancellationToken cancellationToken = default);
    public Task<IEnumerable<Appointment>> GetAllAsync(CancellationToken cancellationToken = default);
    public Task<IEnumerable<Appointment>> ListAsync(Expression<Func<Appointment, bool>> filter, CancellationToken cancellationToken = default);
    public Task<Appointment> AddAppointmentAsync(Appointment appointment, CancellationToken cancellationToken = default);
    public Task<Appointment> UpdateAppointmentAsync(Appointment appointment, CancellationToken cancellationToken = default);
    public Task DeleteAppointmentAsync(int id, CancellationToken cancellationToken = default);
}
