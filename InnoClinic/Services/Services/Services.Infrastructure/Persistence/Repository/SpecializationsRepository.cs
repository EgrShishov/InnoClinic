using DapperExtensions;

public class SpecializationsRepository : ISpecializationsRepository
{
    private readonly ISqlConnectionFactory _connectionFactory;

    public SpecializationsRepository(ISqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<Specialization> AddSpecializationAsync(Specialization specialization, CancellationToken cancellationToken = default)
    {
        const string query = @"
            INSERT INTO Specializations (SpecializationName, IsActive)
            VALUES ( 
            @SpecializationName, 
            @IsActive)
            RETURNING Id;";

        await using var sqlConnection = _connectionFactory.CreateConnection();

        var id = await sqlConnection.QuerySingleAsync<int>(query,
            new
            {
                specialization.SpecializationName,
                specialization.IsActive
            });

        specialization.Id = id;

        return specialization;
    }

    public async Task DeleteSpecializationAsync(int id, CancellationToken cancellationToken = default)
    {
        const string query = @"DELETE FROM Specializations WHERE Id = @Id";

        await using var sqlConnection = _connectionFactory.CreateConnection();

        await sqlConnection.ExecuteAsync(query, new {Id = id});
    }

    public async Task<IEnumerable<Specialization>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        const string query = @"SELECT * FROM Specializations;";

        await using var sqlConnection = _connectionFactory.CreateConnection();

        return await sqlConnection.QueryAsync<Specialization>(query);
    }

    public async Task<Specialization> GetSpecializationByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        const string query = @"SELECT * FROM Specializations WHERE Id = @Id;";

        await using var sqlConnection = _connectionFactory.CreateConnection();

        return await sqlConnection.QuerySingleOrDefaultAsync<Specialization>(query, new
        {
            Id = id
        });
    }

    public async Task UpdateSpecializationAsync(Specialization specialization, CancellationToken cancellationToken = default)
    {
        const string query = @"
            UPDATE Specializations
            SET SpecializationName = @SpecializationName, IsActive = @IsActive
            WHERE Id = @Id;";

        await using var sqlConnection = _connectionFactory.CreateConnection();

        var affectedRows = await sqlConnection.ExecuteAsync(query, new
        {
            specialization.SpecializationName,
            specialization.Id,
            specialization.IsActive
        });

        if (affectedRows == 0)
        {
            throw new Exception("No specialization found with the given Id.");
        }
    }
}
