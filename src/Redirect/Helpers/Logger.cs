namespace Redirect.Helpers;

internal static class Logger
{
    public static void LogWarning(string message)
    {
        var escapedMessage = Markup.Escape(message);
        AnsiConsole.MarkupLine($"[b yellow]war:[/] {escapedMessage}"); 
    }

    public static void LogError(string message)
    {
        var escapedMessage = Markup.Escape(message);
        AnsiConsole.MarkupLine($"[b red]err:[/] {escapedMessage}"); 
    }
    
    public static void LogSuccess(string message)
    {
        var escapedMessage = Markup.Escape(message);
        AnsiConsole.MarkupLine($"[b green]suc:[/] {escapedMessage}"); 
    }
    
    public static void LogException(string message, Exception? exception = default)
    {
        var escapedMessage = Markup.Escape(message);
        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine($"[b red]err: {escapedMessage}[/]");
        
        if (exception is null) return;
        AnsiConsole.WriteException(exception);
        
        Environment.Exit(1);
    }
    
    public static void LogInfo(string message)
    {
        var escapedMessage = Markup.Escape(message);
        AnsiConsole.MarkupLine($"[dim][b]inf:[/] {escapedMessage}[/]");    
    }

    public static void EmptyLine() => AnsiConsole.WriteLine();
}