using QCUniversidad.AppClient.ViewModels;

namespace QCUniversidad.AppClient;

public partial class LoadingPage : ContentPage
{
	public LoadingPage(LoadingPageViewModel viewmodel)
	{
		InitializeComponent();
		BindingContext = viewmodel;
	}
}