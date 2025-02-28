using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

public class AppointmentsRepository : IAppointmentsRepository
{
    private readonly AppointmentsDbContext _context;

    public AppointmentsRepository(AppointmentsDbContext context)
    {
        _context = context;
    }
    public async Task<Appointment> AddAppointmentAsync(Appointment appointment, CancellationToken cancellationToken = default)
    {
        var newAppointment = await _context.Appointments.AddAsync(appointment, cancellationToken);
        return newAppointment.Entity;
    }

    public async Task DeleteAppointmentAsync(int id, CancellationToken cancellationToken = default)
    {
        var appointment = await _context.Appointments.FirstOrDefaultAsync(x => x.Id == id);
        if (appointment != null)
        {
            _context.Entry(appointment).State = EntityState.Deleted;
        }
    }

    public async Task<IEnumerable<Appointment>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Appointments.ToListAsync(cancellationToken);
    }

    public async Task<Appointment> GetAppointmentByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Appointments.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<IEnumerable<Appointment>> ListAsync(Expression<Func<Appointment, bool>> filter, CancellationToken cancellationToken = default)
    {
        var query = _context.Appointments.AsQueryable();

        if (filter != null)
        {
            query = query.Where(filter);
        }

        return await query.ToListAsync();
    }

    public Task<Appointment> UpdateAppointmentAsync(Appointment appointment, CancellationToken cancellationToken = default)
    {
        _context.Entry(appointment).State = EntityState.Modified;
        return Task.FromResult(appointment);
    }
}
