using AgriPenMobile.ViewModels;

namespace AgriPenMobile.Views;

public partial class HomePage : ContentPage
{
	public HomePage(HomeViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
	}

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        ((HomeViewModel)BindingContext).Startup();
    }
}