var uriArgument = new Argument<string>("uri") {Description = "Target URI to check for redirects"};

var timeoutOption = new Option<int>(new[] {"-t", "--timeout"}, () => 5)
{
    Description = "Timeout duration in seconds",
    ArgumentHelpName = "count",
    IsRequired = true
};

var maxRedirectOption = new Option<int>(new[] {"-m", "--max-redirect"}, () => 5)
{
    Description = "Maximum number of redirects to follow",
    ArgumentHelpName = "count",
    IsRequired = true
};

var proxyOption = new Option<string>(new[] {"-p", "--proxy"})
{
    Description = "Proxy to use for the request",
    ArgumentHelpName = "host:port"
};

var headersOption = new Option<string[]>(new[] {"-h", "--header"})
{
    Description = "Custom header to add to the request",
    ArgumentHelpName = "name:value"
};

var ignoreCertErrorsOption = new Option<bool>(new[] {"-i", "--ignore-certificate-errors"}, () => true)
{
    Description = "Ignore HTTPS-related certificate errors"
};

var rootCommand = new RootCommand("A simple tool to check redirect chain information on shortened URLs.")
{
    uriArgument,
    timeoutOption,
    headersOption,
    maxRedirectOption,
    proxyOption,
    ignoreCertErrorsOption
};

rootCommand.Name = "redirect";
rootCommand.SetHandler(context =>
{
    var uri = context.ParseResult.GetValueForArgument(uriArgument);
    var timeout = context.ParseResult.GetValueForOption(timeoutOption);
    var maxRedirect = context.ParseResult.GetValueForOption(maxRedirectOption);
    var proxy = context.ParseResult.GetValueForOption(proxyOption);
    var ignoreCertErrors = context.ParseResult.GetValueForOption(ignoreCertErrorsOption);
    var headers = context.ParseResult.GetValueForOption(headersOption);
    
    if (!Uri.TryCreate(uri, UriKind.Absolute, out var parsedUri))
    {
        Logger.LogError("Invalid URI specified.");
        context.ExitCode = 1;
        return;
    }
    
    if (parsedUri.Scheme is not ("http" or "https"))
    {
        Logger.LogError("Invalid URI scheme specified. Only HTTP and HTTPS are supported.");
        context.ExitCode = 1;
        return;
    }

    if (timeout < 1)
    {
        Logger.LogError("Timeout must be at least 1 second.");
        context.ExitCode = 1;
        return;
    }

    if (!string.IsNullOrEmpty(proxy) && !proxy.Contains(':') && proxy.Split(':').Length != 2)
    {
        Logger.LogError("Invalid proxy specified. Must be in the format of host:port.");
        context.ExitCode = 1;
        return;
    }

    var parsedHeaders = new Dictionary<string, string>();
    headers?.ToList().ForEach(header =>
    {
        var split = header.Split(':');
        if (split.Length != 2)
        {
            Logger.LogError($"Invalid header specified: {header}. Must be in the format of name:value.");
            context.ExitCode = 1;
            return;
        }
        
        parsedHeaders.Add(split[0], split[1]);
    });
    
    var settings = new CommandSettings(parsedUri)
    {
        Timeout = TimeSpan.FromSeconds(timeout),
        MaxRedirects = maxRedirect,
        Proxy = proxy ?? string.Empty,
        IgnoreCertificateErrors = ignoreCertErrors,
        Headers = parsedHeaders
    };

    var arrow = Arrow
        .From(settings.Target)
        .WithMaxRedirects(settings.MaxRedirects)
        .WithTimeout(settings.Timeout);
    
    if (!string.IsNullOrEmpty(settings.Proxy)) arrow.WithProxy(new WebProxy(settings.Proxy));
    if (settings.IgnoreCertificateErrors) arrow.WithCertificateValidationCallback((_, _, _, _) => true);
    
    settings.Headers.ToList().ForEach(header => arrow.WithHeader(header.Key, header.Value));
    
    var token = new CancellationTokenSource().Token;
    
    Logger.LogInfo($"Shooting the arrow at {settings.Target}...");
    var task = arrow.ShootAsync(token);
    task.Wait(token);
    
    Logger.LogSuccess("The arrow landed! Here's what we found: ");
    var result = task.Result;
    Logger.Map(result);
});

await new CommandLineBuilder(rootCommand).UseDefaults()
    .UseHelp(context => context.HelpBuilder.CustomizeLayout(_ => HelpBuilder.Default.GetLayout().Skip(1)
        .Prepend(_ =>
        {
            AnsiConsole.MarkupLine("[bold #00ff9f] __ _  _| o __ _  _ _|_[/]");
            AnsiConsole.MarkupLine("[bold #00ff9f] | (/_(_| | | (/_(_  |_[/]");
        })))
    .UseExceptionHandler((exception, _) => Logger.LogException("An unexpected error occurred.", exception))
    .Build()
    .InvokeAsync(args);
