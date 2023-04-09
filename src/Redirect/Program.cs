using Redirect.Arrow;

var responseMap = await Arrow.From("https://bit.ly/3GwTc8i")
    .WithHeader("User-Agent", "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:10.0.2) Gecko/20100101 Firefox/10.0.2")
    .WithTimeout(TimeSpan.FromSeconds(5))
    .BuildAsync();

var currentMap = responseMap;
while (currentMap != null)
{
    Console.WriteLine($"Source URL: {currentMap.SourceUri}");
    Console.WriteLine($"Destination URL: {currentMap.DestinationUri}");
    Console.WriteLine($"Response Time: {currentMap.ResponseTime}");
    Console.WriteLine($"Status Code: {currentMap.StatusCode}");

    if (currentMap.Headers.Count > 0)
    {
        Console.WriteLine("Headers:");
        foreach (var header in currentMap.Headers)
        {
            Console.WriteLine($"  {header.Key}: {header.Value}");
        }
    }

    if (currentMap.Cookies.Count > 0)
    {
        Console.WriteLine("Cookies:");
        foreach (var cookie in currentMap.Cookies)
        {
            Console.WriteLine($"  {cookie.Name}: {cookie.Value}");
        }
    }

    if (currentMap.QueryParameters.Count > 0)
    {
        Console.WriteLine("Query Parameters:");
        foreach (var parameter in currentMap.QueryParameters)
        {
            Console.WriteLine($"  {parameter.Key}: {parameter.Value}");
        }
    }

    Console.WriteLine(new string('-', 40));
    currentMap = currentMap.NextRedirect;
}