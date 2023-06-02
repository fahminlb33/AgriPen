using AgriPenMobile.Views;

namespace AgriPenMobile;

public partial class App : Application
{
	public App(AppShell page)
	{
		InitializeComponent();
		MainPage = page;
	}
}
