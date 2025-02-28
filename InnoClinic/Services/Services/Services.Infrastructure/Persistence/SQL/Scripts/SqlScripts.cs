public static class SqlScripts
{
    public const string CreateServicesTable = @"
        CREATE TABLE IF NOT EXISTS Services (
            Id SERIAL PRIMARY KEY,
            ServiceCategory INT NOT NULL,
            ServiceName VARCHAR(255) NOT NULL,
            ServicePrice DECIMAL(18,2) NOT NULL,
            SpecializationId INT NOT NULL,
            IsActive BOOLEAN NOT NULL,
            FOREIGN KEY (SpecializationId) REFERENCES Specializations(Id)
        );";

    public const string CreateSpecializationsTables = @"
        CREATE TABLE IF NOT EXISTS Specializations (
            Id SERIAL PRIMARY KEY,
            SpecializationName VARCHAR(255) NOT NULL,
            IsActive BOOLEAN NOT NULL
        );";
}
