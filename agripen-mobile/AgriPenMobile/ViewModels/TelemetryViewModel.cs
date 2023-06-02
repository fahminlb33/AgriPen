using CommunityToolkit.Mvvm.ComponentModel;

namespace AgriPenMobile.ViewModels;

public partial class TelemetryViewModel : ObservableObject
{
    [ObservableProperty]
    private string name;

    [ObservableProperty]
    private double value;
}
