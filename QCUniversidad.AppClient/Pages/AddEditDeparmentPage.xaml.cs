using QCUniversidad.AppClient.ViewModels;

namespace QCUniversidad.AppClient;

public partial class AddEditDeparmentPage : ContentPage
{
	public AddEditDeparmentPage(AddEditDeparmentPageViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}