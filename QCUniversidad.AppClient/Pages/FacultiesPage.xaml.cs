using QCUniversidad.AppClient.ViewModels;

namespace QCUniversidad.AppClient;

public partial class FacultiesPage : ContentPage
{
	public FacultiesPage(FacultiesPageViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}