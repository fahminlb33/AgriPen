using System.Diagnostics;
using System.Net.Http.Json;
using System.Text.Json;
using AgriPenMobile.Services;

namespace AgriPenMobile.API;

public interface IAgriAPI
{
    Task<bool> TestToken();
    Task LoginAsync(string username, string password);
    Task DiseaseAnalysisAsync(Stream stream, GpsData location);
    Task<TelemetryResponse> TelemetryAsync(List<TelemetryItem> sensors, GpsData location);
    Task TelemetryImageAsync(string id, List<string> photos);
}

public class AgriClient : IAgriAPI
{
    private static readonly HttpClient _client = new HttpClient(new LoggingHandler(new HttpClientHandler()))
    {
        BaseAddress = new Uri("https://agripen-api.kodesiana.com")
    };

    private readonly IConfigurationStore _store;

    public AgriClient(IConfigurationStore store)
    {
        _store = store;
    }

    private async Task<bool> RefreshToken()
    {
        // renew access token
        if (string.IsNullOrEmpty(_store.UserId) || string.IsNullOrEmpty(_store.RefreshToken))
        {
            return false;
        }

        // get new access token
        var content = new RefreshTokenRequest { UserId = _store.UserId, RefreshToken = _store.RefreshToken };
        var res = await _client.PostAsJsonAsync("auth/refresh-token", content);

        // still failed, token is expired
        if (!res.IsSuccessStatusCode)
        {
            return false;
        }

        // set data
        var body = await res.Content.ReadFromJsonAsync<LoginResponse>();

        _store.UserId = body.UserId;
        _store.AccessToken = body.AccessToken;
        _store.RefreshToken = body.RefreshToken;
        await _store.Presist();

        return true;
    }

    public async Task LoginAsync(string username, string password)
    {
        // create body
        var content = new LoginRequest
        {
            User = username,
            Password = password
        };

        // hit api
        var res = await _client.PostAsync("auth/login", JsonContent.Create(content));
        res.EnsureSuccessStatusCode();

        // save data
        var body = await res.Content.ReadFromJsonAsync<LoginResponse>();

        _store.UserId = body.UserId;
        _store.Username = username;
        _store.AccessToken = body.AccessToken;
        _store.RefreshToken = body.RefreshToken;
        await _store.Presist();
    }

    public async Task<bool> TestToken()
    {
        // get access token
        if (string.IsNullOrEmpty(_store.AccessToken))
        {
            return false;
        }

        // try get profile
        var req = new HttpRequestMessage(HttpMethod.Get, "auth/profile");
        req.Headers.Authorization = new("Bearer", _store.AccessToken);

        // if the status is OK, return true
        var res = await _client.SendAsync(req);
        if (res.IsSuccessStatusCode)
        {
            return true;
        }

        return await RefreshToken();
    }

    public async Task DiseaseAnalysisAsync(Stream stream, GpsData location)
    {
        var content = new MultipartFormDataContent
        {
            { new StringContent(JsonSerializer.Serialize(location)), "gpsAddress" },
            { new StreamContent(stream), "image", "image.jpg" },
        };

        // create request
        var req = new HttpRequestMessage(HttpMethod.Post, "disease-predictions");
        req.Headers.Authorization = new("Bearer", _store.AccessToken);
        req.Content = content;

        // upload
        var res = await _client.SendAsync(req);
        res.EnsureSuccessStatusCode();
    }

    public async Task<TelemetryResponse> TelemetryAsync(List<TelemetryItem> sensors, GpsData location)
    {
        // create request
        var req = new HttpRequestMessage(HttpMethod.Post, "land-observations");
        req.Headers.Authorization = new("Bearer", _store.AccessToken);
        req.Content = JsonContent.Create(new TelemetryRequest
        {
            GpsAddress = location,
            TimeSeries = sensors
        });

        // upload
        var res = await _client.SendAsync(req);
        res.EnsureSuccessStatusCode();

        return await res.Content.ReadFromJsonAsync<TelemetryResponse>();
    }

    public async Task TelemetryImageAsync(string id, List<string> photos)
    {
        foreach (var item in photos.Take(5)) // limit to 5 pictures
        {
            // create request
            var req = new HttpRequestMessage(HttpMethod.Put, $"land-observations/{id}/images");
            req.Headers.Authorization = new("Bearer", _store.AccessToken);
            req.Content = new MultipartFormDataContent
            {
                { new StreamContent(File.OpenRead(item)), "image", "image.jpg" }
            };

            // upload
            var res = await _client.SendAsync(req);
            res.EnsureSuccessStatusCode();
        }
    }
}

public class LoggingHandler : DelegatingHandler
{
    public LoggingHandler(HttpMessageHandler innerHandler)
        : base(innerHandler)
    {
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        Debug.Print("Request:");
        Debug.Print(request.ToString());
        if (request.Content != null)
        {
            Debug.Print(await request.Content.ReadAsStringAsync());
        }

        var response = await base.SendAsync(request, cancellationToken);

        Debug.Print("Response:");
        Debug.Print(response.ToString());
        if (response.Content != null)
        {
            Debug.Print(await response.Content.ReadAsStringAsync());
        }

        return response;
    }
}