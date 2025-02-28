using Serilog.Sinks.SystemConsole.Themes;

public class CustomConsoleTheme : ConsoleTheme
{
    public override bool CanBuffer => false;

    protected override int ResetCharCount => 0;

    public override void Reset(TextWriter output)
    {
        Console.ResetColor();
    }

    public override int Set(TextWriter output, ConsoleThemeStyle style)
    {
        switch (style)
        {
            case ConsoleThemeStyle.Text:
                Console.ForegroundColor = ConsoleColor.White;
                break;
            case ConsoleThemeStyle.SecondaryText:
                Console.ForegroundColor = ConsoleColor.Gray;
                break;
            case ConsoleThemeStyle.LevelVerbose:
                Console.ForegroundColor = ConsoleColor.Gray;
                break;
            case ConsoleThemeStyle.LevelDebug:
                Console.ForegroundColor = ConsoleColor.Cyan;
                break;
            case ConsoleThemeStyle.LevelInformation:
                Console.ForegroundColor = ConsoleColor.Green;
                break;
            case ConsoleThemeStyle.LevelWarning:
                Console.ForegroundColor = ConsoleColor.Yellow;
                break;
            case ConsoleThemeStyle.LevelError:
                Console.ForegroundColor = ConsoleColor.Red;
                break;
            case ConsoleThemeStyle.LevelFatal:
                Console.ForegroundColor = ConsoleColor.Magenta;
                break;
            default:
                Console.ForegroundColor = ConsoleColor.White;
                break;
        }
        return 0;
    }
}
