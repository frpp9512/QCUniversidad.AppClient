using QCUniversidad.AppClient.ViewModels;

namespace QCUniversidad.AppClient;

public partial class AppShell : Shell
{
	public AppShell(ShellViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
		Routing.RegisterRoute(nameof(LoadingPage), typeof(LoadingPage));
	}
}