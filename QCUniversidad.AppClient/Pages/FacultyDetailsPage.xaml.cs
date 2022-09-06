using QCUniversidad.AppClient.ViewModels;

namespace QCUniversidad.AppClient;

public partial class FacultyDetailsPage : ContentPage
{
	public FacultyDetailsPage(FacultyDetailsPageViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}