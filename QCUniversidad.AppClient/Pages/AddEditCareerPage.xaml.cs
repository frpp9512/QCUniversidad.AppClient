using QCUniversidad.AppClient.ViewModels;

namespace QCUniversidad.AppClient.Pages;

public partial class AddEditCareerPage : ContentPage
{
	public AddEditCareerPage(AddEditCareerPageViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}