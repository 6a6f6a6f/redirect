namespace Redirect.Arrow.Schema;

/// <summary>
/// Represents a mapping of HTTP responses, containing information about a source URI, destination URI,
/// and any additional data related to the redirection, such as headers, cookies, and query parameters.
/// </summary>
/// <param name="SourceUri">The source URI of the response map.</param>
/// <param name="DestinationUri">The destination URI of the response map.</param>
public record ResponseMap(Uri SourceUri, Uri? DestinationUri = default)
{
    /// <summary>
    /// Gets the dictionary of headers added during the redirection process.
    /// </summary>
    public Dictionary<string, string> Headers { get; set; } = new();
    
    /// <summary>
    /// Gets the collection of cookies added during the redirection process.
    /// </summary>
    public List<Cookie> Cookies { get; set; } = [];
    
    /// <summary>
    /// Gets the dictionary of query parameters added during the redirection process.
    /// </summary>
    public Dictionary<string, List<string>> QueryParameters { get; set; } = new();
    
    /// <summary>
    /// Gets or initializes the response time for the current response map.
    /// </summary>
    public TimeSpan ResponseTime { get; set; }
    
    /// <summary>
    /// Gets or initializes the HTTP status code for the current response map.
    /// </summary>
    public HttpStatusCode StatusCode { get; set; }
    
    /// <summary>
    /// Gets or initializes the next response map in the sequence of redirects, if any.
    /// </summary>
    public ResponseMap? NextRedirect { get; set; }
}