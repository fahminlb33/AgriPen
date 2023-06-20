using AgriPenMobile.API;
using AgriPenMobile.Services;
using AgriPenMobile.ViewModels;
using AgriPenMobile.Views;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;

namespace AgriPenMobile;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
        builder.Services.AddSingleton<IAgriAPI, AgriClient>();
        builder.Services.AddSingleton<IAgriBluetoothLogger, AgriBluetoothLogger>();
        builder.Services.AddSingleton<IConfigurationStore, ConfigurationStore>();

        builder.Services.AddTransient<AppShell>();
        builder.Services.AddTransient<HomePage>();
        builder.Services.AddTransient<LoginPage>();
        builder.Services.AddTransient<MeasurementPage>();
        builder.Services.AddTransient<UploadDiseasePage>();

        builder.Services.AddTransient<HomeViewModel>();
        builder.Services.AddTransient<LoginViewModel>();
        builder.Services.AddTransient<MeasurementViewModel>();
        builder.Services.AddTransient<UploadDiseaseViewModel>();

        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			})
            .ConfigureEssentials(essentials =>
            {
                essentials.UseVersionTracking();
            });

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
