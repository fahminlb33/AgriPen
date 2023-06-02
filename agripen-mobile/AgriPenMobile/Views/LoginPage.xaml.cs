using AgriPenMobile.ViewModels;

namespace AgriPenMobile.Views;

public partial class LoginPage : ContentPage
{
    public LoginPage(LoginViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        ((LoginViewModel)BindingContext).Reset();
    }
}
