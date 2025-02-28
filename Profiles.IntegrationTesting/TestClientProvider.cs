using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.VisualStudio.TestPlatform.TestHost;

public class TestClientProvider
{
    public HttpClient _client { get; private set; }
    public TestClientProvider()
    {
        var server = new TestServer(new WebHostBuilder().UseStartup<Program>());

        _client = server.CreateClient();
    }
}
