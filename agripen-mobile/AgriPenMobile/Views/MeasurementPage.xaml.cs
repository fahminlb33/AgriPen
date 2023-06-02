using AgriPenMobile.ViewModels;

namespace AgriPenMobile.Views;

public partial class MeasurementPage : ContentPage
{
	public MeasurementPage(MeasurementViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
	}
}
