namespace Redirect.Arrow;

/// <summary>
/// Entrypoint for the Arrow library.
/// </summary>
public class Arrow
{
    private readonly Uri _target;
    private readonly Dictionary<string, string> _headers = new();
    private readonly CookieContainer _cookieContainer = new();

    private Func<HttpRequestMessage, X509Certificate2, X509Chain, SslPolicyErrors, bool>? _certValidationCallback;
    private IWebProxy? _webProxy;
    private TimeSpan _timeout = TimeSpan.FromSeconds(3);
    private int _maxRedirects = 5;
    
    /// <summary>
    /// Creates a new Arrow instance from a target URI.
    /// </summary>
    /// <param name="target">The target URI.</param>
    /// <returns>A new instance of <see cref="Arrow"/>.</returns>
    /// <exception cref="ArgumentException">If <see cref="target"/> is not an absolute valid URI.</exception>
    public static Arrow From(string target)
    {
        if (!Uri.TryCreate(target, UriKind.Absolute, out var targetUri))
        {
            throw new ArgumentException("Target must be a valid absolute URI.", nameof(target));
        }
        
        return new Arrow(targetUri);
    }
    
    /// <summary>
    /// Creates a new Arrow instance from a target URI.
    /// </summary>
    /// <param name="target">The target URI.</param>
    /// <returns>A new instance of <see cref="Arrow"/>.</returns>
    /// <exception cref="ArgumentException">If the URI scheme is not HTTP or HTTPS.</exception>
    public static Arrow From(Uri target)
    {
        if (target.Scheme is not ("http" or "https"))
        {
            throw new ArgumentException("Target must be a valid absolute HTTP or HTTPS URI.", nameof(target));
        }
        
        return new Arrow(target);
    }
    
    /// <summary>
    /// Sets the timeout duration for the HTTP requests made by <see cref="Arrow"/>.
    /// </summary>
    /// <param name="timeout">The <see cref="TimeSpan"/> representing the timeout duration.</param>
    /// <returns>Returns the current <see cref="Arrow"/> instance with the updated timeout value.</returns>
    /// <exception cref="ArgumentOutOfRangeException">If the timeout duration is less than 1 second.</exception>
    public Arrow WithTimeout(TimeSpan timeout)
    {
        if (timeout.TotalSeconds < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(timeout), "Timeout must be at least 1 second.");
        }
        
        _timeout = timeout;
        return this;
    }
    
    /// <summary>
    /// Adds a custom header to the HTTP requests made by <see cref="Arrow"/>.
    /// </summary>
    /// <param name="key">The header field name.</param>
    /// <param name="value">The header field value.</param>
    /// <returns>Returns the current <see cref="Arrow"/> instance with the updated headers.</returns>
    /// <exception cref="ArgumentException">If the header field name already exists.</exception>
    public Arrow WithHeader(string key, string value)
    {
        if (_headers.ContainsKey(key))
        {
            throw new ArgumentException("Header already exists.", nameof(key));
        }
        
        _headers.Add(key, value);
        return this;
    }
    
    /// <summary>
    /// Adds a custom cookie to the HTTP requests made by <see cref="Arrow"/>.
    /// </summary>
    /// <param name="cookie">The <see cref="Cookie"/> to be added to the requests.</param>
    /// <returns>Returns the current <see cref="Arrow"/> instance with the updated timeout value.</returns>
    public Arrow WithCookie(Cookie cookie)
    {
        _cookieContainer.Add(cookie);
        return this;
    }
    
    /// <summary>
    /// Sets the maximum number of redirects allowed for the HTTP requests made by <see cref="Arrow"/>.
    /// </summary>
    /// <param name="maxRedirects">The maximum number of redirects allowed.</param>
    /// <returns>Returns the current <see cref="Arrow"/> instance with the updated maximum redirects value.</returns>
    /// <exception cref="ArgumentOutOfRangeException">If value is a negative number.</exception>
    public Arrow WithMaxRedirects(int maxRedirects)
    {
        if (maxRedirects < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(maxRedirects), "Max redirects must be a positive number.");
        }

        _maxRedirects = maxRedirects;
        return this;
    }

    /// <summary>
    /// Sets a custom web proxy for the HTTP requests made by <see cref="Arrow"/>.
    /// </summary>
    /// <param name="webProxy">An <see cref="IWebProxy"/> representing the web proxy to be used for the requests.</param>
    /// <returns>Returns the current <see cref="Arrow"/> instance with the specified web proxy.</returns>
    public Arrow WithProxy(IWebProxy webProxy)
    {
        _webProxy = webProxy;
        return this;
    }

    /// <summary>
    /// Sets a custom certificate validation callback for the HTTP requests made by <see cref="Arrow"/>.
    /// </summary>
    /// <param name="certValidationCallback">A delegate representing the custom certificate validation logic.</param>
    /// <returns>Returns the current <see cref="Arrow"/> instance with the specified certificate validation callback.</returns>
    public Arrow WithCertificateValidationCallback(
        Func<HttpRequestMessage, X509Certificate2, X509Chain, SslPolicyErrors, bool> certValidationCallback)
    {
        _certValidationCallback = certValidationCallback;
        return this;
    }

    /// <summary>
    /// The request map for the target pointed at <see cref="From(string)"/>or <see cref="From(Uri)"/>.
    /// </summary>
    /// <returns>The <see cref="ResponseMap"/> up to the final redirection.</returns>
    public async Task<ResponseMap> BuildAsync()
    {
        using var handler = new HttpClientHandler
        {
            AllowAutoRedirect = false,
            CookieContainer = _cookieContainer,
            Proxy = _webProxy,
        };

        if (_certValidationCallback is not null)
        {
            handler.ServerCertificateCustomValidationCallback = _certValidationCallback!;
        }

        using var client = new HttpClient(handler) {Timeout = _timeout};
        foreach (var (key, value) in _headers) client.DefaultRequestHeaders.Add(key, value);

        var currentUri = _target;
        var redirectCount = 0;

        var resultMap = new ResponseMap(currentUri);
        ResponseMap? previousResultMap = default;

        while (redirectCount < _maxRedirects)
        {
            var stopwatch = Stopwatch.StartNew();
            var response = await client.GetAsync(currentUri);
            stopwatch.Stop();

            if (response.IsRedirect())
            {
                redirectCount++;

                var nextUri = response.Headers.Location;
                if (nextUri is not null && !nextUri.IsAbsoluteUri)
                {
                    nextUri = new Uri(currentUri!, nextUri);
                }

                var newResultMap = new ResponseMap(currentUri!, nextUri)
                {
                    Headers = response.Headers
                        .ToDictionary(h => h.Key, h =>
                            string.Join(", ", h.Value)),
                    Cookies = _cookieContainer.GetCookies(currentUri!).ToList(),
                    QueryParameters = nextUri!.ParseQueryString(),
                    StatusCode = response.StatusCode,
                    ResponseTime = stopwatch.Elapsed
                };

                if (previousResultMap != null)
                {
                    previousResultMap.NextRedirect = newResultMap;
                }
                else
                {
                    resultMap = newResultMap;
                }

                previousResultMap = newResultMap;
                currentUri = nextUri;
            }
            else
            {
                resultMap.Headers = response.Headers.ToDictionary(h => h.Key, h =>
                    string.Join(", ", h.Value));
                resultMap.Cookies = _cookieContainer.GetCookies(currentUri!).ToList();
                resultMap.QueryParameters = currentUri!.ParseQueryString();
                resultMap.StatusCode = response.StatusCode;
                resultMap.ResponseTime = stopwatch.Elapsed;
                break;
            }
        }
        
        return resultMap;
    }

    private Arrow (Uri target)
    {
        _target = target;
    }
}