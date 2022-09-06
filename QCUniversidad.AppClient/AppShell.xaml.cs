using QCUniversidad.AppClient.ViewModels;

namespace QCUniversidad.AppClient;

public partial class AppShell : Shell
{
	public AppShell(ShellViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
		Routing.RegisterRoute(nameof(LoadingPage), typeof(LoadingPage));
		Routing.RegisterRoute(nameof(AddEditFacultyPage), typeof(AddEditFacultyPage));
		Routing.RegisterRoute(nameof(FacultyDetailsPage), typeof(FacultyDetailsPage));
		Routing.RegisterRoute(nameof(AddEditDeparmentPage), typeof(AddEditDeparmentPage));
	}
}