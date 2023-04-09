namespace Redirect.Arrow.Extensions;

internal static class UriExtensions
{
    public static Dictionary<string, List<string>> ParseQueryString(this Uri uri)
    {
        var queryString = uri.Query.TrimStart('?');
        
        var queryParameters = HttpUtility.ParseQueryString(queryString);
        var parameters = new Dictionary<string, List<string>>();
        if (queryParameters.Count == 0) return parameters;
        
        foreach (string key in queryParameters.Keys)
        {
            if (string.IsNullOrEmpty(key)) continue;
            if (!parameters.ContainsKey(key)) parameters.Add(key, new List<string>());

            var entity = queryParameters[key];
            if (string.IsNullOrEmpty(entity)) continue;

            parameters[key].Add(entity);
        }

        return parameters;
    }
}