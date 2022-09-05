using QCUniversidad.AppClient.ViewModels;

namespace QCUniversidad.AppClient;

public partial class AddEditFacultyPage : ContentPage
{
	public AddEditFacultyPage(AddEditFacultyPageViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}