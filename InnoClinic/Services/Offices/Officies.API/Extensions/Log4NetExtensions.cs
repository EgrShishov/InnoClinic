public static class Log4NetExtensions
{
    public static void AddLog4Net(this IServiceCollection services)
    {
        XmlConfigurator.Configure(new FileInfo("log4net.config"));
        services.AddSingleton(LogManager.GetLogger(typeof(Program)));
    }
}
