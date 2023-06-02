using BingMapsRESTToolkit;

namespace AgriPen.Infrastructure.Services;

public interface IMapService
{
    Task<string> ReverseGeocodeBingAsync(double latitude, double longitude);
}

public class MapService : IMapService
{
    private readonly string _apiKey;

    public MapService(string apiKey)
    {
        _apiKey = apiKey;
    }

    public async Task<string> ReverseGeocodeBingAsync(double latitude, double longitude)
    {
        // create request
        var req = new ReverseGeocodeRequest()
        {
            BingMapsKey = _apiKey,
            Point = new Coordinate(latitude, longitude),
        };

        // execute
        var response = await req.Execute();

        // check whether the response is valid
        if (response != null && response.ResourceSets != null && response.ResourceSets.Length > 0 && response.ResourceSets[0].Resources != null && response.ResourceSets[0].Resources.Length > 0)
        {
            var result = response.ResourceSets[0].Resources[0] as Location;
            return result!.Name;
        }

        return "";
    }
}