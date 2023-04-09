namespace Redirect.Arrow.Extensions;

internal static class HttpResponseMessageExtensions
{
    public static bool IsRedirect(this HttpResponseMessage response)
    {
        var statusCode = (int)response.StatusCode;
        return statusCode is >= 300 and < 400;
    }
}