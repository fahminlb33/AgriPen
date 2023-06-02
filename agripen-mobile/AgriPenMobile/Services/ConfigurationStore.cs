using System.Text.Json;

namespace AgriPenMobile.Services;

public interface IConfigurationStore
{
    string UserId { get; set; }
    string Username { get; set; }
    string AccessToken { get; set; }
    string RefreshToken { get; set; }

    Task Load();
    Task Presist();
}

public class ConfigurationStore : IConfigurationStore
{
    private readonly string _configFile=  Path.Combine(FileSystem.Current.AppDataDirectory, "config.json");

    public string UserId { get; set; }
    public string Username { get; set; }
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }

    public async Task Load()
    {
        // check if the config file exists
        if (!File.Exists(_configFile)) return;

        // load from json
        using var reader = new FileStream(_configFile, FileMode.Open);
        var config = await JsonSerializer.DeserializeAsync<ConfigurationStore>(reader);

        // set current instance values
        UserId = config.UserId;
        Username = config.Username;
        AccessToken = config.AccessToken;
        RefreshToken = config.RefreshToken;
    }

    public async Task Presist()
    {
        // overwrite existing file
        using var writer = new FileStream(_configFile, FileMode.Create);
        await JsonSerializer.SerializeAsync(writer, this);
    }
}
