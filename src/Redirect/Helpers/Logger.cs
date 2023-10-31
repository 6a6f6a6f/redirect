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
        AnsiConsole.MarkupLine($"[dim b]inf:[/] {escapedMessage}");    
    }

    public static void Map(ResponseMap responseMap)
    {
        var tree = new Tree(new Markup("[bold underline]Response Map:[/]"));

        AddResponseMapNode(responseMap, tree);
        AnsiConsole.Write(tree);
    }

    private static void AddResponseMapNode(ResponseMap map, IHasTreeNodes parentNode)
    {
        var node = parentNode.AddNode($"Source URI: [green]{Markup.Escape(map.SourceUri.ToString())}[/]");
        node.AddNode($"Destination URI: [green]{Markup.Escape(map.DestinationUri?.ToString() ?? "N/A")}[/]");
        node.AddNode($"Response Time: [green]{map.ResponseTime.TotalMilliseconds} ms[/]");
        node.AddNode($"Status Code: [green]{(int) map.StatusCode} [dim]({map.StatusCode})[/][/]");

        if (map.Headers.Any())
        {
            var headersNode = node.AddNode("Headers: ");
            foreach (var header in map.Headers)
            {
                headersNode.AddNode($"[bold]{Markup.Escape(header.Key)}[/]: {Markup.Escape(header.Value)}");
            }
        }

        if (map.Cookies.Any())
        {
            var cookiesNode = node.AddNode("Cookies: ");
            foreach (var cookie in map.Cookies)
            {
                cookiesNode.AddNode($"[bold]{Markup.Escape(cookie.Name)}[/]: {Markup.Escape(cookie.Value)}");
            }
        }

        if (map.QueryParameters.Any())
        {
            var queryParamsNode = node.AddNode("Query Parameters: ");
            foreach (var queryParameter in map.QueryParameters)
            {
                queryParamsNode.AddNode(
                    $"[bold]{Markup.Escape(queryParameter.Key)}[/]: " + 
                    $"{string.Join(", ", Markup.Escape(queryParameter.Value.ToString() ?? string.Empty))}");
            }
        }

        if (map.NextRedirect is null) return;
        AddResponseMapNode(map.NextRedirect, node);
    }
}