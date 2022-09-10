using QCUniversidad.AppClient.ViewModels;

namespace QCUniversidad.AppClient.Pages;

public partial class AddEditDisciplinePage : ContentPage
{
	public AddEditDisciplinePage(AddEditDisciplinePageViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}