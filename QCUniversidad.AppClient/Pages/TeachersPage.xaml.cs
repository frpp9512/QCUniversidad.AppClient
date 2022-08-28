using QCUniversidad.AppClient.ViewModels;

namespace QCUniversidad.AppClient;

public partial class TeachersPage : ContentPage
{
	public TeachersPage(TeachersViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}