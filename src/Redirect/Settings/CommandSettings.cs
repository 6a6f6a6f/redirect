namespace Redirect.Settings;

public record CommandSettings(Uri Target)
{
    public required TimeSpan Timeout { get; init; }
    public required int MaxRedirects { get; init; }
    public required string Proxy { get; init; }
    public required bool IgnoreCertificateErrors { get; init; }

    public required Dictionary<string, string> Headers { get; init; }
}