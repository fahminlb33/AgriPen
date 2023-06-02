using AgriPenMobile.API;
using AgriPenMobile.Services;
using AgriPenMobile.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AgriPenMobile.ViewModels;

public partial class HomeViewModel : ObservableObject
{
    private readonly IAgriAPI _api;
    private readonly IServiceProvider _provider;
    private readonly IConfigurationStore _store;

    [ObservableProperty]
    private string status = "Memuat data...";
    [ObservableProperty]
    private bool isAnalysisEnabled = false;

    public HomeViewModel(IAgriAPI api, IServiceProvider provider, IConfigurationStore store)
    {
        _api = api;
        _provider = provider;
        _store = store;
    }

    public async Task Startup()
    {
        try
        {
            OnPropertyChanged(nameof(IsAnalysisEnabled));

            // check for permissions
            var grantStatus = await Permissions.RequestAsync<AgriPlatformPermissions>();
            if (grantStatus != PermissionStatus.Granted)
            {
                Status = "Perlu konfigurasi izin sistem. Periksa pengaturan.";
                IsAnalysisEnabled = false;
                return;
            }

            // get the token
            await _store.Load();
            if (string.IsNullOrEmpty(_store.UserId))
            {
                Status = "Login sebelum melakukan analisis.";
                IsAnalysisEnabled = false;
                return;
            }

            // load profile
            if (!await _api.TestToken())
            {
                IsAnalysisEnabled = false;
                Status = "Login sebelum melakukan analisis.";
                
                SecureStorage.RemoveAll();
                return;
            }

            // enable controls
            IsAnalysisEnabled = true;
            Status = "Selamat datang di AgriPen Mobile!";
        }
        catch (Exception)
        {
            SecureStorage.Default.RemoveAll();
            Status = "Anda sudah logout. Login kembali.";
            IsAnalysisEnabled = false;
        }
    }

    [RelayCommand]
    private async Task AccountAsync()
    {
        await Shell.Current.Navigation.PushAsync(_provider.GetRequiredService<LoginPage>());
    }

    [RelayCommand]
    private async Task DiseaseAnalysisAsync()
    {
        await Shell.Current.Navigation.PushAsync(_provider.GetRequiredService<UploadDiseasePage>());
    }

    [RelayCommand]
    private async Task LandAnalysisAsync()
    {
        await Shell.Current.Navigation.PushAsync(_provider.GetRequiredService<MeasurementPage>());
    }
}
