using GlobalServices.Interfaces;

namespace GlobalServices;

public class Logger : ILogger
{
    public string Logs { get; private set; }
    private enum LogLevel
    {
        Info,
        Warning,
        Error
    }

    public Logger()
    {
        Logs = string.Empty;
    }
    public void LogError(string message)
    {
        var keyword = "Error: ";
        Log(ConsoleColor.Red, keyword + message);
    }

    public void LogInfo(string message)
    {
        var keyword = "Info: ";
        Log(ConsoleColor.Blue, keyword + message);
    }

    public void LogWarning(string message)
    {
        var keyword = "Warning: ";
        Log(ConsoleColor.Yellow, keyword + message);
    }

    private void Log(ConsoleColor color, string message)
    {
        var stringColor = color.ToString().ToLower();
        Logs += $"\n [color={stringColor}][{DateTime.Now}] {message}[/color]";
        Console.ForegroundColor = color;
        Console.WriteLine($"[{DateTime.Now}] {message}");
        Console.ResetColor();
    }
}