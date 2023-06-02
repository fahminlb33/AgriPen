using AgriPenMobile.ViewModels;

namespace AgriPenMobile.Views;

public partial class UploadDiseasePage : ContentPage
{
	public UploadDiseasePage(UploadDiseaseViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}