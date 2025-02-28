using Microsoft.AspNetCore.Http;

public class ReceptionistTests : BaseIntegrationTest
{
    public ReceptionistTests(IntegrationTestWebAppFactory factory)
        : base(factory)
    {

    }

    [Fact]
    public async Task Create_ShouldAdd_NewReceptionistToDatabase()
    {
        // Arrange
        var command = new CreateReceptionistCommand("Name", "Last", "Middle", "email@yandex.ru", "1", null);

        // Act
        await _sender.Send(command);
        
        // Assert
    }
}
