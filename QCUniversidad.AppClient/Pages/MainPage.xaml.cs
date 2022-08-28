using QCUniversidad.AppClient.ViewModels;

namespace QCUniversidad.AppClient;

public partial class MainPage : ContentPage
{
	public MainPage(MainPageViewModel mainPageViewModel)
	{
		InitializeComponent();
		BindingContext = mainPageViewModel;
	}
}